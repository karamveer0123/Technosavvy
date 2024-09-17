using NavExM.Int.Maintenance.APIs.Static;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace NavExM.Int.Maintenance.APIs.Services
{
    //It will Receive updates from Watch and update Local STatic object for Serving the Clients with Currency Price
    internal class SrvCurrencyWatch : AppConfigBase
    {
        static WatchResult wResult = new WatchResult();
        static string BaseToken { get { return ConfigEx.Config.GetSection("BaseToken").Value; } }
        public string Ex_FIATS { get => $"NavExM.Market.Fiats.Price"; }
        public static List<string> GetAllCurrenciesName()
        {
            return wResult.Rates.Keys.ToList();
        }
        public static List<TokenPrice> GetAllCurrencies()
        {
            return wResult.Rates.Values.ToList();
        }
        public static TokenPrice GetCurrency(string code)
        {
            return wResult.Rates.FirstOrDefault(x => x.Key.ToLower() == code.ToLower()).Value;
        }
        public static List<TokenPrice> GetCurrencies(List<string> names)
        {
            var retval = new List<TokenPrice>();
            foreach (string name in names)
            {
                retval.Add(GetCurrency(name));
            }
            return retval;
        }
        bool isDisplay;

        protected override async Task DoStart()
        {
            if (RegistryToken is null) return;

            ConName = $".FiatPrice";
            // wResult = DeleteMeDefault();
            FiatPriceListner();
            if (!isDisplay)
            {
                LogDebug($"{RegistryToken!.AppId} Currency Value initialized with 'FiatPriceListner'");
                isDisplay = true;
            }
            Task.Delay(2000).Wait();
        }
        #region Fiat Price Listener
        IModel? Ch_Fiat;
        public void FiatPriceListner()
        {
            var isClose = (Ch_Fiat == null || Ch_Fiat.IsClosed);
            if (!isClose) return;

            Ch_Fiat = Ctn_CreateModel(Ch_Fiat);
            Ch_Fiat.ExchangeDeclare(Ex_FIATS, ExchangeType.Fanout);

            var QRes = Ch_Fiat.QueueDeclare(queue: "", durable: false, exclusive: true, autoDelete: true, arguments: null);

            Ch_Fiat.QueueBind(QRes.QueueName, Ex_FIATS, string.Empty);

            var consumer = new EventingBasicConsumer(Ch_Fiat);
            consumer.Received += (s, e) =>
            {
                var bdy = e.Body.ToArray();
                var dt = e.DeliveryTag;
                var msg = Encoding.UTF8.GetString(bdy);
                var data = JsonSerializer.Deserialize<TokenPrice>(msg);
                if (data != null)
                {
                var t = $"{data.TokenName}{BaseToken}".ToLower();
                    wResult.Rates.AddOrUpdate(t, data, (k, v) => data);
                }
            };
            Ch_Fiat.BasicConsume(QRes.QueueName, autoAck: true, consumer: consumer);
        }
        #endregion

    }
    public class WatchResult
    {
        public DateTime LastUpdatedOn { get; set; } = DateTime.Now;
        public ConcurrentDictionary<string, TokenPrice> Rates { get; set; } = new ConcurrentDictionary<string, TokenPrice>();
    }
    public class MarketWatchResult
    {
        public DateTime LastUpdatedOn { get; set; } = DateTime.Now;
        public ConcurrentDictionary<string, PriceIndex> Rates { get; set; } = new ConcurrentDictionary<string, PriceIndex>();
    }
    public class TokenPrice
    {
        public string TokenName { get; set; }
        public double Price { get; set; }
    }
    public class PeriodicChange
    {
        public long Time { get; set; }
        public string mCode { get; set; }
        public double CurrentPrice { get; set; }
        public double Volumn24Hr { get; set; }
        public double Volumn8Hr { get; set; }
        public double PricePercentChange24Hr { get; set; }
        public double VolumnChange24Hr { get; set; }
        public double PricePercentChange8Hr { get; set; }
        public double VolumnChange8Hr { get; set; }
        public double CashBack24Hr { get; set; }
        public double CashBack8Hr { get; set; }
        public double CashBackYTD { get; set; }
        public List<int> Poly8Hr { get; set; }
        public List<int> Poly24Hr { get; set; }

    }
}
