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

namespace GetHabitsAspNet5App
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            //var confConnectString2 = Configuration.GetSection("Data:DefaultConnection:ConnectionString");
            //confConnectString2.Value = @"Server=(localdb)\mssqllocaldb;Database=GetHabitsAspNet5;Trusted_Connection=True;";

            if (!env.IsProduction())
            {
                var confConnectString = Configuration.GetSection("Data:DefaultConnection:ConnectionString");
                confConnectString.Value = @"Server=(localdb)\mssqllocaldb;Database=GetHabitsAspNet5;Trusted_Connection=True;";
            }
            else
            {

            }
        }

        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetSection("Data:DefaultConnection:ConnectionString").Value;
            //var connection = Configuration.Get("Data:DefaultConnection:ConnectionString");

            services.AddMvc();
            services.AddEntityFramework().AddSqlServer()
                .AddDbContext<GetHabitsContext>(options => options.UseSqlServer((connection)));
            services.AddScoped<HabitService>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, GetHabitsContext context)
        {

            context.Database.Migrate();

            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);

            loggerFactory.AddConsole(LogLevel.Verbose);

            app.UseStaticFiles();

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.MapRoute("appRoute", "app/{*all}", new { controller = "Home", action = "Index"});
                //routeBuilder.MapRoute("apiRoute", "api/{controller}");
                routeBuilder.MapRoute("clientSideRouting", "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
