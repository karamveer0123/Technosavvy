namespace TechnoApp.Ext.Web.UI.Model
{
    public class mKYCRecord
    {
        public Guid KYCRecordId { get; set; }
        public mUser UserAccount { get; set; }
        public mCountry CountryOfCitizenship { get; set; }
        public mCountry CountryOfResidence { get; set; }
        public string TaxIdentificationNumber { get; set; }
        public mEnumBoxData TaxIDNType { get; set; }
        public mEnumBoxData KYCStatus { get; set; }
        public bool IsDeclarationAccepted { get; set; }
        public bool IsCompleted { get; set; }
        public List<mKYCDocRecord> KYCDocuments { get; set; }
    }
}
