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

        public readonly Dictionary<string, CultureInfo> AddressAndCultureCorresponding = new Dictionary<string, CultureInfo>();

        public ApplicationHelper()
        {
            AddressAndCultureCorresponding.Add("ru", new CultureInfo("ru-RU"));
            AddressAndCultureCorresponding.Add("en", new CultureInfo("en-US"));
        }
    }
}
