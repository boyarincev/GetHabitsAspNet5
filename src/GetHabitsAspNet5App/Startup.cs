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
using Microsoft.AspNet.Diagnostics.Entity;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.Runtime;
using Microsoft.Data.Entity.Relational.Migrations;

namespace GetHabitsAspNet5App
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder(appEnv.ApplicationBasePath).AddEnvironmentVariables();
            Configuration = builder.Build();

            //Configuration.Set("Data:DefaultConnection:ConnectionString", @"Server=(localdb)\mssqllocaldb;Database=GetHabitsAspNet5;Trusted_Connection=True;");

            if (!env.IsProduction())
            {
                Configuration.Set("Data:DefaultConnection:ConnectionString", @"Server=(localdb)\mssqllocaldb;Database=GetHabitsAspNet5;Trusted_Connection=True;");
            }
            else
            {
                
            }
        }

        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.Get("Data:DefaultConnection:ConnectionString");

            services.AddMvc();
            services.AddEntityFramework().AddSqlServer()
                .AddDbContext<GetHabitsContext>(options => options.UseSqlServer((connection)));
            services.AddScoped<HabitService>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, GetHabitsContext context)
        {

            context.Database.AsRelational().ApplyMigrations();

            app.UseErrorPage();
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
