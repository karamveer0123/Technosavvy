using TechnoApp.Ext.Web.UI.Service;
using TechnoApp.Ext.Web.UI.Model;
using System.Text;

namespace TechnoApp.Ext.Web.UI.Manager;

public class DashboardManager : MaintenanceSvc
{
    //Seek aggregating Report of Data to Display
    //Allow Navigation to Inner Pages for Action

    internal async Task<vmWalletSummery> GetMyWalletSummery()
    {
        var vm = new vmWalletSummery();
        var spotwallet = GetMyWalletSummery(myUS.SpotWalletId);
        var fundwallet = GetMyWalletSummery(myUS.FundingWalletId);
        var ernwallet = GetMyWalletSummery(myUS.EarnWalletId);

        await Task.WhenAll(spotwallet, fundwallet, ernwallet);

        var sw = CheckAndAllocateDefault(spotwallet.Result);
        var fw = CheckAndAllocateDefault(fundwallet.Result);
        var ew = CheckAndAllocateDefault(ernwallet.Result);

        await Task.WhenAll(Task.WhenAll(sw, fw, ew));

        vm.SpotTokens = await DashBoardDisplayInfo(sw.Result);
        vm.FundTokens = await DashBoardDisplayInfo(fw.Result);
        vm.EarnTokens = await DashBoardDisplayInfo(ew.Result);
        vm.TotalBalance = ((vm.SpotTokens.Item3 + vm.FundTokens.Item3 + vm.EarnTokens.Item3) * 100) / 100;
        return vm;
    }
    private async Task<Tuple<string, string, double>> DashBoardDisplayInfo(mWalletSummery m)
    {
        var lst = m.Tokens.Select(x => x.Code).ToList();
        double dValue = 0;
        foreach (var t in m.Tokens)
        {
            dValue += SrvCoinPriceHUB.GetCoin(t.Code).Price * t.Amount;
        }
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < lst.Count; i++)
        {
            if (i >= 3)
            {
                break;
            }
            if (i > 0)
                sb.Append(", ");

            sb.Append($"{lst[i]}");
        }
        sb.Append(" & Others");

        return Tuple.Create(sb.ToString(), myCookieState.Currency, dValue);
    }
    private async Task<mWalletSummery> CheckAndAllocateDefault(mWalletSummery m)
    {
        if (m.Tokens is null || m.Tokens.Count <= 0)
            m.Tokens = await GetCoinsToShow(5);
        if (m.Fiats is null || m.Fiats.Count <= 0)
            m.Fiats = await GetFiatToShow(5);
        return m;
    }
    private async Task<List<mWalletCoin>> GetCoinsToShow(int count)
    {
        var retval = new List<mWalletCoin>();
        var lst = await GetAllActiveCryptoTokens();
        if (lst is null || lst.Count <= 0) return retval;
        lst = lst.Count > count ? lst.Take(count).ToList() : lst;
        lst.ForEach(x =>
        {
            retval.Add(new mWalletCoin
            {
                Amount = 0,
                Code = x.Code,
                CoinId = x.TokenId,
                IsFiatRepresentative = x.IsFiatRepresentative,
                FullName = x.FullName,
                LastTransactionOn = DateTime.UtcNow,
                ShortName = x.ShortName
            });
        });

        return retval;
    }
    private async Task<List<mWalletCoin>> GetFiatToShow(int count)
    {
        var retval = new List<mWalletCoin>();
        var lst = await GetAllActiveFiatTokens();
        if (lst is null || lst.Count <= 0) return retval;
        lst = lst.Count > count ? lst.Take(count).ToList() : lst;

        lst.ForEach(x =>
        {
            retval.Add(new mWalletCoin
            {
                Amount = 0,
                Code = x.Code,
                IsFiatRepresentative = x.IsFiatRepresentative,
                CoinId = x.TokenId,
                FullName = x.FullName,
                LastTransactionOn = DateTime.UtcNow,
                ShortName = x.ShortName
            });
        });

        return retval;
    }
    internal async Task<vmWalletTransactions> GetTransactionSummery()
    {
        vmWalletTransactions vm = new vmWalletTransactions();
        var spotwallet = await GetMyWalletTransactions(myUS.SpotWalletId);
        var fundwallet = await GetMyWalletTransactions(myUS.FundingWalletId);
        var ernwallet = await GetMyWalletTransactions(myUS.EarnWalletId);
        vm.WalletTransactions.AddRange(spotwallet);
        vm.WalletTransactions.AddRange(fundwallet);
        vm.WalletTransactions.AddRange(ernwallet);
        return vm;
    }

}
