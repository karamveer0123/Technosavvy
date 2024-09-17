namespace TechnoApp.Ext.Web.UI.Model
{
    public class mUser
    {
        public string Id { get; set; }
        public mProfile Profile { get; set; }
        public string AccountNumber { get; set; }
        public mRefCodes RefCodes { get; set; }
        public mEmail Email { get; set; }
        public bool IsMultiFactor { get; set; }
        public mTaxResidency TaxResidency { get; set; }
        public mCitizenOf CitizensOf { get; set; }
        public mMobile Mobile { get; set; }
        public mAuthenticator Authenticator { get; set; }
        public bool IsPrimaryCompleted { get; set; }
        public bool IsActive { get; set; }
    }
    public class mRefCodes
    {
        public string? RefferedBy { get; set; }
        public string myCommunity { get; set; }
    }
}
