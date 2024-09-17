namespace NavExM.Int.Maintenance.APIs.ServerModel
{
    public class mWatchRequest
    {
        public Guid RequestId { get; set; }
        public string TokenContractAddress { get; set; }
        public string TokenName { get; set; }
        public string WalletAddress { get; set; }
        public double ExpectedAmount { get; set; }
        public Guid NetworkWalletAddressId { get; set; }
        public Guid SupportedTokenId { get; set; }
    }
}
