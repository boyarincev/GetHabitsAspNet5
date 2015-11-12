using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetHabitsAspNet5App.Helpers
{
    public class GoogleAuthHelper
    {
        public readonly string EmailType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
        public readonly string FullNameType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        public readonly string NameType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
        public readonly string SurNameType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
        public readonly string UserIdType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        public readonly string ProviderName = "Google";
    }
}
