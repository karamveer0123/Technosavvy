using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using TechnoApp.Ext.Web.UI.Service;
using NuGet.Protocol;
using System.Text;

namespace TechnoApp.Ext.Web.UI.Controllers;

[AfterProfile]
[DenyAccessInPreBeta]
[EnsureCompliantCountry]
public class WalletController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IOptions<SmtpConfig> _smtp;
    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;
    public WalletController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;
        _smtp = smtp;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);
        Console2.WriteLine_White($"Info:Wallet Controller Instantiation");

    }
    [HttpGet]
    [ActionName("index")]
    public async Task<IActionResult> Index()
    {
        Console2.WriteLine_Green($"User:{appSessionManager.mySession.UserName} has Requested Wallet Index at.. {DateTime.UtcNow}");
        try
        {
            var WM = GetWalletManager();
            var vm = vmFactory.GetvmWalletHome(appSessionManager);
            vm.UserDetails = await WM.GetvmWalletUserDetails();
            await WM.GetvmWalletHomeAssetDetails(vm);
            vm.MyCashBack = await WM.GetMyCashbackRecords();
            vm.MyRewards= await WM.GetMyRewardsRecords();
            Console.WriteLine($"User: Returned Index");
            return View("Index3", vm);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wallet Index has error:{ex.GetDeepMsg()}");
            return RedirectToAction("error", "error");
        }
    }
    [HttpGet]
    public async Task<IActionResult> GetMyCashback()
    {
        var WM = GetWalletManager();
        var cb = await WM.GetMyCashbackRecords();
        return Json(cb.ToJson());
    }
    [HttpGet]
    public async Task<IActionResult> GetMyRewards()
    {
        var WM = GetWalletManager();
        var cb = await WM.GetMyRewardsRecords();
        return Json(cb.ToJson());
    }
    [HttpGet]
    public async Task<IActionResult> GetWalletAssets()
    {
        try
        {
            var WM = GetWalletManager();
            var vm = vmFactory.GetvmWalletHome(appSessionManager);
            vm.UserDetails = await WM.GetvmWalletUserDetails();
            await WM.GetvmWalletHomeAssetDetails(vm);
            return Json(vm.Assets.ToJson());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetWalletAssets error:{ex.GetDeepMsg()}");
            return RedirectToAction("error", "error");
        }
    }
    [HttpGet]
    public async Task<IActionResult> SpotWallet()
    {
        var WM = GetWalletManager();
        var vm = vmFactory.GetvmSpotWallet(appSessionManager);
        vm.UserDetails = await WM.GetvmWalletUserDetails();
        await WM.GetvmSpotWalletAssetDetails(vm);
        return View("SpotWallet1", vm);
    }
    [HttpGet]
    public async Task<IActionResult> accountstatement()
    {
        var DM = GetWalletManager();
        var vm = vmFactory.GetvmWalletHome(appSessionManager);
        return View(vm);
    }
    [HttpGet]
    public async Task<IActionResult> EarnWallet()
    {
        //if ((await DoesHaveAnActiveSession()) == false)
        //    return RedirectToAction("Login", "Login");

        var WM = GetWalletManager();
        var vm = vmFactory.GetvmEarnWallet(appSessionManager);
        vm.UserDetails = await WM.GetvmWalletUserDetails();
        await WM.GetvmEarnWalletAssetDetails(vm);
        vm.StakeOpportunity = await WM.GetStakingOpportunities();
        vm.Stakings = await WM.GetMyStakings();
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> fundingwallet()
    {
        var WM = GetWalletManager();
        var vm = vmFactory.GetvmFundingWallet(appSessionManager);
        vm.UserDetails = await WM.GetvmWalletUserDetails();
        await WM.GetvmvmFundingWalletAssetDetails(vm);
        return View("fundingwallet", vm);
    }
    [HttpGet]
    public async Task<IActionResult> Transaction()
    {
        var WM = GetWalletManager();
        var vm = vmFactory.GetvmWalletTransactions(appSessionManager);
        vm = await WM.GetvmWalletTransactions(vm);
        return View("Transaction", vm);
    }
    [HttpGet]
    [ActionName("inter-wallet-transfer")]
    public async Task<IActionResult> InterWalletTransfer()
    {
        var vm = await vmFactory.GetvmWalletFundTransfer(appSessionManager);
        return View(vm);
    }
    [HttpGet]
    public async Task<IActionResult> TransferWallet(int wtype)
    {
        try
        {
            var wm = GetWalletManager();
            return Json((await wm.GetWalletTokens(wtype)).ToJson());
            //if (wtype == 2)
            //{
            //    return Json((await wm.GetMySpotWalletSummery()).Tokens.ToJson());
            //}
            //else if (wtype == 3)
            //{
            //    var fw = (await wm.GetMyFundingWalletSummery());
            //    var tks = fw.Tokens;
            //    if (fw.Fiats != null && fw.Fiats.Count > 0)
            //        tks.AddRange(fw.Fiats);
            //    return Json(tks.ToJson());
            //}
            //else if (wtype == 1)
            //{
            //    return Json((await wm.GetMyEarnWalletSummery()).Tokens.ToJson());
            //}
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR:TransferWallet caused error:{ex.GetDeepMsg()}");
        }

        return BadRequest("Invalid Wallet Selection");
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TransferFundInWallet(vmWalletFundTransfer vm)
    {
        try
        {
            var wm = GetWalletManager();
            vm.CheckAndThrowNullArgumentException();
            vm.Amount.CheckAndThrowNullArgumentException();
            if (vm.fromWalletType <= 0) throw new ApplicationException("From Wallet must be selected");
            if (vm.toWalletType <= 0) throw new ApplicationException("To Wallet must be selected");
            vm.fromWalletType.CheckAndThrowNullArgumentException();
            vm.toWalletType.CheckAndThrowNullArgumentException();
            vm.selectedCoin.CheckAndThrowNullArgumentException();
            if (vm.toWalletType == vm.fromWalletType) throw new ApplicationException("To and From Wallet Can't be Same");
            var vm2 = await vmFactory.GetvmWalletFundTransfer(appSessionManager);
            vm2.Amount = vm.Amount;
            vm2.selectedCoin = vm.selectedCoin;
            vm2.Code = vm.Code;
            if (vm.fromWalletType == 1)//Earn
            {
                if (vm.toWalletType == 2)
                    vm2.Status = await wm.SendTokenFromEarnToSpot(vm.selectedCoin!.Value, vm.Amount!.Value);
                else if (vm.toWalletType == 3)
                    vm2.Status = await wm.SendTokenFromEarnToFunding(vm.selectedCoin!.Value, vm.Amount!.Value);
            }
            else if (vm.fromWalletType == 2)//SPOT
            {
                if (vm.toWalletType == 1)
                    vm2.Status = await wm.SendTokenFromSpotToEarn(vm.selectedCoin!.Value, vm.Amount!.Value);
                else if (vm.toWalletType == 3)
                    vm2.Status = await wm.SendTokenFromSpotToFunding(vm.selectedCoin!.Value, vm.Amount!.Value);
            }
            else if (vm.fromWalletType == 3)//FUNDING
            {
                if (vm.toWalletType == 1)
                    vm2.Status = await wm.SendTokenFromFundingToEarn(vm.selectedCoin!.Value, vm.Amount!.Value);
                else if (vm.toWalletType == 2)
                    vm2.Status = await wm.SendTokenFromFundingToSpot(vm.selectedCoin!.Value, vm.Amount!.Value);
            }
            return View("inter-wallet-transfer", vm2);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Occoured in TransferFundInWallet..at:{DateTime.UtcNow.GetCurrentUnix()}\n\t{ex.GetDeepMsg()}");
            ModelState.Clear();
            ModelState.AddModelError("", ex.Message);

        }
        return View("inter-wallet-transfer", vm);
        //return RedirectToAction("index");
    }
    [HttpGet]
    [ActionName("Open-Orders")]
    public async Task<IActionResult> OpenOrders()
    {
        var vm = await vmFactory.GetvmOrders(appSessionManager);
        var wm = GetWalletManager();
        vm.myOrders = await wm.GetMyOpenOrdersOf();
        return View("OpenOrders", vm);
    }
    [HttpGet]
    public async Task<IActionResult> MyOpenOrders()
    {
        var wm = GetWalletManager();
        var lst = await wm.GetMyOpenOrdersOf();
        return Json(lst.ToJson());
    }
    [HttpGet]
    [ActionName("Orders-history")]
    public async Task<IActionResult> OrderHistory()
    {
        var vm = await vmFactory.GetvmOrders(appSessionManager);
        var wm = GetWalletManager();
        //vm.myOrders = await wm.GetMyOpenOrders(mCode);
        var acc = appSessionManager.ExtSession.UserSession.UserAccount.AccountNumber;
        vm.myOrders = await wm.GetOrderHistory(acc);
        return View("OrderHistory", vm);
    }
    [HttpGet]
    public async Task<IActionResult> MyOrdersHistory()
    {
        //  var vm = await vmFactory.GetvmOrders(appSessionManager);
        var wm = GetWalletManager();
        var acc = appSessionManager.ExtSession.UserSession.UserAccount.AccountNumber;
        var lst = await wm.GetOrderHistory(acc);
        return Json(lst.ToJson());
    }
    [HttpGet]
    [ActionName("Trades")]
    public async Task<IActionResult> TradeHistory()
    {
        var vm = await vmFactory.GetvmTrades(appSessionManager);
        var wm = GetWalletManager();
        var acc = appSessionManager.ExtSession.UserSession.UserAccount.AccountNumber;
        vm.myTrades = await wm.MyRecentTrade(acc, -1);
        // var ord = await wm.GetMyOpenOrders(mCode);
        return View("TradeHistory", vm);
    }
    [HttpGet]
    [ActionName("MyTrades")]
    public async Task<IActionResult> MyTrades(int skip, int take)
    {
        var wm = GetWalletManager();
        var acc = appSessionManager.ExtSession.UserSession.UserAccount.AccountNumber;
        var lst = await wm.MyRecentTrade(acc, -1);
        // var ord = await wm.GetMyOpenOrders(mCode);
        return Json(lst.ToJson());
    }
    [HttpGet]
    public async Task<IActionResult> GetCoinBalInMyWallets(Guid tokenId)
    {
        var ret = await GetWalletManager().GetCoinBalInMyWallets(tokenId);
        return Json(ret.ToJson());
    }
    public async Task GetTransactions()
    {

    }


    private async Task<bool> DoesHaveAnActiveSession()
    {
        await appSessionManager.ExtSession.LoadSession();
        return appSessionManager.ExtSession.IsValid;
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
    internal DepositManager GetDepositManager()
    {
        var Mgr = new DepositManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        appSessionManager.ExtSession.LoadSession().GetAwaiter().GetResult();
        return Mgr;
    }
    //add by kavita
    public async Task<IActionResult> RequestHistory()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("RequestHistory", vm);
    }
    //add by kavita
    public async Task<IActionResult> RequestDetail()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("RequestDetail", vm);
    }
    //add by kavita
    public async Task<IActionResult> OrderDetail()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("OrderDetail", vm);
    }
}
