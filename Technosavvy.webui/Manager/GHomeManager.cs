using TechnoApp.Ext.Web.UI.Models;

namespace TechnoApp.Ext.Web.UI.Manager
{
    //public class GHomeManager
    //{
    //    private IHttpContextAccessor _accessor;
    //    private HttpContext _httpContext;
    //    public GHomeManager(IHttpContextAccessor accessor)
    //    {
    //        _accessor = accessor;
    //        _httpContext = accessor.HttpContext;
    //    }
    //    internal CFGeoVm SetGeoLOcation()
    //    {
    //        CFGeoVm cFGeo = new CFGeoVm();
    //        if (!string.IsNullOrEmpty(_httpContext.Request.Headers["cf-ipcountry"]))
    //        {
    //            cFGeo.IP = _httpContext.Request.Headers["CF-Connecting-IP"];
    //            cFGeo.CountryCode = _httpContext.Request.Headers["CF-IPCountry"];
    //            cFGeo.City = _httpContext.Request.Headers["cf-ipcity"];
    //            cFGeo.Ipcontinent = _httpContext.Request.Headers["cf-ipcontinent"];
    //            cFGeo.Longitude = _httpContext.Request.Headers["cf-iplongitude"];
    //            cFGeo.Latitude = _httpContext.Request.Headers["cf-iplatitude"];
    //        }
    //        return cFGeo;
    //    }
    //}
}
