using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TechnoApp.Ext.Web.UI.Extentions;
using TechnoApp.Ext.Web.UI.Service;
using TechnoApp.Ext.Web.UI.ViewModels;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Plugins;

namespace TechnoApp.Ext.Web.UI.Controllers;

[AfterProfile]
[EnsureCompliantCountry]
[DenyAccessInPreBeta]
public class WithdrawController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IOptions<SmtpConfig> _smtp;
    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;

    public WithdrawController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;
        _smtp = smtp;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);

    }
    [HttpGet]
    public async Task<IActionResult> index(string code)
    {
        var vm = await vmFactory.GetvmWithdraw(appSessionManager);
        var tm = GetWalletManager();
        vm.TokenList = await tm.GetAllActiveCryptoTokens();
        vm.CurrencyList = await tm.GetFiatTokensList();
        vm.SupportedNetwork = new List<Model.mSupportedNetwork>();
        vm.ActionMethod = "NetSummary";
        if (code.IsNOT_NullorEmpty())
        {
            var c = vm.TokenList.FirstOrDefault(x => x.Code.ToLower() == code.ToLower());
            var d = vm.CurrencyList.FirstOrDefault(x => x.token.Code.ToLower() == code.ToLower());
            if (c != null)
                vm.selectedCoin = c.TokenId;
            if (d != null)
            {
                vm.selectedCoin = d.token.TokenId;
                vm.Symbole = d.Symbole;
            }
        }
        ModelState.Clear();
        return View("Index", vm);
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> mtype(vmWithdraw vm)
    {
        var vm1 = vm.ToClone(appSessionManager);
        try
        {
            var dm = GetDepositManager();
            vm1.TokenList = await dm.GetAllActiveCryptoTokens();
            vm1.CurrencyList = await dm.GetFiatTokensList();
            if (vm.selectedCoin != null)
            {
                vm1.isBack = "mtype";

                var coin = vm1.CurrencyList.FirstOrDefault(x => x.token.TokenId == vm.selectedCoin);
                vm1.IsFiat = coin != null;
                if (coin != null && coin.token.Code.ToUpper() == "INR")
                {
                    vm1 = await dm.PoplateINRAndMyPayOptions(vm1);
                    var bal = await GetWalletManager().GetCoinBalInMyWallets(coin.token.TokenId);
                    var tBal2 = bal.FundWallet != null ? bal.FundWallet.Amount : 0;
                    vm1.ActionMethod = "Currency";
                    vm1.ReceiverAddr = string.Empty;
                    vm1.WBalance = tBal2;
                    vm1.Amount = tBal2;
                }
                vm1.Token = vm1.TokenList.FirstOrDefault(x => x.TokenId == vm.selectedCoin);
                vm1.IsToken = vm1.Token != null;
                if (vm1.IsToken)
                {
                    vm1.SupportedNetwork = (vm1.Token.SupportedCoin.Select(x => x.RelatedNetwork)).ToList();
                    var wm = GetWalletManager();
                    var wBal = await wm.GetCoinBalInMyWallets(vm1.Token.TokenId);
                    // var tBal = wBal.EarnWallet != null ? wBal.EarnWallet.Amount : 0;
                    var tBal = wBal.SpotWallet != null ? wBal.SpotWallet.Amount : 0;
                    var tBal2 = wBal.FundWallet != null ? wBal.FundWallet.Amount : 0;
                    //override amt
                    vm1.AddBalance = tBal;
                    vm1.WBalance = tBal2;

                    vm1.Amount = tBal2;
                }

                if (vm1.Token != null && vm.selectedNetwork.HasValue && vm1.Token.SupportedCoin.Any(x => x.RelatedNetwork.SupportedNetworkId == vm.selectedNetwork.Value))
                {

                    vm1.IB ??= new InfoBag();
                    vm1.IB.NetFee = GetTokenManager().GetTokensNetWorkFee(vm1.Token.TokenId, vm.selectedNetwork.Value);
                    vm1.ActionMethod = "netSummary";
                }
            }
            ModelState.Clear();
            //return View("summary", vm);
        }
        catch (Exception ex)
        {
            Console2.WriteLine_RED($"mtype caused error:{ex.GetDeepMsg()}");
            vm1.ActionMethod = "Index";
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
        return View("Index", vm1);
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Currency(vmWithdraw vm)
    {
        var vm1 = vm.ToClone(appSessionManager);
        var wm = GetWalletManager();
        try
        {
            var dm = GetDepositManager();
            vm1.TokenList = await dm.GetAllActiveCryptoTokens();
            vm1.CurrencyList = await dm.GetFiatTokensList();
            if (vm.selectedCoin != null)
            {
                var coin = vm1.CurrencyList.FirstOrDefault(x => x.token.TokenId == vm.selectedCoin);
                vm1.IsFiat = coin != null;
                if (coin.token.Code.ToUpper() == "INR")
                {
                    vm1 = await dm.PoplateINRAndMyPayOptions(vm1);
                    if (vm1.IsBankDeposits)
                    {
                        vm1.IB.INRTechnoAppOption.selectedBankDeposits ??= vm1.IB.INRTechnoAppOption.BankDeposits.First();

                        var ss = vm1.IB.INRTechnoAppOption.selectedBankDeposits;

                        vm1.FiatWithdrawFee = vm1.IB.INRTechnoAppOption.selectedBankDeposits.WithdrawlFee;
                        vm1.IB.INRUserOptions.selBankDeposits = vm1.IB.INRUserOptions.BankDeposits.FirstOrDefault();

                        var wBal = await wm.GetCoinBalInMyWallets(coin.token.TokenId);
                        // var tBal = wBal.EarnWallet != null ? wBal.EarnWallet.Amount : 0;
                        var tBal = wBal.SpotWallet != null ? wBal.SpotWallet.Amount : 0;

                        var tBal2 = wBal.FundWallet != null ? wBal.FundWallet.Amount : 0;
                        vm1.AddBalance = tBal;
                        if (vm1.IsAll)
                        {
                            //override amt
                            vm1.Amount = tBal + tBal2;
                        }
                        else
                        {
                            if (!(vm1.Amount <= tBal2))
                                throw new ApplicationException($"Max balance available is {tBal2}");
                        }

                        if (!(vm1.Amount > 0))
                            throw new ApplicationException($"Invalid Amount {vm1.Amount}");
                        if (!(vm1.Amount <= tBal2))
                            throw new ApplicationException($"Max available balance is {tBal2}");
                        if (!(vm1.Amount <= ss.MaxWithdrawl))
                            throw new ApplicationException($"Max single transaction limit is {ss.MaxWithdrawl}");
                        if (!(vm1.Amount >= ss.MinWithdrawl))
                            throw new ApplicationException($"Min single transaction limit is {ss.MinWithdrawl}");
                        if (!((vm1.Amount - ss.WithdrawlFee) > 0))
                            throw new ApplicationException($"Min withdraw is {(ss.WithdrawlFee + vm1.Amount)}");
                    }

                    if (vm1.IsUPI)
                    {
                        //TechnoApp
                        vm1.IB.INRTechnoAppOption.selectedUPI ??= vm1.IB.INRTechnoAppOption.UPI.First();
                        var ssUPI = vm1.IB.INRTechnoAppOption.selectedUPI;
                        vm1.FiatWithdrawFee = ssUPI.WithdrawlFee;
                        //User
                        vm1.IB.INRUserOptions.selUPI = vm1.IB.INRUserOptions.UPI.FirstOrDefault();

                        var wBal = await wm.GetCoinBalInMyWallets(coin.token.TokenId);
                        if (vm1.IsAll)
                        {
                            //var tBal = wBal.EarnWallet != null ? wBal.EarnWallet.Amount : 0;
                            var tBal = wBal.FundWallet != null ? wBal.FundWallet.Amount : 0;
                            tBal += wBal.SpotWallet != null ? wBal.SpotWallet.Amount : 0;
                            //override amt
                            vm1.Amount = tBal;
                        }
                        var tBal2 = wBal.FundWallet != null ? wBal.FundWallet.Amount : 0;
                        if (!(vm1.Amount > 0))
                            throw new ApplicationException($"Invalid Amount {vm1.Amount}");
                        if (!(vm1.Amount <= tBal2))
                            throw new ApplicationException($"Max balance available is {tBal2}");
                        if (!(vm1.Amount <= ssUPI.MaxWithdrawl))
                            throw new ApplicationException($"Max single transaction limit is {ssUPI.MaxWithdrawl}");
                        if (!(vm1.Amount >= ssUPI.MinWithdrawl))
                            throw new ApplicationException($"Min single transaction limit is {ssUPI.MinWithdrawl}");

                        if (!((vm1.Amount - ssUPI.WithdrawlFee) > 0))
                            throw new ApplicationException($"Min withdraw is {(ssUPI.WithdrawlFee + vm1.Amount)}");
                    }
                    vm1.isBack = "mtype";
                    vm1.ActionMethod = "fSummary";
                }
            }
            ModelState.Clear();
            return View("fiattransferprocess", vm1);
        }
        catch (Exception ex)
        {
            vm1.ActionMethod = "Currency";
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
        return View("Index", vm1);
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> NetSummary(vmWithdraw vm)
    {
        var vm1 = vm.ToClone(appSessionManager);
        try
        {
            var dm = GetDepositManager();
            var wm = GetWalletManager();
            var am = GetAddBookManager();
            vm1.TokenList = await dm.GetAllActiveCryptoTokens();
            vm1.CurrencyList = await dm.GetFiatTokensList();
            vm1.Token = vm1.TokenList.FirstOrDefault(x => x.TokenId == vm.selectedCoin);
            vm1.IsToken = vm1.Token != null;
            if (vm1.Token != null)
                vm1.SupportedNetwork = (vm1.Token.SupportedCoin.Select(x => x.RelatedNetwork)).ToList();

            if (vm1.Token != null && vm.selectedNetwork.HasValue && vm1.Token.SupportedCoin.Any(x => x.RelatedNetwork.SupportedNetworkId == vm.selectedNetwork.Value))
            {

                vm1.IB ??= new InfoBag();
                vm1.IB.NetFee = GetTokenManager().GetTokensNetWorkFee(vm1.Token.TokenId, vm.selectedNetwork.Value);
                vm1.ReceiverAddr.CheckAndThrowNullArgumentException(ArguName: "Receiver Address");
                //ToDo:validate Eth Network Address
                //vm1.ReceiverAddr
                if (vm1.AddToAddressBook)
                {
                    am.AddToMyNetWhiteList(vm.selectedNetwork.Value, vm1.ReceiverAddr);
                }
                var wBal = await wm.GetCoinBalInMyWallets(vm1.Token.TokenId);
                //var tBal = wBal.EarnWallet != null ? wBal.EarnWallet.Amount : 0;
                var tBal = wBal.SpotWallet != null ? wBal.SpotWallet.Amount : 0;

                var tBal2 = wBal.FundWallet != null ? wBal.FundWallet.Amount : 0;
                vm1.AddBalance = tBal;
                vm1.WBalance = tBal2;
                if (vm1.IsAll)
                {
                    //override amt
                    vm1.Amount = tBal + tBal2;

                }
                else
                  if (!(vm1.Amount <= tBal2))
                    throw new ApplicationException($"Max balance available is {tBal2}");

                if (!(vm1.Amount > 0))
                    throw new ApplicationException($"Invalid Amount {vm1.Amount}");

                if (!(vm1.Amount <= vm1.IB.NetFee.MaxWithdrawal))
                    throw new ApplicationException($"Max single transaction limit is {vm1.IB.NetFee.MaxWithdrawal}");
                if (!(vm1.Amount > vm1.IB.NetFee.MinWithdrawal))
                    throw new ApplicationException($"Min single transaction limit is {vm1.Amount}");

                var bag = new mWithDrawBag() { IsToken = true, infoBag = vm1.TomWithdrawNetBag().ToJson() };
                vm1.inforBag = _protector.Protect(bag.ToJson());
                vm1.ActionMethod = "Verify2Confirm_";
            }
            vm1.isBack = "mtype";
            ModelState.Clear();
            return View("summary", vm1);
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
            vm1.ActionMethod = "netSummary";
        }
        return View("Index", vm1);
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> fSummary(vmWithdraw vm)
    {
        var vm1 = vm.ToClone(appSessionManager);
        try
        {
            var dm = GetDepositManager();
            var wm = GetWalletManager();
            var am = GetAddBookManager();
            vm1.TokenList = await dm.GetAllActiveCryptoTokens();
            vm1.CurrencyList = await dm.GetFiatTokensList();
            if (vm.selectedCoin != null)
            {
                vm1.isBack = "mtype";
                var coin = vm1.CurrencyList.FirstOrDefault(x => x.token.TokenId == vm.selectedCoin);
                vm1.IsFiat = coin != null;
                if (coin.token.Code.ToUpper() == "INR")
                {
                    vm1.Symbole = coin.Symbole;
                    vm1.Token = coin.token;
                    vm1 = await dm.PoplateINRAndMyPayOptions(vm1);
                    if (vm1.IsBankDeposits)
                    {
                        vm1.IB.INRTechnoAppOption.selectedBankDeposits ??= vm1.IB.INRTechnoAppOption.BankDeposits.First();
                        // var ssUPI = vm1.IB.INRTechnoAppOption.selectedBankDeposits;

                        vm1.FiatWithdrawFee = vm1.IB.INRTechnoAppOption.selectedBankDeposits.WithdrawlFee;
                        vm1.IB.INRUserOptions.selBankDeposits ??= vm1.IB.INRUserOptions.BankDeposits.FirstOrDefault();
                        vm1.IB.INRUserOptions.selBankDeposits = vm1.IB.INRUserOptions.BankDeposits.FirstOrDefault(x => x.ID == vm1.IB.INRUserOptions.selBankDeposits.ID);


                        var bag = new mWithDrawBag() { IsBank = true, infoBag = vm1.TomWithdrawINRBankBag().ToJson() };
                        vm1.inforBag = _protector.Protect(bag.ToJson());
                    }

                    if (vm1.IsUPI)
                    {
                        //TechnoApp
                        vm1.IB.INRTechnoAppOption.selectedUPI ??= vm1.IB.INRTechnoAppOption.UPI.First();
                        vm1.IB.INRTechnoAppOption.selectedUPI = vm1.IB.INRTechnoAppOption.UPI.FirstOrDefault(x => x.ID == vm1.IB.INRTechnoAppOption.selectedUPI.ID);

                        var ssUPI = vm1.IB.INRTechnoAppOption.selectedUPI;
                        vm1.FiatWithdrawFee = ssUPI.WithdrawlFee;
                        //User
                        vm1.IB.INRUserOptions.selUPI ??= vm1.IB.INRUserOptions.UPI.FirstOrDefault();
                        vm1.IB.INRUserOptions.selUPI = vm1.IB.INRUserOptions.UPI.FirstOrDefault(x => x.ID == vm1.IB.INRUserOptions.selUPI.ID);

                        var bag = new mWithDrawBag() { IsUPI = true, infoBag = vm1.TomWithdrawINRUPIBag().ToJson() };
                        vm1.inforBag = _protector.Protect(bag.ToJson());
                    }
                    vm1.ActionMethod = "Verify2Confirm_";
                }
            }
            vm1.isBack = "Currency";
            ModelState.Clear();
            return View("summary", vm1);
        }
        catch (Exception ex)
        {
            vm1.ActionMethod = "Currency";
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
        return View("fiattransferprocess", vm1);
    }
    [HttpPost]
    [AfterProfile]
    [ActionName(nameof(Verify2Confirm_))]
    public async Task<IActionResult> Verify2Confirm_(vmWithdraw vm)
    {
        try
        {
            //ToDo: Check if User is eligible for multi factor
            //Else redirect to setup
            //Redirect to Multi Factor
            var sm = GetSettingsManager();
            var vm2 = vmFactory.Getvm2ndAuth(appSessionManager);
            vm2.ActionMethod = "Verify2Confirm";
            await sm.RequestEmailOTP(vm2._UserName);
            vm2.infoBag = vm.inforBag;
            ModelState.Clear();
            return View("Auth2F", vm2);
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
        return View("Auth2F", vm);
    }

    [HttpPost]
    [AfterProfile]
    [ActionName(nameof(Verify2Confirm))]
    public async Task<IActionResult> Verify2Confirm(vm2ndAuth vm)
    {
        var vm3 = vmFactory.Getvm2ndAuth(appSessionManager);
        try
        {
            //INR 2F verification STEP
            var sm = GetSettingsManager();
            var wm = GetWalletManager();
            var ua = appSessionManager.ExtSession.UserSession.UserAccount;
            vm3.ActionMethod = "Verify2Confirm";
            vm3.infoBag = vm.infoBag;
            var isValid = sm.Verify2Fact(ua.Authenticator.Code, vm.Code);
            isValid = isValid && await sm.VerifyEmailOTP(appSessionManager.mySession.UserName, vm.OTP);
            if (isValid)
            {
                var vm2 = System.Text.Json.JsonSerializer.Deserialize<mWithDrawBag>(_protector.Unprotect(vm.infoBag));
                if (vm2.IsBank)
                {
                    var vmT = System.Text.Json.JsonSerializer.Deserialize<mWithdrawINRBankBag>(vm2.infoBag);
                    var result = await wm.SendTokenToExternal_Withdraw(vmT);
                    ModelState.Clear();
                    var ret = await vmFactory.GetvmWithdrawConfirmation(appSessionManager);
                    ret.Result = result;
                    return View("Confirmation", ret);
                }
                else if (vm2.IsUPI)
                {
                    var vmT = System.Text.Json.JsonSerializer.Deserialize<mWithdrawINRUPIBag>(vm2.infoBag);
                    var result = await wm.SendTokenToExternal_Withdraw(vmT);
                    ModelState.Clear();
                    var ret = await vmFactory.GetvmWithdrawConfirmation(appSessionManager);
                    ret.Result = result;
                    return View("Confirmation", ret);
                }
                else if (vm2.IsToken)
                {
                    var vmT = System.Text.Json.JsonSerializer.Deserialize<mWithdrawNetBag>(vm2.infoBag);
                    var result = await wm.SendTokenToExternal_Withdraw(vmT);
                    ModelState.Clear();
                    var ret = await vmFactory.GetvmWithdrawConfirmation(appSessionManager);
                    ret.Result = result;
                    return View("Confirmation", ret);

                }
                else
                {
                    Console2.WriteLine_RED($"Verify2Confirm ERROR:Invalid Data Detected. ACTION STOPPED");
                    throw new ApplicationException("One or more Technical checks have prevented this Action.Try again later");
                }
            }
            else
            {
                throw new ApplicationException("Invalid Code or OTP");
            }
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
            return View("Auth2F", vm3);
        }
        ModelState.Clear();
        return View("Auth2F", vm3);
    }

    [HttpGet]
    public async Task<IActionResult> WithDrawFee(Guid tid, Guid nid)
    {
        return Json(GetTokenManager().GetTokensNetWorkFee(tid, nid));
    }


    //add by kavita
    [HttpGet]
    public async Task<IActionResult> summary()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("summary", vm);
    }
    //add by kavita
    [HttpGet]
    public async Task<IActionResult> factorverification()
    {

        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("factorverification", vm);
    }
    //add by kavita
    [HttpGet]
    public async Task<IActionResult> confirmation()
    {

        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("confirmation", vm);
    }
    //add by kavita
    [HttpGet]
    public async Task<IActionResult> withdrawfiat()
    {

        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("withdrawfiat", vm);
    }
    //add by kavita
    [HttpGet]
    public async Task<IActionResult> fiatchooseaccount()
    {

        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("fiatchooseaccount", vm);
    }
    //add by kavita
    [HttpGet]
    public async Task<IActionResult> fiattransferprocess()
    {

        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("fiattransferprocess", vm);
    }

    //add by kavita
    [HttpGet]
    public async Task<IActionResult> fiatsummary()
    {

        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("fiatsummary", vm);
    }
    //add by kavita
    [HttpGet]
    public async Task<IActionResult> fiat2factorverification()
    {

        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("fiat2factorverification", vm);
    }
    //add by kavita
    [HttpGet]
    public async Task<IActionResult> fiatconfirmation()
    {

        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("fiatconfirmation", vm);
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
    internal WithdrawManager GetDepositManager()
    {
        var Mgr = new WithdrawManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        appSessionManager.ExtSession.LoadSession().GetAwaiter().GetResult();
        return Mgr;
    }
    internal TokenManager GetTokenManager()
    {
        var Mgr = new TokenManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        appSessionManager.ExtSession.LoadSession().GetAwaiter().GetResult();
        return Mgr;
    }
    internal AddBookManager GetAddBookManager()
    {
        var Mgr = new AddBookManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        appSessionManager.ExtSession.LoadSession().GetAwaiter().GetResult();
        return Mgr;
    }
    internal SettingsManager GetSettingsManager()
    {

        var Mgr = new SettingsManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        Mgr._DataProtector = _protector;
        if (!appSessionManager.ExtSession.IsLoaded)
            appSessionManager.ExtSession.LoadSession().GetAwaiter().GetResult();

        return Mgr;
    }
}
