using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using GetHabitsAspNet5App.Models.DomainModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Entity;
using GetHabitsAspNet5App.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Authentication.OAuth;
using GetHabitsAspNet5App.Models.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using GetHabitsAspNet5App.Helpers;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.WebEncoders;

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

            services.AddLocalization();
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
                app.UseDatabaseErrorPage(opt => opt.EnableAll());
            }

            //TODO do checks
            context.Database.Migrate();
            identContext.Database.Migrate();

            app.UseCookieAuthentication(options =>
            {
                options.AutomaticAuthenticate = true;
                options.AutomaticChallenge = true;
                options.LoginPath = new PathString("/account/login");
                options.LogoutPath = new PathString("/account/logoff");
                options.AuthenticationScheme = appHelper.DefaultAuthScheme;
            });

            app.UseCookieAuthentication(options =>
            {
                options.AutomaticAuthenticate = false;
                options.AuthenticationScheme = appHelper.TempAuthScheme;
            });

            var clientId = Configuration.GetSection("Authentication:Google:ClientId").Value;
            var clientSecret = Configuration.GetSection("Authentication:Google:ClientSecret").Value;

            app.UseGoogleAuthentication(options =>
            {
                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.AuthenticationScheme = googleAuthHelper.ProviderName;
                options.AutomaticAuthenticate = false;
                options.SignInScheme = appHelper.TempAuthScheme;

                options.Events = new OAuthEvents()
                {
                    OnRemoteError = async errorContext =>
                    {
                        var error = errorContext.Error;
                        errorContext.Response.Redirect("/error?ErrorMessage=" + UrlEncoder.Default.UrlEncode(errorContext.Error.Message));
                        errorContext.HandleResponse();
                        await Task.FromResult(0);
                    }
                };
            });

            app.UseIISPlatformHandler();
            app.UseStaticFiles();

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.MapRoute("appRoute", "app/{*all}", new { controller = "App", action = "Index"});
                //routeBuilder.MapRoute("clientSideRouting", "{culture}/{controller=Home}/{action=Index}/{id?}");
                routeBuilder.MapRoute("clientSideRouting", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
