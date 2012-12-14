using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Security;
using WebMatrix.WebData;

namespace MvcMembership.SimpleMembershipSupport
{
    public class SimpleMembershipRepository : ISimpleMembershipRepository
    {
        public MembershipUser GetUserById(int id)
        {
            using (SimpleMembershipContext db = new SimpleMembershipContext())
            {
                var user = this.UsersJoin(db).SingleOrDefault(u => u.UserId == id);
                if (user != null)
                {
                    return user.ToMembershipUser();
                }

                return null;
            }
        }

        public MembershipUser GetUserByName(string userName)
        {
            using (SimpleMembershipContext db = new SimpleMembershipContext())
            {
                var user = this.UsersJoin(db).SingleOrDefault(u => u.UserName == userName);
                if (user != null)
                {
                    return user.ToMembershipUser();
                }

                return null;
            }
        }

        public IEnumerable<MembershipUser> FindByUserName(string userNameToMatch, int pageNumber, int pageSize, out int totalUserCount)
        {
            using (SimpleMembershipContext db = new SimpleMembershipContext())
            {
                var users = this.UsersJoin(db)
                    .ContainsName(userNameToMatch);
                totalUserCount = users.Count();
                return users.PagedAndOrdered(pageNumber, pageSize);
            }
        }

        public IEnumerable<MembershipUser> GetAllUsers(int pageNumber, int pageSize, out int totalUserCount)
        {
            using (SimpleMembershipContext db = new SimpleMembershipContext())
            {
                var usrs = this.UsersJoin(db);
                totalUserCount = usrs.Count();
                return usrs.PagedAndOrdered(pageNumber, pageSize);
            }
        }
        
        public bool DeleteUser(string userName)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (SimpleMembershipContext db = new SimpleMembershipContext())
                {
                    var userToDelete = db.UserProfiles.SingleOrDefault(u => u.UserName == userName);
                    if (userToDelete != null)
                    {
                        this.RemoveMembershipData(db, userToDelete.UserId);
                        this.RemoveOauthMembershipData(db, userToDelete.UserId);
                        db.UserProfiles.Remove(userToDelete);
                        db.SaveChanges();
                        ts.Complete();
                        return true;
                    }

                    return false;
                }
            }
        }

        private IQueryable<SimpleMembershipUser> UsersJoin(SimpleMembershipContext db)
        {
            return db.UserProfiles.GroupJoin(
                  db.Memberships,
                  up => up.UserId,
                  m => m.UserId,
                  (up, m) => new { UserProfiles = up, Memmerships = m })
                .SelectMany(
                  up => up.Memmerships.DefaultIfEmpty(),
                  (up, m) =>
                      new SimpleMembershipUser
                      {
                          UserName = up.UserProfiles.UserName,
                          UserId = up.UserProfiles.UserId,
                          CreationDate = m.CreationDate
                      }
                 );
        }

        private void RemoveMembershipData(SimpleMembershipContext db, int userId)
        {
            var membershipData = db.Memberships.SingleOrDefault(m => m.UserId == userId);
            if (membershipData != null)
            {
                db.Memberships.Remove(membershipData);
            }
        }

        private void RemoveOauthMembershipData(SimpleMembershipContext db, int userId)
        {
            var oauthMembershipData = db.OAuthMemberships.Where(om => om.UserId == userId);
            if (oauthMembershipData.Count() > 0)
            {
                oauthMembershipData.ToList().ForEach(md =>
                {
                    db.OAuthMemberships.Remove(md);
                });
            }
        }
    }
}
