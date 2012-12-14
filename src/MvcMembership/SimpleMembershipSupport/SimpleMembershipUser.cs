using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcMembership.SimpleMembershipSupport
{
    public class SimpleMembershipUser
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
