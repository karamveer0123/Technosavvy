using Microsoft.AspNetCore.Mvc;

namespace TechnoApp.Ext.Web.UI.Controllers;

[DenyAccessInPreBeta]
[EnsureCompliantCountry]
public class BlogController : Controller
{
    private readonly ILogger<SettingsController> _logger;
    private IOptions<SmtpConfig> _smtp;
    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;
    public BlogController(ILogger<SettingsController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        
        _logger = logger;
        _smtp = smtp;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("1anintroductiontoTechnoAppblog", vm);
    }

    [HttpGet]
    [ActionName("what-is-TechnoSavvy")]
    public async Task<IActionResult> Blog1()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("whatisTechnoSavvyblog", vm);
    }
    [HttpGet]
    [ActionName("what-is-value")]
    public async Task<IActionResult> Blog2()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("8whatisvalueblog", vm);
    }
    [HttpGet]
    [ActionName("earn-high-returns-staking-crypto")]
    public async Task<IActionResult> Blog3()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("earnhighreturnsstakingblog", vm);
    }
    [HttpGet]
    [ActionName("Most-Rewarding-Crypto-Trading-Experience")]
    public async Task<IActionResult> Blog4()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("mostrewardingcryptotradingblog", vm);
    }
    [HttpGet]
    [ActionName("an-introduction-to-TechnoApp")]
    public async Task<IActionResult> Blog5()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("1anintroductiontoTechnoAppblog", vm);
    }
    [HttpGet]
    [ActionName("fastest-growing-crypto-currency")]
    public async Task<IActionResult> Blog6()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("fastestgrowingcryptocurrencyblog", vm);
    }
    [HttpGet]
    [ActionName("crypto-arbitrage-opportunity")]
    public async Task<IActionResult> Blog7()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("cryptoarbitrageopportunityblog", vm);
    }
    [HttpGet]
    [ActionName("most-rewarding-crypto-exchange")]
    public async Task<IActionResult> Blog8()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("mostrewardingcryptoexchangeblog", vm);
    }
    [HttpGet]
    [ActionName("how-trade-crypto-currency")]
    public async Task<IActionResult> Blog9()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("howtradecryptocurrencyblog", vm);
    }
    [HttpGet]
    [ActionName("best-crypto-currency-exchange")]
    public async Task<IActionResult> Blog10()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("bestcryptocurrencyexchangeblog", vm);
    }       
    [HttpGet]
    [ActionName("exploring-exciting-utility")]
    public async Task<IActionResult> Blog11()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("exploringexcitingutilityblog", vm);
    }
    [HttpGet]
    [ActionName("crypto-trading-fees")]
    public async Task<IActionResult> Blog12()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("cryptotradingfeesblog", vm);
    }
    [HttpGet]
    [ActionName("how-preserve-your-capital-inflation")]
    public async Task<IActionResult> Blog13()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("preserveyourcapitalinflationblog", vm);
    }
    [HttpGet]
    [ActionName("how-TechnoSavvy-encapsulates-traded-pair")]
    public async Task<IActionResult> Blog14()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("howTechnoSavvyencapsulatestradedpairblog ", vm);
    }    
    [HttpGet]
    [ActionName("TechnoSavvy-floor-price-fixed")]
    public async Task<IActionResult> Blog15()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("TechnoSavvyfloorpricefixedblog", vm);
    }
    [HttpGet]
    [ActionName("TechnoSavvy-velocity-real-TechnoApp")]
    public async Task<IActionResult> Blog16()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("TechnoSavvyvelocityrealTechnoAppblog", vm);
    }  
    [HttpGet]
    [ActionName("types-stakers")]
    public async Task<IActionResult> Blog17()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("typesstakers", vm);
    }


    [HttpGet]
    [ActionName("endless-opportunities")]
    public async Task<IActionResult> Blog18()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("endlessopportunitiesblog", vm);
    }

    [HttpGet]
    [ActionName("how-become-community-member")]
    public async Task<IActionResult> Blog19()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("howbecomecommunitymemberblog", vm);
    }

    [HttpGet]
    [ActionName("most-rewarding-crypto")]
    public async Task<IActionResult> Blog20()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("mostrewardingcryptoblog", vm);
    }
    [HttpGet]
    [ActionName("most-rewarding-crypto-trading")]
    public async Task<IActionResult> Blog21()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("mostrewardingcryptotradingblog", vm);
    }


    public async Task<IActionResult> EnsureValidSession()
    {
        await appSessionManager.ExtSession.LoadSession();
        if (!appSessionManager.ExtSession.IsValid)
            Response.Redirect("/Login/Login",false);

        return null;
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