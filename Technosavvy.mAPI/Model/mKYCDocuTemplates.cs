namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mKYCDocuTemplate 
    {
        public Guid KYCDocuTemplateId { get; set; }
        //i.e. India, USA
        public mCountry CountryApplicable { get; set; }
        //i.e. Identity, tax, Residency
        public string Category { get; set; }
        //i.e. Adhaar, Passport, Visa
        public string Group { get; set; }
        //Adhaar Front, Adhaar Back
        public string DocPlaceHolder { get; set; }
        //Any DocPlaceHoder in Group Mandatory
        public bool AnyInGroup { get; set; }
        //All DocPlaceHoder in Group Mandatory
        public bool AllInGroup { get; set; }
        public bool AllGroupOfCategory { get; set; }
        public int HowManyGroupsInCategory { get; set; }

    }
}
