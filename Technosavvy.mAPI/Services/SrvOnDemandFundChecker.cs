using RabbitMQ.Client;
using System.Collections.Concurrent;
using System.Text;

namespace NavExM.Int.Maintenance.APIs.Services;

internal class SrvOnDemandFundChecker : AppConfigBase
{

    public string Ex_OnDemandTxCheck { get => $"NavExM.Wallet.EthMainNet.Trans.OnDemandCheck"; }

    static ConcurrentQueue<smMainNetWCheck> OnDemanTxCheckReqQue = new ConcurrentQueue<smMainNetWCheck>();
    bool isDisplay;
    protected override async Task DoStart()
    {
        // await Task.Delay(2000);
        if (RegistryToken is null) return;
        Loop_OnDemandCheckPublisher();
        await Task.CompletedTask;
    }
    Mutex mOnDemandCheck = new Mutex();
    bool IsOnDemandRunning;
    void Loop_OnDemandCheckPublisher()
    {
        mOnDemandCheck.WaitOne();
        if (!IsOnDemandRunning)
        {
            new Thread(async () =>
            {
                IsOnDemandRunning = true;
                while (true)
                {
                    try
                    {
                        Thread.Sleep(2000);
                        while (OnDemanTxCheckReqQue.TryDequeue(out var Req))
                        {
                            publishNetworkTxCheck(Req);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in Loop_OnDemandCheckPublisher:\n{ex.GetDeepMsg()}");
                    }
                }
                IsOnDemandRunning = false;
            }).Start();
        }
        mOnDemandCheck.ReleaseMutex();
    }
   
    internal static bool RequestNetworkTxCheck(smMainNetWCheck Req)
    {
        OnDemanTxCheckReqQue.Enqueue(Req);
        return true;
    }
    IModel ch_NetworkCheck;
    void publishNetworkTxCheck(smMainNetWCheck fn)
    {
        //ToDo: Naveen, Incase Multipal network, Do Network Proxy Check here and Publish to Related Exchange
        ch_NetworkCheck = Ctn_CreateModel(ch_NetworkCheck);
        var str = System.Text.Json.JsonSerializer.Serialize(fn);
        Console2.WriteLine_RED($"Publishing Network Tx Check:{str}");
        var bdy = Encoding.UTF8.GetBytes(str);
        ch_NetworkCheck.ExchangeDeclare(Ex_OnDemandTxCheck, ExchangeType.Direct);
        ch_NetworkCheck.BasicPublish(exchange: Ex_OnDemandTxCheck, routingKey: "",
            basicProperties: null, body: bdy);

        Console2.WriteLine_RED($"{T}:Tx Check Request Published:{fn.TxHash}  at..{DateTime.UtcNow}");
    }
    public class smMainNetWCheck
    {
        public string WalletAddress { get; set; }
        public string TxHash { get; set; }
        public string NetworkProxy { get; set; }

    }
}
