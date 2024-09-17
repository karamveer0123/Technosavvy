namespace TechnoApp.Ext.Web.UI.Static;

public static class ConfigEx
{
    public static IConfiguration Config;
    public static void Initialize(IConfiguration configuration)
    {
        Config = configuration;
    }
    public static versionType VersionType
    {
        get
        {
            var str = ConfigEx.Config.GetSection("ReleaseType").Value;
            if(str==null) return versionType.Prod;
            if (str.ToLower() == "prebeta") return versionType.PreBeta;
            if (str.ToLower() == "beta") return versionType.Beta;
            if (str.ToLower() == "dev") return versionType.Dev;
            if (str.ToLower() == "uat") return versionType.UAT;
            if (str.ToLower() == "test") return versionType.Test;
            if (str.ToLower() == "internal") return versionType.Internal;
            else return versionType.Prod;
        }
    }
    public static bool ShouldMini
    {
        get
        {
            try
            {
                var str = ConfigEx.Config.GetSection("ShouldMini").Value;
                return bool.Parse(str);
            }
            catch (Exception ex)
            {

            }
            return false;
            
        }
    }
}
public enum versionType
{
    PreBeta, Beta, Dev, UAT, Test, Internal, Prod
}