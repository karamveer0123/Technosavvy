using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace NavExM.Int.Maintenance.APIs.Controllers;

[ApiController]
[Route("[controller]")]
public class PageEventController : sControllerBase
{
    public PageEventController(EventAppContext _ctx, IHttpContextAccessor _http, IOptions<SmtpConfig> _smtp)
    {
        ectx = _ctx;
        httpContext = _http.HttpContext!;
        smtp = _smtp;
    }
    [HttpPost("Report")]
    public ActionResult Report(List<mPageEventRecord> lst)
    {
        try
        {
            var em = GetPageEventManager();
              em.AddEvents(lst);
            return Ok($"{lst.Count} Events Saved..");
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("GetVisitorsPerCountry")]
    public ActionResult<List<IGrouping<string?, PageEventRecord>>> GetVisitorsPerCountry(int durationInminutes=1440)
    {
        try
        {
            var em = GetPageEventManager();
           var r= em.GetVisitorsPerCountry();
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    private PageEventManager GetPageEventManager()
    {
        var result = new PageEventManager();
        result.edbctx = ectx;
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
