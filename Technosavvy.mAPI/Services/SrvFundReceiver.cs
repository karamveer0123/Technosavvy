using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace NavExM.Int.Maintenance.APIs.Services;
/* Worker Client for Incoming Funds Notification
 */
internal class SrvFundReceiver : AppConfigBase
{
    public string Ex_FundIncomingNotification { get => $"NavExM.Wallet.EthMainNet.Funds.IncomeingOffice"; }
    public static string Ex_EthWalletToBankOffice { get => $"NavExM.Wallet.EthMainNet.ToBank.RequestOffice"; }
    internal SmtpConfig smtp;

    IModel ch_InFunds;
    
    public SrvFundReceiver(SmtpConfig _smtp)
    {
        smtp = _smtp;
    }
    protected override async Task DoStart()
    {
        // await Task.Delay(2000);
        if (RegistryToken is null) return;
        SetUpIncomingFundsListner();
        await Task.CompletedTask;
    }
    void SetUpIncomingFundsListner()
    {
        var isClose = (ch_InFunds == null || ch_InFunds.IsClosed);
        if (!isClose) return;

        ch_InFunds = Ctn_CreateModel(ch_InFunds);
        ch_ToBankReqPub = Ctn_CreateModel(ch_ToBankReqPub);

        ch_InFunds.ExchangeDeclare(Ex_FundIncomingNotification, ExchangeType.Direct);

        var QRes = ch_InFunds.QueueDeclare(queue: $"{Ex_FundIncomingNotification}_Qu", durable: true, exclusive: false, autoDelete: false, arguments: null);

        ch_InFunds.QueueBind(QRes.QueueName, Ex_FundIncomingNotification, string.Empty);

        var consumer = new EventingBasicConsumer(ch_InFunds);
        consumer.Received += (s, e) =>
        {
            Console2.WriteLine_DarkYellow($"{RegistryToken!.AppId}|{T}:Incoming Funds Confirmation");
            var bdy = e.Body.ToArray();
            var dt = e.DeliveryTag;
            var msg = Encoding.UTF8.GetString(bdy);
            var data = JsonSerializer.Deserialize<smFundsNotification>(msg);
            if (data != null && data.WalletAddress.IsNOT_NullorEmpty() && data.userAccount.IsNOT_NullorEmpty())
            {
                Console2.WriteLine_DarkYellow($"{T}: Pre Funds Credited Validation begins at..{DateTime.UtcNow}");
                //validate and Save Funds Transaction
                ValidateAndCreateTransaction(data);
                Console2.WriteLine_DarkYellow($"{T}: Funds Credited at..{DateTime.UtcNow}");
            }
        };
        ch_InFunds.BasicConsume(QRes.QueueName, autoAck: true, consumer: consumer);
        Console2.WriteLine_DarkYellow($"{RegistryToken!.AppId}: Incoming Fund Transaction Service Set up is done at..{DateTime.UtcNow}");
    }
    bool ValidateAndCreateTransaction(smFundsNotification Addr)
    {
        var wm = new WalletManager();
        wm.dbctx = dbctx();
        wm.pbdbctx = pb_dbctx();
        wm.smtp=smtp;
        return wm.ValidateAndReceiveExternalFunds(Addr);
    }
    static IModel ch_ToBankReqPub;
   internal static void PublishFundsToBankRequest(smErc20ToBank data)
    {

       // ch_ToBankReqPub = Ctn_CreateModel(ch_ToBankReqPub);
        ch_ToBankReqPub.ExchangeDeclare(Ex_EthWalletToBankOffice, ExchangeType.Direct);
        var str = JsonSerializer.Serialize(data);
        var bdy = Encoding.UTF8.GetBytes(str);

        ch_ToBankReqPub.BasicPublish(exchange: Ex_EthWalletToBankOffice, routingKey: "",
            basicProperties: null, body: bdy);

        Console2.WriteLine_DarkYellow($"Bank Deposit Request Published for :{data.WalletAddress}  Published at..{DateTime.UtcNow}");
    }
    private ApiAppContext dbctx()
    {
        var o = new DbContextOptionsBuilder<ApiAppContext>();
        o = o.UseSqlServer(ConfigExtention.Configuration.GetConnectionString("ApiDBContext"));
        return new ApiAppContext(o.Options);
    }
    private PreBetaDBContext pb_dbctx()
    {
        var o = new DbContextOptionsBuilder<PreBetaDBContext>();
        o = o.UseSqlServer(ConfigExtention.Configuration.GetConnectionString("PreBetaDBContext"));
        return new PreBetaDBContext(o.Options);
    }
}
public class smFundsNotification
{
    /// <summary>
    /// Public Wallet that Received this Fund
    /// </summary>
    public string WalletAddress { get; set; }
    /// <summary>
    /// User Account Number
    /// </summary>
    public string userAccount { get; set; }
    /// <summary>
    /// Profile Id of the Address Owner
    /// </summary>
    public Guid userAccountId { get; set; }
    /// <summary>
    /// Network proxy URL, that Resulted in this Notification
    /// </summary>
    public string NetworkProxy { get; set; }
    /// <summary>
    /// Network Smart Contarct Address
    /// </summary>
    public string TokenAddress { get; set; }
    /// <summary>
    /// Network Native Amount
    /// </summary>
    public double Amount { get; set; }
    /// <summary>
    /// If this is Smart Contract Amount, 
    /// </summary>
    public string Erc20Amount { get; set; }
    /// <summary>
    /// Flag for network Native Crypto
    /// </summary>
    public bool IsNativeFund { get; set; }
    /// <summary>
    /// Network Transaction Receipt
    /// </summary>
    public string TranHash { get; set; }

}
internal class smErc20ToBank
{
    public string WalletAddress { get; set; }
    public string TokenAddress { get; set; }
    public bool IsNative { get; set; }
}