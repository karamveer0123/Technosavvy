namespace TechnoApp.Ext.Web.UI.Model
{
    public class mAppConstant
    {
        public static string TechnoAppSite { get; } = ConfigExtention.Configuration.GetSection("TechnoAppSite").Value;
        public static string WatDogAPI { get; } = ConfigExtention.Configuration.GetSection("WatchDogAPI").Value;
        public static string WalletWatch { get; } = ConfigExtention.Configuration.GetSection("WalletWatchAPI").Value;
        public static string AppId { get; } = ConfigExtention.Configuration.GetSection("AppId").Value;
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
