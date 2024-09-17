using System.Collections.Concurrent;

namespace TechnoApp.Ext.Web.UI.Service
{
    internal class SrvPageEvents:SvcBase
    {
        static ConcurrentQueue<PageEventRecord> Que = new ConcurrentQueue<PageEventRecord>();
        DateTime LastUpdatedOn = DateTime.UtcNow;

        protected override async Task DoStart()
        {
        int UpdateEvery = 30;//Seconds
            if (LastUpdatedOn.AddSeconds(UpdateEvery) <= DateTime.UtcNow)
            {
                var lst=Que.ToList();
                Que.Clear();
                await ReportEvents(lst);
                LastUpdatedOn = DateTime.UtcNow;
            }
            await Task.CompletedTask;
        }
        internal static void ReportEvent(PageEventRecord ev)
        {
            Que.Enqueue(ev);
        }
    }

}

