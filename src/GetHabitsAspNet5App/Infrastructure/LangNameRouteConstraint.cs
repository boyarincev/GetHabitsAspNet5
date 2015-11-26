using Microsoft.AspNet.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using GetHabitsAspNet5App.Helpers;

namespace GetHabitsAspNet5App.Infrastructure
{
    public class LangNameRouteConstraint : IRouteConstraint
    {
        private IEnumerable<string> _allowedLangs;

        public LangNameRouteConstraint(IEnumerable<string> allowedLangs)
        {
            _allowedLangs = allowedLangs;
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, IDictionary<string, object> values, RouteDirection routeDirection)
        {
            object objRouteValue;
            values.TryGetValue(routeKey, out objRouteValue);

            string routeValue = objRouteValue.ToString();

            if (_allowedLangs.Contains(routeValue))
            {
                return true;
            }

            return false;
        }
    }
}
