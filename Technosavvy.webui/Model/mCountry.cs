namespace TechnoApp.Ext.Web.UI.Model
{
    public class mCountry {
        public Guid CountryId { get; set; }
        public string DialCode { get; set; }
        public string Name { get; set; }
        public string Abbrivation { get; set; }
        public string Continent { get; set; } //Africa, America
        public string Block { get; set; }//Asia, central America..
    }
    public class mSupportedCountry
    {
        public Guid SupportedCountryId { get; set; }
        public DateTime SupportedSince { get; set; }
        public DateTime? SupportEndedOn { get; set; }
        public string? Notes { get; set; }
        public mCountry Country { get; set; }
    }
}
