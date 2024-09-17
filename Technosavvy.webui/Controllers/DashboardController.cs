using Microsoft.AspNetCore.Mvc;
using TechnoApp.Ext.Web.UI.Manager;
using System.Data;

namespace TechnoApp.Ext.Web.UI.Controllers
{
    [AfterProfile]
[DenyAccessInPreBeta]
    [EnsureCompliantCountry]
    public class DashboardController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IOptions<SmtpConfig> _smtp;
        private IConfiguration _configuration;
        private HttpContext _context;
        private IHttpContextAccessor _accessor;
        IDataProtector _protector;
        AppSessionManager appSessionManager = null;
        ITempDataDictionaryFactory _TDDF;
        public DashboardController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
        {
            _logger = logger;
            _smtp = smtp;
            _configuration = configuration;
            _accessor = accessor;
            _TDDF = tddf;
            _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
            appSessionManager = new AppSessionManager(accessor.HttpContext);

        }
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var DM =  GetDashboardManager();
            var vm = vmFactory.GetvmDashboard(appSessionManager);
            vm.WSummery = await DM.GetMyWalletSummery();
            vm.WTrans = await DM.GetTransactionSummery();
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View(vmFactory.GetvmDashboard(appSessionManager));
        }

     
        internal  DashboardManager GetDashboardManager()
        {
            var Mgr = new DashboardManager();
            Mgr._configuration = _configuration;
            Mgr._http = _accessor.HttpContext;
            Mgr._appSessionManager = appSessionManager;
            appSessionManager.ExtSession.LoadSession().GetAwaiter().GetResult();
            return Mgr;
        }
    }
}
