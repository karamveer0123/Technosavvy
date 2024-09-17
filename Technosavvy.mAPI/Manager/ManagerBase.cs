using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NavExM.Int.Maintenance.APIs.Data.Entity.Contents;
using NavExM.Int.Maintenance.APIs.ServerModel;
using NavExM.Int.Maintenance.APIs.Services;
using System.Net.Http.Headers;

namespace NavExM.Int.Maintenance.APIs.Manager
{
    public class ManagerBase
    {
        internal PreBetaDBContext pbdbctx;
        internal SmtpConfig smtp;
        internal CareerAppContext JDdbctx;

        internal ContentAppContext cdbctx;
        internal EventAppContext edbctx;
        internal ApiAppContext dbctx;
        internal RewardAppContext Rewardctx;
        internal HttpContext httpContext;
        internal HttpClient _LogAPIChannel;
        internal static HttpClient _WalletWatcher;
        internal HttpContext _http;
        public Func<string, string, bool> IsSame = (a, b) =>
        {
            if (string.IsNullOrEmpty(a) && string.IsNullOrEmpty(b))
                return true;
            if (!string.IsNullOrEmpty(a) && string.IsNullOrEmpty(b) || string.IsNullOrEmpty(a) && !string.IsNullOrEmpty(b))
                return false;

            return string.Compare(b, a, StringComparison.OrdinalIgnoreCase) == 0;
        };
        public List<eEnumBoxData> GetEnumBoxData<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            try
            {
                var enumType = typeof(TEnum);

                // Optional runtime check for completeness    
                if (!enumType.IsEnum)
                {
                    throw new ArgumentException();
                }
                var enumBoxDataList = dbctx.EnumBoxData.ToList();
                var lstEnums = enumBoxDataList.Where(c => c.EnumType == enumType.Name).OrderByDescending(x => x.Id).ToList();

                // To capitalize the data of the enum box in the UI section.
                //lstEnums.ForEach(c =>
                //{
                //    c.Name = c.Name.ToSplitOnCapitals();
                //});
                return lstEnums;
            }
            catch (Exception ex)
            {
                return new List<eEnumBoxData>();
            }
        }
        internal protected Tuple<bool, string> Ok(bool result, string Msg)
        {
            return new Tuple<bool, string>(result, Msg);
        }
        internal protected void ThrowNullArgumentException(object? obj)
        {
            throw new ArgumentNullException($"{nameof(obj)} can't be null");
        }
        internal protected void ThrowInvalidOperationException(string msg = "")
        {
            if (msg.IsNullorEmpty())
                throw new InvalidOperationException("Mandatory field value was null. operation Aborted..");
            throw new InvalidOperationException(msg);
        }
        public string GetSessionHash()
        {
            //ToDo: Naveen Get this SessionHash fromRequest header
            string retval = "-No User Session-";
            try
            {
                retval = httpContext.Request.Headers["SessionToken"];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No Session Hash exist when Request for secure operation at :{DateTime.UtcNow}");

            }
            return retval;
        }
        internal string GetSignedHash(string str)
        {
            //ToDo: Naveen, SignedHash should be implemented with AppSeed Private Key of Application Instances, Public Key would be communicated to Watcher for Hash Verification
            var a = new PasswordHasher<object>(optionsAccessor: default);
            var res = a.HashPassword(Guid.Empty, str);
            return res;
        }
        internal string GetHash(string str)
        {
            var a = new PasswordHasher<object>(optionsAccessor: default);
            var res = a.HashPassword(Guid.Empty, str);
            return res;
        }
        internal PasswordVerificationResult CompareHash(string Hpwd, string pwd)
        {
            var a = new PasswordHasher<object>(optionsAccessor: default);
            var res = a.VerifyHashedPassword(Guid.Empty, Hpwd, pwd);
            return res;
        }
        internal CFGeo GetGeoLOcation()
        {
            CFGeo cFGeo = new CFGeo();
            if (!string.IsNullOrEmpty(httpContext.Request.Headers["cf-ipcountry"]))
            {
                cFGeo.IP = httpContext.Request.Headers["CF-Connecting-IP"];
                cFGeo.CountryCode = httpContext.Request.Headers["CF-IPCountry"];
                cFGeo.City = httpContext.Request.Headers["cf-ipcity"];
                cFGeo.Ipcontinent = httpContext.Request.Headers["cf-ipcontinent"];
                cFGeo.Longitude = httpContext.Request.Headers["cf-iplongitude"];
                cFGeo.Latitude = httpContext.Request.Headers["cf-iplatitude"];
            }
            return cFGeo;

        }
        protected HttpClient GetLogAPIChannel()
        {
            if (_LogAPIChannel == null)
            {
                _LogAPIChannel = new HttpClient();

                var ch = _LogAPIChannel;//short Name
                                        //ToDo: Naveen, Application Instance Identity should be implemented here
                ch.BaseAddress = new Uri(mAppConstant.WatDogAPI);
                ch.DefaultRequestHeaders.Accept.Clear();
                ch.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            return _LogAPIChannel;
        }
        protected HttpClient GetWalletWatchChannel()
        {
            if (_WalletWatcher == null)
            {
                _WalletWatcher = new HttpClient();

                var ch = _WalletWatcher;//short Name
                                        //ToDo: Naveen, Application Instance Identity should be implemented here
                ch.BaseAddress = new Uri(mAppConstant.WalletWatch);
                ch.DefaultRequestHeaders.Accept.Clear();
                ch.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                ch.DefaultRequestHeaders.Add("callerid", Signer.PublicKey);


            }
            return _WalletWatcher;
        }
        #region WalletWatch
        internal bool RequestWatch(mWatchRequest m)
        {
            var _endPoint = $"Watcher/ReportEvent";
            GetWalletWatchChannel().PostAsJsonAsync(_endPoint, m);
            return true;
        }
        internal string RequestNewWalletAddress()
        {
            string Add = $"0xABCDEFDSGFDSFT{DateTime.Now}XX";
            LogEvent($"Not Implemented Function RequestNewWalletAddress Used: {Add}");
            //ToDo: Naveen, It should call watcher and receive Network Wallet
            return Add;
        }
        #endregion

        #region Log Messages
        internal void LogEvent(string msg)
        {
            Services.SrvPlugIn.LogEventG(msg);
            Console2.WriteLine_White(msg);
            //mLogT vL = new mLogT() { Message = msg };
            //var _endPoint = $"Watcher/ReportEvent";
            //GetLogAPIChannel().PostAsJsonAsync(_endPoint, vL);
        }

        internal void LogError(string msg)
        {
            Services.SrvPlugIn.LogErrorG(msg);
            Console2.WriteLine_RED(msg);
            //mLogT vL = new mLogT() { Message = msg };
            //var _endPoint = $"Watcher/ReportError";
            //GetLogAPIChannel().PostAsJsonAsync(_endPoint, vL);
        }
        internal string LogError(Exception ex)
        {
            var Msg = GetMsg(ex);
            Services.SrvPlugIn.LogErrorG(Msg);
            //mLogT vL = new mLogT() { Message = Msg };
            //var _endPoint = $"Watcher/ReportError";
            //GetLogAPIChannel().PostAsJsonAsync(_endPoint, vL);
            return Msg;
        }
        private string GetMsg(Exception ex)
        {
            if (ex is null) return string.Empty;
            var msg = $"{ex.Message}{GetMsg(ex.InnerException)}";
            return msg;
        }
        protected string T { get => $"T:{Thread.CurrentThread.ManagedThreadId}"; }
        #endregion 

    }
}
