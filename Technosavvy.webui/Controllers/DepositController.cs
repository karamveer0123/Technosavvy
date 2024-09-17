using NuGet.Protocol;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace TechnoApp.Ext.Web.UI.Controllers;
[AfterProfile]
[DenyAccessInPreBeta]
[EnsureCompliantCountry]
public class DepositController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IOptions<SmtpConfig> _smtp;
    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;

    public DepositController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;
        _smtp = smtp;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);

    }
    [HttpGet]
    public async Task<IActionResult> ClaimTran()
    {
        var vm = await vmFactory.GetvmClaimTranDeposit(appSessionManager);
        vm.status = vmTranStatus.NewClaim;
        var wm = GetWalletManager();
        vm.NetworkList = await wm.GetAllSupportedNetwork();
        return View("CliamTran_v2", vm);
    }
    [HttpPost]
    public async Task<IActionResult> ClaimTran(vmClaimTranDeposit vm)
    {
        try
        {
            vm.TranHash.CheckAndThrowNullArgumentException();
            vm.NetworkId.CheckAndThrowNullArgumentException();
            if (vm.TranHash.ToLower().StartsWith("0x"))
            {
                //ToDo:Naveen,Confirm if this Transaction is already Credited to this User
                //-Set Status = ClaimAlreadyActioned

                //ToDo:Naveen,If No, Check if Tx Hash has been previously Investigated
                //-Set Status = ClaimAlreadyActioned
                //Set Msg
                var wm = GetWalletManager();


                //If No, Raise Request for this Tx Hash for this User and Make Record of such Request
                var dm = GetDepositManager();
                var myWall = await dm.GetMyNetworkWallet(vm.NetworkId);
                var res = await dm.GetOnDemandCheckNetworkTx(myWall.Address, vm.NetworkId, vm.TranHash);
                vm.status = res <= Model.mOnDemandRequestResult.Placed ? vmTranStatus.ClaimAccepted : vmTranStatus.ClaimAlreadyActioned;
                var vm2 = await vmFactory.GetvmClaimTranDeposit(appSessionManager);
                vm2.status = vm.status;
                vm2.TranHash = vm.TranHash;
                vm2.NetworkId = vm.NetworkId;
                switch (res)
                {
                    case Model.mOnDemandRequestResult.Placed:
                        vm2.Msg = "Request has been Placed to Claim this Transaction. Please allow us upto 6 Hrs to Process";
                        break;
                    case Model.mOnDemandRequestResult.NoIssue:
                        vm2.Msg = "Request has been Placed to Claim this Transaction. Please allow us upto 6 Hrs to Process";
                        break;
                    case Model.mOnDemandRequestResult.DailyLimitIssue:
                        vm2.Msg = "You have reached your Daily Unclaimed Limit. Please Try again later.";
                        break;
                    case Model.mOnDemandRequestResult.TotalLimitIssue:
                        vm2.Msg = "You have reached your Total Unclaimed Limit. Please Try again later.";
                        break;
                    case Model.mOnDemandRequestResult.AlreadyClaimed:
                        vm2.Msg = "This Transaction has already Claimed.";
                        break;
                    case Model.mOnDemandRequestResult.AlreadyAwaited:
                        vm2.Msg = "This Transaction is currently awaiating processing.";

                        break;
                    default:
                        break;
                }

                return View("CliamTran_v2", vm2);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Occoured in ClaimTran..at:{DateTime.UtcNow.GetCurrentUnix()}\n\t{ex.GetDeepMsg()}");
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
        return View("CliamTran", vm);
    }

    [HttpGet]
    public async Task<IActionResult> Index(string code)
    {
        var vm = await vmFactory.GetvmDeposit(appSessionManager);
        var tm = GetDepositManager();
        vm.TokenList = await tm.GetAllActiveCryptoTokens();
        vm.CurrencyList = await tm.GetFiatTokensList();
        if(code.IsNOT_NullorEmpty())
        {
            var c = vm.TokenList.FirstOrDefault(x => x.Code.ToLower() == code.ToLower());
            var d= vm.CurrencyList.FirstOrDefault(x => x.token.Code.ToLower() == code.ToLower());
            if (c != null)
                vm.selectedCoin = c.TokenId;
            if (d != null)
            {
                vm.selectedCoin = d.token.TokenId;
                vm.Symbole = d.Symbole;
            }
        }
        ModelState.Clear();
        vm.ActionMethod = "Currency";//In case Post is used
        return View("Index", vm);
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Currency(vmDeposit vm)
    {
        var vm1 = vm.ToClone(appSessionManager);
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
                    vm1.ActionMethod = "CurrencyOpt";
                }
            }
            ModelState.Clear();
            return View("Index", vm1);
        }
        catch (Exception ex)
        {
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
    public async Task<IActionResult> CurrencyOpt(vmDeposit vm)
    {
        var vm1 = vm.ToClone(appSessionManager);
        try
        {
            vm1.isBack = "Currency";
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
                    if (vm1.IsUPI)
                    {
                        vm1.ActionMethod = "UPISelect";
                        vm1.IB.INRUserOptions.selUPI = vm1.IB.INRUserOptions.UPI.FirstOrDefault();
                    }
                    else
                    {
                        vm1.ActionMethod = "BDSelect";
                        vm1.IB.INRUserOptions.selBankDeposits = vm1.IB.INRUserOptions.BankDeposits.FirstOrDefault();
                    }
                }
                vm1.Amount.CheckAndThrowNullArgumentException(ArguName: "Amount");
            }
            ModelState.Clear();
            return View("INRStep3", vm1);
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
    public async Task<IActionResult> BDSelect(vmDeposit vm)
    {
        var dm = GetDepositManager();
        var vm1 = vm.ToClone(appSessionManager);
        try
        {
            vm1.isBack = "CurrencyOpt";


            vm1.TokenList = await dm.GetAllActiveCryptoTokens();
            vm1.CurrencyList = await dm.GetFiatTokensList();
            if (vm.selectedCoin != null)
            {
                var coin = vm1.CurrencyList.FirstOrDefault(x => x.token.TokenId == vm.selectedCoin);
                vm1.IsFiat = coin != null;
                if (coin.token.Code.ToUpper() == "INR")
                {
                    vm1 = await dm.PoplateINRAndMyPayOptions(vm1);
                    vm1.ActionMethod = "BDSelectConfim";
                }
            }
            ModelState.Clear();
            vm1.inforBag = _protector.Protect(vm1.IB.ToJson());
            return View("INRStep4", vm1);
        }
        catch (Exception ex)
        {
            vm1.ActionMethod = "CurrencyOpt";
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
        return View("INRStep3", vm1);
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> BDSelectConfim(vmDeposit vm)
    {
        var dm = GetDepositManager();
        var vm1 = vm.ToClone(appSessionManager);
        try
        {
            vm1.isBack = "BDSelect";
            vm1.IB = System.Text.Json.JsonSerializer.Deserialize<InfoBag>(_protector.Unprotect(vm.inforBag));

            vm1.TokenList = await dm.GetAllActiveCryptoTokens();
            vm1.CurrencyList = await dm.GetFiatTokensList();
            if (vm.selectedCoin != null)
            {
                var coin = vm1.CurrencyList.FirstOrDefault(x => x.token.TokenId == vm.selectedCoin);
                vm1.IsFiat = coin != null;
                if (coin.token.Code.ToUpper() == "INR")
                {
                    vm1 = await dm.PoplateINRAndMyPayOptions(vm1);
                    vm1.ActionMethod = "BDUploadReceipt";
                }
            }
            ModelState.Clear();
            return View("INRStep6", vm1);
        }
        catch (Exception ex)
        {
            vm1.ActionMethod = "UPISelect";
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
        return View("INRStep4", vm1);
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> BDUploadReceipt(vmDeposit vm)
    {
        var dm = GetDepositManager();
        var vm1 = vm.ToClone(appSessionManager);
        try
        {
            vm1.isBack = "BDSelectConfim";
            vm1.IB = System.Text.Json.JsonSerializer.Deserialize<InfoBag>(_protector.Unprotect(vm.inforBag));
            vm.Receipt.CheckAndThrowNullArgumentException(ArguName: "Receipt");
            vm.Receipt.CheckAndThrowNullArgumentException(true, ArguName: "Receipt");
            vm.Receipt.CheckAndThrowNullArgumentException(250, ArguName: "Receipt");
           
            var lst = await dm.GetFiatTokensList();
            var coin = lst.FirstOrDefault(x => x.token.TokenId == vm.selectedCoin);
            //Validate And Save Information
            var vm2 = vm1.TomFiatDepositIntimation(appSessionManager);
            vm2.DepositEvidence = dm.ImageString(vm.Receipt);
            vm2.CurrencySymbole = coin.Symbole;
            vm2.CurrencyCode = coin.token.Code;
            var id = await dm.IntimateINRDeposit(vm2);
            if (id != Guid.Empty)
            {
                vm1.ActionMethod = vm2.PublicRequestID;
            }

            ModelState.Clear();
            //Show Confirmation
            return View("INRStep5", vm1);
        }
        catch (Exception ex)
        {
            vm1.ActionMethod = "UPIUploadReceipt";
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
        return View("INRStep6", vm1);
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> UPISelect(vmDeposit vm)
    {
        var dm = GetDepositManager();
        var vm1 = vm.ToClone(appSessionManager);
        try
        {
            vm1.isBack = "CurrencyOpt";


            vm1.TokenList = await dm.GetAllActiveCryptoTokens();
            vm1.CurrencyList = await dm.GetFiatTokensList();
            if (vm.selectedCoin != null)
            {
                var coin = vm1.CurrencyList.FirstOrDefault(x => x.token.TokenId == vm.selectedCoin);
                vm1.IsFiat = coin != null;
                if (coin.token.Code.ToUpper() == "INR")
                {
                    vm1 = await dm.PoplateINRAndMyPayOptions(vm1);
                    vm1.ActionMethod = "UPISelectConfim";
                }
            }
            ModelState.Clear();
            vm1.inforBag = _protector.Protect(vm1.IB.ToJson());
            return View("INRStep4", vm1);
        }
        catch (Exception ex)
        {
            vm1.ActionMethod = "CurrencyOpt";
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
        return View("INRStep3", vm1);
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> UPISelectConfim(vmDeposit vm)
    {
        var dm = GetDepositManager();
        var vm1 = vm.ToClone(appSessionManager);
        try
        {
            vm1.isBack = "UPISelect";
            vm1.IB = System.Text.Json.JsonSerializer.Deserialize<InfoBag>(_protector.Unprotect(vm.inforBag));

            vm1.TokenList = await dm.GetAllActiveCryptoTokens();
            vm1.CurrencyList = await dm.GetFiatTokensList();
            if (vm.selectedCoin != null)
            {
                var coin = vm1.CurrencyList.FirstOrDefault(x => x.token.TokenId == vm.selectedCoin);
                vm1.IsFiat = coin != null;
                if (coin.token.Code.ToUpper() == "INR")
                {
                    vm1 = await dm.PoplateINRAndMyPayOptions(vm1);
                    vm1.ActionMethod = "UPIUploadReceipt";
                }
            }
            ModelState.Clear();
            return View("INRStep6", vm1);
        }
        catch (Exception ex)
        {
            vm1.ActionMethod = "UPISelect";
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
        return View("INRStep4", vm1);
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> UPIUploadReceipt(vmDeposit vm)
    {
        var dm = GetDepositManager();
        var vm1 = vm.ToClone(appSessionManager);
        try
        {
            vm1.isBack = "UPISelectConfim";
            vm1.IB = System.Text.Json.JsonSerializer.Deserialize<InfoBag>(_protector.Unprotect(vm.inforBag));
            vm.Receipt.CheckAndThrowNullArgumentException("UPI Deposit Receipt must be provided to Process");
            vm.Receipt.CheckAndThrowNullArgumentException(true, ArguName: "Receipt");
            vm.Receipt.CheckAndThrowNullArgumentException(250, ArguName: "Receipt");
            var lst = await dm.GetFiatTokensList();
            var coin = lst.FirstOrDefault(x => x.token.TokenId == vm.selectedCoin);
            
            //Validate And Save Information
            var vm2 = vm1.TomFiatDepositIntimation(appSessionManager);
            vm2.DepositEvidence = dm.ImageString(vm.Receipt);
            vm2.CurrencySymbole = coin.Symbole;
            vm2.CurrencyCode = coin.token.Code;
            var id = await dm.IntimateINRDeposit(vm2);
            if(id!=Guid.Empty)
            {
                vm1.ActionMethod = vm2.PublicRequestID;
            }
            ModelState.Clear();
            //Show Confirmation
            return View("INRStep5", vm1);
        }
        catch (Exception ex)
        {
            vm1.ActionMethod = "UPIUploadReceipt";
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
        return View("INRStep6", vm1);
    }

    [HttpPost]
    public async Task<IActionResult> SelectCoin(vmDeposit vm)
    {
        var tm = GetDepositManager();
        vm.TokenList = await tm.GetAllActiveCryptoTokens();
        vm.CurrencyList = await tm.GetFiatTokensList();

        return PartialView("partial/_SelectAssetStep2", vm);
    }
    public async Task<IActionResult> GetMyNetWallet(Guid networkid)
    {
        //Check and Get my Network Wallet
        var dm = GetDepositManager();
        var nWallet = await dm.GetMyNetworkWallet(networkid);
        if (nWallet != null)
            return Json(nWallet.ToJson());
        else
            return Json(false);

    }
    public async Task<IActionResult> GenerateMyNetWallet(Guid networkid)
    {
        //Check and Get my Network Wallet
        var dm = GetDepositManager();
        var nWallet = await dm.RequestToGenerateMyNetworkWallet(networkid);
        return Json(nWallet.ToJson());
        //if (nWallet != null)
        //    return Json(nWallet.ToJson());
        //else
        //    return Json(null);

    }
    public async Task<IActionResult> depositmeta()
    {
        StringBuilder sb = new StringBuilder();
        var tm = GetDepositManager();
        var vm = new vmDeposit();
        vm.TokenList = await tm.GetAllActiveCryptoTokens();
        vm.CurrencyList = await tm.GetFiatTokensList();


        //foreach (var item in vm)
        //{
        //    sb.Append($"<option data-Img-src=\"../Images/{item.Code}.png\" data-tick=\"{item.Tick}\" data-code=\"{item.Code}\" value=\" {item.FiatCurrencyId}\">{item.Code} - ({item.Name}) </option>");

        //}
        return Json(vm.ToJson());
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
    internal WalletManager GetWalletManager()
    {
        var Mgr = new WalletManager();
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
    //add by kavita
    [HttpGet]
    public async Task<IActionResult> DepositFiat()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("depositfiat", vm);
    }

    //add by kavita
    [HttpGet]
    public async Task<IActionResult> choosebankaccount()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("choosebankaccount", vm);
    }
    //add by kavita
    [HttpGet]
    public async Task<IActionResult> transferproceed()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("transferproceed", vm);
    }
    //add by kavita
    [HttpGet]
    public async Task<IActionResult> requestsubmited()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("requestsubmited", vm);
    }
    //add by kavita
    [HttpGet]
    public async Task<IActionResult> transferproceedupi()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("transferproceedupi", vm);
    }
    //add by kavita
    [HttpGet]
    public async Task<IActionResult> uploadreciept()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("uploadreciept", vm);
    }
}
