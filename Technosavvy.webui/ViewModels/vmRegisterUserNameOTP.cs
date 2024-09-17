using System.ComponentModel.DataAnnotations;

namespace TechnoApp.Ext.Web.UI.ViewModels
{
    public class vmRegisterUserNameOTP
    {
        [StringLength(6)]
        public string OTP { get; set; }
    }
    
}
