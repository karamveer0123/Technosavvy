using System.Collections.Concurrent;

namespace TechnoApp.Ext.Web.UI.Service
{
    internal class SrvCurrencyPriceHUB : SvcBase
    {
        int UpdateEvery = 10;//Seconds
         static WatchResult wResult = new WatchResult();
        public static List<string> GetAllCurrenciesName()
        {
            return wResult.Rates.Keys.ToList();
        }
        public static List<TokenPrice> GetAllCurrencies()
        {
            return wResult.Rates.Values.ToList();
        }
        public static TokenPrice GetCurrency(string code)
        {
            var c= wResult.Rates.Values.FirstOrDefault(x => x.TokenName.ToLower() == code.ToLower());
            c = c ?? new TokenPrice();//default 0
            return c;
        }
        protected override async Task DoStart()
        {
            if (wResult.LastUpdatedOn.AddSeconds(UpdateEvery) <= DateTime.UtcNow)
            {
                var lst = await GetCurrencyUpdate();
                ConcurrentDictionary<string, TokenPrice> lCopy = new ConcurrentDictionary<string, TokenPrice>();
                foreach (var coin in lst)
                {
                    lCopy.TryAdd(coin.TokenName, coin);
                }
                wResult.Rates = lCopy;
                wResult.LastUpdatedOn = DateTime.UtcNow;
            }
            await Task.CompletedTask;
        }

    }
}

