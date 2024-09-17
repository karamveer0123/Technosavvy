namespace NavExM.Int.Maintenance.APIs.Data.Entity.Fund;

public class eFiatDepositRequest : BaseEntity2
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid FiatDepositRequestId { get; set; }
    public byte[] RowVersion { get; set; }

    [StringLength(100)]
    public string PublicRequestID { get; set; }
    public string uAccount { get; set; }
    [StringLength(100)]
    public string? TaxResidencyCountryName { get; set; }
    [StringLength(10)]
    public string? TaxResidencyCountryCode { get; set; }
    [StringLength(100)]
    public string KYCStatus { get; set; }
    [StringLength(10)]
    public string CurrencyCode { get; set; }
    [StringLength(10)]
    public string CurrencySymbole { get; set; }
    public double Amount { get; set; }
    public double Charges { get; set; }
    public DateTime RequestedOn { get; set; }
    //Serilized data of any object suitable for the desired curreny/country
    public string NavExMBankDetails { get; set; }
    public string SenderBankDetails { get; set; }
    //IFormFile data in base64 String
    public string DepositEvidence { get; set; }

    public List<eWithdrawlRequestStatus> Status { get; set; }
    public string? GEOInfo { get; set; }
}
public class eFiatWithdrawRequest : BaseEntity2
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid FiatWithdrawRequestId { get; set; }
    public byte[] RowVersion { get; set; }

    [StringLength(100)]
    public string PublicRequestID { get; set; }
    public string uAccount { get; set; }
    [StringLength(100)]
    public string TaxResidencyCountryName { get; set; }
    [StringLength(10)]
    public string TaxResidencyCountryCode { get; set; }
    [StringLength(100)]
    public string KYCStatus { get; set; }
    [StringLength(10)]
    public string CurrencyCode { get; set; }
    [StringLength(10)]
    public string CurrencySymbole { get; set; }
    public double Amount { get; set; }
    public double Charges { get; set; }
    public DateTime RequestedOn { get; set; }
    /// <summary>
    /// Token Reservation Transaction ID
    /// </summary>
    public Guid RequestRefId { get; set; }
    public DateTime? CompletedOn { get; set; }

    //Serilized data of any object suitable for the desired curreny/country
    public string ReceiverBankDetails { get; set; }
    public string Narration { get; set; }

    public List<eWithdrawlRequestStatus> Status { get; set; }
    public string? GEOInfo { get; set; }
}
public class eCryptoWithdrawRequest : BaseEntity2
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CryptoWithdrawRequestId { get; set; }
    public byte[] RowVersion { get; set; }

    [StringLength(100)]
    public string PublicRequestID { get; set; }
    public string uAccount { get; set; }
    [StringLength(100)]
    public string TaxResidencyCountryName { get; set; }
    [StringLength(10)]
    public string TaxResidencyCountryCode { get; set; }
    [StringLength(100)]
    public string KYCStatus { get; set; }
    public Guid TokenId { get; set; }
    [StringLength(50)]
    public string TokenCode { get; set; }
    public string TokenContractAddress { get; set; }
    public Guid NetworkId { get; set; }
    [StringLength(100)]
    public string NetworkName { get; set; }
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
    public DateTime? CompletedOn { get; set; }

    public string ReceiverAddress { get; set; }
    /// <summary>
    /// Status History of this Transaction
    /// </summary>
    public List<eWithdrawlRequestStatus> Status { get; set; }
    public string? GEOInfo { get; set; }
}
public class eWithdrawlRequestStatus : BaseEntity2
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid eWithdrawlRequestStatusId { get; set; }
    public RequestStatus PublicStatus { get; set; }
    public RequestInternalStatus InternalStatus { get; set; }
    public string? Remarks { get; set; }
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
    Failed = 0,//Incase of any error
    Reserved,//reserve Transaction is completed on the Request
    Rejected,//reserve Transaction is rejected
    Completed// external Payments are made
}