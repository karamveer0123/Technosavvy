using Microsoft.AspNetCore.Mvc;
namespace TechnoApp.Ext.Web.UI.Controllers
{
    public class UserRefController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IOptions<SmtpConfig> _smtp;
        private IConfiguration _configuration;
        private HttpContext _context;
        private IHttpContextAccessor _accessor;
        IDataProtector _protector;
        AppSessionManager appSessionManager = null;

        public UserRefController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
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
            var o = appSessionManager.mySession.oURL;
            appSessionManager.mySession.oURL = string.Empty;
            if (o.IsNOT_NullorEmpty())
                return Redirect(o);
            else
                return RedirectToAction("index", "wallet");
        }
    }
}
