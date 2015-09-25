using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using GetHabitsAspNet5App.Models.DomainModels;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GetHabitsAspNet5App.Api
{
    [Route("api/[controller]")]
    public class HabitsController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<Habit> Get()
        {
            return new List<Habit>()
            {
                new Habit() {Id = 1, Name = "Бросить курить", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.Done, HabitId = 1}, new Checkin() { Date = new DateTime(2015, 09, 15), State = CheckinState.NotDone, HabitId = 1 }, new Checkin() { Date = new DateTime(2015, 09, 14), State = CheckinState.NotSet, HabitId = 1 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, HabitId = 1 } } },
                new Habit() {Id = 2, Name = "Бросить пить", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.NotDone, HabitId = 2 }, new Checkin() { Date = new DateTime(2015, 09, 12), State = CheckinState.Done, HabitId = 2 }, new Checkin() { Date = new DateTime(2015, 09, 12), State = CheckinState.Done, HabitId = 2 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, HabitId = 2 } } },
                new Habit() {Id = 3, Name = "Бег по утрам", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.NotDone, HabitId = 3 }, new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.NotDone, HabitId = 3 }, new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.NotDone, HabitId = 3 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, HabitId = 3 } } },
                new Habit() {Id = 4, Name = "Делать зарядку", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.Done, HabitId = 4 }, new Checkin() { Date = new DateTime(2015, 09, 15), State = CheckinState.Done, HabitId = 4 }, new Checkin() { Date = new DateTime(2015, 09, 14), State = CheckinState.Done, HabitId = 4 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, HabitId = 4 } } },
                new Habit() {Id = 5, Name = "Приборка дома", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.Done, HabitId = 5 }, new Checkin() { Date = new DateTime(2015, 09, 15), State = CheckinState.Done, HabitId = 5 }, new Checkin() { Date = new DateTime(2015, 09, 14), State = CheckinState.Done, HabitId = 5 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, HabitId = 5 } } }
            };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
