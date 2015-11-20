using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using GetHabitsAspNet5App.Helpers;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GetHabitsAspNet5App.Controllers
{
    public class AccountController : LocalizeController
    {
        public AccountController(ApplicationHelper appHelper)
            :base(appHelper)
        {

        }

        // GET: /<controller>/
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        public IActionResult External(string provider)
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = "/account/externalCallback"
            };

            return new ChallengeResult(provider, props);
        }

        public async Task<IActionResult> ExternalCallback()
        {
            var externalId = await HttpContext.Authentication.AuthenticateAsync(_appHelper.TempAuthScheme);

            var userId = externalId.FindFirst(ClaimTypes.NameIdentifier).Value;

            var name = externalId.FindFirst(ClaimTypes.GivenName).Value;
            var fullName = externalId.FindFirst(ClaimTypes.Name).Value;
            var surName = externalId.FindFirst(ClaimTypes.Surname).Value;

            var email = externalId.FindFirst(ClaimTypes.Email).Value;

            var newId = new ClaimsIdentity("application", "fullName", "role");
            newId.AddClaim(new Claim("name", name));
            newId.AddClaim(new Claim("surname", surName));
            newId.AddClaim(new Claim("fullName", fullName));
            newId.AddClaim(new Claim("email", email));

            await HttpContext.Authentication.SignInAsync(_appHelper.DefaultAuthScheme, new ClaimsPrincipal(newId));
            await HttpContext.Authentication.SignOutAsync(_appHelper.TempAuthScheme);

            return Redirect(_appHelper.AppPath);
        }

        public async Task Logoff()
        {
            await HttpContext.Authentication.SignOutAsync(_appHelper.DefaultAuthScheme);
            Redirect("/");
        }
    }
}
