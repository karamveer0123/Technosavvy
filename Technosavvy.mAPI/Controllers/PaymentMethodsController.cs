using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace NavExM.Int.Maintenance.APIs.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentMethodsController : sControllerBase
{
    public PaymentMethodsController(ApiAppContext _ctx, IHttpContextAccessor _http, IOptions<SmtpConfig> _smtp)
    {
        ctx = _ctx;
        httpContext = _http.HttpContext!;
        smtp = _smtp;
    }

    [HttpGet("GetmyINRUPISetup")]
    public ActionResult<List<mINRUPI>> GetmyINRUPISetup(Guid profileId)
    {
        try
        {
            var pm = GetPaymentMethodManager();
            var res = pm.GetINRUPISetup(profileId);
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("GetmyINRBankDeposit")]
    public ActionResult<List<mINRBankDeposit>> GetmyINRBankDeposit(Guid profileId)
    {
        try
        {
            var pm = GetPaymentMethodManager();
            var res = pm.GetINRBankDeposit(profileId);
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpPost("CreateINRUPISetup")]
    public ActionResult<mINRUPI> CreateINRUPISetup(mINRUPI m)
    {
        try
        {
            var pm = GetPaymentMethodManager();
            var res = pm.CreateINRUPISetup(m);
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("DeleteINRUPISetup")]
    public ActionResult<bool> DeleteINRUPISetup(Guid m)
    {
        try
        {
            var pm = GetPaymentMethodManager();
            var res = pm.DeleteINRUPISetup(m);
            return Ok(res);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("DeleteINRBankDeposit")]
    public ActionResult<bool> DeleteINRBankDeposit(Guid m)
    {
        try
        {
            var pm = GetPaymentMethodManager();
            var res = pm.DeleteINRBankDeposit(m);
            return Ok(res);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpPost("CreateINRBankDepositSetup")]
    public ActionResult<mINRBankDeposit> CreateINRBankDepositSetup(mINRBankDeposit m)
    {
        try
        {
            var pm = GetPaymentMethodManager();
            var res = pm.CreateINRBankDepositSetup(m);
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }

    [HttpGet("GetINRBankDetails")]
    public ActionResult<List<mBankDepositPaymentMethod>> GetINRBankDetails()
    {
        try
        {
            var tm = GetFiatCurrencyManager();
            var res = tm.GetINRBankDetails();
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("GetINRUPIDetails")]
    public ActionResult<List<mUPIPaymentMethod>> GetINRUPIDetails()
    {
        try
        {
            var tm = GetFiatCurrencyManager();
            var res = tm.GetINRUPIDetails();
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    private PaymentMethodManager GetPaymentMethodManager()
    {
        //ToDo: Secure this Manager
        var result = new PaymentMethodManager();
        result.dbctx = ctx;
        result.httpContext = httpContext;
        return result;
    }
    private FiatCurrencyManager GetFiatCurrencyManager()
    {
        //ToDo: Secure this Manager
        var result = new FiatCurrencyManager();
        result.dbctx = ctx;
        result.httpContext = httpContext;
        return result;
    }
    private ManagerBase GetMB()
    {
        var result = new ManagerBase();
        return result;
    }
}
