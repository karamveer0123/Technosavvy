using TechnoApp.Ext.Web.UI.Service;

namespace TechnoApp.Ext.Web.UI.Manager
{
    public class RewardManager : MaintenanceSvc
    {
        internal async Task<vmRewardCenter> LoadvmRewardCenter(vmRewardCenter vm)
        {
            await _appSessionManager.ExtSession.LoadSession();
            if (myUS?.UserAccount == null) return vm;
            var code = myUS.UserAccount.RefCodes.myCommunity;
            var link = $"{_http.Request.Scheme}://{_http.Request.Host}/?uRef={code}";
            vm.RefLink = link;
            vm.myReferrals = await GetReferredUsersRewardByDateGroup(code);
            vm.myRewards= await GetMyRewardsRecords();
            vm.Reward = await GetReferredRewardStat(code);
            var v = SrvCoinPriceHUB.GetCoin("TechnoSavvy");
            if (v != null)
                vm.TechnoSavvyUSDTVal = v.Price;
            if (double.IsNaN(vm.TechnoSavvyUSDTVal) || vm.TechnoSavvyUSDTVal < 0)
                vm.TechnoSavvyUSDTVal = 0;
            return vm;
        }
    }
}
