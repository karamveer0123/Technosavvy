using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
namespace NavExM.Int.Maintenance.APIs.Controllers;

[ApiController]
[Route("[controller]")]
public class PreBetaController : sControllerBase
{
    public PreBetaController(ApiAppContext _ctx, PreBetaDBContext _pbctx, IHttpContextAccessor _http, IOptions<SmtpConfig> _smtp)
    {
        ctx = _ctx;
        pbctx = _pbctx;
        httpContext = _http.HttpContext!;
        smtp = _smtp;
    }
    [HttpGet("GetPrebetaStats")]
    public async Task<ActionResult> GetPrebetaStats()
    {
        try
        {

            var cm = GetPMManager();
            var res = cm.GetPreBetaStats();
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.Message} inner->{ex.InnerException}");
        }
    }
    [HttpGet("GetPreBetaStages")]
    public ActionResult GetPreBetaStages()
    {
        try
        {
            var cm = GetPMManager();
            var r = cm.GetAllPreBetaStages();
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("GetAllPreBetaFactors")]
    public ActionResult GetAllPreBetaFactors()
    {
        try
        {
            var cm = GetPMManager();
            var r = cm.GetAllPreBetaFactors();
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    //[HttpGet("GetCurrentPreBetaFactors")]
    //public ActionResult GetCurrentPreBetaFactors()
    //{
    //    try
    //    {
    //        var cm = GetPMManager();
    //        var r = cm.GetAllPreBetaFactors();
    //        if(r!=null && r.Count>0)
    //        {
    //            var a=r.OrderByDescending(x => x.CreatedOn).First().FractionFactor;
    //            return Ok(a);
    //        }
    //        return BadRequest("No Records");
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest($"{ex.GetDeepMsg()}");
    //    }
    //}
    [HttpPost("SaveAndStartPreBeta")]
    public ActionResult SaveAndStartPreBeta(List<mPreBetaStages> vm)
    {
        try
        {
            var cm = GetPMManager();
            var r = cm.SavePreBetaStages(vm);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpPost("UpdateFactor")]
    public ActionResult SavePreBetaFactor(mFractionFactor f)
    {
        try
        {
            var cm = GetPMManager();
            var r = cm.SavePreBetaFactors(f);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    private PreBetaManager GetPMManager()
    {
        var result = new PreBetaManager()
        {
            pbdbctx=pbctx,
            dbctx = ctx,
            cdbctx = cctx,
            httpContext = httpContext
        };
        return result;
    }
}

