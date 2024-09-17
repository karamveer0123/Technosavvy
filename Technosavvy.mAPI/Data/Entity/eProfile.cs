using System.Security.Cryptography.Xml;

namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    public class eProfile : secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProfileId { get; set; }
        
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? NickName { get; set; }
        public  Gender gender { get; set; }
        public string? Title { get; set; }

        public DateTime ?DateOfBirth { get; set; }
        public eeKYCStatus KYCStatus { get; set; }
        
        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }
        public eUserAccount UserAccount { get; set; }
        
        [ForeignKey("CurrentAddress")]
        public Guid? AddressId { get; set; }
        public eAddress CurrentAddress { get; set; }
        [NotMapped]
        public string Imprint { get {
                return $"{FirstName}{LastName}{(DateOfBirth.HasValue?DateOfBirth.Value:"")}{UserAccountId}{(AddressId.HasValue?AddressId.Value:"")}";
            } }
    }
    public enum Gender
    {
        NotSpecified,
        Male,
        Female,
        Other
    }
    [Index(nameof(UPIid))]
    public class eUPIPaymentMethod : secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        [StringLength(500)]
        public string MethodName { get; set; } = "UPI";
        [StringLength(500)]
        public string SupportCurrency { get; set; } = "INR";
        [StringLength(500)]
        public string UPIid { get; set; }
        [StringLength(500)]
        public string AccountHolderName { get; set; }
        public string? QRCode { get; set; }

        [ForeignKey(nameof(profile))]
        public Guid profileId { get; set; }
        public eProfile profile { get; set; }

        [ForeignKey(nameof(token))]
        public Guid tokenId { get; set; }
        public eToken token { get; set; }
    }
    [Index(nameof(IFSCCode))]
    public class eBankDepositPaymentMethod : secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        [StringLength(500)]
        public string MethodName { get; set; } = "BankDeposit";
        [StringLength(500)]
        public string SupportCurrency { get; set; } = "INR";
        [StringLength(500)]
        public string IFSCCode { get; set; }
        [StringLength(500)]
        public string AccountHolderName { get; set; }
        [StringLength(500)]
        public string AccountNumber { get; set; }
        [StringLength(500)]
        public string Bank { get; set; }
        [StringLength(500)]
        public string BranchAddress { get; set; }
        
        
        [ForeignKey(nameof(profile))]
        public Guid profileId { get; set; }
        public eProfile profile { get; set; }

        [ForeignKey(nameof(token))]
        public Guid tokenId { get; set; }
        public eToken token { get; set; }
    }
}
