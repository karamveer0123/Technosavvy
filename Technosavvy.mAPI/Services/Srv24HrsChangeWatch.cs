using NavExM.Int.Maintenance.APIs.Static;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
namespace NavExM.Int.Maintenance.APIs.Services
{
    //It will Receive updates from NavC Service and update Local Static Collection of Tokens
    //for Serving the Clients with Token Price in base Valuation Currency -->USDT
    internal class Srv24HrsChangeWatch : AppConfigBase
    {
        public string Ex_1Min { get => $"NavExM.Market.Trades.1Min"; }

        public string Ex_24Hrs { get => $"NavExM.Market.hrChange"; }
   
        static ConcurrentDictionary<string,PeriodicChange> pChanges = new ConcurrentDictionary<string,PeriodicChange>();
   
        public static List<PeriodicChange> GetAll24HrChange()
        {
            return pChanges.Values.ToList();
        }
        public static PeriodicChange GetAll24HrChangeOfMarket(string mCode)
        {
            return pChanges.FirstOrDefault(x => x.Key == mCode).Value;
        }
        public static PeriodicChange GetAll24HrChangeOfToken(string mCode)
        {
            return pChanges.FirstOrDefault(x => x.Value.mCode.StartsWith(mCode)).Value;
        }
        bool isDisplay;
        protected override async Task DoStart()
        {
            if (RegistryToken is null) return;
            ConName = ".24HrsChange";
            Tokens24HrsChangeListner();
            if(!isDisplay)
            {
                LogDebug($"{RegistryToken!.AppId} 24Hrs Change Listner initialized");
                isDisplay = true;
            }
            Task.Delay(2000).Wait();
        }

        #region Tokens 24Hrs Change Listener
        IModel? ch_24HrsChange;

        public void Tokens24HrsChangeListner()
        {
            var isClose = (ch_24HrsChange == null || ch_24HrsChange.IsClosed);
            if (!isClose) return;

            ch_24HrsChange = Ctn_CreateModel(ch_24HrsChange);
            ch_24HrsChange.ExchangeDeclare(Ex_24Hrs, ExchangeType.Fanout);

            var QRes = ch_24HrsChange.QueueDeclare(queue: "", durable: false, exclusive: true, autoDelete: true, arguments: null);

            ch_24HrsChange.QueueBind(QRes.QueueName, Ex_24Hrs, string.Empty);

            var consumer = new EventingBasicConsumer(ch_24HrsChange);
            consumer.Received += (s, e) =>
            {
                LogDebug($"{RegistryToken!.AppId}Token 24Hrs Listener has Received an Update");
                var bdy = e.Body.ToArray();
                var dt = e.DeliveryTag;
                var msg = Encoding.UTF8.GetString(bdy);
                var data = JsonSerializer.Deserialize<PeriodicChange>(msg);
                if (data != null)
                {
                    LogDebug($"{RegistryToken!.AppId} Token:{data.mCode} 24HrPrice:{data.PricePercentChange24Hr} 24HrVolumn:{data.VolumnChange24Hr} 24HrPoly:{data.Poly24Hr.Count}");
                   pChanges.AddOrUpdate(data.mCode, data, (k, v) => data);
                }
            };
            ch_24HrsChange.BasicConsume(QRes.QueueName, autoAck: true, consumer: consumer);
        }
        #endregion
        private ApiAppContext GetDbctx()
        {
            var o = new DbContextOptionsBuilder<ApiAppContext>();
            o = o.UseSqlServer(ConfigEx.Config.GetConnectionString("ApiDBContext"));
            return new ApiAppContext(o.Options);
        }
    }
}
