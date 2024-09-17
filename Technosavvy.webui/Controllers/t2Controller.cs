using Microsoft.AspNetCore.Mvc;

namespace TechnoApp.Ext.Web.UI.Controllers
{
    [EnsureCompliantCountry]
    public class t2Controller : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IOptions<SmtpConfig> _smtp;
        private IConfiguration _configuration;
        private HttpContext _context;
        private IHttpContextAccessor _accessor;
        IDataProtector _protector;
        AppSessionManager appSessionManager = null;
        public t2Controller(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
        {
            _logger = logger;
            _smtp = smtp;
            _configuration = configuration;
            _accessor = accessor;
            _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
            appSessionManager = new AppSessionManager(accessor.HttpContext);
            //should it working this timne

        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        [ActionName("an-introduction-to-TechnoApp-blog")]
        public async Task<IActionResult> anintroductiontoTechnoAppblog()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("anintroductiontoTechnoAppblog", vm);
        }
        [HttpGet]
        [ActionName("appendix")]
        public async Task<IActionResult> appendix()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("appendix", vm);
        }
        [HttpGet]
        [ActionName("best-altcoin-blog")]
        public async Task<IActionResult> bestaltcoinblog()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("bestaltcoinblog", vm);
        }
        [HttpGet]
        [ActionName("cryptotradingblog")]
        public async Task<IActionResult> cryptotradingblog()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("cryptotradingblog", vm);
        }
        [HttpGet]
        [ActionName("disclaimer")]
        public async Task<IActionResult> disclaimer()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("disclaimer", vm);
        }
        [HttpGet]
        [ActionName("earn-high-returns-blog")]
        public async Task<IActionResult> earnhighreturnsblog()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("earnhighreturnsblog", vm);
        }
        [HttpGet]
        [ActionName("encouraginggenzinvestorsblog")]
        public async Task<IActionResult> encouraginggenzinvestorsblog()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("encouraginggenzinvestorsblog", vm);
        }
        [HttpGet]
        [ActionName("forget-password")]
        public async Task<IActionResult> forgetpassword()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("forgetpassword", vm);
        }
        [HttpGet]
        [ActionName("goodtimetoinvest")]
        public async Task<IActionResult> goodtimetoinvest()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("goodtimetoinvest", vm);
        }
        [HttpGet]
        [ActionName("howtoearncryptoblog")]
        public async Task<IActionResult> howtoearncryptoblog()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("howtoearncryptoblog", vm);
        }
        [HttpGet]
        [ActionName("in-aml-policy")]
        public async Task<IActionResult> inamlpolicy()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("inamlpolicy", vm);
        }
        [HttpGet]
        [ActionName("aml-policy")]
        public async Task<IActionResult> inamlpolicyDefault()
        {
            //ToDo:Naveen, Decide the origin country and send view accordingly
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("inamlpolicy", vm);
        }
        [HttpGet]
        [ActionName("in-privacy-policy")]
        public async Task<IActionResult> inprivacypolicy()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("inprivacypolicy", vm);
        }
        [HttpGet]
        [ActionName("privacy-policy")]
        public async Task<IActionResult> inprivacypolicyDefault()
        {
            //ToDo:Naveen, Decide the origin country and send view accordingly
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("inprivacypolicy", vm);
        }
        [HttpGet]
        [ActionName("in-terms-of-use")]
        public async Task<IActionResult> intermsofuse()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("intermsofuse", vm);
        }
        [HttpGet]
        [ActionName("terms-of-use")]
        public async Task<IActionResult> intermsofuseDefault()
        {
            //ToDo:Naveen, Decide the origin country and send view accordingly
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("intermsofuse", vm);
        }
        [HttpGet]
        [ActionName("Index-us")]
        public async Task<IActionResult> Indexus()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("Indexus", vm);
        }
        [HttpGet]
        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("Index", vm);
        }
        [HttpGet]
        [ActionName("isitsafetotrade")]
        public async Task<IActionResult> isitsafetotrade()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("isitsafetotrade", vm);
        }
        [HttpGet]
        [ActionName("licences")]
        public async Task<IActionResult> licences()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("licences", vm);
        }
        [HttpGet]
        [ActionName("login-beta-release")]
        public async Task<IActionResult> loginbetarelease()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("loginbetarelease", vm);
        }
        [HttpGet]
        [ActionName("login-otp")]
        public async Task<IActionResult> loginotp()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("loginotp", vm);
        }
        [HttpGet]
        [ActionName("login")]
        public async Task<IActionResult> login()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("login", vm);
        }
        [HttpGet]
        [ActionName("media-library")]
        public async Task<IActionResult> medialibrary()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("medialibrary", vm);
        }
        [HttpGet]
        [ActionName("mostrewardingcryptoblog")]
        public async Task<IActionResult> mostrewardingcryptoblog()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("mostrewardingcryptoblog", vm);
        }
        [HttpGet]
        [ActionName("TechnoSavvyannouncesitsicolaunchblog")]
        public async Task<IActionResult> TechnoSavvyannouncesitsicolaunchblog()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("TechnoSavvyannouncesitsicolaunchblog", vm);
        }
        [HttpGet]
        [ActionName("new-password")]
        public async Task<IActionResult> newpassword()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("newpassword", vm);
        }
        [HttpGet]
        [ActionName("nextgencryptoblog")]
        public async Task<IActionResult> nextgencryptoblog()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("nextgencryptoblog", vm);
        }
        [HttpGet]
        [ActionName("reasons-to-invest-blog")]
        public async Task<IActionResult> reasonstoinvestblog()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("reasonstoinvestblog", vm);
        }
        [HttpGet]
        [ActionName("referral-policy")]
        public async Task<IActionResult> referralpolicy()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("referralpolicy", vm);
        }
        [HttpGet]
        [ActionName("referral-program")]
        public async Task<IActionResult> referralprogram()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("referralprogram", vm);
        }
        [HttpGet]
        [ActionName("risk-disclosure")]
        public async Task<IActionResult> riskdisclosure()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("riskdisclosure", vm);
        }
        [HttpGet]
        [ActionName("sign-up")]
        public async Task<IActionResult> signup()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("signup", vm);
        }
        [HttpGet]
        [ActionName("signup-otp")]
        public async Task<IActionResult> signupotp()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("signupotp", vm);
        }
        [HttpGet]
        [ActionName("signup-password")]
        public async Task<IActionResult> signuppassword()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("signuppassword", vm);
        }
        [HttpGet]
        [ActionName("the-new-trends-blog")]
        public async Task<IActionResult> thenewtrendsblog()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("thenewtrendsblog", vm);
        }
        [HttpGet]
        [ActionName("things-to-consider-blog")]
        public async Task<IActionResult> thingstoconsiderblog()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("thingstoconsiderblog", vm);
        }
        [HttpGet]
        [ActionName("top-10-reasons-blog")]
        public async Task<IActionResult> top10reasonsblog()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("top10reasonsblog", vm);
        }
        [HttpGet]
        [ActionName("trading-rules-us")]
        public async Task<IActionResult> tradingrulesus()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("tradingrulesus", vm);
        }
        [HttpGet]
        [ActionName("wallet")]
        public async Task<IActionResult> wallet()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("wallet", vm);
        }
        [HttpGet]
        [ActionName("what-is-value-blog")]
        public async Task<IActionResult> whatisvalueblog()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("whatisvalueblog", vm);
        }
        [HttpGet]
        [ActionName("what-makes-TechnoSavvy-gold-blog")]
        public async Task<IActionResult> whatmakesTechnoSavvygoldblog()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("whatmakesTechnoSavvygoldblog", vm);
        }
        [HttpGet]
        [ActionName("whatisTechnoSavvyblog")]
        public async Task<IActionResult> whatisTechnoSavvyblog()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("whatisTechnoSavvyblog", vm);
        }
        [HttpGet]
        [ActionName("why-should-you-invest-blog")]
        public async Task<IActionResult> whyshouldyouinvestblog()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("whyshouldyouinvestblog", vm);
        }
    }
}
