using GetHabitsAspNet5App.Services;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetHabitsAspNet5App.Controllers
{
    public class CheckinsController: Controller
    {
        private readonly HabitService _habitService;

        public CheckinsController(HabitService habitService)
        {
            _habitService = habitService;
        }
    }
}
