using Microsoft.AspNetCore.Identity;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using NavExM.Int.Maintenance.APIs.Static;
using NavExM.Int.Maintenance.APIs.Model.AppInt;

namespace NavExM.Int.Maintenance.APIs.Services;

internal abstract class AppConfigBase
{
    static int PingAfter = 10;//Sec
    static int HealthAfter = 60;//Sec
    static DateTime lastPing;
    static DateTime lastHealth;
    static object dtLock = new object();

    protected internal static RegCompResponse? RegistryToken;
    protected internal HttpClient _LogAPIChannel;
    protected internal HttpClient _WalletWatcher;
    protected internal int pulse = 0;
    //Single Instance per Process
    static ConnectionFactory? conFactory;
    static object _lock = new object();
    //Single Instance per Process
    protected static IConnection? Ctn;
    //Multi Channel as Multi Instances
    static IModel? chAppIntegReq;
    static IModel? chAppLog;
    static IModel? chAppIntegRes;
    static bool IsRegReqSent = false;
    protected CancellationToken myCancellationToken;

    public async Task DoBase(CancellationToken token)
    {
        myCancellationToken = token;
        while (!token.IsCancellationRequested)
        {
            try
            {
                DoAppIntegration();
                if (pulse > 0) await Task.Delay(pulse);
                if (pulse <= 0) pulse = 5000;
                await DoStart();
            }
            catch (TaskCanceledException ex)
            {
                //ToDo:Naveen, Notify WatchDog to Switch to Secondery option
                LogError(ex);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }
    }
    protected async virtual Task DoStart()
    {
        //Notify WatchDog and Close, if not overridden in Drived class

        /* This Class Should hold all the task that it is suppose to do 
         * This Class will remain in context for the duration of the App Life, So as good as Static
         * Read RabbitMQ
         * Update Local Collection
         * Get & Update External Network Address
         * Check and Update Staking renewal
         * Check and Update Staking refunds
         */
        await Task.CompletedTask;
    }

    protected bool IsOKForChannel
    {
        get
        {
            try
            {
                return Ctn is not null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetDeepMsg);
            }
            return false;
        }
    }

    private string DeCodeSignedHash(string str)
    {
        //ToDo: Naveen, Complete Private Key/Public Key implementation
        return str;
    }
    protected internal string GetSignedHash(string str)
    {
        //ToDo: Naveen, SignedHash should be implemented with AppSeed Private Key of Application Instances, Public Key would be communicated to Watcher for Hash Verification
        var a = new PasswordHasher<object>(optionsAccessor: default);
        var res = a.HashPassword(Guid.Empty, str);
        return res;
    }
    protected internal string GetHash(string str)
    {
        var a = new PasswordHasher<object>(optionsAccessor: default);
        var res = a.HashPassword(Guid.Empty, str);
        return res;
    }
    protected internal PasswordVerificationResult CompareHash(string Hpwd, string pwd)
    {
        var a = new PasswordHasher<object>(optionsAccessor: default);
        var res = a.VerifyHashedPassword(Guid.Empty, Hpwd, pwd);
        return res;
    }
    protected internal HttpClient GetLogAPIChannel()
    {
        if (_LogAPIChannel == null)
        {
            _LogAPIChannel = new HttpClient();

            var ch = _LogAPIChannel;//short Name
                                    //ToDo: Naveen, Application Instance Identity should be implemented here
            ch.BaseAddress = new Uri(mAppConstant.WatDogAPI);
            ch.DefaultRequestHeaders.Accept.Clear();
            ch.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        return _LogAPIChannel;
    }
    protected internal HttpClient GetWalletWatchChannel()
    {
        if (_WalletWatcher == null)
        {
            _WalletWatcher = new HttpClient();

            var ch = _WalletWatcher;//short Name
                                    //ToDo: Naveen, Application Instance Identity should be implemented here
            ch.BaseAddress = new Uri(mAppConstant.WalletWatch);
            ch.DefaultRequestHeaders.Accept.Clear();
            ch.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ch.DefaultRequestHeaders.Add("callerid", Signer.PublicKey);
        }
        return _WalletWatcher;
    }
    #region Log Messages
    internal void LogEvent(string msg)
    {
        mLogT vL = mLogT.GetNewLog();
        vL.Message = msg;
        vL.Type = eLogType.Event;

        if (RegistryToken != null)
            vL.AppId = RegistryToken.AppId;
        if (!PublishToEvent(vL))
            Console.WriteLine(msg);

    }
    internal void LogHealthReport()
    {
        var R = RegistryToken!;
        var hr = HealthDetails.GetMyHealthDetails(R.Key, R.AppId, Helper.GetLocalIPs().FirstOrDefault()!, Helper.GetMacAddress().FirstOrDefault()!);
        var msg = JsonSerializer.Serialize(hr);
        mLogT vL = new mLogT() { Message = msg, Type = eLogType.Health };
        if (RegistryToken != null)
            vL.AppId = RegistryToken.AppId;
        if (!PublishToEvent(vL))
            Console.WriteLine(msg);
    }
    internal void LogError(string msg)
    {
        mLogT vL = mLogT.GetNewLog();
        vL.Message = msg;
        vL.Type = eLogType.Error;

        if (RegistryToken != null)
            vL.AppId = RegistryToken.AppId;
        if (!PublishToError(vL))
            Console.WriteLine(msg);
    }
    internal void LogDebug(string msg)
    {
        mLogT vL = mLogT.GetNewLog();
        vL.Message = msg;
        if (RegistryToken != null)
            vL.AppId = RegistryToken.AppId;
        var v = ConfigEx.Config.GetSection("DebugLog");
        if (v.Value != null && v.Value.ToLower() == "true")
            if (!PublishToError(vL))
                Console.WriteLine(msg);
    }

    internal void LogError(Exception ex)
    {
        mLogT vL = mLogT.GetNewLog();
        vL.Message = GetMsg(ex);
        vL.Message += ex.StackTrace;

        if (RegistryToken != null)
            vL.AppId = RegistryToken.AppId;
        if (!PublishToError(vL))
            Console.WriteLine(vL.Message);


    }
    private string GetMsg(Exception ex)
    {
        if (ex is null) return string.Empty;
        var msg = $"{ex.Message}{GetMsg(ex.InnerException)}";
        return msg;
    }
    #endregion

    #region Rabbit Message Queue

    private void DoAppIntegration()
    {
        if (ConfigBase.IsDynamicSet) { PingWatchDog(); return; }
        //ensure connection
        CreateConnection();
        SendIntegrationRequest();
    }
    private void SendIntegrationRequest()
    {
        if (chAppIntegReq is null)
        {
            chAppIntegReq = Get_chAppIntegReq();
            chAppIntegRes = Get_chAppIntegRes();
            lock (chAppIntegReq)
            {
                if (chAppIntegReq.IsClosed)
                {
                    LogError($"Failed to Open Requesting Queue to WatchDog for Integration Channel by {Environment.ProcessId}");
                }
                if (chAppIntegRes.IsClosed)
                {
                    LogError($"Failed to Open Response Queue to WatchDog for Integration Channel by {Environment.ProcessId}");
                }
            }
        }
        if (RegistryToken is null)
        {
            lock (chAppIntegReq)
            {
                // if (IsRegReqSent) return; //we will Request only 1 time per Life cycle
                //Now Send Registration Request
                PublishToRegistry(GetPackage(RegMsgType.NewInstance));
                LogDebug($"{ConfigBase.handShakePackage.InstanceName} -{ConfigBase.handShakePackage.ProcessId} Sent Registration Request");
            }
        }
    }
    static object chAppIntegReq_LOCK = new object();
    static object chAppIntegRes_LOCK = new object();
    static object chAppLog_LOCK = new object();
    private static IModel Get_chAppLog()
    {
        lock (chAppLog_LOCK)
        {

            if (chAppLog is null || chAppLog.IsClosed)
            {
                chAppLog = Ctn.CreateModel();
                chAppLog.ExchangeDeclare("NavExM_LogHUB", ExchangeType.Fanout);
            }
            return chAppLog;
        }
    }
    private static IModel Get_chAppIntegReq()
    {
        lock (chAppIntegReq_LOCK)
        {

            if (chAppIntegReq is null || chAppIntegReq.IsClosed)
            {
                chAppIntegReq = Ctn.CreateModel();
                chAppIntegReq.ExchangeDeclare(ConfigBase.Ex_NameRegReq, ExchangeType.Fanout);
            }
            return chAppIntegReq;
        }
    }
    private IModel Get_chAppIntegRes()
    {
        lock (chAppIntegRes_LOCK)
        {
            chAppIntegRes = Ctn.CreateModel();
            chAppIntegRes.ExchangeDeclare(ConfigBase.Ex_NameRegRes, ExchangeType.Fanout);
            var QRes = chAppIntegRes.QueueDeclare(queue: "", durable: true, exclusive: true, autoDelete: false, arguments: null);

            chAppIntegRes.QueueBind(QRes.QueueName, ConfigBase.Ex_NameRegRes, string.Empty);

            var consumer = new EventingBasicConsumer(chAppIntegRes);
            consumer.Received += Registration_Response;
            chAppIntegRes.BasicConsume(QRes.QueueName, autoAck: false, consumer: consumer);
        }
        return chAppIntegRes;
    }

    private string GetPackage(RegMsgType type)
    {
        var pkg = new MsgWrapper();
        switch (type)
        {
            case RegMsgType.NewInstance:
                ConfigBase.LoadDefault();
                var hPackage = ConfigBase.handShakePackage;
                pkg.Key = hPackage.InstanceKey;
                pkg.MsgType = type;// RegMsgType.NewInstance;
                pkg.Package = hPackage;
                break;
            case RegMsgType.ConfirmHashing:
                pkg.PayLoad = GetHash(RegistryToken!.AppSeed);
                pkg.Key = ConfigBase.handShakePackage.InstanceKey;
                pkg.MsgType = type;
                break;
            case RegMsgType.Ping:
                pkg.Key = ConfigBase.handShakePackage.InstanceKey;
                pkg.MsgType = type;
                break;
        }
        return JsonSerializer.Serialize(pkg);
    }
    private static bool PublishToError(mLogT log)
    {
        var ex = "NavExM_LogHUB";
        chAppLog = Get_chAppLog();
        var str = JsonSerializer.Serialize(log);
        var bdy = Encoding.UTF8.GetBytes(str);
        chAppLog.BasicPublish(exchange: ex, routingKey: "",
            basicProperties: null, body: bdy);
        return false;
    }
    private static bool PublishToEvent(mLogT log)
    {
        var ex = "NavExM_LogHUB";
        chAppLog = Get_chAppLog();
        var str = JsonSerializer.Serialize(log);
        var bdy = Encoding.UTF8.GetBytes(str);
        chAppLog.BasicPublish(exchange: ex, routingKey: "",
            basicProperties: null, body: bdy);
        return false;
    }
    private static void PublishToRegistry(string msg)
    {
        var QNameReq = ConfigBase.Ex_NameRegReq;

        var bdy = Encoding.UTF8.GetBytes(msg);
        // chAppIntegReq.ExchangeDeclare(QNameReq, ExchangeType.Fanout);
        chAppIntegReq = Get_chAppIntegReq();
        chAppIntegReq.BasicPublish(exchange: ConfigBase.Ex_NameRegReq, routingKey: "", basicProperties: null, body: bdy);
        IsRegReqSent = true;
    }
    private void Registration_Response(object? sender, BasicDeliverEventArgs e)
    {
        var bdy = e.Body.ToArray();
        var dt = e.DeliveryTag;
        var msg = Encoding.UTF8.GetString(bdy);
        try
        {
            var data = JsonSerializer.Deserialize<RegCompResponse>(msg);
            VerifyAndActionPackage(data);
            chAppIntegReq.BasicAck(deliveryTag: dt, false);
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
        LogEvent($"{ConfigBase.QNameRegResponse} Received..at {DateTime.UtcNow}");
        ConfigBase.LoadDynamic();
    }
    private void VerifyAndActionPackage(RegCompResponse pkg)
    {
        if (pkg.Key != ConfigBase.handShakePackage.InstanceKey) return;
        //Decode AppSeed
        var str = DeCodeSignedHash(pkg.AppSeed);

        str.CheckAndThrowNullArgumentException();
        var token = new RegCompResponse();
        token.WatcherPrivate = str.Split('|')[0].ToGUID();
        token.AppSeed = str.Split('|')[1];
        token.Key = str.Split('|')[2].ToGUID();
        token.AppId = pkg.AppId;
        token.ProcessId = pkg.ProcessId;
        RegistryToken = token;
        PublishToRegistry(GetPackage(RegMsgType.ConfirmHashing));
        //
        // Hash AppSeed and send back to Registry
        // secure AppSeed and Flag Integration Complete
    }


    protected void PingWatchDog()
    {
        if (((DateTime.UtcNow.Ticks - lastPing.Ticks) / TimeSpan.TicksPerSecond) > PingAfter)
        {
            lock (dtLock)
            {
                PublishToRegistry(GetPackage(RegMsgType.Ping));
                lastPing = DateTime.UtcNow;
                LogEvent($"Ping Sent to WatchDog by {ConfigBase.handShakePackage.InstanceName} -{ConfigBase.handShakePackage.ProcessId}");
                if (((DateTime.UtcNow.Ticks - lastHealth.Ticks) / TimeSpan.TicksPerSecond) > HealthAfter)
                {
                    LogHealthReport();
                    lastHealth = DateTime.UtcNow;
                }
            }
        }

    }
    #region Connection Operation
    public string ConName { get; protected set; }
    protected IConnection? CtnOp;

    object Ctn_CreateLOCK = new object();
    List<IModel> Ch_lst = new List<IModel>();
    protected IModel Ctn_CreateModel(IModel ch)
    {
        IModel retval;
        lock (Ctn_CreateLOCK)
        {
            if (ch != null && ch.IsOpen) return ch;
            var lastCtn = CtnOp;
            if (CtnOp is null)
            {
                LogEvent($"Opening Operation Connection..");
                CreateConnectionOperation();
            }

            retval = CtnOp.CreateModel();
            Ch_lst.Add(retval);
            //  LogDebug($"{ConName}:Channel Id : {retval.ToString()}");
            return retval;// Ctn.CreateModel();
        }
    }
    protected void CloseOpAll()
    {
        if (CtnOp != null && CtnOp.IsOpen)
            CtnOp.Abort();
        Ch_lst.ForEach(x =>
        {
            if (x != null && x.IsOpen)
                x.Abort();
        });

    }
    object Reclaim_LOCK = new object();
    private bool CreateConnectionOperation()
    {
        try
        {
            lock (Reclaim_LOCK)
            {

                if (CtnOp is null || !CtnOp.IsOpen)
                {
                    if (conFactory is null)
                    {
                        CreateConnection();
                    }
                    CtnOp = conFactory!.CreateConnection($"NavExM.Int.Maintenance.APIs.{ConName}");
                }
                return CtnOp.IsOpen;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.GetDeepMsg());
        }
        return false;
    }
    #endregion
    private static void CreateConnection()
    {
        lock (_lock)
        {
            try
            {
                if (conFactory is null)
                {
                    if (!ConfigBase.IsSet) ConfigBase.LoadDefault();
                    conFactory = new ConnectionFactory()
                    {
                        Password = ConfigBase.QPassword,
                        UserName = ConfigBase.QUserName,
                        Uri = new Uri(ConfigBase.QURL),
                        Port = ConfigBase.QPort,
                        VirtualHost = "/"
                    };
                    conFactory.RequestedHeartbeat = new TimeSpan(0, 0, 1800);
                    Ctn = conFactory.CreateConnection($"NavExM.Int.Maintenance.APIs");
                    Console.WriteLine($"Connection Created Successfully.. at {DateTime.UtcNow}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetDeepMsg());
            }
        }

}
    internal string T { get => $"T:{Thread.CurrentThread.ManagedThreadId}"; }

    #endregion

}

public static class Console2
{
    public static void WriteLine_RED(string line)
    {
        var fc1 = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(line);
        Console.ForegroundColor = fc1;
    }
    public static void WriteLine_White(string line)
    {
        var fc1 = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(line);
        Console.ForegroundColor = fc1;
    }
    public static void WriteLine_DarkYellow(string line)
    {
        var fc = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(line);
        Console.ForegroundColor = fc;
    }
    public static void WriteLine_Green(string line)
    {
        var fc2 = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(line);
        Console.ForegroundColor = fc2;
    }
   
}
