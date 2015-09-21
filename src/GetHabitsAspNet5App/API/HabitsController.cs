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
                new Habit() {Id = 1, Name = "Бросить курить", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.Done, Id = 111}, new Checkin() { Date = new DateTime(2015, 09, 15), State = CheckinState.NotDone, Id = 112 }, new Checkin() { Date = new DateTime(2015, 09, 14), State = CheckinState.NotSet, Id = 113 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, Id = 134 } } },
                new Habit() {Id = 2, Name = "Бросить пить", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.NotDone, Id = 114 }, new Checkin() { Date = new DateTime(2015, 09, 12), State = CheckinState.Done, Id = 115 }, new Checkin() { Date = new DateTime(2015, 09, 12), State = CheckinState.Done, Id = 130 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, Id = 133 } } },
                new Habit() {Id = 3, Name = "Бег по утрам", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.NotDone, Id = 116 }, new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.NotDone, Id = 131 }, new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.NotDone, Id = 132 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, Id = 132 } } },
                new Habit() {Id = 4, Name = "Делать зарядку", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.Done, Id = 117 }, new Checkin() { Date = new DateTime(2015, 09, 15), State = CheckinState.Done, Id = 118 }, new Checkin() { Date = new DateTime(2015, 09, 14), State = CheckinState.Done, Id = 119 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, Id = 120 } } },
                new Habit() {Id = 5, Name = "Приборка дома", Checkins = new List<Checkin>() { new Checkin() { Date = new DateTime(2015, 09, 16), State = CheckinState.Done, Id = 117 }, new Checkin() { Date = new DateTime(2015, 09, 15), State = CheckinState.Done, Id = 118 }, new Checkin() { Date = new DateTime(2015, 09, 14), State = CheckinState.Done, Id = 119 }, new Checkin() { Date = new DateTime(2015, 09, 13), State = CheckinState.Done, Id = 120 } } }
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
