using System;
using System.ComponentModel.DataAnnotations;
using EfDemo.Application.Services.Security;
using EfDemo.Core.Model;

namespace EfDemo.Data.Presentation.Web.DefaultSite.Models.Account
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = @"Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = @"The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = @"Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = @"Confirm password")]
        [Compare("Password", ErrorMessage = @"The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        public static explicit operator ApplicationUser(RegisterViewModel source) => new ApplicationUser()
        {
            UserName = source.Email,
            Email = source.Email,
        };        
    }
}