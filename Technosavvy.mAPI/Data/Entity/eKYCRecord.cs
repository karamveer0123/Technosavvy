namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    //Entity to keep KYC Records of a User. If this information is to be change, Current Record would be deleted for Historic reasons and new will be created
    //public class eKYCRecord:secBaseEntity2
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public Guid eKYCRecordId { get; set; }
    //    public eUserAccount UserAccount { get; set; }
        
    //    [ForeignKey("CountryOfCitizenship")]
    //    public Guid CountryOfCitizenshipId { get; set; }
    //    public eCountry CountryOfCitizenship { get; set; }
       
    //    [ForeignKey("CountryOfResidence")]
    //    public Guid CountryOfResidenceId { get; set; }
    //    public eCountry CountryOfResidence { get; set; }
       
    //    [StringLength(1000)]
    //    public string? TaxIdentificationNumber { get; set; }
    //    //TaxIdentificationNumber Type, we are catering every country
    //    //[ForeignKey("TaxIDNType")]
    //    //public Guid TaxIDNTypeId { get; set; }
    //    //public eEnumBoxData TaxIDNType { get; set; }
    //    public List<eKYCDocRecord> KYCDocuments { get; set; }
    //   //Status from Enum since it can have many staged based on country
    //    //[ForeignKey("KYCStatus")]
    //    //public Guid KYCStatusId { get; set; }
    //    //public eEnumBoxData KYCStatus { get; set; }
    //    public bool IsDeclarationAccepted { get; set; }
    //    //simple flag to assess permission for certain action
    //    public bool IsCompleted { get; set; }


    //}
    
   
}
