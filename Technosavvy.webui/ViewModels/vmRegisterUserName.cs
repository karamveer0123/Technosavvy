using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TechnoApp.Ext.Web.UI.ViewModels
{
    public class vmRegisterUserName
    {
        [Required(ErrorMessage = "Email id is required")]
        [Remote("IsExitUserName", "Home", HttpMethod = "POST", ErrorMessage = "User Name already exist. Proceed to LogIn")]
        [MaxLength(50,ErrorMessage = "User Name length exceed limit")]
        public string Email { get; set; }
    }
    
}
