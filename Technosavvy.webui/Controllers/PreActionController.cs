using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace TechnoApp.Ext.Web.UI.Controllers;

[Route("Roadmap")]
[EnsureCompliantCountry]
public class RoadmapController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IOptions<SmtpConfig> _smtp;
    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;

    public RoadmapController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;
        _smtp = smtp;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);

    }
    [HttpGet]
    public async Task<IActionResult> index()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("index", vm);
    }



}

[Route("buy-TechnoSavvy")]
[Route("Secure-TechnoSavvy")]
[Route("buyTechnoSavvy")]
[AfterLogIn]
[EnsureCompliantCountry]
public class buyTechnoSavvyController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IOptions<SmtpConfig> _smtp;
    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;

    public buyTechnoSavvyController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;
        _smtp = smtp;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);

    }
    [HttpGet]
    public async Task<IActionResult> index()
    {
        var vm = await vmFactory.GetvmPrebetaTechnoSavvyBuy(appSessionManager);
        var wm = GetWalletManager();
        var NetworkList = await wm.GetAllSupportedNetwork();
        var addr = await wm.GetMyNetworkWallet(NetworkList.First().SupportedNetworkId);
        if (addr != null)
        {
            vm.frm.ethNetWalletAddress = addr.Address.ToString();
            vm.frm.NetworkId = addr.NetworkId;
        }
        else
        {
            await wm.EnsureNetworkWallet_Prebeta();
            vm.frm.ethNetWalletAddress = "Refresh to Cliam your Network Wallet";
        }
        return View("index", vm);
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> ClaimTran(vmPBTechnoSavvyBuyForm vm)
    {
        try
        {
            vm.TxHash.CheckAndThrowNullArgumentException();
            if (vm.TxHash.Length != 66) throw new ApplicationException("Invalid Transaction Hash");
            vm.NetworkId.CheckAndThrowNullArgumentException();
            vm.ethNetWalletAddress.CheckAndThrowNullArgumentException();
            if (vm.TxHash.ToLower().StartsWith("0x") && vm.TxHash.Length == 66)
            {
                //ToDo:Naveen,Confirm if this Transaction is already Credited to this User
                //-Set Status = ClaimAlreadyActioned

                //ToDo:Naveen,If No, Check if Tx Hash has been previously Investigated
                //-Set Status = ClaimAlreadyActioned
                //Set Msg
                //var wm = GetWalletManager();
                //If No, Raise Request for this Tx Hash for this User and Make Record of such Request
                var dm = GetDepositManager();
                //  var nID = Guid.Parse(vm.NetworkId);
                var IsSuccess = await dm.GetOnDemandCheckNetworkTx(vm.ethNetWalletAddress, vm.NetworkId, vm.TxHash);
                switch (IsSuccess)
                {
                    case Model.mOnDemandRequestResult.Placed:
                        vm.isAwaiting = true;
                        vm.failCount = 0;
                        vm.ErrorMsg = string.Empty;
                        break;
                    case Model.mOnDemandRequestResult.NoIssue:
                        break;
                    case Model.mOnDemandRequestResult.DailyLimitIssue:
                        vm.isAwaiting = false;
                        ModelState.Clear();
                        ModelState.AddModelError("", "Daily limit reached, Try again after 60 minutes");
                        vm.failCount += 1;
                        break;
                    case Model.mOnDemandRequestResult.TotalLimitIssue:
                        vm.isAwaiting = false;
                        ModelState.Clear();
                        ModelState.AddModelError("", "You have exceeded Total Pending verification Limit, Try again after 60 Minutes");
                        vm.failCount += 1;
                        break;
                    case Model.mOnDemandRequestResult.AlreadyClaimed:
                        vm.isAwaiting = false;
                        ModelState.Clear();
                        ModelState.AddModelError("", "This Transaction has already been Claimed");
                        vm.failCount += 1;
                        break;
                    case Model.mOnDemandRequestResult.AlreadyAwaited:
                        vm.isAwaiting = false;
                        ModelState.Clear();
                        ModelState.AddModelError("", "This Transaction has already been Queued for processing");
                        vm.failCount += 1;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                ModelState.Clear();
                ModelState.AddModelError("", "Invalid Transaction Hash");
                vm.failCount = vm.failCount > 0 ? vm.failCount : 1;
            }
            if (vm.failCount >= 3)
            {
                //User Lock
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Occoured in ClaimTran..at:{DateTime.UtcNow.GetCurrentUnix()}\n\t{ex.GetDeepMsg()}");
            ModelState.Clear();
            ModelState.AddModelError("", ex.Message);
        }
        var vm1 = await vmFactory.GetvmPrebetaTechnoSavvyBuy(appSessionManager);
        vm1.frm = vm;

        return View("index", vm1);

    }


    internal WalletManager GetWalletManager()
    {
        var Mgr = new WalletManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        return Mgr;
    }
    internal DepositManager GetDepositManager()
    {
        var Mgr = new DepositManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        appSessionManager.ExtSession.LoadSession().GetAwaiter().GetResult();
        return Mgr;
    }
}
[Route("my-TechnoSavvy")]
[AfterLogIn]
[EnsureCompliantCountry]
public class mypurchaseController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IOptions<SmtpConfig> _smtp;
    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;

    public mypurchaseController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;
        _smtp = smtp;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);

    }
    public async Task<IActionResult> index()
    {
        await appSessionManager.ExtSession.LoadSession();
        var wm = GetWalletManager();
        var vm = await vmFactory.GetvmPBMyPurchase(appSessionManager);
        vm.PrebetaBase.userAccount = appSessionManager.ExtSession.UserSession.UserAccount.AccountNumber;
        vm.myRecords = await wm.GetvmMyPrebetaPurshases(vm);
        await wm.EnsureNetworkWallet_Prebeta();
        return View("index", vm);
    }
    internal WalletManager GetWalletManager()
    {
        var Mgr = new WalletManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        return Mgr;
    }
}

