namespace NavExM.Int.Maintenance.APIs.Model;

public class mStakingSlot
{
    public Guid StakingSlotId { get; set; }
    //Clubbing multipal duration together
    public Guid GroupId { get; set; }
    public string GroupName { get; set; }
    public string GroupDetails { get; set; }

    public List<mStakingSlotInstance> Instances { get; set; }
    public double TotalTarget { get; set; }//Token Qty that will trigger sunset
    public bool IsSunSet { get; set; }
    public DateTime OfferStartedOn { get; set; }
    public DateTime? OfferShouldExpierOn { get; set; }//Null for prepetual untill sunset
    public DateTime? OfferExpiredOn { get; set; }

    [ForeignKey("Token")]
    public Guid TokenId { get; set; }
}
public class mStakingSlot2
{
    public Guid StakingOpportunityId { get; set; }
    //Clubbing multipal duration together
    public Guid GroupId { get; set; }
    public string GroupName { get; set; }
    public string GroupDetails { get; set; }
    public string Name { get; set; }
    public string Details { get; set; }
    public eCommunityCategory Community { get; set; }
    public int Duration { get; set; }
    public double AYPOffered { get; set; }
    public bool AutoRenewAllowed { get; set; }//Auto Renew at the end of duration
    public bool IsHardFixed { get; set; }//can only be redeemd when duration is completed
    public double? MinAmount { get; set; }
    public double? MaxAmount { get; set; }
    public double TotalTarget { get; set; }//Token Qty that will trigger sunset
    public bool IsSunSet { get; set; }
    public DateTime OfferStartedOn { get; set; }
    public DateTime? OfferShouldExpierOn { get; set; }//Null for prepetual untill sunset
    public DateTime? OfferExpiredOn { get; set; }
    public Guid TokenId { get; set; }
    public mToken Token { get; set; }

}