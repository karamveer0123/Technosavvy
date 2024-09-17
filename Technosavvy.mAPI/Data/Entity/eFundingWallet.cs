namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    public class eFundingWallet : secBaseEntity1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FundingWalletId { get; set; }
        public Guid InternalAccountNumber { get; set; }
        // public List<eNetworkWalletAddress> NetworkWalletAddr { get; set; }//Specific to This User Wallet for Incomeing Funds Corelation
        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }
        public eUserAccount UserAccount { get; set; }

        public DateTime StartedOn { get; set; } = DateTime.UtcNow;//Date time when this Wallet was created
        public DateTime? LastActedOn { get; set; }//Last Trade Time
        public List<eFundingWBalance> FundingWBalance { get; set; }
        public List<eFundingNetworkWallet> myNetworkWallets { get; set; }

        //public List<eNetworkWalletAddress> NetworkWalletAddr { get; set; }//Specific to This User Wallet for Incomeing Funds Corelation
        //public List<eNetworkWalletBalance> AvailableBalance { get; set; }
        //ToDo: Naveen Fiet Balance should also be added here
    }
    // Network Wallet that has been Attached to Funding Wallet for specific Network
    public class eFundingNetworkWallet : secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FundingNetworkWalletId { get; set; }
        public DateTime AllocatedOn { get; set; } = DateTime.UtcNow;

        public eNetworkWalletAllocationMode NetworkWalletAllocationMode { get; set; }
        public DateTime? ShouldDeAllocatedOn { get; set; }

        public string RecordHash { get; set; }

        [ForeignKey("FundingWallet")]
        public Guid FundingWalletId { get; set; }
        public eFundingWallet FundingWallet { get; set; }

        [ForeignKey("NetworkWalletAddress")]
        public Guid NetworkWalletAddressId { get; set; }
        public eNetworkWalletAddress NetworkWalletAddress { get; set; }
    }
    public enum eNetworkWalletAllocationMode
    {
        Preptual, Limited
    }
    public enum eWalletType
    {
        SpotWallet, FundingWallet, EarnWallet, EscrowWallet, None, Internal
    }
    public enum eCurrencyType
    {
        Fiat,//Govt. Issued Standard National Currency, A Bank Account would be needed to hold yjos
        Crypto,// Protocol based Crypto Currency, A wallet would be needed to hold this
        FiatCrypto// Govt. Issued/Back Crypto Tender of their National Currency, A wallet would be needed to hold this
    }
    public enum eNetWorkWalletCategory
    {
        NavExMGlobal,
        NavExMRegional,
        NavExMClient,
        NavExMSmartContract,
        NavExMTemp,
    }
}
