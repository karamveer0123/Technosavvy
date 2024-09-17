using Microsoft.AspNetCore.Http;
using TechnoApp.Ext.Web.UI.Model;
using TechnoApp.Ext.Web.UI.Extentions;
using TechnoApp.Ext.Web.UI.Service;
using System.Xml.Linq;

namespace TechnoApp.Ext.Web.UI.Manager
{
    /* Trade Manager will offer all functions related to Trade form, Trade Data on that form, Execution status
     */
    public class TradeManager : MaintenanceSvc
    {
        //Order Placement
        //Order Status
        //Market Specific Orders
        //Trade related Award status/ Details
        //other Trade Supported Functions
        public TradeManager()
        {

        }
        #region Place Order
        internal async Task<Tuple<bool, string>> BuildAndPlaceMarketOrder(eOrderSide side, string mCode, string bCode, string bTag, string qCode, string qTag, double Qty, double Price)
        {
            mOrder order = new mOrder()
            {
                PlacedOn = DateTime.UtcNow,
                OrderType = eOrderType.Market,
                Status = eOrderStatus.Placed,
                BaseTokenCodeName = bCode,
                QuoteTokenCodeName = qCode,
                BaseTokenId = Guid.Parse(bTag),
                QuoteTokenId = Guid.Parse(qTag),
                Volume = Qty,
                Price = Price,
                OrderSide = side,
                MarketCode = mCode
            };
            Qty.CheckAndThrowZeroException(ArguName: "Amount");
            Price.CheckAndThrowZeroException(ArguName: "Price");
            var res = await BuildAndPlace(order);
            await APIHub.UpdateClientIfAny(myUS.UserAccount.AccountNumber, mCode);
            return res;

        }
        internal async Task<Tuple<bool, string>> BuildAndPlaceLimitOrder(eOrderSide side, string mCode, string bCode, string bTag, string qCode, string qTag, double Qty, double Price)
        {
            mOrder order = new mOrder()
            {
                PlacedOn = DateTime.UtcNow,
                OrderType = eOrderType.Limit,
                Status = eOrderStatus.Placed,
                BaseTokenCodeName = bCode,
                QuoteTokenCodeName = qCode,
                BaseTokenId = Guid.Parse(bTag),
                QuoteTokenId = Guid.Parse(qTag),
                Volume = Qty,
                Price = Price,
                OrderSide = side,
                MarketCode = mCode
            };
            Qty.CheckAndThrowZeroException(ArguName: "Amount");
            Price.CheckAndThrowZeroException(ArguName: "Price");
            var res = await BuildAndPlace(order);
            await APIHub.UpdateClientIfAny(myUS.UserAccount.AccountNumber, mCode);
            return res;

        }
        internal async Task<Tuple<bool, string>> BuildAndPlaceStopLimitOrder(eOrderSide side, string mCode, string bCode, string bTag, string qCode, string qTag, double Qty, double Price, double stopPrice = 0)
        {
            mOrder order = new mOrder()
            {
                PlacedOn = DateTime.UtcNow,
                OrderType = eOrderType.STOPLimit,
                Status = eOrderStatus.Placed,
                BaseTokenCodeName = bCode,
                QuoteTokenCodeName = qCode,
                BaseTokenId = Guid.Parse(bTag),
                QuoteTokenId = Guid.Parse(qTag),
                Volume = Qty,
                Price = Price,
                OrderSide = side,
                StopPrice = stopPrice,
                MarketCode = mCode
            };
            Qty.CheckAndThrowZeroException(ArguName: "Amount");
            Price.CheckAndThrowZeroException(ArguName: "Price");
            var res = await BuildAndPlace(order);
            await APIHub.UpdateClientIfAny(myUS.UserAccount.AccountNumber, mCode);
            return res;

        }
        internal async Task<Tuple<bool, string>> BuildAndPlaceStopMarketOrder(eOrderSide side, string mCode, string bCode, string bTag, string qCode, string qTag, double Qty, double Price, double stopPrice)
        {
            mOrder order = new mOrder()
            {
                PlacedOn = DateTime.UtcNow,
                OrderType = eOrderType.MarketLimit,
                Status = eOrderStatus.Placed,
                BaseTokenCodeName = bCode,
                QuoteTokenCodeName = qCode,
                BaseTokenId = Guid.Parse(bTag),
                QuoteTokenId = Guid.Parse(qTag),
                Volume = Qty,
                Price = Price,
                StopPrice = stopPrice,
                OrderSide = side,
                MarketCode = mCode
            };
            Qty.CheckAndThrowZeroException(ArguName: "Amount");
            Price.CheckAndThrowZeroException(ArguName: "Price");
            var res = await BuildAndPlace(order);
            await APIHub.UpdateClientIfAny(myUS.UserAccount.AccountNumber, mCode);
            return res;

        }
        #endregion
        internal async Task<vmMarketTrade> GetvmMarketTrade(vmMarketTrade vm)
        {
            _http.Request.Query.TryGetValue("t", out var b);
            _http.Request.Query.TryGetValue("q", out var q);
            if (b.ToString().IsNullOrEmpty() || q.ToString().IsNullOrEmpty())
                return vm;
            //throw new ApplicationException("No Such Active Market..");

            vm.q = q.ToString().IsNOT_NullorEmpty() ? q.ToString() : "USDT";
            vm.b = b.ToString().IsNOT_NullorEmpty() ? b.ToString() : "TechnoSavvy";

            vm.mCode = $"{vm.b}{vm.q}";
            //get preferred View Name
            //vName = mySession.TradeViewName
            //-----

            var mm = GetMarketManager();
            var m = await mm.GetMarketPair(vm.mCode);
            if (m == null || m.BaseToken == null) throw new ApplicationException("No Such Market Available..!");

            //var qF = m.QuoteToken != null ? m.QuoteToken.Tick : m.QuoteCurrency.Tick;
            var qF = FractionCount(m.MinQuoteOrderTick);
            var bF = FractionCount(m.MinBaseOrderTick);

            //qF = FractionCount(qF);
            //var bF = m.BaseToken.Tick;
            //bF = FractionCount(bF);
            vm.bf = bF.ToString();
            vm.qf = qF.ToString();
            vm.q = m.QuoteToken.Code;
            vm.b = m.BaseToken.Code;
            var all = (await mm.GetMarketPair("TechnoSavvyusdt"));
            if (all == null)
            {
                Console2.WriteLine_RED($"TechnoSavvyUSDT market is not available to assess/quote the TechnoSavvy Prices");
            }
            else
            {
                vm.nf = FractionCount(all.BaseToken.Tick).ToString();
                vm.uf = FractionCount(all.QuoteToken.Tick).ToString();
            }

            if (!await IsValidSession()) return vm;
            vName = $"{"tosec"}{_appSessionManager.mySession.vName}";
            vm.IsUserAuth = true;
            var om = await GetMySwapRateAndCashback(vm.mCode, myUS.UserAccount.AccountNumber);
            vm.CBthreashold = om.Item3;//await GetCBthreashold(vm.mCode, myUS.UserAccount.AccountNumber);
            vm.Bid_SwapRate = om.Item1;
            vm.Ask_SwapRate = om.Item2;
            var wm = GetWalletManager();
            var spot = await wm.GetMyWalletSummery(myUS.SpotWalletId);

            var avQ = m.QuoteToken != null ? spot.Tokens.FirstOrDefault(x => x.Code.ToLower() == q) : spot.Fiats.FirstOrDefault(x => x.Code.ToLower() == q.ToString().ToLower());
            var avB = spot.Tokens.FirstOrDefault(x => x.Code.ToLower() == b.ToString().ToLower());
            Console2.WriteLine_White($"|{avQ.ShortName} Balance is:{avQ.Amount}");
            Console2.WriteLine_White($"|{avB.ShortName} Balance is:{avB.Amount}");


            if (avQ != null && avB != null && avQ != avB)
            {
                vm.AvailableBase = avB.Amount;
                vm.AvailableQuote = avQ.Amount;
                vm.BaseTokenId = avB.CoinId.ToString();
                vm.QuoteTokenId = avQ.CoinId.ToString();
                vm.MinQToken = m.MinQuoteOrderTick;
                vm.MinBToken = m.MinBaseOrderTick;
                vm.MinTradeValue = m.MinOrderSizeValueUSD;
                //vm.BaseFraction = FractionCount(bF);
                //vm.QuoteFraction = FractionCount(qF);
                //vm.TradeOrder = new vmTradeOrder()
                //{
                //    AvailableBase = avB.Amount,
                //    AvailableQuote = avQ.Amount,
                //    BaseName = m.BaseToken.Code,
                //    QuoteName = m.QuoteToken.Code,
                //    MCode = $"{b}{q}",
                //    BaseTokenId = avB.CoinId.ToString(),
                //    QuoteTokenId = avQ.CoinId.ToString(),
                //    MinQToken = m.MinQuoteOrderTick,
                //    MinBToken = m.MinBaseOrderTick,
                //    MinTradeValue = m.MinOrderSizeValueUSD,
                //    BaseFraction = FractionCount(bF),
                //    QuoteFraction = FractionCount(qF)
                //};
            }
            //get user token/order/trades/ details for the VM
            //establish user preference for the view from cookie, update vName
            return vm;
        }
        internal static int FractionCount(double d)
        {
            int i = 0;
            if (d > 0 && d < 1)
            {
                while (d <= 1)
                {
                    i++;
                    d *= 10;
                }
            }
            //ToDo: Delete, HardCore limit, it is implemented in Token Defination already with Admin Level Data.
            if (i > 6) i = 6;
            return i;
        }
        public string vName { get; set; }


        internal WalletManager GetWalletManager()
        {

            var Mgr = new WalletManager();
            Mgr._configuration = _configuration;
            Mgr._http = _http;
            Mgr._appSessionManager = _appSessionManager;
            return Mgr;
        }
        internal MarketManager GetMarketManager()
        {

            var Mgr = new MarketManager();
            Mgr._configuration = _configuration;
            Mgr._http = _http;
            Mgr._appSessionManager = _appSessionManager;
            return Mgr;
        }
        public async Task<bool> IsValidSession()
        {
            await _appSessionManager.ExtSession.LoadSession();
            return _appSessionManager.ExtSession.IsValid;

        }
    }
}
