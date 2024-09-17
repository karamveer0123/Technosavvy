using RabbitMQ.Client;
using System.Collections.Concurrent;
using System.Text;
using NavExM.Int.Maintenance.APIs.Model;
using NavExM.Int.Maintenance.APIs.ServerModel;
using NavExM.Int.Maintenance.APIs.Extension;
using System.Text.Json;
using RabbitMQ.Client.Events;
using NavExM.Int.Maintenance.APIs.Static;

namespace NavExM.Int.Maintenance.APIs.Services
{
    internal class SrvSessionManagement : AppConfigBase
    {
        /* 1. Create Ex UM_HUB
         * 2. Create Child Ex UM_Live - FantOut
         *      - It will Keep all API updated about the currecnt sessioin state of User,
         * 3. Create Child Ex UM_DB - Direct
         *      - It will Presist the Current Sesssion state and User Activity in the database
         */
        readonly string Ex_Name = "NavExM_UMHUB";
        static IModel? chSM_pub;
        static IModel? chSM_Listener;

        static SrvSessionManagement Instance;
        static ConcurrentDictionary<string, smUserSession> dSessionBySessionHash = new ConcurrentDictionary<string, smUserSession>();
        static ConcurrentDictionary<ulong, smUserAccount> dAccountByNo = new ConcurrentDictionary<ulong, smUserAccount>();
        static ConcurrentDictionary<Guid, mWalletSummery> dWalletsById = new ConcurrentDictionary<Guid, mWalletSummery>();
        static ConcurrentDictionary<ulong, List<smUserSession>> dSessionByUserAccount = new ConcurrentDictionary<ulong, List<smUserSession>>();
        IModel? ch;
        bool isAvailable = false;
        DateTime LastBeat = DateTime.MinValue;
        int BeatAfter = 100;//Sec

        public SrvSessionManagement()
        {
            Instance = this;
        }
        protected override async Task DoStart()
        {
            if (RegistryToken is null) return;
            //Ensure Listener is Alive
            EnsureUMPlugIn();
            SendHeartBeat();
            await Task.Delay(5000);
            await Task.CompletedTask;
        }
        private void EnsureUMPlugIn()
        {
            if (chSM_Listener is null || chSM_Listener.IsClosed)
            {
                chSM_Listener = Get_chSessionListSRV();
            }
        }
        private bool SendHeartBeat()
        {
            if(LastBeat.AddSeconds(BeatAfter)>DateTime.UtcNow) return false;
            LastBeat=DateTime.UtcNow;
            var pkg = new smSessionPublishWrapper
            {
                RelatedEvent = SessionEvent.HeartBeat,
                SenderAppId = RegistryToken.AppId,
                SenderTick = DateTime.UtcNow.Ticks,
            };
            return PublishToUserSessionQueue(pkg);
        }
        private bool LogOut(string SessionHash)
        {
            var pkg = new smSessionPublishWrapper
            {
                RelatedEvent = SessionEvent.LogOff,
                SenderAppId = RegistryToken.AppId,
                SenderTick = DateTime.UtcNow.Ticks,
                SessionHash = SessionHash
            };
            return PublishToUserSessionQueue(pkg);

        }
        private bool LogOutAll(string SessionHash)
        {
            var pkg = new smSessionPublishWrapper
            {
                RelatedEvent = SessionEvent.LogOffAll,
                SenderAppId = RegistryToken.AppId,
                SenderTick = DateTime.UtcNow.Ticks,
                SessionHash = SessionHash
            };
            return PublishToUserSessionQueue(pkg);
        }
     
        static object Another_Lock = new object();
        static object New_Lock = new object();
        private void _AddAnotherSession(smUserSession us)
        {
            lock (Another_Lock)
            {

                var ex = dSessionByUserAccount[us.UserAccount.AccountNumber].First();
                us.SessionCount = dSessionByUserAccount[us.UserAccount.AccountNumber].Count + 1;
                us.FundingWBal = ex.FundingWBal;
                us.EscroWBal = ex.EscroWBal;
                us.SpotWBal = ex.SpotWBal;
                us.EarnWBal = ex.EarnWBal;
                us.UserAccount = ex.UserAccount;
                dSessionBySessionHash.TryAdd(us.SessionHash, us);
                dSessionByUserAccount[us.UserAccount.AccountNumber].Add(us);
                LogEvent($"Another Session for Account:{us.UserAccount.AccountNumber} Created..");
            }
        }
        private void _AddNewSession(smUserSession us)
        {
            lock (New_Lock)
            {
                dSessionBySessionHash.TryAdd(us.SessionHash, us);
                us.SessionCount = dSessionByUserAccount[us.UserAccount.AccountNumber].Count + 1;
                var lst = dSessionByUserAccount[us.UserAccount.AccountNumber];
                lst ??= new List<smUserSession>();
                dSessionByUserAccount.TryAdd(us.UserAccount.AccountNumber, lst);
                lst.Add(us);
                var aUA = dAccountByNo[us.UserAccount.AccountNumber];
                aUA ??= us.UserAccount;
                dAccountByNo.TryAdd(aUA.AccountNumber, aUA);
                var wSpot = dWalletsById[us.SpotWBal.WalletId];
                wSpot ??= us.SpotWBal;
                dWalletsById.TryAdd(us.SpotWBal.WalletId, wSpot);

                var wFund = dWalletsById[us.FundingWBal.WalletId];
                wFund ??= us.FundingWBal;
                dWalletsById.TryAdd(us.FundingWBal.WalletId, wFund);

                var wEscro = dWalletsById[us.EscroWBal.WalletId];
                wEscro ??= us.EscroWBal;
                dWalletsById.TryAdd(us.EscroWBal.WalletId, wEscro);

                var wEarn = dWalletsById[us.EarnWBal.WalletId];
                wEarn ??= us.EarnWBal;
                dWalletsById.TryAdd(us.EarnWBal.WalletId, wEarn);


                LogEvent($"New Session for Account:{us.UserAccount.AccountNumber} Created..");
            }
        }
        internal bool _LogOutSession(string SessionHash)
        {
            lock (Another_Lock)
            {

                var ex = dSessionBySessionHash[SessionHash];
                ex.CheckAndThrowNullArgumentException();
                ex.ExpieredOn = DateTime.UtcNow;
                dSessionBySessionHash.Remove(SessionHash, out ex);
                dSessionByUserAccount[ex!.UserAccount.AccountNumber].Remove(ex);
                if (dSessionByUserAccount[ex!.UserAccount.AccountNumber].Count > 0)
                {
                    LogEvent($"One of the Session for Account:{ex.UserAccount.AccountNumber} Removed..");
                    return true;
                }
                //No More session for this Account, Remove Wallets from the List
                var ua = ex.UserAccount;
                var uw = ex.SpotWBal;
                dAccountByNo.Remove(ex.UserAccount.AccountNumber, out ua);
                dWalletsById.Remove(ex.SpotWBal.WalletId, out uw);
                dWalletsById.Remove(ex.FundingWBal.WalletId, out uw);
                dWalletsById.Remove(ex.EscroWBal.WalletId, out uw);
                dWalletsById.Remove(ex.EarnWBal.WalletId, out uw);
                LogEvent($"All Session for Account:{ex.UserAccount.AccountNumber} and Wallets Removed..");
                return true;
            }

        }
        internal bool _LogOutAllSession(string SessionHash)
        {
            lock (Another_Lock)
            {
                var ex = dSessionBySessionHash[SessionHash];
                ex.CheckAndThrowNullArgumentException();
                var uslst = dSessionByUserAccount[ex!.UserAccount.AccountNumber];
                foreach (var ss in uslst)
                {
                    dSessionBySessionHash.Remove(SessionHash, out ex);

                }
                dSessionByUserAccount.Remove(ex!.UserAccount.AccountNumber,out uslst);
                ex.ExpieredOn = DateTime.UtcNow;
                //No More session for this Account, Remove Wallets from the List
                var ua = ex.UserAccount;
                var uw = ex.SpotWBal;
                dAccountByNo.Remove(ex.UserAccount.AccountNumber, out ua);
                dWalletsById.Remove(ex.SpotWBal.WalletId, out uw);
                dWalletsById.Remove(ex.FundingWBal.WalletId, out uw);
                dWalletsById.Remove(ex.EscroWBal.WalletId, out uw);
                dWalletsById.Remove(ex.EarnWBal.WalletId, out uw);
                LogEvent($"All Session for Account:{ex.UserAccount.AccountNumber} and Wallets Removed..");
                return true;
            }
        }
        private bool EnQueueSession(eUserSession e)
        {
            if (RegistryToken == null) throw new ApplicationException("Session Management Service is not Available");
            var sess = e.ToSrvModel();
            var pkg = new smSessionPublishWrapper
            {
                RelatedEvent = SessionEvent.NewAnotherSession,
                SenderAppId = RegistryToken.AppId,
                SenderTick = DateTime.UtcNow.Ticks,
                Session = sess
            };
            var isAny = dSessionByUserAccount.Keys.Any(x => x == sess.UserAccount.AccountNumber);
            if (!isAny)
            {
                // ToDo: Ensure Wallet Tokens are properly Populated
                var wm = GetWalletManager();
                sess.SpotWBal = wm.GetWalletSummery(sess.SpotWBal.WalletId);
                sess.FundingWBal = wm.GetWalletSummery(sess.FundingWBal.WalletId);
                sess.EarnWBal = wm.GetWalletSummery(sess.EarnWBal.WalletId);
                sess.EscroWBal = wm.GetWalletSummery(sess.EscroWBal.WalletId);
                pkg.RelatedEvent = SessionEvent.NewFirstSession;//overwrite

                //AddAnotherSession(sess);
            }

            return PublishToUserSessionQueue(pkg);
        }
      
        static object Publish_Lock = new object();
        private bool PublishToUserSessionQueue(smSessionPublishWrapper us)
        {
            //if (RegistryToken is null) return false;
            lock (Publish_Lock)
            {
                 
                chSM_pub = Get_chSessionPubSRV();
                var str = JsonSerializer.Serialize(us);
                var bdy = Encoding.UTF8.GetBytes(str);
                chSM_pub.BasicPublish(exchange: Ex_Name, routingKey: "",
                    basicProperties: null, body: bdy);
                return true;
            }
        }
        static object chSessionPubSRV_LOCK = new object();
        static object chSessionListSRV_LOCK = new object();
        private IModel Get_chSessionPubSRV()
        {
            lock (chSessionPubSRV_LOCK)
            {

                if (chSM_pub is null || chSM_pub.IsClosed)
                {
                    chSM_pub = Ctn.CreateModel();
                    chSM_pub.ExchangeDeclare(Ex_Name, ExchangeType.Fanout);
                }
                return chSM_pub;
            }
        }
        private IModel Get_chSessionListSRV()
        {
            lock (chSessionListSRV_LOCK)
            {
                chSM_Listener = Ctn.CreateModel();
                chSM_Listener.ExchangeDeclare(Ex_Name, ExchangeType.Fanout);
                var QRes = chSM_Listener.QueueDeclare(queue: "", durable: true, exclusive: true, autoDelete: false, arguments: null);

                chSM_Listener.QueueBind(QRes.QueueName, Ex_Name, string.Empty);

                var consumer = new EventingBasicConsumer(chSM_Listener);
                consumer.Received += UserSessionUpdate_Listener;
                chSM_Listener.BasicConsume(QRes.QueueName, autoAck: false, consumer: consumer);
            }
            return chSM_Listener;
        }

        private void UserSessionUpdate_Listener(object? sender, BasicDeliverEventArgs e)
        {
            //now pocess the incoming session object
            var bdy = e.Body.ToArray();
            var dt = e.DeliveryTag;
            var msg = Encoding.UTF8.GetString(bdy);
            try
            {
                var data = JsonSerializer.Deserialize<smSessionPublishWrapper>(msg);
                VerifyAndActionPackage(data);
                chSM_Listener!.BasicAck(deliveryTag: dt, false);
                LogEvent($"{RegistryToken!.AppId} Processed:{data!.SenderTick} for Event{data.RelatedEvent.ToString()} ..at {DateTime.UtcNow.Ticks}");

            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }
        private void VerifyAndActionPackage(smSessionPublishWrapper pkg)
        {
            switch (pkg.RelatedEvent)
            {
                case SessionEvent.NewAnotherSession:
                    _AddAnotherSession(pkg.Session);
                    break;
                case SessionEvent.NewFirstSession:
                    _AddNewSession(pkg.Session);
                    break;
                case SessionEvent.LogOff:
                    _LogOutSession(pkg.SessionHash);
                    break;
                case SessionEvent.LogOffAll:
                    _LogOutAllSession(pkg.SessionHash);
                    break;
                case SessionEvent.TimeOut:
                    break;
                case SessionEvent.WalletUpdate:
                    break;
                default:
                    break;
            }
        }
        #region Static Methods
        internal static bool UpdateWallet(string SessionHash)
        {
            return true;
        }
        internal static bool LogOutSession(string SessionHash)
        {
            if (Instance == null) throw new ApplicationException("Session Management Service is not Available");
            SessionHash.CheckAndThrowNullArgumentException();

            return Instance.LogOut(SessionHash);

        }
        internal static bool LogOutAllSession(string SessionHash)
        {
            if (Instance == null) throw new ApplicationException("Session Management Service is not Available");
            SessionHash.CheckAndThrowNullArgumentException();
            return Instance.LogOutAll(SessionHash);
        }

        internal static bool AddSession(eUserSession e)
        {
            if (Instance == null) throw new ApplicationException("Session Management Service is not Available");
            e.CheckAndThrowNullArgumentException();

            return Instance.EnQueueSession(e);
        }
        #endregion

        private WalletManager GetWalletManager()
        {
            //ToDo: Secure this Manager, Transaction Count Applied, Active Session

            var result = new WalletManager();
            result.dbctx = GetDbctx();

            return result;
        }
        private ApiAppContext GetDbctx()
        {
            var o = new DbContextOptionsBuilder<ApiAppContext>();
            o = o.UseSqlServer(ConfigEx.Config.GetConnectionString("ApiDBContext"));
            return new ApiAppContext(o.Options);
        }

    }

}
