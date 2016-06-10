using System.Data.Entity;
using Domain;

namespace Data.SQL
{
    public class PersonDbContext : DbContext
    {

        public PersonDbContext(string connectionString)
            :base(connectionString)
        {
            //Default CTOR
        }


        public DbSet<Person> Persons { get; set; }

    }
}