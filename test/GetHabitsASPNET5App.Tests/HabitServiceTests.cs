using GetHabitsAspNet5App.Models.DomainModels;
using GetHabitsAspNet5App.Services;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GetHabitsASPNET5App.Tests
{
    public class HabitServiceTests
    {
        GetHabitsContext _dbContext;
        HabitService _habitService;

        public HabitServiceTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<GetHabitsContext>();
            optionsBuilder.UseInMemoryDatabase();

            _dbContext = new GetHabitsContext(optionsBuilder.Options);

            AddHabits();

            _habitService = new HabitService(_dbContext);
        }

        private void AddHabits()
        {
            _dbContext.Habits.AddRange(new List<Habit>()
            {
                new Habit() {Id = 1, Name = "Бросить курить", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.Done, HabitId = 1}, new Checkin() { Date = new DateTime(2015, 09, 15), State = CheckinState.NotDone, HabitId = 1 }, new Checkin() { Date = new DateTime(2015, 09, 14), State = CheckinState.NotSet, HabitId = 1 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, HabitId = 1 } } },
                new Habit() {Id = 2, Name = "Бросить пить", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.NotDone, HabitId = 2 }, new Checkin() { Date = new DateTime(2015, 09, 12), State = CheckinState.Done, HabitId = 2 }, new Checkin() { Date = new DateTime(2015, 09, 12), State = CheckinState.Done, HabitId = 2 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, HabitId = 2 } } },
                new Habit() {Id = 3, Name = "Бег по утрам", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.NotDone, HabitId = 3 }, new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.NotDone, HabitId = 3 }, new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.NotDone, HabitId = 3 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, HabitId = 3 } } },
                new Habit() {Id = 4, Name = "Делать зарядку", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.Done, HabitId = 4 }, new Checkin() { Date = new DateTime(2015, 09, 15), State = CheckinState.Done, HabitId = 4 }, new Checkin() { Date = new DateTime(2015, 09, 14), State = CheckinState.Done, HabitId = 4 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, HabitId = 4 } } },
                new Habit() {Id = 5, Name = "Приборка дома", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.Done, HabitId = 5 }, new Checkin() { Date = new DateTime(2015, 09, 15), State = CheckinState.Done, HabitId = 5 }, new Checkin() { Date = new DateTime(2015, 09, 14), State = CheckinState.Done, HabitId = 5 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, HabitId = 5 } } }
            });

            _dbContext.SaveChanges();
        }

        [Fact]
        private void GetHabitsReturnsIEnumerable()
        {
            var getHabitsResult = _habitService.GetHabits().Result;

            Assert.True(getHabitsResult is IEnumerable<Habit>);
        }
    }
}
