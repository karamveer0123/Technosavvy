using TechnoApp.Ext.Web.UI.Model;
using TechnoApp.Ext.Web.UI.Models;
using TechnoApp.Ext.Web.UI.Model;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using NuGet.Protocol;

namespace TechnoApp.Ext.Web.UI.Service
{
    internal class SvcBase
    {
        internal HttpClient _maintAPIChannel;
        internal IConfiguration _configuration;
        internal HttpClient _LogAPIChannel;
        string _insID;
        protected internal int pulse = 0;

        public async Task DoBase(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (pulse > 0) await Task.Delay(pulse);
                    if (pulse <= 0) pulse = 5000;
                    await DoStart();
                }
                catch (TaskCanceledException ex)
                {
                    //ToDo:Naveen, Notify WatchDog to Switch to Secondery option
                    LogError(ex);
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            }
        }
        protected virtual async Task DoStart()
        {
            LogError("DoStart in SvcBase has been directed to Base Class. Error Id 102");
            await Task.CompletedTask;
        }

        private string InstanceId
        {
            get
            {
                _insID = _insID ?? AppConstant.InstanceId;
                return _insID;
            }
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
                ch.DefaultRequestHeaders.Add("InstanceToken", InstanceId);

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
                ch.DefaultRequestHeaders.Add("InstanceToken", InstanceId);
            }
            return _LogAPIChannel;
        }

        protected async Task<List<TokenPrice>> GetCurrencyUpdate()
        {
            var _endPoint = $"Watch/GetCurrencyValues";
            List<TokenPrice> Currency = new List<TokenPrice>();

            //GET Method
            HttpResponseMessage response = await GetMaintAPIChannel().PostAsync(_endPoint, null);
            if (response.IsSuccessStatusCode)
            {
                Currency = await response.Content.ReadFromJsonAsync<List<TokenPrice>>();
            }
            return Currency;
        }
        protected async Task ReportEvents(List<PageEventRecord> data)
        {
            var _endPoint = $"PageEvent/Report";
            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, data);
            if (response.IsSuccessStatusCode)
            {
                LogError($"Error Occoured when Reporting PageEvents");
            }
        }
        protected async Task<List<TokenPrice>> GetCoinUpdate()
        {
            var _endPoint = $"Watch/GetCoinValues";
            List<TokenPrice> Coins = new List<TokenPrice>();

            //GET Method
            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                Coins = await response.Content.ReadFromJsonAsync<List<TokenPrice>>();
                Console2.WriteLine_DarkYellow($"Token Price Update Received..");
                //Console2.WriteLine_DarkYellow($"Token Price :{Coins.ToJson()}");
            }
            Console2.WriteLine_DarkYellow($"Token Price :{Coins.ToJson()}");
            return Coins;
        }
        protected async Task<mPreBetaStats> GetPreBetaStats()
        {
            var _endPoint = $"PreBeta/GetPrebetaStats";
            mPreBetaStats stats = new mPreBetaStats();

            //GET Method
            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                stats = await response.Content.ReadFromJsonAsync<mPreBetaStats>();
                Console2.WriteLine_DarkYellow($"PreBeta Stats Received..");
                Console2.WriteLine_DarkYellow($"PreBeta :{stats.ToJson()}");
            }
            else
                Console2.WriteLine_DarkYellow($"PreBeta response failes :{stats.ToJson()}");
            return stats;
        }

        #region Log Messages
        protected internal void LogEvent(string msg)
        {
            vmLog vL = new vmLog() { Message = msg };
            var _endPoint = $"Watcher/ReportEvent";
            GetLogAPIChannel().PostAsJsonAsync(_endPoint, vL);
        }

        protected internal void LogError(string msg)
        {
            vmLog vL = new vmLog() { Message = msg };
            var _endPoint = $"Watcher/ReportError";
            GetLogAPIChannel().PostAsJsonAsync(_endPoint, vL);
        }
        protected internal void LogError(Exception ex)
        {
            vmLog vL = new vmLog() { Message = GetMsg(ex) };
            var _endPoint = $"Watcher/ReportError";
            GetLogAPIChannel().PostAsJsonAsync(_endPoint, vL);
        }

        private string GetMsg(Exception ex)
        {
            if (ex is null) return string.Empty;
            var msg = $"{ex.Message}{GetMsg(ex.InnerException)}";
            return msg;
        }
        #endregion


    }
}
