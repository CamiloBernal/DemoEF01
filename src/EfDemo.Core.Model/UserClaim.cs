namespace EfDemo.Core.Model
{
    public class UserClaim
    {
        public virtual int Id { get; set; }

        public virtual long UserId { get; set; }

        public virtual string ClaimType { get; set; }

        public virtual string ClaimValue { get; set; }
    }
}