using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NavExM.Int.Maintenance.APIs.ServerModel;

namespace NavExM.Int.Maintenance.APIs.Controllers;

[ApiController]
[Route("[controller]")]
public class StaffController : sControllerBase
{
   
    #region Staking
    public StaffController(ApiAppContext _ctx, PreBetaDBContext _pb, IHttpContextAccessor _http, IOptions<SmtpConfig> _smtp)
    {
        pbctx = _pb;
        ctx = _ctx;
        httpContext = _http.HttpContext!;
        smtp = _smtp;
    }
    [HttpPost("CreateStakingSlot")]
    public ActionResult<Tuple<bool, string>> CreateStakingSlot(mStakingSlot mSlot)
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.CreateStakingSlot(mSlot);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("ApproveStakingSlot")]
    public ActionResult<Tuple<bool, string>> ApproveStakingSlot(Guid GroupId)
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.ApproveStakingSlot(GroupId);
            return Ok (r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("GetPendingApprovalStakingSlot")]
    public ActionResult<List<mStakingSlot>> GetPendingApprovalStakingSlot(int pageSize = 5, int skip = 0)
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.GetPendingApprovalForStakingSlots(pageSize, skip);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("GetDraftStakingSlot")]
    public ActionResult<List<mStakingSlot>> GetDraftStakingSlot(int pageSize=5,int skip=0)
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.GetDraftStakingSlot(pageSize, skip);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("GetActiveStakingSlotsAll")]
    public ActionResult<List<mStakingSlot2>> GetActiveStakingSlotsAll()
    {
        try
        {


            var sm = GetStaffManager();
            var r = sm.GetActiveStakingSlots();
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("GetActiveStakingSlots")]
    public ActionResult<List<mStakingSlot2>> GetActiveStakingSlots(int pageSize = 0, int skip = 0)
    {
        try
        {


            var sm = GetStaffManager();
            var r = sm.GetActiveStakingSlots(pageSize, skip);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }

    [HttpGet("GetStakingSlots")]
    public ActionResult<List<mStakingSlot2>> GetStakingSlots(Guid groupId)
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.GetStakingSlots(groupId);
            return (r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("GetActiveStakingSlotsOf")]
    public ActionResult<List<mStakingSlot2>> GetActiveStakingSlotsOf(string tCode)
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.GetActiveStakingSlotsOf(tCode);
            return (r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }

    //-------- Stakings
    [HttpPost("Job_RenewAllStakingsThatAreDue")]
    public ActionResult<mStakingSlot> Job_RenewAllStakingsThatAreDue(int pageSize = 5, int skip = 0, int HowManyPages = 1)
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.Job_RenewAllStakingsThatAreDue(pageSize, skip, HowManyPages);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpPost("Job_ReFundAllStakingsThatAreDue")]
    public ActionResult<mStakingSlot> Job_ReFundAllStakingsThatAreDue(int pageSize = 5, int skip = 0, int HowManyPages = 1)
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.Job_ReFundAllStakingsThatAreDue(pageSize, skip, HowManyPages);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpPost("Job_RenewAllStakingsThatAreDueForToken")]
    public ActionResult<mStakingSlot> Job_RenewAllStakingsThatAreDueForToken(Guid TokenId,int pageSize = 5, int skip = 0, int HowManyPages = 1)
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.Job_RenewAllStakingsThatAreDueForToken(TokenId,pageSize, skip, HowManyPages);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpPost("Job_ReFundAllStakingsThatAreDueForToken")]
    public ActionResult<mStakingSlot> Job_ReFundAllStakingsThatAreDueForToken(Guid TokenId, int pageSize = 5, int skip = 0, int HowManyPages = 1)
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.Job_ReFundAllStakingsThatAreDueForToken(TokenId,pageSize, skip, HowManyPages);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpPost("GetActiveStaking")]
    public ActionResult<mStakingSlot> GetActiveStakings(int pageSize = 5, int skip = 0)
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.GetActiveStakings(pageSize, skip);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("GetRenewalDueStakings")]
    public ActionResult<mStakingSlot> GetRenewalDueStakings(int pageSize = 5, int skip = 0)
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.GetRenewalDueStakings(pageSize, skip);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }

    [HttpPost("GetRefundDueOrOverDueStakings")]
    public ActionResult<mStakingSlot> GetRefundDueOrOverDueStakings(int pageSize = 5, int skip = 0)
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.GetRefundDueOrOverDueStakings(pageSize, skip);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    #endregion
    #region Token
    [HttpPost("CreateToken")]
    public ActionResult<Tuple<bool, string>> CreateToken(mToken m)
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.CreateToken(m);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpPost("CreateSupportedToken")]
    public ActionResult<Tuple<bool, string>> CreateSupportedToken(mSupportedToken m)
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.CreateSupportToken(m);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
   
    #endregion

    #region Market

    [HttpPost("CreateMarket")]
    public ActionResult<Tuple<bool, string>> CreateMarket(mMarket m)
    {
        try
        {
            var sm = GetMarketManager();
            var r = sm.CreateMarket(m);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }

    [HttpGet("ApproveMarket")]
    public ActionResult<Tuple<bool, string>> ApproveMarket(Guid id)
    {
        try
        {
            var sm = GetMarketManager();
            var r = sm.ApproveMarketPair(id);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    
    [HttpGet("GetAllPendingApprovelMarketPair")]
    public ActionResult<List<mMarket>> GetAllPendingApprovelMarketPair()
    {
        try
        {
            var sm = GetMarketManager();
            var r = sm.GetAllPendingApprovelMarketPair();
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("GetMarketsOfToken")]
    public ActionResult<List<mMarket>> GetMarketsOfToken(Guid id)
    {
        try
        {
            var sm = GetMarketManager();
            var r = sm.GetMarketsOfToken(id);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("GetAllMarketSettlementServices")]
    public ActionResult<List<MarketInstances>> GetAllMarketSettlementServices()
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.GetAllMarketSettlementServices();
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("SuspendMarket")]
    public ActionResult<Tuple<bool, string>> SuspendMarket(Guid id)
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.SuspendMarket(id);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("RevokeSuspensionOfMarket")]
    public ActionResult<Tuple<bool, string>> RevokeSuspensionOfMarket(Guid id)
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.RevokeSuspensionMarket(id);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpPost("SWITCHInstances")]
    public ActionResult<Tuple<bool, string>> SWITCHSettlementInstance(string mCode, Guid Current, Guid Proposed)
    {
        try
        {
            var sm = GetStaffManager();
            var r = sm.SWITCHSettlementInstance(mCode,Current,Proposed);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    [HttpGet("GetLiveMarkets")]
    public ActionResult<List<string>> GetLiveMarkets()
    {
        try
        {
            var sm = GetMarketManager();
            var r = sm.GetLiveMarketPair();
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    #endregion

    #region Reports Prebeta
    //PreBeta Report API
    [HttpGet("GetNavCSecured")]
    public  ActionResult<mStakingSlot> GetNavCSecured(int pageSize = 500, int skip = 0)
    {
        try
        {
            var sm = GetWalletManager();
            var r = sm.GetPreBetaPurchases(pageSize, skip);
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    //PreBeta Report API
    [HttpGet("GetCurrentUserCount")]
    public async Task<ActionResult> GetCurrentUserCount()
    {
        try
        {
            var sm = GetUserManager();
            var r = await sm.GetUserCount();
            return Ok(r);
        }
        catch (Exception ex)
        {
            var Msg = GetMB().LogError(ex);
            return BadRequest(Msg);
        }
    }
    #endregion
    #region  Staff Move Internal Tokens 
    [HttpGet("SendTokenFromGlobalToWallet")]
    public ActionResult<bool> SendTokenFromGlobalToWallet(Guid toWalletId,Guid tokenId, double amt, eGlobalPaymentType reason)
    {
        try
        {
            var um = GetWalletManager();
            var res = um.SendTokenFromGlobalToWallet(toWalletId, tokenId, amt,reason);
            if (res)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    #endregion
    #region Private
    private ManagerBase GetMB()
    {
        var result = new ManagerBase();
        return result;
    }
    private StaffManager GetStaffManager()
    {
        var result = new StaffManager();
        result.dbctx = ctx;
        result.httpContext = httpContext;
        return result;
    }
    private MarketManager GetMarketManager()
    {
        var result = new MarketManager();
        result.dbctx = ctx;
        result.httpContext = httpContext;
        return result;
    }
    private PreBetaManager GetPMManager()
    {
        var result = new PreBetaManager()
        {
            pbdbctx = pbctx,
            dbctx = ctx,
            cdbctx = cctx,
            httpContext = httpContext
        };
        return result;
    }
    private WalletManager GetWalletManager()
    {
        var result = new WalletManager()
        {
            pbdbctx = pbctx,
            dbctx = ctx,
            cdbctx = cctx,
            httpContext = httpContext
        };
        return result;
    }
    private UserManager GetUserManager()
    {
        var result = new UserManager()
        {
            pbdbctx = pbctx,
            dbctx = ctx,
            cdbctx = cctx,
            httpContext = httpContext
        };
        return result;
    }
    #endregion
}