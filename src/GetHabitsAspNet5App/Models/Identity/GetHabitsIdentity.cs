using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetHabitsAspNet5App.Models.Identity
{
    public class GetHabitsIdentity: IdentityDbContext<GetHabitsUser>
    {
    }
}
