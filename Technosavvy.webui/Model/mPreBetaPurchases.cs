namespace TechnoApp.Ext.Web.UI.Model
{
    public class mPreBetaPurchases
    {
        public int id { get; set; }
        public DateTime DateOf { get; set; } = DateTime.UtcNow;
        public double TechnoSavvyAmountPurchased { get; set; }
        public double TechnoSavvyUnitRate { get; set; }//in USDT
        public double TokenToUnitRate { get; set; }//in USDT rate 
        public string BuyWith { get; set; }
        public string TranHash { get; set; }
    }
}
