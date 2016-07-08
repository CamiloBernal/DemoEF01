using System.ComponentModel.DataAnnotations;
using EfDemo.Core.Model;

namespace EfDemo.Presentation.Web.DefaultSite.Models
{
    public class ConfidentialUserInfoViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public static explicit operator ConfidentialUserInfo(ConfidentialUserInfoViewModel v)
            => new ConfidentialUserInfo
            {
                Email = v.Email,
                Name = v.Name,
                Age = v.Age,
                LastName = v.LastName
            };
        
    }
}