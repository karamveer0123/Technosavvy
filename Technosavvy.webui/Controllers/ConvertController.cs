using Microsoft.AspNetCore.Mvc;
using TechnoApp.Ext.Web.UI.Model;
using NuGet.Protocol;

namespace TechnoApp.Ext.Web.UI.Controllers;

[AfterProfile]
[DenyAccessInPreBeta]
[EnsureCompliantCountry]
public class ConvertController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;

    public ConvertController(ILogger<HomeController> logger, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;

        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);

    }
    public async Task<IActionResult> Index(string code)
    {
        var vm = await vmFactory.GetvmConvert(appSessionManager);
        var wm = GetWalletManager();
        var funds = await wm.GetMyFundingWalletSummery();
        vm.TokenList = await wm.GetAllActiveCryptoTokens();
        // vm.CurrencyList = vm.TokenList;//await wm.GetFiatTokensList();
        if (code.IsNOT_NullorEmpty())
        {
            var c = vm.TokenList.FirstOrDefault(x => x.Code.ToLower() == code.ToLower());
            if (c != null)
                vm.selectedCoin = c.TokenId;
        }
        ModelState.Clear();
        return View("Index", vm);
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Index(vmConvert vm0)
    {
        try
        {
            var vm = (vmConvert)vmFactory.InitializeBase(vm0, appSessionManager);
            var wm = GetWalletManager();
            //get this from globale variable
            //vm.MinBuyAmt = ;//USDT get this from Global variable
            vm.TokenList = await wm.GetAllActiveCryptoTokens();
            // vm.CurrencyList = vm.TokenList; 
            //vm.selectedCoin = vm0.selectedCoin;
            //vm.PaySelectedCoin = vm0.PaySelectedCoin;
            //vm.BuyAmount = vm0.BuyAmount;
            //vm.PayAmount = vm0.PayAmount;
            if (vm0.selectedCoin.HasValue && vm0.PaySelectedCoin.HasValue)
            {
                var b = vm.TokenList.FirstOrDefault(x => x.TokenId == vm0.selectedCoin);
                var q = vm.TokenList.FirstOrDefault(x => x.TokenId == vm0.PaySelectedCoin);
                vm.Token = b;
                vm.PayToken = q;
                var Amt = vm.BuyAmount > 0 ? vm.BuyAmount : 1;
                //Get Price
                wm.GetEstimatedValueIn(b.Code, q.Code, Amt, out var Rate, out var MinTrade);
                //Fetch Price from 
                vm.ValueOfOne = Rate > 0 ? Rate : double.NaN;
                //if (!double.IsNaN(vm.ValueOfOne))
                //    vm.ValueOfOne += vm.ValueOfOne * await wm.ConvertCharge($"{b.Code}{q.Code}");
                vm.MinBuyAmt = MinTrade;
                if (b == q)//Same Token
                    vm.BuyAmount = vm.PayAmount = 0;//Reset
                if (vm0.selectedCoin.Value == vm0.PaySelectedCoin.Value)
                {
                    vm.PaySelectedCoin = null;
                    vm.ValueOfOne = 0;
                    vm.ValueOfOne = 0;
                }
            }

            if (vm0.PaySelectedCoin.HasValue)
            {
                var funds = await wm.GetCoinBalInMyWallets(vm0.PaySelectedCoin.Value);
                vm.AvailablePayAmount = funds.FundWallet.Amount;
                vm.AddBalance = funds.SpotWallet.Amount;
            }
            ModelState.Clear();
            return View("Index", vm);
        }
        catch (Exception ex)
        {
            ModelState.Clear();
            ModelState.AddModelError("", ex.Message);
        }
        return View("Index", vm0);
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Summary(vmConvert vm0)
    {
        try
        {
            var vm = (vmConvert)vmFactory.InitializeBase(vm0, appSessionManager);
            var wm = GetWalletManager();
            vm.TokenList = await wm.GetAllActiveCryptoTokens();
            if (vm0.PaySelectedCoin.HasValue)
            {
                var funds = await wm.GetCoinBalInMyWallets(vm0.PaySelectedCoin.Value);
                vm.AvailablePayAmount = funds.FundWallet.Amount;
                vm.AddBalance = funds.SpotWallet.Amount;
            }
            if (vm0.selectedCoin.HasValue && vm0.PaySelectedCoin.HasValue)
            {
                var b = vm.TokenList.FirstOrDefault(x => x.TokenId == vm0.selectedCoin);
                var q = vm.TokenList.FirstOrDefault(x => x.TokenId == vm0.PaySelectedCoin);
                vm.Token = b;
                vm.PayToken = q;
                var Amt = vm.BuyAmount > 0 ? vm.BuyAmount : 1;
                //Get Price
                wm.GetEstimatedValueIn(b.Code, q.Code, Amt, out var Rate, out var MinTrade);
                //Fetch Price from 
                vm.ValueOfOne = Rate > 0 ? Rate : double.NaN;
                //vm.ValueOfOne += vm.ValueOfOne * await wm.ConvertCharge($"{b.Code}{q.Code}");
                vm.MinBuyAmt = await wm.MinimumTradeUSDTValue() / vm.ValueOfOne;
                if (vm.PayAmount <= 0 || vm.BuyAmount <= 0)
                    throw new ApplicationException("Invalid Token Amount");
                var isValid = vm.PayAmount <= (vm.IsAll ? vm.AvailablePayAmount + vm.AddBalance : vm.AvailablePayAmount);
                if (!isValid)
                    throw new ApplicationException("Amount exceeds you available Token ");
                if (vm.BuyAmount < vm.MinBuyAmt)
                    throw new ApplicationException($"Minimum buy is {vm.MinBuyAmt} ");
                vm.BuyAmount= vm.PayAmount / vm.ValueOfOne;

                if (vm0.selectedCoin.Value == vm0.PaySelectedCoin.Value)
                {
                    vm.BuyAmount = vm.PayAmount = 0;//Reset
                    vm.PaySelectedCoin = null;
                    vm.ValueOfOne = 0;
                    vm.ValueOfOne = 0;
                    throw new ApplicationException("Invalid Token Conversion");
                }
            }

            ModelState.Clear();
            //Now Send this Request to engine
            var ret = await wm.CreateAndSendConvertTokensRequest(vm);
            Console2.WriteLine_White($"Temp:{ret.ToJson()}");
            if (ret.IsError)
            {
                //vm to display error
                ModelState.Clear();
                ModelState.AddModelError("", ret.ErrMsg);
                return View("Index", vm0);
            }
            return View("convertconfirmation", vm);
        }
        catch (Exception ex)
        {
            ModelState.Clear();
            ModelState.AddModelError("", ex.Message);
        }
        return View("Index", vm0);
    }
    public async Task<IActionResult> Getgoingrate(string q, string b, double Amt)
    {
        if (Amt <= 0 || double.IsNaN(Amt) || double.IsInfinity(Amt))
            Amt = 1;
        if (q.IsNullOrEmpty() || b.IsNullOrEmpty()) return Json("0".ToJson());
        var wm = GetWalletManager();
        wm.GetEstimatedValueIn(b, q, Amt, out var Rate, out var MinTrade);
        var x = new { q = q, Rate = Rate, MinimumTrade = MinTrade };
        return Json(x.ToJson());
    }
    //add by kavita

    public async Task<IActionResult> confirmation()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("confirmation", vm);
    }
    public async Task<IActionResult> convertwithdraw()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("ConvertAndWithdraw", vm);
    }
    public async Task<IActionResult> convertconfirmation()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("convertconfirmation", vm);
    }
    public async Task<IActionResult> conwithdrawprocess()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("conwithdrawprocess", vm);
    }
    public async Task<IActionResult> conwithdrawsummary()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("conwithdrawsummary", vm);
    }
    public async Task<IActionResult> conwithdrawconfirmation()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("conwithdrawconfirmation", vm);
    }
    internal WalletManager GetWalletManager()
    {
        var Mgr = new WalletManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        appSessionManager.ExtSession.LoadSession().GetAwaiter().GetResult();
        return Mgr;
    }
}