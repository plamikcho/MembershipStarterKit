using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace MvcMembership.SimpleMembershipSupport
{
    public static class MembershipExtensions
    {
        public static MembershipUser ToMembershipUser(this SimpleMembershipUser simpleUser)
        {
            return new MembershipUser(
                        "AspNetSqlMembershipProvider",
                        simpleUser.UserName, simpleUser.UserId, null, null, null, true, false,
                        simpleUser.CreationDate.HasValue ? simpleUser.CreationDate.Value : DateTime.MinValue, 
                        DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
        }

        public static IEnumerable<MembershipUser> PagedAndOrdered(this IQueryable<SimpleMembershipUser> usersQuery, int pageNumber, int pageSize)
        {
            return usersQuery.OrderBy(o => o.UserName)
                 .Skip(pageNumber * pageSize).Take(pageSize)
                 .ToList()
                 .Select(s => s.ToMembershipUser());
        }

        public static IQueryable<SimpleMembershipUser> ContainsName(this IQueryable<SimpleMembershipUser> usersQuery, string userNameToMatch)
        {
            return usersQuery.Where(u => u.UserName.Contains(userNameToMatch));
        }
    }
}
