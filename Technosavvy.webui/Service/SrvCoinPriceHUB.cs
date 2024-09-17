using NuGet.Protocol;
using System.Collections.Concurrent;

namespace TechnoApp.Ext.Web.UI.Service
{
    internal class Srv24HrTradePriceHUB : SvcBase
    {

    }
    internal class SrvCoinPriceHUB : SvcBase
    {
        int UpdateEvery = 10;//Seconds
        static WatchResult wResult = new WatchResult();

        public static List<string> GetAllCoinName()
        {
            return wResult.Rates.Keys.ToList();
        }
        public static List<TokenPrice> GetAllCoin()
        {
            var isThere = (wResult.Rates.Values.ToList().Any(x => double.IsNaN(x.Price)));
            if (isThere)
                SyncAll();
            return wResult.Rates.Values.ToList();
        }
        static void SyncAll()
        {
            var all = wResult.Rates.Values.ToList();
            var lst = all.Where(x => !x.TokenName.ToUpper().EndsWith("USDT")).ToList();
            foreach (var t in lst)
            {
                if (double.IsNaN(t.Price))
                {
                    var o = all.FirstOrDefault(x => x.TokenName.ToUpper() == $"{t.TokenName}USDT");
                    if (o != null)
                    {
                        t.Price = o.Price;
                        wResult.Rates.AddOrUpdate(t.TokenName, o, (t, k) => o);
                    }
                }
            }
        }

        public static TokenPrice GetCoin(string code)
        {
            var ret = wResult.Rates.Values.FirstOrDefault(x => x.TokenName.ToLower() == code.ToLower());
            if (ret == null)
            {
                Console2.WriteLine_RED($"ERROR:Token:{code} doesn't have any value Recorded in the Service, Zero(0) returned..at:{DateTime.UtcNow}");
                Console2.WriteLine_White($"Info:Token value List is as follow:{GetAllCoin().ToJson()}..at:{DateTime.UtcNow}");

            }
            if (ret != null && double.IsNaN(ret.Price))
            {
                var o = wResult.Rates.Values.FirstOrDefault(x => x.TokenName.ToLower() == $"{code}USDT".ToLower());
                if (o != null)
                {
                    ret = ret ?? new TokenPrice();//default 0
                    ret.Price = o.Price;
                }
            }

            ret = ret ?? new TokenPrice();//default 0
            return ret;
        }

        protected override async Task DoStart()
        {
            if (wResult.LastUpdatedOn.AddSeconds(UpdateEvery) <= DateTime.UtcNow)
            {
                var lst = await GetCoinUpdate();
                ConcurrentDictionary<string, TokenPrice> lCopy = new ConcurrentDictionary<string, TokenPrice>();
                foreach (var coin in lst)
                {
                    if (double.IsNaN(coin.Price) && !coin.TokenName.ToUpper().EndsWith("USDT"))
                    {
                        var o = lst.FirstOrDefault(x => x.TokenName.ToUpper() == $"{coin.TokenName}USDT".ToUpper());
                        if (o != null)
                        {
                            coin.Price = o.Price;
                        }
                    }
                    lCopy.TryAdd(coin.TokenName, coin);
                }
                wResult.Rates = lCopy;
                wResult.LastUpdatedOn = DateTime.UtcNow;
            }
            await Task.CompletedTask;
        }

    }

}

