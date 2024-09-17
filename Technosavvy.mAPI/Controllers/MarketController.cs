using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NavExM.Int.Maintenance.APIs.Services;

namespace NavExM.Int.Maintenance.APIs.Controllers;
[ApiController]
[Route("[controller]")]
public class MarketController : sControllerBase
{
    /*ToDo: Naveen, Implement following more
     * 1. Suspend Live Market
     * 2. Get List of Live Markets
     * 3. Get Live Market Performace Statics
     * 4. Get Live Market Health Statics
     * 5. Get Live Market Shared Resources
     *      -Group of other Live markets being taken care of by Same Process
     * 
     */
    public MarketController(ApiAppContext _ctx, IHttpContextAccessor _http, IOptions<SmtpConfig> _smtp)
    {
        ctx = _ctx;
        httpContext = _http.HttpContext!;
        smtp = _smtp;
    }
    [HttpGet("GetLiveMarkets")]
    public ActionResult<List<string>> GetLiveMarkets(int pageSize = 20, int skip = 0)
    {
        try
        {
            var sm = GetMarketManager();
            var r = sm.GetLiveMarketPair(pageSize, skip);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.Message} inner->{ex.InnerException}");
        }
    }
    [HttpGet("GetActiveMarkets")]
    public ActionResult<List<mMarket>> GetActiveMarkets(int pageSize = 20, int skip = 0)
    {
        try
        {
            var sm = GetMarketManager();
            var r = sm.GetAllActiveMarketPair(pageSize, skip);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.Message} inner->{ex.InnerException}");
        }
    }
    [HttpGet("GetAllActiveMarketQuoteToken")]
    public ActionResult<List<mMarket>> GetAllActiveMarketQuoteToken(int pageSize = 20, int skip = 0)
    {
        try
        {
            var sm = GetMarketManager();
            var r = sm.GetAllActiveMarketQuoteToken(true, pageSize, skip);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.Message} inner->{ex.InnerException}");
        }
    }
    [HttpGet("GetAllActiveMarketPairOfQuote")]
    public ActionResult<List<mMarket>> GetAllActiveMarketPairOfQuote(string qCode, int pageSize = 20, int skip = 0)
    {
        try
        {
            var sm = GetMarketManager();
            var r = sm.GetAllActiveMarketPairOfQuote(qCode, true, pageSize, skip);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.Message} inner->{ex.InnerException}");
        }
    }

    [HttpGet("GetActiveMarketsFor")]
    public ActionResult<List<mMarket>> GetActiveMarketsForCountry(Guid CountryId)
    {
        try
        {
            var sm = GetMarketManager();
            var r = sm.GetAllActiveMarketPairOf(CountryId);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("GetActiveMarketsForCountry")]
    public ActionResult<List<mMarket>> GetActiveMarketsForCountry(string Abbrivation)
    {
        try
        {
            var sm = GetMarketManager();
            var r = sm.GetAllActiveMarketPairOf(Abbrivation);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("Get24HrChangeOfToken")]
    public ActionResult<PeriodicChange> Get24HrChangeOfToken(string tCode)
    {
        try
        {
            var r = Srv24HrsChangeWatch.GetAll24HrChangeOfToken(tCode);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("Get24HrChangeOfMarket")]
    public ActionResult<PeriodicChange> Get24HrChangeOfMarket(string mCode)
    {
        try
        {
            var r = Srv24HrsChangeWatch.GetAll24HrChangeOfMarket(mCode);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("GetAll24HrChange")]
    public ActionResult<List<PeriodicChange>> GetAll24HrChange()
    {
        try
        {
            var r = Srv24HrsChangeWatch.GetAll24HrChange();
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }

    [HttpGet("GetMarketPairByCode")]
    public ActionResult<mMarket> GetMarketPair(string mCode)
    {
        try
        {
            var sm = GetMarketManager();
            var r = sm.GetMarketPair(mCode);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("GetMarketPairById")]
    public ActionResult<List<mMarket>> GetMarketPair(Guid id)
    {
        try
        {
            var sm = GetMarketManager();
            var r = sm.GetMarketPair(id);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("CreateMarketMakingAccountFor")]
    public ActionResult CreateMarketMakingAccountFor(string mCode)
    {
        try
        {
            var sm = GetMarketManager();
            var r = sm.ValidateAndCreateMarketMakingAccount(mCode);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
//#if (DEBUG)
//    [HttpPost("PlaceRndTestBuyOrder")]
//    public ActionResult<Tuple<bool, string>> PlaceRndTestBuyOrder()
//    {
//        try
//        {
//            var sm = GetMarketManager();
//            var r = sm.PlaceTestBuyOrder(DateTime.UtcNow);
//            return Ok(r);
//        }
//        catch (Exception ex)
//        {
//            var Msg = GetMB().LogError(ex);
//            return BadRequest(Msg);
//        }
//    }
//    [HttpPost("PlaceRndTestSellOrder")]
//    public ActionResult<Tuple<bool, string>> PlaceRndTestSellOrder()
//    {
//        try
//        {
//            var sm = GetMarketManager();
//            var r = sm.PlaceTestSellOrder(DateTime.UtcNow);
//            return Ok(r);
//        }
//        catch (Exception ex)
//        {
//            var Msg = GetMB().LogError(ex);
//            return BadRequest(Msg);
//        }
//    }
//    [HttpPost("PlaceRndTestBuyOrderFor")]
//    public ActionResult<List<Tuple<bool, string>>> PlaceRndTestBuyOrderFor(int NoOfOrders, string BCode, string QCode)
//    {
//        try
//        {
//            var sm = GetMarketManager();
//            List<Tuple<bool, string>> result = new List<Tuple<bool, string>>();
//            while (NoOfOrders > 0)
//            {
//                var r = sm.PlaceTestBuyOrder(DateTime.UtcNow, BCode, QCode);
//                result.Add(r);
//                NoOfOrders--;
//            }
//            return Ok(result);
//        }
//        catch (Exception ex)
//        {
//            var Msg = GetMB().LogError(ex);
//            return BadRequest(Msg);
//        }
//    }
//    [HttpPost("PlaceRndTestSellOrderFor")]
//    public ActionResult<List<Tuple<bool, string>>> PlaceRndTestSellOrderFor(int NoOfOrders, string BCode, string QCode)
//    {
//        try
//        {
//            var sm = GetMarketManager();
//            List<Tuple<bool, string>> result = new List<Tuple<bool, string>>();
//            while (NoOfOrders > 0)
//            {
//                var r = sm.PlaceTestSellOrder(DateTime.UtcNow, BCode, QCode);
//                result.Add(r);
//                NoOfOrders--;
//            }
//            return Ok(result);
//        }
//        catch (Exception ex)
//        {
//            var Msg = GetMB().LogError(ex);
//            return BadRequest(Msg);
//        }
//    }
//    [HttpPost("PlaceRndTestBuyOrderForDate")]
//    public ActionResult<List<Tuple<bool, string>>> PlaceRndTestBuyOrderForDate(int NoOfOrders, int PastDays, string BCode, string QCode)
//    {
//        try
//        {
//            var sm = GetMarketManager();
//            List<Tuple<bool, string>> result = new List<Tuple<bool, string>>();
//            while (NoOfOrders > 0)
//            {
//                var r = sm.PlaceTestBuyOrder(DateTime.UtcNow.AddDays(-PastDays), BCode, QCode);
//                result.Add(r);
//                NoOfOrders--;
//            }
//            return Ok(result);
//        }
//        catch (Exception ex)
//        {
//            var Msg = GetMB().LogError(ex);
//            return BadRequest(Msg);
//        }
//    }
//    [HttpPost("PlaceRndTestSellOrderForDate")]
//    public ActionResult<List<Tuple<bool, string>>> PlaceRndTestSellOrderForDate(int NoOfOrders, int PastDays, string BCode, string QCode)
//    {
//        try
//        {
//            var sm = GetMarketManager();
//            List<Tuple<bool, string>> result = new List<Tuple<bool, string>>();
//            while (NoOfOrders > 0)
//            {
//                var r = sm.PlaceTestSellOrder(DateTime.UtcNow.AddDays(-PastDays), BCode, QCode);
//                result.Add(r);
//                NoOfOrders--;
//            }
//            return Ok(result);
//        }
//        catch (Exception ex)
//        {
//            var Msg = GetMB().LogError(ex);
//            return BadRequest(Msg);
//        }
//    }
//#endif

    private MarketManager GetMarketManager()
    {
        var result = new MarketManager();
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
