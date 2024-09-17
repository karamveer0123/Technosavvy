using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TechnoApp.Ext.Web.UI.ViewModels
{
    public class vmUser : vmBase
    {
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Email id is required")]
        [Remote("IsExitUserName", "Home", HttpMethod = "POST", ErrorMessage = "User Name already exit")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Captch is required")]
        public string Captch { get; set; }
        public string OTP { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long and a max of 15 characters.", MinimumLength = 9)]
        [RegularExpression("^(?=.*[!@#$%^&*-])(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{0,20}$", ErrorMessage = "Include 1 upper case letter, 1 lower case letter, 1 special character, and 1 numeric value.")]
        public string? Password { get; set; }
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string RefCode { get; set; }
    }
}
