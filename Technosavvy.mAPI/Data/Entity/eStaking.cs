namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    public class eStaking:secBaseEntity1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid StakingId { get; set; }
        public Guid StakingWalletId { get; set; }//This would be the wallet Id for this staking Record i.e. Fixed Deposit number
        [Required]
        public Guid FromTransactionId { get; set; }
        [ForeignKey("FromFundWallet")]
        public Guid fromFundWalletId { get; set; }//should always be fund wallet
        public eFundingWallet FromFundWallet { get; set; }
        public double Amount { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime ExpectedEndData { get; set; }
        public double MatureAmount { get; set; }
        public bool AutoRenew { get; set; }
        public bool IsRedeemed { get; set; }
        public DateTime? RedeemedOn { get; set; }
        public Guid? ToTransactionId { get; set; }
        public string RecordHash { get; set; }

        [ForeignKey("StakingOpportunity")]
        [Required]
        public Guid StakingOpportunityId { get; set; }
        public eStakingOpportunity StakingOpportunity { get; set; }
       
        [ForeignKey("userAccount")]
        public Guid userAccountId { get; set; }
        public eUserAccount userAccount { get; set; }
    }
}
