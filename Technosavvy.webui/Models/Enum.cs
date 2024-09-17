namespace TechnoApp.Ext.Web.UI.Models
{
    public class Enum
    {
    }
    public enum eActionName {
        SendOtp, VerifyOtp, CreatePassword
    }
    public enum eCookiesName { 
    Theme
    }
    public enum eThemeName {
    Dark,Light
    }
    public static class ConfigExtention
    {
        public static IConfiguration Configuration;
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;

        }
    }
}
