using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MvcMembership.SimpleMembershipSupport
{
    public class SimpleMembershipContext : DbContext
    {
        public SimpleMembershipContext() : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserMembership> Memberships { get; set; }
        public DbSet<OAuthMembership> OAuthMemberships { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets employee id 
        /// (used for additional profile information currently stored in the EMPLOY TABLE)
        /// </summary>
        public int EmployeeID { get; set; }
    }

    [Table("webpages_Membership")]
    public class UserMembership
    {
        [Key]
        public int UserId { get; set; }

        [ReadOnly(true)]
        [Column("CreateDate")]
        public virtual DateTime CreationDate { get; set; }
        public virtual bool IsConfirmed { get; set; }
    }

    [Table("webpages_OAuthMembership")]
    public class OAuthMembership
    {
        [Key]
        [Column("Provider", Order = 1)]
        public string Provider { get; set; }

        [Key]
        [Column("ProviderUserId", Order = 2)]
        public string ProviderUserId { get; set; }
        public int UserId { get; set; }
    }
}
