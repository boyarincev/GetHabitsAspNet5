using GetHabitsAspNet5App.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Entity.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using Microsoft.AspNet.Http;
using GetHabitsAspNet5App.Helpers;

namespace GetHabitsAspNet5App.Services
{
    //TODO add logging calls
    //TODO need clear Habit from private info before send

    public class HabitService
    {
        private GetHabitsContext _dbContext;
        private HttpContext _httpContext;
        private string _userId;
        private ILogger _logger;
        private ApplicationHelper _appHelper;

        public HabitService(GetHabitsContext dbContext, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory, ApplicationHelper appHelper)
        {
            _dbContext = dbContext;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = loggerFactory.CreateLogger<HabitService>();
            _appHelper = appHelper;

            var claimUserId = _httpContext.User.FindFirst(appHelper.TypeClaimUserId);

            if (claimUserId == null)
            {
                _logger.LogCritical("HttpContext doesn't contain UserId claim");
                _userId = Guid.NewGuid().ToString();
            }
            else
            {
                _userId = claimUserId.Value;
            }
        }

        /// <summary>
        /// Get all Habit entities
        /// </summary>
        /// <returns>querying Habit collection</returns>
        public async Task<IEnumerable<Habit>> GetHabits()
        {
            return await _dbContext.Habits.Where(h => h.UserId == _userId).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get all Habits with populated collection of checkins.
        /// </summary>
        /// <param name="checkinLastDaysAmount">Amount checkins for last days</param>
        /// <returns></returns>
        public async Task<IEnumerable<Habit>> GetHabitsWithCheckins(int checkinLastDaysAmount = 0)
        {
            var dateRange = GetStartAndEndDatesForLastAmountDays(checkinLastDaysAmount);
            return await GetHabitsWithCheckins(dateRange.StartDate, dateRange.EndDate);
        }

        /// <summary>
        /// Get all Habits with populated collection of checkins.
        /// </summary>
        /// <param name="checkinStartDate">Smallest date for checkin</param>
        /// <param name="checkinEndDate">Largest date for checkin</param>
        /// <returns></returns>
        public async Task<IEnumerable<Habit>> GetHabitsWithCheckins(DateTime checkinStartDate, DateTime checkinEndDate)
        {
            var habitCheckinsList = await _dbContext.Habits
                .Where(h => h.UserId == _userId)
                .GroupJoin(_dbContext.Checkins.Where(ch => ch.Date >= checkinStartDate && ch.Date <= checkinEndDate),
                    habit => habit.Id,
                    checkin => checkin.HabitId,
                    (habit, checkins) => new { Habit = habit, Checkins = checkins }).AsNoTracking().ToListAsync();

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
            if (dateDifferent > _appHelper.MaxCheckinCount)
                dateDifferent = _appHelper.MaxCheckinCount;

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
        /// <param name="viewHabit">Habit for saving, Id have to be equal 0</param>
        /// <param name="checkinLastDaysAmount">Amount last checkins in Habit Entity</param>
        /// <returns>Habit if creating is happened and null if not</returns>
        public async Task<Habit> CreateHabit(Habit viewHabit, int checkinLastDaysAmount = 0)
        {
            //we create new entity if only Id equal 0
            if (viewHabit.Id != 0)
                return null;

            var dbHabit = new Habit(viewHabit.Name, _userId);
            _dbContext.Habits.Add(dbHabit);
            await _dbContext.SaveChangesAsync();

            var dateRange = GetStartAndEndDatesForLastAmountDays(checkinLastDaysAmount);
            var checkinEmptyList = new List<Checkin>().AsEnumerable();

            dbHabit.Checkins = GetFullCheckinArray(dbHabit.Id, checkinEmptyList, dateRange.StartDate, dateRange.EndDate).ToList();

            return dbHabit;
        }

        /// <summary>
        /// Edit Habit entity
        /// </summary>
        /// <param name="viewHabit">Habit with changed fields</param>
        /// <param name="checkinLastDaysAmount">Amount last checkins in Habit Entity</param>
        /// <returns>Changed Habit if editing is happened and null if not</returns>
        public async Task<Habit> EditHabit(Habit viewHabit, int checkinLastDaysAmount = 0)
        {
            var dateRange = GetStartAndEndDatesForLastAmountDays(checkinLastDaysAmount);

            //TODO need split to 2 query. Checkins need get as NoTracking, them may be a lot
            var habitCheckinsList = await _dbContext.Habits
                .Where(h => h.UserId == _userId)
                .GroupJoin(_dbContext.Checkins.Where(ch => ch.Date >= dateRange.StartDate && ch.Date <= dateRange.EndDate),
                    h => h.Id,
                    checkin => checkin.HabitId,
                    (h, checkins) => new { Habit = h, Checkins = checkins })
                        .FirstOrDefaultAsync(hc => hc.Habit.Id == viewHabit.Id);

            var dbHabit = habitCheckinsList.Habit;

            if (dbHabit == null)
                return null;

            dbHabit.Name = viewHabit.Name;
            await _dbContext.SaveChangesAsync();

            var checkinsResult = GetFullCheckinArray(dbHabit.Id, habitCheckinsList.Checkins, dateRange.StartDate, dateRange.EndDate);
            dbHabit.Checkins = checkinsResult.ToList();

            return dbHabit;
        }

        /// <summary>
        /// Delete Habit entity
        /// </summary>
        /// <param name="habit">Habit entity for deleting</param>
        /// <returns></returns>
        public async Task<Boolean> DeleteHabit(Int64 Id)
        {
            var original = await _dbContext.Habits.Where(h => h.UserId == _userId && h.Id == Id).FirstOrDefaultAsync();

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

            var checkins = await _dbContext.Habits
                .Where(h => h.UserId == _userId && habitId == h.Id)
                .GroupJoin(_dbContext.Checkins
                .Where(ch => ch.Date >= startDate && ch.Date <= endDate),
                h => h.Id, checkin => checkin.HabitId, (habit, checks) => checks)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return checkins.ToList();
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

            var checkins = await _dbContext.Habits
                .Where(h => h.UserId == _userId && checkin.HabitId == h.Id)
                .GroupJoin(_dbContext.Checkins
                .Where(ch => ch.Date.Date == checkin.Date.Date),
                h => h.Id, chk => chk.HabitId, (habit, checks) => checks)
                .FirstOrDefaultAsync();

            var dbCheckin = checkins.FirstOrDefault();

            //TODO need refactoring
            if (dbCheckin != null)
            {
                if (checkin.State != dbCheckin.State)
                    dbCheckin.State = checkin.State;
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

        private DateRange GetStartAndEndDatesForLastAmountDays(int lastAmountDays)
        {
            return new DateRange(DateTime.Now.Date.AddDays(-(lastAmountDays - 1)), DateTime.Now.Date);
        }

        private struct DateRange
        {
            public DateRange(DateTime startDate, DateTime endDate)
            {
                _startDate = startDate.Date;
                _endDate = endDate.Date;
            }

            private DateTime _startDate;
            private DateTime _endDate;

            public DateTime StartDate { get { return _startDate; } }
            public DateTime EndDate { get { return _endDate; } }
        }
    }
}
