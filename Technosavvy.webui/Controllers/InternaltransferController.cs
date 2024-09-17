using NuGet.Protocol;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace TechnoApp.Ext.Web.UI.Controllers;
[AfterProfile]
[DenyAccessInPreBeta]
[EnsureCompliantCountry]
public class InternaltransferController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IOptions<SmtpConfig> _smtp;
    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;

    public InternaltransferController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;
        _smtp = smtp;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);

    }
    //add by kavita
    [HttpGet]
    public async Task<IActionResult> index()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("index", vm);
    }
    [HttpGet]
    public async Task<IActionResult> internaltransferconfirmation()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("internaltransferconfirmation", vm);
    }
    [HttpGet]
    public async Task<IActionResult> internaltransfersummary()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("internaltransfersummary", vm);
    }
    [HttpGet]
    public async Task<IActionResult> internaltransferverification()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("internaltransferverification", vm);
    }
}