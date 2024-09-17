using Microsoft.AspNetCore.Mvc;

namespace TechnoApp.Ext.Web.UI.Controllers;

[Route("press-release")]
[Route("pressrelease")]
[DenyAccessInPreBeta]
    [EnsureCompliantCountry]
public class pressreleaseController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IOptions<SmtpConfig> _smtp;
    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;

    public pressreleaseController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;
        _smtp = smtp;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);

    }
    public async Task<IActionResult> index()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("Index", vm);
    }
    //[HttpGet]
    //public async Task<IActionResult> index()
    //{
    //    var vm = await vmFactory.GetvmBase(appSessionManager);
    //    return View("Index", vm);
    //}

   

}
