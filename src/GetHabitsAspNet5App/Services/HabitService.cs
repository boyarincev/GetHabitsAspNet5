using GetHabitsAspNet5App.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Framework.Logging;
using Microsoft.Data.Entity.Internal;
using Microsoft.Framework.DependencyInjection;

namespace GetHabitsAspNet5App.Services
{
    public class HabitService
    {
        private GetHabitsContext _dbContext;

        public HabitService(GetHabitsContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get all Habit entities
        /// </summary>
        /// <returns>querying Habit collection</returns>
        public async Task<IEnumerable<Habit>> GetHabits()
        {
            return await _dbContext.Habits.ToListAsync();
        }

        /// <summary>
        /// Get all Habits with populated collection of checkins
        /// </summary>
        /// <param name="checkinStartDate">Smallest date for checkin</param>
        /// <param name="checkinEndDate">Largest date for checkin</param>
        /// <returns></returns>
        public async Task<IEnumerable<Habit>> GetHabitsWithCheckins(DateTime checkinStartDate, DateTime checkinEndDate)
        {
            var habitCheckinsList = await _dbContext.Habits
                .GroupJoin(_dbContext.Checkins.Where(ch => ch.Date >= checkinStartDate && ch.Date <= checkinEndDate),
                    habit => habit.Id,
                    checkin => checkin.HabitId,
                    (habit, checkins) => new { Habit = habit, Checkins = checkins}).ToListAsync();

            var resultHabitList = new List<Habit>();

            foreach (var item in habitCheckinsList)
            {
                item.Habit.Checkins = GetFullCheckinArray(item.Habit.Id, item.Checkins.ToList(), checkinStartDate, checkinEndDate).ToList();
                resultHabitList.Add(item.Habit);
            }

            return resultHabitList.AsEnumerable();
        }

        /// <summary>
        /// Get Array of checkins, located in order from largest date to smallest date. 
        /// If input checkin List, not contains checkin for any date, for this date create new checkin with State equal NotSet.
        /// If difference between end date and start date large than 30 days, result array will be contains only 30 checkins for largest dates
        /// </summary>
        /// <param name="habitId"></param>
        /// <param name="checkins">exist in Db checkins</param>
        /// <param name="checkinStartDate"></param>
        /// <param name="checkinEndDate"></param>
        /// <returns>Array of checkins, located in order from largest date to smallest date.</returns>
        private IEnumerable<Checkin> GetFullCheckinArray(Int64 habitId, IEnumerable<Checkin> checkins, DateTime checkinStartDate, DateTime checkinEndDate)
        {
            var dateDifferent = (checkinEndDate.Date - checkinStartDate.Date).Days;

            //While restrict possible amount queryed days
            if (dateDifferent > 30)
                dateDifferent = 30;

            var fullCheckins = new List<Checkin>();

            var sortedCheckinsFromDb = checkins.OrderByDescending(ch => ch.Date).ToArray();
            var checkinArrEnum = 0;
            
            for (int i = 0; i <= dateDifferent; i++)
            {
                if (sortedCheckinsFromDb.Length > checkinArrEnum && DateTime.Now.Date.AddDays(-i) == sortedCheckinsFromDb[checkinArrEnum].Date.Date )
                {
                    fullCheckins.Add(sortedCheckinsFromDb[checkinArrEnum]);
                    checkinArrEnum++;
                    continue;
                }

                fullCheckins.Add(new Checkin() { Date = DateTime.Now.Date.AddDays(-i), HabitId = habitId, State = CheckinState.NotSet });
            }

            return fullCheckins;
        }

        /// <summary>
        /// Create new Habit
        /// </summary>
        /// <param name="habit">Habit for saving, Id have to be equal 0</param>
        /// <returns>Habit if creating is happened and null if not</returns>
        public async Task<Habit> CreateHabit(Habit habit)
        {
            //we create new entity if only Id equal 0
            if (habit.Id != 0)
                return null;

            var clearHabit = new Habit(habit.Name);

            _dbContext.Habits.Add(clearHabit);
            await _dbContext.SaveChangesAsync();

            return clearHabit;
        }

        /// <summary>
        /// Edit Habit entity
        /// </summary>
        /// <param name="habit">Habit with changed fields</param>
        /// <returns>Changed Habit if editing is happened and null if not</returns>
        public async Task<Habit> EditHabit(Habit habit)
        {
            var original = await _dbContext.Habits.FirstOrDefaultAsync(h => h.Id == habit.Id);

            if (original == null)
                return null;

            original.Name = habit.Name;
            await _dbContext.SaveChangesAsync();

            return original;
        }

        /// <summary>
        /// Delete Habit entity
        /// </summary>
        /// <param name="habit">Habit entity for deleting</param>
        /// <returns></returns>
        public async Task<Boolean> DeleteHabit(Int64 Id)
        {
            var original = await _dbContext.Habits.FirstOrDefaultAsync(h => h.Id == Id);

            if (original == null)
                return false;

            _dbContext.Remove(original);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Checkin>> GetCheckins(Int64 habitId, DateTime startDate, DateTime endDate)
        {

            var dateDiff = DateTime.Compare(startDate, endDate);

            if (dateDiff > 0)
            {
                return new List<Checkin>();
            }

            var checkins = await _dbContext.Checkins
                .Where(ch => ch.HabitId == habitId && ch.Date >= startDate && ch.Date <= endDate)
                .ToListAsync();

            return checkins;
        } 

        /// <summary>
        /// Sets checkin state. Creates checkin if it doesn't exist.
        /// </summary>
        /// <param name="checkin">Checkin for saving</param>
        /// <returns>Saved checkin or Null if Habit doesn't exist</returns>
        public async Task<Checkin> SetCheckinState(Checkin checkin)
        {
            var habitExists = _dbContext.Habits.Any(h => h.Id == checkin.HabitId);

            if (!habitExists)
                return null;

            var dbCheckin = _dbContext.Checkins.Where(ch => ch.HabitId == checkin.HabitId && ch.Date.Date == checkin.Date.Date).FirstOrDefault();

            if (dbCheckin != null)
            {
                if (checkin.State != dbCheckin.State)
                    checkin.State = dbCheckin.State;
            }
            else
            {
                var newCheckin = new Checkin() { Date = checkin.Date.Date, HabitId = checkin.HabitId, State = checkin.State };
                _dbContext.Checkins.Add(newCheckin);
                dbCheckin = newCheckin;
            }

            await _dbContext.SaveChangesAsync();

            return dbCheckin;
        }
    }
}
