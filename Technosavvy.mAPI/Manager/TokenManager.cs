using NavExM.Int.Maintenance.APIs.Data.Entity.KYC;
using NavExM.Int.Maintenance.APIs.Services;

namespace NavExM.Int.Maintenance.APIs.Manager
{
    internal class TokenManager : ManagerBase
    {

        internal List<mToken> GetAllTokensOfNetwork(Guid networkId)
        {
            var all = dbctx.Token
                 .Include(x => x.SupportedCoin)
                 .ThenInclude(x => x.RelatedNetwork)
                 .Include(x => x.AllowedCountries)
                 .ThenInclude(x => x.Country)
                 .Where(x =>
                 x.SupportedCoin.Any(z => z.RelatedNetworkId == networkId)
                 && x.DeletedOn.HasValue == false || (x.DeletedOn.Value.Date < DateTime.UtcNow.Date)
                 ).ToList();
            return all.ToModel();
        }
        internal List<mToken> GetAllInActiveTokensModel()
        {
            return GetAllInActiveTokens().ToModel();
        }
        internal List<mToken> GetAllActiveTokensModel()
        {
            return GetAllActiveTokens().ToModel();
        }
        internal mToken GetActiveTokenModel(Guid id)
        {
            return GetSpecificToken(id).ToModel();
        }
        internal mToken GetActiveTokenModel(string code)
        {
            return GetSpecificToken(code).ToModel();
        }
        internal List<mToken> GetActiveTokensModel(int count)
        {
            return GetAllActiveTokens(count).ToModel();
        }
        internal List<mTokenNetworkFee> GetAllTokensNetWorkFee()
        {
            return dbctx.TokenNetworkFee
                  .Include(x => x.SupportedNetwork)
                  .Include(x => x.Token)
                  .Where(x => x.DeletedOn.HasValue == false).ToList().ToModel();

        }
        internal mTokenNetworkFee GetTokensNetWorkFee(Guid tokenId, Guid netId)
        {
            return dbctx.TokenNetworkFee
                  .Include(x => x.SupportedNetwork)
                  .Include(x => x.Token)
                  .FirstOrDefault(x => x.DeletedOn.HasValue == false
                  && x.TokenId == tokenId
                  && x.SupportedNetworkId == netId
                  ).ToModel();

        }
        private List<eToken> GetAllActiveTokens(int count = 0)
        {
            if (count > 0)
            {
                return dbctx.Token
                      .Include(x => x.SupportedCoin)
                      .ThenInclude(x => x.RelatedNetwork)
                      .Include(x => x.AllowedCountries)
                 .ThenInclude(x => x.Country)
                      .Where(x => x.DeletedOn.HasValue == false).Take(count).ToList();
            }
            else
            {
                return dbctx.Token
                        .Include(x => x.SupportedCoin)
                        .ThenInclude(x => x.RelatedNetwork)
                        .Include(x => x.AllowedCountries)
                 .ThenInclude(x => x.Country)
                        .Where(x => x.DeletedOn.HasValue == false).ToList();
            }

        }
        internal List<eToken> GetAllActiveFiatTokens(int count = 0)
        {
            if (count > 0)
            {
                return dbctx.Token
                      .Include(x => x.SupportedCoin)
                      .ThenInclude(x => x.RelatedNetwork)
                      .Include(x => x.AllowedCountries)
                 .ThenInclude(x => x.Country)
                      .Where(x => x.DeletedOn.HasValue == false && x.IsFiatRepresentative).OrderBy(x => x.Code).Take(count).ToList();
            }
            else
            {
                return dbctx.Token
                        .Include(x => x.SupportedCoin)
                        .ThenInclude(x => x.RelatedNetwork)
                        .Include(x => x.AllowedCountries)
                 .ThenInclude(x => x.Country)
                        .Where(x => x.DeletedOn.HasValue == false && x.IsFiatRepresentative).OrderBy(x => x.Code).ToList();
            }

        }
        internal eToken? GetSpecificToken(Guid id)
        {
            return dbctx.Token
                .Include(x => x.SupportedCoin)
                  .ThenInclude(x => x.RelatedNetwork)
                  .Include(x => x.AllowedCountries)
                 .ThenInclude(x => x.Country)
                .FirstOrDefault(x => x.TokenId == id && x.DeletedOn.HasValue == false);
        }
        internal bool UpdateTokenWatchListCount(string code, int c)
        {
            return dbctx.Token
               .Where(x => x.Code.ToUpper() == code.ToUpper() && x.DeletedOn.HasValue == false).ExecuteUpdate(p => p
                  .SetProperty(a => a.WatchList, a => a.WatchList + c)) > 0;
        }
        internal bool UpdateTokenFavListCount(string code, int c)
        {
            return dbctx.Token
               .Where(x => x.Code.ToUpper() == code.ToUpper() && x.DeletedOn.HasValue == false).ExecuteUpdate(p => p
                  .SetProperty(a => a.WatchList, a => a.WatchList + c)) > 0;
        }
        internal eToken? GetSpecificToken(string code)
        {
            return dbctx.Token
                .Include(x => x.SupportedCoin)
                  .ThenInclude(x => x.RelatedNetwork)
                  .Include(x => x.AllowedCountries)
                 .ThenInclude(x => x.Country)
                .FirstOrDefault(x => x.Code.ToLower() == code.ToLower() && x.DeletedOn.HasValue == false);
        }
        private List<eToken> GetAllInActiveTokens()
        {
            return dbctx.Token
                  .Include(x => x.SupportedCoin)
                  .ThenInclude(x => x.RelatedNetwork)
                  .Include(x => x.AllowedCountries)
                 .ThenInclude(x => x.Country)
                  .Where(x => x.DeletedOn.HasValue == true).ToList();

        }
        internal List<eToken> GetAllTokens()
        {
            return dbctx.Token
                  .Include(x => x.SupportedCoin)
                  .ThenInclude(x => x.RelatedNetwork)
                  .Include(x => x.AllowedCountries)
                 .ThenInclude(x => x.Country)

                  .ToList();
        }
        internal void GetEstimatedValueIn(string bCode, string qCode, double Amt, out double Rate, out double MinTrade)
        {
            var cm = GetCDManager();
            double retval = 0;
            Rate = 0;
            MinTrade = 0;
            double Mply = 1;
            var m = SrvCoinWatch.GetAllCoins();
            var d = m.FirstOrDefault(x => x.TokenName.ToLower() == $"{bCode}{qCode}".ToLower());

            if (d != null)
            {
                Rate = GetTokenMarketEstimatedBuyPrice(d.TokenName, Amt);
                Rate += Rate * cm.ConvertCharge($"{bCode}{qCode}").GetAwaiter().GetResult();

                MinTrade = cm.MinimumTradeUSDTValue() / Rate;
                return;
            }
            //if (qCode.ToUpper() != "USDT")
            //{
            //    var _ply = m.FirstOrDefault(x => x.TokenName.ToLower() == $"{qCode}USDT".ToLower());
            //    if (_ply == null)
            //        throw new ApplicationException($"{qCode} doesn't have an active USDT Market..");
            //    Mply = _ply.Price;
            //}
            if (bCode.ToUpper() == "USDT")
            {
                d = m.FirstOrDefault(x => x.TokenName.ToLower() == $"{qCode}{bCode}".ToLower());
                if (d != null)
                {
                    Rate = GetTokenMarketEstimatedSellPrice(d.TokenName, Amt);
                    Rate += Rate * cm.ConvertCharge($"{bCode}{qCode}").GetAwaiter().GetResult();

                    MinTrade = cm.MinimumTradeUSDTValue() / Rate;
                    return;
                }
            }
            //Now we are in double Trade Zone
            //-To USDT
            // -From USDT
            var A = m.FirstOrDefault(x => x.TokenName.ToLower() == $"USDT{qCode}".ToLower());
            var A_r = m.FirstOrDefault(x => x.TokenName.ToLower() == $"{qCode}USDT".ToLower());

            var B = m.FirstOrDefault(x => x.TokenName.ToLower() == $"{bCode}USDT".ToLower());
            var B_r = m.FirstOrDefault(x => x.TokenName.ToLower() == $"USDT{bCode}".ToLower());

            if (A != null && B != null)
            {
                var Two = GetTokenMarketEstimatedBuyPrice(B.TokenName, Amt);
                var one = GetTokenMarketEstimatedBuyPrice(A.TokenName, Amt * Two);
                Rate = one * Two;
                Rate += Rate * cm.ConvertCharge($"{bCode}{qCode}").GetAwaiter().GetResult();

                MinTrade =cm.MinimumTradeUSDTValue() / Two;
                return;
            }
            if (A_r != null && B != null)
            {
                var Two = GetTokenMarketEstimatedBuyPrice(B.TokenName, Amt);
                Rate = GetTokenMarketEstimatedSellTotalValueUnitsCount(A_r.TokenName, Two);
                Rate += Rate * cm.ConvertCharge($"{bCode}{qCode}").GetAwaiter().GetResult();
                Two += Two *cm. ConvertCharge($"{bCode}{qCode}").GetAwaiter().GetResult();
                //Base Minimum Trade value USDT=10
                MinTrade = (cm.MinimumTradeUSDTValue() / Two);
                //Quote Minimum Trade value USDT=10
                //MinTrade = (MinimumTradeUSDTValue().GetAwaiter().GetResult() / Two)*Rate;

                return;
                // = !(double.IsNaN(one) && double.IsNaN(Two)) ? (Amt * Two) / one : double.NaN;

            }
            return;
        }
        internal double GetTokenMarketEstimatedBuyPrice(string mCode, double Amt)
        {
            double tr = 0;
            double vol = 0;
            try
            {
                double lp = 0;
                var lst = new List<Data.OrderCheck.eSellOrder>();
                using (var db = _dbMktctx(mCode, false))
                {
                    if (db == null) return 0;
                    var tl = db.Market_SellOrders.Where(x => (x.ReserveId == null || x.ReserveId.Equals("")) && x.CurrentVolume > 0).OrderBy(x => x.Price).Sum(x => x.CurrentVolume);
                    if (tl < Amt) return double.NaN;//Total Liquidity
                    do
                    {
                        var ord = db.Market_SellOrders.Where(x => (x.ReserveId == null || x.ReserveId.Equals("")) && x.CurrentVolume > 0).OrderBy(x => x.Price).ThenBy(x => x.Price).Skip(lst.Count).Take(100).ToList();
                        lst.AddRange(ord);
                    }
                    while (lst.Sum(x => x.CurrentVolume) < Amt);

                    foreach (var o in lst.OrderBy(x => x.Price).ToList())
                    {
                        if (vol < Amt)
                        {
                            tr += o.CurrentVolume * o.Price;
                            vol += o.CurrentVolume;
                            lp = o.Price;//lastPrice
                        }
                        else
                            return tr / vol;

                    }
                    return tr / vol;
                }

            }
            catch (Exception ex)
            {

            }
            return 0;
        }
        internal double GetTokenMarketEstimatedSellTotalValueUnitsCount(string mCode, double Amt)
        {
            double tr = 0;
            double vol = 0;
            try
            {
                double lp = 0;
                var lst = new List<Data.OrderCheck.eBuyOrder>();
                var ord = new List<Data.OrderCheck.eBuyOrder>();
                using (var db = _dbMktctx(mCode, false))
                {
                    if (db == null) return 0;
                    var tl = db.Market_BuyOrders.Where(x => (x.ReserveId == null || x.ReserveId.Equals("")) && x.CurrentVolume > 0).OrderBy(x => x.Price).Sum(x => x.CurrentVolume*x.Price);
                    if (tl < Amt) return double.NaN;//Total Liquidity
                    do
                    {
                        ord = db.Market_BuyOrders.Where(x => (x.ReserveId == null || x.ReserveId.Equals("")) && x.CurrentVolume > 0).OrderBy(x => x.Price).ThenBy(x => x.Price).Skip(lst.Count).Take(100).ToList();
                        lst.AddRange(ord);
                    }
                    while ((lst.Sum(x => x.CurrentVolume * x.Price) < Amt) || ord.Count <= 0);

                    foreach (var o in lst.OrderBy(x => x.Price).ToList())
                    {
                        if (tr < Amt)
                        {
                            var tr2 = o.CurrentVolume * o.Price;
                            if ((tr + tr2) > Amt)
                            {
                                var gap = (tr + tr2) - Amt;
                                vol = vol + (o.CurrentVolume - (gap / o.Price));
                                return vol;
                            }
                            else
                            {
                                tr += tr2;
                                vol += o.CurrentVolume;
                                lp = o.Price;//lastPrice
                            }
                        }
                        else
                            return vol;

                    }
                    return vol;
                }

            }
            catch (Exception ex)
            {

            }
            return 0;
        }
        internal double GetTokenMarketEstimatedSellPrice(string mCode, double Amt)
        {
            double tr = 0;
            double vol = 0;
            try
            {
                double lp = 0;
                var lst = new List<Data.OrderCheck.eBuyOrder>();
                using (var db = _dbMktctx(mCode, false))
                {
                    if (db == null) return 0;
                    var tl = db.Market_BuyOrders.Where(x => (x.ReserveId == null || x.ReserveId.Equals("")) && x.CurrentVolume > 0).OrderBy(x => x.Price).Sum(x => x.CurrentVolume);
                    if (tl < Amt) return double.NaN;//Total Liquidity
                    do
                    {
                        var ord = db.Market_BuyOrders.Where(x => (x.ReserveId == null || x.ReserveId.Equals("")) && x.CurrentVolume > 0).OrderBy(x => x.Price).ThenBy(x => x.Price).Skip(lst.Count).Take(100).ToList();
                        lst.AddRange(ord);
                    }
                    while (lst.Sum(x => x.CurrentVolume) < Amt);

                    foreach (var o in lst.OrderBy(x => x.Price).ToList())
                    {
                        if (vol < Amt)
                        {
                            tr += o.CurrentVolume * o.Price;
                            vol += o.CurrentVolume;
                            lp = o.Price;//lastPrice
                        }
                        else
                            return tr / vol;

                    }
                    return tr / vol;
                }

            }
            catch (Exception ex)
            {

            }
            return 0;
        }


        private List<eCountry> GetCountriesEntity(List<mCountry> m)
        {
            return dbctx.Country.ToList().Where(x => m.Any(z => z.CountryId == x.CountryId)).ToList();
        }
        private Data.OrderCheck.MktBookContext _dbMktctx(string MarketCode, bool shouldDisplay = true)
        {
            try
            {
                var o = new DbContextOptionsBuilder<Data.OrderCheck.MktBookContext>();
                o = o.UseSqlServer(ConfigEx.Config.GetConnectionString(MarketCode));
                o.EnableSensitiveDataLogging();
                o.EnableThreadSafetyChecks();
                return new Data.OrderCheck.MktBookContext(o.Options);
            }
            catch (Exception ex)
            {
                if (shouldDisplay)//we will casually use this to get connection 
                    SrvPlugIn.LogErrorG($"{AppConfigBase.RegistryToken!.AppId}- in Market:{MarketCode} T:{Thread.CurrentThread.ManagedThreadId} in Connection for caused error in SQL Connection/Object {ex.GetDeepMsg()}");
            }
            return null;
        }
        private CommonDataManager GetCDManager()
        {
            var tm = new CommonDataManager();
            tm.dbctx = dbctx;
            tm._http = _http;
            tm.httpContext = httpContext;
            return tm;
        }
    }

}
