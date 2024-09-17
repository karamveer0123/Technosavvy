namespace TechnoApp.Ext.Web.UI.Model;

public class mAddress
{
    public Guid AddressId { get; set; }
    public Guid UserAccountId { get; set; }
    public string UnitNo { get; set; }
    public string StreetAdd { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostCode { get; set; }
    public Guid CountryId { get; set; }
    //public mCountry Country { get; set; }
}
public class mWithdrawRequestResult
{
    public CryptoWithdrawRequestStatus status { get; set; }
    public string PublicRequestId { get; set; }
    public double Amount { get; set; }
    public string TokenCode { get; set; }
    public Guid CryptoWithdrawRequestId { get; set; }
}
public class mCryptoWithdrawRequest
{
    public Guid CryptoWithdrawRequestId { get; set; }
    public string PublicRequestID { get; set; }
    public string uAccount { get; set; }
    public string TaxResidencyCountryName { get; set; }
    public string TaxResidencyCountryCode { get; set; }
    public string KYCStatus { get; set; }
    public Guid TokenId { get; set; }
    public string TokenCode { get; set; }
    public string TokenContractAddress { get; set; } = "??";
    public Guid NetworkId { get; set; }
    public string NetworkName { get; set; }
    public bool IsAll { get; set; }
    public double Amount { get; set; }
    public double Charges { get; set; }
    /// <summary>
    /// Token Reservation Transaction ID
    /// </summary>
    public Guid RequestRefId { get; set; }
    /// <summary>
    /// Network Transaction Receipt String
    /// </summary>
    public string? TransactionReceipt { get; set; } = "??";
    public DateTime RequestedOn { get; set; }
    public DateTime CompletedOn { get; set; } = DateTime.MinValue;

    public string ReceiverAddress { get; set; }
    /// <summary>
    /// Status History of this Transaction
    /// </summary>
    public List<mWithdrawlRequestStatus> Status { get; set; }
    public string? GEOInfo { get; set; }

}
public class mFiatWithdrawRequest
{
    public Guid FiatWithdrawRequestId { get; set; }
    public string PublicRequestID { get; set; }
    public string uAccount { get; set; }
    public string TaxResidencyCountryName { get; set; }
    public string TaxResidencyCountryCode { get; set; }
    public string KYCStatus { get; set; }
    public string CurrencyCode { get; set; }
    public string CurrencySymbole { get; set; }
    public double Amount { get; set; }
    public double Charges { get; set; }
    /// <summary>
    /// Token Reservation Transaction ID
    /// </summary>
    public Guid RequestRefId { get; set; }
    public DateTime RequestedOn { get; set; }
    public DateTime CompletedOn { get; set; }

    //Serilized data of any object suitable for the desired curreny/country
    public string ReceiverBankDetails { get; set; }
    public string Narration { get; set; }

    public List<mWithdrawlRequestStatus> Status { get; set; }

    public string? GEOInfo { get; set; }
}
public class mFiatDepositIntimation
{
    public Guid FiatDepositRequestId { get; set; }
    public string PublicRequestID { get; set; }
    public string uAccount { get; set; }
    public string TaxResidencyCountryName { get; set; }
    public string TaxResidencyCountryCode { get; set; }
    public string KYCStatus { get; set; }
    public string CurrencyCode { get; set; }
    public string CurrencySymbole { get; set; }
    public double Amount { get; set; }
    public double Charges { get; set; }
    public DateTime RequestedOn { get; set; }
    //Serilized data of any object suitable for the desired curreny/country
    public string TechnoAppBankDetails { get; set; }
    public string SenderBankDetails { get; set; }
    //IFormFile data in base64 String
    public string DepositEvidence { get; set; }

    public List<mWithdrawlRequestStatus> Status { get; set; }
    public string? GEOInfo { get; set; }
}
public class mWithdrawlRequestStatus
{
    public Guid eWithdrawlRequestStatusId { get; set; }
    public  RequestStatus PublicStatus { get; set; }
    public  RequestInternalStatus InternalStatus { get; set; }
    public string? Remarks { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
}

public class mConvertTokenRequest
{
    public Guid TradeId { get; set; }
    public Guid TransactionId { get; set; }
    public Guid fromTokenId { get; set; }
    public string qCode { get; set; }
    public string bCode { get; set; }
    public Guid toTokenId { get; set; }
    /// <summary>
    /// Token Amount that is used to pay with
    /// </summary>
    public double fromAmt { get; set; }
    /// <summary>
    /// Token Amount that is purchased or intended to purchased
    /// </summary>
    public double toTokenAmt { get; set; }
    public double RateOfOneToToken { get; set; }
    public bool IsFundWalletAllowed { get; set; }
    public bool IsSpotWalletAllowed { get; set; }
    public bool IsError { get; set; }
    public string ErrMsg { get; set; }
}
public enum RequestStatus
{
    Submitted = 0,
    Hold = 1,
    UnderProcess = 2,
    Rejected = 3,
    Completed = 4,
    Cancelled = 5,
}
public enum RequestInternalStatus
{
    Submitted = 0,
    AccountAcknowledged,
    AccountApproved,
    AccountRejected,
    AccountHold,
    FundApproved,
    FundRejected,
    FundHold,
    FundProcessCompleted
}
public enum CryptoWithdrawRequestStatus
{
    Failed=0,//Incase of any error
    Reserved,//reserve Transaction is completed on the Request
    Rejected,//reserve Transaction is rejected
    Completed// external Payments are made
}