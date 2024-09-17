using NuGet.Protocol;
using Microsoft.AspNetCore.Mvc;
using TechnoApp.Ext.Web.UI.Model;

namespace TechnoApp.Ext.Web.UI.Controllers;

[AfterProfile]
[DenyAccessInPreBeta]
[EnsureCompliantCountry]
public class AddbookController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IOptions<SmtpConfig> _smtp;
    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;

    public AddbookController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;
        _smtp = smtp;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);

    }
    [HttpGet]
    public IActionResult GetMyNetWhiteList(Guid networkId)
    {
        var vm = new List<mNetAddr>() { new mNetAddr() { Address = "0xDDSA88763S0878s0877",Network="ETH", Name = "ABC" }, new mNetAddr() { Address = "0xDASA88763S0878s0873", Network = "ETH", Name = "ABC2" } };
        return Json(vm.ToJson());
    }
}
