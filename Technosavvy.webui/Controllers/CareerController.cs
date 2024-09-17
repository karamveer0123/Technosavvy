using Microsoft.AspNetCore.Mvc;

namespace TechnoApp.Ext.Web.UI.Controllers;

[DenyAccessInPreBeta]
[EnsureCompliantCountry]
[AfterLogIn]
public class CareerController : Controller
{
    private readonly ILogger<SettingsController> _logger;
    private IOptions<SmtpConfig> _smtp;
    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;
    public CareerController(ILogger<SettingsController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {

        _logger = logger;
        _smtp = smtp;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);
    }
    [HttpGet]
    public async Task<IActionResult> Index(string RefCode = "")
    {
        var vm = await vmFactory.GetvmJD(appSessionManager);
        var cm = new CareerManager();
        vm.JD = await cm.GetApprovedJDByRef(RefCode);
        return View("Index", vm);
    }
    [HttpPost]
    public async Task<IActionResult> Acknowledge(vmJD vm)
    {
        try
        {
            var cm = GetCareerManager();
            var result = await cm.UpdateJDUser(vm.JD.id);
            if (result)
                return Json("");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Acknowledge Error:{ex.GetDeepMsg()}");
        }
        return Json("<div> System is Busy, Please come back Later.</div>");
    }
    internal CareerManager GetCareerManager()
    {
        var Mgr = new CareerManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        appSessionManager.ExtSession.LoadSession().GetAwaiter().GetResult();
        return Mgr;
    }
}
/*
* .com/Blog/An-Introduction-to-TechnoApp-Trading-Ecosystem-and-TechnoSavvy-Token
.com/Blog/Best-Alt-Coin-2022
.com/Blog/Crypto Trading is Now Rewarding
.com/Blog/Earn-High-Returns-by-Staking-Crypto
.com/blog/Encouraging-GenZ-Investors-to-Trade
.com/Blog/Good-Time-to-Invest-in-Crypto-is-Now
.com/Blog/How-to-Earn-Crypto-for-Free?
.com/Blog/Is-It-Safe-ToTrade-Cryptocurrency
.com/Blog/Most-Rewarding-Crypto-Trading
.com/Blog/Next-genration-crypto
.com/Blog/Reasons-to-invest
.com/Blog/The-New-Trends-In-Crypto
.com/Blog/Things-to-consider-in-before-investing-in-TechnoSavvyToken
.com/Blog/Top-ten-reasons-to-buy-TechnoSavvy
.com/Blog/Is-it-safe-to-trade
.com/Blog/What-Is-Value-Variance-Inflationary-Token
.com/Blog/What-makes-TechnoSavvy-a-gold-coin
.com/Blog/why-you-should-invest-in-TechnoSavvy
*/