namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    [Index(nameof(ProfileId))]
    [Index(nameof(CategoryId))]
    [Index(nameof(PlaceHolderId))]
    public class eKYCDocRecord:secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid eKYCDocRecordId { get; set; }
        public Guid MatchId { get; set; }
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }//Other KYC Admin database
        public string PlaceHolderName { get; set; }
        public Guid PlaceHolderId { get; set; }//Other KYC Admin database
        public string CountryAbbrivation { get; set; }
        [StringLength(100)]
        public string PublicName { get; set; }
        [StringLength(100)]
        public string InternalName { get; set; }
        [StringLength(10)]
        public string Ext { get; set; }
        [StringLength(1000)]
        public string? Location { get; set; }
        public int DocFileSize { get; set; }
        public bool IsFront { get; set; }
        public bool IsBack { get; set; }
        public eDocumentStatus Status { get; set; }
       
        public Guid ProfileId { get; set; }
    }
    public enum eDocumentStatus
    {
        Submitted,
        Accepted,
        Rejected
    }
}
