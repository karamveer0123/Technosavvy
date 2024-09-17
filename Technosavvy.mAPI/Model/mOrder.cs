namespace NavExM.Int.Maintenance.APIs.Model;

public class rOrder {
    public string OrderID { get; set; }
    public Guid InternalOrderID { get; set; }
    public string MarketCode { get; set; }
    public string MarketName { get; set; }
    public DateTime PlacedOn { get; set; }
    public double Volume { get; set; }
    public eOrderType OrderType { get; set; }
    public eOrderSide OrderSide { get; set; }
    public double OriginalVolume { get; set; }
    public double CurrentVolume { get; set; }
    public double Trigger { get; set; }
    public double ProcessedVolume { get; set; }
    public double Price { get; set; }
    public Guid BaseTokenId { get; set; }
    public Guid QuoteTokenId { get; set; }
    public string QuoteTokenCodeName { get; set; }
    public string BaseTokenCodeName { get; set; }
    public eOrderStatus Status { get; set; }
    public double _OrderSwapTradeValue { get; set; }
    public double _OrderAssetAmount { get; set; }
    public double _OrderTrigger { get; set; }
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
//Market making Order
public class mMMOrder
{
    public Guid IterationID { get; set; }
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
public enum eOrderSide
{
    Buy,
    Sell,
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
