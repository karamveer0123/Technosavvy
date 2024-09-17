using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Runtime;
namespace TechnoApp.Ext.Web.UI.Controllers;
[AfterLogIn]
[DenyAccessInPreBeta]
[EnsureCompliantCountry]
public class SettingsController : baseController
{
    private readonly ILogger<SettingsController> _logger;
    //private IOptions<SmtpConfig> _smtp;
    //private IConfiguration _configuration;
    //private HttpContext _context;
    //private IHttpContextAccessor _accessor;
    //IDataProtector _protector;
    //AppSessionManager appSessionManager = null;
    public SettingsController(ILogger<SettingsController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {

        _logger = logger;
        _smtp = smtp;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);
    }

    public async Task<IActionResult> Index()
    {
        var sm = await SettingsManager.Instance(this);
        var vm = await sm.GetSettings();
        //var result = await (await GetSettingsManager()).GetSettings();
        return View(vm);
    }
    public async Task<IActionResult> Test()
    {
        var sett = GetSettingsManager();
        var r = await sett.GetSettings();
        var res = r.MultiFactor;
        return PartialView("partial/_GAuth", res);


    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(vmSettings vm)
    {
        //ToDo: Naveen Setting update View
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ProfileSetting(vmSettingProfile vm)
    {
        var result = GetSettingsManager();
        var mm = GetKYCManager();
        var vm1 = mm.GetProfile(vm.UserId).Result;
        vm1.NickName = vm.UserNicName;
        var isSave = mm.UpdateProfilePersonalInfo(vm1);
        //if (isSave)
        GUtilityManager.MessageToaster(this, "Nic Name", "Your Nic name changed successfuly.", "success");
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MultiFactorEnabled(vmSettingsMultiFactorMain vm)
    {
        var sm = GetSettingsManager();
        vm = await sm.IsMultiFactorEnabled(vm);
        return PartialView("partial/_GAuth", vm);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MultiFactorDisabled(vmSettingsMultiFactorMain vm)
    {
        var result = GetSettingsManager();
        var isDis = await result.IsMultiFactorDisabled(vm.Email);
        // var settingVm = result.GetSettings().Result;
        // settingVm.MultiFactor = vm;
        GUtilityManager.MessageToaster(this, "Multi Factor", "Your multi factor disabled successfuly", "success");
        return PartialView("partial/_GAuth", vm);

        //return View("Settings", settingVm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TwoFactorAuthenticate(vmSettingsMultiFactorMain vm)
    {
        var sm = GetSettingsManager();
        //var settingVm =await  sm.GetSettings();
        var res = sm.Verify2Fact(vm.SetUPAuthenticator);
        if (res)
        {
            var rs = await sm.EnableMultiFactor(vm.Email);
            sm.UpdateGCodeFor2Factor(vm);
            vm.IsAuthenticator = true;
            vm.IsSetUPAuthenticator = false;
        }
        else
        {
            vm.AuthErrorMsg = "Invalid Auth Key";
            return PartialView("partial/_GAuth", vm);

            // GUtilityManager.MessageToaster(this, "LogIn", "You are not a vaid user", "error");
        }

        return PartialView("partial/_GAuth", vm);

        // return View("Settings", settingVm);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TwoFactorAuthenticateVerify(vmSettingsAuth vm)
    {

        var result = GetSettingsManager();
        vmSettingsAuthenticator vm1 = new vmSettingsAuthenticator() { Code = vm.Code, OTP = vm.OTP };
        var res = result.Verify2Fact(vm1);
        if (res)
        {
            GUtilityManager.MessageToaster(this, "LogIn", "You are login successfuly", "success");
            return RedirectToAction("Wallet", "Index");
        }
        else
        {
            GUtilityManager.MessageToaster(this, "LogIn", "You are not a vaid user", "error");
            return RedirectToAction("Login", "Login");
        }
    }


    [HttpGet]
    [AfterProfile]
    [ActionName(nameof(AddPaymentMethod))]
    public async Task<IActionResult> AddPaymentMethod()
    {
        var sm = GetSettingsManager();
        var vm = await vmFactory.GetvmPaymentMethodSetup(appSessionManager);

        if (!vm._Is2FAEnabled)
            return RedirectToAction("paymentdeposit", "settings");

        vm = await sm.GetPaymentMethodsSetupVM(vm);
        vm.ActionMethod = "SetPM";
        return View("AddPaymentMethod", vm);
    }
    [HttpPost]
    [AfterProfile]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> SetPM(vmPaymentMethodSetup vm)
    {
        var sm = GetSettingsManager();
        var vm1 = await vmFactory.GetvmPaymentMethodSetup(appSessionManager);
        vm1 = await sm.GetPaymentMethodsSetupVM(vm1);
        if (vm.FiatToken != null && vm.FiatToken.Code.IsNOT_NullorEmpty() && vm1.FiatTokenList.Any(x => x.token.Code == vm.FiatToken.Code))
        {
            vm1.ActionMethod = "ProcessPM";//Next Step
            vm1.FiatToken = vm1.FiatTokenList.First(x => x.token.Code == vm.FiatToken.Code).token;
            ModelState.Clear();
        }
        else
        {
            vm1.ActionMethod = "SetPM";//Repeat This Step
            ModelState.Clear();
            ModelState.AddModelError("", "Please Select Currency");
        }
        return View("AddPaymentMethod", vm1);
    }
    [HttpPost]
    [AfterProfile]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> ProcessPM(vmPaymentMethodSetup vm)
    {
        var sm = GetSettingsManager();
        var str = _accessor.HttpContext.Request.Headers["NSOW"];
        _accessor.HttpContext.Response.Headers.Add("NSOW", Guid.NewGuid().ToString().Replace("-", ""));
        var vm1 = await vmFactory.GetvmPaymentMethodSetup(appSessionManager);
        vm1 = await sm.GetPaymentMethodsSetupVM(vm1);
        if (vm.SelectedPaymentMethod.IsNOT_NullorEmpty() && vm1.ValidPaymentMethods.Any(x => x == vm.SelectedPaymentMethod))
        {
            vm1.ActionMethod = "AddPaymentMethod";//Next Step
            vm1.SelectedPaymentMethod = vm.SelectedPaymentMethod;
            vm1.FiatToken = vm1.FiatTokenList.First(x => x.token.Code == vm.FiatToken.Code).token;
            ModelState.Clear();
        }
        else
        {
            vm1.ActionMethod = "ProcessPM"; //Repeat This Step
            vm1.FiatToken = vm1.FiatTokenList.First(x => x.token.Code == vm.FiatToken.Code).token;
            ModelState.Clear();
            ModelState.AddModelError("", "Please Select Payment Method");
        }
        return View("AddPaymentMethod", vm1);
    }
    [HttpPost]
    [AfterProfile]
    [ActionName(nameof(AddPaymentMethod))]
    public async Task<IActionResult> AddPaymentMethod(vmPaymentMethodSetup vm)
    {
        var sm = GetSettingsManager();
        var vm1 = await vmFactory.GetvmPaymentMethodSetup(appSessionManager);
        try
        {
            vm1 = await sm.GetPaymentMethodsSetupVM(vm1);
            if (vm.SelectedPaymentMethod.IsNOT_NullorEmpty() && vm1.ValidPaymentMethods.Any(x => x == vm.SelectedPaymentMethod))
            {
                vm.ActionMethod = "AddPaymentMethod";
                vm1.SelectedPaymentMethod = vm.SelectedPaymentMethod;
                vm1.FiatToken = vm1.FiatTokenList.First(x => x.token.Code == vm.FiatToken.Code).token;
                ModelState.Clear();
            }
            else
            {
                return RedirectToAction("ProcessPM");
                //if (vm.FiatToken != null && vm.FiatToken.Code.IsNOT_NullorEmpty())
                //    vm.ActionMethod = "ProcessPM";
                //else
                //    vm.ActionMethod = "SetPM";
            }
            ModelState.Clear();
            await sm.ValidateINR(vm);
            var mm = await sm.TovmPaymentMethodToSave(vm);
            //Redirect to Multi Factor
            var vm2 = vmFactory.Getvm2ndAuthPaymentMethod(appSessionManager);
            vm2.ActionMethod = "Verify2Confirm";
            await sm.RequestEmailOTP(vm2._UserName);
            vm2.infoBag = _protector.Protect($"{mm.ToJson()}");
            return View("Auth2F", vm2);
        }
        catch (Exception ex)
        {
            ModelState.Clear();
            ModelState.AddModelError("", ex.Message);
        }
        return View("AddPaymentMethod", vm);
    }
    [HttpPost]
    [AfterProfile]
    [ActionName(nameof(Verify2Confirm))]
    public async Task<IActionResult> Verify2Confirm(vm2ndAuthPaymentMethod vm)
    {
        var vm1 = vmFactory.Getvm2ndAuthPaymentMethod(appSessionManager);
        vm1.infoBag = vm.infoBag;
        vm1.ActionMethod = "Verify2Confirm";
        try
        {
            //INR 2F verification STEP
            var sm = GetSettingsManager();
            var ua = appSessionManager.ExtSession.UserSession.UserAccount;

            var isValid = sm.Verify2Fact(ua.Authenticator.Code, vm.AuthCode);
            isValid = isValid && await sm.VerifyEmailOTP(appSessionManager.mySession.UserName, vm.EmailOTP);
            if (isValid)
            {
                var vm2 = System.Text.Json.JsonSerializer.Deserialize<mPaymentMethodToSave>(_protector.Unprotect(vm.infoBag));

                var ret = await sm.SavePaymentMethodINR(vm2);
                if (ret)
                {
                    if (vm2.SelectedPaymentMethod == "BankTransfer")
                        return RedirectToAction("paymentdeposit");
                    else
                        return RedirectToAction("paymentdepositupi");
                }
                else
                    throw new ApplicationException("One or more Technical checks have prevented this Action.Try again later");
            }
            else
            {
                throw new ApplicationException("Invalid Code or OTP");
            }
        }
        catch (Exception ex)
        {
            ModelState.Clear();
            ModelState.AddModelError("", ex.Message);
        }
        return View("Auth2F", vm1);
    }

    //add by kavita
    [HttpGet]
    [AfterProfile]
    public async Task<IActionResult> accountactivity()
    {
        //ToDo:p2,User Session List page p2
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("accountactivity", vm);
    }
    [HttpGet]
    [AfterProfile]
    public async Task<IActionResult> addressmanagement()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("addressmanagement", vm);
    }

    //[HttpGet]
    //public async Task<IActionResult> devicemanagement()
    //{
    //    var vm = await vmFactory.GetvmBase(appSessionManager);
    //    return View("devicemanagement", vm);
    //}
    [HttpGet]
    [AfterProfile]
    public async Task<IActionResult> paymentdeposit()
    {
        var vm = await vmFactory.GetvmPaymentDeposits(appSessionManager);
        var sm = GetSettingsManager();
        var p = appSessionManager.ExtSession.UserSession.UserAccount.Profile;
        vm.BankDeposits = await sm.GetmyINRBankDeposit(p.ProfileId);
        return View("paymentdeposit", vm);
    }
    [HttpGet]
    [AfterProfile]


    public async Task<IActionResult> DeletebankDeposit(Guid did)
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        var sm = GetSettingsManager();
        await sm.DeleteINRBankDeposit(did);
        return RedirectToAction("paymentdeposit");
    }
    [HttpGet]
    [AfterProfile]

    public async Task<IActionResult> deleteUPI(Guid did)
    {
        var vm = await vmFactory.GetvmPaymentDeposits(appSessionManager);
        var sm = GetSettingsManager();
        await sm.DeleteINRUPISetup(did);
        return RedirectToAction("paymentdepositupi");

    }
    [HttpGet]
    [AfterProfile]

    public async Task<IActionResult> paymentdepositupi()
    {
        var vm = await vmFactory.GetvmPaymentDeposits(appSessionManager);
        var sm = GetSettingsManager();
        var p = appSessionManager.ExtSession.UserSession.UserAccount.Profile;
        vm.UPI = await sm.GetmyINRUPISetup(p.ProfileId);

        return View("paymentdepositupi", vm);
    }
    [HttpGet]
    [AfterProfile]

    public async Task<IActionResult> addressmanagementform()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("addressmanagementform", vm);
    }
    [HttpGet]
    [AfterProfile]

    public async Task<IActionResult> securityverification()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("securityverification", vm);
    }
    //public async Task<IActionResult> twofactorauthentication()
    //{
    //    var vm = await vmFactory.GetvmBase(appSessionManager);
    //    return View("twofactorauthentication", vm);
    //}

    [HttpGet]
    [AfterProfile]
    public async Task<IActionResult> ChangePassword()
    {
        var vm = await vmFactory.GetvmBase(appSessionManager);
        return View("ChangePassword", vm);
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
    internal KYCManager GetKYCManager()
    {
        if (!appSessionManager.ExtSession.IsLoaded)
            appSessionManager.ExtSession.LoadSession().GetAwaiter().GetResult();

        var Mgr = new KYCManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        return Mgr;
    }
    internal TokenManager GetTokenManager()
    {
        if (!appSessionManager.ExtSession.IsLoaded)
            appSessionManager.ExtSession.LoadSession().GetAwaiter().GetResult();
        var Mgr = new TokenManager();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        Mgr._DataProtector = _protector;
        return Mgr;
    }
}
