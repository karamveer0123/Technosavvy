using TechnoApp.Ext.Web.UI.Service;
using Google.Authenticator;
using System.ComponentModel;
using TechnoApp.Ext.Web.UI.Controllers;
using System.Collections.Generic;

namespace TechnoApp.Ext.Web.UI.Manager
{
    public class SettingsManager : MaintenanceSvc
    {
        //2Factor Enablement
        //Google Authenticator Config
        //Minumum Profile
        //KYC
        //PasswordChange
        //EmailChange
        public static async Task<SettingsManager> Instance(baseController bc)
        {

            await bc.appSessionManager.ExtSession.LoadSession();

            var Mgr = new SettingsManager();
            Mgr._configuration = bc._configuration;
            Mgr._http = bc._accessor.HttpContext;
            Mgr._appSessionManager = bc.appSessionManager;
            Mgr._DataProtector = bc._protector;
            return Mgr;

        }
        internal async Task<bool> ValidateINR(vmPaymentMethodSetup vm)
        {
            var p = _appSessionManager.ExtSession.UserSession.UserAccount.Profile;

            vm.CheckAndThrowNullArgumentException("Technical Error: Null Object Provided");
            vm.FullName.CheckAndThrowNullArgumentException("Profile Name must be available");
            vm.FiatToken.CheckAndThrowNullArgumentException("No Currency Selected");
            vm.FiatToken.Code.CheckAndThrowNullArgumentException("No Currency Selected");
            if (vm.SelectedPaymentMethod == "BankTransfer")
            {
                vm.BankDeposit.AccountNumber.CheckAndThrowNullArgumentException();
                vm.BankDeposit.BankName.CheckAndThrowNullArgumentException();
                vm.BankDeposit.BranchName.CheckAndThrowNullArgumentException();
                vm.BankDeposit.IFSCCode.CheckAndThrowNullArgumentException();
                vm.BankDeposit.VoidCheque.CheckAndThrowNullArgumentException("A Copy of Check must be provided");
                vm.BankDeposit.VoidCheque.CheckAndThrowNullArgumentException(200);
                vm.BankDeposit.VoidCheque.CheckAndThrowNullArgumentException(true);

                var lst = await GetmyINRBankDeposit(p.ProfileId);
                if (lst != null && lst.Any(x => x.AccountNumber.ToUpper() == vm.BankDeposit.AccountNumber.ToUpper()))
                    throw new ApplicationException($"Bank Account with Number:{vm.BankDeposit.AccountNumber} already exist");
            }
            else if (vm.SelectedPaymentMethod == "UPI")
            {
                vm.UPI.UPIId.CheckAndThrowNullArgumentException("UPI Id must be provided");
                vm.UPI.QRCode.CheckAndThrowNullArgumentException("UPI Id must be provided");
                vm.UPI.QRCode.CheckAndThrowNullArgumentException(200);
                vm.UPI.QRCode.CheckAndThrowNullArgumentException(true);

                var lst = await GetmyINRUPISetup(p.ProfileId);
                if (lst != null && lst.Any(x => x.UPIid.ToUpper() == vm.UPI.UPIId.ToUpper()))
                    throw new ApplicationException($"UPI with ID:{vm.UPI.UPIId} already exist");
            }
            else
            {
                vm.SelectedPaymentMethod.CheckAndThrowNullArgumentException();
                vm.SelectedPaymentMethod.ThrowInvalidOperationException("Payment Method Type Must be selected");
            }
            return true;
        }
        internal async Task<bool> SavePaymentMethodINR(mPaymentMethodToSave vm)
        {
            if (vm.SelectedPaymentMethod == "BankTransfer")
            {
                var m = await this.CreateINRBankDepositSetup(vm.BankDeposit);
                return m != null;
            }
            else
            {
                var m = await this.CreateINRUPISetup(vm.UPI);
                return m != null;
            }
        }
        internal async Task<mPaymentMethodToSave> TovmPaymentMethodToSave(vmPaymentMethodSetup vm)
        {
            var retval = new mPaymentMethodToSave();
            var p = _appSessionManager.ExtSession.UserSession.UserAccount.Profile;
            if (vm.SelectedPaymentMethod == "BankTransfer")
            {
                var m = new Model.mINRBankDeposit
                {
                    AccountHolderName = vm.FullName,
                    AccountNumber = vm.BankDeposit.AccountNumber,
                    Bank = vm.BankDeposit.BankName,
                    BranchAddress = vm.BankDeposit.BranchName,
                    IFSCCode = vm.BankDeposit.IFSCCode,
                    ProfileId = p.ProfileId,
                    TokenId = vm.FiatToken.TokenId,
                };

                retval.SelectedPaymentMethod = vm.SelectedPaymentMethod;
                retval.BankDeposit = m;
                return retval;
            }
            else
            {
                var m = new Model.mINRUPI
                {
                    AccountHolderName = vm.FullName,
                    UPIid = vm.UPI.UPIId,
                    QRCode = ImageString(vm.UPI.QRCode),
                    ProfileId = p.ProfileId,
                    TokenId = vm.FiatToken.TokenId,
                };
                retval.SelectedPaymentMethod = vm.SelectedPaymentMethod;
                retval.UPI = m;
                return retval;
            }

        }
        internal async Task<vmPaymentMethodSetup> GetPaymentMethodsSetupVM(vmPaymentMethodSetup vm)
        {
            var tm = GetTokenManager();
            if (vm.FullName.IsNullOrEmpty())
            {
                var p = _appSessionManager.ExtSession.UserSession.UserAccount.Profile;
                vm.FullName = $"{p.FirstName} {p.LastName}";
            }

            var tlst = tm.GetActiveFiatTokens();
            var flst = await tm.GetActiveFiat(1000);
            vm.FiatTokenList = new List<Model.mToken3>();
            tlst.ForEach(x =>
            {
                var f = flst.FirstOrDefault(z => z.Code.ToUpper() == x.Code.ToUpper());
                if (vm.FiatToken != null && vm.FiatToken.Code.IsNOT_NullorEmpty() && vm.FiatToken.Code == x.Code)
                    vm.FiatToken = x;
                vm.FiatTokenList.Add(new Model.mToken3
                {
                    token = x,
                    Symbole = f.Symbole
                });
            });
            vm.ValidPaymentMethods ??= new List<string>();
            vm.ValidPaymentMethods.Add("BankTransfer");
            vm.ValidPaymentMethods.Add("UPI");
            return vm;
        }
        internal async Task<vmSettings> GetSettings()
        {
            var vm = vmFactory.GetvmSettings(_appSessionManager);
            //ToDo: Naveen, Populate VM with Information
            vm.MultiFactor = await GetMultiFactorMain(vm);
            vm.SettingProfile = await GetProfileSetting(vm);
            return vm;
        }
        private async Task<vmSettingProfile> GetProfileSetting(vmSettings vm)
        {
            vmSettingProfile vmSettingProfile = new vmSettingProfile();
            await Task.Run(async () =>
            {
                var profile = await GetProfile(vm._UserId.ToGuid());
                vmSettingProfile.UserNicName = profile.NickName;
                vmSettingProfile.UserId = profile.UserAccountId;
            });
            return vmSettingProfile;
        }

        private async Task<vmSettingsMultiFactorMain> GetMultiFactorMain(vmSettings vm)
        {
            vmSettingsMultiFactorMain VmS = new vmSettingsMultiFactorMain();
            await Task.Run(async () =>
            {
                var user = await GetUserByName(vm._UserName);
                VmS.IsMultiFactorEnabled = user.IsMultiFactor;
                VmS.Email = user.Email.To;
            });
            return VmS;
        }


        internal async Task<vmSettingsMultiFactorMain> IsMultiFactorEnabled(vmSettingsMultiFactorMain setting)
        {
            await Task.Run(() =>
            {
                setting = SetUpAuthenticator(setting);
            });
            return setting;
        }
        private vmSettingsMultiFactorMain SetUpAuthenticator(vmSettingsMultiFactorMain setting)
        {

            string GAuthPrivKey = _configuration.GetSection("GAuthPrivateKey").Value;
            string UserUniqueKey = (setting.Email + GAuthPrivKey);
            if (setting.IsMultiFactorEnabled)
            {

                //Two Factor Authentication Setup
                TwoFactorAuthenticator TwoFacAuth = new TwoFactorAuthenticator();
                var setupInfo = TwoFacAuth.GenerateSetupCode("TechnoApp.com", setting.Email, UserUniqueKey, false, 2);
                setting.SetUPAuthenticator = new vmSettingsAuthenticator();
                setting.SetUPAuthenticator.QRImg = setupInfo.QrCodeSetupImageUrl;
                setting.SetUPAuthenticator.Code = _DataProtector.Protect(UserUniqueKey);
                //ViewBag.SetupCode = setupInfo.ManualEntryKey;

            }
            setting.IsSetUPAuthenticator = true;
            return setting;
        }

        internal bool Verify2Fact(vmSettingsAuthenticator vm)
        {
            return Verify2Fact(vm.Code, vm.OTP);
            //var token = vm.OTP;
            //TwoFactorAuthenticator TwoFacAuth = new TwoFactorAuthenticator();
            //string UserUniqueKey = _DataProtector.Unprotect(vm.Code);
            //bool isValid = TwoFacAuth.ValidateTwoFactorPIN(UserUniqueKey, token);

            //return isValid;
        }
        internal bool Verify2Fact(string protectedCode, string OTP)
        {
            try
            {
                TwoFactorAuthenticator TwoFacAuth = new TwoFactorAuthenticator();
                string UserUniqueKey = _DataProtector.Unprotect(protectedCode);
                bool isValid = TwoFacAuth.ValidateTwoFactorPIN(UserUniqueKey, OTP);
                return isValid;
            }
            catch (Exception ex)
            {

                Console2.WriteLine_RED($"Multifactor Verify2Fact caused error:{ex.GetDeepMsg()}");
            }
            return false;

        }

        internal async void UpdateGCodeFor2Factor(vmSettingsMultiFactorMain vm)
        {
            var res = await UpdateUserGCodeFor2Factor(vm.Email, vm.SetUPAuthenticator.Code);
        }

        internal async Task<bool> IsMultiFactorDisabled(string Email)
        {
            var res = true;
            await DisabledMultiFactor(Email);
            return res;
        }
        internal TokenManager GetTokenManager()
        {
            if (!_appSessionManager.ExtSession.IsLoaded)
                _appSessionManager.ExtSession.LoadSession().GetAwaiter().GetResult();
            var Mgr = new TokenManager();
            Mgr._configuration = _configuration;
            Mgr._http = _http;
            Mgr._appSessionManager = _appSessionManager;
            Mgr._DataProtector = _DataProtector;
            return Mgr;
        }
    }
}
