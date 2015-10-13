using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using GetHabitsAspNet5App.Models.DomainModels;
using GetHabitsAspNet5App.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GetHabitsAspNet5App.Api
{
    [Route("api/[controller]")]
    public class HabitsController : Controller
    {
        private readonly HabitService _habitService;

        public HabitsController(HabitService habitService)
        {
            _habitService = habitService;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<Habit>> Get(int checkinLastDaysAmount)
        {
            //return new List<Habit>()
            //{
            //    new Habit() {Id = 1, Name = "Бросить курить", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.Done, HabitId = 1}, new Checkin() { Date = new DateTime(2015, 09, 15), State = CheckinState.NotDone, HabitId = 1 }, new Checkin() { Date = new DateTime(2015, 09, 14), State = CheckinState.NotSet, HabitId = 1 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, HabitId = 1 } } },
            //    new Habit() {Id = 2, Name = "Бросить пить", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.NotDone, HabitId = 2 }, new Checkin() { Date = new DateTime(2015, 09, 12), State = CheckinState.Done, HabitId = 2 }, new Checkin() { Date = new DateTime(2015, 09, 12), State = CheckinState.Done, HabitId = 2 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, HabitId = 2 } } },
            //    new Habit() {Id = 3, Name = "Бег по утрам", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.NotDone, HabitId = 3 }, new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.NotDone, HabitId = 3 }, new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.NotDone, HabitId = 3 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, HabitId = 3 } } },
            //    new Habit() {Id = 4, Name = "Делать зарядку", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.Done, HabitId = 4 }, new Checkin() { Date = new DateTime(2015, 09, 15), State = CheckinState.Done, HabitId = 4 }, new Checkin() { Date = new DateTime(2015, 09, 14), State = CheckinState.Done, HabitId = 4 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, HabitId = 4 } } },
            //    new Habit() {Id = 5, Name = "Приборка дома", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.Done, HabitId = 5 }, new Checkin() { Date = new DateTime(2015, 09, 15), State = CheckinState.Done, HabitId = 5 }, new Checkin() { Date = new DateTime(2015, 09, 14), State = CheckinState.Done, HabitId = 5 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, HabitId = 5 } } }
            //};


            var result = await _habitService.GetHabitsWithCheckins(DateTime.Now.Date.AddDays(-(checkinLastDaysAmount - 1)), DateTime.Now.Date);
            return result;
        }

        //TODO need implement
        // GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Habit habit)
        {
            Habit habitResult;

            if (habit.Id != 0)
                return HttpBadRequest();

            habitResult = await _habitService.CreateHabit(habit);

            if (habitResult == null)
                return HttpBadRequest();

            return new ObjectResult(habitResult);
        }

        // PUT api/values/5
        [HttpPost("{id}")]
        public async Task<IActionResult> Post(int id, [FromBody]Habit habit)
        {
            if (habit.Id == 0)
                return HttpBadRequest();

            var habitResult = await _habitService.EditHabit(habit);

            if (habitResult == null)
            {
                return HttpBadRequest();
            }

            return new ObjectResult(habitResult);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Int64 id)
        {
            var result = await _habitService.DeleteHabit(id);

            var resultStatus = result ? new HttpStatusCodeResult(200) : HttpBadRequest();

            return resultStatus;
        }
    }
}
