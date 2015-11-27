using Microsoft.AspNet.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using GetHabitsAspNet5App.Helpers;

namespace GetHabitsAspNet5App.Infrastructure
{
    /// <summary>
    /// Constraint checks that first segment dosen't contain localized segment
    /// </summary>
    public class RequestUnLocalizedRouteConstraint : IRouteConstraint
    {
        private IEnumerable<string> _allowedLangs;

        public RequestUnLocalizedRouteConstraint(IEnumerable<string> allowedLangs)
        {
            _allowedLangs = allowedLangs;
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, IDictionary<string, object> values, RouteDirection routeDirection)
        {
            object objRouteValue;
            values.TryGetValue(routeKey, out objRouteValue);

            string routeValue = objRouteValue.ToString();
            string firstSegment = GetFirstSegment(routeValue);
            var requestContainsLocalizedSegment = _allowedLangs.Contains(firstSegment);
            
            if (!requestContainsLocalizedSegment)
            {
                return true;
            }

            return false;
        }

        private static string GetFirstSegment(string routeValue)
        {
            var firstSlashIndex = routeValue.IndexOf('/');
            string firstSegment = routeValue.Substring(0, firstSlashIndex);
            return firstSegment;
        }
    }
}
