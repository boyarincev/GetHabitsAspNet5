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
            var result = await _habitService.GetHabitsWithCheckins(DateTime.Now.Date.AddDays(-(checkinLastDaysAmount - 1)), DateTime.Now.Date);
            return result;
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Habit habit, int checkinLastDaysAmount)
        {
            Habit habitResult;

            if (habit.Id != 0)
                return HttpBadRequest();

            habitResult = await _habitService.CreateHabit(habit, checkinLastDaysAmount);

            if (habitResult == null)
                return HttpBadRequest();
            
            return new ObjectResult(habitResult);
        }

        // PUT api/values/5
        [HttpPost("{id}")]
        public async Task<IActionResult> Post(int id, [FromBody]Habit habit, int checkinLastDaysAmount)
        {
            if (habit.Id == 0)
                return HttpBadRequest();

            var habitResult = await _habitService.EditHabit(habit, checkinLastDaysAmount);

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
