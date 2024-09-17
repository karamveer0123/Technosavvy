
namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    public class eUserAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserAccountId { get; set; }
        [Required]
        public eAuthorizedEmail? AuthEmail { get; set; }
        /// <summary>
        /// Human Readable Numeric Account Number of the User
        /// </summary>
        public ulong? AccountNumber { get; set; }
        /// <summary>
        /// Formated Account Number for Quick grouping
        /// </summary>
        [StringLength(22)]
        public string? FAccountNumber { get; set; }
        public eSecurePassword? SecurePassword { get; set; }
        /// <summary>
        /// True, If User has asked for Authenticator
        /// </summary>
        public bool IsMultiFactor { get; set; }
        /// <summary>
        /// Object Created if User have Completed the setup of google Authenticator
        /// </summary>
        public eProfile? UserProfile { get; set; }
        public eAuthenticator? Authenticator { get; set; }
        public eTaxResidency? TaxResidency { get; set; }
        public eCitizenship? CitizenOf { get; set; }
        public eMobile? Mobile { get; set; }
        /// <summary>
        /// Third Paty code who Referred this User
        /// </summary>
        public eRefCodes RefCode { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// True, if Your has completed its Password stage
        /// </summary>
        public bool IsPrimaryCompleted { get; set; }
        /// <summary>
        /// False, If User has been Blocked by Staff or System
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// List of All Email Validation Attempt to Confirm This Account's Email Ownership
        /// </summary>
        public List<eEmailValidationAction> EmailValidationActions { get; set; }

        [ForeignKey("FundingWallet")]
        public Guid? FundingWalletId { get; set; }
        public eFundingWallet? FundingWallet { get; set; }

        [ForeignKey("SpotWallet")]
        public Guid? SpotWalletId { get; set; }
        public eSpotWallet? SpotWallet { get; set; }

        [ForeignKey("HoldingWallet")]
        public Guid? HoldingWalletId { get; set; }
        public eHoldingWallet? HoldingWallet { get; set; }

        [ForeignKey("EarnWallet")]
        public Guid? EarnWalletId { get; set; }
        public eEarnWallet? EarnWallet { get; set; }

        [ForeignKey("EscrowWallet")]
        public Guid? EscrowWalletId { get; set; }
        public eEscrowWallet? EscrowWallet { get; set; }

    }
    public class eMultiFactorStatus : secBaseEntity1
    {
        public Guid MultiFactorStatusId { get; set; }
        public bool ActionIsEnabled { get; set; }
        public eUserAccount UserAccount { get; set; }

    }
    [Index("myCommunity", IsUnique = true)]
    public class eRefCodes
    {
        /// <summary>
        /// Code that is Used to create this Account
        /// </summary>
        public string? RefferedBy { get; set; }
        /// <summary>
        /// Code that will be used to Create Community of this User
        /// </summary>
        public string myCommunity { get; set; }
        /// <summary>
        /// Reward Earned by this User 
        /// </summary>
       // public eRefReward? myRefReward { get; set; }
        public bool myRefRewardProcessed { get; set; }

    }
   
}
