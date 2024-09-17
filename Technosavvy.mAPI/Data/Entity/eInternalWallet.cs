namespace NavExM.Int.Maintenance.APIs.Data.Entity;

[Index(nameof(BelongsTo))]
[Index(nameof(WalletType))]
[Index(nameof(WalletNature))]
[Index(nameof(RelatedCountryId))]
public class eInternalWallet : secBaseEntity1
{
    /*when we have a same type of wallet for different country but need to map with single External Wallet, Global Wallet will be used as External Wallet Representation and Country Specific Wallet will be used for Internal Transaction but balance status will only be drived using Global Adjustment and GroupId will point all such wallets
     * 1. Each country will have specific Market Wallet, But a Global Wallet must exist already
     * 2. Global Wallet will be Mapped with External Wallet
     * 3. Country Specific Wallet with be Attached with Global Wallet using GroupId
     */
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid InternalWalletId { get; set; }
    [StringLength(50)]
    public string Name { get; set; }
    [StringLength(50)]
    public string BelongsTo { get; set; }//Market Name ETHUSDT,Department/Process name 
    public eInternalWalletType WalletType { get; set; }
    public eWalletNature WalletNature { get; set; }
    /// <summary>
    /// InternalWalletId of a Globale Wallet, will be globalId if wallet is specific to a country
    /// </summary>
    public Guid? GlobalId { get; set; }
    //If this Wallet is specific to a country else Global
    [ForeignKey("RelatedCountry")]
    public Guid? RelatedCountryId { get; set; }
    public eCountry? RelatedCountry { get; set; }

    public List<eInternalWBalance> InternalWBalance { get; set; }
}
[Index(nameof(InternalWalletId),nameof(NetworkWalletAddressId),IsUnique =true)]
[Index(nameof(InternalWalletId))]
[Index(nameof(NetworkWalletAddressId))]
public class eInternalWalletMapToExternal : secBaseEntity1
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [ForeignKey(nameof(InternalWallet))]
    public Guid InternalWalletId { get; set; }
    public eInternalWallet InternalWallet { get; set; }

    [ForeignKey(nameof(NetworkWalletAddress))]
    public Guid NetworkWalletAddressId { get; set; }
    public eNetworkWalletAddress NetworkWalletAddress { get; set; }

}
public enum eInternalWalletType
{
    /// <summary>
    /// All Payments Received in External ScWallet at default NavExM Wallet
    /// </summary>
    Global,
    //Logical Type of all Reasons that can cause an Internal Transaction 
    /// <summary>
    /// When a Trade Swap fee is charges
    /// </summary>
    Swap,
    /// <summary>
    /// Trade cashback Reward
    /// </summary>
    Cashback,
    /// <summary>
    /// Cashback refunded to Pool
    /// </summary>
    PoolRefund,
    /// <summary>
    /// Any Adjustment for the User due to process Fault
    /// </summary>
    ProcessFault,
    /// <summary>
    /// When an order is placed in a Market
    /// </summary>
    Market,
    /// <summary>
    /// When a Staking opportunity is Created 
    /// </summary>
    Staking
}
public enum eWalletNature
{
    /// <summary>
    /// All Payments Received in External ScWallet at default NavExM Wallet
    /// </summary>
    Global,
    //Nature of Transaction Type using internal Wallet 
    /// <summary>
    /// When Nature of the wallet is Earning
    /// </summary>
    Earnings,
    /// <summary>
    /// When Nature of the wallet Associated is to Diversify Token Types 
    /// </summary>
    Reserves,
    /// <summary>
    /// When Nature of the Wallet Associated is to Create Token Asset
    /// </summary>
    Assets,
    /// <summary>
    /// Liabilities towards external Parties i.e funds Deposited/Staked by Users, 
    /// </summary>
    Liability,
    /// <summary>
    /// When Nature of the wallet is to execute Reward Transaction
    /// </summary>
    Rewards
}
[Index(nameof(Balance))]
[Index(nameof(ChangeAgent))]
[Index(nameof(TokenId))]
[Index(nameof(InternalWalletId))]
[Index(nameof(InternalWalletId), nameof(ChangeAgent), IsUnique = true)]
public class eInternalWBalance : secBaseEntity1
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid InternalWBalanceId { get; set; }
    public double Balance { get; set; }

    public Guid ChangeAgent { get; set; }//Transaction Id
                                         //  public eCurrencyType CurrencyType { get; set; }

    [ForeignKey("Token")]
    public Guid? TokenId { get; set; }
    public eToken Token { get; set; }
    //Idea is to convert Fiat into eFiat and use that as Token. Fiat will restrict to Bank Account Only
    ////OR
    //[ForeignKey("FiatCurrency")]
    //public Guid? FiatCurrencyId { get; set; }
    //public eFiatCurrency? FiatCurrency { get; set; }

    [StringLength(500)]
    public string? Narration { get; set; }

    [ForeignKey("InternalWallet")]
    public Guid InternalWalletId { get; set; }
    public eInternalWallet InternalWallet { get; set; }
}