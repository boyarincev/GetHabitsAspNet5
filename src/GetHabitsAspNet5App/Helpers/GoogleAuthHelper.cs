using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetHabitsAspNet5App.Helpers
{
    public static class GoogleAuthHelper
    {
        public static readonly string EmailType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
        public static readonly string FullNameType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        public static readonly string NameType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
        public static readonly string SurNameType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
        public static readonly string UserIdType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    }
}
