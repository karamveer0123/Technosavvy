using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace TechnoApp.Ext.Web.UI.Controllers
{
    [DenyAccessInPreBeta]
    [EnsureCompliantCountry]
    public class MarketController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IOptions<SmtpConfig> _smtp;
        private IConfiguration _configuration;
        private HttpContext _context;
        private IHttpContextAccessor _accessor;
        IDataProtector _protector;
        AppSessionManager appSessionManager = null;

        public MarketController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
        {
            _logger = logger;
            _smtp = smtp;
            _configuration = configuration;
            _accessor = accessor;
            _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
            appSessionManager = new AppSessionManager(accessor.HttpContext);

        }
        // GET: MarketController
        public async Task<IActionResult> Index()
        {
            var vm = vmFactory.GetvmMarketTile(appSessionManager);
            return View("index2", vm);//Client Side Script will use Ext API for Data/Formatting
        }
        public async Task<IActionResult> GetMarketsFee(string abb)
        {
            //await appSessionManager.ExtSession.LoadSession();
            //var abb = appSessionManager.mySession.UserCountry;
            var mm = new MarketManager();
            var mlst=await mm.GetActiveMarketsForCountry(abb);
           // var ret = mlst.ToJson();
           // Console.WriteLine($"Info:GetMarketsFee:{ret}");
            return Json(mlst.ToJson());
        }


        public async Task<IActionResult> Explore()
        {
            return View("ListMode", await vmFactory.GetvmBase(appSessionManager));
        }
        // GET: MarketController/Details/5



        public async Task<IActionResult> EnsureValidSession()
        {
            await appSessionManager.ExtSession.LoadSession();
            if (!appSessionManager.ExtSession.IsValid)
                return RedirectToAction("Login", "Login");

            return null;
        }
        internal async Task<SettingsManager> GetSettingsManager()
        {
            await EnsureValidSession();

            var Mgr = new SettingsManager();
            Mgr._configuration = _configuration;
            Mgr._http = _accessor.HttpContext;
            Mgr._appSessionManager = appSessionManager;
            Mgr._DataProtector = _protector;
            return Mgr;
        }
        internal async Task<KYCManager> GetKYCManager()
        {
            await EnsureValidSession();

            var Mgr = new KYCManager();
            Mgr._configuration = _configuration;
            Mgr._http = _accessor.HttpContext;
            Mgr._appSessionManager = appSessionManager;
            return Mgr;
        }
        //add by kavita
        public async Task<IActionResult> othertoken()
        {
            var vm = await vmFactory.GetvmBase(appSessionManager);
            return View("othertoken", vm);
        }
    }


}
