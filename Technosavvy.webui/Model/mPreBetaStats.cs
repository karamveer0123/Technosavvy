namespace TechnoApp.Ext.Web.UI.Model
{
    public class mPreBetaStats
    {
        public string userAccount { get; set; }
        public double User24Hrs { get; set; } = 1;
        public double UserTotal { get; set; } = 1;
        public double Token24Hrs { get; set; } = 1;
        public double TokenTotal { get; set; } = 1;
        public double NavVCurrentPrice { get; set; } = 1;
        public DateTime BetaLiveIn { get; set; }
        public int TotalStages { get; set; }
        public int CompletedStages { get; set; }
        public List<string> TechnoSavvyPriceInfo { get; set; } = new List<string>();
        public List<string> TechnoSavvyTokenSaleInfo { get; set; } = new List<string>();
    }
}
