using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NavExM.Int.Maintenance.APIs.Services;

namespace NavExM.Int.Maintenance.APIs.Controllers;

[ApiController]
[Route("[controller]")]
public class WatchController : sControllerBase
{
    public WatchController(ApiAppContext _ctx, IHttpContextAccessor _http, IOptions<SmtpConfig> _smtp)
    {
        ctx = _ctx;
        httpContext = _http.HttpContext!;
        smtp = _smtp;
    }
    [HttpPost("GetCurrencyNames")]
    [HttpGet("GetCurrencyNames")]
    public ActionResult<List<string>> GetCurrencyNames()
    {
        try
        {
            var res = SrvCurrencyWatch.GetAllCurrenciesName();
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpPost("GetCurrencyValues")]
    [HttpGet("GetCurrencyValues")]
    public ActionResult<List<TokenPrice>> GetCurrencyValues()
    {
        try
        {
            var res = SrvCurrencyWatch.GetAllCurrencies();
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpGet("GetCurrencyValuesWithName")]
    public ActionResult<List<TokenPrice>> GetCurrencyValues(List<string> Names)
    {
        try
        {
            var res = SrvCurrencyWatch.GetCurrencies(Names);
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpGet("GetCoinValuesWithName")]
    public ActionResult<List<TokenPrice>> GetCoinValues(List<string> Names)
    {
        try
        {
            var res = SrvCoinWatch.GetCoins(Names);
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpGet("GetCoinValues")]
    public ActionResult<List<TokenPrice>> GetCoinValues()
    {
        try
        {
            var res = SrvCoinWatch.GetAllCoins();
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpGet("GetCoin")]
    public ActionResult<TokenPrice> GetCoin(string Name)
    {
        try
        {
            var res = SrvCoinWatch.GetCoin(Name );
                return Ok(res);
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    #region Private

    private WalletManager GetWalletManager()
    {
        //ToDo: Secure this Manager, Transaction Count Applied, Active Session

        var result = new WalletManager();
        result.dbctx = ctx;
        result.httpContext = httpContext;
        if (result.GetSessionHash().IsNullorEmpty()) throw new UnauthorizedAccessException("No Session exist..");
        return result;
    }
    private ManagerBase GetMB()
    {
        var result = new ManagerBase();
        return result;
    }
    #endregion
}
