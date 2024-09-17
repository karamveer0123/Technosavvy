using Microsoft.AspNetCore.Mvc;
using TechnoApp.Ext.Web.UI.Service;
using NuGet.Protocol;

namespace TechnoApp.Ext.Web.UI.Controllers;

[DenyAccessInPreBeta]
[EnsureCompliantCountry]
public class TradeController : Controller
{
    private readonly ILogger<TradeController> _logger;
    private IOptions<SmtpConfig> _smtp;
    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;

    public TradeController(ILogger<TradeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;
        _smtp = smtp;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);

    }
    // GET: MarketController
    public async Task<ActionResult> Index()
    {
        if (!Request.QueryString.HasValue)
            Response.Redirect($"{Request.PathBase}?cat=trade&t=TechnoSavvy&q=usdt");

        var tm = await GetTradeManager();
        var vm = vmFactory.GetvmMarketTrade(appSessionManager);
        vm = await tm.GetvmMarketTrade(vm);
        if (vm._SessionHash.IsNOT_NullorEmpty() && vm.TradeOrder != null)
        {
            return View(tm.vName, vm);
        }
        else
        {
            var v = tm.vName.IsNullOrEmpty() ? appSessionManager.mySession.vName : tm.vName;
            return View(v, (vmBaseTrade)vm);
        }
    }
    public IActionResult SwitchView(string name)
    {
        name = name.ToLower();
        if (name == "classic" || name == "advance" || name == "fullscreen")
        {
            appSessionManager.mySession.vName = name;
            return Json("True");
        }
        appSessionManager.mySession.vName = "Advance";
        return Json("False");
    }

    public async Task<ActionResult> Index1(string Market)
    {
        var vm = vmFactory.GetvmMarketTrade(appSessionManager);
        return View(vm);
    }
    public async Task<ActionResult> Index2(string Market)
    {
        var vm = vmFactory.GetvmMarketTrade(appSessionManager);
        return View(vm);
    }
    public async Task<ActionResult> Index3(string Market)
    {
        var vm = vmFactory.GetvmMarketTrade(appSessionManager);
        return View(vm);
    }
    [ActionName("index-register")]
    public async Task<ActionResult> IndexReg(string Market)
    {
        var vm = vmFactory.GetvmMarketTrade(appSessionManager);
        return View(vm);
    }
    //Cancel Order
    [AfterProfile]
    public async Task<IActionResult> CancelMyOrder(string mCode, string id)
    {
        var wm = await GetMarketManager();
        var ord = await wm.CancelMyOrder(id, mCode);
        return Json(ord.ToJson());
        // return PartialView("_OpenOrders", ord);

    }
    //Open Orders
    [AfterProfile]
    public async Task<IActionResult> GetMyOpenOrders(string mCode)
    {
        //For Open Order Page with Search Filters..ToDo

        var wm = await GetWalletManager();
        var ord = await wm.GetMyOpenOrders(mCode);
        return PartialView("_OpenOrders", ord);

    }
    internal async Task<List<vmMarketMyTrade>> GetMyTrades(string mCode)
    {
        throw new NotImplementedException();
    }
    internal async Task<List<vmMarketMyTrade>> GetOrderHistory()
    {
        throw new NotImplementedException();
    }
    public async Task<IActionResult> EnsureValidSession()
    {
        await appSessionManager.ExtSession.LoadSession();
        if (!appSessionManager.ExtSession.IsValid)
            return RedirectToAction("Login", "Login");

        return null;
    }
    public async Task<bool> IsValidSession()
    {
        await appSessionManager.ExtSession.LoadSession();
        return appSessionManager.ExtSession.IsValid;

    }
    internal async Task<SettingsManager> GetSettingsManager()
    {
        await EnsureValidSession();

        var Mgr = new SettingsManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        Mgr._DataProtector = _protector;
        return Mgr;
    }
    internal async Task<KYCManager> GetKYCManager()
    {
        await EnsureValidSession();

        var Mgr = new KYCManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        Mgr._DataProtector = _protector;
        return Mgr;
    }
    internal async Task<TradeManager> GetTradeManager()
    {
        await EnsureValidSession();

        var Mgr = new TradeManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        Mgr._DataProtector = _protector;
        return Mgr;
    }
    internal async Task<MarketManager> GetMarketManager()
    {
        await EnsureValidSession();

        var Mgr = new MarketManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        Mgr._DataProtector = _protector;
        return Mgr;
    }
    internal async Task<WalletManager> GetWalletManager()
    {
        await EnsureValidSession();

        var Mgr = new WalletManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        Mgr._DataProtector = _protector;
        return Mgr;
    }

}
