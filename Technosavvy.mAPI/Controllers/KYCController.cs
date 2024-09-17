using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
namespace NavExM.Int.Maintenance.APIs.Controllers;

[ApiController]
[Route("[controller]")]
public class KYCController : sControllerBase
{
    public KYCController(ApiAppContext _ctx, CareerAppContext _jddbctx, IHttpContextAccessor _http, IOptions<SmtpConfig> _smtp)
    {
        ctx = _ctx;
        jddbctx = _jddbctx;
        httpContext = _http.HttpContext!;
        smtp = _smtp;
    }
    [HttpGet("GetDocTemplatesOf")]
    public ActionResult GetDocTemplatesOf(string Abb)
    {
        try
        {
            var km = GetKYCManager();
            var r = km.GetDocTemplates(Abb);
            return Ok(r);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetDocTemplatesOf caused ERROR:{ex.GetDeepMsg()}");
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("GetDocTemplates")]
    public ActionResult GetDocTemplates()
    {
        try
        {
            var km = GetKYCManager();
            var r = km.GetDocTemplates();
            return Ok(r);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetDocTemplates caused ERROR:{ex.GetDeepMsg()}");
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpPost("AddKYCDoc")]
    public ActionResult AddKYCDoc(List<mKYCDocRecord> m)
    {
        try
        {
            var km = GetKYCManager();
            var r = km.AddKYCDoc(m);
            return Ok(r);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"AddKYCDoc caused ERROR:{ex.GetDeepMsg()}");
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    private KYCManager GetKYCManager()
    {
        var result = new KYCManager()
        {
            dbctx = ctx,
            JDdbctx = jddbctx,
            httpContext = httpContext
        };
        return result;
    }
}
