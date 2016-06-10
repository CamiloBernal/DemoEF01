using System.Collections.Generic;

namespace Domain
{
    public class Company
    {
        public string Address { get; set; }
        public int CompanyId { get; set; }
        public virtual Country Country { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Person> Persons { get; set; } = new HashSet<Person>();
    }
}