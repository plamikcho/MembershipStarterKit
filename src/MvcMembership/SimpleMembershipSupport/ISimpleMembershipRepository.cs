using System;

namespace MvcMembership.SimpleMembershipSupport
{
    public interface ISimpleMembershipRepository
    {
        bool DeleteUser(string userName);
        System.Collections.Generic.IEnumerable<System.Web.Security.MembershipUser> FindByUserName(string userNameToMatch, int pageNumber, int pageSize, out int totalUserCount);
        System.Collections.Generic.IEnumerable<System.Web.Security.MembershipUser> GetAllUsers(int pageNumber, int pageSize, out int totalUserCount);
        System.Web.Security.MembershipUser GetUserById(int id);
        System.Web.Security.MembershipUser GetUserByName(string userName);
    }
}
