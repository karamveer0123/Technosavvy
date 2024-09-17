using Microsoft.AspNetCore.Mvc;
using NavExM.Int.Maintenance.APIs.Model.AppInt;

namespace NavExM.Int.Maintenance.APIs.Controllers;

[ApiController]
[Route("[controller]")]
public class CommonDataController:sControllerBase
{
    public CommonDataController(ApiAppContext _ctx, IHttpContextAccessor _http)
    {
        ctx = _ctx;
        httpContext = _http.HttpContext!;
    }
    [HttpGet("GetCountries")]
    public ActionResult<List<mCountry>> GetCountries()
    {

        try
        {
            var um = GetManager();
            var result = um.GetCountries();
            return Ok(result);
        }
        catch (Exception ex)
        {
            //ToDo: Santosh, Log This message to watcher
            return BadRequest($"{ex.Message} inner->{ex.InnerException}");
            throw;
        }
    }
    [HttpGet("GetHandShakePackage")]
    public ActionResult<mHandShakePackage> GetHandShakePackage()
    {

        try
        {
            var um = GetManager();
            var result = um.DoHandShake();
            return Ok(result);
        }
        catch (Exception ex)
        {
            //ToDo: Santosh, Log This message to watcher
            return BadRequest($"{ex.Message} inner->{ex.InnerException}");
            throw;
        }
    }
    [HttpGet("GetGlobalVariables")]
    public ActionResult<List<Tuple<string, string>>> GetVariablesl()
    {
        try
        {
            var um = GetManager();
            var result = um.GetVariables();
            return Ok(result);
        }
        catch (Exception ex)
        {
            //ToDo: Santosh, Log This message to watcher
            
            return BadRequest($"{ex.Message} inner->{ex.InnerException}");
            throw;
        }
    }
//#if(DEBUG)
//    [HttpGet("GetDummyMarket")]
//    public ActionResult<mMarket> GetDummyMarket()
//    {
//        try
//        {
//            var um = GetManager();
//            var result = um.DummyMarket();
//            return Ok(result);
//        }
//        catch (Exception ex)
//        {
//            //ToDo: Santosh, Log This message to watcher
//            return BadRequest($"{ex.Message} inner->{ex.InnerException}");
//            throw;
//        }
//    }
//#endif
    [HttpGet("WhatToWatch_ForGeko")]
    public ActionResult<mCoinWatch> WhatToWatchForGeko()
    {
        try
        {
            var um = GetManager();
            var result = um.GetCoinWatcher_ForGeko();
            return Ok(result);
        }
        catch (Exception ex)
        {
            //ToDo: Santosh, Log This message to watcher
            return BadRequest($"{ex.Message} inner->{ex.InnerException}");
            throw;
        }
    }
    #region Private
    private CommonDataManager GetManager()
    {
        var result = new CommonDataManager();
        result.dbctx = ctx;
        result.httpContext = httpContext;
        return result;
    }
    #endregion
}

