using System.Collections.Generic;

namespace EfDemo.Core.Model
{
    public class Role
    {
        public Role()
        {
            Users = new List<UserRole>();
        }

        public virtual ICollection<UserRole> Users { get; private set; }

        public long Id { get; set; }

        public string Name { get; set; }
    }
}