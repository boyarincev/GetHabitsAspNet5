using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using GetHabitsAspNet5App.Helpers;
using System.Globalization;
using Microsoft.AspNet.Localization;
using Microsoft.AspNet.Http.Features;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GetHabitsAspNet5App.Controllers
{
    /// <summary>
    /// Insulate redirect logic from unlocalized to localized paths
    /// </summary>
    public abstract class LocalizeController : Controller
    {
        protected ApplicationHelper _appHelper;

        public LocalizeController(ApplicationHelper appHelper)
        {
            _appHelper = appHelper;
        }

        [Route("/", Name = "SiteRoot")]
        public IActionResult SiteRoot()
        {
            string redirectAddress = GetAddressForRedirectFromSiteRoot();

            return Redirect(redirectAddress);
        }

        public IActionResult UnLocalizedRequest()
        {
            var localizeSegment = GetAddressForRedirectFromSiteRoot();

            var redirectAddress = "/" + localizeSegment + HttpContext.Request.Path;

            if (HttpContext.Request.QueryString.HasValue)
            {
                redirectAddress += "?" + HttpContext.Request.QueryString;
            }

            return Redirect(redirectAddress);
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
