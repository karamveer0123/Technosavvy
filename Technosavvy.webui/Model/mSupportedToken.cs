namespace TechnoApp.Ext.Web.UI.Model
{
    public class mSupportedToken
    {
        public Guid SupportedTokenId { get; set; }
        public string Code { get; set; }
        public string Narration { get; set; }
        public bool IsNative { get; set; }
        public string ContractAddress { get; set; }
        public mSupportedNetwork RelatedNetwork { get; set; }

    }
}
