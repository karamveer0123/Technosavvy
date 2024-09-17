namespace TechnoApp.Ext.Web.UI.ViewModels
{
    public class vmMarketTrade : vmBaseTrade
    {
        public vmTradeOrder TradeOrder { get; set; } //= new vmTradeOrder();
        public List<vmSpotWalletTokenDetail> Funds { get; set; } 
        public List<vmMarketMyOrder> OpenOrders { get; set; } 
        public vmMarketTrade_OrderHistory OrderHistory { get; set; } 
        public vmMarketTrade_TradeHistory TradeHistory { get; set; } 
    }
    public class vmMarketTrade_TradeHistory
    {
        public vmMarketTradeSearchCriteria SearchCriteria { get; set; } = new vmMarketTradeSearchCriteria();
        public List<vmMarketMyTrade> Trades { get; set; } = new List<vmMarketMyTrade>();

    }
    public class vmMarketTrade_OrderHistory
    {
        public vmMarketTradeSearchCriteria SearchCriteria { get; set; } = new vmMarketTradeSearchCriteria();
        public List<vmMarketMyOrder> Orders { get; set; } = new List<vmMarketMyOrder>();
    }
    public class vmTradeOrder : vmBase
    {
        public long Ticks { get; set; }
        public string MCode { get; set; }//Market Code
        public double AvailableBase { get; set; }
        public double AvailableQuote { get; set; }
        public string BaseName { get; set; }
        public string QuoteName { get; set; }
        public string BaseTokenId{ get; set; }
        public string QuoteTokenId{ get; set; }
        public string QuoteFormat { get; set; }//to be deleted?
        public string BaseFormat { get; set; }//to be deleted?
        public int QuoteFraction { get; set; }//Token Display Fraction
        public int BaseFraction { get; set; }//Token Display Fraction
        public double MinQToken { get; set; }//Min Fraction of Quote in a Trade
        public double MinBToken { get; set; }//Min Fraction of Base in a Trade
        public double MinTradeValue { get; set; }//in USDT
    }

    public class vmMarketMyOrder : vmBase
    {
        public long OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string MarketCode { get; set; }//BTC_USDT
        public string Asset { get; set; } //BTC
        public string OrderType { get; set; } //BUY
        public double Price { get; set; }
        public double Amount { get; set; }
        public double Filled { get; set; } //If Partial Fill
        public double Total { get; set; }
        public string TriggerCondition { get; set; }

    }


    public class vmMarketMyTrade
    {
        public long OrderId { get; set; }
        public DateTime TradeDate { get; set; }
        public string MarketCode { get; set; }
        public string OrderType { get; set; }
        public double Executed { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }//?
        public double Filled { get; set; }
        public double Total { get; set; }

    }
    public class vmMarketTradeSearchCriteria
    {
        public int Time { get; set; }//in days
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LookFor { get; set; }
    }
    public class mMarketOrder
    {
        public double Amount { get; set; }
        public int side { get; set; }//0=buy|1=sell
        public double Price { get; set; }
        public string mCode { get; set; }
        public string quoteCode { get; set; }
        public string qTag { get; set; }
        public string baseCode { get; set; }
        public string bTag { get; set; }
        public string OrderId { get; set; }
        public double stopPrice { get; set; }
    }
   
}
