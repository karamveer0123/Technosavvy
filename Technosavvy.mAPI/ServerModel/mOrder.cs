using Microsoft.VisualBasic;

namespace NavExM.Int.Maintenance.APIs.ServerModel;
[Index("UserAccountNo")]
[Index("AuthId")]
[Index("WalletID")]
[Index("OrderID")]
[Index("InternalOrderID")]
[Index("PlacedOn")]
[Index("CreatedOn")]
[Index("OrderBookID")]
[Index("OrderType")]
[Index("OrderSide")]
[Index("OriginalVolume")]
[Index("Price")]
[Index("Status")]
[Index("BaseTokenId")]
[Index("QuoteTokenId")]
[Index("DiscloseAmount")]
[Index("DiscloseValue")]
[Index("StopPrice")]
[Index("LimitPrice")]
[Index("LoopCount")]
[Index("SwapRate")]
[Index("SwapValue")]
[Index("CBThreashold")]
[Index("CBCommitment")]
[Index("CashBackNavCValue")]
[Index("CashBackNavCTokens")]
[Index("PoolRefunds")]
[Index("SwapTradeValue")]
public class smOrder
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid id { get; set; }
    public string SessionId { get; set; }
    //SessionId+WalletId+OrderId+OrderType+Amount+Price
    public string? UserAccountNo { get; set; }
    public string AuthId { get; set; }
    public byte[] RowVersion { get; set; }
    public Guid WalletID { get; set; }
    //AccountNo-SessionCounter-MarketCode-Ticks-Random
    //100525898.1.BTCUSDT.595448498171000000.015
    public string OrderID { get; set; }
    public Guid InternalOrderID { get; set; }
    public DateTime PlacedOn { get; set; }//Time Ticks
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;//date time to display
    public ulong OrderBookID { get; set; }
    public string MarketCode { get; set; }
    public eOrderType OrderType { get; set; }
    public eOrderSide OrderSide { get; set; }
    public double OriginalVolume { get; set; }
    public double CurrentVolume { get; set; }
    public double Price { get; set; }
    public eOrderStatus Status { get; set; }
    public Guid BaseTokenId { get; set; }
    public string BaseTokenCodeName { get; set; }
    public string QuoteTokenCodeName { get; set; }
    public Guid QuoteTokenId { get; set; }
    public double DiscloseAmount { get; set; }
    public double DiscloseValue { get; set; }
    public double StopPrice { get; set; }//also, used for Limit Order price Cap
    public double LimitPrice { get; set; }
    public int LoopCount { get; set; }
    //-------
    public double SwapRate { get; set; }//0.02%
    public double SwapValue { get; set; }//Calculation of Trade*SwapRate
    public double CBThreashold { get; set; }//0.05%
    /// <summary>
    /// Staking or Other business Commitment based Assured Rate of Return
    /// </summary>
    public double CBCommitment { get; set; }//Staking Assurance
    /// <summary>
    /// Value of NavC Token given in Cashback
    /// </summary>
    public double CashBackNavCValue { get; set; }
    /// <summary>
    /// Count of NavC Token given in Cashback, to be advised after cashback event
    /// </summary>
    public double CashBackNavCTokens { get; set; }
    public double PoolRefunds { get; set; }
    public double SwapTradeValue { get; set; }
    //------- Client UI Side Report Only, Do not Carry on Server side process
    public double _OrderSwapTradeValue { get; set; }
    public double _OrderAssetAmount { get; set; }
    public double _OrderTrigger { get; set; }
    public List<eProcessedOrder> ProcessedOrder { get; set; } = new List<eProcessedOrder>();
    //public List<smTrade> OrderTrades { get; set; } = new List<smTrade>();
}
public class smOrderPublishWrapper
{
    public long SenderTick { get; set; }
    public OrderEvent RelatedEvent { get; set; }
    public smOrder Order { get; set; }
    public string SenderAppId { get; set; }
    public string MachineName { get; set; }

}

[Index("SellOrderID")]
[Index("BuyOrderID")]
[Index("SellInternalId")]
[Index("BuyInternalId")]
[Index("TradeId")]
[Index("TradePrice")]
[Index("TradeVolumn")]
[Index("TradeValue")]
[Index("CreatedOn")]
public class TokenRefundIssues//ToDo: Still to be done
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public byte[] RowVersion { get; set; }
    public Guid TradeId { get; set; }
    public string Issue { get; set; }
    public string SellOrderID { get; set; }
    [StringLength(500)]
    public string BuyOrderID { get; set; }
    public Guid SellInternalId { get; set; }
    public Guid BuyInternalId { get; set; }
    [StringLength(50)]
    public string MarketCode { get; set; }
    public double TradePrice { get; set; }
    public double TradeVolumn { get; set; }
    public double TradeValue { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}
[Index("SellOrderID")]
[Index("BuyOrderID")]
[Index("SellInternalId")]
[Index("BuyInternalId")]
[Index("TradeId")]
[Index("TradePrice")]
[Index("TradeVolumn")]
[Index("TradeValue")]
[Index("CreatedOn")]
public class TradeIssues
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public byte[] RowVersion { get; set; }
    public Guid TradeId { get; set; }
    public string Issue { get; set; }
    public string SellOrderID { get; set; }
    [StringLength(500)]
    public string BuyOrderID { get; set; }
    public Guid SellInternalId { get; set; }
    public Guid BuyInternalId { get; set; }
    [StringLength(50)]
    public string MarketCode { get; set; }
    public double TradePrice { get; set; }
    public double TradeVolumn { get; set; }
    public double TradeValue { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}
[Index("RelatedEvent")]
[Index("ReceivedAt")]
[Index("UserAccountNo")]
[Index("OrderID")]
[Index("InternalOrderID")]
public class OrderAck
{
    [Key]
    public Guid dbStateId { get; set; }
    public OrderEvent RelatedEvent { get; set; }
    public DateTime ReceivedAt { get; set; }
    [StringLength(100)]
    public string? UserAccountNo { get; set; }
    [StringLength(500)]//MM-NAVCUSDT-1005248.NAVCUSDT.638392588751124900.11'
    public string OrderID { get; set; }
    [StringLength(20)]
    public string mCode { get; set; }
    public Guid InternalOrderID { get; set; }
    public DateTime PlacedOn { get; set; }//Time Ticks
}
public enum OrderEvent
{
    PlaceOrder,
    CancelOrder,
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
public class Market
{
    public List<smOrder> BuyOrders { get; set; } = new List<smOrder>();
    public List<smOrder> SellOrders { get; set; } = new List<smOrder>();
    public List<smOrder> BuyOrdersCompleted { get; set; } = new List<smOrder>();
    public List<smOrder> SellOrdersCompleted { get; set; } = new List<smOrder>();
    public List<smTrade> Trades { get; set; } = new List<smTrade>();
}
[Index("TradeId")]
[Index("GroupId")]
[Index("MarketCode")]
[Index("dateTimeUTC")]
[Index("CreatedOn")]
[Index("SellOrderID")]
[Index("BuyOrderID")]
[Index("SellInternalId")]
[Index("BuyInternalId")]
[Index("BaseTokenCodeName")]
[Index("QuoteTokenCodeName")]
[Index("SwapRate")]
[Index("SwapValue")]
[Index("CBThreashold")]
[Index("CBCommitment")]
[Index("CashBackNavCValue")]
[Index("TradePrice")]
[Index("TradeVolumn")]
[Index("TradeValue")]
[Index("IsApproved")]
[Index("ApprovedAt")]
[Index("ApprovedBy")]
[Index(nameof(BuyInternalId), nameof(SellInternalId), IsUnique = true)]
public class smTrade
{
    [Key]
    public Guid Id { get; set; }
    public Guid TradeId { get; set; }
    public Guid GroupId { get; set; }
    [StringLength(50)]
    public string MarketCode { get; set; }
    public DateTime dateTimeUTC { get; set; }
    public DateTime CreatedOn { get; set; }

    [StringLength(500)]
    public string SellOrderID { get; set; }
    [StringLength(500)]
    public string BuyOrderID { get; set; }
    public Guid SellInternalId { get; set; }
    public Guid BuyInternalId { get; set; }
    [StringLength(50)]
    public string BaseTokenCodeName { get; set; }//Added Field
    [StringLength(50)]
    public string QuoteTokenCodeName { get; set; }//-Added Field
    public double SwapRate { get; set; }//0.02%
    public double SwapValue { get; set; }//Calculation of Trade*SwapRate

    public double CBThreashold { get; set; }//0.05%
    public double CBCommitment { get; set; }//Staking Assurance
    public double CashBackNavCValue { get; set; }
    public double TradePrice { get; set; }
    public double TradeVolumn { get; set; }
    public double TradeValue { get; set; }
    public double _BuySWAPRate { get; set; }
    public double _SellSWAPRate{ get; set; }
    public double _BuySWAPValue { get; set; }
    public double _SellSWAPValue { get; set; }
    public double _BuyAssetAmount { get; set; }
    public double _SellAssetAmount { get; set; }
    public double _BuyAssetAmountValue { get; set; }
    public double _SellAssetAmountValue { get; set; }
    public double _BuyCashBackNavCValue { get; set; }
    public double _SellCashBackNavCValue { get; set; }
    public double _BuyPoolRefund{ get; set; }
    public double _SellPoolRefund { get; set; }
    public bool IsApproved { get; set; }
    public DateTime? ApprovedAt { get; set; }
    [StringLength(500)]
    public string? ApprovedBy { get; set; }
}
[Index("WalletID")]
[Index("OrderID")]
[Index("InternalOrderID")]
[Index("PlacedOn")]
[Index("ProcessedOn")]
[Index("OriginalVolume")]
[Index("Price")]
[Index("Status")]
[Index("BaseTokenId")]//-
[Index("QuoteTokenId")]//-
[Index("StopPrice")]//-
[Index("LimitPrice")]//-
[Index("SwapRate")]//-
[Index("SwapValue")]//-
[Index("CBThreashold")]//-
[Index("CBCommitment")]//-
[Index("CashBackNavCValue")]//-
[Index("CashBackNavCTokens")]//-
[Index("PoolRefunds")]//-
[Index("SwapTradeValue")]//-
[Index("OrderType")]//-
[Index("OrderSide")]//-
[Index("ProcessedVolume")]//-
[Index("TradeValue")]//-
[Index("PersistedOn")]//-
public class eProcessedOrder
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid WalletID { get; set; }
    public string? UserAccountNo { get; set; }

    public byte[] RowVersion { get; set; }
    [StringLength(500)]
    public string OrderID { get; set; }
    public Guid InternalOrderID { get; set; }
    public DateTime PlacedOn { get; set; }//Time Ticks
    public DateTime ProcessedOn { get; set; }//date time to display
    [StringLength(50)]
    public string MarketCode { get; set; }
    public Guid BaseTokenId { get; set; }
    public Guid QuoteTokenId { get; set; }
    [StringLength(50)]
    public string BaseTokenCodeName { get; set; }//Added Field
    [StringLength(50)]
    public string QuoteTokenCodeName { get; set; }//-Added Field
    public double SwapRate { get; set; }//0.02%
    public double SwapValue { get; set; }//Calculation of Trade*SwapRate

    public double CBThreashold { get; set; }//0.05%
    public double CBCommitment { get; set; }//Staking Assurance
    public double CashBackNavCValue { get; set; }
    public double CashBackNavCTokens { get; set; }
    public double PoolRefunds { get; set; }
    public double SwapTradeValue { get; set; }
    public eOrderType OrderType { get; set; }
    public eOrderSide OrderSide { get; set; }
    public double OriginalVolume { get; set; }
    public double ProcessedVolume { get; set; }
    public double TradeValue { get; set; }
    public double Price { get; set; }
    public eOrderStatus Status { get; set; }
    public double StopPrice { get; set; }
    public double LimitPrice { get; set; }
    public DateTime PersistedOn { get; set; } = DateTime.UtcNow;
    //--
    [ForeignKey("myOrder")]
    public Guid? myOrderId { get; set; }
    public smOrder? myOrder { get; set; }
}
public class smNetWalletBox
{
    public Guid NetworkId { get; set; }
    public string WalletAddress { get; set; }
    public Guid userAccountId { get; set; }
    public WalletOwnerType OwnerType { get; set; }
    public string userAccount { get; set; }
    public string SessionHash { get; set; }
    public DateTime CreatedOn { get; set; }
}
public class mTrade
{
    public Guid TradeId { get; set; }
    public string MarketCode { get; set; }
    public DateTime dateTimeUTC { get; set; }
    public eOrderSide OrderSide { get; set; }
    public eOrderType OrderType { get; set; }

    //public string SellOrderID { get; set; }
    //public string BuyOrderID { get; set; }
    public string OrderID { get; set; }
    public double SwapValue { get; set; }//Calculation of Trade*SwapRate
    public double CashBackNavCValue { get; set; }
    public double TradePrice { get; set; }
    public double TradeValue { get; set; }
    public double TradeVolumn { get; set; }
    public double _SWAPRate { get; set; }
 
    public double _SWAPValue { get; set; }
 
    public double _AssetAmount { get; set; }
 
    public double _AssetAmountValue { get; set; }
 
    public double _CashBackNavCValue { get; set; }
 
    public double _PoolRefund { get; set; }
}
[Index(nameof(WalletID))]
[Index(nameof(OrderID))]
[Index(nameof(IterationId))]
public class smIterationOrders
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid id { get; set; }
    public byte[] RowVersion { get; set; }
    public Guid WalletID { get; set; }
    //AccountNo-SessionCounter-MarketCode-Ticks-Random
    //100525898.1.BTCUSDT.595448498171000000.015
    [StringLength(500)]
    public string OrderID { get; set; }
    [StringLength(100)]
    public string mCode { get; set; }
    public int Status { get; set; }
    public Guid IterationId { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public List<smOrder> Orders{ get; set; }
}
public enum WalletOwnerType
{
    ClientInitial = 0,
    ClientPermanent = -1,
    Staff = 1,
    System = 2
}