using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;

namespace NavExM.Int.Maintenance.APIs.Static
{
    internal static class Helper
    {
        internal static TimeSpan? ApproximateUpTimeDuration()
        {
            TimeSpan? retVal;

            var envTickCountInMs =
                Environment.TickCount;

            try
            {
                retVal =
                    envTickCountInMs > 0
                        ?
                            DateTime.UtcNow.AddMilliseconds(Environment.TickCount) -
                                    DateTime.MinValue
                        :
                            new TimeSpan(
                                new DateTime(
                                    ((long)int.MaxValue + (envTickCountInMs & int.MaxValue)) * 10 * 1000).Ticks);
            }
            catch (Exception)
            {
                // IGNORE
                retVal = null;
            }

            return retVal;
        }
        internal static List<string> GetLocalIPs()
        {
            List<string> vlist = new List<string>();
            Dns.GetHostAddresses(Dns.GetHostName()).ToList().ForEach(x =>
            {
                var l = x.ToString().Split('.').ToList();
                if (l.Count == 4 && x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    vlist.Add(x.ToString());
                }

            });
            return vlist;
        }
        internal static List<string> GetMacAddress()
        {
            List<string> retval = new List<string>();
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                nic.OperationalStatus == OperationalStatus.Up)
                {
                    retval.Add(nic.GetPhysicalAddress().ToString());
                }
            }
            return retval;
        }
        internal static string GetVersion()
        {
            string version = string.Empty;
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                version = fvi.FileVersion;
                var ver = Assembly.GetEntryAssembly().GetName().Version;
                var ver2 = typeof(string).Assembly.GetName().Version;
            }
            catch (Exception ex)
            {
                //ToDo:EventLogging in System Event System
                //Utility.DoLocalLog("Error in GetProcessorID Method...");
            }
            return version;
        }
    }
}
