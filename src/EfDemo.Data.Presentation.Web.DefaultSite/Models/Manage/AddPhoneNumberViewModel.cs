using System.ComponentModel.DataAnnotations;

namespace EfDemo.Data.Presentation.Web.DefaultSite.Models.Manage
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }
}