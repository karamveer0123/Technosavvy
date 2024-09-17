using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
namespace NavExM.Int.Maintenance.APIs.Controllers;

[ApiController]
[Route("[controller]")]
public class FAQsController : sControllerBase
{
    /*ToDo: Naveen, Implement following more
     * 1. FAQ Related Functions
     *  Suggestion, Approval, Cancel, and other of that sort
     */
    public FAQsController(ApiAppContext _ctx, ContentAppContext _cctx, IHttpContextAccessor _http, IOptions<SmtpConfig> _smtp)
    {
        ctx = _ctx;
        cctx = _cctx;
        httpContext = _http.HttpContext!;
        smtp = _smtp;
    }
    [HttpGet("GetApprovedFAQs")]
    public ActionResult GetApprovedFAQs()
    {
        try
        {
            var cm = GetContentManager();
            var r = cm.GetAllApprovedFAQs();
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpPost("SavePreFAQ")]
    public ActionResult SavePreFAQ(mFAQDisplay vm)
    {
        try
        {
            var cm = GetContentManager();
            var r = cm.SaveFAQ(vm);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    private ContentManager GetContentManager()
    {
        var result = new ContentManager()
        {
            dbctx = ctx,
            cdbctx = cctx,
            httpContext = httpContext
        };
        return result;
    }
}
