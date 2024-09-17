namespace TechnoApp.Ext.Web.UI.Model
{
    public class mStakingSlotInstance
    {
        public Guid StakingSlotId { get; set; }
        public Guid StakingSlotInstanceId { get; set; }//child Id

        public string Name { get; set; }
        public string Details { get; set; }
        public int Duration { get; set; }
        public double AYPOffered { get; set; }
        public bool AutoRenewAllowed { get; set; }//Auto Renew at the end of duration
        public bool IsHardFixed { get; set; }//can only be redeemd when duration is completed
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
    }
}
