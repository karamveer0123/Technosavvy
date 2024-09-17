using Microsoft.AspNetCore.Mvc;
using TechnoApp.Ext.Web.UI.Service;
using NuGet.Protocol;

namespace TechnoApp.Ext.Web.UI.Controllers;

public class TokenController : Controller
{
    private readonly ILogger<TokenController> _logger;
    private IOptions<SmtpConfig> _smtp;
    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;

    public TokenController(ILogger<TokenController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;
        _smtp = smtp;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);

    }
    // GET: MarketController
    public async Task<ActionResult> Index(string tName)
    {
        if (tName.IsNullOrEmpty())
            RedirectToAction("index", "about-TechnoSavvy");


        var vm = vmFactory.GetvmTokenDetails(appSessionManager);
        vm.TokenCode = tName;
        Console2.WriteLine_RED("TODO:Naveen, Token Details Page Data Implementation is still Pending");
        return View("index", vm);//This page should be cached for 24 hour

    }
    public async Task<ActionResult> details(Guid id)
    {
        var tm = new TokenManager();
        var details = tm.GetActiveToken(id);
        return Json(details.ToJson());
    }
    public async Task<ActionResult> detailsOf(string tName)
    {
        var tm = new TokenManager();
        var details = tm.GetActiveTokenOfCode(tName);
        return Json(details.ToJson());
    }
    public async Task<ActionResult> NetworkFees()
    {
        var tm = new TokenManager();
        var details = tm.GetAllTokensNetWorkFee();
        return Json(details.ToJson());
    }
}