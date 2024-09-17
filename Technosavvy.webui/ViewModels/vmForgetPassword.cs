using System.ComponentModel.DataAnnotations;

namespace TechnoApp.Ext.Web.UI.ViewModels
{
    public class vmForgetPassword : vmBase
    {
        [Required(ErrorMessage = "Email id is required")]
        public string Email { get; set; }
    }
}
