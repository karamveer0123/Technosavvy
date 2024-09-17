namespace NavExM.Int.Maintenance.APIs.Data.OrderCheck;
public class MktBookContext : DbContext
{

    public DbSet<smOrder> Orders { get; set; }
    public DbSet<eProcessedOrder> ProcessedSellOrders { get; set; }
    public DbSet<eProcessedOrder> ProcessedBuyOrders { get; set; }
    public DbSet<smTrade> Trades { get; set; }
    public DbSet<eBuyOrder> Market_BuyOrders { get; set; }
    public DbSet<eSellOrder> Market_SellOrders { get; set; }
    public DbSet<smOrder> _2ndSellOrders { get; set; }
    public DbSet<smOrder> _2ndBuyOrders { get; set; }
    public DbSet<OrderWrap> Triggered2ndOrders { get; set; }
    public DbSet<Buy2ndOrderWithTrigger> Buy2ndOrderWithTrigger { get; set; }
    public DbSet<Sell2ndOrderWithTrigger> Sell2ndOrderWithTrigger { get; set; }
    public MktBookContext(DbContextOptions<MktBookContext> options) : base(options)
    {
    }

}
public class OrderWrap
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public eOrderSide side { get; set; }
    [ForeignKey("buyOrder")]
    public Guid? buyOrderId { get; set; }

    public smOrder? buyOrder { get; set; }
    [ForeignKey("sellOrder")]
    public Guid? sellOrderId { get; set; }
    public smOrder? sellOrder { get; set; }
    public DateTime PlacedOn { get; set; }
    public string? ReserveId { get; set; }
    public DateTime? ReservasationExpiry { get; set; }
}
[Index("SessionId")]
[Index("WalletID")]
[Index("OrderID")]
[Index("InternalOrderID")]
[Index("PlacedOn")]
[Index("CreatedOn")]
[Index("MarketCode")]
[Index("OriginalVolume")]
[Index("CurrentVolume")]
[Index("ProcessedVolume")]
[Index("Price")]
[Index("Status")]
[Index("OrderType")]
[Index("StopPrice")]
[Index("LimitPrice")]
[Index("BaseTokenId")]
[Index("QuoteTokenId")]
[Index("SwapRate")]
[Index("SwapValue")]
[Index("CBThreashold")]
[Index("CBCommitment")]
[Index("CashBackNavCValue")]
[Index("ReserveId")]
[Index("ReservasationExpiry")]
public class eSellOrder
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [StringLength(1000)]
    public string SessionId { get; set; }
    public byte[] RowVersion { get; set; }
    public Guid WalletID { get; set; }
    //AccountNo-SessionCounter-MarketCode-Ticks-Random
    //100525898.1.BTCUSDT.595448498171000000.015
    [StringLength(1000)]
    public string OrderID { get; set; }
    public Guid InternalOrderID { get; set; }
    public DateTime PlacedOn { get; set; }//Time Ticks
    public DateTime CreatedOn { get; set; }//date time to display
    [StringLength(100)]
    public string MarketCode { get; set; }
    public double OriginalVolume { get; set; }
    public double CurrentVolume { get; set; }
    public double ProcessedVolume { get; set; }

    public double Price { get; set; }
    public eOrderStatus Status { get; set; }
    public eOrderType OrderType { get; set; }
    public double StopPrice { get; set; }
    public double LimitPrice { get; set; }
    public Guid BaseTokenId { get; set; }
    public Guid QuoteTokenId { get; set; }
    [StringLength(100)]
    public string BaseTokenCodeName { get; set; }//Added Field
    [StringLength(100)]
    public string QuoteTokenCodeName { get; set; }//-Added Field
    public double SwapRate { get; set; }//0.02%
    public double SwapValue { get; set; }//Calculation of Trade*SwapRate

    public double CBThreashold { get; set; }//0.05%
    public double CBCommitment { get; set; }//Staking Assurance
    public double CashBackNavCValue { get; set; }
    [NotMapped]
    public List<eTrade> OrderTrades { get; set; } = new List<eTrade>();
    public smOrder oOrder { get; set; }
    [ForeignKey("oOrder")]
    public Guid oOrderId { get; set; }
    public string? ReserveId { get; set; }
    public DateTime? ReservasationExpiry { get; set; }
    [NotMapped]
    public bool isMarketBook { get; set; }
}

[Index("TradeId")]
[Index("GroupId")]
[Index("TradeId")]
[Index("MarketCode")]
[Index("dateTimeUTC")]
[Index("CreatedOn")]
[Index("BuyOrderId")]
[Index("TradePrice")]
[Index("TradeValue")]
[Index("BaseTokenCodeName")]
[Index("QuoteTokenCodeName")]
[Index("SwapRate")]
[Index("CBThreashold")]
[Index("CBCommitment")]
[Index("CashBackNavCValue")]
public class eTrade
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    private eBuyOrder buyOrder;
    private eSellOrder saleOrder;
    public Guid TradeId { get; set; } = Guid.NewGuid();
    /// <summary>
    /// If Sale or Buy Order has resulted in more than one Trade then all these Trades will belong to same Group
    /// </summary>
    public Guid GroupId { get; set; }
    [StringLength(50)]
    public string MarketCode { get; set; }
    /// <summary>
    /// UTC Standard date time of the Trade
    /// </summary>
    public DateTime dateTimeUTC { get; set; }
    public DateTime CreatedOn { get; set; }//{ get => dateTimeUTC; }
    /// <summary>
    /// Sale Order that was fully or partially executed in this Trade
    /// </summary>
    [NotMapped]
    public eSellOrder SaleOrder { get => saleOrder; set { saleOrder = value; SellOrderId = value.InternalOrderID; } }
    public Guid SellOrderId { get; set; }

    /// <summary>
    /// Buy Order that was fully or partially executed in this Trade
    /// </summary>
    [NotMapped]
    public eBuyOrder BuyOrder
    {
        get
        {
            return buyOrder;
        }
        set
        {
            buyOrder = value;
            BuyOrderId = value.InternalOrderID;
        }
    }

    public Guid BuyOrderId { get; set; }

    /// <summary>
    /// Price of per unit Base Security in this trade
    /// </summary>
    public double TradePrice { get; set; }
    /// <summary>
    /// Total Quantity/Volumn of the Base security exchanged in this trade
    /// </summary>
    public double TradeVolumn { get; set; }
    /// <summary>
    /// Multipal of volume and Price
    /// </summary>
    public double TradeValue { get; set; }
    [StringLength(50)]
    public string BaseTokenCodeName { get; set; }//Added Field
    [StringLength(50)]
    public string QuoteTokenCodeName { get; set; }//-Added Field
    public double SwapRate { get; set; }//0.02%
    public double SwapValue { get; set; }//Calculation of Trade*SwapRate

    public double CBThreashold { get; set; }//0.05%
    public double CBCommitment { get; set; }//Staking Assurance
    public double CashBackNavCValue { get; set; }
}
[Index("SessionId")]
[Index("WalletID")]
[Index("OrderID")]
[Index("InternalOrderID")]
[Index("PlacedOn")]
[Index("CreatedOn")]
[Index("MarketCode")]
[Index("OriginalVolume")]
[Index("CurrentVolume")]
[Index("ProcessedVolume")]
[Index("Price")]
[Index("Status")]
[Index("OrderType")]
[Index("StopPrice")]
[Index("LimitPrice")]
[Index("BaseTokenId")]
[Index("QuoteTokenId")]
[Index("SwapRate")]
[Index("SwapValue")]
[Index("CBThreashold")]
[Index("CBCommitment")]
[Index("CashBackNavCValue")]
[Index("ReserveId")]
[Index("ReservasationExpiry")]
public class eBuyOrder
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string SessionId { get; set; }
    public byte[] RowVersion { get; set; }
    public Guid WalletID { get; set; }
    //AccountNo-SessionCounter-MarketCode-Ticks-Random
    //100525898.1.BTCUSDT.595448498171000000.015
    public string OrderID { get; set; }
    public Guid InternalOrderID { get; set; }
    public DateTime PlacedOn { get; set; }//Time Ticks
    public DateTime CreatedOn { get; set; }//date time to display
    public string MarketCode { get; set; }
    public double OriginalVolume { get; set; }
    public double CurrentVolume { get; set; }
    public double ProcessedVolume { get; set; }
    public double Price { get; set; }
    public eOrderStatus Status { get; set; }
    public eOrderType OrderType { get; set; }
    public double StopPrice { get; set; }
    public double LimitPrice { get; set; }
    public Guid BaseTokenId { get; set; }
    public Guid QuoteTokenId { get; set; }
    public string BaseTokenCodeName { get; set; }//Added Field
    public string QuoteTokenCodeName { get; set; }//-Added Field
    public double SwapRate { get; set; }//0.02%
    public double SwapValue { get; set; }//Calculation of Trade*SwapRate

    public double CBThreashold { get; set; }//0.05%
    public double CBCommitment { get; set; }//Staking Assurance
    public double CashBackNavCValue { get; set; }
    [NotMapped]
    public List<eTrade> OrderTrades { get; set; } = new List<eTrade>();
    public smOrder oOrder { get; set; }
    [ForeignKey("oOrder")]
    public Guid oOrderId { get; set; }
    public string? ReserveId { get; set; }
    public DateTime? ReservasationExpiry { get; set; }
    [NotMapped]
    public bool isMarketBook { get; set; }

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
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    public bool IsApproved { get; set; }
    public DateTime? ApprovedAt { get; set; }
    [StringLength(500)]
    public string? ApprovedBy { get; set; }
}
[Index("WalletID")]
[Index("OrderID")]
[Index("UserAccountNo")]
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
    public byte[] RowVersion { get; set; }
    [StringLength(500)]
    public string? UserAccountNo { get; set; }
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
}
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
    public Guid Id { get; set; }

    public byte[] RowVersion { get; set; }
    public string? UserAccountNo { get; set; }
    public string SessionId { get; set; }
    //SessionId+WalletId+OrderId+OrderType+Amount+Price
    public string AuthId { get; set; }
    public Guid WalletID { get; set; }
    //AccountNo-SessionCounter-MarketCode-Ticks-Random
    //100525898.1.BTCUSDT.595448498171000000.015
    public string OrderID { get; set; }
    public Guid InternalOrderID { get; set; }
    public DateTime PlacedOn { get; set; }//Time Ticks
    public DateTime CreatedOn { get; set; }//date time to display
    public ulong OrderBookID { get; set; }
    [StringLength(50)]
    public string MarketCode { get; set; }
    public eOrderType OrderType { get; set; }
    public eOrderSide OrderSide { get; set; }
    public double OriginalVolume { get; set; }
    public double CurrentVolume { get; set; }
    public double Price { get; set; }
    public eOrderStatus Status { get; set; }
    public Guid BaseTokenId { get; set; }
    public Guid QuoteTokenId { get; set; }
    [StringLength(50)]
    public string BaseTokenCodeName { get; set; }//Added Field
    [StringLength(50)]
    public string QuoteTokenCodeName { get; set; }//-Added Field
    public double DiscloseAmount { get; set; }
    public double DiscloseValue { get; set; }
    public double StopPrice { get; set; }
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

    //-------
    public List<eTrade> OrderTrades { get; set; } = new List<eTrade>();
    public List<eProcessedOrder> ProcessedOrder { get; set; } = new List<eProcessedOrder>();

    //----
    /// <summary>
    /// If this version of smOrder already exist, then reference to that
    /// </summary>
    [NotMapped]
    public eBuyOrder? buyOrder { get; set; }
    /// <summary>
    /// If this version of smOrder already exist, then reference to that
    /// </summary>
    [NotMapped]
    public eSellOrder? sellOrder { get; set; }
    /// <summary>
    /// In case 2ndOrder is to be executed and it is triggered
    /// </summary>
    [NotMapped]
    public bool IsTriggered { get; set; }
}
[Index("TriggerVal")]
public class Buy2ndOrderWithTrigger
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public double TriggerVal { get; set; }
    [ForeignKey("Order")]
    public Guid? OrderId { get; set; }
    public smOrder? Order { get; set; }
    [StringLength(50)]
    public string? LockKey { get; set; }
}
[Index("TriggerVal")]
public class Sell2ndOrderWithTrigger
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public double TriggerVal { get; set; }
    [ForeignKey("Order")]
    public Guid? OrderId { get; set; }
    public smOrder? Order { get; set; }
    [StringLength(50)]
    public string? LockKey { get; set; }
}
