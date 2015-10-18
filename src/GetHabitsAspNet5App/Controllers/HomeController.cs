using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Configuration;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GetHabitsAspNet5App.Api
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            //ViewBag.Configuration = Startup.Configuration.GetConfigurationSections().ToList();
            var confEnv = Startup.Configuration.GetSection("ASPNET_ENV");
            ViewBag.Environment = confEnv.Value;

            var confConnectString = Startup.Configuration.GetSection("Data:DefaultConnection:ConnectionString");
            ViewBag.ConnectionString = confConnectString.Value;
            return View();
        }
    }
}
