using Microsoft.AspNetCore.SignalR;

namespace NavExM.Int.Maintenance.APIs.Services;

public class APIHub : Hub
{
    public static IHubCallerClients AllClients = null;
    static int _count = 0;
    public static int conCount { get { return _count; } }
    public override async Task<Task> OnConnectedAsync()
    {
        AllClients ??= Clients;
        await base.OnConnectedAsync();
        _count++;
        var Qstr = Context.GetHttpContext()!.Request.QueryString;
        Console2.WriteLine_White($"WebUI SignalR Client Connection Request with QString:{Qstr} at {DateTime.UtcNow}");
        return Task.CompletedTask;
    }
    public override async Task<Task> OnDisconnectedAsync(Exception? stopCalled)
    {
        Console2.WriteLine_DarkYellow($"Web Client UI Disconnect at..{DateTime.UtcNow}");
        await base.OnDisconnectedAsync(stopCalled);
        _count--;
        return Task.CompletedTask;
    }

}
