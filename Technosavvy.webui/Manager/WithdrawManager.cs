using TechnoApp.Ext.Web.UI.Service;

namespace TechnoApp.Ext.Web.UI.Manager
{
    public class WithdrawManager : MaintenanceSvc {
        public async Task<vmWithdraw> PoplateINRAndMyPayOptions(vmWithdraw vm)
        {
            vm.IB ??= new InfoBag();
            vm.IB.INRTechnoAppOption ??= new mINRDepositOption();
            vm.IB.INRTechnoAppOption.BankDeposits = await GetINRBankDetailsOfTechnoApp();
            vm.IB.INRTechnoAppOption.selectedBankDeposits = vm.IB.INRTechnoAppOption.BankDeposits.FirstOrDefault(x => x.IsDepositAllowed && x.DepositStartDate <= DateTime.UtcNow);

            vm.IB.INRTechnoAppOption.UPI = await GetINRUPIDetailsOfTechnoApp();
            vm.IB.INRTechnoAppOption.selectedUPI = vm.IB.INRTechnoAppOption.UPI.FirstOrDefault(x => x.IsDepositAllowed && x.DepositStartDate <= DateTime.UtcNow);

            var p = _appSessionManager.ExtSession.UserSession.UserAccount.Profile;

            vm.IB.INRUserOptions ??= new mPaymentDeposits();
            vm.IB.INRUserOptions.BankDeposits = await GetmyINRBankDeposit(p.ProfileId);
            vm.IB.INRUserOptions.UPI = await GetmyINRUPISetup(p.ProfileId);
            return vm;
        }
    }
}
