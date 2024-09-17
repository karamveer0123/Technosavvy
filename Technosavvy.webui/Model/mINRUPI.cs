namespace TechnoApp.Ext.Web.UI.Model
{
    public class mINRUPI
    {
        public Guid ID { get; set; }
        public string MethodName { get; set; } = "UPI";
        public string SupportCurrency { get; set; } = "INR";
        public string UPIid { get; set; }
        public string AccountHolderName { get; set; }
        public string QRCode { get; set; }

        public DateTime Createdon { get; set; }
        public DateTime Modifiedon { get; set; }
        public DateTime DeletedOn { get; set; }
        public Guid ProfileId { get; set; }
        public Guid TokenId { get; set; }
    }
}
