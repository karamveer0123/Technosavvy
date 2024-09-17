using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore.Query.Internal;
using TechnoApp.Ext.Web.UI.Model;
using NuGet.Protocol;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace TechnoApp.Ext.Web.UI.Service;
public class APIHub : Hub
{
    private readonly ILogger<APIHub> _logger;
    internal IConfiguration _configuration;
    internal HttpContext _context;
    internal IHttpContextAccessor _accessor;
    internal IDataProtector _protector;
    internal AppSessionManager appSessionManager = null;
    //string userAccount = "??";
    //string conId = "??";
    public static string PageId { get => "pid".ToLower(); }
    public static string MktCode { get => "mkt".ToLower(); }
    //public static string Pluse { get => "pl".ToLower(); }
    public static IHubCallerClients AllClients = null;
    static StringBuilder sb = new StringBuilder();
    static ConcurrentDictionary<string, SignalRuser> AllUsers = new ConcurrentDictionary<string, SignalRuser>();

    [AfterProfile]
    public override async Task<Task> OnConnectedAsync()
    {
        AllClients ??= Clients;
        if (!IsStartup)
            Publish_StartUpPackage();
        await base.OnConnectedAsync();
        bool isError = true;
        appSessionManager = new AppSessionManager(Context.GetHttpContext());

        try
        {
            var u = new SignalRuser();
            await appSessionManager.ExtSession.LoadSession();
            string userAccount = appSessionManager.ExtSession.UserSession?.UserAccount.AccountNumber;
            var spW = appSessionManager.ExtSession.UserSession?.SpotWalletId;
            u.SpotWallet = spW!.Value;
            //--
            var mCode = GetValue(MktCode);
            var mm = GetMarketManager();
            var mkt = await mm.GetMarketPair(mCode);
            if (mkt != null && !mkt.IsTradingAllowed)
            {
                await Clients.Caller.SendAsync("LogReceiver", $"Subscription failed...!. Invalid Market:{mCode.ToLower()}");
            }
            u.contextId = Context.ConnectionId;
            userAccount = userAccount.ToLower();
            u.userAccount = userAccount;
            u.mCode = mCode.ToLower();
            await AddToGroup(u.contextId);
            await AddToGroup(userAccount);
            await AddToGroup($"{userAccount}.{mCode.ToLower()}");
            await Clients.Caller.SendAsync("LogReceiver", $"Subscription granted...to User:{userAccount}\n Market:{DateTime.UtcNow}");
            InitialPackage.Enqueue(u);
            AllUsers.TryAdd(u.userAccount.ToLower(), u);
            await Clients.Caller.SendAsync("LogReceiver", $"Initial Loading..{mCode.ToLower()}");
            Console2.WriteLine_White($"{T}|Serving Users:{AllUsers.Count()} as of..{DateTime.UtcNow}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection Error...:{ex.GetDeepMsg()}");
            await Clients.Caller.SendAsync("LogReceiver", $"Subscription Error...!\n Market Allowed:{ex.GetDeepMsg()}");
        }
        return Task.CompletedTask;
    }
    public override async Task<Task> OnDisconnectedAsync(Exception? stopCalled)
    {
        AllUsers.TryRemove(Context.ConnectionId, out var gb);
        if (gb != null)
            Console2.WriteLine_DarkYellow($"Disconnect called for User:{gb.userAccount}");
        return base.OnDisconnectedAsync(stopCalled);
    }
    static Mutex pubLoc = new Mutex();
    static bool IsPub;

    static DateTime lastPub = DateTime.MinValue;

    internal static void CancelMyOrder(string uAcc, string Id)
    {
        //ToDo: Establish Ownership of the order
        //Estabvlish Session
        var key = $"--";
        if (uAcc.IsNOT_NullorEmpty())
            key = $"{uAcc.ToLower()}";
        Console2.WriteLine_Green($"CancelMyOrder called on:{key} with Order InternalId:{Id}");
        AllUsers.TryGetValue(key, out var SrU);
        if (SrU != null && AllClients != null)
        {

        }
    }
    static void Test_PublishLoop()
    {
        pubLoc.WaitOne();
        if (!IsPub)
        {
            new Thread(() =>
            {
                IsPub = true;
                try
                {
                    while (true)
                    {
                        if (lastPub.AddSeconds(2) < DateTime.UtcNow)
                        {
                            lastPub = DateTime.UtcNow;


                            //Console.WriteLine($" Market Allowed:{PublicDataRepo.GetAllMarkets().Count()}|{sb}");
                            //PublishLog($"Server says {lastPub}");
                            //Console.WriteLine($"Server says {lastPub}");

                        }
                    }
                }
                catch (Exception ex)
                {


                }
                IsPub = false;
            }).Start();
        }
        pubLoc.ReleaseMutex();
    }
    #region Publish StartUP Package for new Joinning Users
    static bool IsStartup;
    static ConcurrentQueue<SignalRuser> InitialPackage = new ConcurrentQueue<SignalRuser>();
    static int last = 0;
    static void Publish_StartUpPackage()
    {
        pubLoc.WaitOne();
        if (!IsStartup)
        {
            new Thread(() =>
            {
                IsStartup = true;
                try
                {
                    while (true)
                    {
                        if (last != DateTime.UtcNow.Second && (DateTime.UtcNow.Second % 5) == 0)
                        {
                            last = DateTime.UtcNow.Second;
                            Console2.WriteLine_White($"Processing Loop has Begun|Initial Package Count is {InitialPackage.Count} Users To process..at:{DateTime.UtcNow}");
                            Console2.WriteLine_White($"Connected Users are:{APIHub.AllUsers.Count} Users To process..at:{DateTime.UtcNow}");
                            Console2.WriteLine_White($"MAPI SignalR Connection State is:{SrvBroadcast.ConState()}..at:{DateTime.UtcNow}");
                        }
                        while (InitialPackage.TryDequeue(out var NewUser))
                        {
                            try
                            {
                                Console2.WriteLine_White($"{NewUser.mCode} has been DeQueue for Processing....at:{DateTime.UtcNow}");
                                new Thread(async () => await PublishOpenOrder(NewUser)).Start();
                                new Thread(async () => await PublishOrderHistory(NewUser)).Start();
                                new Thread(async () => await PublishSpotWalletSummary(NewUser)).Start();
                                new Thread(async () => await PublishRecentTrades(NewUser)).Start();
                                new Thread(async () => await PublishMyTradeHistory(NewUser)).Start();

                            }
                            catch (Exception ex)
                            {
                                Console2.WriteLine_RED($"ERROR in Publish_StartUpPackage:{ex.GetDeepMsg()}");

                            }
                        }
                    }
                }
                catch (Exception ex)
                {


                }
                IsStartup = false;
            }).Start();
        }
        pubLoc.ReleaseMutex();
    }
    static async Task PublishOpenOrder(SignalRuser user)
    {
        try
        {
            if (AllClients == null) return;
            //This new Session of this User ONLY
            var lst = AllClients.Group($"{user.userAccount.ToLower()}.{user.mCode.ToLower()}");
            if (lst != null)
            {
                var wm = new WalletManager();
                var ord = await wm.GetOpenOrders(user.mCode, user.userAccount);
                if (ord.Count > 10)
                    ord = ord.OrderByDescending(x => x.PlacedOn).Take(10).ToList();
                await lst.SendAsync("OpenOrder", ord);
                Console2.WriteLine_White($"{T}| userAccount:{user.userAccount} for Markert:{user.mCode} has been sent:{ord.Count} OpenOrders");
            }
        }
        catch (Exception ex)
        {
            Console2.WriteLine_RED($"ERROR in PublishOpenOrder:{ex.GetDeepMsg()}");
        }
    }
    static async Task PublishSpotWalletSummary(SignalRuser user)
    {
        try
        {
            if (AllClients == null) return;
            //This new Session of this User ONLY
            var lst = AllClients.Group($"{user.userAccount.ToLower()}.{user.mCode.ToLower()}");
            if (lst != null)
            {
                var wm = new WalletManager();
                var spot = await wm.GetMyWalletSummery(user.SpotWallet);
                await lst.SendAsync("Funds", spot);
                Console2.WriteLine_White($"{T}| userAccount:{(user != null ? user.userAccount : "Null")} for Markert:{(user != null ? user.mCode : "Null")} has been sent SpotWallet Summary:{(spot != null && spot.Tokens != null ? spot.Tokens.Count : 0)} Tokens");
            }
        }
        catch (Exception ex)
        {
            Console2.WriteLine_RED($"ERROR in PublishSpotWalletSummary:{ex.GetDeepMsg()}");
        }

    }
    static async Task PublishRecentTrades(SignalRuser user)
    {
        try
        {
            if (AllClients == null) return;
            //This new Session of this User ONLY
            var lst = AllClients.Group($"{user.userAccount.ToLower()}.{user.mCode.ToLower()}");
            if (lst != null)
            {
                var wm = new WalletManager();
                var trades = await wm.MyRecentTrade(user.userAccount, 20);
                await lst.SendAsync("RecentTrade", trades);
                Console2.WriteLine_White($"{T}| userAccount:{user.userAccount} has been sent Recent {trades.Count} Trades");
            }
        }
        catch (Exception ex)
        {
            Console2.WriteLine_RED($"ERROR in PublishRecentTrades:{ex.GetDeepMsg()}");
        }
    }
    static async Task PublishOrderHistory(SignalRuser user)
    {
        try
        {
            if (AllClients == null) return;
            //This new Session of this User ONLY
            var lst = AllClients.Group($"{user.userAccount.ToLower()}.{user.mCode.ToLower()}");
            if (lst != null)
            {
                var wm = new WalletManager();
                var ord = await wm.GetOrderHistory(user.mCode, user.userAccount);
                await lst.SendAsync("OrderHistory", ord);
                Console2.WriteLine_White($"{T}| userAccount:{user.userAccount} for Markert:{user.mCode} has been sent:{ord.Count} Orders");
            }
        }
        catch (Exception ex)
        {
            Console2.WriteLine_RED($"ERROR in PublishOrderHistory:{ex.GetDeepMsg()}");
        }
    }
    static async Task PublishMyTradeNotification(SignalRuser user)
    {
        try
        {
            //Seek and Publish To Client (BUYER & SELLER), Trade after last Timestamp
            //ToDo:, Not here,Add Internal Bell Notification (BUYER & SELLER)

            if (AllClients == null) return;
            //This new Session of this User ONLY 1005242
            var lst = AllClients.Group($"{user.userAccount.ToLower()}.{user.mCode.ToLower()}");
            if (lst != null)
            {
                var wm = new WalletManager();
                var trades = await wm.MyRecentTrade(user.userAccount, 20);
                await lst.SendAsync("RecentTrade", trades);
                Console2.WriteLine_White($"{T}| userAccount:{user.userAccount} has been sent Recent {trades.Count} Trades");
            }
        }
        catch (Exception ex)
        {
            Console2.WriteLine_RED($"ERROR in PublishOpenOrder:{ex.GetDeepMsg()}");
        }
    }
    static async Task PublishMyTradeHistory(SignalRuser user)
    {
        try
        {
            if (AllClients == null) return;
            //This new Session of this User ONLY 1005242
            var lst = AllClients.Group($"{user.userAccount.ToLower()}.{user.mCode.ToLower()}");
            if (lst != null)
            {
                var wm = new WalletManager();
                var trades = await wm.MyRecentTradeOf(user.userAccount, user.mCode, 20);
                await lst.SendAsync("TradeHistory", trades);
                Console2.WriteLine_White($"{T}| userAccount:{user.userAccount} has been sent Trade History of {trades.Count} Trades");
            }
        }
        catch (Exception ex)
        {
            Console2.WriteLine_RED($"ERROR in PublishOpenOrder:{ex.GetDeepMsg()}");
        }
    }
    #endregion

    public static async Task UpdateClientIfAny(string uAcc, string mCode)
    {
        Console2.WriteLine_White($"UpdateClientIfAny Called ..at:{DateTime.UtcNow}");
        try
        {
            var key = $"--";
            if (uAcc.IsNOT_NullorEmpty())
                key = $"{uAcc.ToLower()}";
            Console2.WriteLine_White($"Scheduling key:{key}..at:{DateTime.UtcNow}");
            Console2.WriteLine_White($"Scheduling Count is:{AllUsers.Count}..at:{DateTime.UtcNow}");
            Console2.WriteLine_White($"Scheduling AllClients NUll is:{AllClients == null}");
            AllUsers.TryGetValue(key, out var SrU);
//#if (DEBUG)
//            if (SrU == null)
//            {
//                SrU = AllUsers.Values.FirstOrDefault();
//            }
//#endif

            if (SrU != null && AllClients != null)
            {
                Console2.WriteLine_White($"SignalR User Object NULL status is:{SrU == null}");
                if (AllClients.Group(key) == null)
                {
                    AllUsers.Remove(key, out var gSrU);
                }
                else
                {
                    PublishLog($"{SrU.userAccount} has been Scheduled to be Reported On");
                    if (!InitialPackage.ToList().Any(x => x.userAccount == key))
                        InitialPackage.Enqueue(SrU);
                }
            }
            else
            {
                Console2.WriteLine_RED($"SignalR User Key:{key} Requested to update client is Null");
            }
        }
        catch (Exception ex)
        {
            Console2.WriteLine_RED($"Client Notification Error:\n{ex.GetDeepMsg()}");
        }
    }
    public static void PublishLog(string str, string mCode = "")
    {
        try
        {
            var lst = AllClients != null ? mCode.IsNOT_NullorEmpty() ? AllClients.Group($"{str}.{mCode.ToLower()}") : AllClients.All : null;
            if (lst != null)
            {
                lst.SendAsync("LogReceiver", $"{str}..at {DateTime.UtcNow}");
                Console.WriteLine("Message Sent");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR in LogReceiver:{ex.GetDeepMsg()}");

        }
    }
    public string ServerHello()
    {
        return $"Bye at {DateTime.UtcNow}";
    }



    public async Task<bool> AddToGroup(string gName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, gName);
        return true;
    }
    public async Task<bool> ChangeGroup(string fromGName, string toGName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, fromGName);
        await Groups.AddToGroupAsync(Context.ConnectionId, toGName);
        return true;
    }
    private string GetValue(string key, bool optional = false, string optionalVal = "")
    {
        if (!Context.GetHttpContext()!.Request.Query.TryGetValue(key, out var retval))
        {
            if (optional)
            {
                Console.WriteLine($"Subscription Error: for key:{key}");
                Clients.Caller.SendAsync("LogReceiver", "Subscription failed...!");
                throw new ApplicationException("invalid connection request");
            }
            else return optionalVal;
        }
        return retval.ToString().ToLower();
    }
    //public static Tuple<IClientProxy, int> GetClients(string grp)
    //{
    //    var my = AllUsers.Where(x => x.Value.Item2 == grp.ToLower()).ToList().Select(x => x.Value.Item1).ToList().AsReadOnly();

    //    return Tuple.Create(AllClients.Clients(my), my.Item1.Count);

    //}
    static string T { get => $"T:{Thread.CurrentThread.ManagedThreadId}"; }

    internal MarketManager GetMarketManager()
    {
        var Mgr = new MarketManager();
        Mgr._configuration = _configuration;
        Mgr._http = Context.GetHttpContext(); //_accessor.HttpContext;

        Mgr._appSessionManager = appSessionManager;
        Mgr._DataProtector = _protector;
        return Mgr;
    }

    internal WalletManager GetWalletManager()
    {
        var Mgr = new WalletManager();
        Mgr._configuration = _configuration;
        Mgr._http = Context.GetHttpContext(); //_accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        Mgr._DataProtector = _protector;
        return Mgr;
    }
}
class SignalRuser
{
    public DateTime startedOn { get; set; }
    public DateTime lastSent { get; set; }
    public string userAccount { get; set; }
    public string sessionHash { get; set; }
    public Guid SpotWallet { get; set; }
    public string mCode { get; set; }
    public string contextId { get; set; }
    public string myAllSessionGroup { get => $"{userAccount}.{mCode.ToLower()}"; }
    //Market Switching is allowed
    public string thisSessionOnly { get => $"{contextId}.{mCode.ToLower()}"; }
}