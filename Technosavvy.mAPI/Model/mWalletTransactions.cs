namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mWalletTransactions
    {
        public Guid TransactionId { get; set; }
        public string wName { get; set; }
        public DateTime Date { get; set; }
        public string Narration { get; set; }
        public Guid TokenId { get; set; }
        public string TokenCode { get; set; }
        public string TokenName { get; set; }
        public bool IsFiatRepresentative { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public bool isFrom { get; set; }
        public bool IsWithInMyWallet { get; set; }
    }
    public class mPreBetaPurchases
    {
        public int id { get; set; }
        public DateTime DateOf { get; set; } = DateTime.UtcNow;
        public double NavCAmountPurchased { get; set; }
        public double NavCUnitRate { get; set; }//in USDT
        public double TokenToUnitRate { get; set; }//in USDT rate 
        public string BuyWith { get; set; }
        public string TranHash { get; set; }
        public string WalletAddress { get; set; }

    }
    public class mPreBetaStats
    {
        public double User24Hrs { get; set; } 
        public double UserTotal { get; set; }
        public double Token24Hrs { get; set; }
        public double TokenTotal { get; set; } 
        public double NavVCurrentPrice { get; set; }
        public int TotalStages { get; set; }
        public int CompletedStages { get; set; }
        public DateTime BetaLiveIn { get; set; }
        public List<string> NavCPriceInfo { get; set; } = new List<string>();
        public List<string> NavCTokenSaleInfo { get; set; } = new List<string>();
    }

}
