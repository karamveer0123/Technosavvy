using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NavExM.Int.Maintenance.APIs.Services;

namespace NavExM.Int.Maintenance.APIs.Controllers;
[ApiController]
[Route("[controller]")]
public class TokenController : sControllerBase
{
    public TokenController(ApiAppContext _ctx, IHttpContextAccessor _http, IOptions<SmtpConfig> _smtp)
    {
        ctx = _ctx;
        httpContext = _http.HttpContext!;
        smtp = _smtp;
    }
    [HttpGet("GetEstimatedValueIn")]
    public ActionResult<Tuple<double, double>> GetEstimatedValueIn(string bCode, string qCode, double Amt)
    {
        try
        {
            var tm = GetTokenManager();
            tm.GetEstimatedValueIn(bCode, qCode, Amt, out var Rate, out var MinTrade);
            return new Tuple<double, double>(Rate, MinTrade);

        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("GetTokenMarketEstimatedBuyPrice")]
    public ActionResult<List<mToken>> GetTokenMarketEstimatedBuyPrice(string mCode, double Amt)
    {
        try
        {
            var tm = GetTokenManager();
            var res = tm.GetTokenMarketEstimatedBuyPrice(mCode, Amt);
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
    [HttpGet("GetTokenMarketEstimatedSellPrice")]
    public ActionResult<List<mToken>> GetTokenMarketEstimatedSellPrice(string mCode, double Amt)
    {
        try
        {
            var tm = GetTokenManager();
            var res = tm.GetTokenMarketEstimatedSellPrice(mCode, Amt);
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
    [HttpGet("GetTokenMarketEstimatedSellTotalValueUnitsCount")]
    public ActionResult<List<mToken>> GetTokenMarketEstimatedSellTotalValueUnitsCount(string mCode, double TradeValue)
    {
        try
        {
            var tm = GetTokenManager();
            var res = tm.GetTokenMarketEstimatedSellTotalValueUnitsCount(mCode, TradeValue);
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

    [HttpGet("GetAllActiveTokens")]
    public ActionResult<List<mToken>> GetAllActiveTokens()
    {
        try
        {
            var tm = GetTokenManager();
            var res = tm.GetAllActiveTokensModel();
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
    [HttpGet("GetActiveToken")]
    public ActionResult<List<mToken>> GetActiveToken(Guid id)
    {
        try
        {
            var tm = GetTokenManager();
            var res = tm.GetActiveTokenModel(id);
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
    [HttpGet("GetActiveTokenOfCode")]
    public ActionResult<List<mToken>> GetActiveTokenOfCode(string code)
    {
        try
        {
            var tm = GetTokenManager();
            var res = tm.GetActiveTokenModel(code);
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
    [HttpGet("GetActiveTokens")]
    public ActionResult<List<mToken>> GetActiveTokens(int count)
    {
        try
        {
            var tm = GetTokenManager();
            var res = tm.GetActiveTokensModel(count);
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

    [HttpGet("GetAllInActiveTokens")]
    public ActionResult<List<mToken>> GetAllInActiveTokens()
    {
        try
        {
            var tm = GetTokenManager();
            var res = tm.GetAllInActiveTokensModel();
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
    [HttpGet("GetAllTokensNetWorkFee")]
    public ActionResult<List<mTokenNetworkFee>> GetAllTokensNetWorkFee()
    {
        try
        {
            var tm = GetTokenManager();
            var res = tm.GetAllTokensNetWorkFee();
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
    [HttpGet("GetTokensNetWorkFee")]
    public ActionResult<mTokenNetworkFee> GetTokensNetWorkFee(Guid tokenId, Guid netId)
    {
        try
        {
            var tm = GetTokenManager();
            var res = tm.GetTokensNetWorkFee(tokenId, netId);
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

    [HttpGet("GetAllSupportedNetworks")]
    public ActionResult<List<mSupportedNetwork>> GetAllSupportedNetworks()
    {
        try
        {
            var tm = GetSNManager();
            var res = tm.GetAllSupportedNetwork();
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
    [HttpGet("GetAllNetworkTokens")]
    public ActionResult<List<mToken>> GetAllNetworkTokens(Guid networkId)
    {
        try
        {
            var tm = GetTokenManager();
            var res = tm.GetAllTokensOfNetwork(networkId);
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
    [HttpGet("GetTokenDetails")]
    public ActionResult<mToken> GetTokenDetails(Guid id)
    {
        try
        {
            var tm = GetTokenManager();
            var res = tm.GetSpecificToken(id);
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
    [HttpGet("RollTokenWatchListCounter")]
    public ActionResult<mToken> RollTokenWatchListCounter(string code, int count)
    {
        try
        {
            var tm = GetTokenManager();
            var res = tm.UpdateTokenWatchListCount(code, count);
            return Ok(res);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("RollTokenFavListCounter")]
    public ActionResult<mToken> RollTokenFavListCounter(string code, int count)
    {
        try
        {
            var tm = GetTokenManager();
            var res = tm.UpdateTokenFavListCount(code, count);
            return Ok(res);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }

    //[HttpPost("CreateToken")]
    //public ActionResult<Tuple<bool, string>> CreateToken(mToken token)
    //{
    //    try
    //    {
    //        var tm = GetTokenManager();
    //        var res = tm.CreateToken(token);
    //        return Ok(res);

    //    }
    //    catch (Exception ex)
    //    {
    //        var Msg = GetMB().LogError(ex);
    //        return BadRequest(Msg);
    //    }
    //}
    //[HttpPost("CreateSupportedNetwork")]
    //public ActionResult<Tuple<bool, string>> CreateSupportedNetwork(mSupportedNetwork m)
    //{
    //    try
    //    {
    //        var tm = GetSNManager();
    //        var res = tm.CreateSupportedNetwork(m);
    //        return Ok(res);

    //    }
    //    catch (Exception ex)
    //    {
    //        var Msg = GetMB().LogError(ex);
    //        return BadRequest(Msg);
    //    }
    //}

    [HttpGet("GetAllSupportedNetwork")]
    public ActionResult<List<mSupportedNetwork>> GetAllSupportedNetwork()
    {
        try
        {
            var tm = GetSNManager();
            var res = tm.GetAllSupportedNetwork();
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
    /*
     * Fiat Currency Related Methods
     */
    [HttpGet("GetAllActiveFiatCurrencies")]
    public ActionResult<List<mFiatCurrency>> GetAllActiveFiatCurrencies()
    {
        try
        {
            var tm = GetFiatCurrencyManager();
            var res = tm.GetAllFiatCurrencies();
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
    [HttpGet("GetAllActiveFiatTokens")]
    public ActionResult<List<mToken>> GetAllActiveFiatTokens()
    {
        try
        {
            var tm = GetTokenManager();
            var res = tm.GetAllActiveFiatTokens();
            if (res != null)
                return Ok(res.ToModel());
            else return BadRequest();
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("GetActiveFiatCurrencies")]
    public ActionResult<List<mFiatCurrency>> GetActiveFiatCurrencies(int count)
    {
        try
        {
            var tm = GetFiatCurrencyManager();
            var res = tm.GetFiatCurrencies(count);
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

    [HttpGet("GetFiatCurrency")]
    public ActionResult<mFiatCurrency> GetFiatCurrency(Guid id)
    {
        try
        {
            var tm = GetFiatCurrencyManager();
            var res = tm.GetSpecificFiatCurrenciesM(id);
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
    [HttpGet("LiveCoinPricesALL")]
    public ActionResult<List<TokenPrice>> LiveCoinPrices()
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
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    //[HttpPost("CreateFiatCurrencies")]
    //public ActionResult<Tuple<bool, string>> CreateFiatCurrencies(mFiatCurrency m)
    //{
    //    try
    //    {
    //        var tm = GetFiatCurrencyManager();
    //        var res = tm.CreateFiatCurrency(m);
    //        if (res != null)
    //            return Ok(res);
    //        else return BadRequest();
    //    }
    //    catch (Exception ex)
    //    {
    //        var Msg = GetMB().LogError(ex);
    //        return BadRequest(Msg);
    //    }
    //}
    #region Private

    private FiatCurrencyManager GetFiatCurrencyManager()
    {
        //ToDo: Secure this Manager
        var result = new FiatCurrencyManager();
        result.dbctx = ctx;
        result.httpContext = httpContext;
        return result;
    }
    private TokenManager GetTokenManager()
    {
        //ToDo: Secure this Manager
        var result = new TokenManager();
        result.dbctx = ctx;
        result.httpContext = httpContext;
        return result;
    }
    private SupportNetworkManager GetSNManager()
    {
        //ToDo: Secure this Manager
        var result = new SupportNetworkManager();
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
