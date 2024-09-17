using Microsoft.AspNetCore.SignalR;
using NavExM.Int.Watcher.WatchDog.Model;
using NavExM.Int.Watcher.WatchDog.Manager;
using System.Collections.Concurrent;
using System.Text.Json;

namespace NavExM.Int.Watcher.WatchDog.WHub
{
    public class ErrorHub : Hub
    {
        public async Task BroadCastError()=>
            await Clients.All.SendAsync("LogErrorData", new WatcherManager().GetLogErrorList());
    }

    public class EventHub : Hub
    {
        public async Task BroadCastEvent() =>
            await Clients.All.SendAsync("LogEventData", new WatcherManager().GetLogEventList());
    }

    public class LogHub : Hub
    {
        public async Task BroadCastLogData(List<mLogT> data) =>
            await Clients.All.SendAsync("LogData", new WatcherManager().GetLogList());
    }

    public class MyHub : Hub
    {
        public static ConcurrentDictionary<string, MyUserType> MyUsers = new ConcurrentDictionary<string, MyUserType>();

        public override async Task<Task> OnConnectedAsync()
        {
            MyUsers.TryAdd(Context.ConnectionId, new MyUserType() { ConnectionId = Context.ConnectionId, Headers= JsonSerializer.Serialize(Context.GetHttpContext().Request.QueryString) });
            PushData();
            return  base.OnConnectedAsync();
        }
        public override async Task<Task> OnDisconnectedAsync(Exception? stopCalled)
        {
            MyUserType garbage;
            MyUsers.TryRemove(Context.ConnectionId, out garbage);
            return base.OnDisconnectedAsync(stopCalled);
        }

        public void PushData()
        {
            //Values is copy-on-read but Clients.Clients expects IList, hence ToList()
            Clients.Clients(MyUsers.Keys.ToList()).SendAsync("xza");
        }
    }

    public class MyUserType
    {
        public string ConnectionId { get; set; }
        public string Headers { get; set; }
        // Can have whatever you want here
    }

    // Your external procedure then has access to all users via MyHub.MyUsers
    // or
}
