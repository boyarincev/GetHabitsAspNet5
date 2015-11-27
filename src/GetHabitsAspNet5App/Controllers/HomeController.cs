using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using GetHabitsAspNet5App.Helpers;
using System.Globalization;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Localization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GetHabitsAspNet5App.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationHelper _appHelper;

        public HomeController(ApplicationHelper appHelper)
        {
            _appHelper = appHelper;
        }

        public IActionResult Index()
        {
            var confEnv = Startup.Configuration.GetSection("ASPNET_ENV");
            ViewBag.Environment = confEnv.Value;

            //var connectionEnv = Startup.Configuration.GetSection("Data:DefaultConnection:ConnectionString");
            //ViewBag.ConnectionString = connectionEnv.Value;

            return View();
        }
    }
}
