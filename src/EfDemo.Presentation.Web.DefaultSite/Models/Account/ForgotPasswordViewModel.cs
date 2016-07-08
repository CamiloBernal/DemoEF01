using System.ComponentModel.DataAnnotations;

namespace EfDemo.Presentation.Web.DefaultSite.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}