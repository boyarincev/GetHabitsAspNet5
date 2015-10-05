using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;

namespace GetHabitsAspNet5App
{
    public class Startup
    {
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

        }

        public void Configure(IApplicationBuilder ap    )
        {
            app.UseStaticFiles();

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.MapRoute("appRoute", "app/{*all}", new { controller = "Home", action = "Index"});
                routeBuilder.MapRoute("clientSideRouting", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
