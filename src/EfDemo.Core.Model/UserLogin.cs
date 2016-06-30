namespace EfDemo.Core.Model
{
    public class UserLogin
    {
        public virtual string LoginProvider { get; set; }

        public virtual string ProviderKey { get; set; }

        public virtual long UserId { get; set; }
    }
}