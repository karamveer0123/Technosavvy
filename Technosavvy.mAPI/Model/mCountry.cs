namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mCountry {
        public Guid CountryId { get; set; }
        public string DialCode { get; set; }
        public string Name { get; set; }
        public string Abbrivation { get; set; }
        public string Continent { get; set; } //Africa, America
        public string Block { get; set; }//Asia, central America..
    }
}
