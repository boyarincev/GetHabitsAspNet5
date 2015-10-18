using GetHabitsAspNet5App.Models.DomainModels;
using GetHabitsAspNet5App.Services;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetHabitsAspNet5App.Controllers
{
    [Route("api/[controller]")]
    public class CheckinsController: Controller
    {
        private readonly HabitService _habitService;

        public CheckinsController(HabitService habitService)
        {
            _habitService = habitService;
        }

        [HttpPost]
        public async Task<IActionResult> SetCheckinState([FromBody]Checkin checkin)
        {
            var result = await _habitService.SetCheckinState(checkin);

            if (result == null)
            {
                return HttpBadRequest();
            }

            return new ObjectResult(result);
        }
    }
}
