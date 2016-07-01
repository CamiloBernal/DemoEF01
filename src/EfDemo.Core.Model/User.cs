using System;
using System.Collections.Generic;

namespace EfDemo.Core.Model
{
    public class User

    {
        public int AccessFailedCount { get; set; }
        public virtual ICollection<UserClaim> Claims { get; private set; } = new HashSet<UserClaim>();
        public DateTime DateOfCreation { get; set; }
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public long Id { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public virtual ICollection<UserLogin> Logins { get; private set; } = new HashSet<UserLogin>();
        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public virtual ICollection<UserRole> Roles { get; private set; } = new HashSet<UserRole>();
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public virtual ICollection<Category> UserCategories { get; set; }
        public string UserName { get; set; }
    }
}