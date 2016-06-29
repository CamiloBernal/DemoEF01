using System;

namespace EfDemo.Core.Model
{
    public interface IUser
    {
        int AccessFailedCount { get; set; }
        string Email { get; set; }
        bool EmailConfirmed { get; set; }

        string Id { get; set; }

        bool LockoutEnabled { get; set; }

        DateTime? LockoutEndDateUtc { get; set; }

        string PasswordHash { get; set; }

        string PhoneNumber { get; set; }

        bool PhoneNumberConfirmed { get; set; }

        string SecurityStamp { get; set; }

        bool TwoFactorEnabled { get; set; }

        string UserName { get; set; }

        DateTime? DateOfCreation { get; set; }
    }
}