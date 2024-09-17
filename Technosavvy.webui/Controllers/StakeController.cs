using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Security.Cryptography;

namespace TechnoApp.Ext.Web.UI.Controllers;

[AfterProfile]
[DenyAccessInPreBeta]
[EnsureCompliantCountry]
//page add by kavita
public class StakeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;

    public StakeController(ILogger<HomeController> logger, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);

    }
    [HttpGet]
    public async Task<IActionResult> GetOpportunities()
    {
        var WM = GetWalletManager();
        var vm = await vmFactory.GetvmBase(appSessionManager);
        var lst = await WM.GetStakingOpportunities();
        return Json(lst.ToJson());
    }
    [HttpGet]
    public async Task<IActionResult> GetMyStakings()
    {
        var WM = GetWalletManager();
        var vm = await vmFactory.GetvmBase(appSessionManager);
        var lst = await WM.GetMyStakings();
        return Json(lst.ToJson());
    }
    //[HttpGet]
    //public async Task<IActionResult> Index2(string code)
    //{
    //    var vm = await vmFactory.GetvmMyStaking(appSessionManager);
    //    return View("Index2", vm);
    //}
    [HttpGet]
    public async Task<IActionResult> stakeopportunities(string code)
    {
        var vm = await vmFactory.GetvmMyStaking(appSessionManager);
        return View("stakeopportunities", vm);
    }
    public async Task<IActionResult> Index(string code)
    {
        var vm = await vmFactory.GetvmMyStaking(appSessionManager);
        var WM = GetWalletManager();
        vm = await WM.LoadvmMyStaking(vm, code);
        return View("Index2", vm);
    }
    [HttpGet]
    public async Task<IActionResult> Commit(Guid sid)
    {
        var vm = await vmFactory.GetvmMyStaking(appSessionManager);
        var WM = GetWalletManager();
        vm.selectedStakingSlot ??= new mStakingSlot2();
        vm.selectedStakingSlot.StakingOpportunityId = sid;
        vm = await WM.LoadvmMyStaking(vm);
        return View("Index2", vm);
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Index(vmMyStaking vm)
    {
        var vm1 = await vmFactory.GetvmMyStaking(appSessionManager);
        try
        {
            var WM = GetWalletManager();
            vm1.Token = vm.Token;
            vm1.selectedStakingSlot = vm.selectedStakingSlot;
            ModelState.Clear();
            vm1 = await WM.LoadvmMyStaking(vm1);
        }
        catch (Exception ex)
        {
            ModelState.Clear();
            if (ex is ApplicationException || ex is ArgumentException || ex is InvalidOperationException)
            {
                ModelState.AddModelError("", ex.Message);
            }
            else
            {
                Console2.WriteLine_RED($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} in {this.GetType().Name} caused error:{ex.GetDeepMsg()}");
                ModelState.AddModelError("", "Technical Issue. Please try again later");
            }
        }
        return View("Index2", vm1);
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Summary(vmMyStaking vm)
    {
        var vm1 = await vmFactory.GetvmMyStaking(appSessionManager);
        try
        {
            var WM = GetWalletManager();
            vm1.Token = vm.Token;
            vm1.selectedStakingSlot = vm.selectedStakingSlot;
            vm1.Amount = vm.Amount;
            vm1.AutoRenew = vm.AutoRenew;
            vm1.IsIncludeOtherBalance = vm.IsIncludeOtherBalance;
            ModelState.Clear();
            vm1 = await WM.LoadvmMyStaking(vm1);
            await WM.ValidateVmMyStaking(vm1);
            //await WM.CommitStake(vm1);
            return View("StakeSummary", vm1);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Occoured in StakingOpportunity..at:{DateTime.UtcNow}\n\t{ex.GetDeepMsg()}");
            ModelState.Clear();
            if (ex is ApplicationException || ex is ArgumentException || ex is InvalidOperationException)
            {
                ModelState.AddModelError("", ex.Message);
            }
            else
            {
                Console2.WriteLine_RED($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} in {this.GetType().Name} caused error:{ex.GetDeepMsg()}");
                ModelState.AddModelError("", "Technical Issue. Please try again later");
            }
        }
        return View("Index2", vm1);
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Confirmation(vmMyStaking vm)
    {
        var vm1 = await vmFactory.GetvmMyStaking(appSessionManager);
        try
        {
            var WM = GetWalletManager();
            vm1.Token = vm.Token;
            vm1.selectedStakingSlot = vm.selectedStakingSlot;
            vm1.Amount = vm.Amount;
            vm1.AutoRenew = vm.AutoRenew;
            vm1.IsIncludeOtherBalance = vm.IsIncludeOtherBalance;
            vm1.IsAgree = vm.IsAgree;
            ModelState.Clear();
            vm1 = await WM.LoadvmMyStaking(vm1);
            if (!vm.IsAgree)
                throw new ApplicationException("You must agree to the Terms");
            await WM.ValidateVmMyStaking(vm1);
            var vm2 = await vmFactory.GetvmMyStakingConfirm(appSessionManager);

            vm2.Record = await WM.CommitStake(vm1);
            vm2.Token = vm1.Token;
            return View("stakeconfirmation", vm2);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Occoured in StakingOpportunity..at:{DateTime.UtcNow}\n\t{ex.GetDeepMsg()}");
            ModelState.Clear();
            if (ex is ApplicationException || ex is ArgumentException || ex is InvalidOperationException)
            {
                ModelState.AddModelError("", ex.Message);
            }
            else
            {
                Console2.WriteLine_RED($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} in {this.GetType().Name} caused error:{ex.GetDeepMsg()}");
                ModelState.AddModelError("", "Technical Issue. Please try again later");
            }
        }
        return View("StakeSummary", vm1);
    }
    [HttpGet]
    public async Task<IActionResult> redeem(Guid sid)
    {
        var vm = await vmFactory.GetvmMyStakingConfirm(appSessionManager);
        var WM = GetWalletManager();
        var ret = await WM.GetMyStakings();
        vm.Record = ret.FirstOrDefault(x => x.StakeId == sid);
        if (vm.Record != null)
        {
            vm.Token = vm.Record.StakingSlot.Token;
        }
        return View("redeem", vm);
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> redeem_(vmMyStakingConfirm vm)
    {
        var vm2 = await vmFactory.GetvmMyStakingConfirm(appSessionManager);
        try
        {
            var WM = GetWalletManager();
            var ret = await WM.RedeemMyStake(vm.Record.StakeId);
            vm2.Record = ret;
            if (vm2.Record != null)
            {
                vm2.Token = vm2.Record.StakingSlot.Token;
            }
            return View("redeemconfirmation", vm2);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Occoured in redeem_..at:{DateTime.UtcNow}\n\t{ex.GetDeepMsg()}");
            ModelState.Clear();
            if (ex is ApplicationException || ex is ArgumentException || ex is InvalidOperationException)
            {
                ModelState.AddModelError("", ex.Message);
            }
            else
            {
                Console2.WriteLine_RED($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} in {this.GetType().Name} caused error:{ex.GetDeepMsg()}");
                ModelState.AddModelError("", "Technical Issue. Please try again later");
            }
        }
        return View("redeemconfirmation", vm2);
    }

    internal WalletManager GetWalletManager()
    {
        var Mgr = new WalletManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        appSessionManager.ExtSession.LoadSession().GetAwaiter().GetResult();
        return Mgr;
        //}
    }
}
public class StakingController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;

    public StakingController(ILogger<HomeController> logger, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);

    }
    [HttpGet]
    public async Task<IActionResult> index()
    {
        var WM = GetWalletManager();
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("stakeopportunities",vm);
    }
    [HttpGet]
    public async Task<IActionResult> opportunities()
    {
        var WM = GetWalletManager();
        // var vm = await vmFactory.GetvmBase(appSessionManager);
        var lst = await WM.GetStakingOpportunities();
        return Json(lst.ToJson());
    }
    internal WalletManager GetWalletManager()
    {
        var Mgr = new WalletManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        appSessionManager.ExtSession.LoadSession().GetAwaiter().GetResult();
        return Mgr;
        //}
    }
}