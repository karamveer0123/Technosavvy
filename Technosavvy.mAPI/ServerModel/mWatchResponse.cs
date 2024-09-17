namespace NavExM.Int.Maintenance.APIs.ServerModel
{
    public class mWatchResponse
    {
        public Guid RequestId { get; set; }
        public Guid NetworkWalletAddressId { get; set; }
        public Guid SupportedTokenId { get; set; }
        public double Amount { get; set; }
        public string TransactionId { get; set; }
    }
}

 
