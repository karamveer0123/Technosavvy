using Microsoft.AspNetCore.Mvc;

namespace TechnoApp.Ext.Web.UI.Controllers
{
    public class t4Controller:Controller
    {

        private readonly ILogger<HomeController> _logger;
        private IOptions<SmtpConfig> _smtp;
        private IConfiguration _configuration;
        private HttpContext _context;
        private IHttpContextAccessor _accessor;
        IDataProtector _protector;
        AppSessionManager appSessionManager = null;
        public t4Controller(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
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
        [ActionName("about-TechnoSavvy")]
        public async Task<IActionResult> aboutTechnoSavvy()
        {
            var vm = vmFactory.GetvmUserLogin(appSessionManager);

            return View("about-TechnoSavvy",vm);
        }
        [HttpGet]
        [ActionName("academy-detail")]
        public async Task<IActionResult> academydetail()
        {
            return View("academy-detail",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("academy")]
        public async Task<IActionResult> academy()
        {
            return View("academy",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("account-activity")]
        public async Task<IActionResult> accountactivity()
        {
            return View("account-activity",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("account-statement")]
        public async Task<IActionResult> accountstatement()
        {
            return View("account-statement",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("account-verify")]
        public async Task<IActionResult> accountverify()
        {
            return View("account-verify",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("airdrop-program")]
        public async Task<IActionResult> airdropprogram()
        {
            return View("airdrop-program",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("an-introduction-to-TechnoApp-blog")]
        public async Task<IActionResult> anintroductiontoTechnoAppblog()
        {
            return View("anintroductiontoTechnoAppblog",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("appendix")]
        public async Task<IActionResult> appendix()
        {
            return View("appendix",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("basic-information")]
        public async Task<IActionResult> basicinformation()
        {
            return View("basicinformation",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("best-altcoin-blog")]
        public async Task<IActionResult> bestaltcoinblog()
        {
            return View("bestaltcoinblog",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("candlestick-touch")]
        public async Task<IActionResult> candlesticktouch()
        {
            return View("candlesticktouch",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("cashback-main")]
        public async Task<IActionResult> cashbackmain()
        {
            return View("cashback-main",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("cashback")]
        public async Task<IActionResult> cashback()
        {
            return View("cashback",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("country-residence")]
        public async Task<IActionResult> countryresidence()
        {
            return View("country-residence",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("covert-history")]
        public async Task<IActionResult> coverthistory()
        {
            return View("covert-history",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("create-personal-account")]
        public async Task<IActionResult> createpersonalaccount()
        {
            return View("create-personal-account",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("cryptotradingblog")]
        public async Task<IActionResult> cryptotradingblog()
        {
            return View("cryptotradingblog",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("dashboard-2")]
        public async Task<IActionResult> dashboard2()
        {
            return View("dashboard-2",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("dashboard-3")]
        public async Task<IActionResult> dashboard3()
        {
            return View("dashboard-3",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("dashboard-notification-detail")]
        public async Task<IActionResult> dashboardnotificationdetail()
        {
            return View("dashboard-notification-detail",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("dashboard-notification")]
        public async Task<IActionResult> dashboardnotification()
        {
            return View("dashboard-notification",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("dashboard")]
        public async Task<IActionResult> dashboard()
        {
            return View("dashboard",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("deposit-accepted")]
        public async Task<IActionResult> depositaccepted()
        {
            return View("deposit-accepted",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("deposit")]
        public async Task<IActionResult> deposit()
        {
            return View("deposit",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("device-management")]
        public async Task<IActionResult> devicemanagement()
        {
            return View("device-management",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("disclaimer")]
        public async Task<IActionResult> disclaimer()
        {
            return View("disclaimer",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("earn-high-returns-blog")]
        public async Task<IActionResult> earnhighreturnsblog()
        {
            return View("earnhighreturnsblog",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("earn-history")]
        public async Task<IActionResult> earnhistory()
        {
            return View("earn-history",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("earn-wallet")]
        public async Task<IActionResult> earnwallet()
        {
            return View("earn-wallet",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("email-verification")]
        public async Task<IActionResult> emailverification()
        {
            return View("email-verification",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("encouraginggenzinvestorsblog")]
        public async Task<IActionResult> encouraginggenzinvestorsblog()
        {
            return View("encouraginggenzinvestorsblog",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("entity-category")]
        public async Task<IActionResult> entitycategory()
        {
            return View("entity-category",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("entity-summary")]
        public async Task<IActionResult> entitysummary()
        {
            return View("entity-summary",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("faq")]
        public async Task<IActionResult> faq()
        {
            return View("faq",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("fees-charges")]
        public async Task<IActionResult> feescharges()
        {
            return View("fees-charges",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("fiat-enablement")]
        public async Task<IActionResult> fiatenablement()
        {
            return View("fiat-enablement",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("forget-password")]
        public async Task<IActionResult> forgetpassword()
        {
            return View("forget-password",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("funding-wallet")]
        public async Task<IActionResult> fundingwallet()
        {
            return View("funding-wallet",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("goodtimetoinvest")]
        public async Task<IActionResult> goodtimetoinvest()
        {
            return View("goodtimetoinvest",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("howtoearncryptoblog")]
        public async Task<IActionResult> howtoearncryptoblog()
        {
            return View("howtoearncryptoblog",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("in-aml-policy")]
        public async Task<IActionResult> inamlpolicy()
        {
            return View("in-aml-policy",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("in-privacy-policy")]
        public async Task<IActionResult> inprivacypolicy()
        {
            return View("in-privacy-policy",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("in-terms-of-use")]
        public async Task<IActionResult> intermsofuse()
        {
            return View("in-terms-of-use",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("Index-india")]
        public async Task<IActionResult> Indexindia()
        {
            return View("Index-india",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("Index-us")]
        public async Task<IActionResult> Indexus()
        {
            return View("Index-us",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            return View("Index",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("index1")]
        public async Task<IActionResult> index1()
        {
            return View("index1",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("index2")]
        public async Task<IActionResult> index2()
        {
            return View("index2",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("index3")]
        public async Task<IActionResult> index3()
        {
            return View("index3",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("institutional-services")]
        public async Task<IActionResult> institutionalservices()
        {
            return View("institutional-services",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("inter-wallet-summary")]
        public async Task<IActionResult> interwalletsummary()
        {
            return View("inter-wallet-summary",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("inter-wallet-transfer")]
        public async Task<IActionResult> interwallettransfer()
        {
            return View("inter-wallet-transfer",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("interwallet-accepted")]
        public async Task<IActionResult> interwalletaccepted()
        {
            return View("interwallet-accepted",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("isitsafetotrade")]
        public async Task<IActionResult> isitsafetotrade()
        {
            return View("isitsafetotrade",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("kyc-appendic")]
        public async Task<IActionResult> kycappendic()
        {
            return View("kyc-appendic",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("kyc-personal-verification-upload")]
        public async Task<IActionResult> kycpersonalverificationupload()
        {
            return View("kyc-personal-verification-upload",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("kyc-personal-verification")]
        public async Task<IActionResult> kycpersonalverification()
        {
            return View("kyc-personal-verification",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("kyc-upload-sefie")]
        public async Task<IActionResult> kycuploadsefie()
        {
            return View("kyc-upload-sefie",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("license")]
        public async Task<IActionResult> license()
        {
            return View("license",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("list-aggrement")]
        public async Task<IActionResult> listaggrement()
        {
            return View("list-aggrement",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("list-page")]
        public async Task<IActionResult> listpage()
        {
            return View("list-page",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("login-beta-release")]
        public async Task<IActionResult> loginbetarelease()
        {
            return View("login-beta-release",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("login-otp")]
        public async Task<IActionResult> loginotp()
        {
            return View("login-otp",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("login")]
        public async Task<IActionResult> login()
        {
            return View("login",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("market-detail")]
        public async Task<IActionResult> marketdetail()
        {
            return View("market-detail",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("market-gainercoins")]
        public async Task<IActionResult> marketgainercoins()
        {
            return View("market-gainercoins",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("market-highlightcoin")]
        public async Task<IActionResult> markethighlightcoin()
        {
            return View("market-highlightcoin",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("market-new-listing")]
        public async Task<IActionResult> marketnewlisting()
        {
            return View("market-new-listing",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("market-topvolume")]
        public async Task<IActionResult> markettopvolume()
        {
            return View("market-topvolume",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("market")]
        public async Task<IActionResult> market()
        {
            return View("market",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("media-library")]
        public async Task<IActionResult> medialibrary()
        {
            return View("media-library",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("mostrewardingcryptoblog")]
        public async Task<IActionResult> mostrewardingcryptoblog()
        {
            return View("mostrewardingcryptoblog",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("TechnoSavvyannouncesitsicolaunchblog")]
        public async Task<IActionResult> TechnoSavvyannouncesitsicolaunchblog()
        {
            return View("TechnoSavvyannouncesitsicolaunchblog",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("nc-aml-policy")]
        public async Task<IActionResult> ncamlpolicy()
        {
            return View("nc-aml-policy",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("nc-privacy-policy")]
        public async Task<IActionResult> ncprivacypolicy()
        {
            return View("nc-privacy-policy",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("nc-terms-condition")]
        public async Task<IActionResult> nctermscondition()
        {
            return View("nc-terms-condition",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("new-password")]
        public async Task<IActionResult> newpassword()
        {
            return View("new-password",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("nextgencryptoblog")]
        public async Task<IActionResult> nextgencryptoblog()
        {
            return View("nextgencryptoblog",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("open-orders")]
        public async Task<IActionResult> openorders()
        {
            return View("open-orders",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("order-history")]
        public async Task<IActionResult> orderhistory()
        {
            return View("order-history",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("p2p-order")]
        public async Task<IActionResult> p2porder()
        {
            return View("p2p-order",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("personal-identity")]
        public async Task<IActionResult> personalidentity()
        {
            return View("personal-identity",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("press-release")]
        public async Task<IActionResult> pressrelease()
        {
            return View("press-release",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("reasons-to-invest-blog")]
        public async Task<IActionResult> reasonstoinvestblog()
        {
            return View("reasonstoinvestblog",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("referral-policy")]
        public async Task<IActionResult> referralpolicy()
        {
            return View("referral-policy",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("referral-program")]
        public async Task<IActionResult> referralprogram()
        {
            return View("referral-program",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("register-business")]
        public async Task<IActionResult> registerbusiness()
        {
            return View("register-business",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("related-parties")]
        public async Task<IActionResult> relatedparties()
        {
            return View("related-parties",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("reward-center")]
        public async Task<IActionResult> rewardcenter()
        {
            return View("reward-center",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("risk-disclosure")]
        public async Task<IActionResult> riskdisclosure()
        {
            return View("risk-disclosure",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("security-verificationgoogle")]
        public async Task<IActionResult> securityverificationgoogle()
        {
            return View("security-verificationgoogle",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("security")]
        public async Task<IActionResult> security()
        {
            return View("security",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("sign-up")]
        public async Task<IActionResult> signup()
        {
            return View("sign-up",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("signup-otp")]
        public async Task<IActionResult> signupotp()
        {
            return View("signup-otp",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("signup-password")]
        public async Task<IActionResult> signuppassword()
        {
            return View("signup-password",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("spot-wallet")]
        public async Task<IActionResult> spotwallet()
        {
            return View("spot-wallet",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("spotopen-orders")]
        public async Task<IActionResult> spotopenorders()
        {
            return View("spotopen-orders",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("the-new-trends-blog")]
        public async Task<IActionResult> thenewtrendsblog()
        {
            return View("thenewtrendsblog",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("things-to-consider-blog")]
        public async Task<IActionResult> thingstoconsiderblog()
        {
            return View("thingstoconsiderblog",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("token-compliance")]
        public async Task<IActionResult> tokencompliance()
        {
            return View("token-compliance",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("token-infomation")]
        public async Task<IActionResult> tokeninfomation()
        {
            return View("token-infomation",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("token-legal")]
        public async Task<IActionResult> tokenlegal()
        {
            return View("token-legal",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("token-legal2")]
        public async Task<IActionResult> tokenlegal2()
        {
            return View("token-legal2",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("token-security")]
        public async Task<IActionResult> tokensecurity()
        {
            return View("token-security",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("token-submit")]
        public async Task<IActionResult> tokensubmit()
        {
            return View("token-submit",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("tokeninfo")]
        public async Task<IActionResult> tokeninfo()
        {
            return View("tokeninfo",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("top-10-reasons-blog")]
        public async Task<IActionResult> top10reasonsblog()
        {
            return View("top10reasonsblog",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("trade-history")]
        public async Task<IActionResult> tradehistory()
        {
            return View("trade-history",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("trading-rules-svg")]
        public async Task<IActionResult> tradingrulessvg()
        {
            return View("trading-rules-svg",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("trading-rules-us")]
        public async Task<IActionResult> tradingrulesus()
        {
            return View("trading-rules-us",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("transaction")]
        public async Task<IActionResult> transaction()
        {
            return View("transaction",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("uploaded-document")]
        public async Task<IActionResult> uploadeddocument()
        {
            return View("uploaded-document",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("us-aml-policy")]
        public async Task<IActionResult> usamlpolicy()
        {
            return View("us-aml-policy",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("us-privacy-policy")]
        public async Task<IActionResult> usprivacypolicy()
        {
            return View("us-privacy-policy",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("us-terms-conditions")]
        public async Task<IActionResult> ustermsconditions()
        {
            return View("us-terms-conditions",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("user-profile")]
        public async Task<IActionResult> userprofile()
        {
            return View("user-profile",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("wallet-2")]
        public async Task<IActionResult> wallet2()
        {
            return View("wallet-2",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("wallet")]
        public async Task<IActionResult> wallet()
        {
            return View("wallet",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("what-is-value-blog")]
        public async Task<IActionResult> whatisvalueblog()
        {
            return View("whatisvalueblog",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("what-makes-TechnoSavvy-gold-blog")]
        public async Task<IActionResult> whatmakesTechnoSavvygoldblog()
        {
            return View("what-makes-TechnoSavvy-gold-blog",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("whatisTechnoSavvyblog")]
        public async Task<IActionResult> whatisTechnoSavvyblog()
        {
            return View("whatisTechnoSavvyblog",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("why-should-you-invest-blog")]
        public async Task<IActionResult> whyshouldyouinvestblog()
        {
            return View("why-should-you-invest-blog",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("withdrawal-accepted")]
        public async Task<IActionResult> withdrawalaccepted()
        {
            return View("withdrawal-accepted",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("withdrawal-now")]
        public async Task<IActionResult> withdrawalnow()
        {
            return View("withdrawal-now",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("withdrawal-summary")]
        public async Task<IActionResult> withdrawalsummary()
        {
            return View("withdrawal-summary",vmFactory.GetvmUserLogin(appSessionManager));
        }
        [HttpGet]
        [ActionName("withdrawal")]
        public async Task<IActionResult> withdrawal()
        {
            return View("withdrawal",vmFactory.GetvmUserLogin(appSessionManager));
        }
    }
}
