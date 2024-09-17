using NavExM.Int.Maintenance.APIs.Model.AppInt;

namespace NavExM.Int.Maintenance.APIs.Static
{
    internal static class ConfigBase
    {
        internal static mHandShakePackage handShakePackage { get; private set; }
        internal static string QNameRegResponse
        {
            get
            {
                return $"{handShakePackage.InstanceName}_RegRes_{handShakePackage.MacAddress.FirstOrDefault()}_{handShakePackage.ProcessId}";
            }
        }

        internal static string Ex_NameRegReq { get { return $"NavExM.Int.Maintenance.APIs.exe_RegReq"; } }
        internal static string Ex_NameRegRes { get { return $"NavExM.Int.Maintenance.APIs.exe_RegRes"; } }
        //internal static string QNameError { get { return $"{handShakePackage.InstanceKey}_Error"; } }
        //internal static string QNameEvent { get { return $"{handShakePackage.InstanceKey}_Event"; } }
        internal static string QURL { get; set; }
        internal static int QPort { get; set; }
        internal static string QPassword { get; set; }
        internal static string QUserName { get; set; }
        internal static string QueueName { get; set; }
        internal static bool IsSet { get; set; }
        internal static string PublicKey { get; set; } = "ABCStillPending";
        internal static string PrivateKey { get; set; } = "YouShouldNotSeeMePending";
        internal static string AppSeed { get; set; } = "AppRegistrationSeed";
        internal static bool IsDynamicSet { get; set; }
        internal static void LoadDynamic()
        {
            IsDynamicSet = true;
        }
        internal static void LoadDefault()
        {
            try
            {
                handShakePackage = DoHandShake();
                QURL = ConfigEx.Config.GetSection("AppMainQServer").Value;
                QPort = Convert.ToInt16(ConfigEx.Config.GetSection("AppMainQPort").Value);
                QPassword = ConfigEx.Config.GetSection("AppMainQPassword").Value;
                QUserName = ConfigEx.Config.GetSection("AppMainQUser").Value;
                QueueName = $"{Environment.UserDomainName}-{handShakePackage.InstanceName}-{Environment.ProcessId}";
                IsSet = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetDeepMsg());
            }
        }
        private static mHandShakePackage DoHandShake()
        {
            var r = new mHandShakePackage();
            r.ComputerName = Environment.MachineName;
            r.ManagedThreadId = Environment.CurrentManagedThreadId;
            r.CurrentDirectory = Environment.CurrentDirectory;
            r.ProcessPath = Environment.ProcessPath;
            r.ProcessId = Environment.ProcessId;
            r.ProcessodCount = Environment.ProcessorCount;
            r.OS = Helper.GetVersion();
            r.MacAddress = Helper.GetMacAddress();
            r.GetLocalIPs = Helper.GetLocalIPs();
            r.InstanceName = "NavExM.Int.Maintenance.APIs.exe";// Environment.ProcessPath.Split('\\').Last();
            r.ServiceAccount = Environment.UserName;
            r.DomainName = Environment.UserDomainName;

            return r;
        }
    }
}
