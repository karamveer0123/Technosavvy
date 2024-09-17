using Microsoft.AspNetCore.Mvc;

namespace TechnoApp.Ext.Web.UI.Controllers;

[Route("referral-program")]
[Route("referralprogram")]
[Route("referral")]
[DenyAccessInPreBeta]
    [EnsureCompliantCountry]
public class ReferralProgramController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IOptions<SmtpConfig> _smtp;
    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;

    public ReferralProgramController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;
        _smtp = smtp;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);
    }
 // [AfterProfile] //comment by kavita
    public async Task<IActionResult> Index()
    {
       //var vm = await vmFactory.GetvmBase(appSessionManager);
         var vm = vmFactory.GetvmRewardCenter(appSessionManager);
         var rm = GetRewardManager();
         vm = await rm.LoadvmRewardCenter(vm);
        //var vm = vmFactory.GetvmRewardCenter(appSessionManager);
        return View("Index", vm);
    }
    [AfterProfile]
    [HttpGet("Reward")]
    public async Task<IActionResult> Reward()
    {
        var vm =  vmFactory.GetvmRewardCenter(appSessionManager);
        var rm = GetRewardManager();
        vm=await rm.LoadvmRewardCenter(vm);
        return View("Reward", vm);
    }
    internal RewardManager GetRewardManager()
    {
        var Mgr = new RewardManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        appSessionManager.ExtSession.LoadSession().GetAwaiter().GetResult();
        return Mgr;
    }
}