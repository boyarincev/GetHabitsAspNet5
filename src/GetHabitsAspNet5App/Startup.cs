using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using GetHabitsAspNet5App.Models.DomainModels;
using Microsoft.Framework.Configuration;
using Microsoft.Data.Entity;
using GetHabitsAspNet5App.Services;
using Microsoft.Framework.Logging;
using Microsoft.AspNet.Hosting;
using Microsoft.Dnx.Runtime;
using Microsoft.AspNet.Diagnostics.Entity;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Authentication.Google;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using GetHabitsAspNet5App.Models.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using GetHabitsAspNet5App.Helpers;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Server.Kestrel;

namespace GetHabitsAspNet5App
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            //Setting up configuration builder
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddEnvironmentVariables();

            if (!env.IsProduction())
            {
                builder.AddUserSecrets();
            }
            else
            {

            }

            Configuration = builder.Build();

            //Setting up configuration
            if (!env.IsProduction())
            {
                var confConnectString = Configuration.GetSection("Data:DefaultConnection:ConnectionString");
                confConnectString.Value = @"Server=(localdb)\mssqllocaldb;Database=GetHabitsAspNet5;Trusted_Connection=True;";

                var identityConnection = Configuration.GetSection("Data:IdentityConnection:ConnectionString");
                identityConnection.Value = @"Server=(localdb)\mssqllocaldb;Database=GetHabitsIdentity;Trusted_Connection=True;";
            }
            else
            {

            }
        }

        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetSection("Data:DefaultConnection:ConnectionString").Value;
            var identityConnection = Configuration.GetSection("Data:IdentityConnection:ConnectionString").Value;

            services.AddMvc();

            services.AddEntityFramework().AddSqlServer()
                .AddDbContext<GetHabitsContext>(options => options.UseSqlServer(connection))
                .AddDbContext<GetHabitsIdentity>(options => options.UseSqlServer(identityConnection));

            services.AddScoped<HabitService>();

            services.AddIdentity<GetHabitsUser, IdentityRole>(setup =>
            {

            })
            .AddEntityFrameworkStores<GetHabitsIdentity>();

            services.AddSingleton<GoogleAuthHelper, GoogleAuthHelper>();
            services.AddSingleton<ApplicationHelper, ApplicationHelper>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            GetHabitsContext context, GetHabitsIdentity identContext, GoogleAuthHelper googleAuthHelper, ApplicationHelper appHelper)
        {
            if (env.IsProduction())
            {
                loggerFactory.AddConsole(LogLevel.Verbose);
            }
            else
            {
                loggerFactory.AddConsole(LogLevel.Verbose);
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);
            }

            //TODO do checks
            context.Database.Migrate();
            identContext.Database.Migrate();

            app.UseCookieAuthentication(options =>
            {
                options.AutomaticAuthentication = true;
                options.LoginPath = new PathString("/auth");
                options.AuthenticationScheme = new IdentityCookieOptions().ExternalCookieAuthenticationScheme;
            });

            var clientId = Configuration.GetSection("Authentication:Google:ClientId").Value;
            var clientSecret = Configuration.GetSection("Authentication:Google:ClientSecret").Value;

            app.UseGoogleAuthentication(options =>
            {
                options.ClientId = clientId;
                options.ClientSecret = clientSecret;

                options.Events = new OAuthEvents()
                {
                    OnCreatingTicket = async ticketContext =>
                    {
                        await CheckExistOrCreateUser(ticketContext, googleAuthHelper, appHelper);
                    }
                };
            });

            app.UseIISPlatformHandler();
            app.UseStaticFiles();

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.MapRoute("appRoute", "app/{*all}", new { controller = "Home", action = "Index"});
                routeBuilder.MapRoute("clientSideRouting", "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private async Task CheckExistOrCreateUser(OAuthCreatingTicketContext ticketContext, GoogleAuthHelper googleAuthHelper, ApplicationHelper appHelper)
        {
            var identityContext = ticketContext.HttpContext.RequestServices.GetRequiredService<GetHabitsIdentity>();
            var userManager = ticketContext.HttpContext.RequestServices.GetRequiredService<UserManager<GetHabitsUser>>();

            var userClaims = ticketContext.Identity.Claims.ToList();

            var googleUserId = userClaims.Where(c => c.Type == googleAuthHelper.UserIdType).FirstOrDefault().Value;

            var user = await identityContext.Users
                .Where(u => u.UserName == googleUserId && u.ProviderName == googleAuthHelper.ProviderName)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            string userId = user == null ? null : user.Id;

            if (user == null)
            {
                GetHabitsUser userEntity = await CreateUser(userManager, userClaims, googleUserId, googleAuthHelper);

                userId = userEntity.Id;
            }

            ticketContext.Identity.AddClaim(new Claim(appHelper.TypeClaimUserId, userId));

        }

        private async Task<GetHabitsUser> CreateUser(UserManager<GetHabitsUser> userManager, List<Claim> userClaims, string googleUserId, GoogleAuthHelper googleAuthHelper)
        {
            var email = userClaims.Where(c => c.Type == googleAuthHelper.EmailType).FirstOrDefault().Value;
            var fullName = userClaims.Where(c => c.Type == googleAuthHelper.FullNameType).FirstOrDefault().Value;
            var userName = userClaims.Where(c => c.Type == googleAuthHelper.UserIdType).FirstOrDefault().Value;
            var name = userClaims.Where(c => c.Type == googleAuthHelper.NameType).FirstOrDefault().Value;
            var surName = userClaims.Where(c => c.Type == googleAuthHelper.SurNameType).FirstOrDefault().Value;

            var userEntity = new GetHabitsUser()
            {
                Email = email,
                FullName = fullName,
                UserName = googleUserId,
                Name = name,
                SurName = surName,
                ProviderName = googleAuthHelper.ProviderName
            };

            await userManager.CreateAsync(userEntity);

            return userEntity;
        }
    }
}
