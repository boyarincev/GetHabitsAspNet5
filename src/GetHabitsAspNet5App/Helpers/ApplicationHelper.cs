using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GetHabitsAspNet5App.Helpers
{
    public class ApplicationHelper
    {
        /// <summary>
        /// Type of Claim contains UserId
        /// </summary>
        public readonly string TypeClaimUserId = "UserId";

        /// <summary>
        /// Maximum count checkins, what can queried from HabitServise
        /// </summary>
        public readonly int MaxCheckinCount = 30;

        public readonly string DefaultAuthScheme = "Cookies";

        public readonly string TempAuthScheme = "Temp";

        public readonly string AppPath = "/app/";

        public readonly string DefaultLangName = "ru";

        public readonly Dictionary<string, CultureInfo> LangNameAndCultureNameCorresponding = new Dictionary<string, CultureInfo>();

        public ApplicationHelper()
        {
            LangNameAndCultureNameCorresponding.Add("ru", new CultureInfo("ru-RU"));
            LangNameAndCultureNameCorresponding.Add("en", new CultureInfo("en-US"));
        }
    }
}
