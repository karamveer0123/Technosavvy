using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TechnoApp.Ext.Web.UI.Controllers
{
    //[Route("_academy")]
    [EnsureCompliantCountry]
    [DenyAccessInPreBeta]

    public class AcademyController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IOptions<SmtpConfig> _smtp;
        private IConfiguration _configuration;
        private HttpContext _context;
        private IHttpContextAccessor _accessor;
        IDataProtector _protector;
        AppSessionManager appSessionManager = null;
        private bool optional;

        public AcademyController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
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
            //if ((await DoesHaveAnActiveSession()) == false)
            //    return RedirectToAction("Login", "Login");

            var DM = await GetWalletManager();
            var vm = vmFactory.GetvmWalletHome(appSessionManager);
            return View("index0", vm);
        }
        [HttpGet]
        [ActionName("detail")]
        public async Task<IActionResult> detail()
        {
            if (Request.Query.Any())
            {
                var vm = vmFactory.GetvmAcademyDetail(appSessionManager);
                Request.Query.TryGetValue("tab", out var tab1);
                Request.Query.TryGetValue("name", out var name1);
                vm.Tab = tab1;
                vm.Topic = name1;
                return View("index2", vm);
            }
            else
            {
                var vm = vmFactory.GetvmAcademyDetail(appSessionManager);
                return View("index2", vm);
            }
        }

        private async Task<bool> DoesHaveAnActiveSession()
        {
            await appSessionManager.ExtSession.LoadSession();
            return appSessionManager.ExtSession.IsValid;
        }
        internal async Task<WalletManager> GetWalletManager()
        {
            var Mgr = new WalletManager();
            Mgr._configuration = _configuration;
            Mgr._http = _accessor.HttpContext;
            Mgr._appSessionManager = appSessionManager;
            return Mgr;
            //}
        }
        private string GetValue(string key, bool NOToptional = false, string optionalVal = "")
        {
            if (Request.Query.TryGetValue(key, out var retval))
            {
                if (NOToptional)
                {
                    throw new ApplicationException("invalid parameter for request");
                }
                else return optionalVal;
            }
            return retval.ToString().ToLower();
        }
    }
}
