namespace TechnoApp.Ext.Web.UI.Model
{
    public class mWalletTransactions
    {
        public Guid TransactionId { get; set; }
        public string wName { get; set; }
        public DateTime Date { get; set; }
        public string Narration { get; set; }
        public Guid TokenId { get; set; }
        public string TokenCode { get; set; }
        public bool IsFiatRepresentative { get; set; }
        public string TokenName { get; set; }
        public double Amount { get; set; }
        public double Balance{ get; set; }
        public bool isFrom { get; set; }
        public bool IsWithInMyWallet { get; set; }
    }
}
