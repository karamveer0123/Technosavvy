using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;
using NavExM.Int.Watcher.WatchDog.Data;
using NavExM.Int.Watcher.WatchDog.Manager;
using NavExM.Int.Watcher.WatchDog.Model;
using NavExM.Int.Watcher.WatchDog.WHub;

namespace NavExM.Int.Watcher.WatchDog.Service
{
    public class LogBackgroundCaller : BackgroundService
    {
        bool isDisplayed = false;
        static IConfiguration _configuration;
        //ApiAppContext _WatchDB;
        public WatcherManager _watcherMgr;
        public IHubContext<ErrorHub> _errorHub { get; }
        public IHubContext<EventHub> _eventHub { get; }
        public IHubContext<LogHub> _logHub { get; }
        public IHubContext<MyHub> _myHub { get; }
        public LogBackgroundCaller(IHubContext<ErrorHub> errorHub, IHubContext<EventHub> eventHub, IHubContext<LogHub> logHub, IHubContext<MyHub> myhub, IConfiguration configuration)
        {
            _configuration = configuration;
            Instance = this;
            _errorHub = errorHub;
            _eventHub = eventHub;
            _logHub = logHub;
            _myHub = myhub;
        }
        public static LogBackgroundCaller Instance { get; private set; }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //if (!isDisplayed)
                //    Console.WriteLine("Time Relay in On");
                //await RelayTimeData();
                //await RelayMyData();
                //GetDBContext();
                //_watcherMgr = GetWatcheraManager();
                if (!true)
                {
                    await RelayErrorLogData();
                    await RelayEventLogData();
                    await RelayLogData();
                }

                await Task.Delay(10000);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        private async Task RelayErrorLogData()
        {
            var a = LogBackgroundCaller.Instance;
            List<mLogT> data = new List<mLogT>();
            try
            {
                data = a._watcherMgr.GetLogErrorList();
                await _errorHub.Clients.All.SendAsync("RefreshErrorData", System.Text.Json.JsonSerializer.Serialize(data));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private async Task RelayTimeData()
        {
            try
            {
                var data = DateTime.Now.ToLongTimeString();
                await _eventHub.Clients.All.SendAsync("TimeData", System.Text.Json.JsonSerializer.Serialize(data));
                Console.WriteLine($"Sent: {data}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private async Task RelayMyData()
        {
            try
            {
                var data = MyHub.MyUsers.Keys.First();
                if (MyHub.MyUsers.Count > 0)
                {
                    var data1 = System.Text.Json.JsonSerializer.Serialize(MyHub.MyUsers);
                    await _myHub.Clients.All.SendAsync("RelayMyData", System.Text.Json.JsonSerializer.Serialize("Headers: " + data1));
                    await _myHub.Clients.All.SendAsync("RelayMyData", System.Text.Json.JsonSerializer.Serialize("Count: " + data));

                    Console.WriteLine($"Count: {System.Text.Json.JsonSerializer.Serialize(MyHub.MyUsers)}");
                }
                    else
                {
                    Console.WriteLine($"Count: 0");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async Task RelayEventLogData()
        {
            var a = LogBackgroundCaller.Instance;
            List<mLogT> data = new List<mLogT>();
            try
            {
                data = a._watcherMgr.GetLogEventList();
                var jsond = System.Text.Json.JsonSerializer.Serialize(data);
                await _eventHub.Clients.All.SendAsync("RefreshEventData", System.Text.Json.JsonSerializer.Serialize(data));
            }
            catch
            {

            }
        }

        private async Task RelayLogData()
        {
            var a = LogBackgroundCaller.Instance;
            List<mLogT> data = new List<mLogT>();
            try
            {
                data = a._watcherMgr.GetLogList();
                await _logHub.Clients.All.SendAsync("RefreshLogData", System.Text.Json.JsonSerializer.Serialize(data));
            }
            catch
            {

            }
        }

        private void GetDBContext()
        {
            //DbContextOptionsBuilder<ApiAppContext> op = new DbContextOptionsBuilder<ApiAppContext>();
            //op.UseSqlServer(_configuration.GetConnectionString("ApiDBContext"));
            //_WatchDB = new ApiAppContext(op.Options);
        }
        //private WatcherManager GetWatcheraManager()
        //{
        //    WatcherManager mgr = new WatcherManager();
        //    mgr.dbctx = _WatchDB;
        //    return mgr;
        //}

    }
}
