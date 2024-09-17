namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    //ToDo:, Naveen, This Class Purposes is for External Network Wallet, Transaction, Balance Related
    public class eNetworkWalletBalance : secBaseEntity1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid NetworkWalletBalanceId { get; set; }
        public double Balance { get; set; }

        [ForeignKey("NetworkWalletAddress")]
        public Guid NetworkWalletAddressId { get; set; }
        public eNetworkWalletAddress NetworkWalletAddress { get; set; }

        [ForeignKey("Token")]
        public Guid TokenId { get; set; }
        public eToken Token { get; set; }

        /// <summary>
        /// {NetworkWalletBalanceId+Balance+NetworkAddressId+TokenId+SessionHash}
        /// </summary>
        public string RecordHash { get; set; }
    }
}
