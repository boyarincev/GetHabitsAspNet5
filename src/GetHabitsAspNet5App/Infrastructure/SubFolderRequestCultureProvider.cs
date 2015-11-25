using Microsoft.AspNet.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using GetHabitsAspNet5App.Helpers;
using System.Globalization;

namespace GetHabitsAspNet5App.Infrastructure
{
    public class SubFolderRequestCultureProvider : IRequestCultureProvider
    {
        private ApplicationHelper _appHelper;

        public SubFolderRequestCultureProvider(ApplicationHelper appHelper)
        {
            _appHelper = appHelper;
        }

        public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            string path = GetPath(httpContext);
            
            string firstSegment = GetFirstSegment(path);

            List<KeyValuePair<string, CultureInfo>> listAvailableCulture = GetListAvailableCulture(firstSegment);

            if (listAvailableCulture.Count != 0)
            {
                ProviderCultureResult providerCultureResult = CreateProviderCultureResult(listAvailableCulture);
                return Task.FromResult(providerCultureResult);
            }

            return Task.FromResult<ProviderCultureResult>(null);
        }

        private ProviderCultureResult CreateProviderCultureResult(List<KeyValuePair<string, CultureInfo>> listAvailableCulture)
        {
            return new ProviderCultureResult(listAvailableCulture[0].Value.Name, listAvailableCulture[0].Value.Name);
        }

        private List<KeyValuePair<string, CultureInfo>> GetListAvailableCulture(string firstSegmentFromPath)
        {
            return _appHelper.AddressAndCultureCorresponding.Where(acc => firstSegmentFromPath == "/" + acc.Key + "/").ToList();
        }

        private string GetPath(HttpContext httpContext)
        {
            return httpContext.Request.Path.Value.ToLowerInvariant();
        }

        private string GetFirstSegment(string pathWithBeginningSlash)
        {
            pathWithBeginningSlash = EnsureBeginningSlash(pathWithBeginningSlash);
            var firstSegment = "";
            
            var secondSlashIndex = pathWithBeginningSlash.IndexOf("/", 1);
            if (secondSlashIndex == -1)
            {
                firstSegment = pathWithBeginningSlash;
            }
            else
            {
                firstSegment = pathWithBeginningSlash.Substring(0, secondSlashIndex + 1);
            }

            firstSegment = EnsureTrailedSlash(firstSegment);

            return firstSegment;
        }

        private string EnsureBeginningSlash(string path)
        {
            if (path[0] != '/')
                path = '/' + path;

            return path;
        }

        private string EnsureTrailedSlash(string path)
        {
            if (!path.EndsWith("/"))
                path += '/';

            return path;
        }
    }
}
