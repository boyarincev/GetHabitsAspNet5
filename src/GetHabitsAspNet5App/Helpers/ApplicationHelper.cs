using System;
using System.Collections.Generic;
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

        public readonly Dictionary<string, string> CultureAndAddressCorresponding = new Dictionary<string, string>();

        public ApplicationHelper()
        {
            CultureAndAddressCorresponding.Add("ru-RU", "ru");
            CultureAndAddressCorresponding.Add("en", "en");
        }
    }
}
