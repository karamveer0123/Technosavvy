namespace TechnoApp.Ext.Web.UI.Model;

public class mDocumentTemplate
{
    public Guid DocumentTemplateId { get; set; }
    public string? DocTemplateName { get; set; }
    public string? DocTemplateDescription { get; set; }
    public string? DocTemplateHelpInfo { get; set; }
    public bool IsFrontRequired { get; set; }
    public bool IsBackRequired { get; set; }
    public int? Score { get; set; }// Score Value of this Template Associated Document Instance
    public int? DocInstanceSize { get; set; }//File Seize Allowed
    public AuthStatus ApprovalStatus { get; set; }
    public Guid CategoryId { get; set; }
}
public class mCategoryDocTemplate
{
    public Guid CategoryId { get; set; }
    public string CountryName { get; set; }
    public string CountryAbbr { get; set; }
    public string CategoryName { get; set; }
    public string? CategoryDesc { get; set; }
    public int PassScore { get; set; }
    public AuthStatus ApprovalStatus { get; set; }
    public List<mDocumentTemplate> DocumentTemplates { get; set; }
}
public enum AuthStatus
{
    Proposed = 0, Accepted = 1, Rejected = 2
}