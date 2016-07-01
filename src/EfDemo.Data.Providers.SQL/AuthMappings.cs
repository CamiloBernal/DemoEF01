using System.Data.Entity.ModelConfiguration;
using EfDemo.Core.Model;

namespace EfDemo.Data.Providers.SQL
{
    public class UserLoginMappings : EntityTypeConfiguration<UserLogin>
    {

        public UserLoginMappings()
        {
            HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId });
        }
    }


    public class UserRoleMappings : EntityTypeConfiguration<UserRole>
    {

        public UserRoleMappings()
        {
            HasKey(r => new { r.UserId, r.RoleId });
        }
    }


}