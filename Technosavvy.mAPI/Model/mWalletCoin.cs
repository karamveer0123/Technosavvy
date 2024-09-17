namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mWalletCoin
    {
        public Guid CoinId { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Code { get; set; }
        public bool IsFiatRepresentative { get; set; }
        public double Amount { get; set; }
        public string IconLocation { get; set; }
        public DateTime LastTransactionOn { get; set; }
        public Guid? LastChangeAgent { get; set; }//Last Transaction
    }
}
