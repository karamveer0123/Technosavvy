namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mKYCDocRecord
    {
        public Guid KYCDocRecordId { get; set; }
        //This Document belongs to KYC Record
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }//Other KYC Admin database
        public string PlaceHolderName { get; set; }
        public Guid PlaceHolderId { get; set; }//Other KYC Admin database
        public string CountryAbbrivation { get; set; }
        public string PublicName { get; set; }
        public string InternalName { get; set; }
        public string Ext { get; set; }
        public string? Location { get; set; }
        public int DocFileSize { get; set; }
        public bool IsFront { get; set; }
        public bool IsBack { get; set; }
        public eDocumentStatus Status { get; set; }
        public Guid ProfileId { get; set; }
        public DateTime CreatedOn { get; set; }

        public Guid UserAccountId { get; set; }
        public string data { get; set; }

        //  public byte[]? Content { get; set; }
    }
}
