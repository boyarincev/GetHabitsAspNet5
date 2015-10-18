using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Framework.DependencyInjection;

namespace GetHabitsASPNET5App.Tests
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class RoutingTests
    {
        [Fact]
        public void Test1()
        {
            Assert.True(true);
        }

        //[Fact]
        //public void AppRouteTest()
        //{
        //    var startup = new GetHabitsAspNet5App.Startup();

        //    var serviceCollection = new ServiceCollection();
        //    //serviceCollection.AddMvc();
        //    startup.ConfigureServices(serviceCollection);

        //    var applicationBuilder = new ApplicationBuilder(serviceCollection.BuildServiceProvider());
        //    //applicationBuilder.UseMvc();

        //    startup.Configure(applicationBuilder);

        //    var app = applicationBuilder.Build();

        //    //TODO need create httpcontext
        //    //app.Invoke()
        //}
    }
}
