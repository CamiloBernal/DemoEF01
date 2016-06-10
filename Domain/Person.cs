using System.Collections.Generic;

namespace Domain
{
    public class Person
    {
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public int Id { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Phone> Phones { get; set; } = new HashSet<Phone>();
    }
}