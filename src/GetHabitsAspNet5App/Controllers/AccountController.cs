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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GetHabitsAspNet5App.Controllers
{
    public class AccountController : LocalizeController
    {
        private GetHabitsIdentity _identityContext;
        private UserManager<GetHabitsUser> _userMng;
        private GoogleAuthHelper _googleAuthHelper;

        public AccountController(ApplicationHelper appHelper, GoogleAuthHelper googleAuthHelper, GetHabitsIdentity identityContext, UserManager<GetHabitsUser> userManager)
            : base(appHelper)
        {
            _identityContext = identityContext;
            _userMng = userManager;
            _googleAuthHelper = googleAuthHelper;
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
            var googleUserClaimsPrincipal = await HttpContext.Authentication.AuthenticateAsync(_appHelper.TempAuthScheme);

            GetHabitsUser internalUser = PopulateInternalUserFromGoogleClaims(googleUserClaimsPrincipal);

            if (GoogleUserExist(internalUser.AuthProviderId))
            {
                internalUser = await GetSavedGoogleUser(internalUser.AuthProviderId);
            }
            else
            {
                internalUser = await CreateUser(internalUser);
            }

            ClaimsPrincipal internalClaimsPrincipal = CreateInternalClaimsPrincipal(internalUser);

            await HttpContext.Authentication.SignInAsync(_appHelper.DefaultAuthScheme, internalClaimsPrincipal);
            await HttpContext.Authentication.SignOutAsync(_appHelper.TempAuthScheme);

            return Redirect(_appHelper.AppPath);
        }

        public async Task Logoff()
        {
            await HttpContext.Authentication.SignOutAsync(_appHelper.DefaultAuthScheme);
            Redirect("/");
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

        private GetHabitsUser PopulateInternalUserFromGoogleClaims(ClaimsPrincipal googleUserClaims)
        {
            GetHabitsUser internalUser = new GetHabitsUser();

            internalUser.AuthProviderId = googleUserClaims.FindFirst(ClaimTypes.NameIdentifier).Value;

            internalUser.Name = googleUserClaims.FindFirst(ClaimTypes.GivenName).Value;
            internalUser.FullName = googleUserClaims.FindFirst(ClaimTypes.Name).Value;
            internalUser.SurName = googleUserClaims.FindFirst(ClaimTypes.Surname).Value;

            internalUser.Email = googleUserClaims.FindFirst(ClaimTypes.Email).Value;

            return internalUser;
        }

        private async Task<GetHabitsUser> CreateUser(GetHabitsUser userEntity)
        {
            //TODO don't save user into Db, need check saving result
            await _userMng.CreateAsync(userEntity);

            return userEntity;
        }

        private async Task<GetHabitsUser> GetSavedGoogleUser(string googleUserId)
        {
            var user = await _identityContext.Users
                .Where(u => u.AuthProviderId == googleUserId && u.FromAuthProvider == _googleAuthHelper.ProviderName)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return user;
        }

        private bool GoogleUserExist(string googleUserId)
        {
            var isExist = _identityContext.Users
                .Any(u => u.AuthProviderId == googleUserId && u.FromAuthProvider == _googleAuthHelper.ProviderName);

            return isExist;
        }
    }
}
