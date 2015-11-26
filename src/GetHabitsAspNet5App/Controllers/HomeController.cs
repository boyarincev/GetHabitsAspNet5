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
    public class HomeController : LocalizeController
    {
        public HomeController(ApplicationHelper appHelper)
            : base(appHelper)
        {

        }

        [Route("/", Name = "SiteRoot")]
        public IActionResult SiteRoot()
        {
            string redirectAddress = GetAddressForRedirectFromSiteRoot();

            return Redirect(redirectAddress);
        }

        public IActionResult Index()
        {
            var confEnv = Startup.Configuration.GetSection("ASPNET_ENV");
            ViewBag.Environment = confEnv.Value;

            //var connectionEnv = Startup.Configuration.GetSection("Data:DefaultConnection:ConnectionString");
            //ViewBag.ConnectionString = connectionEnv.Value;

            return View();
        }

        private string GetAddressForRedirectFromSiteRoot()
        {
            var requestCulture = GetRequestCulture();
            var requestLangName = requestCulture.TwoLetterISOLanguageName;

            if (SiteSupportsLangName(requestLangName))
            {
                return requestLangName;
            }

            return _appHelper.DefaultLangName;
        }

        private CultureInfo GetRequestCulture()
        {
            var requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            var requestCulture = requestCultureFeature.RequestCulture;

            return requestCulture.Culture;
        }

        private bool SiteSupportsLangName(string langName)
        {
            CultureInfo cultureInfo;
            _appHelper.LangNameAndCultureNameCorresponding.
                TryGetValue(langName, out cultureInfo);

            var siteSupportsRequestLangName = cultureInfo != null;
            return siteSupportsRequestLangName;
        }
    }
}
