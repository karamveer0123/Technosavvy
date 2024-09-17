
using NavExM.Int.Watcher.WatchDog.Model.AppInt;

namespace NavExM.Int.Watcher.WatchDog.Service
{
    /// <summary>
    /// This Config File will hold and manipulate values From Provided Config or HardDrive
    /// Once System Started, MQueue connection made, These Values will not used anymore
    /// </summary>
    internal static class ConfigBase
    {
        internal static mHandShakePackage handShakePackage { get; private set; }
        internal static string QNameRegResponse
        {
            get
            {
                return $"{handShakePackage.InstanceName}_RegRes_{handShakePackage.MacAddress}_{handShakePackage.ProcessId}";
            }
        }
        internal static string QNameRegReq { get { return $"{handShakePackage.InstanceName}_RegReq"; } }
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
            //ToDo, Naveen, Further values provided in Respose by WatchDog should be stored here in this class
            IsDynamicSet = true;
        }
        internal static void LoadDefault()
        {
            try
            {
               // Console.WriteLine("Hand Shake Called..");
                handShakePackage = DoHandShake();
                QURL = ConfigEx.Config.GetSection("AppMainQServer").Value;
                QPort = Convert.ToInt16(ConfigEx.Config.GetSection("AppMainQPort").Value);
                QPassword = ConfigEx.Config.GetSection("AppMainQPassword").Value;
                QUserName = ConfigEx.Config.GetSection("AppMainQUser").Value;
                QueueName = $"{Environment.UserDomainName}-{handShakePackage.InstanceName}-{Environment.ProcessId}";
                IsSet = true;
                //Console.WriteLine($"QURL:{QURL}");
                //Console.WriteLine($"QPort:{QPort}");
                //Console.WriteLine($"QPassword:{QPassword}");
                //Console.WriteLine($"QUserName:{QUserName}");
                //Console.WriteLine($"QueueName:{QueueName}");

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
            r.InstanceName = "NavExM.Int.Watcher.WatchDog.exe"; //Environment.ProcessPath.Split('\\').Last();
            r.ServiceAccount = Environment.UserName;
            r.DomainName = Environment.UserDomainName;
            
            return r;
        }
    }
}
