using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using GetHabitsAspNet5App.Helpers;
using Microsoft.Extensions.DependencyInjection;
using GetHabitsAspNet5App.Models.Identity;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GetHabitsAspNet5App.Controllers
{
    public class AccountController : LocalizeController
    {
        private GetHabitsIdentity _identityContext;
        private UserManager<GetHabitsUser> _userMng;
        private GoogleAuthHelper _googleAuthHelper;
        private ILogger _logger;

        public AccountController(ApplicationHelper appHelper, GoogleAuthHelper googleAuthHelper, 
            GetHabitsIdentity identityContext, UserManager<GetHabitsUser> userManager, ILoggerFactory loggerFactory)
            : base(appHelper)
        {
            _identityContext = identityContext;
            _userMng = userManager;
            _googleAuthHelper = googleAuthHelper;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        public IActionResult External(string provider)
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = "/account/external" + provider + "Callback"
            };

            return new ChallengeResult(provider, props);
        }

        public async Task<IActionResult> ExternalGoogleCallback()
        {
            await SigninGoogleUser();
            return Redirect(_appHelper.AppPath);
        }

        public async Task<IActionResult> Logoff()
        {
            if(HttpContext.User.IsSignedIn())
                await HttpContext.Authentication.SignOutAsync(_appHelper.DefaultAuthScheme);

            return Redirect("/");
        }

        private async Task SigninGoogleUser()
        {
            GetHabitsUser internalUser = await GetInternalUserFromGoogleClaims();

            if (isGoogleUserSaved(internalUser.ExternalId))
            {
                internalUser = await GetGoogleUserFromDb(internalUser.ExternalId);
            }
            else
            {
                internalUser = await CreateUser(internalUser);
            }

            ClaimsPrincipal internalClaimsPrincipal = CreateInternalClaimsPrincipal(internalUser);

            await HttpContext.Authentication.SignInAsync(_appHelper.DefaultAuthScheme, internalClaimsPrincipal);
            await HttpContext.Authentication.SignOutAsync(_appHelper.TempAuthScheme);
        }

        private async Task<GetHabitsUser> GetInternalUserFromGoogleClaims()
        {
            var googleUserClaimsPrincipal = await HttpContext.Authentication.AuthenticateAsync(_appHelper.TempAuthScheme);
            GetHabitsUser internalUser = PopulateInternalUserFromGoogleClaims(googleUserClaimsPrincipal);

            return internalUser;
        }

        private GetHabitsUser PopulateInternalUserFromGoogleClaims(ClaimsPrincipal googleUserClaims)
        {
            GetHabitsUser internalUser = new GetHabitsUser();

            internalUser.ExternalId = googleUserClaims.FindFirst(ClaimTypes.NameIdentifier).Value;
            internalUser.UserName = internalUser.ExternalId;

            internalUser.Name = googleUserClaims.FindFirst(ClaimTypes.GivenName).Value;
            internalUser.FullName = googleUserClaims.FindFirst(ClaimTypes.Name).Value;
            internalUser.SurName = googleUserClaims.FindFirst(ClaimTypes.Surname).Value;

            internalUser.Email = googleUserClaims.FindFirst(ClaimTypes.Email).Value;

            return internalUser;
        }

        private async Task<GetHabitsUser> CreateUser(GetHabitsUser userEntity)
        {
            var result = await _userMng.CreateAsync(userEntity);

            if (!result.Succeeded)
            {
                //if not succeeded do nothing. it's not critical, user whatever can authenticate.
            }

            return userEntity;
        }

        private async Task<GetHabitsUser> GetGoogleUserFromDb(string googleUserId)
        {
            var user = await _identityContext.Users
                .Where(u => u.ExternalId == googleUserId && u.FromAuthProvider == _googleAuthHelper.ProviderName)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return user;
        }

        private bool isGoogleUserSaved(string googleUserId)
        {
            var isExist = _identityContext.Users
                .Any(u => u.ExternalId == googleUserId && u.FromAuthProvider == _googleAuthHelper.ProviderName);

            return isExist;
        }

        private ClaimsPrincipal CreateInternalClaimsPrincipal(GetHabitsUser internalUser)
        {
            var newClaims = new ClaimsIdentity("application", "fullName", "role");
            newClaims.AddClaim(new Claim("name", internalUser.Name));
            newClaims.AddClaim(new Claim("surname", internalUser.SurName));
            newClaims.AddClaim(new Claim("fullName", internalUser.FullName));
            newClaims.AddClaim(new Claim("email", internalUser.Email));
            newClaims.AddClaim(new Claim("id", internalUser.Id));

            var internalClaimsPrincipal = new ClaimsPrincipal(newClaims);
            return internalClaimsPrincipal;
        }
    }
}
