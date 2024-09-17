using NavExM.Int.Maintenance.APIs.Model.AppInt;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;

namespace NavExM.Int.Maintenance.APIs.Manager
{
    internal class CommonDataManager : ManagerBase
    {
        public CommonDataManager()
        {

        }
        internal List<mCountry> GetCountries()
        {
            return dbctx.Country.ToList().ToModel();
        }
        internal List<Tuple<string, string>> GetVariables()
        {
            return dbctx.GlobalVariables.Select(x => Tuple.Create(x.Key, x.Value)).ToList();
        }
        internal mHandShakePackage DoHandShake()
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
        internal List<mCoinWatch> GetCoinWatcher_ForGeko()
        {
            //ToDo: Naveen, Implement the List of Coins that are to be watched for the Price
            return new List<mCoinWatch>();
        }
        /// <summary>
        /// Returns Convert Charges of Specific Market, else Global settings
        /// </summary>
        /// <param name="mCode">Market Code</param>
        /// <returns>Percentage Equal Fraction value</returns>
        internal async Task<double> ConvertCharge(string mCode)
        {
            var vs =   GetVariables();
            var o = vs.FirstOrDefault(x => x.Item1.ToUpper() == $"CONVERT{mCode}".ToUpper());
            if (o != null)
            {
                return double.Parse(o.Item2);
            }
            //Check Global Convert value
            o = vs.FirstOrDefault(x => x.Item1.ToUpper() == $"CONVERT".ToUpper());
            if (o != null)
            {
                return double.Parse(o.Item2);
            }
            return 0.03;//Hardcode value;
        }
        /// <summary>
        /// Returns Buy Charges of Specific Market, else Global settings
        /// </summary>
        /// <param name="mCode">Market Code</param>
        /// <returns>Percentage Equal Fraction value</returns>
        internal async Task<double> BuyCharge(string mCode)
        {
            var vs = GetVariables();
            var o = vs.FirstOrDefault(x => x.Item1.ToUpper() == $"BUY{mCode}".ToUpper());
            if (o != null)
            {
                return double.Parse(o.Item2);
            }
            //Check Global Convert value
            o = vs.FirstOrDefault(x => x.Item1.ToUpper() == $"BUY".ToUpper());
            if (o != null)
            {
                return double.Parse(o.Item2);
            }
            return 0.03;//Hardcode value;
        }
        /// <summary>
        /// Returns Minimum USDT value of for an Order in the Market
        /// </summary>
        /// <param name="mCode">Market Code</param>
        /// <returns>Minimum USDT value for a Valid Trade</returns>
        internal double MinimumTradeUSDTValue()
        {
            var vs = GetVariables();
            var o = vs.FirstOrDefault(x => x.Item1.ToUpper() == $"MinOrderSizeValueUSD".ToUpper());
            if (o != null)
            {
                return double.Parse(o.Item2);
            }

            return 10;//Hardcode value;
        }
        protected async Task<int> MinumumGlobalTick()
        {
            var vs = (GetVariables()).FirstOrDefault(x => x.Item1 == "MinumumGlobalTick").Item2;
            return Convert.ToInt16(vs);
        }
    }
   
}
