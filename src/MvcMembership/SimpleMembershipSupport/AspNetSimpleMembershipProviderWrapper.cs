using System;
using System.Collections.Generic;
using System.Web.Security;
using MvcMembership.Settings;
using MvcMembership.SimpleMembershipSupport;
using PagedList;
using WebMatrix.WebData;

namespace MvcMembership
{
    public class AspNetSimpleMembershipProviderWrapper : IUserService, IPasswordService
    {
        private readonly MembershipProvider _membershipProvider;
        private readonly ISimpleMembershipRepository _simpleMembershipRepository;

		public AspNetSimpleMembershipProviderWrapper() : this(Membership.Provider, new SimpleMembershipRepository())
		{
		}

        public AspNetSimpleMembershipProviderWrapper(MembershipProvider membershipProvider) 
            : this(membershipProvider, new SimpleMembershipRepository())
		{
		}

        public AspNetSimpleMembershipProviderWrapper(ISimpleMembershipRepository simpleMembershipRepository)
            : this(Membership.Provider, simpleMembershipRepository)
        {
        }

        public AspNetSimpleMembershipProviderWrapper(MembershipProvider membershipProvider, ISimpleMembershipRepository simpleMembershipRepository)
        {
            _membershipProvider = membershipProvider;
            _simpleMembershipRepository = simpleMembershipRepository;
        }

		public AspNetMembershipProviderSettingsWrapper Settings
		{
			get{ return new AspNetMembershipProviderSettingsWrapper(_membershipProvider); }
		}

        #region IUserService Members

        public IPagedList<MembershipUser> FindAll(int pageNumber, int pageSize)
        {
            // get one page of users
            int totalUserCount = 0;
            var usersListSimple = _simpleMembershipRepository.GetAllUsers(pageNumber - 1, pageSize, out totalUserCount);
            return new StaticPagedList<MembershipUser>(usersListSimple, pageNumber, pageSize, totalUserCount);
        }

        public IPagedList<MembershipUser> FindByEmail(string emailAddressToMatch, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IPagedList<MembershipUser> FindByUserName(string userNameToMatch, int pageNumber, int pageSize)
        {
            // get one page of users
            int totalUserCount;
            var usersListSimple = _simpleMembershipRepository.FindByUserName(userNameToMatch, pageNumber - 1, pageSize, out totalUserCount);
            return new StaticPagedList<MembershipUser>(usersListSimple, pageNumber, pageSize, totalUserCount);
        }

        public MembershipUser Get(string userName)
        {
            return _membershipProvider.GetUser(userName, false);
        }

        public MembershipUser Get(object providerUserKey)
        {
            return _simpleMembershipRepository.GetUserById(Convert.ToInt32(providerUserKey));
        }

        public MembershipUser Create(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved)
        {
            WebSecurity.CreateUserAndAccount(username, password, null, false);
            int userid = WebSecurity.GetUserId(username);
            var cd = WebSecurity.GetCreateDate(username);
            return new SimpleMembershipUser { UserId = userid, UserName = username, CreationDate = cd }.ToMembershipUser();
        }

        public MembershipUser Create(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey)
        {
            return Create(username, password, email, passwordQuestion, passwordAnswer, isApproved);
        }

        public void Update(MembershipUser user)
        {
            _membershipProvider.UpdateUser(user);
        }

        public void Delete(MembershipUser user)
        {
            Delete(user, false);
        }

        public void Delete(MembershipUser user, bool deleteAllRelatedData)
        {
            _simpleMembershipRepository.DeleteUser(user.UserName);
        }

        public MembershipUser Touch(MembershipUser user)
        {
            return _membershipProvider.GetUser(user.UserName, true);
        }

        public MembershipUser Touch(string userName)
        {
            return _membershipProvider.GetUser(userName, true);
        }

        public MembershipUser Touch(object providerUserKey)
        {
            return _membershipProvider.GetUser(providerUserKey, true);
        }

        public int TotalUsers
        {
            get
            {
                int totalUsers = 0;
                _simpleMembershipRepository.GetAllUsers(1, 1, out totalUsers);
                return totalUsers;
            }
        }

        public int UsersOnline
        {
            get
            {
                return _membershipProvider.GetNumberOfUsersOnline();
            }
        }

        #endregion

        #region IPasswordService Members

        public void Unlock(MembershipUser user)
        {
            throw new NotImplementedException();
            //user.UnlockUser();
        }

        public string ResetPassword(MembershipUser user)
        {
            throw new NotImplementedException();
            //return user.ResetPassword();
        }

        public string ResetPassword(MembershipUser user, string passwordAnswer)
        {
            throw new NotImplementedException();
            //return user.ResetPassword(passwordAnswer);
        }

        public void ChangePassword(MembershipUser user, string newPassword)
        {
            throw new NotImplementedException();
            //var resetPassword = user.ResetPassword();
            //if(!user.ChangePassword(resetPassword, newPassword))
            //    throw new MembershipPasswordException("Could not change password.");
        }

        public void ChangePassword(MembershipUser user, string oldPassword, string newPassword)
        {
            if (!WebSecurity.ChangePassword(user.UserName, oldPassword, newPassword))
                throw new MembershipPasswordException("Could not change password.");
        }

        #endregion
    }
}
