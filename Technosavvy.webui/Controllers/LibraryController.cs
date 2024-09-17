using Microsoft.AspNetCore.Mvc;

namespace TechnoApp.Ext.Web.UI.Controllers
{
    [EnsureCompliantCountry]
    public class LibraryController : Controller
    {
        private readonly ILogger<SettingsController> _logger;
        private IOptions<SmtpConfig> _smtp;
        private IConfiguration _configuration;
        private HttpContext _context;
        private IHttpContextAccessor _accessor;
        IDataProtector _protector;
        AppSessionManager appSessionManager = null;
        public LibraryController(ILogger<SettingsController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
        {

            _logger = logger;
            _smtp = smtp;
            _configuration = configuration;
            _accessor = accessor;
            _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
            appSessionManager = new AppSessionManager(accessor.HttpContext);
        }

        [HttpGet]
        [DenyAccessInPreBeta]
        public async Task<IActionResult> Index()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("medialibrary", vm);
        }
        [HttpGet]
        [ActionName("FAQ")]
        public async Task<IActionResult> Faq()
        {
            var vm = vmFactory.GetvmFaqDisplay(appSessionManager);
            var cm = GetUserManager();
            vm = await cm.GetAllApprovedFAQsToDisplay(vm);
            return View("index4", vm);

        }
        internal ContentManager GetUserManager()
        {
            var Mgr = new ContentManager();
            Mgr._configuration = _configuration;
            Mgr._http = _accessor.HttpContext;
            Mgr._appSessionManager = appSessionManager;
            return Mgr;
        }

    }
}
