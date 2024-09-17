using System.Text.Json.Serialization;

namespace TechnoApp.Ext.Web.UI.Model
{
    public class mOrder
    {
        public DateTime PlacedOn { get; set; }
        public string MarketCode { get; set; }
        public eOrderType OrderType { get; set; }
        public eOrderSide OrderSide { get; set; }
        public double Volume { get; set; }
        public double Price { get; set; }
        public eOrderStatus Status { get; set; }
        public Guid BaseTokenId { get; set; }
        public string BaseTokenCodeName { get; set; }
        public string QuoteTokenCodeName { get; set; }
        public Guid QuoteTokenId { get; set; }
        public double DiscloseAmount { get; set; }
        public double DiscloseValue { get; set; }
        public double StopPrice { get; set; }
        public double LimitPrice { get; set; }
        public int LoopCount { get; set; }
    }
    public class mTrade
    {
        public Guid TradeId { get; set; }
        public string MarketCode { get; set; }
        public DateTime dateTimeUTC { get; set; }
        public eOrderType OrderType { get; set; }
        public string OrderTypeT { get => OrderType.ToString(); }
        public eOrderSide OrderSide { get; set; }
        public string OrderSideT { get => OrderSide.ToString(); }
        //public string SellOrderID { get; set; }
        //public string BuyOrderID { get; set; }
        public string OrderID { get; set; }
        public double SwapValue { get; set; }//Calculation of Trade*SwapRate
        public double CashBackTechnoSavvyValue { get; set; }
        public double TradePrice { get; set; }
        public double TradeValue { get; set; }
        public double TradeVolumn { get; set; }
        public double _SWAPRate { get; set; }
        public double _SWAPValue { get; set; }
        public double _AssetAmount { get; set; }
        public double _AssetAmountValue { get; set; }
        public double _CashBackTechnoSavvyValue { get; set; }
        public double _PoolRefund { get; set; }
    }
    public enum eOrderSide
    {
        Buy,
        Sell,
    }
    public enum eOrderStatus
    {
        Received = -1,
        /// <summary>
        /// Wallet balance,Session Verfied and good to place this order
        /// </summary>
        VerifiedToRequest = 0,
        /// <summary>
        /// when order is accepted in the Books
        /// </summary>
        Placed = 1,
        /// <summary>
        /// For Any Reason before Execution, This Order is Rejected
        /// </summary>
        Rejected = 2,
        /// <summary>
        /// When Order is partially Completed
        /// </summary>
        PartialCompleted = 4,
        /// <summary>
        /// When Order is Cancelled
        /// </summary>
        Cancelled = 8,
        /// <summary>
        /// When Order is Completed
        /// </summary>
        Completed = 16,
    }
    public enum eOrderType
    {
        Market,
        Limit,
        IceBurg,
        IOC,
        Loop,
        STOPLimit,
        MarketLimit,
        OCO,
        FOK
    }
    /// <summary>
    /// Report Only Order Class for Trade UI Reporting
    /// </summary>
    public class rOrder
    {
        public string OrderID { get; set; }
        public Guid InternalOrderID { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public DateTime PlacedOn { get; set; }
        public double Volume { get; set; }
        public eOrderType OrderType { get; set; }
        public string OrderTypeT { get => OrderType.ToString(); }
        public eOrderSide OrderSide { get; set; }
        public string OrderSideT { get => OrderSide.ToString(); }
        public double OriginalVolume { get; set; }
        public double CurrentVolume { get; set; }
        public double ProcessedVolume { get; set; }
        public double Trigger { get; set; }
        public double Price { get; set; }
        public Guid BaseTokenId { get; set; }
        public Guid QuoteTokenId { get; set; }
        public string QuoteTokenCodeName { get; set; }
        public string BaseTokenCodeName { get; set; }
        public eOrderStatus Status { get; set; }
        public string StatusT { get => Status.ToString(); }

        public double _OrderSwapTradeValue { get; set; }
        public double _OrderAssetAmount { get; set; }
        public double _OrderTrigger { get; set; }
    }
}
