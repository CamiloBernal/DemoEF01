using System.Data.Entity;
using EfDemo.Core.Model;

namespace EfDemo.Data.Providers.SQL
{
    public class CategoriesDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public CategoriesDbContext(string connectionString)
            : base(connectionString)
        {
            //Default CTOR
        }
    }
}