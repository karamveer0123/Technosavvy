using Microsoft.AspNetCore.SignalR.Client;
using TechnoApp.Ext.Web.UI.Model;
using NuGet.Protocol.Plugins;

namespace TechnoApp.Ext.Web.UI.Service;

/* Things to Do
 * 1. User Session should be Routed from UM Services
 *      - To Ensure Multi Session Counter
 *      - Wallet Balance
 *      - Pending Compliance Acknoledgement
 * 2. Session should now have session counter to support multi sessions
 * 3. All MY Functions should reload the Data from database
 * 4. Wallet Balance if existing session should always flow from TradeAPI
 */
internal class SrvBroadcast : SvcBase
{
    /* This Class is in the Web.UI for SignalR Broadcast to connected Users
     * 1. Wallet Balance for logged-in Users
     *      - External Payments Confirmation (In & Out)
     *      - MarketTrade Order Resulted in Change
     * 2. Dashboard Update for
     *      - Coin Price chnage/Performance
     *      - Message Notification
     *      - Live Session Termination/Notification
     * 3. 
     */
    int UpdateEvery = 10;//Seconds
    static WatchResult wResult = new WatchResult();
    static HubConnection conn;
    DateTime startAttempted = DateTime.MaxValue;
    static DateTime conLastCheck = DateTime.MinValue;
    internal static string ConState()
    {
        if (conn != null)
        {
            if (conn.State == HubConnectionState.Disconnected)
            {
                if (conLastCheck.AddSeconds(10) < DateTime.UtcNow)
                {
                    conLastCheck = DateTime.UtcNow;
                    conn = null;
                    return HubConnectionState.Disconnected.ToString();
                }
            }
            return conn.State.ToString();
        }
        else
        {
            return "Null";
        }
    }
    protected override async Task DoStart()
    {
        pulse = 10000;
        if (conn == null || startAttempted.AddSeconds(5) < DateTime.UtcNow)
        {
            string url = "";
            if ($"{AppConstant.MaintenanceAPI}".EndsWith("/"))
                url = AppConstant.MaintenanceAPI;
            else
                url = AppConstant.MaintenanceAPI + "/";

              url = $"{url}MAPIStream";
            Console2.WriteLine_White($"MAPI SignalR URL Attempting to connect is:{url}");
            conn = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();

            conn.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                Console2.WriteLine_White($"WebUI SignalR status:{conn.State} at {DateTime.UtcNow}");
                await conn.StartAsync();
                startAttempted = DateTime.UtcNow;
            };
            Connect();
            Console2.WriteLine_White($"WebUI SignalR status:{conn.State} at {DateTime.UtcNow}");
        }
        await Task.CompletedTask;
    }
    private async void Connect()
    {
        conn.On<string, string>("OrderTradeUpdate", async (uAccount, mCode) =>
        {
            APIHub.PublishLog($"MAPI stated that:{mCode} has some Update to Report On");
            uAccount = uAccount.IsNOT_NullorEmpty() ? uAccount.ToLower() : uAccount;
            await APIHub.UpdateClientIfAny(uAccount, mCode);
            Console2.WriteLine_White($"SignalR Reported for:{uAccount}.{mCode} at {DateTime.UtcNow}");

        });

        try
        {
            await conn.StartAsync();
        }
        catch (Exception ex)
        {
            Console2.WriteLine_RED($"SignalR WebUI Connect Error:\n{ex.GetDeepMsg()}");
        }
    }

}

internal class SrvPrebeta : SvcBase
{

    int UpdateEvery = 10;//Seconds
    static mPreBetaStats stats = new mPreBetaStats();
    DateTime LastUpdate = DateTime.MinValue;


    protected override async Task DoStart()
    {
        pulse = 60000;
        if (LastUpdate.AddMinutes(1) < DateTime.UtcNow)
        {
            LastUpdate = DateTime.UtcNow;
            stats = await GetPreBetaStats();
            Console2.WriteLine_Green($"Info:AfterFactor:Status Updated Received..at{DateTime.UtcNow}");

            // Console2.WriteLine_White($"WebUI SignalR status:{conn.State} at {DateTime.UtcNow}");
        }
        await Task.CompletedTask;
    }
    internal static mPreBetaStats GetPrebetaStats { get => stats; }
    internal static bool CompliantCountry(string ctCode)
    {
        if (ctCode.IsNullOrEmpty()) return true;
        ctCode = ctCode.ToUpper();
       return  !NegativeCountry.ToList().Any(x => x == ctCode);
    }
    //ToDo: Naveen Use Nagative Country List from Database
    static string[] NegativeCountry { get => new string[] {"VC" };  }
   // static string[] NegativeCountry { get => new string[] {"AD","SVG","AF" }; }
}