namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mNetworkWallet
    {
        public Guid NetworkId { get; set; }
        public Guid NetworkWalletId { get; set; }
        public string NetworkName { get; set; }
        public DateTime AllocateOn { get; set; }
        public string Address { get; set; }
        public WalletType WType { get; set; }

    }
    public enum WalletType
    {
        Standard, SmartContract
    }
}
