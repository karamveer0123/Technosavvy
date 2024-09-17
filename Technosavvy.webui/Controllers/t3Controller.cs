using Microsoft.AspNetCore.Mvc;
using TechnoApp.Ext.Web.UI.Manager;

namespace TechnoApp.Ext.Web.UI.Controllers
{
    public class TestController : Controller
    {
        private IHttpContextAccessor _accessor;
        AppSessionManager appSessionManager = null;
        public TestController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
        {
            _accessor = accessor;
            appSessionManager = new AppSessionManager(accessor.HttpContext);

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("Index",vm);
        }
    }
    public class t3Controller : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IOptions<SmtpConfig> _smtp;
        private IConfiguration _configuration;
        private HttpContext _context;
        private IHttpContextAccessor _accessor;
        IDataProtector _protector;
        AppSessionManager appSessionManager = null;
        public t3Controller(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
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
            return View("List", vm);
        }
        [HttpGet]
        [ActionName("airdrop-program")]
        public async Task<IActionResult> airdropprogram()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("airdropprogram", vm);
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
        [ActionName("fees-charges")]
        public async Task<IActionResult> feescharges()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("feescharges", vm);
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
        [ActionName("in-privacy-policy")]
        public async Task<IActionResult> inprivacypolicy()
        {
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
        [ActionName("license")]
        public async Task<IActionResult> license()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("license", vm);
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
        [ActionName("market-detail")]
        public async Task<IActionResult> marketdetail()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("marketdetail", vm);
        }
        [HttpGet]
        [ActionName("market-favourites")]
        public async Task<IActionResult> marketfavourites()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("marketfavourites", vm);
        }
        [HttpGet]
        [ActionName("market-gainercoins")]
        public async Task<IActionResult> marketgainercoins()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("marketgainercoins", vm);
        }
        [HttpGet]
        [ActionName("market-highlightcoin")]
        public async Task<IActionResult> markethighlightcoin()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("markethighlightcoin", vm);
        }
        [HttpGet]
        [ActionName("market-listings")]
        public async Task<IActionResult> marketlistings()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("marketlistings", vm);
        }
        [HttpGet]
        [ActionName("market-mostpurchase")]
        public async Task<IActionResult> marketmostpurchase()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("marketmostpurchase", vm);
        }
        [HttpGet]
        [ActionName("market-new-listing")]
        public async Task<IActionResult> marketnewlisting()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("marketnewlisting", vm);
        }
        [HttpGet]
        [ActionName("market-topvolume")]
        public async Task<IActionResult> markettopvolume()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("markettopvolume", vm);
        }
        [HttpGet]
        [ActionName("market")]
        public async Task<IActionResult> market()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("market", vm);
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
        [ActionName("nc-aml-policy")]
        public async Task<IActionResult> ncamlpolicy()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("ncamlpolicy", vm);
        }
        [HttpGet]
        [ActionName("nc-privacy-policy")]
        public async Task<IActionResult> ncprivacypolicy()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("ncprivacypolicy", vm);
        }
        [HttpGet]
        [ActionName("nc-terms-condition")]
        public async Task<IActionResult> nctermscondition()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("nctermscondition", vm);
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
        [ActionName("press-release")]
        public async Task<IActionResult> pressrelease()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("pressrelease", vm);
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
        [ActionName("us-aml-policy")]
        public async Task<IActionResult> usamlpolicy()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("usamlpolicy", vm);
        }
        [HttpGet]
        [ActionName("us-privacy-policy")]
        public async Task<IActionResult> usprivacypolicy()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("usprivacypolicy", vm);
        }
        [HttpGet]
        [ActionName("us-terms-conditions")]
        public async Task<IActionResult> ustermsconditions()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View("ustermsconditions", vm);
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
