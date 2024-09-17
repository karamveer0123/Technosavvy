namespace NavExM.Int.Maintenance.APIs.Services
{
    public class MaintBGService : IHostedService
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
}
