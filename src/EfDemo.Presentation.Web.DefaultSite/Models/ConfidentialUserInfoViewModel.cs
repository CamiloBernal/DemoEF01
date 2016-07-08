using System.ComponentModel.DataAnnotations;
using EfDemo.Core.Model;

namespace EfDemo.Presentation.Web.DefaultSite.Models
{
    public class ConfidentialUserInfoViewModel
    {
        [Required]
        [Display(Name = @"Name:")]
        public string Name { get; set; }

        [Required]
        [Display(Name = @"Last name:")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = @"Age:")]
        public int Age { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = @"Email:")]
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