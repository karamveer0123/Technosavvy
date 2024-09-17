namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mToken
    {
        public Guid TokenId { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public List<mSupportedToken> SupportedCoin { get; set; }
        public double Tick { get; set; }
        public string? Category { get; set; }//todo: Changedb
        public string Details { get; set; }
        public bool IsFiatRepresentative { get; set; }
        public int WatchList { get; set; }
        public int FavList { get; set; }
        public string? WebURL { get; set; }
        public string? WhitePaper { get; set; }
        public mTokenAttribute Attr { get; set; }
        //We may not use these Numbers in case of NavExM only information is to displayed
        public double MarketCap { get; set; }
        public double FDMarketCap { get; set; }
        public double CirculatingSupply { get; set; }
        public double Volumn { get; set; }
        public List<mSupportedCountry> AllowedCountries { get; set; }
    }
    public class mTokenAttribute
    {
        public string WebURL { get; set; }
        public string WhitePaperURL { get; set; }
        public double PreviousMarketCap { get; set; }
        public double CurrentMarketCap { get; set; }
        public double PreviousCirculatingSupply { get; set; }
        public double CurrentCirculatingSupply { get; set; }
        public List<mTokenAggregator> MyAggrigators { get; set; }
    }
    public class mTokenAggregator
    {
        public string AggrigatorName { get; set; }
        public string AggrigatorURL { get; set; }
        public string AggrigatorTokenCode { get; set; }
        public DateTime AggrigatorReportedOn { get; set; }
    }
    public class mTokenNetworkFee 
    {
        public mToken Token { get; set; }
        public mSupportedNetwork SupportedNetwork { get; set; }
        public double MaxWithdrawal { get; set; }
        public double MinWithdrawal { get; set; }
        public double DepositFee { get; set; }
        public double WithdrawalFee { get; set; }
        public bool IsDepositAllowed { get; set; }
        public bool IsWithdrawalAllowed { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
    public class mFiatCurrency
    {
        public Guid FiatCurrencyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }//INR USD AUD
        public string Symbole { get; set; }//₹ $
        public double Tick { get; set; }
        public List<mFiatProfile> Profiles { get; set; }
    }
    public class mFiatProfile
    {
        public Guid FiatProfileId { get; set; }
        public mCountry CountryOrigin { get; set; }
        public bool IsExchangeAllowed { get; set; }//if This Country allows crypto exchange to trade using their Fiat
        public bool IsP2PAllowed { get; set; }//if This Country allows crypto exchange to trade using their Fiat
        public List<mBankAccount> BankAccounts { get; set; }
    }
    public class mBankAccount
    {
        public Guid BankAccountId { get; set; }
        public Guid BankAccountWallet { get; set; } // for internal Transaction Only
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AdditionalInfo { get; set; }
        public string BranchAddress { get; set; }
        public mCountry LocatedAt { get; set; }
        public List<IPaymentMethod> PaymentMethod { get; set; }
    }
    
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
        public Guid TokenId{ get; set; }
    }
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
       // public string QRCode { get; set; }
        public DateTime Createdon { get; set; } = DateTime.UtcNow;
        public DateTime Modifiedon { get; set; }
        public DateTime DeletedOn { get; set; }
        public Guid ProfileId { get; set; }
        public Guid TokenId { get; set; }
    }

    public class mUPIPaymentMethod : IPaymentMethod
    {
        public Guid ID { get; set; }
        public string MethodName { get; set; } = "UPI";
        public string SupportCurrency { get; set; } = "INR";
        public string UPIid { get; set; }
        public string AccountHolderName { get; set; }
        public string QRCode { get; set; }
        //deposit
        public double MinDeposit { get; set; }
        public double MaxDeposit { get; set; }
        public double DepositFee { get; set; }
        public bool IsDepositAllowed { get; set; }
        public DateTime DepositStartDate { get; set; } = DateTime.UtcNow;
        //Withdrawl
        public double MinWithdrawl { get; set; }
        public double MaxWithdrawl { get; set; }
        public double WithdrawlFee { get; set; }
        public bool IsWithDrawlAllowed { get; set; }
        public DateTime WithDrawlStartDate { get; set; } = DateTime.UtcNow;

        public DateTime Createdon { get; set; } = DateTime.UtcNow;
        public DateTime Modifiedon { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class mBankDepositPaymentMethod : IPaymentMethod
    {
        public Guid ID { get; set; }
        public string MethodName { get; set; } = "BankDeposit";
        public string SupportCurrency { get; set; } = "INR";
        public string IFSCCode { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }
        public string Bank { get; set; }
        public string BranchAddress { get; set; }
        public string QRCode { get; set; }
        //deposit
        public double MinDeposit { get; set; }
        public double MaxDeposit { get; set; }
        public double DepositFee { get; set; }
        public bool IsDepositAllowed { get; set; }
        public DateTime DepositStartDate { get; set; } = DateTime.UtcNow;
        //Withdrawl
        public double MinWithdrawl { get; set; }
        public double MaxWithdrawl { get; set; }
        public double WithdrawlFee { get; set; }
        public bool IsWithDrawlAllowed { get; set; }
        public DateTime WithDrawlStartDate { get; set; } = DateTime.UtcNow;

        public DateTime Createdon { get; set; } = DateTime.UtcNow;
        public DateTime Modifiedon { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
    public interface IPaymentMethod
    {
        string MethodName { get; set; }
        string SupportCurrency { get; set; }
    }
}
