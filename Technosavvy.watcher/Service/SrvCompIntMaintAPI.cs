using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using NavExM.Int.Watcher.WatchDog.Core;
namespace NavExM.Int.Watcher.WatchDog.Service
{
    internal class SrvCompStaffWeb : AppConfigBase
    {
        public SrvCompStaffWeb()
        {
            RegName = "Srv-4-StaffWeb";
            AppName = $"NavExM.Int.Staff.Web.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 4; //Staff API is 4
            IsAutoRegEnabled = true;
        }
    }
    //NavExM.Int.Staff.APIs --Authentication,Team Management, Multi Authorization
    internal class SrvCompStaffAPIs : AppConfigBase
    {
        public SrvCompStaffAPIs()
        {
            RegName = "Srv-5-CompStaffAPIs";
            AppName = $"NavExM.Int.Staff.APIs.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 5; //Staff API is 5
            IsAutoRegEnabled = true;
        }
    } //NavExM.Int.Staff.Web.exe --Authentication,Team Management, Multi Authorization
    //NavExM.Int.Watcher.Price
    //NavExM.Int.Watcher.Wallet.exe
    internal class SrvCompWatcherPrice : AppConfigBase
    {
        public SrvCompWatcherPrice()
        {
            RegName = "Srv-7-CompWatcherPrice";
            AppName = $"NavExM.Int.Watcher.Price.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 7; //Watcher.Price is 7
            IsAutoRegEnabled = true;
        }
    }
    //NavExM.Int.Watcher.Wallet
    internal class SrvCompWatcherWallet : AppConfigBase
    {
        public SrvCompWatcherWallet()
        {
            RegName = "Srv-8-CompWatcherWallet";
            AppName = $"NavExM.Int.Watcher.Wallet.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 8; //Watcher.Wallet is 8
            IsAutoRegEnabled = true;
        }
    }
    //NavExM.Int.UM.APIs
    internal class SrvCompUMAPIs : AppConfigBase
    {
        public SrvCompUMAPIs()
        {
            RegName = "Srv-9-CompUMAPIs";
            AppName = $"NavExM.Int.UM.APIs.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 9; //UM.APIs is 9
            IsAutoRegEnabled = true;
        }
    }
    internal class SrvCompIntTradingAPI : AppConfigBase
    {
        public SrvCompIntTradingAPI()
        {
            RegName = "Srv-10-CompIntTradingAPI";
            AppName = $"NavExM.Int.Trading.APIs.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 10; //TradingAPI is 10
            IsAutoRegEnabled = true;
        }
    }
    //"NavExM.Int.Trading.Data.Broker.exe";
    internal class SrvCompDataBroker : AppConfigBase
    {
        public SrvCompDataBroker()
        {
            RegName = "Srv-11-CompDataBroker";
            AppName = $"NavExM.Int.Trading.Data.Broker.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 11;
            IsAutoRegEnabled = true;
        }
    }
    internal class SrvCompIntMaintAPI : AppConfigBase
    {
        public SrvCompIntMaintAPI()
        {
            RegName = "Srv-13-CompIntMaintAPI";
            AppName = $"NavExM.Int.Maintenance.APIs.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 13; //IntMaintAPI is 13
            IsAutoRegEnabled = true;
        }
    }
    internal class SrvCompBroadcastWorker : AppConfigBase
    {
        public SrvCompBroadcastWorker()
        {
            RegName = "Srv-14-CompBroadcastWorker";
            AppName = $"NavExM.Int.Watcher.BroadcastWorker.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 14; //BroadcastWorker is 14
            IsAutoRegEnabled = true;
        }
    }
    //NavExM.Int.Trading.Arbitrage
    internal class SrvCompTradingArbitrage : AppConfigBase
    {
        public SrvCompTradingArbitrage()
        {
            RegName = "Srv-15-CompTradingArbitrage";
            AppName = $"NavExM.Int.Trading.Arbitrage.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 15; //Trading.Arbitrage is 15
            IsAutoRegEnabled = true;
        }
    }
    //NavExM.Int.Trading.Settlement
    internal class SrvCompTradingSettlement : AppConfigBase
    {
        public SrvCompTradingSettlement()
        {
            RegName = "Srv-16-CompTradingSettlement";
            AppName = $"NavExM.Int.Trading.Settlement.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 16; //Trading.Settlement is 16
            IsAutoRegEnabled = true;
        }
    }
    //NavExM.Int.Watcher.Wallet
    internal class SrvWalletWatcher : AppConfigBase
    {
        public SrvWalletWatcher()
        {
            RegName = "Srv-17-WalletWatcher";
            AppName = $"NavExM.Int.Watcher.Wallet.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 17; //WalletWatcher is 17
            IsAutoRegEnabled = true;
        }
    }
    //NavExM.Int.NavC.Price
    internal class SrvCompNavCPrice : AppConfigBase
    {
        public SrvCompNavCPrice()
        {
            RegName = "Srv-18-CompNavCPrice";
            AppName = $"NavExM.Int.NavC.Price.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 18; //NavExM.Int.NavC.Price is 17
            IsAutoRegEnabled = true;
        }
    }
    //NavExM.Ext.Market.Data.Broadcast
    internal class SrvCompDataBroadcast : AppConfigBase
    {
        public SrvCompDataBroadcast()
        {
            RegName = "Srv-19-CompDataBroadcast";
            AppName = $"NavExM.Ext.Market.Data.Broadcast.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 19; //Data.Broadcast is 19
            IsAutoRegEnabled = true;
        }
    }
    //NavExM.Int.Trading.Data.Summary
    internal class SrvCompDataSummary : AppConfigBase
    {
        public SrvCompDataSummary()
        {
            RegName = "Srv-21-CompDataSummary";
            AppName = $"NavExM.Int.Trading.Data.Summary.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 21; //Data.Summary is 21
            IsAutoRegEnabled = true;
        }
    }
    //NavExM.Int.Trading.Cashback
    internal class SrvCompTradingCashback : AppConfigBase
    {
        public SrvCompTradingCashback()
        {
            RegName = "Srv-22-CompTradingCashback";
            AppName = $"NavExM.Int.Trading.Cashback.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 22; //BroadcastWorker is 22
            IsAutoRegEnabled = true;
        }
    }
    internal class SrvCompWDDBLogger : AppConfigBase
    {
        public SrvCompWDDBLogger()
        {
            RegName = "Srv-24-CompWDDBLogger";
            AppName = $"NavExM.Int.Watcher.WatchDog.DBWorker.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 24; //DBWorker is 24
            IsAutoRegEnabled = true;
        }
    }
    //NavExM.Int.Trading.Data.Persistence
    internal class SrvCompDataPersistence : AppConfigBase
    {
        public SrvCompDataPersistence()
        {
            RegName = "Srv-25-CompDataPersistence";
            AppName = $"NavExM.Int.Trading.Data.Persistence.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 25; //Data.Persistence is 25
            IsAutoRegEnabled = true;
        }
    }
    //NavExM.Int.Trading.Data.Audit
    internal class SrvCompDataAudit : AppConfigBase
    {
        public SrvCompDataAudit()
        {
            RegName = "Srv-26-CompDataAudit";
            AppName = $"NavExM.Int.Trading.Data.Audit.exe";
            Ex_NameRegReq = $"{AppName}_RegReq";
            Ex_NameRegRes = $"{AppName}_RegRes";
            ComponentGroupId = 26; //trade Data Audit DB Logger 26
            IsAutoRegEnabled = true;
        }
    }
    
    
}


