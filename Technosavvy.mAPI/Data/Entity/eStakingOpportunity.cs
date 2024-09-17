namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    public class eStakingOpportunity:secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public DateTime? OfferShouldExpierOn { get; set; }//Null for prepetual
        public DateTime? OfferExpiredOn{ get; set; }

        public bool IsApproved { get; set; }//Initial Create is Draft and isApprove will bring it in Action
        [StringLength(1000)]
        public string? ApprovedBy { get; set; }
        //
        public string RecordHash { get; set; }
        public List<eStaking> RelatedStakings { get; set; }

        [ForeignKey("Token")]
        public Guid TokenId { get; set; }
        public eToken Token { get; set; }

    }
    public enum eCommunityCategory
    {
        None=0,
        Community=1,
        Standard=2,
        Premimum=3
    }
}
