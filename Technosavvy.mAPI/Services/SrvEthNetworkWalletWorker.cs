using Microsoft.IdentityModel.Tokens;
using NavExM.Int.Maintenance.APIs.Extension;
using NavExM.Int.Maintenance.APIs.ServerModel;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
namespace NavExM.Int.Maintenance.APIs.Services;

internal class SrvEthNetworkWalletWorker : AppConfigBase
{
    /*Request and Receive Network Wallet Address and store the same in User Profile
     */
    public string Ex_EthWalletRequestOffice { get => $"NavExM.Wallet.EthMainNet.Wallet.Request"; }
    public string Ex_EthWalletAllocationOffice { get => $"NavExM.Wallet.EthMainNet.Wallet.Confirm"; }
    static ConcurrentQueue<smNetWalletBox> WalletReqQue = new ConcurrentQueue<smNetWalletBox>();
    bool isDisplay;
    internal static bool RequestETHNetworkWallet(smNetWalletBox Req)
    {
        WalletReqQue.Enqueue(Req);
        return true;
    }
    protected override async Task DoStart()
    {
        if (RegistryToken is null) return;
        ConName = ".WalletWorker";
        Loop_WalletRequestPublisher();
        SetUpWalletAllocationListner();
        if (!isDisplay)
        {
            LogDebug($"{RegistryToken!.AppId} Eth Wallet Worker initialized at..{DateTime.UtcNow}");
            isDisplay = true;
        }
        EnsureGlobalWallet();
        Task.Delay(2000).Wait();
    }
    Mutex mWalletReq = new Mutex();
    bool IsWalletReqRunning;
    void Loop_WalletRequestPublisher()
    {
        mWalletReq.WaitOne();
        if (!IsWalletReqRunning)
        {
            new Thread(async () =>
            {
                IsWalletReqRunning = true;
                while (true)
                {
                    try
                    {
                        Thread.Sleep(2000);
                        while (WalletReqQue.TryDequeue(out var Req))
                        {
                            //Publish Wallet Request to Channel
                            publishWalletRequest(Req);
                            Console2.WriteLine_White($"{Req.userAccount} published a request for Eth Network Wallet at {DateTime.UtcNow}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in Loop_WalletRequestPublisher:\n{ex.GetDeepMsg()}");
                    }
                }
                IsWalletReqRunning = false;
            }).Start();
            Console2.WriteLine_White($"Loop_WalletRequestPublisher initiallized.. at {DateTime.UtcNow}");
        }
        mWalletReq.ReleaseMutex();
    }
    static IModel ch_AllocationOff;
    static IModel ch_WalletRequest;

    void publishWalletRequest(smNetWalletBox Req)
    {
        //Publish cb for mining
        ch_WalletRequest = Ctn_CreateModel(ch_WalletRequest);
        var str = JsonSerializer.Serialize(Req);
        var bdy = Encoding.UTF8.GetBytes(str);
        ch_WalletRequest.BasicPublish(exchange: Ex_EthWalletRequestOffice, routingKey: "", basicProperties: null, body: bdy);
        Console2.WriteLine_DarkYellow($"{T}:A EthNetwork Wallet is Requested for {Req.userAccountId} at..{DateTime.UtcNow}");
    }
    void SetUpWalletAllocationListner()
    {
        var isClose = (ch_AllocationOff == null || ch_AllocationOff.IsClosed);
        if (!isClose) return;

        ch_AllocationOff = Ctn_CreateModel(ch_AllocationOff);
        ch_AllocationOff.ExchangeDeclare(Ex_EthWalletAllocationOffice, ExchangeType.Direct);

        var QRes = ch_AllocationOff.QueueDeclare(queue: $"{Ex_EthWalletAllocationOffice}_Qu", durable: true, exclusive: false, autoDelete: false, arguments: null);

        ch_AllocationOff.QueueBind(QRes.QueueName, Ex_EthWalletAllocationOffice, string.Empty);

        var consumer = new EventingBasicConsumer(ch_AllocationOff);
        consumer.Received += (s, e) =>
        {
            Console2.WriteLine_DarkYellow($"{RegistryToken!.AppId}|{T}:Eth Wallet Allocation has Received a New Wallet Confirmation");
            var bdy = e.Body.ToArray();
            var dt = e.DeliveryTag;
            var msg = Encoding.UTF8.GetString(bdy);
            var data = JsonSerializer.Deserialize<smNetWalletBox>(msg);
            if (data != null && data.userAccountId != Guid.Empty && data.userAccount.IsNOT_NullorEmpty())
            {
                //Save Wallet in database
                Console2.WriteLine_DarkYellow($"{RegistryToken!.AppId}|{T}: profile:{data.userAccountId} has Received an Network Wallet address {data.WalletAddress} that is to be saved in Db.. at..{DateTime.UtcNow}");
                SaveEthAddress(data);
                //ToDo: Naveen, Send Wallet Related Notification to User, Messages
                Console2.WriteLine_DarkYellow($"{RegistryToken!.AppId}|{T}: profile:{data.userAccountId} has Received an Network Wallet address {data.WalletAddress} at..{DateTime.UtcNow}");
            }
        };
        ch_AllocationOff.BasicConsume(QRes.QueueName, autoAck: true, consumer: consumer);
        Console2.WriteLine_DarkYellow($"{RegistryToken!.AppId}: Wallet Allocation Worker Set up is done at..{DateTime.UtcNow}");
    }
    DateTime LastGlabalCheck = DateTime.MinValue;
    void EnsureGlobalWallet()
    {
        if (LastGlabalCheck.AddMinutes(5) <= DateTime.UtcNow)
        {
            var wm = new WalletManager() { dbctx = dbctx() };
            wm.AddOrGetDefaultGlobalInternalWallet();
        }
    }
    bool SaveEthAddress(smNetWalletBox Addr)
    {
        var wm = new WalletManager();
        wm.dbctx = dbctx();
        return wm.SaveEthAddress(Addr);
    }
    private ApiAppContext dbctx()
    {
        var o = new DbContextOptionsBuilder<ApiAppContext>();
        o = o.UseSqlServer(ConfigExtention.Configuration.GetConnectionString("ApiDBContext"));
        return new ApiAppContext(o.Options);
    }
}
