using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcMembership
{
    public class AspNetSimpleMembershipProviderUserServiceFactory : IUserServiceFactory
    {
        public IUserService Make()
        {
            return new AspNetSimpleMembershipProviderWrapper();
        }
    }
}
