using System.Collections.Concurrent;
namespace TechnoApp.Ext.Web.UI.Service
{
    public class WebUIBGService : IHostedService
    {
        List<Task> tasks = new List<Task>();
        CancellationTokenSource cts = new CancellationTokenSource();

        async Task IHostedService.StartAsync(CancellationToken cToken)
        {
            try
            {
                foreach (var w in AppWorkerFactory.workers)
                {
                    tasks.Add(Task.Run(async () => await w.DoBase(cToken)));
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                //Log Error
            }
        }
        async Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            if (tasks.Count <= 0 || tasks.All(t => t.IsCompleted))
                return;
            try
            {
                cts.Cancel();
            }
            finally
            {
                //Log Service Stopping with WatchDog
                await Task.WhenAll(tasks);
            }
        }
    }
    public class WatchResult
    {
        public DateTime LastUpdatedOn { get; set; }
        public ConcurrentDictionary<string, TokenPrice> Rates { get; set; } = new ConcurrentDictionary<string, TokenPrice>();

    }
    public class TokenPrice
    {
        public string TokenName { get; set; }
        public double Price { get; set; }
    }
    public class PageEventRecord
    {
        public string LTUID { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PageInstanceId { get; set; }
        public double Scroll { get; set; }
        public double ScreenHeight { get; set; }
        public string IP { get; set; }
        public string Page { get; set; }
        public string Event { get; set; }
        public DateTime At { get; set; }
    }
}

