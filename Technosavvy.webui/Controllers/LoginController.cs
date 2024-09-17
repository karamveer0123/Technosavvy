using Microsoft.AspNetCore.Mvc;
using Google.Authenticator;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;
using TechnoApp.Ext.Web.UI.Service;

namespace TechnoApp.Ext.Web.UI.Controllers;
    [EnsureCompliantCountry]
public class LoginController : baseController
{
    private readonly ILogger<HomeController> _logger;
    public LoginController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;
        _smtp = smtp;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);
        Console2.WriteLine_White($"Info:LogIn Controller Instantiation");
    }
    [HttpGet]
    public async Task<IActionResult> LogIn()
    {
        if (await DoesHaveAnActiveSession())
            return RedirectToAction("Wallet", "Index");

        var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View(vm);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogIn(vmUserLogin userLoginVm)
    {
        try
        {
            var lm = GetLogInManager();
            var auth = await lm.LogIn(userLoginVm);
            lm.LogEvent("Login Event");
            if (auth.AccountNumber == null)
            {
                var vm = vmFactory.GetvmUserLogin(appSessionManager);
                vm.UserName = userLoginVm.UserName;

                ModelState.AddModelError("", "Please Enter valid user name and Password");
                Console.WriteLine($" A login Attept was made for User:{userLoginVm.UserName}, when no such user exist");
                return View(vm);
            }
            else
            {
                Console.WriteLine($"User:{userLoginVm.UserName} has Acount No:{auth.AccountNumber}");
                Console.WriteLine($"Auth:{JsonSerializer.Serialize(auth)}");
            }
            if (auth.MultiFact_Enabled)
            {
                //Done: Naveen, do needful to present 2factor UI here
                vm2ndAuth vm = vmFactory.Getvm2ndAuth(appSessionManager);
                vm.infoBag = _protector.Protect($"{auth.mAuthId}");
                // vm.Code = auth.GAuthCode;
                //vm.Code = _protector.Protect(auth.GAuthCode);
                return View("TwoFactorSecurity", vm);
            }
            else
            {
                //setup session
                Console.WriteLine($"About to create Session for:{auth.userName}.. at..{DateTime.UtcNow}");
                await GetLogInManager().SetUpSession(auth);
                Console.WriteLine($"Session Created for:{auth.userName}, Now Redirecting to Wallet.. at..{DateTime.UtcNow}");
                //decide where to send user after login
                if (ConfigEx.VersionType == versionType.PreBeta)
                    return RedirectToAction("index", "my-TechnoSavvy");
                else
                {
                    //ToDo: User return URL for originating URL Redirect
                    return RedirectToAction("index", "Wallet");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login process Error:{ex.GetDeepMsg()}");
            var vm = vmFactory.GetvmUserLogin(appSessionManager);
            return View(vm);
        }

    }
    [HttpGet]
    public async Task<IActionResult> LogOut()
    {
        var isActive = await DoesHaveAnActiveSession();
        if (isActive)
        {
            var res = GetLogInManager();
            res.LogEvent("Log Out User");
            await res.LogOut();
            appSessionManager.LogOff();
        }
        return RedirectToAction("LogIn", "LogIn");
    }
   
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TwoFactorLogIn(vm2ndAuth vm)
    {
        try
        {
            var obj = _protector.Unprotect(vm.infoBag);
            Guid.TryParse(obj, out var auId);
            var lm = GetLogInManager();
            var auth = await lm.GetAuth(auId);
            var isvalid = (await SettingsManager.Instance(this)).Verify2Fact(auth.GAuthCode, vm.OTP);
            if (isvalid)
            {
                lm.LogEvent("2nd Factor Login Event");
                await GetLogInManager().SetUpSessionAfter2ndFactor(new Model.mAuth() { mAuthId = auId, userName = auth.userName });
                return RedirectToAction("index", "wallet");
            }
            else
            {
                vm = (vm2ndAuth)vmFactory.InitializeBase(vm, appSessionManager);
                vm.ErrMsg = "Incorrect OTP";
                return View("TwoFactorSecurity", vm);
            }
        }
        catch (Exception ex)
        {
            return RedirectToAction("Error", new ErrorViewModel() { DeepMsg = ex.GetDeepMsg() });
        }
    }
    public async Task<IActionResult> TwoFactorSecurity()
    {
        return View(await vmFactory.GetvmBase(appSessionManager));
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
    private async Task<bool> DoesHaveAnActiveSession()
    {
        await appSessionManager.ExtSession.LoadSession();
        return appSessionManager.ExtSession.IsValid;
    }
    internal LoginManager GetLogInManager()
    {
        var Mgr = new LoginManager();
        appSessionManager.ExtSession.LoadSession().GetAwaiter().GetResult();
        Mgr._configuration = _configuration;
        Mgr._http = _accessor.HttpContext;
        Mgr._appSessionManager = appSessionManager;
        return Mgr;
    }
}
