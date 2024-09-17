using TechnoApp.Ext.Web.UI.Model;
using TechnoApp.Ext.Web.UI.Service;

namespace TechnoApp.Ext.Web.UI.Manager
{
    public class MarketManager : MaintenanceSvc
    {
        
        //GetMarketPair with profile
        internal async Task<mMarket?> GetMarketPair(string mCode)
        {
            var retval = await base.GetMarketPair(mCode);
            return retval;
        }
        internal async Task<List<mMarket?>> GetActiveMarketsForCountry(string Abbr)
        {
            var retval = await base.GetActiveMarketsForCountry(Abbr);
            return retval;
        }
        internal async Task<List<mMarketDataSummary>> GetTopGainsMarket(vmTradeOrder vm, string mName)
        {
            var retval = new List<mMarketDataSummary>();
            return retval;
        }
        internal async Task<List<mMarketDataSummary>> GetTopVolumnMarket(vmTradeOrder vm, string mName)
        {
            var retval = new List<mMarketDataSummary>();
            return retval;
        }
        internal async Task<List<mMarketDataSummary>> GetMyFavouriteMarket(vmTradeOrder vm, string mName)
        {
            var retval = new List<mMarketDataSummary>();
            return retval;
        }
    }
}