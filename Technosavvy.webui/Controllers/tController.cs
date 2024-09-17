using Microsoft.AspNetCore.Mvc;

namespace TechnoApp.Ext.Web.UI.Controllers
{
    [EnsureCompliantCountry]
    public class tController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IOptions<SmtpConfig> _smtp;
        private IConfiguration _configuration;
        private HttpContext _context;
        private IHttpContextAccessor _accessor;
        IDataProtector _protector;
        AppSessionManager appSessionManager = null;
        public tController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
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
        public async Task<IActionResult> Index()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);

        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> academy()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);

        }
        [HttpGet]
        public async Task<IActionResult> accountstatement()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> buyconfirmation()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> buycryptohistory()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> buycrypto()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> buymatchprocess()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> buyorderplaced()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> buyorderprocess()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> buyorderprogress()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> buypayment()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> buyproofsubmit()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> buytransactioncancell()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> coverthistory()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> dashboard()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> earnhistory()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> earnwallet()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> forgetpassword()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> fundingwallet()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Index1()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> kycpersonalverificationupload()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> kycpersonalverification()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> kycuploadsefie()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> loginbetarelease()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> loginotp()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> login()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> marketdetail()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> marketnewlisting()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> market()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> newpassword()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> openorders()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> personalidentity()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> securityverificationemailgoogle()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> securityverification()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> securityverificationgoogle()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> security()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> sellbankstatement()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> sellchooseaccount()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> sellconfirmation()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> sellcrypto()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> sellmatchprocess()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> sellorderprocess()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> sellorderprogressrecevied()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> sellorderprogress()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> sellproofsubmit()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> selltrasctionproof()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> signup()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> signupotp()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> signuppassword()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> spotwallet()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> staking()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> transaction()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> userprofile()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> verificationqrcode()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> wallet()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
    }
    public class t1Controller : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IOptions<SmtpConfig> _smtp;
        private IConfiguration _configuration;
        private HttpContext _context;
        private IHttpContextAccessor _accessor;
        IDataProtector _protector;
        AppSessionManager appSessionManager = null;
        public t1Controller(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
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
        public async Task<IActionResult> academytrade()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> academy()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> anintroductiontoTechnoAppblog()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> bestaltcoinblog()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> earnhighreturnsblog()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> faq()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> inamlpolicy()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> inprivacypolicy()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> intermsofuse()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Index1()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> license()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> medialibrary()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> ncamlpolicy()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> ncprivacypolicy()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> nctermscondition()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> reasonstoinvestblog()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> referralpolicy()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> referralprogram()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> thenewtrendsblog()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> thingstoconsiderblog()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> top10reasonsblog()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> usamlpolicy()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> usprivacypolicy()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> ustermsconditions()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> whatisvalueblog()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> whatmakesTechnoSavvygoldblog()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> whyshouldyouinvestblog()
        {

            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
    }
}
