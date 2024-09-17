using Microsoft.AspNetCore.Mvc;
using TechnoApp.Ext.Web.UI.Model;
using TechnoApp.Ext.Web.UI.Service;

namespace TechnoApp.Ext.Web.UI.Manager
{
    public class DepositManager : MaintenanceSvc
    {
        public new List<mToken> GetAllActiveTokens()
        {
            return (base.GetAllActiveTokens().Result);
        }
        public List<mFiatCurrency> GetAllActiveFiat()
        {
            return (base.GetActiveFiat(1000).Result);
        }
        public List<mSupportedNetwork> GetAllSupportedNetwork()
        {
            return (base.GetAllSupportedNetwork().Result);
        }
        public async Task<vmDeposit> PoplateINRAndMyPayOptions(vmDeposit vm)
        {
            vm.IB ??= new InfoBag();
            vm.IB.INRTechnoAppOption ??= new mINRDepositOption();
            vm.IB.INRTechnoAppOption.BankDeposits = await GetINRBankDetailsOfTechnoApp();
            vm.IB.INRTechnoAppOption.selectedBankDeposits ??= vm.IB.INRTechnoAppOption.BankDeposits.FirstOrDefault(x=>x.IsDepositAllowed && x.DepositStartDate<=DateTime.UtcNow);
          
            vm.IB.INRTechnoAppOption.selectedBankDeposits = vm.IB.INRTechnoAppOption.BankDeposits.FirstOrDefault(x=>x.AccountNumber == vm.IB.INRTechnoAppOption.selectedBankDeposits.AccountNumber);

            vm.IB.INRTechnoAppOption.UPI = await GetINRUPIDetailsOfTechnoApp();
            vm.IB.INRTechnoAppOption.selectedUPI = vm.IB.INRTechnoAppOption.UPI.FirstOrDefault(x => x.IsDepositAllowed && x.DepositStartDate <= DateTime.UtcNow);

            var p = _appSessionManager.ExtSession.UserSession.UserAccount.Profile;

            vm.IB.INRUserOptions ??= new mPaymentDeposits();
            vm.IB.INRUserOptions.BankDeposits = await GetmyINRBankDeposit(p.ProfileId);
            vm.IB.INRUserOptions.selBankDeposits ??= vm.IB.INRUserOptions.BankDeposits.FirstOrDefault();
            vm.IB.INRUserOptions.selBankDeposits = vm.IB.INRUserOptions.BankDeposits.FirstOrDefault(x => x.AccountNumber == vm.IB.INRUserOptions.selBankDeposits.AccountNumber);

            vm.IB.INRUserOptions.UPI = await GetmyINRUPISetup(p.ProfileId);
            vm.IB.INRUserOptions.selUPI ??= vm.IB.INRUserOptions.UPI.FirstOrDefault();
            vm.IB.INRUserOptions.selUPI = vm.IB.INRUserOptions.UPI.FirstOrDefault(x => x.UPIid == vm.IB.INRUserOptions.selUPI.UPIid);
            return vm;
        }
        
    }
}
