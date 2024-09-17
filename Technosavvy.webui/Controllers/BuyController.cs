using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using TechnoApp.Ext.Web.UI.Model;
using TechnoApp.Ext.Web.UI.Service;
using NuGet.Protocol;

namespace TechnoApp.Ext.Web.UI.Controllers;

[AfterProfile]
[DenyAccessInPreBeta]
[EnsureCompliantCountry]
public class BuyController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;

    public BuyController(ILogger<HomeController> logger, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;

        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);

    }
    [HttpGet]
    public async Task<IActionResult> Index(string code)
    {
        var vm = await vmFactory.GetvmBuy(appSessionManager);
        var wm = GetWalletManager();
        var funds = await wm.GetMyFundingWalletSummery();
        vm.TokenList = await wm.GetAllActiveCryptoTokens();
        vm.CurrencyList = await wm.GetFiatTokensList();
        // funds.Fiats;
        //  vm.CurrencyList = await wm.GetFiatTokensList();// funds.Fiats;
        // vm.CurrencyList ??= new List<mWalletCoin>();
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
    public async Task<IActionResult> Index(vmBuy vm0)
    {
        try
        {
            var vm = (vmBuy)vmFactory.InitializeBase(vm0, appSessionManager);
            var wm = GetWalletManager();
            vm.TokenList = await wm.GetAllActiveCryptoTokens();
            vm.CurrencyList = await wm.GetFiatTokensList();
            if (vm0.selectedCoin.HasValue)
            {
                vm.Token = vm.TokenList.FirstOrDefault(x => x.TokenId == vm0.selectedCoin);
            }
            if (vm0.selectedCoin.HasValue && vm0.PaySelectedCoin.HasValue)
            {
                var b = vm.TokenList.FirstOrDefault(x => x.TokenId == vm0.selectedCoin);
                var q = vm.CurrencyList.FirstOrDefault(x => x.token.TokenId == vm0.PaySelectedCoin);
                var Amt = vm.BuyAmount > 0 ? vm.BuyAmount : 1;
                //Get Price
                wm.GetEstimatedValueIn(b.Code, q.token.Code, Amt, out var Rate, out var MinTrade);
                vm.MinBuyAmt = MinTrade;
                //Fetch Price from 
                //vm.MinBuyAmt= 10 USDT
                vm.ValueOfOne = Rate > 0 ? Rate : double.NaN;
                if (q.token.Code == b.Code)
                    vm.BuyAmount = vm.PayAmount = 0;//Reset
            }
            if (vm0.PaySelectedCoin.HasValue)
            {
                vm.PayToken = vm.CurrencyList.FirstOrDefault(x => x.token.TokenId == vm0.PaySelectedCoin);
                var funds = await wm.GetMyFundingWalletSummery();
                var b = funds.Tokens.FirstOrDefault(x => x.CoinId == vm0.PaySelectedCoin.Value);
                if (b != null)
                    vm.AvailablePayAmount = b.Amount;
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
    public async Task<IActionResult> summary(vmBuy vm0)
    {
        try
        {
            var vm = (vmBuy)vmFactory.InitializeBase(vm0, appSessionManager);
            var wm = GetWalletManager();
            vm.TokenList = await wm.GetAllActiveCryptoTokens();
            vm.CurrencyList = await wm.GetFiatTokensList();
            if (vm0.selectedCoin.HasValue)
            {
                vm.Token = vm.TokenList.FirstOrDefault(x => x.TokenId == vm0.selectedCoin);
            }
            if (vm0.selectedCoin.HasValue && vm0.PaySelectedCoin.HasValue)
            {
                var b = vm.TokenList.FirstOrDefault(x => x.TokenId == vm0.selectedCoin);
                var q = vm.CurrencyList.FirstOrDefault(x => x.token.TokenId == vm0.PaySelectedCoin);
                var Amt = vm.BuyAmount > 0 ? vm.BuyAmount : 1;
                //Get Price
                wm.GetEstimatedValueIn(b.Code, q.token.Code, Amt, out var Rate, out var MinTrade);
                vm.MinBuyAmt = MinTrade;
                //Fetch Price from 
                //vm.MinBuyAmt= 10 USDT
                vm.ValueOfOne = Rate > 0 ? Rate : double.NaN;
                if (q.token.Code == b.Code)
                    vm.BuyAmount = vm.PayAmount = 0;//Reset
            }
            if (vm0.PaySelectedCoin.HasValue)
            {
                vm.PayToken = vm.CurrencyList.FirstOrDefault(x => x.token.TokenId == vm0.PaySelectedCoin);
                var funds = await wm.GetMyFundingWalletSummery();
                var b = funds.Tokens.FirstOrDefault(x => x.CoinId == vm0.PaySelectedCoin.Value);
                if (b != null)
                    vm.AvailablePayAmount = b.Amount;
            }
            vm.BuyAmount = vm.PayAmount / vm.ValueOfOne;
            //Now Validate
            if (vm.PayToken == null)
                throw new ApplicationException("Buying Token Must be Selected");
            if (vm.BuyAmount < vm.MinBuyAmt)
                throw new ApplicationException($"Minimum Buy Amount is {vm.MinBuyAmt}");
            if (vm.PayToken == null)
                throw new ApplicationException("Paying Token Must be Selected");
            if (vm.PayAmount <= 0 || vm.PayAmount > vm.AvailablePayAmount)
                throw new ApplicationException("Invalid Paying Token Amount");

            ModelState.Clear();
            return View("buysummary", vm);
        }
        catch (Exception ex)
        {
            ModelState.Clear();
            ModelState.AddModelError("", ex.Message);
        }
        return View("Index", vm0);
    }
    public async Task<IActionResult> Confirm(vmBuy vm0)
    {
        try
        {
            var vm = (vmBuy)vmFactory.InitializeBase(vm0, appSessionManager);
            var wm = GetWalletManager();
            vm.TokenList = await wm.GetAllActiveCryptoTokens();
            vm.CurrencyList = await wm.GetFiatTokensList();
            if (vm0.selectedCoin.HasValue)
            {
                vm.Token = vm.TokenList.FirstOrDefault(x => x.TokenId == vm0.selectedCoin);
            }
            if (vm0.selectedCoin.HasValue && vm0.PaySelectedCoin.HasValue)
            {
                var b = vm.TokenList.FirstOrDefault(x => x.TokenId == vm0.selectedCoin);
                var q = vm.CurrencyList.FirstOrDefault(x => x.token.TokenId == vm0.PaySelectedCoin);
                var Amt = vm.BuyAmount > 0 ? vm.BuyAmount : 1;
                //Get Price
                wm.GetEstimatedValueIn(b.Code, q.token.Code, Amt, out var Rate, out var MinTrade);
                vm.MinBuyAmt = MinTrade;
                //Fetch Price from 
                //vm.MinBuyAmt= 10 USDT
                vm.ValueOfOne = Rate > 0 ? Rate : double.NaN;
                if (q.token.Code == b.Code)
                    vm.BuyAmount = vm.PayAmount = 0;//Reset
            }
            if (vm0.PaySelectedCoin.HasValue)
            {
                vm.PayToken = vm.CurrencyList.FirstOrDefault(x => x.token.TokenId == vm0.PaySelectedCoin);
                var funds = await wm.GetMyFundingWalletSummery();
                var b = funds.Tokens.FirstOrDefault(x => x.CoinId == vm0.PaySelectedCoin.Value);
                if (b != null)
                    vm.AvailablePayAmount = b.Amount;
            }
            vm.BuyAmount = vm.PayAmount / vm.ValueOfOne;
            //Now Validate
            if (vm.PayToken == null)
                throw new ApplicationException("Buying Token Must be Selected");
            if (vm.BuyAmount < vm.MinBuyAmt)
                throw new ApplicationException($"Minimum Buy Amount is {vm.MinBuyAmt}");
            if (vm.PayToken == null)
                throw new ApplicationException("Paying Token Must be Selected");
            if (vm.PayAmount <= 0 || vm.PayAmount > vm.AvailablePayAmount)
                throw new ApplicationException("Invalid Paying Token Amount");

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
            return View("Buyconfirmation", vm);
        }
        catch (Exception ex)
        {
            ModelState.Clear();
            ModelState.AddModelError("", ex.Message);
        }
        return View("Index", vm0);
    }
    //[HttpPost]
    //public async Task<IActionResult> summary(vmBuy vm)
    //{
    //    var vm1 = (vmBuy)vmFactory.InitializeBase(vm, appSessionManager);
    //    try
    //    {
    //        var wm = GetWalletManager();

    //        if (vm.selectedCoin.HasValue && vm.PaySelectedCoin.HasValue)
    //        {
    //            //Get Price
    //            var m = SrvCoinPriceHUB.GetAllCoin();
    //            //Fetch Price from 
    //            //vm.MinBuyAmt= 10 USDT
    //            vm.ValueOfOne = 1.3087;
    //            vm.BuyAmount = vm.PayAmount = 0;//Reset
    //        }
    //        return View("buysummary", vm1);
    //    }
    //    catch (Exception ex)
    //    {
    //        ModelState.Clear();
    //        ModelState.AddModelError("", ex.Message);
    //    }
    //    return View("index", vm1);
    //}
    [HttpGet]
    public async Task<IActionResult> BuyTechnoSavvy()
    {
        //await EnsureValidSession();
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("Index", vm);
    }
    //add by kavita
    [HttpGet]
    public async Task<IActionResult> buyconfirmation()
    {
        //await EnsureValidSession();
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("buyconfirmation", vm);
    }
    //add by kavita
    [HttpGet]
    public async Task<IActionResult> summary()
    {
        //await EnsureValidSession();
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("buysummary", vm);
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
