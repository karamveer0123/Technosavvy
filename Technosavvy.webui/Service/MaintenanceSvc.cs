using TechnoApp.Ext.Web.UI.Model;
using TechnoApp.Ext.Web.UI.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using NuGet.Protocol;
using Microsoft.AspNetCore.Routing.Constraints;

namespace TechnoApp.Ext.Web.UI.Service
{
    public class MaintenanceSvc
    {
        internal HttpClient _maintAPIChannel;
        internal HttpClient _tradeAPIChannel;
        internal IConfiguration _configuration;
        internal HttpContext _http;
        internal AppSessionManager _appSessionManager;
        internal HttpClient _LogAPIChannel;
        internal IDataProtector _DataProtector;
        protected HttpClient GetTradeAPIChannel()
        {
            if (_tradeAPIChannel == null)
            {
                _tradeAPIChannel = new HttpClient();

                var ch = _tradeAPIChannel;//short Name
                                          //ToDo: Naveen, Application Instance Identity should be implemented here
                ch.BaseAddress = new Uri(AppConstant.TradeAPI);
                ch.DefaultRequestHeaders.Accept.Clear();
                ch.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (((string)_http.Request.Headers["CF-IPCountry"]).IsNOT_NullorEmpty())
                {
                    ch.DefaultRequestHeaders.Add("cf-ipcountry", (string)_http.Request.Headers["CF-IPCountry"]);
                    ch.DefaultRequestHeaders.Add("cf-ipcity", (string)_http.Request.Headers["cf-ipcity"]);
                    ch.DefaultRequestHeaders.Add("cf-ipcontinent", (string)_http.Request.Headers["cf-ipcontinent"]);
                    ch.DefaultRequestHeaders.Add("cf-iplongitude", (string)_http.Request.Headers["cf-iplongitude"]);
                    ch.DefaultRequestHeaders.Add("cf-iplatitude", (string)_http.Request.Headers["cf-iplatitude"]);
                    ch.DefaultRequestHeaders.Add("CF-Connecting-IP", (string)_http.Request.Headers["CF-Connecting-IP"]);
                    ch.DefaultRequestHeaders.Add("headerstr", (string)_http.Request.Headers["headerstr"]);
                }

                ch.DefaultRequestHeaders.Add("SessionToken", (string)_appSessionManager.mySession.SessionHash);

            }
            return _tradeAPIChannel;
        }
        protected HttpClient GetMaintAPIChannel()
        {
            if (_maintAPIChannel == null)
            {
                _maintAPIChannel = new HttpClient();

                var ch = _maintAPIChannel;//short Name
                                          //ToDo: Naveen, Application Instance Identity should be implemented here
                ch.BaseAddress = new Uri(AppConstant.MaintenanceAPI);
                ch.DefaultRequestHeaders.Accept.Clear();
                ch.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (_http != null && ((string)_http.Request.Headers["CF-IPCountry"]).IsNOT_NullorEmpty())
                {
                    ch.DefaultRequestHeaders.Add("cf-ipcountry", (string)_http.Request.Headers["CF-IPCountry"]);
                    ch.DefaultRequestHeaders.Add("cf-ipcity", (string)_http.Request.Headers["cf-ipcity"]);
                    ch.DefaultRequestHeaders.Add("cf-ipcontinent", (string)_http.Request.Headers["cf-ipcontinent"]);
                    ch.DefaultRequestHeaders.Add("cf-iplongitude", (string)_http.Request.Headers["cf-iplongitude"]);
                    ch.DefaultRequestHeaders.Add("cf-iplatitude", (string)_http.Request.Headers["cf-iplatitude"]);
                    ch.DefaultRequestHeaders.Add("CF-Connecting-IP", (string)_http.Request.Headers["CF-Connecting-IP"]);
                    ch.DefaultRequestHeaders.Add("headerstr", (string)_http.Request.Headers["headerstr"]);
                }
                if (_appSessionManager?.mySession != null)
                    ch.DefaultRequestHeaders.Add("SessionToken", (string)_appSessionManager.mySession.SessionHash);

            }
            return _maintAPIChannel;
        }

        protected HttpClient GetLogAPIChannel()
        {
            if (_LogAPIChannel == null)
            {
                _LogAPIChannel = new HttpClient();

                var ch = _LogAPIChannel;//short Name
                                        //ToDo: Naveen, Application Instance Identity should be implemented here
                ch.BaseAddress = new Uri(AppConstant.WatDogAPI);
                ch.DefaultRequestHeaders.Accept.Clear();
                ch.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (((string)_http.Request.Headers["CF-IPCountry"]).IsNOT_NullorEmpty())
                {
                    ch.DefaultRequestHeaders.Add("cf-ipcountry", (string)_http.Request.Headers["CF-IPCountry"]);
                    ch.DefaultRequestHeaders.Add("cf-ipcity", (string)_http.Request.Headers["cf-ipcity"]);
                    ch.DefaultRequestHeaders.Add("cf-ipcontinent", (string)_http.Request.Headers["cf-ipcontinent"]);
                    ch.DefaultRequestHeaders.Add("cf-iplongitude", (string)_http.Request.Headers["cf-iplongitude"]);
                    ch.DefaultRequestHeaders.Add("cf-iplatitude", (string)_http.Request.Headers["cf-iplatitude"]);
                    ch.DefaultRequestHeaders.Add("cf-ip", (string)_http.Request.Headers["cf-ipcity"]);
                    ch.DefaultRequestHeaders.Add("headerstr", (string)_http.Request.Headers["headerstr"]);

                }

            }
            return _LogAPIChannel;
        }

        protected internal async Task<List<Model.mToken3>> GetFiatTokensList()
        {
            var ret = new List<Model.mToken3>();
            var tlst = await GetAllActiveTokens();
            var flst = await GetActiveFiat(1000);
            tlst.Where(x => x.IsFiatRepresentative).ToList().ForEach(x =>
            {
                var f = flst.FirstOrDefault(z => z.Code.ToUpper() == x.Code.ToUpper());

                ret.Add(new Model.mToken3
                {
                    token = x,
                    Symbole = f.Symbole
                });
            });
            return ret;
        }

        #region KYC API


        #endregion
        #region INR PaymentMethods
        /// <summary>
        /// Returns the Bank details of INR Bank Account of the Exchange
        /// </summary>
        /// <returns></returns>
        protected internal async Task<List<mBankDepositPaymentMethod>> GetINRBankDetailsOfTechnoApp()
        {
            List<mBankDepositPaymentMethod> res = new List<mBankDepositPaymentMethod>();
            var _endPoint = $"PaymentMethods/GetINRBankDetails";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mBankDepositPaymentMethod>>();
            }
            return res;
        }
        /// <summary>
        /// Returns the UPI details of INR Bank Account of the Exchange
        /// </summary>
        /// <returns></returns>
        protected internal async Task<List<mUPIPaymentMethod>> GetINRUPIDetailsOfTechnoApp()
        {
            List<mUPIPaymentMethod> res = new List<mUPIPaymentMethod>();
            var _endPoint = $"PaymentMethods/GetINRUPIDetails";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mUPIPaymentMethod>>();
            }
            return res;
        }
        protected internal async Task<mINRBankDeposit> CreateINRBankDepositSetup(mINRBankDeposit m)
        {
            mINRBankDeposit res = null;
            var _endPoint = $"PaymentMethods/CreateINRBankDepositSetup";

            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, m);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mINRBankDeposit>();
            }
            else { Console2.WriteLine_RED($"ERROR:CreateINRUPISetup has failed in Response with message:{response.ToJson()} for Data:{m.ToJson()}"); }
            return res;
        }
        protected internal async Task<bool> DeleteINRBankDeposit(Guid m)
        {
            bool res = false;
            var _endPoint = $"PaymentMethods/DeleteINRBankDeposit?m={m}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            return res;
        }
        protected internal async Task<List<mINRBankDeposit>> GetmyINRBankDeposit(Guid profileId)
        {
            List<mINRBankDeposit> res = new List<mINRBankDeposit>();
            var _endPoint = $"PaymentMethods/GetmyINRBankDeposit?profileId={profileId}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mINRBankDeposit>>();
            }
            else { Console2.WriteLine_RED($"ERROR:GetmyINRBankDeposit has failed in Response with message:{response.ToJson()} "); }
            return res;
        }
        protected internal async Task<mINRUPI> CreateINRUPISetup(mINRUPI m)
        {
            mINRUPI res = null;
            var _endPoint = $"PaymentMethods/CreateINRUPISetup";

            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, m);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mINRUPI>();
            }
            else { Console2.WriteLine_RED($"ERROR:CreateINRUPISetup has failed in Response with message:{response.ToJson()} for Data:{m.ToJson()}"); }
            return res;
        }
        protected internal async Task<bool> DeleteINRUPISetup(Guid m)
        {
            bool res = false;
            var _endPoint = $"PaymentMethods/DeleteINRUPISetup?m={m}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            return res;
        }
        protected internal async Task<List<mINRUPI>> GetmyINRUPISetup(Guid profileId)
        {
            List<mINRUPI> res = new List<mINRUPI>();
            var _endPoint = $"PaymentMethods/GetmyINRUPISetup?profileId={profileId}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mINRUPI>>();
            }
            return res;
        }
        #endregion
        #region Trade API
        protected internal async Task<List<mWalletTransactions>> GetMyWalletTransactions(Guid WalletId)
        {
            List<mWalletTransactions> res = null;

            var _endPoint = $"Wallet/GetMyWalletTransactions?walletId={WalletId}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mWalletTransactions>>();
            }
            return res;
        }
        protected internal async Task<Tuple<double, double>> GetEstimatedValueIn(string bCode, string qCode, double Amt)
        {

            Tuple<double, double> res= new Tuple<double, double>(double.NaN,double.NaN);

            var _endPoint = $"Token/GetEstimatedValueIn?bCode={bCode}&qcode={qCode}&Amt={Amt}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<Tuple<double, double>>();
            }
            return res;
        }
        protected internal async Task<double> GetTokenMarketEstimatedBuyPrice(string mCode, double Amt)
        {

            double res = double.NaN;

            var _endPoint = $"Token/GetTokenMarketEstimatedBuyPrice?mCode={mCode}&Amt={Amt}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<double>();
            }
            return res;
        }
        protected internal async Task<double> GetTokenMarketEstimatedSellPrice(string mCode, double Amt)
        {

            double res = double.NaN;

            var _endPoint = $"Token/GetTokenMarketEstimatedSellPrice?mCode={mCode}&Amt={Amt}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<double>();
            }
            return res;
        }
        protected internal async Task<double> GetTokenMarketEstimatedSellTotalValueUnitsCount(string mCode, double TradeValue)
        {

            double res = double.NaN;

            var _endPoint = $"Token/GetTokenMarketEstimatedSellTotalValueUnitsCount?mCode={mCode}&TradeValue={TradeValue}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<double>();
            }
            return res;
        }
        #endregion

        #region Reward API
        protected internal async Task<List<mAccruedCashBack>> GetMyCashbackRecords()
        {

            List<mAccruedCashBack> res = new List<mAccruedCashBack>();

            var _endPoint = $"Wallet/GetMyCashbackRecords";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mAccruedCashBack>>();
            }
            return res;
        }
        protected internal async Task<List<mUserRef>> GetReferredUsersRewardByDateGroup(string code)
        {

            List<mUserRef> res = null;

            var _endPoint = $"Wallet/GetReferredUsersRewardByDateGroup?RefCode={code}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mUserRef>>();
            }
            return res;
        }
        protected internal async Task<mRewardStats> GetReferredRewardStat(string code)
        {

            mRewardStats res = null;

            var _endPoint = $"Wallet/GetReferredRewardStat?RefCode={code}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mRewardStats>();
            }
            return res;
        }
        protected internal async Task<List<mMyReward>> GetMyRewardsRecords()
        {

            List<mMyReward> res = null;

            var _endPoint = $"Wallet/GetMyRewardsRecords";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mMyReward>>();
            }
            return res;
        }
        protected internal async Task<int> GetReferredUsersCount(string code)
        {

            var res = 0;

            var _endPoint = $"Wallet/GetReferredUsersCount?RefCode={code}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<int>();
            }
            return res;
        }
        #endregion

        #region MaintAPI

        internal mUserSession myUS
        {
            get
            {
                return _appSessionManager.ExtSession.UserSession;
            }
        }
        protected AppSession myCookieState { get { return _appSessionManager.mySession; } }
        public async Task<mUser> GetUserByName(string name)
        {
            var _endPoint = $"User/GetUserByName?uName={name}";
            mUser user = new mUser();

            //GET Method
            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadFromJsonAsync<mUser>();
            }
            return user;
        }
        public async Task<bool> GetAnyUser(string name)
        {
            var _endPoint = $"User/AnyUser?uName={name}";
            bool result = false;

            //GET Method
            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<bool>();
            }
            return result;
        }
        public async Task<Guid> RegisterUser(string name, string RefCode)
        {
            var _endPoint = $"User/RegisterUser?uName={name}";
            if (RefCode.IsNOT_NullorEmpty())
                _endPoint = $"User/RegisterUser?uName={name}&refCode={RefCode}";
            Guid result = Guid.Empty;

            //GET Method
            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<Guid>();
            }
            return result;
        }
        /// <summary>
        /// this function work for send otp
        /// </summary>
        /// <returns></returns>
        protected internal async Task<bool> RequestEmailOTP(string uName)
        {
            bool res = false;
            var _endPoint = $"User/RequestEmailOTP?uName={uName}";

            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, uName);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            return res;
        }
        /// <summary>
        /// this function work for send otp for forget user
        /// </summary>
        /// <returns></returns>
        protected internal async Task<bool> RequestForgetEmailOTP(string uName)
        {
            bool res = false;
            var _endPoint = $"User/CreateEmailOTPFOrgetPassword?uName={uName}";

            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, uName);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            return res;
        }

        /// <summary>
        /// this function work for verify otp
        /// </summary>
        /// <returns></returns>
        protected internal async Task<bool> VerifyEmailOTP(string name, string otp)
        {
            bool res = false;
            var _endPoint = $"User/VerifyEmailOTP?uName={name}&otp={otp}";
            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            return res;
        }
        protected internal async Task<bool> CanSetUserPassword(string name)
        {
            bool res = false;
            var _endPoint = $"User/CanSetUserPassword?uName={name}";
            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            return res;
        }
        protected internal async Task<List<mCountry>> GetAllCountries()
        {
            List<mCountry> res = new List<mCountry>();
            var _endPoint = $"CommonData/GetCountries";
            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mCountry>>();
            }
            return res;
        }
        protected async Task<string> GetPoly(string code)
        {
            //implment SrvPoly to generate floating points of Token trade values
            return "00,120 20,50 40,50 60,50 80,80 100,80 120,100 140,80 160,100 180,80 200,50 220,80 240,80 260,50 280,50 300,80 320,60 340,60 360,40 380,40 400,40 420,50 440,50 460,40 480,40 500,40".Replace(" ", ",");
        }
        private List<Tuple<string, string>> _var;
        internal async Task<List<Tuple<string, string>>> Variables()
        {
            _var = _var ?? (await GetGlobalVariables());
            return _var;
        }
        /// <summary>
        /// Returns Convert Charges of Specific Market, else Global settings
        /// </summary>
        /// <param name="mCode">Market Code</param>
        /// <returns>Percentage Equal Fraction value</returns>
        internal async Task<double> ConvertCharge(string mCode)
        {
            var vs = await Variables();
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
            var vs = await Variables();
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
        internal async Task<double> MinimumTradeUSDTValue()
        {
            var vs = await Variables();
            var o = vs.FirstOrDefault(x => x.Item1.ToUpper() == $"MinOrderSizeValueUSD".ToUpper());
            if (o != null)
            {
                return double.Parse(o.Item2);
            }
           
            return 10;//Hardcode value;
        }
        protected async Task<int> MinumumGlobalTick()
        {
            var vs = (await Variables()).FirstOrDefault(x => x.Item1 == "MinumumGlobalTick").Item2;
            return Convert.ToInt16(vs);
        }
        private string _dispFormatLess1k;
        private string _dispFormat;
        internal string DisplayFormat(int len, double val)
        {
            if (_dispFormat.IsNullOrEmpty())
            {
                StringBuilder sb = new StringBuilder();
                var str0 = "#.";
                var str1 = "0,000.";
                for (int i = 0; i < len; i++)
                {
                    sb.Append("0");
                }
                _dispFormat = $"{{0:{str1 + sb.ToString()}}}";
                _dispFormatLess1k = $"{{0:{str0 + sb.ToString()}}}";
                // FinalFormat = $"{{0:{sb.ToString()}}}";

            }
            if (val < 1000)
                return string.Format(_dispFormatLess1k, val);
            else
                return string.Format(_dispFormat, val);

        }
        private async Task<List<Tuple<string, string>>> GetGlobalVariables()
        {
            var res = new List<Tuple<string, string>>();
            var _endPoint = $"CommonData/GetGlobalVariables";
            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<Tuple<string, string>>>();
            }
            return res;
        }
        protected internal async Task<mProfile> GetProfile(Guid userAccountId)
        {
            var res = new mProfile();
            var _endPoint = $"Profile/GetProfile?userAccountID={userAccountId}";
            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mProfile>();
            }
            return res;
        }

        /// <summary>
        /// this function work for Create Password
        /// </summary>
        /// <returns></returns>
        protected internal async Task<bool> CreatePassword(Guid userId, string password)
        {
            bool res = false;
            var _endPoint = $"User/UpdatePassword?userId={userId}&password={password}";
            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            return res;
        }


        /// <summary>
        /// this function work for Create Password
        /// </summary>
        /// <returns></returns>
        protected async Task<mAuth> LogIn(vmUserLogin loginVm)
        {
            mAuth res = new mAuth();
            var _endPoint = $"User/LogIn";
            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, loginVm);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mAuth>();
            }
            return res;
        }
        /// <summary>
        /// Use this for Deposit/WithDraw Request that need 2F Auth
        /// </summary>
        protected async Task<mAuth> InSession2FAuth(vmUserLogin loginVm)
        {
            mAuth res = new mAuth();
            var _endPoint = $"User/InSession2FAuth";
            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mAuth>();
            }
            return res;
        }
        /// <summary>
        /// Get AuthEvent Object of Provided Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal async Task<mAuth> GetAuth(Guid id)
        {
            mAuth res = new mAuth();
            var _endPoint = $"User/GetAuth?authId={id.ToString()}";
            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mAuth>();
            }
            return res;
        }
        protected async Task<bool> LogOut()
        {
            var res = false;
            var _endPoint = $"User/SignOut";
            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            return res;
        }
        protected internal async Task<mUserSession> RequestSession(mUserSessionRequest SessionAuth)
        {
            var res = new mUserSession();
            var _endPoint = $"User/RequestSession";
            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, SessionAuth);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mUserSession>();
            }
            return res;
        }
        protected internal async Task<mUserSession> ReportGAuthForSession(mUserSessionRequest SessionAuth)
        {
            var res = new mUserSession();
            var _endPoint = $"User/Report2factor?uName={SessionAuth.UserName}&AuthId={SessionAuth.authId}&mode=auth";
            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, SessionAuth);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mUserSession>();
            }
            return res;
        }
        protected internal async Task<mUserSession> GetMySession()
        {
            mUserSession res = null;

            var _endPoint = $"User/GetMySession";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mUserSession>();
            }
            return res;
        }
        protected internal async Task<Guid> IntimateINRDeposit(mFiatDepositIntimation m)
        {
            Guid res = Guid.Empty;
            var _endPoint = $"Wallet/IntimateINRDeposit";

            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, m);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<Guid>();
            }
            else { Console2.WriteLine_RED($"ERROR:IntimateINRDeposit has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        protected internal async Task<mWithdrawRequestResult> RequestINRWithdraw(mFiatWithdrawRequest m)
        {
            mWithdrawRequestResult res =null;
            var _endPoint = $"Wallet/RequestINRWithdraw";

            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, m);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mWithdrawRequestResult>();
            }
            else { Console2.WriteLine_RED($"ERROR:RequestINRWithdraw has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        protected internal async Task<mWithdrawRequestResult> RequestCryptoTokenWithdraw(mCryptoWithdrawRequest m)
        {
            mWithdrawRequestResult res = null;
            var _endPoint = $"Wallet/RequestCryptoTokenWithdraw";

            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, m);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mWithdrawRequestResult>();
            }
            else { Console2.WriteLine_RED($"ERROR:RequestCryptoTokenWithdraw has failed in Response with message:{response.ToJson()}"); }
            return res;
        }

        #region Log Messages
        protected async internal void LogEvent(string msg)
        {
            try
            {
                //ToDo: naveen Report Event for Infra Dashbaoard
                Console2.WriteLine_White($"EVENT:{msg}");
                return;
                vmLog vL = new vmLog() { Message = msg };
                var _endPoint = $"Watcher/ReportEvent";
                await GetLogAPIChannel().PostAsJsonAsync(_endPoint, vL);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LogEvent Casued an Exception at..{DateTime.UtcNow.GetCurrentUnix()}\n\t{ex.GetDeepMsg()}");

            }

        }

        protected internal void LogError(string msg)
        {
            vmLog vL = new vmLog() { Message = msg };
            var _endPoint = $"Watcher/ReportError";
            GetLogAPIChannel().PostAsJsonAsync(_endPoint, vL);
        }
        #endregion


        /// <summary>
        /// this function work for update user google uniqe code
        /// </summary>
        /// <returns></returns>
        protected internal async Task<bool> UpdateUserGCodeFor2Factor(string UserName, string GCode)
        {
            bool res = false;
            var _endPoint = $"User/IncludeMFAuthenticator?uName={UserName}&Code={GCode}";
            //var dataContant = JsonContent.Create(new { uName = UserName, Code = GCode });
            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, UserName);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            return res;
        }

        protected internal async Task<bool> EnableMultiFactor(string uName)
        {
            bool res = false;
            var _endPoint = $"User/EnableMultiFactor?uName={uName}";
            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, uName);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            return res;
        }

        /// <summary>
        /// this function work for Disable Multifactor
        /// </summary>
        /// <returns></returns>
        protected internal async Task<bool> DisabledMultiFactor(string uName)
        {
            bool res = false;
            var _endPoint = $"User/DisableMultiFactor?uName={uName}";
            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, uName);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            return res;
        }


        protected internal async Task<mOnDemandRequestResult> GetOnDemandCheckNetworkTx(string WalletAddress, Guid networkId, string TxHash)
        {
            mOnDemandRequestResult res = mOnDemandRequestResult.Placed;

            var _endPoint = $"Wallet/OnDemandCheckNetworkTx?WalletAddress={WalletAddress}&networkid={networkId}&txhash={TxHash}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mOnDemandRequestResult>();
            }
            else { Console2.WriteLine_RED($"ERROR:GetOnDemandCheckNetworkTx has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        protected internal async Task<mStake> StakeTokens(mCreateStake m)
        {
            mStake res = null;

            var _endPoint = $"Wallet/StakeTokens";

            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint,m);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mStake>();
            }
            else { Console2.WriteLine_RED($"ERROR:StakeTokens has failed in Response with message:{response.ToJson()} for DataModel:{m.ToJson()}"); }
            return res;
        }
        protected internal async Task<List<mStakingSlot2>> GetStakingOpportunities()
        {
            List<mStakingSlot2> res = new List<mStakingSlot2>();

            var _endPoint = $"Staff/GetActiveStakingSlotsAll";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mStakingSlot2>>();
            }
            else { Console2.WriteLine_RED($"ERROR:GetStakingOpportunitiesAll has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        protected internal async Task<List<mStake>> GetMyStakings()
        {
            List<mStake> res = new List<mStake>();

            var _endPoint = $"Wallet/GetMyStakings";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mStake>>();
            }
            else { Console2.WriteLine_RED($"ERROR:GetMyStakings has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        protected internal async Task<mStake> RedeemMyStake(Guid StakeId)
        {
            mStake res = null;

            var _endPoint = $"Wallet/RedeemMyStake?StakeId={StakeId}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mStake>();
            }
            else { Console2.WriteLine_RED($"ERROR:RedeemMyStake has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        protected internal async Task<List<mStakingSlot2>> GetActiveStakingSlotsOf(string tCode)
        {
            List<mStakingSlot2> res = new List<mStakingSlot2>();

            var _endPoint = $"Staff/GetActiveStakingSlotsOf?tCode={tCode}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mStakingSlot2>>();
            }
            else { Console2.WriteLine_RED($"ERROR:GetActiveStakingSlotsOf:{tCode} has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        protected internal async Task<mWalletSummery> GetMyWalletSummery(Guid WalletId)
        {
            mWalletSummery res = new mWalletSummery();

            var _endPoint = $"Wallet/GetMyWalletSummery?walletId={WalletId}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mWalletSummery>();
            }
            else { Console2.WriteLine_RED($"ERROR:GetMyWalletSummery has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        //MY Orders
        protected internal async Task<List<rOrder>> GetOpenOrders(string mCode, string userAccount)
        {
            List<rOrder> res = null;

            var _endPoint = $"Order/MyOpenOrder?mCode={mCode}&uAccount={userAccount}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<rOrder>>();
            }
            else { Console2.WriteLine_RED($"ERROR:GetOpenOrders has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        protected internal async Task<List<rOrder>> GetOpenOrders(string userAccount)
        {
            List<rOrder> res = null;

            var _endPoint = $"Order/MyOpenOrdersOf?uAccount={userAccount}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<rOrder>>();
            }
            else { Console2.WriteLine_RED($"ERROR:GetOpenOrders has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        protected internal async Task<List<rOrder>> GetOrderHistory(string mCode, string userAccount)
        {
            List<rOrder> res = new List<rOrder>();

            var _endPoint = $"Order/MyOrderHistory?mCode={mCode}&uAccount={userAccount}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<rOrder>>();
                res = res ?? new List<rOrder>();
            }
            else { Console2.WriteLine_RED($"ERROR:GetOrderHistory has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        protected internal async Task<List<rOrder>> GetOrderHistory(string userAccount)
        {
            List<rOrder> res = new List<rOrder>();

            var _endPoint = $"Order/MyOrderHistoryOf?uAccount={userAccount}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<rOrder>>();
                res = res ?? new List<rOrder>();
            }
            else { Console2.WriteLine_RED($"ERROR:GetOrderHistory has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        protected internal async Task<List<mTrade>> MyRecentTrade(string userAccount, int MaxCount = 20)
        {
            List<mTrade> res = new List<mTrade>();

            var _endPoint = $"Order/MyRecentTrade?uAccount={userAccount}&count={MaxCount}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mTrade>>();
                res = res ?? new List<mTrade>();
            }
            else { Console2.WriteLine_RED($"ERROR:MyRecentTrade has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        protected internal async Task<List<mTrade>> MyRecentTradeOf(string userAccount, string mCode, int MaxCount = -1)
        {
            List<mTrade> res = new List<mTrade>();

            var _endPoint = $"Order/MyRecentTradeOf?uAccount={userAccount}&mCode={mCode}&count={MaxCount}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mTrade>>();
                res = res ?? new List<mTrade>();
            }
            else { Console2.WriteLine_RED($"ERROR:MyRecentTradeOf has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        protected internal async Task<List<mTrade>> MyRecentBuyTradeOf(string userAccount, string mCode, int MaxCount = -1)
        {
            List<mTrade> res = new List<mTrade>();

            var _endPoint = $"Order/MyRecentBuyTradeOf?uAccount={userAccount}&mCode={mCode}&count={MaxCount}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mTrade>>();
                res = res ?? new List<mTrade>();
            }
            else { Console2.WriteLine_RED($"ERROR:MyRecentBuyTradeOf has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        protected internal async Task<List<mTrade>> MyRecentBuyTrade(string userAccount, int MaxCount = -1)
        {
            List<mTrade> res = new List<mTrade>();

            var _endPoint = $"Order/MyRecentBuyTrade?uAccount={userAccount}&count={MaxCount}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mTrade>>();
                res = res ?? new List<mTrade>();
            }
            else { Console2.WriteLine_RED($"ERROR:MyRecentBuyTrade has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        //--
        protected internal async Task<List<mTrade>> MyRecentSellTradeOf(string userAccount, string mCode, int MaxCount = -1)
        {
            List<mTrade> res = new List<mTrade>();

            var _endPoint = $"Order/MyRecentSellTradeOf?uAccount={userAccount}&mCode={mCode}&count={MaxCount}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mTrade>>();
                res = res ?? new List<mTrade>();
            }
            else { Console2.WriteLine_RED($"ERROR:MyRecentSellTradeOf has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        protected internal async Task<List<mTrade>> MyRecentSellTrade(string userAccount, int MaxCount = -1)
        {
            List<mTrade> res = new List<mTrade>();

            var _endPoint = $"Order/MyRecentSellTrade?uAccount={userAccount}&count={MaxCount}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mTrade>>();
                res = res ?? new List<mTrade>();
            }
            else { Console2.WriteLine_RED($"ERROR:MyRecentSellTrade has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        protected internal async Task<List<rOrder>> GetMyOpenOrders(string mCode)
        {
            return await GetOpenOrders(mCode, myUS.UserAccount.AccountNumber);
        }
        protected internal async Task<List<rOrder>> GetMyOpenOrdersOf()
        {
            return await GetOpenOrders(myUS.UserAccount.AccountNumber);
        }
        /// <summary>
        /// (buySWAP, SellSWAP, CBRate, CBCommit,Limit,(int)cstatus))
        /// </summary>
        /// <param name="mCode"></param>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        protected internal async Task<Tuple<double, double, double, double, double, int>> GetMySwapRateAndCashback(string mCode, string userAccount)
        {
            Tuple<double, double, double, double, double, int> res = null;

            var _endPoint = $"Order/GetMySwapRateAndCashback?mCode={mCode}&uAccount={userAccount}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<Tuple<double, double, double, double, double, int>>();
            }
            else { Console2.WriteLine_RED($"ERROR:GetMySwapRate has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        protected internal async Task<double> GetCBthreashold(string mCode, string userAccount)
        {
            double res = 0;

            var _endPoint = $"Order/GetCBthreashold?mCode={mCode}&uAccount={userAccount}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<double>();
            }
            else { Console2.WriteLine_RED($"ERROR:GetCBthreashold has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        protected internal async Task<mConvertTokenRequest> ConvertToken(mConvertTokenRequest m)
        {
            mConvertTokenRequest res = null;
            var _endPoint = $"Order/ConvertToken";

            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, m);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mConvertTokenRequest>();
            }
            else { Console2.WriteLine_RED($"ERROR:ConvertToken has failed in Response with message:{response.ToJson()}"); }
            return res;
        }
        #region Transfer Tokens within Wallet
        protected internal async Task<mWithdrawRequestResult> SendTokenToExternal_Withdraw(mWithdrawINRBankBag m)
        {
            var p = _appSessionManager.ExtSession.UserSession.UserAccount;
            var sm = new mFiatWithdrawRequest
            {
                Amount = m.Amount,
                Charges = m.TechnoAppBankAccount.WithdrawlFee,
                CurrencyCode = m.TokenName,
                CurrencySymbole = m.Symbol,
                PublicRequestID = Guid.NewGuid().ToString().Replace("-", "").ToUpper(),
                KYCStatus = p.Profile.KYCStatus.ToString(),
                GEOInfo = _appSessionManager.GetGeoLOcation().ToJson(),
                uAccount = p.AccountNumber,
                Narration = m.RefNarration.IsNOT_NullorEmpty() ? m.RefNarration : "??",
                ReceiverBankDetails = m.UserBankAccount.ToJson(),
                TaxResidencyCountryCode = p.Profile.TaxResidency.Abbrivation,
                TaxResidencyCountryName = p.Profile.TaxResidency.Name,
                RequestedOn = DateTime.UtcNow,
                Status = new List<mWithdrawlRequestStatus>()
            };

            var ret = await RequestINRWithdraw(sm);
            return ret;//!= Guid.Empty ? sm : null;
        }
        protected internal async Task<mWithdrawRequestResult> SendTokenToExternal_Withdraw(mWithdrawINRUPIBag m)
        {
            var p = _appSessionManager.ExtSession.UserSession.UserAccount;
            var sm = new mFiatWithdrawRequest
            {
                Amount = m.Amount,
                Charges = m.TechnoAppUPI.WithdrawlFee,
                CurrencyCode = m.TokenName,
                CurrencySymbole = m.Symbol,
                PublicRequestID = Guid.NewGuid().ToString().Replace("-", "").ToUpper(),
                KYCStatus = p.Profile.KYCStatus.ToString(),
                GEOInfo = _appSessionManager.GetGeoLOcation().ToJson(),
                uAccount = p.AccountNumber,
                Narration = m.RefNarration.IsNOT_NullorEmpty() ? m.RefNarration:"??",
                ReceiverBankDetails = m.UserUPIAcc.ToJson(),
                TaxResidencyCountryCode = p.Profile.TaxResidency.Abbrivation,
                TaxResidencyCountryName = p.Profile.TaxResidency.Name,
                RequestedOn = DateTime.UtcNow,
                Status = new List<mWithdrawlRequestStatus>()
            };

            var ret = await RequestINRWithdraw(sm);
            return ret;//!= Guid.Empty ? sm : null;

        }
        protected internal async Task<mWithdrawRequestResult> SendTokenToExternal_Withdraw(mWithdrawNetBag m)
        {
            var p = _appSessionManager.ExtSession.UserSession.UserAccount;
            var sm = new mCryptoWithdrawRequest
            {
                Amount = m.Amount,
                Charges = m.NetFee.WithdrawalFee,
                NetworkId = m.selectedNetwork!.Value,
                NetworkName = m.NetFee.SupportedNetwork.Name,
                ReceiverAddress = m.ReceiverAddr, 
                IsAll=m.IsAll,
                TokenCode = m.NetFee.Token.Code,
                TokenId = m.NetFee.Token.TokenId,
                PublicRequestID = Guid.NewGuid().ToString().Replace("-", "").ToUpper(),
                KYCStatus = p.Profile.KYCStatus.ToString(),
                GEOInfo = _appSessionManager.GetGeoLOcation().ToJson(),
                uAccount = p.AccountNumber,
                TaxResidencyCountryCode = p.Profile.TaxResidency.Abbrivation,
                TaxResidencyCountryName = p.Profile.TaxResidency.Name,
                RequestedOn = DateTime.UtcNow,
                Status = new List<mWithdrawlRequestStatus>()
            };

            var ret = await RequestCryptoTokenWithdraw(sm);
            return ret;
        }
        protected internal async Task<bool> SendTokenFromFundingToSpot(Guid tokenId, double amt)
        {
            bool res = false;

            var _endPoint = $"Wallet/SendTokenFromFundingToSpot?tokenId={tokenId}&amt={amt}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            return res;
        }
        protected internal async Task<bool> SendTokenFromFundingToEarn(Guid tokenId, double amt)
        {
            bool res = false;

            var _endPoint = $"Wallet/SendTokenFromFundingToEarn?tokenId={tokenId}&amt={amt}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            return res;
        }
        protected internal async Task<bool> SendTokenFromSpotToFunding(Guid tokenId, double amt)
        {
            bool res = false;

            var _endPoint = $"Wallet/SendTokenFromSpotToFunding?tokenId={tokenId}&amt={amt}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            return res;
        }
        protected internal async Task<bool> SendTokenFromSpotToEarn(Guid tokenId, double amt)
        {
            bool res = false;

            var _endPoint = $"Wallet/SendTokenFromSpotToEarn?tokenId={tokenId}&amt={amt}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            return res;
        }
        protected internal async Task<bool> SendTokenFromEarnToSpot(Guid tokenId, double amt)
        {
            bool res = false;

            var _endPoint = $"Wallet/SendTokenFromEarnToSpot?tokenId={tokenId}&amt={amt}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            return res;
        }
        protected internal async Task<bool> SendTokenFromEarnToFunding(Guid tokenId, double amt)
        {
            bool res = false;

            var _endPoint = $"Wallet/SendTokenFromEarnToFunding?tokenId={tokenId}&amt={amt}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            return res;
        }
        #endregion

        internal async Task<mNetworkWallet?> GetMyNetworkWallet(Guid networkId)
        {
            mNetworkWallet res = null;

            var _endPoint = $"Wallet/GetMyNetworkWallet?networkId={networkId}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mNetworkWallet>();
            }
            else
            {
                LogError($"Method:GetMyNetworkWallet caused error at {DateTime.UtcNow}");
            }
            return res;
        }
        internal async Task<bool?> RequestToGenerateMyNetworkWallet(Guid networkId)
        {
            bool res = false;

            var _endPoint = $"Wallet/ProvisionNetworkWalletForMe?networkId={networkId}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            else
            {
                LogError($"Method:RequestToGenerateMyNetworkWallet caused error at {DateTime.UtcNow}");
            }
            return res;
        }
        internal async Task<bool?> RequestToGenerateMySmartNetworkWallet(Guid networkId)
        {
            bool res = false;

            var _endPoint = $"Wallet/ProvisionscNetworkWalletForMe?networkId={networkId}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            else
            {
                LogError($"Method:RequestToGenerateMySmartNetworkWallet caused error at {DateTime.UtcNow}");
            }
            return res;
        }
        internal async Task<List<mPreBetaPurchases>> GetMyPrebetaPurshases()
        {
            List<mPreBetaPurchases> res = new List<mPreBetaPurchases>();

            var _endPoint = $"Wallet/MyPreBetaPurchases";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mPreBetaPurchases>>();
            }
            else
            {
                LogError($"Method:GetMyPrebetaPurshases caused error at {DateTime.UtcNow}");
            }
            return res;
        }
        protected internal async Task<List<mToken>> GetActiveTokens(int count=10000)
        {
            List<mToken> res = new List<mToken>();

            var _endPoint = $"Token/GetActiveTokens?count={count}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mToken>>();
            }
            else
            {
                LogError($"Method:GetActiveTokens(int count) with count: {count} caused error at {DateTime.UtcNow}");
            }
            return res;
        }
        protected internal async Task<bool> RollTokenFavListCounter(string code, int count)
        {
            bool res = false;

            var _endPoint = $"Token/RollTokenFavListCounter?count={count}&count={count}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            else
            {
                LogError($"Method:RollTokenFavListCounter with code: {code} caused error at {DateTime.UtcNow}");
            }
            return res;
        }
        protected internal async Task<bool> RollTokenWatchListCounter(string code, int count)
        {
            bool res = false;

            var _endPoint = $"Token/RollTokenWatchListCounter?count={count}&count={count}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<bool>();
            }
            else
            {
                LogError($"Method:RollTokenWatchListCounter with code: {code} caused error at {DateTime.UtcNow}");
            }
            return res;
        }
        protected internal async Task<List<mToken>> GetAllActiveCryptoTokens()
        {
            var lst = await GetAllActiveTokens();
            if (lst == null) return lst;
            return lst.Where(x => !x.IsFiatRepresentative).ToList();
        }
        protected internal async Task<List<mToken>> GetAllActiveTokens()
        {
            List<mToken> res = new List<mToken>();

            var _endPoint = $"Token/GetAllActiveTokens";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mToken>>();
            }
            else
            {
                LogError($"Method:GetAllActiveTokens caused error at {DateTime.UtcNow}");
            }
            return res;
        }

        protected internal async Task<List<mToken>> GetAllActiveFiatTokens()
        {
            List<mToken> res = new List<mToken>();

            var _endPoint = $"Token/GetAllActiveFiatTokens";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mToken>>();
            }
            else
            {
                LogError($"Method:GetAllActiveTokens caused error at {DateTime.UtcNow}");
            }
            return res;
        }
        protected internal async Task<mTokenNetworkFee> GetTokensNetWorkFee(Guid tokenId, Guid netId)
        {
            mTokenNetworkFee res = null;

            var _endPoint = $"Token/GetTokensNetWorkFee?tokenId={tokenId}&netId={netId}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mTokenNetworkFee>();
            }
            else
            {
                LogError($"Method:GetTokensNetWorkFee caused error at {DateTime.UtcNow}");
            }
            return res;
        }
        protected internal async Task<List<mTokenNetworkFee>> GetAllTokensNetWorkFee()
        {
            List<mTokenNetworkFee> res = new List<mTokenNetworkFee>();

            var _endPoint = $"Token/GetAllTokensNetWorkFee";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mTokenNetworkFee>>();
            }
            else
            {
                LogError($"Method:GetAllTokensNetWorkFee caused error at {DateTime.UtcNow}");
            }
            return res;
        }
        protected internal async Task<mToken> GetActiveToken(Guid id)
        {
            mToken res = null;

            var _endPoint = $"Token/GetActiveToken?id={id.ToString()}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mToken>();
            }
            else
            {
                LogError($"Method:GetAllActiveTokens caused error at {DateTime.UtcNow}");
            }
            return res;
        }
        protected internal async Task<mToken> GetActiveTokenOfCode(string Code)
        {
            mToken res = null;

            var _endPoint = $"Token/GetActiveTokenOfCode?code={Code}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mToken>();
            }
            else
            {
                LogError($"Method:GetAllActiveTokens caused error at {DateTime.UtcNow}");
            }
            return res;
        }
        protected internal async Task<List<mSupportedNetwork>> GetAllSupportedNetwork()
        {
            List<mSupportedNetwork> res = new List<mSupportedNetwork>();

            var _endPoint = $"Token/GetAllSupportedNetwork";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mSupportedNetwork>>();
            }
            else
            {
                LogError($"Method:GetAllSupportedNetwork caused error at {DateTime.UtcNow}");
            }
            return res;
        }
        internal async Task<List<mFiatCurrency>> GetActiveFiat(int count)
        {
            List<mFiatCurrency> res = new List<mFiatCurrency>();

            var _endPoint = $"Token/GetActiveFiatCurrencies?count={count}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mFiatCurrency>>();
            }
            else
            {
                LogError($"Method:GetActiveFiat(int count) with count: {count} caused error at {DateTime.UtcNow}");
            }
            return res;
        }

        protected internal async Task<List<mWalletTransactions>> xxGetMyWalletTransactions(Guid WalletId)
        {
            List<mWalletTransactions> res = null;

            var _endPoint = $"Wallet/GetMyWalletTransactions?walletId={WalletId}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mWalletTransactions>>();
            }
            return res;
        }
        #endregion

        #region Market Order & Trade Functions
        /// <summary>
        /// this function work for send otp
        /// </summary>
        /// <returns></returns>
        protected async Task<Tuple<bool, string>> BuildAndPlace(mOrder order)
        {
            var res = new Tuple<bool, string>(false, "Technical failure...");
            var _endPoint = $"Order/BuildAndPlaceOrder";

            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, order);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<Tuple<bool, string>>();
            }
            return res;
        }
        internal async Task<Tuple<bool, string>> CancelMyOrder(string orderId, string mCode)
        {
            var res = new Tuple<bool, string>(false, "Technical failure...");
            var _endPoint = $"Order/CancelMyOrder?orderId={orderId}&mCode={mCode}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<Tuple<bool, string>>();
            }
            else
                Console2.WriteLine_RED($"ERROR:CancelOrder for OrderId:{orderId} in market:{mCode} caused Technical Failure at..{DateTime.UtcNow}");
            return res;
        }
        public async Task<mMarket?> GetMarketPair(string name)
        {
            var _endPoint = $"Market/GetMarketPairByCode?mCode={name}";
            mMarket market = null;

            //GET Method
            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                market = await response.Content.ReadFromJsonAsync<mMarket>();
            }
            return market;
        }
        public async Task<List<mMarket?>> GetActiveMarketsForCountry(string Abbr)
        {
            var _endPoint = $"Market/GetActiveMarketsForCountry?Abbrivation={Abbr}";
            List<mMarket> market = new List<mMarket>();

            //GET Method
            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                market = await response.Content.ReadFromJsonAsync<List<mMarket>>();
            }
            return market;
        }
        #endregion

        #region Static Content
        protected internal async Task<List<mFAQDisplay>> GetAllApprovedFAQs()
        {
            var result = new List<mFAQDisplay>();
            var _endPoint = $"FAQs/GetApprovedFAQs";
            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<List<mFAQDisplay>>();
            }
            return result;
        }
        protected internal async Task<mJD> GetApprovedJDByRef(string RefCode)
        {
            var result = new mJD();
            var _endPoint = $"Career/GetPublishedJD?RefNo={RefCode}";
            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<mJD>();
            }
            return result;
        }
        protected internal async Task<bool> UpdateJDUser(Guid Id)
        {
            bool result = false;
            var _endPoint = $"Career/UpdateJDUser?id={Id}";
            var ch = GetMaintAPIChannel();
            HttpResponseMessage response = await ch.GetAsync(_endPoint);

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<bool>();
            }
            return result;
        }
        #endregion
        internal string ImageString(IFormFile file)
        {
            string str = "";
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                str = ImageString(ms.ToArray(), Path.GetExtension(file.FileName));
            }
            return str;
        }

        internal string ImageString(byte[] imageBytes, string ext)
        {

            string base64String = Convert.ToBase64String(imageBytes, 0, imageBytes.Length);
            string imageUrl = string.Concat($"data:image/{ext};base64,", base64String);
            return imageUrl;
        }
    }
}
