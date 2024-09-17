namespace TechnoApp.Ext.Web.UI.Model
{
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

        public Guid TokenId { get; set; }
    }
}
