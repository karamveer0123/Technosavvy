using Microsoft.AspNetCore.SignalR;
using NavExM.Int.Maintenance.APIs.ServerModel;
using NavExM.Int.Maintenance.APIs.Static;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NavExM.Int.Maintenance.APIs.Services
{
    internal class SrvMarketProxy : AppConfigBase
    {
        /* - Start The Market if All Conditions are meet and Not started yet
         * - Listen to Market Admin Channel for Already Running Instances
         * - Compare Should be with What is available, Accordingly Issue Events on MarketIndex for
         *       - New Market Instance
         *       - Suspend Market
         * - Maintain a Copy of Market Profile
         * - STOP the Market if Instructed
         * - Receive User Trades, 
         *      - Check Session, 
         *      - Verify/Update Wallet Balance,
         *      - Verify Tax Residency to Allow Trade
         *      - Queue them for Settlement
         */
        // static ConcurrentDictionary<string, mMarket> dMarket = new ConcurrentDictionary<string, mMarket>();
        readonly string Ex_MarketLive = "NavExM.Market_Live";
        readonly string Ex_MarketSettlementOffice = "NavExM.MarketSettlement_Office";
        #region Function Specific Exchanges
        //----------------
        public string Ex_FIATS { get => $"NavExM.Market.Fiats"; }
        public string Ex_Name { get => $"NavExM.Market.Name"; }
        public string Ex_TOKENS { get => $"NavExM.Market.Tokens"; }
        public string Ex_ADMIN { get => $"NavExM.Market.Admin"; }
        public string Ex_ADMINSuspended { get => $"NavExM.Market.Admin.Suspended"; }
        public string Ex_ADMINRestore { get => $"NavExM.Market.Admin.Restore"; }
        public string Ex_OrderUpdate { get => $"NavExM.Market.Orders.Update"; }
        public string Ex_OrderUpdateMAPIOffice { get => $"NavExM.Market.Orders.Update.MAPIOffice"; }
        public string Ex_Order { get => $"NavExM.Market.Orders"; }
        public string Ex_OrderAck { get => $"NavExM.Market.Orders.Ack"; }
        public string Ex_OrderAckOffice { get => $"NavExM.Market.Orders.Ack.Office"; }
        public string Ex_RejectOrder { get => $"NavExM.Market.Orders.Reject"; }
        //   public string Ex_CancelOrder { get => $"NavExM.Market.Orders.Cancel"; }
        public string Ek_2ndBookTriggered { get => $"NavExM.Market.2ndBookTriggered"; }
        public string Ex_2ndBuyBook { get => $"NavExM.Market.2ndBuyBook"; }
        public string Ex_2ndSellBook { get => $"NavExM.Market.2ndSellBook"; }
        public string Ex_BuyBook { get => $"NavExM.Market.BuyBook"; }
        public string Ex_SellBook { get => $"NavExM.Market.SellBook"; }
        public string Ex_Trades { get => $"NavExM.Market.Trades"; }
        public string Ex_TradeMAPIOffice { get => $"NavExM.Market.Trades.MAPIOffice"; }
        public string Ex_MMOrder { get => $"NavExM.Market.MMOrder"; }
        public string Ex_MMOrderMAPIOffice { get => $"NavExM.Market.MMOrder.MAPIOffice"; }
        #endregion

        static SrvMarketProxy Instance;
        static IModel? chSM_pub;
        // static IModel? chSM_Listener;
        IModel Ch_AdminListner = null;
        IModel Ch_Admin = null;
        IModel Ch_Tokens = null;
        IModel Ch_Fiats = null;
        IModel Ch_MarketName = null;
        static IModel chMarketConn;
        ConcurrentBag<string> lstToken = new ConcurrentBag<string>();
        ConcurrentBag<string> lstFiats = new ConcurrentBag<string>();

        bool InitialLoad = false;
        DateTime LastMarketCheck = DateTime.MinValue;
        static int PingExpectation { get { return Convert.ToInt16(ConfigEx.Config.GetSection("MarketPingExpectation").Value); } }


        public SrvMarketProxy()
        {
            Instance = this;
        }
        protected override async Task DoStart()
        {
            if (RegistryToken is null) return;
            DoStartUpAction();
            if (!InitialLoad)
            {
                InitialLoad = true;
                //LoadTokensState();
                //LoadFiatsState();
            }
            //Ensure Listener is Alive
            EnsureMarketAdminPlugIn();
            EnsureAllMarketsAreInAction();
            EnsureOrderAreReleasedToSettlement();
            SetUpMarketMakingOrderListner();
            pulse = 1000;//1 sec
            await Task.CompletedTask;
        }
        bool isDisplay;
        private void DoStartUpAction()
        {
            if (RegistryToken is null) return;
            ConName = ".MarketProxy";
            EnsureMarketConnectionBuild();
            SetUpOrderAcknowledgmentListner();
            SetUpTradeListner();
            SetUpOrderUpdateListner();
            if (!isDisplay)
            {
                LogDebug($"{RegistryToken!.AppId} MarketProxy initialized");
                isDisplay = true;
                Console2.WriteLine_White($"Info|{T}| DoStartUpAction called..at:{DateTime.UtcNow}");
            }
        }
        private void EnsureAllMarketsAreInAction()
        {
            /* - Wait 100 Sec for First Run and Every runafter.
             * - Load Eligible Markets from database.
             * - Compare If Desired Market is not Running/Provisioned Then Request an Instance
             */

            if (LastMarketCheck.AddSeconds(10) <= DateTime.UtcNow)
            {
                LastMarketCheck = DateTime.UtcNow;
                //Console2.WriteLine_White($"Info|{T}| Token & Market Name Publication called..at:{DateTime.UtcNow}");
                GetMarketsAndEnsureOp();
            }
        }

        #region Load State

        //private void LoadTokensState()
        //{//Load Tokens that Are Approved for Markets and have already Released
        //    Ch_Tokens = Ctn_CreateModel(Ch_Tokens);
        //    var QRes = Ch_Tokens.QueueDeclare(queue: $"{Ex_TOKENS}_QuHOLD", durable: true, exclusive: false, autoDelete: false, arguments: null);

        //    Ch_Tokens.QueueBind(QRes.QueueName, Ex_TOKENS, string.Empty);

        //    var consumer = new EventingBasicConsumer(Ch_Tokens);
        //    int MsgCount = Convert.ToInt32(Ch_Tokens.MessageCount($"{Ex_TOKENS}_QuHOLD"));
        //    consumer.Received += (s, e) =>
        //    {
        //        MsgCount--;
        //        var bdy = e.Body.ToArray();
        //        var dt = e.DeliveryTag;
        //        var msg = Encoding.UTF8.GetString(bdy);
        //        var data = JsonSerializer.Deserialize<string>(msg);
        //        if (!lstToken.Contains(data))
        //            lstToken.Add(data);
        //    };
        //    Ch_Tokens.BasicConsume(QRes.QueueName, autoAck: false, consumer: consumer);
        //    while (MsgCount > 0)
        //    {
        //        {
        //            Thread.Sleep(250);
        //        }
        //    }
        //    Ch_Tokens.Close();
        //    LogEvent($"{RegistryToken!.AppId}:SrvMarketProxi has Loaded {lstToken.Count} Tokens from {Ex_TOKENS}_Qu");
        //}
        //private void LoadFiatsState()
        //{
        //    Ch_Fiats = Ctn_CreateModel(Ch_Fiats);
        //    var QRes = Ch_Fiats.QueueDeclare(queue: $"{Ex_FIATS}_QuHOLD", durable: true, exclusive: false, autoDelete: false, arguments: null);

        //    Ch_Fiats.QueueBind(QRes.QueueName, Ex_FIATS, string.Empty);

        //    var consumer = new EventingBasicConsumer(Ch_Fiats);
        //    int MsgCount = Convert.ToInt32(Ch_Fiats.MessageCount($"{Ex_FIATS}_QuHOLD"));
        //    consumer.Received += (s, e) =>
        //    {
        //        MsgCount--;
        //        var bdy = e.Body.ToArray();
        //        var dt = e.DeliveryTag;
        //        var msg = Encoding.UTF8.GetString(bdy);
        //        var data = JsonSerializer.Deserialize<string>(msg);
        //        if (!lstFiats.Contains(data))
        //            lstFiats.Add(data);
        //    };
        //    Ch_Fiats.BasicConsume(QRes.QueueName, autoAck: false, consumer: consumer);
        //    while (MsgCount > 0)
        //    {
        //        {
        //            Thread.Sleep(250);
        //        }
        //    }
        //    Ch_Fiats.Close();
        //    LogEvent($"{RegistryToken!.AppId}:SrvMarketProxi has Loaded {lstFiats.Count} Fiats from {Ex_FIATS}_Qu");
        //}
        #endregion

        #region Publish Commands
        static object Publish_Lock = new object();
        private bool PublishToMarket_Live(smMarketPublishWrapper us)
        {
            //if (RegistryToken is null) return false;
            lock (Publish_Lock)
            {
                chSM_pub = Ctn_CreateModel(chSM_pub);
                var str = JsonSerializer.Serialize(us);
                var bdy = Encoding.UTF8.GetBytes(str);
                chSM_pub.BasicPublish(exchange: Ex_MarketLive, routingKey: "",
                    basicProperties: null, body: bdy);
                return true;
            }
        }
        private bool PublishToSuspendQue_Live(string mCode)
        {
            //if (RegistryToken is null) return false;
            lock (Publish_Lock)
            {
                chSM_pub = Ctn_CreateModel(chSM_pub);
                var str = JsonSerializer.Serialize(mCode);
                var bdy = Encoding.UTF8.GetBytes(str);
                chSM_pub.BasicPublish(exchange: Ex_ADMINSuspended, routingKey: "",
                    basicProperties: null, body: bdy);
                return true;
            }
        }
        private bool PublishToTokensEx(string mCode)
        {
            //if (RegistryToken is null) return false;
            lock (Publish_Lock)
            {
                Ch_Tokens = Ctn_CreateModel(Ch_Tokens);
                var str = JsonSerializer.Serialize(mCode);
                var bdy = Encoding.UTF8.GetBytes(str);
                Ch_Tokens.BasicPublish(exchange: Ex_TOKENS, routingKey: "",
                    basicProperties: null, body: bdy);
                Console2.WriteLine_White($"{RegistryToken!.AppId}:SrvMarketProxy has Published Token {mCode} to {Ex_TOKENS}");
                return true;

            }
        }
        private bool PublishToCurrencyEx(string mCode)
        {
            //if (RegistryToken is null) return false;
            lock (Publish_Lock)
            {
                Ch_Fiats = Ctn_CreateModel(Ch_Fiats);
                var str = JsonSerializer.Serialize(mCode);
                var bdy = Encoding.UTF8.GetBytes(str);
                Ch_Fiats.BasicPublish(exchange: Ex_FIATS, routingKey: "",
                    basicProperties: null, body: bdy);
                Console2.WriteLine_White($"{RegistryToken!.AppId}:SrvMarketProxy has Published Fiat {mCode} to {Ex_FIATS}");
                return true;
            }
        }
        /// <summary>
        /// Publish List of Market Name(code) that are Approved for Trading i.e. ETHUSDT,ETHNavC, ETHINR,USDTINR
        /// </summary>
        /// <param name="mCode"></param>
        /// <returns></returns>
        private bool PublishToMarketNameEx(PriceIndex mCode)
        {
            //if (RegistryToken is null) return false;
            lock (Publish_Lock)
            {
                Ch_MarketName = Ctn_CreateModel(Ch_MarketName);
                var str = JsonSerializer.Serialize(mCode);
                var bdy = Encoding.UTF8.GetBytes(str);
                Ch_MarketName.BasicPublish(exchange: Ex_Name, routingKey: "",
                    basicProperties: null, body: bdy);
                Console2.WriteLine_White($"{RegistryToken!.AppId}:SrvMarketProxy has Published Market Name {mCode.mCode} to {Ex_Name}");
                return true;
            }
        }

        private bool PublishToMarketAdmin(SettlementAdminPackage pkg)
        {
            //if (RegistryToken is null) return false;
            lock (Publish_Lock)
            {
                var str = JsonSerializer.Serialize(pkg);
                var bdy = Encoding.UTF8.GetBytes(str);
                Ch_Admin = Ctn_CreateModel(chSM_pub);
                Ch_Admin.BasicPublish(exchange: $"{Ex_ADMIN}.{pkg.MarketCode}", routingKey: "",
                    basicProperties: null, body: bdy);
                LogEvent($"Market {pkg.MarketCode} SWITCH Instance Instruction Issued to {pkg.SuggestedPrimary} to takeOver at.. {DateTime.UtcNow}");
                return true;
            }
        }
        private SettlementAdminPackage GetPackage_SwitchRole(string mCode, Guid Current, Guid Proposed)
        {
            var retval = new SettlementAdminPackage
            {
                Event = SettlementAdminEvent.SwitchRoleToActive,
                MarketCode = mCode,
                SentAt = DateTime.UtcNow,
                SenderAppId = RegistryToken!.AppId,
                SenderInstancId = RegistryToken!.Key,
                ExistingPrimary = Current,
                SuggestedPrimary = Proposed
            };
            return retval;
        }

        #endregion
        static object chMarketPubSRV_LOCK = new object();
        static object chMarketListSRV_LOCK = new object();
        private List<MarketInstances> GetMarketSettlementDetails(string mCode)
        {
            return MarketList.Where(x => x.MarketCode == mCode).ToList();
        }
        private void SuspendMarket(string mCode)
        {
            PublishToSuspendQue_Live(mCode);
            LogEvent($"Market {mCode} Suspension Request Published as on {DateTime.UtcNow}");
        }

        private bool SWITCHInstance(string mCode, Guid Current, Guid Proposed)
        {
            PublishToMarketAdmin(GetPackage_SwitchRole(mCode, Current, Proposed));

            return true;
        }
        private bool RestoreMarketService(string mCode)
        {
            //Ack Message on Suspend for this MarketCode
            //Publish to Restore Queue
            PublishToSuspendQue_Live($"-{mCode}");
            LogEvent($"Market {mCode} Restore Request Published as on {DateTime.UtcNow}");
            return true;
        }
        bool IsOrderReleaseRunning = false;
        private void EnsureOrderAreReleasedToSettlement()
        {
            if (!IsOrderReleaseRunning)
                new Thread(() =>
                {
                    IsOrderReleaseRunning = true;
                    while (!myCancellationToken.IsCancellationRequested)
                    {
                        foreach (var M in Orders.Keys)
                        {
                            if (!AllMarketsOrders.ContainsKey(M))
                            {
                                //Done:Each Market have its Own Queue for Order Release
                                var t = new Thread(() =>
                                {
                                    while (!myCancellationToken.IsCancellationRequested)
                                    {
                                        try
                                        {
                                            //Done, Change Logic to ensure Rate of Release is per Market, not over all
                                            var mCode = M;
                                            var dm = DateTime.UtcNow.Minute;
                                            var Que = Orders[mCode];
                                            while (Que.TryDequeue(out var o))
                                            {
                                                Console2.WriteLine_Green($"{T}: Now Releaseing order id:{o.Order.OrderID}");
                                                if (o != null)
                                                {
                                                    BroadCastOrderToSettelment(o);
                                                    LogDebug($"{RegistryToken!.AppId} Holds {Que.Count} Orders for Market:{mCode} at {DateTime.UtcNow}");
                                                }
                                            }
                                            if (dm != DateTime.UtcNow.Minute)
                                            {
                                                dm = DateTime.UtcNow.Minute;
                                                Console2.WriteLine_White($"{T}: is working on Order Release for:{mCode}");

                                            }
                                            Thread.Sleep(50);
                                        }
                                        catch (Exception ex)
                                        {
                                            LogError($"{RegistryToken!.AppId} Reporting Error in EnsureOrderAreReleasedToSettlement for Market:{M} at {DateTime.UtcNow}");
                                            LogError(ex);
                                            LogError(ex.StackTrace ?? "StackTrace is empty");
                                        }
                                    }
                                });
                                AllMarketsOrders.TryAdd(M, t);
                                t.Start();
                            }
                        }
                    }
                    IsOrderReleaseRunning = false;
                    LogEvent($"Info|{RegistryToken!.AppId} Notification Loop Thread Closed");
                }).Start();
        }
        #region Static Methods
        static ConcurrentDictionary<string, mMarket> MktQueList = new ConcurrentDictionary<string, mMarket>();
        static ConcurrentDictionary<string, mTradingFee> MktSWAPFee = new ConcurrentDictionary<string, mTradingFee>();
        static ConcurrentDictionary<string, ConcurrentQueue<eInternalWallet>> MktOrderWallet = new ConcurrentDictionary<string, ConcurrentQueue<eInternalWallet>>();
        static ConcurrentDictionary<string, ConcurrentQueue<smOrderPublishWrapper>> Orders = new ConcurrentDictionary<string, ConcurrentQueue<smOrderPublishWrapper>>();
        static ConcurrentDictionary<string, Thread> AllMarketsOrders = new ConcurrentDictionary<string, Thread>();
        static List<MarketInstances> MarketList = new List<MarketInstances>();


        #region Order rate Limiting
        public static bool PlaceOrUpdateOrder(smOrderPublishWrapper wobj)
        {

            if (Orders.TryGetValue(wobj.Order.MarketCode.ToLower(), out var Que))
            {
                Que.Enqueue(wobj);
                Console2.WriteLine_White($"Order from User:{wobj.Order.UserAccountNo} has been Queued for Market:{wobj.Order.MarketCode} to be Released in accordance with Release rate.");
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                Orders.Keys.ToList().ForEach(x => sb.Append($"{x} |"));
                SrvPlugIn.LogDebugG($"MAPI has Received order for {wobj.Order.MarketCode} when No Such Market Queue Exists with me. I only support:{sb} as of {DateTime.UtcNow}");
                return false;
            }
            return true;
        }
        static int PerSecOrderRate { get => Convert.ToInt16(ConfigEx.Config.GetSection("PerSecOrderRate").Value); }

        static ConcurrentDictionary<string, RateStructuer> RateBox = new ConcurrentDictionary<string, RateStructuer>();
        static string GetStamp()
        {
            return DateTime.UtcNow.ToString("dd.MMM.yy.HH.mm.ss");
        }
        static void LimitRateOfOrder_ByBLOCKING(string mCode)
        {
            if (PerSecOrderRate <= 0) return;//That mean,Disable Blocking
            mCode = mCode.ToLower();
            RateStructuer box;
            if (!RateBox.ContainsKey(mCode))
            {
                box = new RateStructuer();
                box.TimeStamp = GetStamp();
                RateBox.TryAdd(mCode, box);
            }
            box = RateBox[mCode];
            if (box.TimeStamp == GetStamp())
            {
                if (box.RateCounter >= PerSecOrderRate)
                {
                    Instance.LogDebug($"{RegistryToken!.AppId} Market Proxy Order Rate for {mCode} has Reached Limit of {PerSecOrderRate} at {DateTime.UtcNow} So Thread:{Thread.GetCurrentProcessorId} Blocked");

                    while (box.TimeStamp == GetStamp())
                    {
                        Thread.Sleep(20);
                        box.RateCounter = 1;
                    }
                    box.TimeStamp = GetStamp();

                    Instance.LogDebug($"{RegistryToken!.AppId} Market Proxy Thread:{Thread.GetCurrentProcessorId} Released fro Market:{mCode}");
                }
                else
                {
                    box.RateCounter++;
                    Console.WriteLine($"{mCode} is {box.RateCounter}");
                }

            }
            else
            {
                box.TimeStamp = GetStamp();
                box.RateCounter = 1;
                Console.WriteLine($"{mCode} is {box.RateCounter}");
            }
        }

        #endregion

        static IModel ch_OrderBroadCasting;
        static internal void BroadCastOrderToSettelment(smOrderPublishWrapper op)
        {
            op.Order.MarketCode = op.Order.MarketCode.ToUpper();
            LimitRateOfOrder_ByBLOCKING(op.Order.MarketCode);
            string Mk_Order = $"NavExM.Market.Orders.{op.Order.MarketCode}";
            //if (op.Order.BaseTokenCodeName.ToLower() == "navc")
            //{
            //    Mk_Order = $"NavExM.Market.Orders.NavC{op.Order.QuoteTokenCodeName.ToUpper()}";
            //}
            //else if (op.Order.QuoteTokenCodeName.ToLower() == "navc")
            //{
            //    Mk_Order = $"NavExM.Market.Orders.{op.Order.BaseTokenCodeName.ToUpper()}NavC";
            //}
            //else
            //{
            //    Mk_Order = $"NavExM.Market.Orders.{op.Order.MarketCode.ToUpper()}";
            //}

            lock (Publish_Lock)
            {
                ch_OrderBroadCasting = Instance.Ctn_CreateModel(ch_OrderBroadCasting);

                var str = JsonSerializer.Serialize(op);
                var bdy = Encoding.UTF8.GetBytes(str);
                ch_OrderBroadCasting.BasicPublish(exchange: Mk_Order, routingKey: "",
                        basicProperties: null, body: bdy);
                //Instance.LogDebug($"Order:{op.Order.OrderID} BroadCasted To Settlement...");
                Console2.WriteLine_RED($"Order:{op.Order.OrderID} for Market:{op.Order.MarketCode} BroadCasted To Settlement Service Exchange:{Mk_Order}...at:{DateTime.UtcNow}");
            }
        }

        internal static Guid GetOrderWalletForMarket(string mCode)
        {

            var wKey = MktOrderWallet.FirstOrDefault(x => x.Key.ToLower() == mCode.ToLower());
            if (wKey.Value != null)
            {
                return wKey.Value.ToList().First(x => x.WalletType == eInternalWalletType.Market && x.WalletNature == eWalletNature.Global).InternalWalletId;
            }
            else
                throw new ApplicationException($"No Such Market:{mCode} or Wallet is available");
        }
        /// <summary>
        /// Returns SWAP rate of the provided Market for Provided Country
        /// </summary>
        /// <param name="mCode"></param>
        /// <param name="countryId"></param>
        /// <param name="BuySWAP"></param>
        /// <param name="SellSWAP"></param>
        /// <returns>
        /// Null if No Profile Provisioned for Such Country
        /// </returns>
        /// <exception cref="ApplicationException">If Invalid Country Id Provided</exception>
        internal static bool GetSWAPRateForMarket(string mCode, Guid countryId, out eTradingFee? BuySWAP, out eTradingFee? SellSWAP)
        {
            Guid gid = Guid.Empty;
            //Fee of Maker and Taker | Buyer and Seller are Same
            //Though system is designed to provision these SWAP Fees seprately
            //But Same Object can be assigned to all such types
            var Mkt = MktQueList.FirstOrDefault(x => x.Key.ToLower() == mCode.ToLower());
            if (Mkt.Value != null)
            {
                //Specific to provided Country
                var res = Mkt.Value;
                var pro = res.MarketProfile.FirstOrDefault(x => x.ProfileFor.Any(x => x.CountryId == countryId));

                if (pro == null)
                {
                    var ct = GetDbctx().Country.FirstOrDefault(x => x.CountryId == countryId);
                    if (ct == null)
                    { throw new ApplicationException($"No Such Country:{countryId}  Exist"); }
                    BuySWAP = SellSWAP = null;
                    Console2.WriteLine_RED($"No Such Market:{mCode} or Profile for Country:{ct.Name} is available");
                    return false;
                }
                using (var db = GetDbctx())
                {
                    BuySWAP = db.TradingFee.First(x => x.TokenFeeId == pro.BaseTokenMakerFeeId);
                    SellSWAP = db.TradingFee.First(x => x.TokenFeeId == pro.QuoteTokenMakerFeeId);
                }
                return true;
            }

            throw new ApplicationException($"No Such Market:{mCode} or SWAP Wallet is available");
        }
        internal static Guid GetSWAPWalletForMarket(string mCode, Guid countryId)
        {
            Guid gid = Guid.Empty;
            var wKey = MktOrderWallet.FirstOrDefault(x => x.Key.ToLower() == mCode.ToLower());
            if (wKey.Value != null)
            {
                //Specific to provided Country
                var res = wKey.Value.ToList().FirstOrDefault(x => x.WalletType == eInternalWalletType.Swap && x.RelatedCountryId.HasValue && x.RelatedCountryId == countryId);


                if (res == null)
                {
                    //Use Global Wallet since no such Specific Wallet
                    res = wKey.Value.ToList().First(x => x.WalletType == eInternalWalletType.Swap && x.WalletNature == eWalletNature.Global);
                }

                return res.InternalWalletId;
            }

            throw new ApplicationException($"No Such Market:{mCode} or SWAP Wallet is available");
        }
        internal static List<MarketInstances> GetMarketInstances()//ok
        {
            return MarketList.ToList();
        }

        #region Order Copy Save in DB
        OrderAck? SaveToDB(OrderAck ok)
        {
            if (ok == null) return ok;
            using (var db = Orderctx(ok.mCode))
            {
                try
                {
                    db.OrderAck.Add(ok);
                    db.SaveChanges();
                    return ok;
                }
                catch (Exception ex)
                {
                    Console2.WriteLine_RED($"{T}|ERROR:SaveToDB for Trade:{ex.GetDeepMsg()}");
                }

            }
            return null;
        }
        OrderAck? MMSaveToDB(OrderAck ok)
        {
            if (ok == null) return ok;
            using (var db = MMOrderctx(ok.mCode))
            {
                try
                {
                    db.OrderAck.Add(ok);
                    db.SaveChanges();
                    return ok;
                }
                catch (Exception ex)
                {
                    Console2.WriteLine_RED($"{T}|ERROR:SaveToDB for Trade:{ex.GetDeepMsg()}");
                }

            }
            return null;
        }
        smTrade? SaveToDB(smTrade tr)
        {
            if (tr == null) return tr;
            using (var db = Orderctx(tr.MarketCode))
            {
                try
                {
                    db.Trades.Add(tr);
                    db.SaveChanges();
                    return tr;
                }
                catch (Exception ex)
                {
                    Console2.WriteLine_RED($"{T}|ERROR:SaveToDB for Trade:{ex.GetDeepMsg()}");
                }

            }
            return null;
        }

        void NotifyWebClient(string uAcc, string mCode)
        {

        }
        void NotifyWebClient(string uAcc, string mCode, string fName)
        {
            //Report Website App
            try
            {
                Console2.WriteLine_White($"Notifing WebUI over SignalR  for:{uAcc} from:{fName} at..{DateTime.UtcNow}");
                if (APIHub.AllClients != null && APIHub.AllClients.All != null)
                {
                    APIHub.AllClients.All.SendAsync("OrderTradeUpdate", uAcc, mCode);
                    Console2.WriteLine_White($"Notification Sent to WebUI over SignalR  for:{uAcc} from:{fName} at ..{DateTime.UtcNow}");
                }
                Console2.WriteLine_Green($"WebUI Connection Count is:{APIHub.conCount}");
            }
            catch (Exception ex)
            {
                Console2.WriteLine_RED($"NotifyWebClient caused ERROR on SignalR  for:{uAcc} from:{fName} at ..{DateTime.UtcNow}|ERROR:{ex.GetDeepMsg()}");

            }


        }
        void RelayNotificationToClient(smTrade tr)
        {
            if (tr == null) return;
            using (var db = Orderctx(tr.MarketCode))
            {
                var bo = db.Orders.FirstOrDefault(x => x.InternalOrderID == tr.BuyInternalId);
                if (bo != null && bo.UserAccountNo.IsNOT_NullorEmpty())
                {
                    NotifyWebClient(bo.UserAccountNo, bo.MarketCode, "TradeBuySide");
                    Console2.WriteLine_White($"{bo.UserAccountNo}| Notified.. for {tr.TradeVolumn}");
                }
                var so = db.Orders.FirstOrDefault(x => x.InternalOrderID == tr.SellInternalId);
                if (so != null && so.UserAccountNo.IsNOT_NullorEmpty())
                {
                    NotifyWebClient(so.UserAccountNo, so.MarketCode, "TradeSellSide");
                    Console2.WriteLine_White($"{so.UserAccountNo}| Notified.. for {tr.TradeVolumn}");
                }

            }
        }
        eProcessedOrder? SaveToDB(eProcessedOrder po)
        {
            if (po == null) return po;
            using (var db = Orderctx(po.MarketCode))
            {
                try
                {
                    po.myOrder = db.Orders.FirstOrDefault(x => x.InternalOrderID == po.InternalOrderID);
                    db.ProcessedOrder.Add(po);
                    db.SaveChanges();
                    return po;
                }
                catch (Exception ex)
                {
                    Console2.WriteLine_RED($"{T}|ERROR:SaveToDB for ProcessedOrder:{ex.GetDeepMsg()}");
                }

            }
            return null;
        }
        eProcessedOrder? SaveToMMOrderDB(eProcessedOrder po)
        {
            if (po == null) return po;
            using (var db = MMOrderctx(po.MarketCode))
            {
                try
                {
                    po.myOrder = db.Orders.FirstOrDefault(x => x.InternalOrderID == po.InternalOrderID);
                    db.ProcessedOrder.Add(po);
                    db.SaveChanges();
                    return po;
                }
                catch (Exception ex)
                {
                    Console2.WriteLine_RED($"{T}|ERROR:SaveToMMOrderDB for ProcessedOrder:{ex.GetDeepMsg()}");
                }

            }
            return null;
        }
        #endregion
        /// <summary>
        /// List of Markets that are allowed to Trade. Not Suspended
        /// That are Activly Running/Ping with Instandards
        /// </summary>
        /// <returns></returns>
        internal static List<string> GetLiveMarkets()//ok
        {
            return MarketList.Where(x =>
            x.isActive == true
            && x.LastPingOn.AddSeconds(PingExpectation) >= DateTime.UtcNow
            ).Select(x => x.MarketCode).ToList();
        }
        internal static void STOPMarketInstance(string mCode) //ok
        {
            Instance.CheckAndThrowNullArgumentException($"Market Proxy is not Yet Available  {DateTime.UtcNow}");
            Instance.SuspendMarket(mCode);
        }
        internal static List<MarketInstances> GetMarketSettlementInstancesOf(string mCode)
        {
            Instance.CheckAndThrowNullArgumentException($"Market Proxy is not Yet Available  {DateTime.UtcNow}");
            return Instance.GetMarketSettlementDetails(mCode);
        }
        internal static bool SWITCHSettlementInstance(string mCode, Guid Current, Guid Proposed)
        {
            Instance.CheckAndThrowNullArgumentException($"Market Proxy is not Yet Available  {DateTime.UtcNow}");
            return Instance.SWITCHInstance(mCode, Current, Proposed);
        }
        internal static bool RestoreSUSPENDEDMarket(string mCode)
        {
            Instance.CheckAndThrowNullArgumentException($"Market Proxy is not Yet Available  {DateTime.UtcNow}");
            return Instance.RestoreMarketService(mCode);

        }
        internal static List<mMarket> GetMarketsWithOperatingQueue()
        {
            return MktQueList.Values.ToList();
        }
        #endregion
        PriceIndex FromMarket(mMarket m)
        {
            return new PriceIndex { mCode = m.CodeName, Base = m.BaseToken != null ? m.BaseToken.Code : "", Quote = m.QuoteCurrency != null ? m.QuoteCurrency.Code : "", Price = 0 };
        }
        private void GetMarketsAndEnsureOp()
        {
            Console2.WriteLine_White($"Info|{T}| GetMarketsAndEnsureOp called..at:{DateTime.UtcNow}");
            var mm = GetMarketManager();
            int count = 0;
            int pSize = 50;
            List<eInternalWallet> ilst = new List<eInternalWallet>();
            var lst = mm.GetAllActiveMarketPair(pSize, count);
            LogDebug($"{RegistryToken!.AppId} SrvMarketProxy has establsied that there are {lst.Count} Active Market Pairs.");
            using (var db = GetDbctx())
            {
                ilst = db.InternalWallet.ToList();
            }


            while (lst.Count > 0)
            {
                count += pSize;
                foreach (var item in lst)
                {
                    //Token and Market Name must be published every cycle so every new and old consumer have this information
                    PublishToTokensEx(item.BaseToken!.Code);
                    if (item.QuoteToken != null)
                        PublishToTokensEx(item.QuoteToken!.Code);
                    if (item.QuoteCurrency != null && item.QuoteCurrency.Code.IsNOT_NullorEmpty())
                        PublishToCurrencyEx(item.QuoteCurrency!.Code);
                    PublishToMarketNameEx(FromMarket(item));

                    if (!lstToken.Contains(item.BaseToken!.Code))
                    {
                        lstToken.Add(item.BaseToken!.Code);
                    }
                    if (item.QuoteToken != null && !lstToken.Contains(item.QuoteToken!.Code))
                    {
                        lstToken.Add(item.QuoteToken!.Code);
                    }
                    if (item.QuoteCurrency != null && !lstFiats.Contains(item.QuoteCurrency!.Code))
                    {
                        lstFiats.Add(item.QuoteCurrency!.Code);
                    }

                    //Check and Add Order Queue for this Market for Controller Broadcast
                    Orders.TryAdd(item.CodeName.ToLower(), new ConcurrentQueue<smOrderPublishWrapper>());
                    //Maintain SWAP Rate
                    //MktSWAPFee

                    //Maintain Complete List of Markets with all Referece Objects here for Quick Information Access and avoid round trip to database for each order.
                    var m = ilst.Where(x => x.BelongsTo.ToLower() == item.CodeName.ToLower()).ToList();
                    if (m != null)
                    {
                        ConcurrentQueue<eInternalWallet> wq = new ConcurrentQueue<eInternalWallet>();
                        m.ForEach(x => wq.Enqueue(x));
                        MktOrderWallet.TryAdd(item.CodeName.ToLower(), wq);
                        MktQueList.TryAdd(item.CodeName.ToLower(), item);
                    }
                    Console2.WriteLine_White($"Order Queue for {item.CodeName} has been established at {DateTime.UtcNow}"); ;

                }
                lst = mm.GetAllActiveMarketPair(pSize, count);
            }
        }


        #region Market Admin
        static readonly object chMarketConnection_LOCK = new object();

        static object PublishAdmin_Lock = new object();
        static object ListenAdmin_Lock = new object();
        private void EnsureMarketConnectionBuild()
        {
            lock (chMarketConnection_LOCK)
            {
                if (chMarketConn is null || chMarketConn.IsClosed)
                {
                    Console2.WriteLine_White($"Info|{T}| EnsureMarketConnectionBuild called..at:{DateTime.UtcNow}");
                    chMarketConn = Ctn_CreateModel(chMarketConn);
                    //Market Level Exchange
                    chMarketConn.ExchangeDeclare(Ex_ADMIN, ExchangeType.Fanout);
                    chMarketConn.ExchangeDeclare(Ex_TOKENS, ExchangeType.Fanout);
                    chMarketConn.ExchangeDeclare(Ex_FIATS, ExchangeType.Fanout);
                    chMarketConn.ExchangeDeclare(Ex_ADMINSuspended, ExchangeType.Fanout);
                    chMarketConn.ExchangeDeclare(Ex_ADMINRestore, ExchangeType.Fanout);
                    chMarketConn.ExchangeDeclare(Ex_Order, ExchangeType.Fanout);
                    chMarketConn.ExchangeDeclare(Ex_OrderUpdate, ExchangeType.Fanout);
                    chMarketConn.ExchangeDeclare(Ex_RejectOrder, ExchangeType.Fanout);
                    chMarketConn.ExchangeDeclare(Ex_2ndBuyBook, ExchangeType.Fanout);
                    chMarketConn.ExchangeDeclare(Ex_2ndSellBook, ExchangeType.Fanout);
                    chMarketConn.ExchangeDeclare(Ek_2ndBookTriggered, ExchangeType.Fanout);
                    chMarketConn.ExchangeDeclare(Ex_BuyBook, ExchangeType.Fanout);
                    chMarketConn.ExchangeDeclare(Ex_SellBook, ExchangeType.Fanout);
                    chMarketConn.ExchangeDeclare(Ex_Trades, ExchangeType.Fanout);

                    chMarketConn.ExchangeDeclare(Ex_MarketLive, ExchangeType.Fanout);
                    chMarketConn.ExchangeDeclare(Ex_MarketSettlementOffice, ExchangeType.Direct);

                }
            }
        }

        //----------------- Market Making Order Listner
        ConcurrentDictionary<string, mUserSession> lstMM = new ConcurrentDictionary<string, mUserSession>();
        mUserSession GetMMSession(string mCode)
        {
            //Note:Even if Session Expired, we can still place MM Order
            Console2.WriteLine_Green($"MMOrder Auth Session Request for {mCode}");
            if (!lstMM.TryGetValue(mCode.ToUpper(), out var sess))
            {
                var mm = GetMarketManager();
                var um = GetUserManager();
                if (!um.IsAny(mm.GetMarketMakingAccountName(mCode)))
                    mm.CreateMarketMakingAccount(mCode);
                sess = mm.GetMarketMakingSession(mCode);
                lstMM.AddOrUpdate(mCode.ToUpper(), sess, (k, v) => sess);
                Console2.WriteLine_Green($"MMOrder Auth Session for:{mCode} SpotWallet:{sess!.SpotWalletId} Created for {mCode}");
            }
            return sess;
        }
        IModel Ch_MMOrder;
        void SetUpMarketMakingOrderListner()
        {
            var isClose = (Ch_MMOrder == null || Ch_MMOrder.IsClosed);
            if (!isClose) return;

            Ch_MMOrder = Ctn_CreateModel(Ch_MMOrder);
            Ch_MMOrder.ExchangeDeclare(Ex_MMOrder, ExchangeType.Fanout);
            Ch_MMOrder.ExchangeDeclare(Ex_MMOrderMAPIOffice, ExchangeType.Direct);
            Ch_MMOrder.ExchangeBind(Ex_MMOrderMAPIOffice, Ex_MMOrder, "");
            var QRes = Ch_MMOrder.QueueDeclare(queue: $"{Ex_MMOrderMAPIOffice}_Qu", durable: true, exclusive: false, autoDelete: false, arguments: null);

            Ch_MMOrder.QueueBind(QRes.QueueName, Ex_MMOrderMAPIOffice, string.Empty);

            var consumer = new EventingBasicConsumer(Ch_MMOrder);
            OrderManager om = null;



            consumer.Received += (s, e) =>
            {
                Console2.WriteLine_DarkYellow($"{RegistryToken!.AppId}|{T}:MMOrder Info Received");
                var bdy = e.Body.ToArray();
                var dt = e.DeliveryTag;
                var msg = Encoding.UTF8.GetString(bdy);
                var data = JsonSerializer.Deserialize<mMMOrder>(msg);
                if (data != null)
                {
                    try
                    {
                        //Place MM Order via Order Manager Channel Only
                        om ??= GetOrderManager();
                        om.TryBuildAndPlace_MMOrder(data, GetMMSession(data.MarketCode));

                        Console2.WriteLine_DarkYellow($"{T}: MMorder in Market:{data.MarketCode} @{data.Price} for {data.Volume} at..{DateTime.UtcNow}");
                    }
                    catch (Exception ex)
                    {
                        Console2.WriteLine_RED($"ERROR: failed to process MMOrder due to error:{ex.GetDeepMsg()}");
                    }

                }
            };
            Ch_MMOrder.BasicConsume(QRes.QueueName, autoAck: true, consumer: consumer);
            Console2.WriteLine_DarkYellow($"{RegistryToken!.AppId}: SetUp Market Making Order Set up is done at..{DateTime.UtcNow}");
        }
        //------------------
        IModel Ch_OrderAck;
        void SetUpOrderAcknowledgmentListner()
        {
            var isClose = (Ch_OrderAck == null || Ch_OrderAck.IsClosed);
            if (!isClose) return;

            Ch_OrderAck = Ctn_CreateModel(Ch_OrderAck);
            Ch_OrderAck.ExchangeDeclare(Ex_OrderAck, ExchangeType.Fanout);
            Ch_OrderAck.ExchangeDeclare(Ex_OrderAckOffice, ExchangeType.Direct);
            Ch_OrderAck.ExchangeBind(Ex_OrderAckOffice, Ex_OrderAck, "");
            var QRes = Ch_OrderAck.QueueDeclare(queue: $"{Ex_OrderAckOffice}_Qu", durable: true, exclusive: false, autoDelete: false, arguments: null);

            Ch_OrderAck.QueueBind(QRes.QueueName, Ex_OrderAckOffice, string.Empty);

            var consumer = new EventingBasicConsumer(Ch_OrderAck);
            consumer.Received += (s, e) =>
            {
                Console2.WriteLine_DarkYellow($"{RegistryToken!.AppId}|{T}:Order Acknowledgement Received");
                var bdy = e.Body.ToArray();
                var dt = e.DeliveryTag;
                var msg = Encoding.UTF8.GetString(bdy);
                var data = JsonSerializer.Deserialize<OrderAck>(msg);
                if (data != null)
                {
                    //Save to Db
                    SaveToDB(data);
                    if (data.OrderID.StartsWith("MM"))
                        MMSaveToDB(data);

                    //Relay Confirmation to Client
                    NotifyWebClient(data.UserAccountNo, data.mCode, "OrderAck");

                    Console2.WriteLine_DarkYellow($"{T}: Order Ack received for userAccount:{data.UserAccountNo} in Market:{data.mCode} for OrderId:{data.OrderID} at..{DateTime.UtcNow}");
                }
            };
            Ch_OrderAck.BasicConsume(QRes.QueueName, autoAck: true, consumer: consumer);
            Console2.WriteLine_DarkYellow($"{RegistryToken!.AppId}: SetUp Order Acknowledgment Listner Set up is done at..{DateTime.UtcNow}");
        }
        IModel Ch_Trade;
        void SetUpTradeListner()
        {
            var isClose = (Ch_Trade == null || Ch_Trade.IsClosed);
            if (!isClose) return;

            Ch_Trade = Ctn_CreateModel(Ch_Trade);
            Ch_Trade.ExchangeDeclare(Ex_Trades, ExchangeType.Fanout);
            Ch_Trade.ExchangeDeclare(Ex_TradeMAPIOffice, ExchangeType.Direct);
            Ch_Trade.ExchangeBind(Ex_TradeMAPIOffice, Ex_Trades, "");
            var QRes = Ch_Trade.QueueDeclare(queue: $"{Ex_TradeMAPIOffice}_Qu", durable: true, exclusive: false, autoDelete: false, arguments: null);

            Ch_Trade.QueueBind(QRes.QueueName, Ex_TradeMAPIOffice, string.Empty);

            var consumer = new EventingBasicConsumer(Ch_Trade);
            consumer.Received += (s, e) =>
            {
                Console2.WriteLine_DarkYellow($"{RegistryToken!.AppId}|{T}:Trade Info Received");
                var bdy = e.Body.ToArray();
                var dt = e.DeliveryTag;
                var msg = Encoding.UTF8.GetString(bdy);
                var data = JsonSerializer.Deserialize<smTrade>(msg);
                if (data != null)
                {
                    var wm = GetWalletManager();
                    //Save Trade in OrderBank & Create Token Transactions in Wallet
                    wm.TradePayOut(data);
                    //Relay Confirmation to Client
                    RelayNotificationToClient(data);
                    //}
                    Console2.WriteLine_DarkYellow($"{T}: Trade in Market:{data.MarketCode} @{data.TradePrice} for BuyInternalId:{data.BuyInternalId} and SellInternalId:{data.SellInternalId} at..{DateTime.UtcNow}");
                }
            };
            Ch_Trade.BasicConsume(QRes.QueueName, autoAck: true, consumer: consumer);
            Console2.WriteLine_DarkYellow($"{RegistryToken!.AppId}: SetUp Trade Listner Set up is done at..{DateTime.UtcNow}");
        }

        IModel Ch_OrderaUpdate;
        Mutex updateM = new Mutex();
        void SetUpOrderUpdateListner()
        {
            var isClose = (Ch_OrderaUpdate == null || Ch_OrderaUpdate.IsClosed);
            if (!isClose) return;
            updateM.WaitOne();

            Ch_OrderaUpdate = Ctn_CreateModel(Ch_OrderaUpdate);
            Ch_OrderaUpdate.ExchangeDeclare(Ex_OrderUpdate, ExchangeType.Fanout);
            Ch_OrderaUpdate.ExchangeDeclare(Ex_OrderUpdateMAPIOffice, ExchangeType.Direct);
            Ch_OrderaUpdate.ExchangeBind(Ex_OrderUpdateMAPIOffice, Ex_OrderUpdate, "");
            var QRes = Ch_OrderaUpdate.QueueDeclare(queue: $"{Ex_OrderUpdateMAPIOffice}_Qu", durable: true, exclusive: false, autoDelete: false, arguments: null);

            Ch_OrderaUpdate.QueueBind(QRes.QueueName, Ex_OrderUpdateMAPIOffice, string.Empty);

            var consumer = new EventingBasicConsumer(Ch_OrderaUpdate);
            consumer.Received += (s, e) =>
            {
                Console2.WriteLine_DarkYellow($"{RegistryToken!.AppId}|{T}:Order Update Received");
                var bdy = e.Body.ToArray();
                var dt = e.DeliveryTag;
                var msg = Encoding.UTF8.GetString(bdy);
                var data = JsonSerializer.Deserialize<eProcessedOrder>(msg);
                if (data != null)
                {
                    //Save to Db
                    var isDone = SaveToDB(data) != null;
                    //Save to MM DB if Required
                    if (data.OrderID.StartsWith("MM"))
                        SaveToMMOrderDB(data);
                    if (isDone)
                    {
                        var wm = GetWalletManager();
                        //Create Token Transactions in Wallet
                        wm.OrderCancelTokenRefunds(data);
                        //Relay Confirmation to Client
                        NotifyWebClient(data.UserAccountNo, data.MarketCode, "OrderUpdate");
                    }
                    Console2.WriteLine_DarkYellow($"{T}: Order cancel in Market:{data.MarketCode} @{(data.OriginalVolume - data.ProcessedVolume)} for OrderSide:{data.OrderSide} at..{DateTime.UtcNow}");
                }
            };
            Ch_OrderaUpdate.BasicConsume(QRes.QueueName, autoAck: true, consumer: consumer);
            Console2.WriteLine_DarkYellow($"{RegistryToken!.AppId}: SetUp Order Update Listner Set up is done at..{DateTime.UtcNow}");
            updateM.ReleaseMutex();
        }

        //----------------------
        private void EnsureMarketAdminPlugIn()
        {
            /* - Plug-In Market.Admin Exchange for Listening Events
             * - Build List of Running Markets/Instances
             * - Update List As and When Events Update
             */
            if (Ch_AdminListner is null || Ch_AdminListner.IsClosed)
                Ch_AdminListner = Get_Ch_AdminListner();

        }
        private IModel Get_Ch_AdminListner()
        {
            lock (ListenAdmin_Lock)
            {
                Ch_AdminListner = Ctn_CreateModel(Ch_AdminListner);
                var QRes = Ch_AdminListner.QueueDeclare(queue: "", durable: true, exclusive: true, autoDelete: false, arguments: null);
                Ch_AdminListner.QueueBind(QRes.QueueName, Ex_ADMIN, string.Empty);

                var consumer = new EventingBasicConsumer(Ch_AdminListner);
                consumer.Received += MarketADMIN_Listener;
                Ch_AdminListner.BasicConsume(QRes.QueueName, autoAck: false, consumer: consumer);
            }

            return Ch_AdminListner;
        }

        private void MarketADMIN_Listener(object? sender, BasicDeliverEventArgs e)
        {
            var bdy = e.Body.ToArray();
            var dt = e.DeliveryTag;
            var msg = Encoding.UTF8.GetString(bdy);
            try
            {
                var data = JsonSerializer.Deserialize<SettlementAdminPackage>(msg);
                VerifyAndActionAdminPackage(data);
                Ch_AdminListner!.BasicAck(deliveryTag: dt, false);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private void AddOrUpdateActive(SettlementAdminPackage pkg)
        {
            var m = MarketList.FirstOrDefault(x => x.MarketCode == pkg.MarketCode && x.Role == SettlementSrvRole.Active);
            if (m is null)
            {
                m = new MarketInstances();
                m.MarketCode = pkg.MarketCode;
                m.AppId = pkg.SenderAppId;
                MarketList.Add(m);
            }
            if (m.InstanceId != pkg.SenderInstancId)
            {
                LogEvent($"{m.MarketCode} has Active Settlement Services Changed from AppId:{m.AppId} with InstanceId:{m.InstanceId} to AppId:{pkg.SenderAppId} with InstanceId:{pkg.SenderAppId}");
            }
            m.isActive = true;
            m.MarketCode = pkg.MarketCode;
            m.Role = SettlementSrvRole.Active;
            m.InstanceId = pkg.SenderInstancId;
            m.AppId = pkg.SenderAppId;
            m.LastPingOn = pkg.SentAt;
        }
        private void AddOrUpdateToShutDown(SettlementAdminPackage pkg)
        {
            var m = MarketList.FirstOrDefault(x => x.InstanceId == pkg.SenderInstancId);
            if (m is null)
            {
                LogError($"No Markjet With Code:{pkg.MarketCode} and InstanceId:{pkg.SenderInstancId} Exist in Market proxy to Record Shutdown");
            }

            m.isActive = false;
            m.LastPingOn = pkg.SentAt;
        }

        private void AddOrUpdatePassive(SettlementAdminPackage pkg)
        {
            var m = MarketList!.FirstOrDefault(x => x.MarketCode == pkg.MarketCode && x.Role == SettlementSrvRole.Passive);
            if (m is null)
            {
                m = new MarketInstances();
                MarketList.Add(m);
            }
            if (m.InstanceId != pkg.SenderInstancId)
            {
                LogEvent($"{m.MarketCode} has Passive Settlement Services Changed from AppId:{m.AppId} with InstanceId:{m.InstanceId} to AppId:{pkg.SenderAppId} with InstanceId:{pkg.SenderAppId}");
            }
            m.isActive = true;
            m.MarketCode = pkg.MarketCode;
            m.Role = SettlementSrvRole.Passive;
            m.InstanceId = pkg.SenderInstancId;
            m.AppId = pkg.SenderAppId;
            m.LastPingOn = pkg.SentAt;
        }
        private void VerifyAndActionAdminPackage(SettlementAdminPackage pkg)
        {
            //LogEvent($"{RegistryToken!.AppId} Received market Admin Notification:{pkg!.SenderAppId} for Event{pkg.Event.ToString()} ..at {DateTime.UtcNow.Ticks}");
            switch (pkg.Event)
            {
                case SettlementAdminEvent.WorkingAsActive:
                    AddOrUpdateActive(pkg);
                    break;
                case SettlementAdminEvent.WorkingAsPassive:
                    AddOrUpdatePassive(pkg);
                    break;

                case SettlementAdminEvent.ShuttingDown:
                    AddOrUpdateToShutDown(pkg);
                    break;
                default:
                    break;
            }

        }
        #endregion

        #region Managers And Support Functions
        static long i = 0;
        private static long Next { get { return i++; } }
        private MarketManager GetMarketManager()
        {
            var result = new MarketManager();
            result.dbctx = GetDbctx();
            return result;
        }
        private WalletManager GetWalletManager()
        {
            var result = new WalletManager();
            result.dbctx = GetDbctx();
            return result;
        }
        private UserManager GetUserManager()
        {
            var result = new UserManager();
            result.dbctx = GetDbctx();
            return result;
        }
        private OrderManager GetOrderManager()
        {
            var result = new OrderManager();
            result.dbctx = GetDbctx();
            return result;
        }
        private static ApiAppContext GetDbctx()
        {
            var o = new DbContextOptionsBuilder<ApiAppContext>();
            o = o.UseSqlServer(ConfigEx.Config.GetConnectionString("ApiDBContext"));
            return new ApiAppContext(o.Options);
        }
        private OrderAppContext Orderctx(string mCode)
        {
            try
            {
                var o = new DbContextOptionsBuilder<OrderAppContext>();
                var str = ConfigEx.Config.GetConnectionString($"OrderAppContext");
                str = str!.Replace("<template>", $"OrdBank{mCode}");
                o = o.UseSqlServer(str);
                var ctx = new OrderAppContext(o.Options);
                ctx.Database.SetConnectionString(str);
                ctx.Database.EnsureCreated();
                return ctx;
            }
            catch (Exception ex)
            {
                SrvPlugIn.LogErrorG($"{AppConfigBase.RegistryToken!.AppId}- in Market:{mCode}|{T} in Connection caused error in Order SQL Connection/Object {ex.GetDeepMsg()}");
            }
            return null;
        }
        private OrderAppContext MMOrderctx(string mCode)
        {
            try
            {
                var o = new DbContextOptionsBuilder<OrderAppContext>();
                var str = ConfigEx.Config.GetConnectionString($"MMAppContext"); var po = ConfigEx.Config.GetSection("OrderPostFix").Value;
                str = str!.Replace("<template>", $"MMOrdBank{mCode}{po}");
                o = o.UseSqlServer(str);
                var ctx = new OrderAppContext(o.Options);
                ctx.Database.SetConnectionString(str);
                ctx.Database.EnsureCreated();
                return ctx;
            }
            catch (Exception ex)
            {
                SrvPlugIn.LogErrorG($"{AppConfigBase.RegistryToken!.AppId}- in MMOrderCtx Market:{mCode}|{T} in Connection caused error in Order SQL Connection/Object {ex.GetDeepMsg()}");
            }
            return null;
        }
        #endregion

    }
    public class RateStructuer
    {
        public string TimeStamp { get; set; }
        public int RateCounter { get; set; }
    }

    public class PriceIndex
    {
        public string mCode { get; set; }
        public string Base { get; set; }
        public string Quote { get; set; }
        public string QuoteCurrency { get; set; }
        public double Price { get; set; }
    }
}
