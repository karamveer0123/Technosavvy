using Microsoft.AspNetCore.Mvc;
using TechnoApp.Ext.Web.UI.Model;
using System.ComponentModel.DataAnnotations;

namespace TechnoApp.Ext.Web.UI.ViewModels
{
    public class vmProfile:vmBase
    {
        public Guid ProfileId { get; set; }
        public Guid UserAccountId { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        public Gender gender { get; set; }
        public string Title { get; set; }
        public string NickName { get; set; }
        [Required(ErrorMessage = "Date Of Birth is required")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Remote("MinDateOfBirth", "KYCUser", HttpMethod = "POST", ErrorMessage = "User must be 18 or above years of age")]
        public DateTime  DateOfBirth { get; set; }
        public List<vmCountry> lstCountries { get; set; }
        [Required(ErrorMessage = "Tax Residency is required")]
        public Guid? TaxResidencyId { get; set; }
        [Required(ErrorMessage = "Country Of Citizenship is required")]
        public Guid? CitizenshipId { get; set; }
        public vmCountry selectedTaxResidency { get; set; }
        public vmCountry selectedCitizenOf { get; set; }
        public vmAddress Address { get; set; }
        public Guid MobileCountryId { get; set; }
        public string Mobile { get; set; }
        public eeKYCStatus KYCStatus{ get; set; }
        public List<mKYCDocRecord> myDocs { get; set; }

    }
    public class vmKYCDocUploadStage : vmBase
    {
        public vmProfile Profile { get; set; }
       // public List<mCategoryDocTemplate> Category { get; set; }
        public List<mCategoryDocFile> DocInstances { get; set; }
    }
    public class mCategoryDocFile
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int PassScore { get; set; }
        public List<mDocFileInstance> DocumentTemplates { get; set; }
        public List<string> CatErrorList { get; set; }
    }
    public class mDocFileInstance
    {
        public Guid PlaceHolderId { get; set; }
        public string PlaceHolderName { get; set; }
        public int PointValue { get; set; }
        public int MaxAllowedFileSize { get; set; }
        public string HelpInfo { get; set; }
        public string? DocNumber { get; set; }
        public bool IsFront { get; set; }
        public bool IsBack { get; set; }
        public IFormFile? Front { get; set; }
        public IFormFile? Back { get; set; }
    }

    //public class mDocInstance
    //{
    //    public Guid PlaceHolderId { get; set; }
    //    public byte[] data { get; set; }
    //    public string FileName { get; set; }
    //    public string Ext { get; set; }
    //    public int FileSize { get; set; }
    //    public bool IsFront { get; set; }
    //    public bool IsBack { get; set; }
    //}
   
  
    //public class mPlaceHolderCategory
    //{
    //    public Guid Id { get; set; }
    //    public string Name { get; set; }
    //    public string HelpInfo{ get; set; }
    //    public string Desc { get; set; }
    //    public int PassScore { get; set; }
    //    public List<mPlaceHolder> PlaceHolders { get; set; }
    //}
    //public class mPlaceHolder
    //{
    //    public Guid Id { get; set; }
    //    public string Name { get; set; }
    //    public string HelpInfo { get; set; }
    //    public string Desc { get; set; }
    //    public int ScoreValue { get; set; }
    //    public bool IsFront { get; set; }
    //    public bool IsBack { get; set; }
    //    public int MaxSize { get; set; }
    //}
}
