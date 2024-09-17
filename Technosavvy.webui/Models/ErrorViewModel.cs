namespace TechnoApp.Ext.Web.UI.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string DeepMsg { get; set; }
        public string Msg { get; set; }
    }
}