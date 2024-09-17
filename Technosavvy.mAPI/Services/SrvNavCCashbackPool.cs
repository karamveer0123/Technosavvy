using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace NavExM.Int.Maintenance.APIs.Services;

internal class SrvNavCCashbackPool : AppConfigBase
{
    IModel? Ch_NavC;
    public string Ex_NavC { get => $"NavExM.Market.NavC.Price"; }
    public string Ex_CashBack { get => $"NavExM.Market.Cashback"; }
    /// <summary>
    /// Every 3 Sec Update of NavC FloorPrice- Trade Control
    /// </summary>
    public static NavCStatices LastNavCBLOCK = new NavCStatices();
    /// <summary>
    /// Current Cashbasck cycle
    /// </summary>
    public static CBCycle CurrentCBCycle = new CBCycle();

    public static bool IsCBCalculationBegin = false;
    public static bool IsRefCalculationBegin = false;

    protected override async Task DoStart()
    {
        if (RegistryToken is null) return;
        NavCPriceListner();
        CashBackCycleListner();
    }
    public void NavCPriceListner()
    {
        var isClose = (Ch_NavC == null || Ch_NavC.IsClosed);
        if (!isClose) return;

        Ch_NavC = Ctn_CreateModel(Ch_NavC);
        Ch_NavC.ExchangeDeclare(Ex_NavC, ExchangeType.Fanout);

        var QRes = Ch_NavC.QueueDeclare(queue: "", durable: false, exclusive: true, autoDelete: true, arguments: null);

        Ch_NavC.QueueBind(QRes.QueueName, Ex_NavC, string.Empty);

        var consumer = new EventingBasicConsumer(Ch_NavC);
        consumer.Received += (s, e) =>
        {
            var bdy = e.Body.ToArray();
            var dt = e.DeliveryTag;
            var msg = Encoding.UTF8.GetString(bdy);
            var data = JsonSerializer.Deserialize<NavCStatices>(msg);
            if (data != null)
            {
                if (LastNavCBLOCK == null || data.Start >= LastNavCBLOCK.Start)
                {
                    //
                    LastNavCBLOCK = data;
                }
            }
        };
        Ch_NavC.BasicConsume(QRes.QueueName, autoAck: true, consumer: consumer);
        LogEvent($"{RegistryToken!.AppId} NavC Listner Setup for Data.Broker");
    }


    IModel Ch_CBack;
    public void CashBackCycleListner()
    {
        var isClose = (Ch_CBack == null || Ch_CBack.IsClosed);
        if (!isClose) return;

        Ch_CBack = Ctn_CreateModel(Ch_CBack);
        Ch_CBack.ExchangeDeclare(Ex_CashBack, ExchangeType.Fanout);

        var QRes = Ch_CBack.QueueDeclare(queue: "", durable: false, exclusive: true, autoDelete: true, arguments: null);

        Ch_CBack.QueueBind(QRes.QueueName, Ex_CashBack, string.Empty);

        var consumer = new EventingBasicConsumer(Ch_CBack);
        consumer.Received += (s, e) =>
        {
            var bdy = e.Body.ToArray();
            var dt = e.DeliveryTag;
            var msg = Encoding.UTF8.GetString(bdy);
            if (msg == null) return;
            var data = JsonSerializer.Deserialize<CBCycle>(msg);
            if (data != null)
            {
                if (data.End.When <= DateTime.UtcNow)//Cycle Ended
                {
                    if (data.IsCompleted)
                        CheckAndProcessCashBack(data);
                }
                else if (CurrentCBCycle == null || (CurrentCBCycle.Start.When <= data.Start.When) && (data.Start.When <= DateTime.UtcNow))
                {
                    CurrentCBCycle = data;
                }
            }
        };
        Ch_CBack.BasicConsume(QRes.QueueName, autoAck: true, consumer: consumer);
        LogEvent($"{RegistryToken!.AppId} CashBack Listner Setup for MAPI CashbackPool");
    }
    private void CheckAndProcessCashBack(CBCycle cyc)
    {
        try
        {
            Console2.WriteLine_White($"Referral And Cashback Calculation begins for the period of {cyc.Start.When} to {cyc.End.When} at..:{DateTime.UtcNow}");
            var wm = GetWalletManager();
            var p=SrvCoinWatch.GetCoin("navC").Price;

            wm.DEBITCashBackForThisCycle(cyc.Start.When, cyc.End.When, p);
           
            new Thread(()=>CheckAndProcessReferralReward(p)).Start();

        }
        catch (Exception ex)
        {
            Console2.WriteLine_RED($"ERROR:Cashback Calculation and Deposit Process has STOPPED due to following Error..at:{DateTime.UtcNow}");
            Console2.WriteLine_RED($"ERROR:CheckAndProcessCashBack at:{ex.GetDeepMsg()}..at:{DateTime.UtcNow}");
        }
    }
    private void CheckAndProcessReferralReward(double NavCPrice)
    {
        try
        {
            //It will only happen if Month has ended and calculation of previous month is pending
         if (IsRefCalculationBegin) return;
            var wm = GetWalletManager();
            wm.CalculateRewardForThisMonth();
            wm.DEBITRewardForThisMonth(NavCPrice);
        }
        catch (Exception ex)
        {
            Console2.WriteLine_RED($"ERROR:Referral Reward Calculation and Deposit Process has STOPPED due to following Error..at:{DateTime.UtcNow}");
            Console2.WriteLine_RED($"ERROR:CheckAndProcessReferralReward at:{ex.GetDeepMsg()}..at:{DateTime.UtcNow}");
        }
    }
    private WalletManager GetWalletManager()
    {
        //ToDo: Secure this Manager, Transaction Count Applied, Active Session

        var result = new WalletManager();
        result.dbctx = _ApiAppContext();

        return result;
    }
    private ApiAppContext _ApiAppContext()
    {
        var o = new DbContextOptionsBuilder<ApiAppContext>();
        o = o.UseSqlServer(ConfigEx.Config.GetConnectionString("ApiDBContext"));
        return new ApiAppContext(o.Options);
    }
}

internal class NavCStatices
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    /// <summary>
    /// Total Trade in this Block, leading to NavC Price encapsulation
    /// </summary>
    public double TotalTrade { get; set; }
    /// <summary>
    /// Total Cash back Paid till now
    /// </summary>
    public double TotalCashBack { get; set; }
    /// <summary>
    /// NavC Token Price as of Now
    /// </summary>
    public double NavCPrice { get; set; }
    /// <summary>
    /// NavC Tokens Currently in Pool
    /// </summary>
    public double NavCPoolCount { get; set; }
    public CBCycle CurrentCBCycle { get; set; }
    public string stream { get; set; }

}
public class CBCycle
{
    public Guid CycleID { get; set; }
    public CycleEvent Start { get; set; } = new CycleEvent();
    public CycleEvent End { get; set; } = new CycleEvent();
    public bool IsCompleted { get; set; }
    public int EventNo { get; set; }
    /// <summary>
    /// This Wallet Id would be USed in Transaction to deposit NavC cashBack Tokens in SpotWallet
    /// </summary>
    public Guid WalletID { get; set; }
    public string CompareTo { get => $"{Start.When}{End.When}{Start.PoolTokensCount}"; }
}

public class CycleEvent
{
    public DateTime When { get; set; }
    public long UnixWhen { get; set; }
    public double PoolTokensCount { get; set; }
    public double CashBackTillNow { get; set; }
    public double TokenPrice { get; set; }
    public double PoolValue { get { return PoolTokensCount * TokenPrice; } }
}