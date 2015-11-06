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
        public string TypeClaimUserId = "UserId";

        /// <summary>
        /// Maximum count checkins, what can queried from HabitServise
        /// </summary>
        public int MaxCheckinCount = 30;
    }
}
