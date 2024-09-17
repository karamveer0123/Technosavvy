using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace NavExM.Int.Maintenance.APIs.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController : sControllerBase
{
    public ProfileController(ApiAppContext _ctx, IHttpContextAccessor _http, IOptions<SmtpConfig> _smtp)
    {
        ctx = _ctx;
        httpContext = _http.HttpContext!;
        smtp = _smtp;
    }
    [HttpPost("UpdateAddress")]
    public ActionResult<mProfile> UpdateAddress(mProfile mPro)
    {
        try
        {
            var m = GetManager();
            mPro = m.UpdateAddress(mPro).ToModel();
            return (mPro);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpPost("UpdateProfilePersonalDetails")]
    public ActionResult<mProfile> UpdateProfilePersonalDetails(mProfile mPro)
    {
        try
        {
            var m = GetManager();
            mPro = m.UpdateProfilePersonalDetails(mPro).ToModel();
            return (mPro);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
   

    [HttpPost("CreateProfile")]
    public ActionResult<mProfile>CreateProfile(mProfile mPro)
    {
        try
        {
            var m = GetManager();
           mPro= m.CreateProfile(mPro).ToModel();
            return (mPro);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("GetProfile")]
    public ActionResult<mProfile> GetProfile(Guid userAccountID)
    {
        try
        {
            var m = GetManager();
            var result = m.GetProfileM(userAccountID);
            return Ok(result);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }

    #region Private
    private ProfileManager GetManager()
    {
        EnsureValidSession();
        var result = new ProfileManager();
        result.dbctx = ctx;
        result.httpContext = httpContext;
        return result;
    }
    private ManagerBase GetMB()
    {
        var result = new ManagerBase();
        return result;
    }
    #endregion
}
