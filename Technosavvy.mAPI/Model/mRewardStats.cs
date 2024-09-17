namespace NavExM.Int.Maintenance.APIs.Model;

public class mRewardStats
{
    public string RefLink { get; set; }
    public int TotalNoOfRef { get; set; }
    public int LastMonthRef { get; set; }
    public bool LastMonthPayStatus { get; set; }
    public bool ThisMonthPayStatus { get; set; }
    public int ThisMonthRef { get; set; }
    public int RegisteredUserCount { get; set; }
    public int CommunityUserCount { get; set; }
    public int QualifiedUserCount { get; set; }
    public double NavCPaid { get; set; }
    public double NavCUnPaid { get; set; }
}
public class mMyReward
{
    public DateTime EarnedOn { get; set; }
    public DateTime? PaidOn { get; set; }
    public string RewardType { get; set; }
    /// <summary>
    /// USDT value ofReward Given
    /// </summary>
    public double Amount { get; set; }
    /// <summary>
    /// No of NavC Tokens
    /// </summary>
    public double? Reward { get; set; }

    public eRewardStatus Status { get; set; }
}
public class mUserRef
{
    public string Id { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string UserStatus { get; set; }
    public bool IsActive { get; set; }
    public double Amount { get; set; }
    public int Count { get; set; }
}
public class mAccruedCashBack
{
    public eCommunityCategory Community { get; set; }
    public Guid RewardTransactionId { get; set; }
    public Guid TradeId { get; set; }
    public string MarketCode { get; set; }
    public double TradeValue { get; set; }
    public double CashBackNavCValue { get; set; }
    public double CashBackNavCTokens { get; set; }
    public DateTime CreatedOn { get; set; }
}

public class mConvertTokenRequest
{
    public Guid TradeId { get; set; }
    public Guid TransactionId { get; set; }
    public Guid fromTokenId { get; set; }
    public string qCode { get; set; }
    public string bCode { get; set; }
    public Guid toTokenId { get; set; }
    /// <summary>
    /// Token Amount that is used to pay with
    /// </summary>
    public double fromAmt { get; set; }
    /// <summary>
    /// Token Amount that is purchased or intended to purchased
    /// </summary>
    public double toTokenAmt { get; set; }
    public double RateOfOneToToken { get; set; }
    public bool IsFundWalletAllowed { get; set; }
    public bool IsSpotWalletAllowed { get; set; }
    public bool IsError { get; set; }
    public string ErrMsg { get; set; }
}

