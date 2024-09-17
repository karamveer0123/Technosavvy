using Microsoft.AspNetCore.Mvc;

namespace TechnoApp.Ext.Web.UI.Controllers
{
    [EnsureCompliantCountry]
    public class PolicyController : Controller
    {
        private readonly ILogger<SettingsController> _logger;
        private IOptions<SmtpConfig> _smtp;
        private IConfiguration _configuration;
        private HttpContext _context;
        private IHttpContextAccessor _accessor;
        IDataProtector _protector;
        AppSessionManager appSessionManager = null;
        public PolicyController(ILogger<SettingsController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
        {

            _logger = logger;
            _smtp = smtp;
            _configuration = configuration;
            _accessor = accessor;
            _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
            appSessionManager = new AppSessionManager(accessor.HttpContext);
        }

        [HttpGet]
        [ActionName("Privacy-Policy")]
        public async Task<IActionResult> privacy()
        {
            var vm = await vmFactory.GetvmBase(appSessionManager);
            switch (vm._Country.ToLower())
            {
                case "in":
                    return View("inprivacypolicy", vm);
                case "us":
                    return View("usprivacypolicy", vm);
                case "au":
                    return View("auprivacypolicy", vm);//ToDo:Australia Policy View
                default:
                    return View("ncprivacypolicy", vm);
            }
        }
        [HttpGet]
        [ActionName("Referral-policy")]
        public async Task<IActionResult> Index2()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("referralpolicy", vm);
        }
        [HttpGet]
        [ActionName("Trading-rules")]
        public async Task<IActionResult> TradingRules()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("tradingrulesus", vm);
        }
        [HttpGet]
        [ActionName("aml-policy")]
        public async Task<IActionResult> ncamlpolicy()
        {
            var vm = await vmFactory.GetvmBase(appSessionManager);
            switch (vm._Country.ToLower())
            {
                case "in":
                    return View("inamlpolicy", vm);
                case "us":
                    return View("usamlpolicy", vm);
                case "au":
                    return View("auamlpolicy", vm);//ToDo:Australia Policy View
                default:
                    return View("ncamlpolicy", vm);
            }
        }
        [HttpGet]
        [ActionName("Licences")]
        public async Task<IActionResult> Licences()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            switch (vm._Country.ToLower())
            {
                case "us":
                    return View("usLicences", vm);
                case "au":
                    return View("auLicences", vm);//ToDo:Australia Policy View
                default:
                    return View("ncLicences", vm);
            }
        }
        [HttpGet]
        [ActionName("Risk-Disclosure")]
        public async Task<IActionResult> RiskDisclosure()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("riskdisclosure", vm);
        }
        [HttpGet]
        [ActionName("Disclaimer")]
        public async Task<IActionResult> Disclaimer()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("Disclaimer", vm);
        }

        //add by kavita
        [HttpGet]
        [ActionName("cookiespreference")]
        public async Task<IActionResult> cookiespreference()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("cookiespreference", vm);
        }
        //add by kavita
        [HttpGet]
        [ActionName("stakingpolicy")]
        public async Task<IActionResult> stakingpolicy()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("stakingpolicy", vm);
        }

        [HttpGet]
        [ActionName("terms-of-use")]
        public async Task<IActionResult> Index11()
        {
            var vm = await vmFactory.GetvmBase(appSessionManager);
            switch (vm._Country.ToLower())
            {
                case "in":
                    return View("intermsofuse", vm);
                case "us":
                    return View("ustermsofuse", vm);
                case "au":
                    return View("autermsofuse", vm);//ToDo:Australia Policy View
                default:
                    return View("nctermsofuse", vm);

            }
        }
    }
}
