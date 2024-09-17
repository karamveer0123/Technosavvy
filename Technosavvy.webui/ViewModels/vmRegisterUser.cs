namespace TechnoApp.Ext.Web.UI.ViewModels
{
    public class vmRegisterUser : vmBase
    {
        public vmRegisterUserName vmRegUserName { get; set; }
        public vmRegisterUserNameOTP EmailOTP { get; set; }
        public vmRegisterUserPassword vmPassword { get; set; }
        public bool OTPSendStatus { get; set; }
        public bool OTPVerifyStatus { get; set; }
        public bool RegistrationStatus { get; set; }
        public bool ForgetPasswordStatus { get; set; }
        public string ErrorMessage { get; set; }
    }
    
}
