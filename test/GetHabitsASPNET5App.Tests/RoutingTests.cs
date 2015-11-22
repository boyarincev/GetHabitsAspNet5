using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Builder.Internal;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;

namespace GetHabitsASPNET5App.Tests
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class RoutingTests
    {
        [Fact]
        public void AppRouteTest()
        {
            //var startup = new GetHabitsAspNet5App.Startup();

            //var serviceCollection = new ServiceCollection();
            //serviceCollection.AddMvc();
            //var serviceProvider = serviceCollection.BuildServiceProvider();
            //startup.ConfigureServices(serviceCollection);

            //var applicationBuilder = new ApplicationBuilder(serviceProvider);

            //applicationBuilder.UseMvc();

            //startup.Configure(applicationBuilder);

            //var app = applicationBuilder.Build();

            //TODO need create httpcontext
            //var context = new DefaultHttpContext();
            //app.Invoke(context);
        }
    }
}
