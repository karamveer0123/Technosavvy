using Microsoft.AspNetCore.Mvc;

namespace TechnoApp.Ext.Web.UI.Controllers
{
    [EnsureCompliantCountry]
    public class SignUpController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IOptions<SmtpConfig> _smtp;
        private IConfiguration _configuration;
        private HttpContext _context;
        private IHttpContextAccessor _accessor;
        IDataProtector _protector;
        AppSessionManager appSessionManager = null;
        public SignUpController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
        {
            _logger = logger;
            _smtp = smtp;
            _configuration = configuration;
            _accessor = accessor;
            _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
            appSessionManager = new AppSessionManager(accessor.HttpContext);

        }
        [HttpGet]
        public async Task<IActionResult> RegisterUser()
        {
            if (await DoesHaveAnActiveSession())
                return RedirectToAction("Index", "Wallet");

            var vm = vmFactory.GetvmRegisterUser(appSessionManager);
            return View("RegisterUser1", vm);
        }
        [HttpGet]
        public async Task<IActionResult> ForgetPassword()
        {
            if (await DoesHaveAnActiveSession())
                return RedirectToAction("Wallet", "Index");
            await appSessionManager.ExtSession.LoadSession();
            var vm = vmFactory.GetvmForgetPassword(appSessionManager);
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckAndRegisterUser(vmRegisterUser VM)
        {
            var vm = vmFactory.GetvmRegisterUser(appSessionManager);
            try
            {
                vmRegisterUserName VmObject = VM.vmRegUserName;
                vm.vmRegUserName = VM.vmRegUserName;

                if (VmObject is null || VmObject.Email.IsNullOrEmpty())
                {
                    vm.vmRegUserName = VM.vmRegUserName;
                    ModelState.AddModelError("", "Enter valid user name.");
                    // GUtilityManager.MessageToaster(this, "User name", "Enter valid user name.", "error");
                    return View("RegisterUser1", vm);
                }
                if (VmObject.Email.IsNOT_NullorEmpty() && VmObject.Email.Length > 51)
                    throw new ApplicationException("Invalid User name attempted");
                var u = VmObject.Email.Split("@");
                if (!(u.Count() == 2 && u.First().Length <= 25 && u.Last().Length <= 25))
                    throw new ApplicationException("Invalid User name attempted");
                //return RedirectToAction("RegisterUser");


                var sm = GetSignUpManager();
                sm.LogEvent("Get: new User Check and send Email Otp");
                var isExist = await sm.GetAnyUser(VmObject.Email);
                Console.WriteLine($"RegisterUser|AlreadyExist:{isExist}");
                if (!isExist)
                {
                    var result = await sm.NewUserCheckAndSendEmailOtp(VmObject.Email, appSessionManager.mySession.RefCode);
                    vm._UserId = result.Item1.ToString();
                    vm.OTPSendStatus = result.Item2;
                    if (vm.OTPSendStatus)
                        GUtilityManager.MessageToaster(this, "User Register", "OTP sent on your email.", "success");
                    else
                        GUtilityManager.MessageToaster(this, "User Register", "Invalid UserName.", "error");

                }
                else
                {
                    ModelState.AddModelError("", "User already exist.Please proceed to login");
                }
                Console.WriteLine($"RegisterUser|Operation Return at..{DateTime.UtcNow.GetCurrentUnix()}");
                return View("RegisterUser1", vm);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"CheckAndRegisterUser Casued an Exception at..{DateTime.UtcNow.GetCurrentUnix()}\n\t{ex.GetDeepMsg()}");
                ModelState.AddModelError("", "Invalid user name.");

            }
            return View("RegisterUser1", vm);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckUserAndSendOtp(vmForgetPassword VM)
        {
            vmRegisterUser VM1 = vmFactory.GetvmRegisterUser(appSessionManager);
            //VM1.vmRegUserName = new vmRegisterUserName();
            VM1.vmRegUserName.Email = VM.Email;
            vmRegisterUserName VmObject = VM1.vmRegUserName;
            if (VM.Email.IsNullOrEmpty())
                return RedirectToAction("Login", "Login");

            var sm = GetSignUpManager();
            sm.LogEvent("Post:: new User Check and send Email Otp");
            var result = await sm.GetAnyUser(VmObject.Email);
            if (result)
            {
                sm.RequestForgetEmailOTP(VmObject.Email);
                VM1.OTPSendStatus = result;
                VM1.ForgetPasswordStatus = true;
                VM = (vmForgetPassword)vmFactory.InitializeBase(VM, appSessionManager);
                GUtilityManager.MessageToaster(this, "Check Email", "OTP sent on your email.", "success");
                return View("RegisterUser1", VM1);
            }
            return RedirectToAction("Login", "Login");
        }

        public async Task<IActionResult> ResendUserOTP(string Email)
        {

            vmRegisterUser VM1 = vmFactory.GetvmRegisterUser(appSessionManager);
            VM1.vmRegUserName = new vmRegisterUserName();
            VM1.vmRegUserName.Email = Email;
            vmRegisterUserName VmObject = VM1.vmRegUserName;
            if (VmObject is null || VmObject.Email.IsNullOrEmpty())
                return RedirectToAction("Login", "Login");

            var result = await GetSignUpManager().UserCheckAndReSendEmailOtp(VmObject.Email);
            if (result)
            {
                VM1.OTPSendStatus = result;
                VM1.ForgetPasswordStatus = true;
                return View("RegisterUser1", VM1);
            }
            return RedirectToAction("Login", "Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEmailOTP(vmRegisterUser VM)
        {
            vmRegisterUserNameOTP VmObject = VM.EmailOTP;

            var sm = GetSignUpManager();
            var result = await sm.NewUserCheckOTPAndConfirmForPassword(VM.vmRegUserName.Email, VmObject.OTP);
            if (result)
            {
                sm.LogEvent("OTP verified successfully");
                VM.OTPVerifyStatus = result;
                GUtilityManager.MessageToaster(this, "OTP Verification", "OTP verified successfully.", "success");
            }
            else
            {
                sm.LogError("OTP verification failed");

                VM.ErrorMessage = "Invalid OTP";
            }
            VM = (vmRegisterUser)vmFactory.InitializeBase(VM, appSessionManager);
            return View("RegisterUser1", VM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetNewUserPassword(vmRegisterUser VM)
        {
            vmRegisterUserPassword VmObject = VM.vmPassword;
            var result = false;
            var msg = "Your account registered successfully !";
            if (VM.ForgetPasswordStatus)
            {
                var sm = GetSignUpManager();
                var user = await sm.GetUserByName(VM.vmRegUserName.Email);
                VM._UserId = user.Id;
                msg = "Your password updated successfully !";
            }
            var gsm = GetSignUpManager();
            gsm.LogEvent("Password updated successfully");
            result = await gsm.CreatePassword(Guid.Parse(VM._UserId), VmObject.Password);

            if (result)
            {
                //All Good, Let us Auto Login this User now
                var vmLogin = vmFactory.GetvmUserLogin(appSessionManager);
                vmLogin.UserName = VM.vmRegUserName.Email;
                vmLogin.Password = VM.vmPassword.Password;
                GUtilityManager.MessageToaster(this, "Account", msg, "success");
                return RedirectToAction("LogIn", "Login");
            }
            VM = (vmRegisterUser)vmFactory.InitializeBase(VM, appSessionManager);

            VM.ErrorMessage = "Invalid Password..";
            return View("RegisterUser1", VM);
        }


        private async Task<bool> DoesHaveAnActiveSession()
        {
            await appSessionManager.ExtSession.LoadSession();
            return appSessionManager.ExtSession.IsValid;
        }
        internal SignUpManager GetSignUpManager()
        {
            var Mgr = new SignUpManager();
            Mgr._configuration = _configuration;
            Mgr._http = _accessor.HttpContext;
            Mgr._appSessionManager = appSessionManager;
            appSessionManager.ExtSession.LoadSession().GetAwaiter().GetResult();
            return Mgr;
        }
    }
}
