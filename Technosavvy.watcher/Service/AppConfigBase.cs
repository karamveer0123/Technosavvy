using Microsoft.AspNetCore.Identity;
using NavExM.Int.Watcher.WatchDog.Core;
using NavExM.Int.Watcher.WatchDog.Model.AppInt;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace NavExM.Int.Watcher.WatchDog.Service
{
    internal abstract class AppConfigBase : LoggerBase
    {
        #region Instance Variables
        double PingPassPercentage = 0.95;
        long summeryReportSpan = 120;//seconds
        long CheckPingEvery = 30;//seconds
        DateTime PingLastCheckedOn = DateTime.MinValue;
        IModel ch;
        object OThreadOAction = new object();
        protected internal int pulse = 0;
        internal protected string RegName { get; protected set; }
        internal protected string AppName { get; protected set; }
        internal protected string Ex_NameRegReq { get; protected set; }

        internal protected string Ex_NameRegRes { get; protected set; }
        internal protected int ComponentGroupId = 00;//WatchDog is 00
        internal protected bool IsRegOpen = false;
        //Permanently enable the Auto Registration in the Registry
        internal protected bool IsAutoRegEnabled= false;
        #endregion

        #region static variables
        static IModel? chAppLog;
        protected static IConnection? Ctn;
        //Single Instance per Process
        static ConnectionFactory? conFactory;
        //Single Instance per Process
        protected static IConnection? CtnReceiver;

        internal static RegConfig? WDCofig;
        static IModel chLog;
        static bool ShouldLogging;
        static IModel chVote;
        static IModel chEventReceiver;
        static IModel chEventPub;
        static IModel chLogSummaryPub;
        static IModel chLogDBPub;
        static int VotingGap = 2000;//2 Sec
        static ConcurrentDictionary<string, List<HealthDetails>> _HealthData = new ConcurrentDictionary<string, List<HealthDetails>>();
        static ConcurrentDictionary<string, List<string>> LogQueus = new ConcurrentDictionary<string, List<string>>();
        static ConcurrentDictionary<string, RegConfig> RegConfigs = new ConcurrentDictionary<string, RegConfig>();
        static ConcurrentDictionary<string, List<mHandShakePackage>> _CompCandidates = new ConcurrentDictionary<string, List<mHandShakePackage>>();
        static ConcurrentDictionary<string, List<RegRecord>> _Components = new ConcurrentDictionary<string, List<RegRecord>>();
        static ConcurrentDictionary<string, List<mHandShakePackage>> _ApprovedCompCandidates = new ConcurrentDictionary<string, List<mHandShakePackage>>();
        static ConcurrentDictionary<string, List<mHandShakePackage>> _RejectedCompCandidates = new ConcurrentDictionary<string, List<mHandShakePackage>>();
        internal static LoggerBase instance = null;
        static object _lock = new object();


        //Multi Channel as Multi Instances
        static IModel? chWatchDogHub;
        #endregion

        public async Task DoBase(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    instance = this;
                    await DoWatchDogJob();
                    if (pulse > 0) await Task.Delay(pulse);

                    if (pulse <= 0) pulse = 5000;
                    await DoStart();
                }
                catch (TaskCanceledException)
                {
                    //Notify WatchDog to Switch to Secondery option
                }
            }
        }
        private void CloseLoggingProcess()
        {
            ShouldLogging = false;
            if (chLog != null && chLog.IsOpen)
            {
                chLog.Abort();
            }
        }
        private async Task DoWatchDogJob()
        {
            OpenRegistryChannells();
            SeekAndExecuteApprovedComponent();

            if (WDCofig.IsPrimary && !IsRegOpen)
            {
                //Function for Primary
                if (!WDCofig.IsDBLog)
                    CloseLoggingProcess();

                EnsureRegistrationIsOpenAndListening();
                new Thread(CalculatePingAttendence).Start();

            }
            else if (WDCofig.IsPrimary && !IsOKForChannel)
            {
                var m = $"{RegName} Registry is waiting underlying WatchDog service to complete Handshake negotiation.{Ex_NameRegReq} is Pending";
                LogEvent(m);
            }
            //if (IsOKForChannel && VotingManager.ResolvedRound > 0)


            if (WDCofig.IsDBLog)
            {
                //Function for DBLog
                WatchDogEventReceiverUpdate();
                SetUpLoggingExAndQueue();
                CalculateLogs();
            }
            if (WDCofig.IsReport)
            {
                //Function for Report
                WatchDogEventReceiverUpdate();
                SetUpLoggingExAndQueue();
                CalculateLogs();
            }


            await Task.CompletedTask;
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
        protected string RootAppSeed
        {
            get
            {
                return "ABC from Registry";//This will be Directly entered by Admin
            }
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
                    LogError(ex);
                }
                return false;
            }
        }

        protected internal string GetSignedHash(byte[] pubKey, string MsgToSign)
        {
            //ToDo: Naveen, Security Implement Asymmetric Public Key Descrption here..
            return MsgToSign;
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

        #region Static Component Registration
        protected void RegisterLogQueue(string qName)
        {
            lock (LogQueus)
            {
                var lst = LogQueus[RegName];
                if (!lst.Any(x => x == qName))
                    lst.Add(qName);
            }
        }

        internal static List<string> GetComponentRegistries()
        {
            return RegConfigs.Keys.ToList();
        }
        internal static List<mHandShakePackage> GetRegistrationSeekers(string RegistryName)
        {
            lock (_CompCandidates)
            {
                return _CompCandidates[RegistryName].ToList();
            }
        }
        internal static Tuple<bool, string> RejectRegistration(string RegistryName, Guid candidateId)
        {
            lock (_CompCandidates)
            {

                var lst = _CompCandidates[RegistryName].ToList();
                var a = lst.FirstOrDefault(x => x.InstanceKey == candidateId);
                if (a != null)
                {
                    lst.Remove(a);
                    _CompCandidates[RegistryName] = lst;

                    return new Tuple<bool, string>(true, "Component Registration Rejected..");
                }
                return new Tuple<bool, string>(false, "No Such Component to Reject..");

            }
        }
        internal static Tuple<bool, string> AcceptRegistration(string RegistryName, Guid InstanceId)
        {
            lock (_CompCandidates)
            {

                var lst = _CompCandidates[RegistryName].ToList();
                var a = lst.FirstOrDefault(x => x.InstanceKey == InstanceId);
                if (a != null)
                {
                    lst.Remove(a);
                    _CompCandidates[RegistryName] = lst;
                    if (!_ApprovedCompCandidates[RegistryName].Any(s => s.InstanceKey == a.InstanceKey))
                        _ApprovedCompCandidates[RegistryName].Add(a);

                    return new Tuple<bool, string>(true, "Component Registration Accepted. Pending Process..");
                }
                return new Tuple<bool, string>(false, "No Such Component to Accept..");

            }
        }
        internal static RegConfig GetRegistryConfig(string RegistryName)
        {
            lock (RegConfigs)
            {

                if (RegConfigs.Keys.Contains(RegistryName))
                    return RegConfigs[RegistryName];
                else
                    return null;
            }
        }
        internal static List<string> GetAllRegistryConfigKey()
        {
            lock (RegConfigs)
            {
                return RegConfigs.Keys.ToList();
            }
        }
        static RegConfig CreateOrUpdateRegistryConfig(string registryName, RegConfig regConfig)
        {
            lock (RegConfigs)
            {
                if (RegConfigs.Keys.Contains(registryName))
                    RegConfigs[registryName] = regConfig;
                else
                    RegConfigs.TryAdd(registryName, regConfig);
            }
            return RegConfigs[registryName];
        }
        private void SetupMyRegistryEntry()
        {
            var name = RegName;
            var config = GetRegistryConfig(name);
            if (config is null)
            {
                var c = new RegConfig();
                ConfigBase.LoadDefault();
                c.Instance = ConfigBase.handShakePackage;
                CreateOrUpdateRegistryConfig(name, c);
                SetUpCandidatesCollections();
            }
        }
        protected RegConfig myConfig { get { return RegConfigs[this.RegName]; } }
        protected void SetUpCandidatesCollections()
        {
            _CompCandidates.TryAdd(this.RegName, new List<mHandShakePackage>());
            _Components.TryAdd(this.RegName, new List<RegRecord>());
            _ApprovedCompCandidates.TryAdd(this.RegName, new List<mHandShakePackage>());
            _RejectedCompCandidates.TryAdd(this.RegName, new List<mHandShakePackage>());


        }
        protected List<mHandShakePackage> CompCandidates
        {
            get
            {
                _CompCandidates.TryAdd(this.RegName, new List<mHandShakePackage>());
                return _CompCandidates[this.RegName];
            }
        }
        protected List<RegRecord> Components
        {
            get
            {
                _Components.TryAdd(this.RegName, new List<RegRecord>());
                return _Components[this.RegName];
            }
        }
        protected List<mHandShakePackage> ApprovedComponents
        {
            get
            {
                _ApprovedCompCandidates.TryAdd(this.RegName, new List<mHandShakePackage>());
                return _ApprovedCompCandidates[this.RegName];
            }
        }
        protected bool ClearFromApprovedCompCandidate(mHandShakePackage a)
        {
            try
            {
                var lst = _ApprovedCompCandidates[this.RegName];
                var f = lst.FirstOrDefault(z => z.InstanceKey == a.InstanceKey);
                if (f == null) return false;
                lst.Remove(f);
                _ApprovedCompCandidates[this.RegName] = lst;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return true;
        }

        #endregion

        #region Protected Component Registration
        internal void EnsureRegistrationIsOpenAndListening()
        {
            lock (OThreadOAction)
            {

                if (ch is null)
                {
                    ch = Ctn.CreateModel();
                    if (ch.IsOpen)
                    {
                        ch.ExchangeDeclare(Ex_NameRegReq, ExchangeType.Fanout);

                        var Q = ch.QueueDeclare(queue: "", durable: true, exclusive: true, autoDelete: false, arguments: null);
                        if (Q != null)
                        {
                            ch.QueueBind(Q.QueueName, Ex_NameRegReq, string.Empty);

                            var consumer = new EventingBasicConsumer(ch);
                            consumer.Received += RegistrationRequest_Notification;

                            ch.BasicConsume(Q.QueueName, autoAck: false, consumer: consumer);
                            IsRegOpen = true;
                            var m = $"WatchDog Registration for Ex:{Ex_NameRegReq} is OPEN with Queue Binding {Q.QueueName}";
                            LogEvent(m);
                        }
                    }
                }
            }
        }
        private void RegistrationRequest_Notification(object? sender, BasicDeliverEventArgs e)
        {
            try
            {
                var bdy = e.Body.ToArray();
                var msg = Encoding.UTF8.GetString(bdy);
                var hPackage = JsonSerializer.Deserialize<MsgWrapper>(msg);
                LogDebug($"{101} Received Registration message Notification");
                ProcessMessage(hPackage);
                ch.BasicAck(e.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetDeepMsg());
                LogError(ex);
            }
        }
        protected void ProcessMessage(MsgWrapper msg)
        {
            msg.CheckAndThrowNullArgumentException();
            switch (msg.MsgType)
            {
                case RegMsgType.NewInstance:
                    ProcessRegistration(msg.Package);
                    break;
                case RegMsgType.ConfirmHashing:
                    var isOk = CompareHash(msg.PayLoad, RootAppSeed) == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
                    if (isOk)
                    {
                        var a = Components.FirstOrDefault(x => x.handShakePackage.InstanceKey == msg.Key);
                        a.Security.isCompleted = true;
                        a.Security.CompletedOn = DateTime.UtcNow;
                        LogEvent($"Proc:{a.Response.ProcessId} Inst:{a.Response.AppId} Confirmed integration.");
                        //SetUpLogsQueue(a.Response.AppId);
                    }
                    break;
                case RegMsgType.Ping:
                    var a1 = Components.FirstOrDefault(x => x.handShakePackage.InstanceKey == msg.Key);
                    if (a1 != null)
                    {
                        a1.Security.Ping();
                        LogEvent($"Ping Received from {a1.handShakePackage.InstanceName}-{a1.handShakePackage.ProcessId} at {DateTime.UtcNow}");
                    }
                    else
                        throw new Exception($"Ping Rejected by unregisterted component {msg.Key}");
                    break;
                default:
                    break;// Do nothing for unwanted message
            }
        }

        protected void SeekAndExecuteApprovedComponent()
        {
            lock (ApprovedComponents)
            {
                foreach (var h in ApprovedComponents)
                {
                    var isThere = Components.Any(x => x.handShakePackage.InstanceKey == h.InstanceKey);
                    if (isThere)
                        ClearFromApprovedCompCandidate(h);//Already Register
                    else
                    {
                        try
                        {
                            var result = RegisterComponent(h);

                        }
                        catch (Exception ex)
                        {
                            LogError(ex);
                            LogEvent($"Component Registration caused error. Removed from the Queue");
                        }
                        finally
                        {
                            ClearFromApprovedCompCandidate(h);
                        }
                    }
                }

            }
        }
        private bool NotExistingComponent(mHandShakePackage msg)
        {
            var isExisting = Components.Any(x => x.handShakePackage.InstanceKey == msg.InstanceKey) || CompCandidates.Any(x => x.InstanceKey == msg.InstanceKey);
            return isExisting;
        }
        private void ProcessRegistration(mHandShakePackage msg)
        {
            LogEvent($"Process Registration Called by {this.RegName}");
            msg.CheckAndThrowNullArgumentException();
            var isExisting = NotExistingComponent(msg);
            if (isExisting)
            {
                //Duplicate Request of Already Registered component, Do nothing..!
                return;
            }
            // Open 1-2-1 Queue
            if (myConfig.IsAutoRegistration|| IsRegOpen ||IsAutoRegEnabled)
            {
                //ToDo: Security, Check if Machine is member of the same Domain this SPInstance is.
                //ToDo: Security, we should check this DC as well, Just claiming isn't enough.
                //ToDo: Karamveer, Fix for Security Breach.!!Use MAC instead
                //if (msg.DomainName == myConfig.Instance.DomainName)
                //{
                //    RegisterComponent(msg);
                //    LogEvent($"RegisterComponent.. by {this.RegName}");
                //}
                //else
                //{
                //    RejectComponent(msg);
                //    LogEvent($"Rule Based Rejection.RejectComponent.. by {this.RegName}");
                //}
                RegisterComponent(msg);
                LogEvent($"RegisterComponent.. by {this.RegName}");
            }
            else
            {
                //ToDo: Notify Admin for Component Registration Request
                //ToDo: Naveen,SignalR to update StaffClient for confirming this instance for Integration.

                CompCandidates.Add(msg);
                PublishEvent(new EventNotification()
                {
                    etype = EventType.NewCandidate,
                    RegistryName = RegName,
                    Candidate = msg
                });
            }
            //var rCha = $"{msg.InstanceName}_RegRes_{msg.MacAddress}_{msg.ProcessId}";

        }
        private void RejectComponent(mHandShakePackage comp)
        {
            var isDone = JustRejectComponent(comp);
            if (!isDone) return;
            PublishEvent(new EventNotification()
            {
                etype = EventType.RejectRegistration,
                RegistryName = RegName,
                Candidate = comp
            });
            var str = ($"{this.RegName} rejected a Component Instance {comp.InstanceName}-{comp.InstanceKey}");
            LogDebug(str);
        }
        private bool JustRejectComponent(mHandShakePackage comp)
        {
            var a = CompCandidates.FirstOrDefault(x => x.InstanceKey == comp.InstanceKey);
            if (a == null) return false;
            CompCandidates.Remove(a);
            return true;
        }
        private bool RegisterComponent(RegRecord rec)
        {
            Components.Add(rec);
            LogEvent($"Budy Sync Event Update {this.RegName} for {rec.handShakePackage.ProcessId} Given AppId:{rec.Response.AppId}");
            return true;
        }
        private RegRecord RegisterComponent(mHandShakePackage comp)
        {
            var rec = new RegRecord() { Security = new Security(), handShakePackage = comp };
            var id = Guid.NewGuid();
            rec.Response = new RegCompResponse()
            {
                WatcherPrivate = id,
                AppSeed = GetSignedHash(Encoding.UTF8.GetBytes(comp.PubKey), $"{id}|{RootAppSeed}|{comp.InstanceKey}"),
                AppId = $"{ComponentGroupId}-{Components.Count + 1}",
                Key = comp.InstanceKey,
                ProcessId = comp.ProcessId.ToString()
            };
            Components.Add(rec);
            LogEvent($"Registeration Completed by {this.RegName} for {comp.ProcessId} Given AppId{rec.Response.AppId}");

            //ToDo: Sign Message with Requestee Public Key, So Only Request can see the message 
            PublishInRegistrationQueue(rec.Response);
            PublishEvent(new EventNotification()
            {
                etype = EventType.AcceptRegistration,
                RegistryName = RegName,
                RegRecord = rec
            });
            return rec;
        }
        private void PublishInRegistrationQueue(RegCompResponse response)
        {
            LogEvent($"PublishInRegistrationQueue.. by {this.RegName}");
            var Msg = JsonSerializer.Serialize(response);
            //Now Send Registration Request
            using (var pCh = Ctn.CreateModel())
            {
                if (pCh.IsOpen)
                {
                    pCh.ExchangeDeclare(Ex_NameRegRes, ExchangeType.Fanout);
                    //Now Send Registration Request
                    var bdy = Encoding.UTF8.GetBytes(Msg);
                    pCh.BasicPublish(exchange: Ex_NameRegRes, routingKey: "", basicProperties: null, body: bdy);
                    var m = $"Registration Confirmed on Ex:{Ex_NameRegRes} for {response.AppId}";
                    LogEvent(m);
                }
            }
        }
        #endregion

        private static void CreateConnection()
        {
            try
            {
                if (conFactory is null)
                {
                    Console.WriteLine("Connection Called...Let us See");
                    if (!ConfigBase.IsSet) ConfigBase.LoadDefault();
                    conFactory = new ConnectionFactory()
                    {
                        Password = ConfigBase.QPassword,
                        UserName = ConfigBase.QUserName,
                        Uri = new Uri(ConfigBase.QURL),
                        Port = ConfigBase.QPort,
                        VirtualHost = "/"
                    };
                    conFactory.RequestedHeartbeat = new TimeSpan(0,0,1800);
                    Ctn = conFactory.CreateConnection($"NavExM.Int.Watcher.WatchDog");
                    CtnReceiver = Ctn;
                    //CtnReceiver = conFactory.CreateConnection();
                    Console.WriteLine($"Connection Created Successfully.. at {DateTime.UtcNow}");

                    WatchDogVoteSetUp();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetDeepMsg());
            }
        }
        private void OpenRegistryChannells()
        {
            lock (_lock)
            {
                //Setup Collections and Prepair to receive Candidates
                SetupMyRegistryEntry();

                //ensure connection
                CreateConnection();
                //Setup and Start Independent Voting to establish Order
                WatchDogVoteSetUp();
            }
        }


        private static void Registration_Response(object? sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine($"{ConfigBase.QNameRegResponse} Received..at {DateTime.UtcNow}");
            ConfigBase.LoadDynamic();
        }


        #region NavExM Log Entry In memory Processing Queue For Summary
        // This Queue & Exchange for Releasing the Log for Database saving
        protected void SetUpLoggingExAndQueue()
        {
            lock (OThreadOAction)
            {

                LoggingHUB();
                chLog = Get_chLog();
                chLogSummaryPub = Get_chLogSummaryPub();
                if (chLog is not null && chLog.IsOpen)
                {
                    var Q = chLog.QueueDeclare(queue: "", durable: false, exclusive: true, autoDelete: false, arguments: null);
                    chLog.QueueBind(Q.QueueName, "NavExM_Logs", string.Empty);
                    if (Q != null)
                    {
                        var consumer = new EventingBasicConsumer(chLog);
                        consumer.Received += AppLogs_handler;
                        chLog.BasicConsume(Q.QueueName, autoAck: false, consumer: consumer);
                        var m = $"Exchange NavExM_Logs ESTABLISHED with Channel {Q.QueueName}";
                        LogEvent(m);
                    }
                }
            }

        }
        private void AppLogs_handler(object? sender, BasicDeliverEventArgs e)
        {
            /* This Logging Worker Thread will be Created Per Registry Class in the WatchDog for Higher Throughput. Calculation will be dealt with a Loop of DoWork. always ensure Thread Lock for Common Resource
             */
            try
            {
                var bdy = e.Body.ToArray();
                var msg = Encoding.UTF8.GetString(bdy);
                var obj = JsonSerializer.Deserialize<mLogT>(msg);
                LogList.AddLog(obj);
                chLog.BasicAck(e.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }
        static object chLog_LOCK = new object();
        private static IModel Get_chLog()
        {
            lock (chLog_LOCK)
            {
                if ((chLog is null || chLog.IsClosed) && (ShouldLogging))
                {
                    chLog = Ctn.CreateModel();
                    chLog.ExchangeDeclare("NavExM_Logs", ExchangeType.Direct);
                }
                return chLog;
            }
        }
        static object chLogSummaryPub_LOCK = new object();
        private static IModel Get_chLogSummaryPub()
        {
            lock (chLogSummaryPub_LOCK)
            {
                if (chLogSummaryPub is null || chLogSummaryPub.IsClosed)
                {
                    chLogSummaryPub = Ctn.CreateModel();
                    chLogSummaryPub.ExchangeDeclare("NavExM_LogSummary", ExchangeType.Fanout);
                }
                return chLogSummaryPub;
            }
        }
        private static void PublishToLogSummary(LogSummary pkg)
        {
            var msg = JsonSerializer.Serialize(pkg);
            var bdy = Encoding.UTF8.GetBytes(msg);
            chLogSummaryPub = Get_chLogSummaryPub();
            chLogSummaryPub.BasicPublish(exchange: "NavExM_LogSummary", routingKey: "", basicProperties: null, body: bdy);
        }
        #endregion

        #region NavExM Log DB Broadcast Queue
        // This Queue & Exchange for Releasing the Log for Database saving
        // ToDo: Remove from here since Exchange to Exchange Binding is sending message to LogDB Exchange
        //
        static object chLogDBPub_LOCK = new object();
        private static IModel Get_chLogDBPub()
        {
            lock (chLogDBPub_LOCK)
            {
                if (chLogDBPub is null || chLogDBPub.IsClosed)
                {
                    chLogDBPub = Ctn.CreateModel();
                    chLogDBPub.ExchangeDeclare("NavExM_LogDB", ExchangeType.Direct);
                }
                return chLogDBPub;
            }
        }
        private void PublishLogToDB(mLogT pkg)
        {
            var msg = JsonSerializer.Serialize(pkg);
            var bdy = Encoding.UTF8.GetBytes(msg);
            chLogSummaryPub = Get_chLogSummaryPub();
            chLogSummaryPub.BasicPublish(exchange: "NavExM_LogDB", routingKey: "", basicProperties: null, body: bdy);
        }
        //**--------------
        static IModel chLoggingHUB;
        static object chLoggingHUB_LOCK = new object();
        private void LoggingHUB()
        {
            lock (chLoggingHUB_LOCK)
            {
                if (chLoggingHUB is null || chLoggingHUB.IsClosed)
                {
                    chLoggingHUB = Ctn.CreateModel();
                    chLoggingHUB.ExchangeDeclare("NavExM_LogHUB", ExchangeType.Fanout);
                    chLoggingHUB.ExchangeDeclare("NavExM_LogDB", ExchangeType.Direct);
                    chLoggingHUB.ExchangeDeclare("NavExM_Logs", ExchangeType.Direct);
                    chLoggingHUB.ExchangeBind("NavExM_LogDB", "NavExM_LogHUB", "");
                    chLoggingHUB.ExchangeBind("NavExM_Logs", "NavExM_LogHUB", "");
                }
            }
        }
        #endregion
      
        #region Watch Dog Event
        static object chEventReceiver_LOCK = new object();
        static bool IsEventSet = false;
        protected void WatchDogEventReceiverUpdate()
        {
            lock (chEventReceiver_LOCK)//Since it is Instance Method and we want only one Object
            {

                if (chEventReceiver is null && !IsEventSet)
                {
                    chEventReceiver = Ctn.CreateModel();
                    lock (chEventReceiver)
                    {
                        chEventReceiver.ExchangeDeclare($"WatchDogE", ExchangeType.Fanout);
                        WDCofig = WDCofig ?? new RegConfig();
                        if (chEventReceiver.IsOpen)
                        {
                            var Q = chEventReceiver.QueueDeclare(queue: "", durable: true, exclusive: true, autoDelete: true, arguments: null);
                            if (Q != null)
                            {
                                chEventReceiver.QueueBind(Q.QueueName, "WatchDogE", string.Empty);
                                //chEventReceiver.BasicQos
                                var consumer = new EventingBasicConsumer(chEventReceiver);
                                consumer.Received += WatchDog_ReceiveEventUpdate;
                                chEventReceiver.BasicConsume(Q.QueueName, autoAck: false, consumer: consumer);
                                var m = $"Exchange WatchDogE is Set with Logging Channel {Q.QueueName}";
                                LogEvent(m);
                            }
                        }
                    }
                }
            }
        }
        private void WatchDog_ReceiveEventUpdate(object? sender, BasicDeliverEventArgs e)
        {
            try
            {
                var bdy = e.Body.ToArray();
                var msg = Encoding.UTF8.GetString(bdy);
                var obj = JsonSerializer.Deserialize<EventNotification>(msg);
                ProcessInwardEvent(obj);
                chEventReceiver.BasicAck(e.DeliveryTag, false);
                LogEvent($"Exchange WatchDogE provided Event Update to {WDCofig.MyId} for  {obj.etype}");
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }
        private void ProcessInwardEvent(EventNotification en)
        {
            switch (en.etype)
            {
                case EventType.AcceptRegistration:
                    if (RegName == en.RegistryName)
                        RegisterComponent(en.RegRecord);
                    break;
                case EventType.NewCandidate:
                    if (!NotExistingComponent(en.Candidate))
                        CompCandidates.Add(en.Candidate);
                    break;
                case EventType.RejectRegistration:
                    JustRejectComponent(en.Candidate);
                    break;
                default:
                    break;
            }
        }
        static object chEventPub_LOCK = new object();
        private static IModel Get_chEventPub()
        {
            lock (chEventPub_LOCK)
            {
                if (chEventPub is null || chEventPub.IsClosed)
                {
                    chEventPub = Ctn.CreateModel();
                    chEventPub.ExchangeDeclare("WatchDogE", ExchangeType.Fanout);
                }
                return chEventPub;
            }
        }

        private void PublishEvent(EventNotification pkg)
        {
            var msg = JsonSerializer.Serialize(pkg);
            var bdy = Encoding.UTF8.GetBytes(msg);
            chEventPub = Get_chEventPub();
            chEventPub.BasicPublish(exchange: "WatchDogE", routingKey: "", basicProperties: null, body: bdy);
        }
        #endregion

        #region Watch Dog Voting
        protected static void WatchDogVoteSetUp()
        {
            if (chVote is null)
            {
                chVote = CtnReceiver.CreateModel();
                lock (chVote)
                {
                    chVote.ExchangeDeclare($"WatchDogV", ExchangeType.Fanout);
                    WDCofig = WDCofig ?? new RegConfig();
                    if (chVote.IsOpen)
                    {
                        var Q = chVote.QueueDeclare(queue: "", durable: true, exclusive: true, autoDelete: true, arguments: null);
                        if (Q != null)
                        {
                            chVote.QueueBind(Q.QueueName, "WatchDogV", string.Empty);
                            //chVote.BasicQos
                            var consumer = new EventingBasicConsumer(chVote);
                            consumer.Received += WatchDog_ReceiveVotes;
                            chVote.BasicConsume(Q.QueueName, autoAck: false, consumer: consumer);
                            var m = $"Exchange WatchDogV is Set with Logging Channel {Q.QueueName}";
                            LogEvent(m);
                        }
                    }
                }
                var tVoting = new Thread(WatchDogDoVotig);
                tVoting.Start();
            }

        }
        protected static void WatchDogDoVotig()
        {
            while (true)
            {
                lock (chVote)
                {

                    if (chVote.IsOpen)
                    {
                        var v = new Vote();
                        v.Id = WDCofig.MyId;
                        v.Round = VotingManager.ThisRound + 1;
                        v.StartTime = WDCofig.StartedOn;
                        var vPack = JsonSerializer.Serialize(v);

                        var bdy = Encoding.UTF8.GetBytes(vPack);
                        chVote.BasicPublish(exchange: "WatchDogV", routingKey: "", basicProperties: null, body: bdy);
                    }
                }
                Thread.Sleep(VotingGap);//2 Sec
            }
        }

        private static void WatchDog_ReceiveVotes(object? sender, BasicDeliverEventArgs e)
        {
            try
            {
                var bdy = e.Body.ToArray();
                var msg = Encoding.UTF8.GetString(bdy);
                var obj = JsonSerializer.Deserialize<Vote>(msg);
                VotingManager.AddVote(obj);
                chVote.BasicAck(e.DeliveryTag, false);
                LogDebug($"Vote Received from {obj.Id} for Round {obj.Round}");
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        #endregion

        #region Log Messages
        internal static void LogEvent(string msg)
        {
            mLogT vL = new mLogT() { Message = msg, Type = eLogType.Event, AppId = "00" };
            if (!PublishToEvent(vL))
                Console.WriteLine(msg);
        }
        internal static void LogDebug(string msg)
        {
            mLogT vL = new mLogT() { Message = msg, Type = eLogType.Debug, AppId = "00" };

            LogList.AddLog(vL);
            var v = ConfigEx.Config.GetSection("DebugLog");
            if (v.Value != null && v.Value.ToLower() == "true")
                if (!PublishToError(vL))
                    Console.WriteLine(msg);
        }
        internal static void LogError(string msg)
        {
            mLogT vL = new mLogT() { Message = msg, Type = eLogType.Error, AppId = "00" };
          
            if (!PublishToError(vL))
                Console.WriteLine(msg);

        }
        internal static void LogError(Exception ex)
        {

            mLogT vL = new mLogT() { Message = GetMsg(ex), Type = eLogType.Error, AppId = "00" };
            LogList.AddLog(vL);
            if (!PublishToError(vL))
                Console.WriteLine(vL.Message);

        }
        internal static void LogHealthReport()
        {
            var hr = HealthDetails.GetMyHealthDetails(WDCofig.Instance.InstanceKey, "00", Helper.GetLocalIPs().FirstOrDefault()!, Helper.GetMacAddress().FirstOrDefault()!);
            var msg = JsonSerializer.Serialize(hr);
            PublishToLogSummary(new LogSummary { type = SummaryType.Health, message = msg });
        }
        internal static string GetMsg(Exception ex)
        {
            if (ex is null) return string.Empty;
            var msg = $"{ex.Message}{GetMsg(ex.InnerException)}";
            return msg;
        }
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
        #endregion

        #region Calculate Logs
        internal void CalculateLogs()
        {
            //ToDO: Naveen this is where we should calculate the Logs Groups based on Ticks
        }
        internal void CalculatePingAttendence()
        {
            if ((PingLastCheckedOn.Ticks + (TimeSpan.TicksPerSecond * CheckPingEvery)) > DateTime.UtcNow.Ticks) return;
            var t = TimeSpan.TicksPerSecond * summeryReportSpan;
            foreach (var comp in Components)
            {
                if (comp.Security.CompletedOn.AddTicks(t) > DateTime.UtcNow)
                {//Registeted less than summeryReportSpan(2 Minutes ago)
                    var max = ((DateTime.UtcNow - comp.Security.CompletedOn).Ticks / t) * 15;
                    double percent = (comp.Security.PingedOn.Count / max);
                    if (percent < PingPassPercentage)
                    {
                        var m = ComponentErrorMessage(comp, percent);
                        LogError(m);
                        PublishToLogSummary(new LogSummary { message = m, type = SummaryType.Error });
                    }
                    else
                    {
                        var m = ComponentUpdateMessage(comp, percent);
                        LogEvent(m);
                        PublishToLogSummary(new LogSummary { message = m, type = SummaryType.Update });
                    }
                }
                else
                {
                    var max = DateTime.UtcNow.Ticks - DateTime.UtcNow.AddSeconds(-summeryReportSpan).Ticks;
                    double percent = (comp.Security.PingedOn.Count / max);
                    if (percent < PingPassPercentage)
                    {
                        var m = ComponentErrorMessage(comp, percent);
                        LogError(m);
                        PublishToLogSummary(new LogSummary { message = m, type = SummaryType.Error });
                    }
                    else
                    {
                        var m = ComponentUpdateMessage(comp, percent);
                        LogEvent(m);
                        PublishToLogSummary(new LogSummary { message = m, type = SummaryType.Update });
                    }
                }
            }
            PingLastCheckedOn = DateTime.UtcNow;
        }
        string ComponentErrorMessage(RegRecord Comp, double p)
        {
            return $"Ping Error:- A Component {Comp.handShakePackage.InstanceKey} of type {Comp.Response.AppId} has Reported {p}%";
        }
        string ComponentUpdateMessage(RegRecord Comp, double p)
        {
            return $"Ping Update:- A Component {Comp.handShakePackage.InstanceKey} of type {Comp.Response.AppId} has Reported {p}%";
        }
        #endregion
    }
    public class LogSummary
    {
        public SummaryType type { get; set; }
        public string message { get; set; }
    }
    public enum SummaryType
    {
        Health, Action, Error, Update
    }
    public class EventNotification
    {
        public EventType etype { get; set; }
        public Guid AppCandidateId { get; set; }
        public string RegistryName { get; set; }
        public RegRecord RegRecord { get; set; }
        public mHandShakePackage Candidate { get; set; }
    }
    public enum EventType
    {
        AcceptRegistration, NewCandidate, RejectRegistration
    }
}
