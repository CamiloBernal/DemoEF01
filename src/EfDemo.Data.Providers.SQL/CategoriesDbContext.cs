using System.Data.Entity;
using EfDemo.Core.Model;

namespace EfDemo.Data.Providers.SQL
{
    public class CategoriesDbContext : DbContext
    {
        public IDbSet<Category> Categories { get; set; }
        //public IDbSet<User> Users { get; set; }

        //public IDbSet<UserLogin> UserLogins { get; set; }

        //public IDbSet<UserRole> UserRoles { get; set; }

        public CategoriesDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //Default CTOR
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new CategoryMappings());
            modelBuilder.Configurations.Add(new UserLoginMappings());
            modelBuilder.Configurations.Add(new UserRoleMappings());
        }
    }
}