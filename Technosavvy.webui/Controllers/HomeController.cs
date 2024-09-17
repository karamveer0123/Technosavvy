using TechnoApp.Ext.Web.UI.Static;
using Microsoft.AspNetCore.Mvc;
using TechnoApp.Ext.Web.UI.Manager;
using TechnoApp.Ext.Web.UI.Models;
using System.Diagnostics;
using Google.Authenticator;
using System.Web;
using Microsoft.AspNetCore.DataProtection;
using TechnoApp.Ext.Web.UI.Extentions;
using TechnoApp.Ext.Web.UI.Service;

namespace TechnoApp.Ext.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IOptions<SmtpConfig> _smtp;
        private IConfiguration _configuration;
        private HttpContext _context;
        private IHttpContextAccessor _accessor;
        IDataProtector _protector;
        AppSessionManager appSessionManager = null;

        public HomeController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
        {
            _logger = logger;
            _smtp = smtp;
            _configuration = configuration;
            _accessor = accessor;
            _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
            appSessionManager = new AppSessionManager(accessor.HttpContext);

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var um = GetUserManager();
            um.LogEvent("TechnoApp Home Page");
            if (ConfigEx.VersionType == versionType.PreBeta)
                return View("indexpre", await vmFactory.GetvmBase(appSessionManager));

            //Return Home Page
            return View("index0", await vmFactory.GetvmBase(appSessionManager));
        }
        [HttpGet]
        public async Task<IActionResult> NoService()
        {
            var ok = SrvPrebeta.CompliantCountry(appSessionManager.mySession.UserCountry);
            if (ok)
                return RedirectToAction("index", "home");

            var vm = await vmFactory.GetvmBase(appSessionManager);
            return View("NoService", vm);
        }
        [HttpGet]
        public async Task<IActionResult> NoUrl()
        {
          
            var vm = await vmFactory.GetvmBase(appSessionManager);
            return View("nourl404", vm);
        }
        [HttpGet]
        public async Task<IActionResult> Appendix()
        {
            var vm = await vmFactory.GetvmBase(appSessionManager);
            return View("Appendix", vm);
        }
        [HttpGet]
        public async Task<IActionResult> SetCountry(string abbr)
        {

            var KM = GetKYCManager();
            var result = await KM.GetvmCountriesList();
            if (result.Any(x => x.Abbri.ToLower() == abbr.ToLower()))
            {
                appSessionManager.mySession.UserCountry = abbr.ToUpper();
            }
            return Json(appSessionManager.mySession.UserCountry);
        }
        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var KM = GetKYCManager();
            var result = await KM.GetvmCountriesList();
            return Json(result);

        }
        #region May Not Required
        public async Task<IActionResult> VerifyOtp(vmUser userVm)
        {
            var um = GetUserManager();
            var result = await um.VerifyEmailOTP(userVm.Email, userVm.OTP);
            um.LogEvent("Verify Otp Method");
            var retun = eActionName.VerifyOtp.ToString() + "|" + result.ToString();
            return Json(retun);
        }

        public async Task<IActionResult> CreatePassword(vmUser userVm)
        {
            var um = GetUserManager();
            var result = await um.CreatePassword(userVm.UserId, userVm.Password);
            um.LogEvent("Create Password Method");
            var retun = eActionName.CreatePassword.ToString() + "|" + result.ToString();

            return Json(retun);
        }
        [AfterLogIn]
        public async Task<IActionResult> IsTranForPurchases(string trId)
        {
            try
            {
                trId.CheckAndThrowNullArgumentException();
                var wm = GetWalletManager();
                var lst = await wm.GetMyPrebetaPurshases();
                return Json(lst.FirstOrDefault(x => x.TranHash == trId));

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Occoured in IsTranForPurchases..at:{DateTime.UtcNow.GetCurrentUnix()}\n\t{ex.GetDeepMsg()}");
            }
            return Json(false);
        }
        #endregion
        public async Task<IActionResult> MinimumProfile()
        {
            return View(await vmFactory.GetvmBase(appSessionManager));

        }
        [HttpGet]
        public async Task<IActionResult> LogIn()
        {

            //ToDo: Naveen, This Implement is for Multi factor Auth Only, Use LogIn/Login
            RedirectToAction("login", "login");
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(vmUserLogin userLoginVm)
        {
            try
            {
                var um = GetUserManager();
                var result = await um.LogIn(userLoginVm);
                um.LogEvent("Log In Method");
                if (result.AccountNumber == null)
                {
                    ModelState.AddModelError("", "Please Enter valid User-Name & Password");
                    return View(userLoginVm);
                }
                if (result.MultiFact_Enabled)
                {
                    //ToDo: Naveen  do needful to present 2factor UI here
                }
                string message = "";
                bool status = false;

                string GAuthPrivKey = _configuration.GetSection("GAuthPrivateKey").Value;
                string UserUniqueKey = (userLoginVm.UserName + GAuthPrivKey);  // TODO:Arjun Lamba:- Refactor user unique key
                if (result.AccountNumber != null)
                {
                    status = true;
                    appSessionManager.mySession.UserName = userLoginVm.UserName;

                    if (_configuration.GetSection("GAuthEnable").Value.ToString() == "1")
                    {

                        int k = 0;
                        if (appSessionManager.mySession.UserCode.IsNOT_NullorEmpty())
                        {
                            k = 1;
                        }
                        else
                        {
                            if (appSessionManager.mySession.UserCode.IsNullOrEmpty())
                            {
                                string UserCodeE = appSessionManager.mySession.UserCode;
                                string UserCodeD = _protector.Unprotect(UserCodeE);
                                if (UserUniqueKey == UserCodeD)
                                {
                                    return RedirectToAction(nameof(Dashboard));
                                }
                                else
                                {
                                    k = 1;
                                }
                            }
                        }
                        if (k == 1)
                        {
                            message = "Two Factor Authentication Verification";
                            //Two Factor Authentication Setup
                            TwoFactorAuthenticator TwoFacAuth = new TwoFactorAuthenticator();
                            SetSessionCookies("UserUniqueKey", UserUniqueKey);
                            var setupInfo = TwoFacAuth.GenerateSetupCode("TechnoApp.com", userLoginVm.UserName, UserUniqueKey, false, 3);
                            ViewBag.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;
                            ViewBag.SetupCode = setupInfo.ManualEntryKey;
                        }
                    }
                    else
                    {
                        //FormsAuthentication.SetAuthCookie(Session["Username"].ToString(), true);
                        //ViewBag.Message = "Welcome to Mr. " + Session["Username"].ToString();
                        //       return View("UserProfile");
                        if (ConfigEx.VersionType == versionType.PreBeta)
                        {
                            var wm = GetWalletManager();
                            await wm.EnsureNetworkWallet_Prebeta();
                            um.LogEvent("Log In Method redirected to PreBeta MyPurchases");
                            return RedirectToAction("index", "my-TechnoSavvy");
                        }
                        else
                        {
                            um.LogEvent("Log In Method redirected to dashboard");
                            return RedirectToAction(nameof(Dashboard));
                        }
                        
                    }
                }
                else
                {
                    um.LogEvent("Log In Method Invalid Credential");
                    message = "Please Enter the Valid Credential!";
                }
                ViewBag.Message = message;
                ViewBag.Status = status;
                return View(userLoginVm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LogIn caused an error at..{DateTime.UtcNow.GetCurrentUnix()}\n\t{ex.GetDeepMsg()}");
                ModelState.AddModelError("", "Please Try again with valid credentials");
                return View(userLoginVm);
            }
        }
        /// <summary>
        /// this function work for check user two TwoFactorAuthenticate
        /// </summary>
        /// <param name="CodeDigit"></param>
        /// <returns></returns>
        public IActionResult TwoFactorAuthenticate(string CodeDigit)
        {
            var token = CodeDigit;
            TwoFactorAuthenticator TwoFacAuth = new TwoFactorAuthenticator();
            string UserUniqueKey = GetSessionCookies("UserUniqueKey").ToString();
            bool isValid = TwoFacAuth.ValidateTwoFactorPIN(UserUniqueKey, token);
            if (isValid)
            {
                string UserCode = _protector.Protect(UserUniqueKey);
                appSessionManager.mySession.UserCode = UserCode;

                return RedirectToAction(nameof(Dashboard));
            }
            GUtilityManager.MessageToaster(this, "LogIn", "You are not a vaid user", "error");
            return RedirectToAction("Login", "Home");
        }
        //public IActionResult Privacy()
        //{
        //    return View();
        //}
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult ThemeChange()
        {
            var um = GetUserManager();
            um.LogEvent("Theme Changed");
            string returnValue = true.ToString() + "|" + appSessionManager.SwitchTheme().ToString();
            var vm = vmFactory.GetvmBase(appSessionManager);
            // return PartialView(vm);
            return Json(returnValue);
        }
        public IActionResult AcceptCookie()
        {
            var um = GetUserManager();
            appSessionManager.mySession.CookieConsent = true;
            um.LogEvent("Cookie policy Consented");
            return Json("ackn");
        }
        public IActionResult AddtoFav(string str)
        {
            var um = GetUserManager();
            appSessionManager.AddToFavourite(str);
            um.LogEvent($"{str} Added to Favourite list");
            return Json("ackn");
        }
        public IActionResult RemoveFromFav(string str)
        {
            var um = GetUserManager();
            appSessionManager.RemoveFromFavourite(str);
            um.LogEvent($"{str} Remove from Favourite list");
            return Json("ackn");
        }
        public IActionResult AddtoWatch(string str)
        {
            var um = GetUserManager();
            appSessionManager.AddToWatchList(str);
            um.LogEvent($"{str} Added to Watch list");
            return Json("ackn");
        }
        public IActionResult RemoveFromWatch(string str)
        {
            var um = GetUserManager();
            appSessionManager.RemoveFromWatchList(str);
            um.LogEvent($"{str} Remove from Watch list");
            return Json("ackn");
        }
        public IActionResult UpdateCurrency(string str)
        {
            appSessionManager.SetCurrency(str);
            return Json(appSessionManager.mySession.Currency);
        }
        public IActionResult LoadEvent(string ev)
        {
            var id = Guid.NewGuid().ToString().Replace("-", "");
            appSessionManager.ReportEvent(ev, "PageLoad", id);
            return Json(id);
        }
        public IActionResult NavigationEvent(string ev, string pid, double sc, double scH)
        {
            appSessionManager.ReportEvent(ev, "Navigation Away", pid, sc, scH);
            return Json("ok");
        }
        public IActionResult ScrollEvent(string ev, string pid, double sc, double scH)
        {
            appSessionManager.ReportEvent(ev, "ScrollTo", pid, sc, scH);
            return Json("ok");
        }

        private void SetSessionCookies(string sessionCookie, string cockieValue)
        {
            //ToDo:Naveen, Fix this Cookie Logic. It should not be in Controller
            // Try to get the session ID from the request; otherwise create a new ID.  

            //var cookeidata = sessionId;

            //we do not know this user, so create a new userid to begin
            CookieOptions cookieOptions = new CookieOptions()
            {
                Expires = new DateTimeOffset(2099, 1, 1, 0, 0, 0, TimeSpan.FromHours(0))
            };
            HttpContext.Response.Cookies.Append(sessionCookie, cockieValue, cookieOptions);
        }

        private string GetSessionCookies(string sessionCookieName)
        {
            var cookieSession = HttpContext.Request.Cookies.ContainsKey(sessionCookieName)
                                        ? HttpContext.Request.Cookies[sessionCookieName] : string.Empty;
            return cookieSession;
        }

        /// <summary>
        /// Check If user already exit or not
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public async Task<IActionResult> IsExitUserName(vmRegisterUser VM)
        {
            var result = await GetUserManager().GetAnyUser(VM.vmRegUserName.Email);
            var dataRes = result ? false : true;
            return Json(dataRes);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        internal UserManager GetUserManager()
        {
            var Mgr = new UserManager();
            Mgr._configuration = _configuration;
            Mgr._http = _accessor.HttpContext;
            Mgr._appSessionManager = appSessionManager;
            return Mgr;
        }
        internal KYCManager GetKYCManager()
        {
            var Mgr = new KYCManager();
            Mgr._configuration = _configuration;
            Mgr._http = _accessor.HttpContext;
            Mgr._appSessionManager = appSessionManager;
            return Mgr;
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
}