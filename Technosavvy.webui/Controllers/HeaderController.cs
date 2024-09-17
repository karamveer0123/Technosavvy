using Microsoft.AspNetCore.Mvc;

namespace TechnoApp.Ext.Web.UI.Controllers
{
[DenyAccessInPreBeta]
    [EnsureCompliantCountry]
    public class HeaderController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IOptions<SmtpConfig> _smtp;
        private IConfiguration _configuration;
        private HttpContext _context;
        private IHttpContextAccessor _accessor;
        IDataProtector _protector;
        AppSessionManager appSessionManager = null;
        public HeaderController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
        {
            _logger = logger;
            _smtp = smtp;
            _configuration = configuration;
            _accessor = accessor;
            _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
            appSessionManager = new AppSessionManager(accessor.HttpContext);

        }
        public async Task<IActionResult> Header()
        {
            return View(await vmFactory.GetvmBase(appSessionManager));
        }
        public async Task<IActionResult> sHeader()
        {
            return View(await vmFactory.GetvmBase(appSessionManager));
        }
        internal HeaderManager GetUserManager()
        {
            var Mgr = new HeaderManager();
            Mgr._configuration = _configuration;
            Mgr._http = _accessor.HttpContext;
            Mgr._appSessionManager = appSessionManager;
            return Mgr;
        }
    }
}
