using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Localization;
using Microsoft.AspNet.Http.Features;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GetHabitsAspNet5App.Controllers
{
    public class AppController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            //var requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            //var requestCulture = requestCultureFeature.RequestCulture;

            return View();
        }
    }
}
