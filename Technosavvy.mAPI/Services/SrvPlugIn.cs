using System.Collections.Concurrent;

namespace NavExM.Int.Maintenance.APIs.Services
{
    /*This class is to ensure that This instance of Application is registered with WatchDog and Provide Plugged(Queue) in Error Handling
             */
    internal class SrvPlugIn : AppConfigBase
    {
        static SrvPlugIn? instance;
        static ConcurrentBag<object> ErrorHolder = new ConcurrentBag<object>();
        static ConcurrentBag<string> EventHolder = new ConcurrentBag<string>();
        static ConcurrentBag<string> DebugHolder = new ConcurrentBag<string>();
        bool displayed = false;
        public SrvPlugIn()
        {
            instance = this;
        }
        protected override async Task DoStart()
        {
            if (RegistryToken == null)
                Console.WriteLine($"PlugIn Called and State Of Registration is {RegistryToken != null} at {DateTime.Now}");
            else
            {
                if (!displayed)
                    Console.WriteLine($"PlugIn Called and State Of Registration is TRUE at {DateTime.Now}");
                displayed = true;
            }
            Task.Delay(2000).Wait();
            PublishLocalHoldings();
        }
        private void PublishLocalHoldings()
        {
            if (RegistryToken == null) return;
            ErrorHolder.ToList().ForEach(msg =>
            {
                if (msg.GetType().Name == nameof(Exception))
                    LogError((Exception)msg);
                else
                    LogError((string)msg);
            });
            ErrorHolder.Clear();
            EventHolder.ToList().ForEach(msg =>
            {
                LogEvent(msg);
            });
            EventHolder.Clear();
            DebugHolder.ToList().ForEach(msg =>
            {
                LogDebug(msg);
            });
            DebugHolder.Clear();

        }
        public static void LogErrorG(Exception ex)
        {
            if (instance != null)
                instance.LogError(ex);
            else
                ErrorHolder.Add(ex);
        }
        public static void LogErrorG(string msg)
        {
            if (instance != null)
                instance.LogError(msg);
            else
                ErrorHolder.Add(msg);
        }
        public static void LogEventG(string msg)
        {
            if (instance != null)
                instance.LogEvent(msg);
            else
                EventHolder.Add(msg);
        }
        public static void LogDebugG(string msg)
        {
            if (instance != null)
                instance.LogDebug(msg);
            else
                DebugHolder.Add(msg);
        }
    }
}

