using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Identity;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GetHabitsAspNet5App.Controllers
{
    public class AuthController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(string authScheme)
        {
            await HttpContext.Authentication.ChallengeAsync(authScheme, new AuthenticationProperties() { RedirectUri = "/" });

            return Content("signin-google");
        }

        public async Task LogOut()
        {
            await HttpContext.Authentication.SignOutAsync(new IdentityOptions().Cookies.ExternalCookieAuthenticationScheme);
        }
    }
}
