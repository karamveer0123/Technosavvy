namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    /// <summary>
    /// List of Networks that we will support on this exchange for its Coins and Transactions
    /// </summary>
    public class eSupportedNetwork : secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SupportedNetworkId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? NetworkProxy { get; set; }
        public string NativeCurrencyCode { get; set; }
        public string RecordHash { get; set; }
        public bool IsSmartContractEnabled { get; set; }
    }
}
