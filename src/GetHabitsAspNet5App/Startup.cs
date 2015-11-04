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

            services.AddAuthentication(options => { options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; });

            services.AddMvc();

            services.AddEntityFramework().AddSqlServer()
                .AddDbContext<GetHabitsContext>(options => options.UseSqlServer(connection))
                .AddDbContext<GetHabitsIdentity>(options => options.UseSqlServer(identityConnection));

            services.AddScoped<HabitService>();

            //not need yet
            //services.AddIdentity<GetHabitsUser, IdentityRole>(setup => { })
            //    .AddEntityFrameworkStores<GetHabitsIdentity>();
        }

        //TODO split to prod and develop parts
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, GetHabitsContext context, GetHabitsIdentity identContext)
        {
            loggerFactory.AddConsole(LogLevel.Verbose);
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);

            //TODO do checks
            context.Database.Migrate();
            identContext.Database.Migrate();

            app.UseCookieAuthentication(options =>
            {
                options.AutomaticAuthentication = true;
                options.LoginPath = new PathString("/auth");
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
                        await CheckExistOrCreateUser(app, ticketContext);
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

        private async Task CheckExistOrCreateUser(IApplicationBuilder app, OAuthCreatingTicketContext ticketContext)
        {
            var identityContext = app.ApplicationServices.GetService<GetHabitsIdentity>();

            var emailType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
            var fullNameType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
            var nameType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
            var surNameType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
            var userIdType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

            var userClaims = ticketContext.Identity.Claims.ToList();

            var userId = userClaims.Where(c => c.Type == userIdType).FirstOrDefault().Value;
            var providerName = "Google";

            var user = await identityContext.Users
                .Where(u => u.UserName == userId && u.ProviderName == providerName)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            //TODO add claim with user Id
            //ticketContext.Identity.AddClaim(new System.Security.Claims.Claim("UserId", userId));

            if (user != null)
            {
                return;
            }

            var email = userClaims.Where(c => c.Type == emailType).FirstOrDefault().Value;
            var fullName = userClaims.Where(c => c.Type == fullNameType).FirstOrDefault().Value;
            var userName = userClaims.Where(c => c.Type == userIdType).FirstOrDefault().Value;
            var name = userClaims.Where(c => c.Type == nameType).FirstOrDefault().Value;
            var surName = userClaims.Where(c => c.Type == surNameType).FirstOrDefault().Value;

            var userEntity = new GetHabitsUser()
            {
                Email = email,
                FullName = fullName,
                UserName = userId,
                Name = name,
                SurName = surName,
                ProviderName = providerName
            };

            identityContext.Users.Add(userEntity);
            await identityContext.SaveChangesAsync();
        }
    }
}
