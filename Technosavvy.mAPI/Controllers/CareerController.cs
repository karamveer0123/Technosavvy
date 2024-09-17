using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
namespace NavExM.Int.Maintenance.APIs.Controllers;
[ApiController]
[Route("[controller]")]
public class CareerController : sControllerBase
{
    /*ToDo: Naveen, Implement following more
     * 1. FAQ Related Functions
     *  Suggestion, Approval, Cancel, and other of that sort
     */
    public CareerController(ApiAppContext _ctx, CareerAppContext _jddbctx, IHttpContextAccessor _http, IOptions<SmtpConfig> _smtp)
    {
        ctx = _ctx;
        jddbctx = _jddbctx;
        httpContext = _http.HttpContext!;
        smtp = _smtp;
    }
    [HttpGet("GetPublishedJD")]
    public ActionResult GetJD(string RefNo)
    {
        try
        {
            var cm = GetCareerManager();
            var r = cm.GetPublishedJDByRefNo(RefNo);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("GetPublishedJDList")]
    public ActionResult GetJDList()
    {
        try
        {
            var cm = GetCareerManager();
            var r = cm.GetPublishedJDs();
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("UpdateJDUser")]
    public ActionResult UpdateJDUser(Guid Id)
    {
        try
        {
            var cm = GetCareerManager();
            var r = cm.RegisterViewer(Id);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    private CareerManager GetCareerManager()
    {
        var result = new CareerManager()
        {
            dbctx = ctx,
            JDdbctx = jddbctx,
            httpContext = httpContext
        };
        return result;
    }
}