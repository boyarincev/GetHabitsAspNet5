using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetHabitsAspNet5App.Models.Identity
{
    public class GetHabitsUser: IdentityUser
    {
        public string FromAuthProvider { get; set; }
        public string ExternalId { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string FullName { get; set; }
    }
}
