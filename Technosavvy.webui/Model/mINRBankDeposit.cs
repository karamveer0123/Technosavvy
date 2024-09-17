namespace TechnoApp.Ext.Web.UI.Model
{
    //User Bank Account for INR Deposit
    public class mINRBankDeposit
    {
        public Guid ID { get; set; }
        public string MethodName { get; set; } = "BankDeposit";
        public string SupportCurrency { get; set; } = "INR";
        public string IFSCCode { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }
        public string Bank { get; set; }
        public string BranchAddress { get; set; }
        public DateTime Createdon { get; set; } = DateTime.UtcNow;
        public DateTime Modifiedon { get; set; }
        public DateTime DeletedOn { get; set; }
        public Guid ProfileId { get; set; }
        public Guid TokenId { get; set; }
    }
}
