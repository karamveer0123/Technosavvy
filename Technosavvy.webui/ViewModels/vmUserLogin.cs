using System.ComponentModel.DataAnnotations;

namespace TechnoApp.Ext.Web.UI.ViewModels
{
    public class vmAcademyDetail: vmBase
    {
        public string Tab { get; set; }
        public string  Topic { get; set; }
    }
    public class vmUserLogin : vmBase
    {
        [Required(ErrorMessage = "Email is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
    public class vmLog : vmBase
    {
        public string Message { get; set; }
        public string Id { get; } = AppConstant.AppId;
       
    }

}
