namespace TechnoApp.Ext.Web.UI.Model;

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
    public double TechnoSavvyPaid { get; set; }
    public double TechnoSavvyUnPaid { get; set; }
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
    /// No of TechnoSavvy Tokens
    /// </summary>
    public double? Reward { get; set; }

    public eRewardStatus Status { get; set; }
}
public enum eRewardStatus
{
    None = 0,
    Earned = 1,
    Paid = 2
}