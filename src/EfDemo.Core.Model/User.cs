using System;
using System.Collections.Generic;

namespace EfDemo.Core.Model
{
    public class User

    {
        public virtual string Email { get; set; }

        public virtual bool EmailConfirmed { get; set; }

        public virtual string PasswordHash { get; set; }

        public virtual string SecurityStamp { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual bool PhoneNumberConfirmed { get; set; }

        public virtual bool TwoFactorEnabled { get; set; }

        public virtual DateTime? LockoutEndDateUtc { get; set; }

        public virtual bool LockoutEnabled { get; set; }

        public virtual int AccessFailedCount { get; set; }

        public virtual ICollection<UserRole> Roles { get; private set; } = new List<UserRole>();

        public virtual ICollection<UserClaim> Claims { get; private set; } = new List<UserClaim>();

        public virtual ICollection<UserLogin> Logins { get; private set; } = new List<UserLogin>();

        public virtual long Id { get; set; }

        public virtual string UserName { get; set; }
    }
}