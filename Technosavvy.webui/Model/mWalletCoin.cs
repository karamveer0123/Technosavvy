namespace TechnoApp.Ext.Web.UI.Model
{
    public class mWalletCoin
    {
        public Guid CoinId { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public bool IsFiatRepresentative { get; set; }
        public string Code { get; set; }
        public double Amount { get; set; }
        public string IconLocation { get; set; }
        public DateTime LastTransactionOn { get; set; }
    }
    public class mCoinInWallet
    {
        public mWalletCoin SpotWallet { get; set; }
        public mWalletCoin FundWallet { get; set; }
        public mWalletCoin EarnWallet { get; set; }
    }
}
