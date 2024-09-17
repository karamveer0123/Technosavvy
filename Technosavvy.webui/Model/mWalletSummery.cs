namespace TechnoApp.Ext.Web.UI.Model
{
    public class mWalletSummery
    {
        public Guid WalletId { get; set; }
        public string Name { get; set; }
        public List<mWalletCoin> Tokens { get; set; }
        public List<mWalletCoin> Fiats { get; set; }
        //Fiat...!!
    }
    public class mCreateStake
    {
        public Guid StakeId { get; set; }
     //   public Guid fromFundWalletId { get; set; }
        public double Amount { get; set; }
        public bool AutoRenew { get; set; }
        public Guid StakingOpportunityId { get; set; }
       // public Guid userAccountId { get; set; }

    }
    public class mStake
    {
        public Guid StakeId { get; set; }
        public Guid FromTransactionId { get; set; }
        public Guid fromFundWalletId { get; set; }
        public double Amount { get; set; }

        public DateTime StartedOn { get; set; }
        public DateTime ExpectedEndData { get; set; }
        public double MatureAmount { get; set; }
        public bool AutoRenew { get; set; }
        public bool IsRedeemed { get; set; }
        public DateTime? RedeemedOn { get; set; }
        public Guid? ToTransactionId { get; set; }
        public Guid StakingOpportunityId { get; set; }
        public mStakingSlot2 StakingSlot{ get; set; }
        public Guid userAccountId { get; set; }

    }
}