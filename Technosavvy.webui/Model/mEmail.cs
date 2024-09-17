namespace TechnoApp.Ext.Web.UI.Model
{
    public class mEmail
    {
        public string? Id { get; set; }
        //public string Email { get; set; } 
        public string? From { get; set; }
        public string To { get; set; }
        public List<string>? Cc { get; set; }
        public List<string>? Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string? AttachmentFileUrl { get; set; }

    }    
    //public class SmtpConfig
    //{
    //    public const string Smtp = "SmtpConfig";
    //    public string SmtpServer { get; set; } = string.Empty;
    //    public string Port { get; set; } = string.Empty;
    //    public string EmailFrom { get; set; } = string.Empty;
    //    public string Password { get; set; } = string.Empty;
    //    public string DisplayName { get; set; } = string.Empty;
    //}

}
