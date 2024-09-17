namespace TechnoApp.Ext.Web.UI.ViewModels;

public class vmAddress
{
    public Guid AddressId { get; set; }
    public Guid UserAccountId { get; set; }
    public string? UnitNo { get; set; }
    public string StreetAdd { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostCode { get; set; }
    public vmCountry Country { get; set; }
    public Guid? selectedAddressCountryId { get; set; }
}
