namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    //public class eKYCDocuTemplate:secBaseEntity1
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public Guid KYCDocuTemplatesId { get; set; }
    //    //i.e. India, USA
    //    public eCountry CountryApplicable { get; set; }
    //    //i.e. Identity, tax, Residency
    //    [StringLength(100)]
    //    public string Category { get; set; }
    //    //i.e. Adhaar, Passport, Visa
    //    [StringLength(100)]
    //    public string Group { get; set; }
    //    //Adhaar Front, Adhaar Back
    //    [StringLength(100)]
    //    public string DocPlaceHolder { get; set; }
    //    //Any DocPlaceHoder in Group Mandatory
    //    public bool AnyInGroup { get; set; }
    //    //All DocPlaceHoder in Group Mandatory
    //    public bool AllInGroup { get; set; }
    //    //True, If all Groups withIn category is required
    //    public bool AllGroupOfCategory { get; set; }
    //    //No of Groups withIn Category if not all, All=0
    //    public int HowManyGroupsInCategory { get; set; }

    //}
    public class eGlobalVariables: secBaseEntity2
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GlobalVariablesId { get; set; }
        [StringLength(250)]
        public string Key { get; set; }
        [StringLength(500)]
        public string Value { get; set; }
    }
}
