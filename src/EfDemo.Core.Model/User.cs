using System;
using System.Collections.Generic;

namespace EfDemo.Core.Model
{
    public class User

    {
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public virtual ICollection<UserRole> Roles { get; private set; } = new HashSet<UserRole>();

        public virtual ICollection<UserClaim> Claims { get; private set; } = new HashSet<UserClaim>();

        public virtual ICollection<UserLogin> Logins { get; private set; } = new HashSet<UserLogin>();

        public long Id { get; set; }

        public string UserName { get; set; }

        public DateTime DateOfCreation { get; set; }
    }
}