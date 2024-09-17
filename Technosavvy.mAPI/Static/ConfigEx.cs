namespace NavExM.Int.Maintenance.APIs.Static;

public static class ConfigEx
{
    public static IConfiguration Config;
    public static void Initialize(IConfiguration configuration)
    {
        Config = configuration;
    }
    static string _VersionType = "";
    public static versionType VersionType
    {
        get
        {
            if (_VersionType.IsNullorEmpty())
                _VersionType = ConfigEx.Config.GetSection("ReleaseType").Value;
            var str = _VersionType;
            if (str == null) return versionType.Prod;
            if (str.ToLower() == "prebeta") return versionType.PreBeta;
            if (str.ToLower() == "beta") return versionType.Beta;
            if (str.ToLower() == "dev") return versionType.Dev;
            if (str.ToLower() == "uat") return versionType.UAT;
            if (str.ToLower() == "test") return versionType.Test;
            if (str.ToLower() == "internal") return versionType.Internal;
            else return versionType.Prod;
        }
    }
    public static versionEnv VersionEnvironment
    {
        get
        {
            var str = ConfigEx.Config.GetSection("ReleaseEnv").Value;
            if (str == null) return versionEnv.Prod;
            if (str.ToLower() == "preprod") return versionEnv.PreProd;
            if (str.ToLower() == "devtest") return versionEnv.DevTest;
            if (str.ToLower() == "dev") return versionEnv.Dev;
            if (str.ToLower() == "uat") return versionEnv.UAT;
            if (str.ToLower() == "prod") return versionEnv.Prod;
            if (str.ToLower() == "internal") return versionEnv.Internal;
            else return versionEnv.Prod;
        }
    }

}
public enum versionType
{
    PreBeta, Beta, Dev, UAT, Test, Internal, Prod
}
public enum versionEnv
{
    Dev, DevTest, UAT, PreProd, Internal, Prod
}

