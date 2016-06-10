using System.Collections;
using System.Collections.Generic;

namespace Domain
{
    public class Country
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Person> Persons { get; set; } = new HashSet<Person>();
        public virtual ICollection<Phone> Phones { get; set; } = new HashSet<Phone>();
        public virtual ICollection<Company> Companies { get; set; } = new HashSet<Company>();
    }
}