using Microsoft.AspNetCore.Mvc;

namespace TechnoApp.Ext.Web.UI.Controllers;

public class t5Controller:Controller
{
    private readonly ILogger<HomeController> _logger;
    private IOptions<SmtpConfig> _smtp;
    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;
    public t5Controller(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;
        _smtp = smtp;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);
        //should it working this timne

    }
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View(vm);
    }
    [HttpGet]
    [ActionName("about-btc")]
    public async Task<IActionResult> aboutbtc()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("aboutbtc", vm);
    }
    [HttpGet]
    [ActionName("detail")]
    public async Task<IActionResult> academydetail()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("academydetail", vm);
    }
    [HttpGet]
    [ActionName("airdrop-program")]
    public async Task<IActionResult> airdropprogram()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("airdropprogram",vm);
    }
    [HttpGet]
    [ActionName("appendix")]
    public async Task<IActionResult> appendix()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("appendix", vm);
    }
    [HttpGet]
    [ActionName("cashbackmain")]
    public async Task<IActionResult> cashbackmain()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("cashbackmain", vm);
    }
    [HttpGet]
    [ActionName("change-password")]
    public async Task<IActionResult> changepassword()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("changepassword", vm);
    }
    [HttpGet]
    [ActionName("faq")]
    public async Task<IActionResult> faq()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("faq", vm);
    }
    [HttpGet]
    [ActionName("fees-charges")]
    public async Task<IActionResult> feescharges()
    {
        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("feescharges", vm);
    }
}
