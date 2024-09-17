using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NavExM.Int.Maintenance.APIs.ServerModel;
using System.Drawing;

namespace NavExM.Int.Maintenance.APIs.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : sControllerBase
{
    /*ToDo: Naveen, Implement following more
     * 1. GetOrderStatus
     * 2. CancelOrder
     * 3. GetMyActiveOrders
     * 4. GetMyCompletedOrders
     * 5. GetMyOrderOfMarket/Tokens
     * 
     */
    public OrderController(ApiAppContext _ctx, IHttpContextAccessor _http, IOptions<SmtpConfig> _smtp)
    {
        ctx = _ctx;
        httpContext = _http.HttpContext!;
        smtp = _smtp;
    }
    [HttpPost("BuildAndPlaceOrder")]
    public ActionResult<Tuple<bool, string>> BuildAndPlaceOrder(mOrder order)
    {
        try
        {
            var om = GetOrderManager();
            var r = om.TryBuildAndPlaceOrder(order);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("CancelMyOrder")]
    public ActionResult<Tuple<bool, string>> CancelMyOrder(string orderId, string mCode)
    {
        try
        {
            var om = GetOrderManager();
            var r = om.ConfirmAndCancel(orderId, mCode);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("MyOpenOrder")]
    public ActionResult<List<rOrder>> MyOpenOrder(string mCode, string uAccount)
    {
        try
        {
            var om = GetOrderManager();
            var r = om.GetOpenOrdersOf(uAccount, mCode);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("MyOpenOrdersOf")]
    public ActionResult<List<rOrder>> MyOpenOrdersOf(string uAccount)
    {
        try
        {
            var om = GetOrderManager();
            var r = om.GetOpenOrdersOf(uAccount);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("MyOrderHistory")]
    public ActionResult<List<rOrder>> MyOrderHistory(string mCode, string uAccount)
    {
        try
        {
            var om = GetOrderManager();
            var r = om.GetOrdersHistoryOf(uAccount, mCode);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("MyOrderHistoryOf")]
    public ActionResult<List<rOrder>> MyOrderHistoryOf(string uAccount)
    {
        try
        {
            var om = GetOrderManager();
            var r = om.GetOrdersHistoryOf(uAccount);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("GetMySwapRateAndCashback")]
    public ActionResult<Tuple<double, double, double, double, double,int>> GetMySwapRateAndCashback(string mCode, string uAccount)
    {
        try
        {
            var om = GetOrderManager();
            var r = om.GetmyBuyTradingSwap( mCode, uAccount, out var buySWAP, out var CBRate, out var CBCommit, out var Limit, out var cstatus);
            r = om.GetmySellTradingSwap( mCode, uAccount, out var SellSWAP, out CBRate, out CBCommit, out  Limit, out  cstatus);
            //ToDo: It should provision buy rate and Sell Rate differntly
            if (r)
                return Ok(new Tuple<double, double, double, double,double,int>(buySWAP, SellSWAP, CBRate, CBCommit,Limit,(int)cstatus));
            return Ok(new Tuple<double, double, double, double, double,int>(0, 0, 0, 0,0,0));
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    /// <summary>
    /// Get Trading Rate for this User for this Market
    /// </summary>
    /// <param name="mCode">Market Code</param>
    /// <param name="uAccount">User Account</param>
    /// <returns></returns>
    [HttpGet("GetMySwapRate")]
    public ActionResult<Tuple<double, double,int>> GetMySwapRate(string mCode, string uAccount)
    {
        try
        {
            var om = GetOrderManager();
            var r = om.GetmyBuyTradingSwap(mCode, uAccount, out var buySWAP, out var CBRate, out var CBCommit, out var Limit,out var cStatus);
            r = om.GetmySellTradingSwap(mCode, uAccount, out var SellSWAP, out CBRate, out CBCommit, out Limit,out cStatus);
            //ToDo: It should provision buy rate and Sell Rate differntly
            if (r)
                return Ok(new Tuple<double, double,int>(buySWAP, SellSWAP,(int)cStatus));
            return Ok(new Tuple<double, double,int>(0, 0,0));
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("GetCBthreashold")]
    public ActionResult<double> GetCBthreashold(string mCode, string uAccount)
    {
        try
        {
            var om = GetOrderManager();
            var r = om.GetmyBuyTradingSwap(uAccount, mCode, out var buySWAP, out var CBRate, out var CBCommit, out var Limit,out var cStatus);
           
            // var r = om.GetCBthreashold(uAccount, mCode);
            if (r)
                return Ok(CBRate > CBCommit ? CBRate : CBCommit);
            return Ok(0);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
   
    //Swagger is doing some issue with this function, for now use object than model
    [HttpGet("MyRecentTrade")]
    public ActionResult<List<object>> MyRecentTrade(string uAccount, int count = -1)
    {
        try
        {
            var om = GetOrderManager();
            var r = om.GetTradesOf(uAccount, "", count);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("MyRecentTradeOf")]
    public ActionResult<List<object>> MyRecentTradeOf(string uAccount, string mCode, int count = -1)
    {
        try
        {
            var om = GetOrderManager();
            var r = om.GetTradesOf(uAccount, mCode, count);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("MyRecentBuyTradeOf")]
    public ActionResult<List<object>> MyRecentBuyTradeOf(string uAccount, string mCode, int count = -1)
    {
        try
        {
            var om = GetOrderManager();

            var r = om.GetBuyTradesOf(uAccount, mCode, count);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("MyRecentBuyTrade")]
    public ActionResult<List<object>> MyRecentBuyTrade(string uAccount, int count = -1)
    {
        try
        {
            var om = GetOrderManager();

            var r = om.GetBuyTradesOf(uAccount, "", count);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("MyRecentSellTradeOf")]
    public ActionResult<List<object>> MyRecentSellTradeOf(string uAccount, string mCode, int count = -1)
    {
        try
        {
            var om = GetOrderManager();
            var r = om.GetSellTradesOf(uAccount, mCode, count);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("MyRecentSellTrade")]
    public ActionResult<List<object>> MyRecentSellTrade(string uAccount, int count = -1)
    {
        try
        {
            var om = GetOrderManager();
            var r = om.GetSellTradesOf(uAccount, "", count);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpPost("ConvertToken")]
    public ActionResult<mConvertTokenRequest> ConvertToken(mConvertTokenRequest Req)
    {
        try
        {
            var om = GetOrderManager();
            var r = om.ConvertToken(Req);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    private OrderManager GetOrderManager()
    {
        var result = new OrderManager();
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
