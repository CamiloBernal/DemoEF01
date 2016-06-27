using System.Data.Entity;
using EfDemo.Core.Model;

namespace EfDemo.Data.Providers.SQL
{
    public class GoalsDbContext : DbContext
    {
        public GoalsDbContext(string connectionString)
            : base(connectionString)
        {
            //Default CTOR
        }

        public DbSet<Goal> Goals { get; set; }
    }
}