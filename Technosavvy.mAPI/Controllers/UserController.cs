using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace NavExM.Int.Maintenance.APIs.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController : sControllerBase
{
    public UserController(ApiAppContext _ctx, IHttpContextAccessor _http, IOptions<SmtpConfig> _smtp, RewardAppContext _Rectx)
    {
        ctx = _ctx;
        httpContext = _http.HttpContext!;
        smtp = _smtp;
        rctx = _Rectx;
    }

    #region Get
    [HttpGet("AnyUser")]
    public ActionResult<bool> AnyUser(string uName)
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("Verify User exists?");
            var result = um.IsAny(uName);
            return Ok(result);
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpGet("GetUserByName")]
    public ActionResult<mUser> GetUser(string uName)
    {
        var user = new mUser();
        var um = GetUserManager();
        try
        {
            um.LogEvent("Get User details?");
            var u = um.GetUser(uName).ToModel();
            return Ok(u);
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpGet("GetFundWalletOf")]
    public ActionResult<Guid> GetFundWalletOf(string userAccount)
    {
        var user = new mUser();
        var um = GetUserManager();
        try
        {
            //um.LogEvent("Get User details?");
            var u = um.GetFundWalletOf(userAccount);
            return Ok(u);
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }

    [HttpGet("CanSetUserPassword")]
    public ActionResult<bool> CanSetUserPassword(string uName)
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("CanSetUserPassword");
            var result = um.CanSetPassword(uName);
            return Ok(result);
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpGet("IsEmailVerified")]
    public ActionResult<bool> IsEmailVerified(string uName)
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("IsEmailVerified");
            var result = um.IsEmailVerified(uName);
            return Ok(result);
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpPost("EnableMultiFactor")]
    public ActionResult<bool> EnableMultiFactor(string uName)
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("IsEmailVerified");
            var result = um.EnableMultiFactor(uName);
            return Ok(result);
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpPost("DisableMultiFactor")]
    public ActionResult<bool> DisableMultiFactor(string uName)
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("DisableMultiFactor");
            var result = um.DiableMultiFactor(uName);
            return Ok(result);
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpPost("IncludeMFAuthenticator")]
    public ActionResult<bool> IncludeMFAuthenticator(string uName, string Code)
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("IncludeMFAuthenticator");
            var result = um.IncludeMFAuthenticator(uName, Code);
            return Ok(result);
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpGet("GetUserByID")]
    public ActionResult<mUser> GetUser(Guid id)
    {
        var user = new mUser();
        var um = GetUserManager();

        try
        {
            um.LogEvent("GetUser By Id");
            var res = um.GetUserbyId(id);
            if (res is not null)
                return Ok(res.ToModel());
            else return BadRequest();

        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
        return user;
    }
    #endregion

    #region Post

    [HttpGet("VerifyEmailOTP")]
    public ActionResult<bool> ValidateOTP(string uName, string otp)
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("GetUser By Id");
            var result = um.ValidateOTP(uName, otp);
            return Ok(result);
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
            throw;
        }
    }
    [HttpGet("RegisterUser")]
    public ActionResult<Guid> CreateUser(string uName,string? refCode="")
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("CreateUser");
            var res = um.AddUserAccount(uName,refCode);
            if (res != Guid.Empty)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpPost("RequestEmailOTP")]
    public ActionResult<bool> CreateEmailOTP(string uName)
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("CreateUser");
            var res = um.SendEmailOTP(uName, smtp, HttpContext);
            if (res == true)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpPost("CreateEmailOTPFOrgetPassword")]
    public ActionResult<bool> CreateEmailOTPFOrgetPassword(string uName)
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("CreateEmailOTPFOrgetPassword");
            var res = um.SendEmailOTPForgetPassword(uName, smtp, HttpContext);
            if (res == true)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpPost("SuspendMeAutoFor")]
    public ActionResult<bool> SuspendMeAutoFor(string Msg, DateTime till)
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("SuspendMeAutoFor");
            var res = um.SuspendMeAutoFor(Msg, till);
            if (res == true)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpPost("SuspendMeForStaffIntervention")]
    public ActionResult<bool> SuspendMeForStaffIntervention(string Msg)
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("SuspendMeForStaffIntervention");
            var res = um.SuspendMeForStaffIntervention(Msg);
            if (res == true)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpGet("UpdatePassword")]
    public ActionResult<bool> UpdatePassword(Guid userId, string password)
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("UpdatePassword");
            var res = um.UpdatePasswordOfUserAccount(userId, password, smtp);
            if (res == true)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
        return false;
    }
    [HttpPost("LogIn")]
    [HttpPost("Authenticate")]
    public ActionResult<mAuth> LogIn(mLogIn mLogin)
    {
        var um = GetUserManager();
        try
        {

            um.LogEvent("LogIn");
            var result = um.Authenticate(mLogin.UserName, mLogin.Password, HttpContext);
            if (result != null)
                return Ok(result);
            else
                return BadRequest();
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    /// <summary>
    /// Use this for Deposit/WithDraw Request that need 2F Auth
    /// </summary>
    [HttpGet("InSession2FAuth")]
    public ActionResult<mAuth> InSession2FAuth()
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("LogIn");
            var result = um.InSession2FAuth();
            if (result != null)
                return Ok(result);
            else
                return BadRequest();
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpGet("GetAuth")]
    public ActionResult<mAuth> GetAuth(Guid authId)
    {
        var um = GetUserManager();
        try
        {

            um.LogEvent("Get AuthId");
            var auth = um.GetAuthEvent(authId);
            var mAuth = auth.ToModel();
            if (auth != null)
            {
                if (auth.IsMultiFactor)
                    mAuth.GAuthCode = auth.UserAccount.Authenticator != null ? auth.UserAccount.Authenticator.Code : string.Empty;

                return Ok(mAuth);
            }

            else
                return BadRequest();
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpGet("SignOut")]
    public ActionResult<bool> SignOut()
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("SignOut");
            var res = um.SignOut();
            if (res == true)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpGet("SignOutAllDevices")]
    public ActionResult<bool> SignOutAll()
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("SignOutAllDevices");
            var res = um.SignOutAllDevices();
            if (res == true)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }

    [HttpGet("GetMySession")]
    public ActionResult<mUserSession> GetMySession()
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("GetMySession");
            var res = um.GetMySession();
            if (res is not null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }

    [HttpPost("RequestSession")]
    public ActionResult<mUserSession> RequestSession(mUserSessionRequest m)
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("RequestSession");
            var res = um.RequestSession(m.authId, m.UserName);
            if (res is not null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpPost("Report2factor")]
    public ActionResult<mUserSession> Report2factor(Guid AuthId, string uName, string mode)
    {
        var um = GetUserManager();
        try
        {
            um.LogEvent("RequestSession");
            var res = um.Report2ndFactor(AuthId, uName, mode);
            if (res is not null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            um.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    #endregion

    #region Private
    private UserManager GetUserManager()
    {
        return new UserManager {
            dbctx = ctx,
            Rewardctx = rctx,
            httpContext = httpContext
        };
    }

    #endregion
}
