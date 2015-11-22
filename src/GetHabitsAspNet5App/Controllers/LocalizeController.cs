using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using GetHabitsAspNet5App.Helpers;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GetHabitsAspNet5App.Controllers
{
    public abstract class LocalizeController : Controller
    {
        protected ApplicationHelper _appHelper;

        public LocalizeController(ApplicationHelper appHelper)
        {
            _appHelper = appHelper;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
            //currentCulture.

            //context.Result = new RedirectToActionResult()
        }
    }
}
