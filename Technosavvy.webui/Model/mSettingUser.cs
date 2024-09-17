namespace TechnoApp.Ext.Web.UI.Model
{
    public class mSettingUser
    {
    }
    public class mSettingProfile
    {
        public string UserNicName { get; set; }
        public Guid UserId { get; set; }
    }
    public class mSettingGCode
    {
        public string GoogleUCode { get; set; }
        public bool IsMultiFactorEnabled { get; set; }
        public Guid UserId { get; set; }
    }
}
