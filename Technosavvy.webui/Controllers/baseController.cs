using Microsoft.AspNetCore.Mvc;
namespace TechnoApp.Ext.Web.UI.Controllers;

public class baseController : Controller
{
    // private readonly ILogger<SettingsController> _logger;
    internal IOptions<SmtpConfig> _smtp;
    internal IConfiguration _configuration;
    internal HttpContext _context;
    internal IHttpContextAccessor _accessor;
    internal IDataProtector _protector;
    internal AppSessionManager appSessionManager = null;
    public IActionResult ErrorCatchAndReport(Exception ex)
    {
        var id = System.Diagnostics.Activity.Current.Id;
        Console.WriteLine($"TraceId:{id}\n{ex.GetDeepMsg()}");
        return RedirectToAction("Error", new ErrorViewModel() { RequestId = id, Msg = "Internal Server Error" });
    }
     
}
