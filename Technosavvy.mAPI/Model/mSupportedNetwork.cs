namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mSupportedNetwork
    {
        public Guid SupportedNetworkId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string NativeCurrencyCode { get; set; }
        public bool IsSmartContractEnabled { get; set; }
    }
}
