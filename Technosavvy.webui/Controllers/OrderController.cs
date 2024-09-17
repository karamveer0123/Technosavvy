using Microsoft.AspNetCore.Mvc;
using TechnoApp.Ext.Web.UI.Service;
using System.Security.Principal;

namespace TechnoApp.Ext.Web.UI.Controllers
{
    [AfterProfile]
    [DenyAccessInPreBeta]
    [EnsureCompliantCountry]
    public class OrderController : Controller
    {
        private readonly ILogger<TradeController> _logger;
        private IOptions<SmtpConfig> _smtp;
        private IConfiguration _configuration;
        private HttpContext _context;
        private IHttpContextAccessor _accessor;
        IDataProtector _protector;
        AppSessionManager appSessionManager = null;

        public OrderController(ILogger<TradeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
        {
            _logger = logger;
            _smtp = smtp;
            _configuration = configuration;
            _accessor = accessor;
            _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
            appSessionManager = new AppSessionManager(accessor.HttpContext);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StopMarketOrderBuy(mMarketOrder order)
        {
            try
            {
                order.OrderId = DateTime.UtcNow.Ticks.ToString();
                var tm = await GetTradeManager();
                var res = await tm.BuildAndPlaceStopMarketOrder(Model.eOrderSide.Buy, order.mCode, order.baseCode, order.bTag, order.quoteCode, order.qTag, order.Amount, order.Price, order.stopPrice);

                return Json(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StopMarketOrderSell(mMarketOrder order)
        {
            try
            {
                order.OrderId = DateTime.UtcNow.Ticks.ToString();
                var tm = await GetTradeManager();
                var res = await tm.BuildAndPlaceStopMarketOrder(Model.eOrderSide.Sell, order.mCode, order.baseCode, order.bTag, order.quoteCode, order.qTag, order.Amount, order.Price, order.stopPrice);

                return Json(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StopLimitOrderBuy(mMarketOrder order)
        {
            try
            {
                order.OrderId = DateTime.UtcNow.Ticks.ToString();
                var tm = await GetTradeManager();
                var res = await tm.BuildAndPlaceStopLimitOrder(Model.eOrderSide.Buy, order.mCode, order.baseCode, order.bTag, order.quoteCode, order.qTag, order.Amount, order.Price,order.stopPrice);
                await APIHub.UpdateClientIfAny(appSessionManager.ExtSession.UserSession.UserAccount.AccountNumber, order.mCode);
                return Json(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StopLimitOrderSell(mMarketOrder order)
        {
            //if ((await DoesHaveAnActiveSession()) == false)
            //    return BadRequest("No User or Active Session. Please login..");
            try
            {
                order.OrderId = DateTime.UtcNow.Ticks.ToString();
                var tm = await GetTradeManager();
                var res = await tm.BuildAndPlaceStopLimitOrder(Model.eOrderSide.Sell, order.mCode, order.baseCode, order.bTag, order.quoteCode, order.qTag, order.Amount, order.Price, order.stopPrice);
                await APIHub.UpdateClientIfAny(appSessionManager.ExtSession.UserSession.UserAccount.AccountNumber, order.mCode);

                return Json(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: MarketController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MarketOrderBuy(mMarketOrder order)
        {
            //if ((await DoesHaveAnActiveSession()) == false)
            //    return BadRequest("No User or Active Session. Please login..");

            try
            {
                order.OrderId = DateTime.UtcNow.Ticks.ToString();
                var tm = await GetTradeManager();
                var res = await tm.BuildAndPlaceMarketOrder(Model.eOrderSide.Buy, order.mCode, order.baseCode, order.bTag, order.quoteCode, order.qTag, order.Amount, order.Price);

                return Json(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MarketOrderSell(mMarketOrder order)
        {
            //if ((await DoesHaveAnActiveSession()) == false)
            //    return BadRequest("No User or Active Session. Please login..");
            try
            {
                order.OrderId = DateTime.UtcNow.Ticks.ToString();
                var tm = await GetTradeManager();
                var res = await tm.BuildAndPlaceMarketOrder(Model.eOrderSide.Sell, order.mCode, order.baseCode, order.bTag, order.quoteCode, order.qTag, order.Amount, order.Price);

                return Json(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LimitOrderBuy(mMarketOrder order)
        {
            //if ((await DoesHaveAnActiveSession()) == false)
            //    return BadRequest("No User or Active Session. Please login..");

            try
            {
                order.OrderId = DateTime.UtcNow.Ticks.ToString();
                var tm = await GetTradeManager();
                var res = await tm.BuildAndPlaceLimitOrder(Model.eOrderSide.Buy, order.mCode, order.baseCode, order.bTag, order.quoteCode, order.qTag, order.Amount, order.Price);
                await APIHub.UpdateClientIfAny(appSessionManager.ExtSession.UserSession.UserAccount.AccountNumber, order.mCode);
                return Json(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LimitOrderSell(mMarketOrder order)
        {
            //if ((await DoesHaveAnActiveSession()) == false)
            //    return BadRequest("No User or Active Session. Please login..");
            try
            {
                order.OrderId = DateTime.UtcNow.Ticks.ToString();
                var tm = await GetTradeManager();
                var res = await tm.BuildAndPlaceLimitOrder(Model.eOrderSide.Sell, order.mCode, order.baseCode, order.bTag, order.quoteCode, order.qTag, order.Amount, order.Price);
                await APIHub.UpdateClientIfAny(appSessionManager.ExtSession.UserSession.UserAccount.AccountNumber, order.mCode);

                return Json(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OpenOrders()
        {
            if ((await DoesHaveAnActiveSession()) == false)
                return BadRequest("No User or Active Session. Please login..");
            var tm = new TradeManager();
            var oo = await tm.GetMyOpenOrders("test");
            return Json(oo);
        }
        private async Task<bool> DoesHaveAnActiveSession()
        {
            await appSessionManager.ExtSession.LoadSession();
            return appSessionManager.ExtSession.IsValid;
        }
        public async Task<IActionResult> EnsureValidSession()
        {
            await appSessionManager.ExtSession.LoadSession();
            if (!appSessionManager.ExtSession.IsValid)
                return RedirectToAction("Login", "Login");

            return null;
        }
        internal async Task<SettingsManager> GetSettingsManager()
        {
            await EnsureValidSession();

            var Mgr = new SettingsManager();
            Mgr._configuration = _configuration;
            Mgr._http = _accessor.HttpContext;
            Mgr._appSessionManager = appSessionManager;
            Mgr._DataProtector = _protector;
            return Mgr;
        }
        internal async Task<TradeManager> GetTradeManager()
        {
            await EnsureValidSession();

            var Mgr = new TradeManager();
            Mgr._configuration = _configuration;
            Mgr._http = _accessor.HttpContext;
            Mgr._appSessionManager = appSessionManager;
            return Mgr;
        }
    }
}
