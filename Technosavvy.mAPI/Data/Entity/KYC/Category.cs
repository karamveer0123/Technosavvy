using System.ComponentModel.DataAnnotations;
namespace NavExM.Int.Maintenance.APIs.Data.Entity.KYC;

public class eCategory : BaseEntity2
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CategoryId { get; set; }
    [StringLength(250)]
    public string CountryName { get; set; }
    [StringLength(6)]
    public string CountryAbbr { get; set; }
    [StringLength(250)]
    public string CategoryName { get; set; }
    public string? CategoryDesc { get; set; }
    public int PassScore { get; set; }
    public AuthStatus ApprovalStatus { get; set; }

    public List<eDocumentTemplate> DocumentTemplates { get; set; }
    public bool IsSuspended { get; set; }
    public DateTime? SuspendedOn { get; set; }
}
public class eDocumentTemplate : BaseEntity2
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid DocumentTemplateId { get; set; }
    public string DocTemplateName { get; set; }
    public string DocTemplateDescription { get; set; }
    public string DocTemplateHelpInfo { get; set; }
    public bool IsFrontRequired { get; set; }
    public bool IsBackRequired { get; set; }
    public int Score { get; set; }// Score Value of this Template Associated Document Instance
    public int DocInstanceSize { get; set; }//File Seize Allowed
    public AuthStatus ApprovalStatus { get; set; }

    [ForeignKey(nameof(Category))]
    public Guid CategoryId { get; set; }
    public eCategory Category { get; set; }
}
//public class eDocumentInstance : BaseEntity2
//{
//    [Key]
//    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//    public Guid DocumentInstanceId { get; set; }
//    [ForeignKey(nameof(DocumentTemplate))]
//    public Guid DocumentTemplateId { get; set; }
//    public eDocumentTemplate DocumentTemplate { get; set; }
//    public string FAccount { get; set; }
//    public Guid UserAccountId { get; set; }
//    public byte[] data { get; set; }
//    public string FileName { get; set; }
//    public string Ext { get; set; }
//    public int FileSize { get; set; }

//}
[Index(nameof(ProfileId))]
[Index(nameof(CategoryId))]
[Index(nameof(PlaceHolderId))]
public class eKYCDocAdminRecord : BaseEntity2
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

    public string data { get; set; } 
    [StringLength(10)]
    public string Ext { get; set; }
    [StringLength(1000)]
    public string? Location { get; set; }
    public int DocFileSize { get; set; }
    public bool IsFront { get; set; }
    public bool IsBack { get; set; }
    public eDocumentStatus Status { get; set; }

    public Guid UserAccountId { get; set; }
    public Guid ProfileId { get; set; }
}
public enum eDocumentStatus
{
    Submitted,
    Accepted,
    Rejected
}
public enum AuthStatus
{
    Proposed = 0, Accepted = 1, Rejected = 2
}
