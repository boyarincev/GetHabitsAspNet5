using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Configuration;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GetHabitsAspNet5App.Api
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        [Authorize]
        public IActionResult Index()
        {
            var confEnv = Startup.Configuration.GetSection("ASPNET_ENV");
            ViewBag.Environment = confEnv.Value;

            var connectionEnv = Startup.Configuration.GetSection("Data:DefaultConnection:ConnectionString");
            ViewBag.ConnectionString = connectionEnv.Value;

            return View();
        }
    }
}
