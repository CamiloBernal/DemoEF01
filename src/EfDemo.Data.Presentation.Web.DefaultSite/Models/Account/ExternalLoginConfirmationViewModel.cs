using System.ComponentModel.DataAnnotations;

namespace EfDemo.Data.Presentation.Web.DefaultSite.Models.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}