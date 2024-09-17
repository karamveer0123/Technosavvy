using System.Collections.Concurrent;
using System.Net.NetworkInformation;

namespace NavExM.Int.Maintenance.APIs.Model;

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
public class mCryptoWithdrawRequestResult
{
    public Data.Entity.Fund.CryptoWithdrawRequestStatus status { get; set; }
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
    public string TokenContractAddress { get; set; }
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
    public string? TransactionReceipt { get; set; }
    public DateTime RequestedOn { get; set; }
    public DateTime CompletedOn { get; set; }

    public string ReceiverAddress { get; set; }
    /// <summary>
    /// Status History of this Transaction
    /// </summary>
    public List<mWithdrawlRequestStatus> Status { get; set; }
    public string? GEOInfo { get; set; }

}
public class mFiatWithdrawRequest
{
    public Guid eFiatWithdrawRequestId { get; set; }
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
    /// <summary>
    /// Token Reservation Transaction ID
    /// </summary>
    public Guid RequestRefId { get; set; }
    public DateTime CompletedOn { get; set; }

    //Serilized data of any object suitable for the desired curreny/country
    public string ReceiverBankDetails { get; set; }
    public string Narration { get; set; }

    public List<mWithdrawlRequestStatus> Status { get; set; }

    public string? GEOInfo { get; set; }
}
public class mFiatDepositIntimation
{
    public Guid eFiatDepositRequestId { get; set; }
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
    public string NavExMBankDetails { get; set; }
    public string SenderBankDetails { get; set; }
    //IFormFile data in base64 String
    public string DepositEvidence { get; set; }

    public List<mWithdrawlRequestStatus> Status { get; set; }
    public string? GEOInfo { get; set; }
}
public class mWithdrawlRequestStatus
{
    public Guid eWithdrawlRequestStatusId { get; set; }
    public Data.Entity.Fund.RequestStatus PublicStatus { get; set; }
    public Data.Entity.Fund.RequestInternalStatus InternalStatus { get; set; }
    public string? Remarks { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
}


public enum mOnDemandRequestResult
{
    Placed = 1,
    NoIssue = 2,
    DailyLimitIssue = 4,
    TotalLimitIssue = 8,
    AlreadyClaimed = 16,
    AlreadyAwaited = 32
}
