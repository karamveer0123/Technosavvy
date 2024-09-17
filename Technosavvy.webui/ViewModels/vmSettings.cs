using TechnoApp.Ext.Web.UI.Model;
using System.ComponentModel.DataAnnotations;

namespace TechnoApp.Ext.Web.UI.ViewModels
{
    public class vmSettings : vmBase
    {
        public vmSettingsMultiFactorMain MultiFactor { get; set; }
        public vmSettingsSecurity Security { get; set; }
        public vmSettingsDevices Devices { get; set; }
        public vmSettingsTrade Trades { get; set; }
        public vmSettingProfile SettingProfile { get; set; }
    }
    public class vmSettingsMultiFactorMain
    {
        public bool IsMultiFactorEnabled { get; set; }
        public bool IsEmailEnabled { get; set; }
        public bool IsAuthenticator { get; set; }
        public string AuthErrorMsg { get; set; }
        public bool IsSetUPAuthenticator { get; set; }
        public vmSettingsAuthenticator SetUPAuthenticator { get; set; }
        public bool IsMobile { get; set; }
        public string Email { get; set; }
    }
    public class vmSettingsAuthenticator
    {

        public string QRImg { get; set; }
        public string Code { get; set; }
        public string OTP { get; set; }
    }
    public class vmSettingsSecurity { }
    public class vmSettingsDevices { }
    public class vmSettingsTrade { }
    public class vmSettingProfile
    {
        [Required(ErrorMessage = "User Nic Name is required")]
        public string UserNicName { get; set; }
        public Guid UserId { get; set; }
    }
    public class vmSettingsAuth : vmBase
    {
        public string Code { get; set; }
        public string OTP { get; set; }
    }
    public class vm2ndAuth : vmBase
    {
        public string ActionMethod { get; set; }
        public string infoBag { get; set; }
        public string Code { get; set; }
        public string OTP { get; set; }
        public string ErrMsg { get; set; }
    }
    public class vm2ndAuthPaymentMethod : vmBase
    {
        public string ActionMethod { get; set; }
        public Guid id{ get; set; }//
        public string infoBag { get; set; }
        public string AuthCode { get; set; }//Auth Code
        public string EmailOTP { get; set; }
        public string ErrMsg { get; set; }
    }
    public class mPaymentMethodToSave
    {
        public string SelectedPaymentMethod { get; set; }
        public mINRBankDeposit BankDeposit { get; set; }
        public mINRUPI UPI { get; set; }
    }
    public class vmPaymentMethodSetup: vmBase
    {
        public string ActionMethod { get; set; }
        public string FullName { get; set; }
        public List<string> ValidPaymentMethods { get; set; }
        public bool IsValidPaymentMethods { get; set; }
        public List<mToken3> FiatTokenList { get; set; }
        public mToken FiatToken { get; set; }
        public List<string> PaymentMethod { get; set; }
        public  string SelectedPaymentMethod { get; set; }
        public vmBankDeposit BankDeposit { get; set; }
        public vmUPI UPI { get; set; }
    }
    public class vmUPI {
        public Guid Id { get; set; }
        public string UPIId { get; set; }
        public IFormFile QRCode { get; set; }

    }
    public class vmBankDeposit
    {
        public Guid Id { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string BranchName { get; set; }
        public IFormFile VoidCheque { get; set; }
    }
    public class vmPaymentDeposits : vmBase
    {
        public mINRBankDeposit selBankDeposits { get; set; }
        public List<mINRBankDeposit> BankDeposits { get; set; }
        public mINRUPI selUPI { get; set; }
        public List<mINRUPI> UPI { get; set; }
    }
    public class mPaymentDeposits 
    {
        /// <summary>
        /// Selected User Bank Account for INR Deposit
        /// </summary>
        public mINRBankDeposit selBankDeposits { get; set; }
        /// <summary>
        /// User Bank Account for INR Deposit
        /// </summary>
        public List<mINRBankDeposit> BankDeposits { get; set; }
        /// <summary>
        /// Selected User UPI Account for INR Deposit
        /// </summary>
        public mINRUPI selUPI { get; set; }
        /// <summary>
        /// User UPI Account for INR Deposit
        /// </summary>
        public List<mINRUPI> UPI { get; set; }
    }
}
