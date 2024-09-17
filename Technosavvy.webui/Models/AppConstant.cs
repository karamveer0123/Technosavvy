namespace TechnoApp.Ext.Web.UI.Models
{
    public class AppConstant
    {
        public static string InstanceId { get; } = ConfigExtention.Configuration.GetSection("InstanceId").Value;
        public static string MaintenanceAPI { get; } = ConfigExtention.Configuration.GetSection("MaintenanceAPI").Value;
        public static string TradeAPI { get; } = ConfigExtention.Configuration.GetSection("TradeAPI").Value;
        public static string SENDOTP { get; } = "SendOtp";

        public static string TechnoAppSite { get; } = ConfigExtention.Configuration.GetSection("TechnoAppSite").Value;

        public static string TechnoAppFaceBook { get; } = ConfigExtention.Configuration.GetSection("TechnoAppFaceBook").Value;
        public static string TechnoAppMedium { get; } = ConfigExtention.Configuration.GetSection("TechnoAppMedium").Value;
        public static string TechnoAppYouTube { get; } = ConfigExtention.Configuration.GetSection("TechnoAppYouTube").Value;
        public static string TechnoAppInstagram { get; } = ConfigExtention.Configuration.GetSection("TechnoAppInstagram").Value;
        public static string TechnoAppTwitter { get; } = ConfigExtention.Configuration.GetSection("TechnoAppTwitter").Value;
        public static string TechnoAppReddit { get; } = ConfigExtention.Configuration.GetSection("TechnoAppReddit").Value;
        public static string TechnoAppTelegram { get; } = ConfigExtention.Configuration.GetSection("TechnoAppTelegram").Value;
        public static string TechnoAppLinkedIn { get; } = ConfigExtention.Configuration.GetSection("TechnoAppLinkedIn").Value;

        public static string WatDogAPI { get; } = ConfigExtention.Configuration.GetSection("WatchDogAPI").Value;
        public static string AppId { get; } = ConfigExtention.Configuration.GetSection("AppId").Value;
    }

}
