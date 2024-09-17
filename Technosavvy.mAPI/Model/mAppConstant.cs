namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mAppConstant
    {
        public static string NavExMSite { get; } = ConfigExtention.Configuration.GetSection("NavExMSite").Value;
        public static string WatDogAPI { get; } = ConfigExtention.Configuration.GetSection("WatchDogAPI").Value;
        public static string WalletWatch { get; } = ConfigExtention.Configuration.GetSection("WalletWatchAPI").Value;
        public static string AppId { get; } = ConfigExtention.Configuration.GetSection("AppId").Value;
        public static string MMEmailDomain { get; } = ConfigExtention.Configuration.GetSection("MMEmailDomain").Value;
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
