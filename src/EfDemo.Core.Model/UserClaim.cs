﻿namespace EfDemo.Core.Model
{
    public class UserClaim
    {
        public int Id { get; set; }

        public long UserId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }
    }
}