using NavExM.Int.Maintenance.APIs.Static;
using NuGet.Protocol;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
namespace NavExM.Int.Maintenance.APIs.Services;
//It will Receive updates from NavC Service and update Local Static Collection of Tokens
//for Serving the Clients with Token Price in base Valuation Currency -->USDT
internal class SrvCoinWatch : AppConfigBase
{
    public string Ex_TOKENS { get => $"NavExM.Market.Tokens.Price"; }
    public string Ex_Market { get => $"NavExM.Market.Price"; }
    public string Ex_TOKENSExternalPrice { get => $"NavExM.Market.Tokens.ExternalPrice"; }
    public string Ex_1Min { get => $"NavExM.Market.Trades.1Min"; }
    static string BaseToken { get { return ConfigEx.Config.GetSection("BaseToken").Value; } }
    static MarketWatchResult mResult = new MarketWatchResult();
    static WatchResult wResult = new WatchResult();
    static WatchResult wExResult = new WatchResult();

    //-------- External Coin Rates/values
    public static List<string> GetAllExternalCoinsName()
    {
        return wExResult.Rates.Keys.ToList();
    }
    public static List<TokenPrice> GetAllExternalCoins()
    {
        return wExResult.Rates.Values.ToList();
    }
    public static List<TokenPrice> GetExternalCoins(List<string> names)
    {
        var retval = new List<TokenPrice>();
        foreach (string name in names)
        {
            retval.Add(GetExternalCoin(name));
        }
        return retval;
    }
    public static TokenPrice GetExternalCoin(string code)
    {
        return wExResult.Rates.FirstOrDefault(x => x.Key.ToLower() == code.ToLower()).Value;
    }
    //-------- Exchange Coin Rates/values
    public static List<string> GetAllCoinsName()
    {
        return wResult.Rates.Keys.ToList();
    }
    public static List<TokenPrice> GetAllCoins()
    {
        return wResult.Rates.Values.ToList();
    }
    public static List<TokenPrice> GetCoins(List<string> names)
    {
        var retval = new List<TokenPrice>();
        foreach (string name in names)
        {
            retval.Add(GetCoin(name));
        }
        return retval;
    }
    public static TokenPrice GetCoin(string code)
    {
        if (code.IsNOT_NullorEmpty())
            code = code.ToLower();

        var ret = wResult.Rates.FirstOrDefault(x => x.Key.ToLower() == code.ToLower()).Value;
        if (code != "usdt")
            code = code + "usdt";
        ret = wResult.Rates.FirstOrDefault(x => x.Key.ToLower() == code.ToLower()).Value;
        return ret;
    }
    bool isSetUp = false;
    Mutex setLock = new Mutex();
    bool isDisplay;
    protected override async Task DoStart()
    {
        if (RegistryToken is null) return;
        ConName = ".TokenPrice";
        SetUpBaseTokenMarketList();
        TokensPriceListner();
        TokensExternalPriceListner();
        MarketPriceListner();
        if (!isDisplay)
        {
            LogDebug($"{RegistryToken!.AppId} Coins Value initialized with 'TokensPriceListner'");
            isDisplay = true;
        }
        Task.Delay(2000).Wait();
    }
    private void SetUpBaseTokenMarketList()//USDT Pairs
    {
        setLock.WaitOne();
        try
        {
            if (isSetUp == false)
            {
                var mm = GetMarketManager();
                var lst = mm.GetAllActiveMarketPairOfQuote(BaseToken, false);
                LogError($"Coin Service rceived:{lst.Count} Pair of BaseQuote:{BaseToken}");
                lst.ForEach(x =>
                {
                    var tp = new TokenPrice { TokenName = x.BaseToken!.Code, Price = double.NaN };
                    wResult.Rates.AddOrUpdate(x.CodeName.ToLower(), tp, (k, v) => tp);
                    isSetUp = true;
                });
                var tp = new TokenPrice
                {
                    TokenName = "usdt",
                    Price = 1
                };
                wResult.Rates.AddOrUpdate(tp.TokenName, tp, (k, v) => tp);
            }
        }
        catch (Exception ex)
        {

            LogError(ex);
        }

        setLock.ReleaseMutex();
    }

    #region Tokens Price Listener
    IModel? Ch_Tokens;
    IModel? Ch_mCode;
    IModel? Ch_TokensExternalPrice;
    //BaseToken{USDT} market Price Listener to maintain USDT value of each Token
    public void TokensPriceListner()
    {
        var isClose = (Ch_Tokens == null || Ch_Tokens.IsClosed);
        if (!isClose) return;

        Ch_Tokens = Ctn_CreateModel(Ch_Tokens);
        Ch_Tokens.ExchangeDeclare(Ex_TOKENS, ExchangeType.Fanout);

        var QRes = Ch_Tokens.QueueDeclare(queue: "", durable: false, exclusive: true, autoDelete: true, arguments: null);

        Ch_Tokens.QueueBind(QRes.QueueName, Ex_TOKENS, string.Empty);
        var bToken = BaseToken;
        var consumer = new EventingBasicConsumer(Ch_Tokens);
        consumer.Received += (s, e) =>
        {
            LogDebug($"{RegistryToken!.AppId}Token Price Listener has Received a Price Update");
            var bdy = e.Body.ToArray();
            var dt = e.DeliveryTag;
            var msg = Encoding.UTF8.GetString(bdy);
            var data = JsonSerializer.Deserialize<TokenPrice>(msg);
            if (data != null)
            {
                LogDebug($"{RegistryToken!.AppId}|Info|{T}|Token:{data.TokenName} Price:{data.Price} Update");
                var t = $"{data.TokenName}{bToken}".ToLower();
                wResult.Rates.AddOrUpdate(t, data, (k, v) => data);
            }
        };
        Ch_Tokens.BasicConsume(QRes.QueueName, autoAck: true, consumer: consumer);
        Console2.WriteLine_DarkYellow($"{RegistryToken!.AppId}Token Price Listener has Started..");

    }
    public void TokensExternalPriceListner()
    {
        var isClose = (Ch_TokensExternalPrice == null || Ch_TokensExternalPrice.IsClosed);
        if (!isClose) return;

        Ch_TokensExternalPrice = Ctn_CreateModel(Ch_TokensExternalPrice);
        Ch_TokensExternalPrice.ExchangeDeclare(Ex_TOKENSExternalPrice, ExchangeType.Fanout);

        var QRes = Ch_TokensExternalPrice.QueueDeclare(queue: "", durable: false, exclusive: true, autoDelete: true, arguments: null);

        Ch_TokensExternalPrice.QueueBind(QRes.QueueName, Ex_TOKENSExternalPrice, string.Empty);
        var bToken = BaseToken;
        var consumer = new EventingBasicConsumer(Ch_TokensExternalPrice);
        DateTime dtlast = DateTime.MinValue;
        consumer.Received += (s, e) =>
        {
            // LogDebug($"{RegistryToken!.AppId}Token External Price Listener has Received a Price Update");
            // Console2.WriteLine_Green($"{RegistryToken!.AppId}Token External Price Listener has Received a Price Update");
            var bdy = e.Body.ToArray();
            var dt = e.DeliveryTag;
            var msg = Encoding.UTF8.GetString(bdy);
            var data = JsonSerializer.Deserialize<TokenPrice>(msg);
            if (data != null)
            {
                if (dtlast.Minute != DateTime.UtcNow.Minute)
                {
                    LogDebug($"{RegistryToken!.AppId}Info|{T}|Token:{data.TokenName} External Price:{data.Price} Update");
                    dtlast = DateTime.UtcNow;
                }
                var t = $"{data.TokenName}{bToken}".ToLower();
                wExResult.Rates.AddOrUpdate(t, data, (k, v) => data);
            }
        };
        Ch_TokensExternalPrice.BasicConsume(QRes.QueueName, autoAck: true, consumer: consumer);
        Console2.WriteLine_DarkYellow($"{RegistryToken!.AppId}Token External Price Listener has Started..");
    }
    public void MarketPriceListner()
    {
        var isClose = (Ch_mCode == null || Ch_mCode.IsClosed);
        if (!isClose) return;

        Ch_mCode = Ctn_CreateModel(Ch_mCode);
        Ch_mCode.ExchangeDeclare(Ex_Market, ExchangeType.Fanout);

        var QRes = Ch_mCode.QueueDeclare(queue: "", durable: false, exclusive: true, autoDelete: true, arguments: null);

        Ch_mCode.QueueBind(QRes.QueueName, Ex_Market, string.Empty);
        var bToken = BaseToken;
        DateTime dtlast = DateTime.MinValue;
        var consumer = new EventingBasicConsumer(Ch_mCode);
        consumer.Received += (s, e) =>
        {
            var bdy = e.Body.ToArray();
            var dt = e.DeliveryTag;
            var msg = Encoding.UTF8.GetString(bdy);
            var data = JsonSerializer.Deserialize<PriceIndex>(msg);
            if (data != null)
            {
                if (data.Price > 0)
                {
                    var t = new TokenPrice { Price = data.Price, TokenName = data.mCode };
                    if (data.mCode.ToUpper().EndsWith("USDT"))
                    {
                        var tt = new TokenPrice
                        {
                            TokenName = data.mCode.ToUpper().Replace("USDT", ""),
                            Price = data.Price
                        };
                        wResult.Rates.AddOrUpdate(data.mCode.ToLower(), tt, (k, v) => tt);
                    }

                    wResult.Rates.AddOrUpdate(data.mCode.ToLower(), t, (k, v) => t);

                    mResult.Rates.AddOrUpdate(data.mCode.ToLower(), data, (k, v) => data);

                    wResult.LastUpdatedOn = DateTime.Now;
                    mResult.LastUpdatedOn = DateTime.Now;
                    if (dtlast.Minute != DateTime.UtcNow.Minute)
                    {
                        dtlast = DateTime.UtcNow;
                        wResult.Rates.ToList().ForEach(x =>
                        {
                            Console2.WriteLine_White($"Info|{RegistryToken!.AppId} Market:{x.Key} Price:{x.Value.Price} Updated.. at:{DateTime.UtcNow}");

                        });
                    }
                }
            }
        };
        Ch_mCode.BasicConsume(QRes.QueueName, autoAck: true, consumer: consumer);
        Console2.WriteLine_DarkYellow($"{RegistryToken!.AppId} Market Price Listener has Started..");
    }

    #endregion

    private MarketManager GetMarketManager()
    {
        var result = new MarketManager();
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
