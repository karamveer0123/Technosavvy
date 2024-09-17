namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    //ToDo:, Naveen, This Class Purposes is for External Network Wallet Address, Transaction, Balance Related
    [Index(nameof(Address), nameof(NetworkId), IsUnique = true)]
    [Index(nameof(Address))]
    public class eNetworkWalletAddress : secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid NetworkWalletAddressId { get; set; }
        [StringLength(1000)]
        public string Notes { get; set; }

        [StringLength(1000)]
        public string Address { get; set; }
        public eNetWorkWalletCategory Category { get; set; }

        [ForeignKey("Network")]
        public Guid NetworkId { get; set; }
        public eSupportedNetwork Network { get; set; }


        public List<eNetworkWalletAddressWatch> AddressWatch { get; set; }
        /// <summary>
        /// {Id+NetworkName+CreatedOn+Address+SessionHash}
        /// </summary>
        public string RecordHash { get; set; }
    }
    //This Class is not required in actual implementation network Transaction since Logic flow has been changed.
    public class eNetworkWalletAddressWatch : secBaseEntity3
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid NetworkWalletAddressWatchId { get; set; }

        [ForeignKey("NetworkWalletAddress")]
        public Guid NetworkWalletAddressId { get; set; }
        public eNetworkWalletAddress NetworkWalletAddress { get; set; }

        [ForeignKey("SupportedToken")]
        public Guid SupportedTokenId { get; set; }
        public eSupportedToken SupportedToken { get; set; }
        public double ExpectedAmount { get; set; }
        public string? NetworkTransactionId { get; set; }
        public DateTime? NetworkTransExecutionTime { get; set; }
        public string? InternalTransactionId { get; set; }
        public DateTime? InternalTransExecutionTime { get; set; }
    }


}
