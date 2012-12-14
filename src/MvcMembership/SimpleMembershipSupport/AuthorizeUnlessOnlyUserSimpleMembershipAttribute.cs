using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcMembership
{
    public class AuthorizeUnlessOnlyUserSimpleMembershipAttribute : AuthorizeUnlessOnlyUserAttribute
    {
        public AuthorizeUnlessOnlyUserSimpleMembershipAttribute() 
            : base(new AspNetSimpleMembershipProviderWrapper(), new AspNetRoleProviderWrapper())
        {
        }
    }
}
