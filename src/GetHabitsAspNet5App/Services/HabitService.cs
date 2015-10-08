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
        public async Task<IEnumerable<Habit>> Get()
        {
            return await _dbContext.Habits.ToListAsync();
        }

        /// <summary>
        /// Create new Habit
        /// </summary>
        /// <param name="habit">Habit for saving, Id have to be equal 0</param>
        /// <returns>Habit if creating is happened and null if not</returns>
        public async Task<Habit> Create(Habit habit)
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
        public async Task<Habit> Edit(Habit habit)
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
        public async Task<Boolean> Delete(Int64 Id)
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

            var list = _dbContext.Habits.ToList();
            var habitQuery = _dbContext.Habits.Include(h => h.Checkins);
            var habit = habitQuery.FirstOrDefault(h => h.Id == habitId);
            
            return habit.Checkins;
        } 
    }
}
