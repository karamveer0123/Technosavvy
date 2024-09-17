namespace TechnoApp.Ext.Web.UI.Model
{
    public class mEnumBoxData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string EnumValue { get; set; }
        public string EnumType { get; set; }
        public string VersionNo { get; set; } = "0";
        public int SortingOrderId { get; set; }//for sorting order
   
    }
}
