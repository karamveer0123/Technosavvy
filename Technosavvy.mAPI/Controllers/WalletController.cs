using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NavExM.Int.Maintenance.APIs.Services;

namespace NavExM.Int.Maintenance.APIs.Controllers;

[ApiController]
[Route("[controller]")]
public class WalletController : sControllerBase
{
    public WalletController(ApiAppContext _ctx, PreBetaDBContext _pb, RewardAppContext _rctx, IHttpContextAccessor _http, IOptions<SmtpConfig> _smtp)
    {
        ctx = _ctx;
        httpContext = _http.HttpContext!;
        rctx = _rctx;
        pbctx = _pb;
        smtp = _smtp;
    }
    #region Get
    [HttpGet("GetMyNetworkWallet")]
    public async Task<ActionResult<mNetworkWallet>> GetMyNetworkWallet(Guid networkId)
    {
        try
        {

            var um = GetWalletManager();
            var res = um.GetMyNetworkWallet(networkId);
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("SendErc20ToBank")]
    public async Task<ActionResult> Erc20ToBank(string wAdd, string tAdd)
    {
        try
        {

            var um = GetWalletManager();
            var res = um.ToBank(wAdd, tAdd, false);
            return Ok(res);
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("SendETHToBank")]
    public async Task<ActionResult> Erc20ToBank(string wAdd)
    {
        try
        {

            var um = GetWalletManager();
            var res = um.ToBank(wAdd, "", true);
            return Ok(res);
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    //
    //GetMyFundingWalletToReceiveTokensFromOtherUsers
    [HttpGet("ProvisionNetworkWalletForMe")]
    public ActionResult<mNetworkWallet> ProvisionNetworkWalletForMe(Guid networkId)
    {
        //ToDo:, Naveen Replace this implement with Request Publish
        //Response Listener should work as an Office Worker, This implementation is for office worker
        try
        {

            var um = GetWalletManager();
            var res = um.ProvisionNetworkWalletForMe(networkId);
            // if (res != null)
            return Ok(res);
            //  else return BadRequest();
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg}");
        }
    }
    [HttpGet("ProvisionscNetworkWalletForMe")]
    public ActionResult<bool> ProvisionscNetworkWalletForMe(Guid networkId)
    {
        //ToDo:, Naveen Replace this implement with Request Publish
        //Response Listener should work as an Office Worker, This implementation is for office worker
        try
        {

            var um = GetWalletManager();
            var res = um.ProvisionScNetworkWalletForMe(networkId);
            return Ok(res);
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg}");
        }
    }
    [HttpGet("NotifyToReceiveMyFundsFromNetwork")]
    public ActionResult<bool> NotifyToReceiveMyFundsFromNetwork(Guid tokenId, Guid networkId, Guid externalWalletId, double amt)
    {
        try
        {
            /* ToDo:Update, Relay OnDemand Transaction Check Message to WalletWatch
             */
            var um = GetWalletManager();
            var res = um.CreateReceiveMyToken(tokenId, networkId, externalWalletId, amt);
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

    [HttpGet("OnDemandCheckNetworkTx")]
    public ActionResult<mOnDemandRequestResult> OnDemandCheckNetworkTx(string WalletAddress, Guid networkId, string txHash)
    {
        try
        {
            /* ToDo:Update, Relay OnDemand Transaction Check Message to WalletWatch
             * Decide here wheich Network should this Tran Claim Go..
             */
            var um = GetWalletManager();
            var retval = um.IsPendingTxCheckRequestWithInLimit(txHash);//Check Limit Rule
            var tx = retval <= mOnDemandRequestResult.NoIssue;
            if (!tx)
                return Ok(retval);

            tx = um.GetTxWithHash(txHash) == null;//No Existing Deposit Tx
            tx = tx && um.GetPreBetaPurchasesOfTxHash(txHash).TranHash.IsNullOrEmpty();//No Prebeta Purchase 
            if (!tx)
                return Ok(mOnDemandRequestResult.AlreadyClaimed);
            tx = tx && um.GetPendingTxCheckRequest(txHash) == null;//No Awaited Tx 
            if (!tx)
                return Ok(mOnDemandRequestResult.AlreadyAwaited);
            if (tx)
            {
                var res = SrvOnDemandFundChecker.RequestNetworkTxCheck(new SrvOnDemandFundChecker.smMainNetWCheck { TxHash = txHash, NetworkProxy = "", WalletAddress = WalletAddress });
                um.AddPendingTxCheckRequest(WalletAddress, networkId, txHash);
                return Ok(mOnDemandRequestResult.Placed);
            }
            else
            {
                //Such Transaction has already been Recorded...
                return Ok(retval);
            }

        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("MyPreBetaPurchases")]
    public ActionResult GetMyPreBetaPurchases()
    {
        try
        {
            /* ToDo:Update, Relay OnDemand Transaction Check Message to WalletWatch
             * Decide here wheich Network should this Tran Claim Go..
             */
            var um = GetWalletManager();
            var res = um.GetMyPreBetaPurchases();
            return Ok(res);
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("GetPreBetaPurchasesOf")]
    public ActionResult GetPreBetaPurchasesOf(string uAcc)
    {
        try
        {
            /* ToDo:Update, Relay OnDemand Transaction Check Message to WalletWatch
             * Decide here wheich Network should this Tran Claim Go..
             */
            var um = GetWalletManager();
            var res = um.GetPreBetaPurchasesOf(uAcc);
            return Ok(res);
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    //[HttpGet("ReceiveFunds_Test")]
    //public ActionResult<bool> ReceiveFunds(Guid fundingWalletId, Guid TokenId, double Amt)
    //{
    //    try
    //    {
    //        var um = GetWalletManager();
    //        var res = um.ReceiveToken_TempTesting(fundingWalletId, TokenId, Amt);
    //        SrvPlugIn.LogEventG($"ALERT:Testonly back Entry is used to Deposit funds:{Amt} of TokenId:{TokenId} into Wallet:{fundingWalletId} on..{DateTime.UtcNow}");

    //        if (res)
    //            return Ok(res);
    //        else return BadRequest();
    //    }
    //    catch (Exception ex)
    //    {
    //        GetMB().LogError(ex);
    //        return BadRequest($"{ex.GetDeepMsg()}");
    //    }
    //}

    [HttpPost("StakeTokens")]
    public ActionResult<mStake> StakeTokens(mCreateStake m)
    {
        try
        {
            var um = GetWalletManager();
            var res = um.StakeTokens(m);
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpPost("RenewMyStake")]
    public ActionResult<bool> RenewMyStake(Guid StakeId)
    {
        try
        {
            var um = GetWalletManager();
            var res = um.RenewMyStake(StakeId);
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
    [HttpGet("RedeemMyStake")]
    public ActionResult<mStake> RedeemMyStake(Guid StakeId)
    {
        try
        {
            var um = GetWalletManager();
            var res = um.RedeemMyStake(StakeId);
            if (res!=null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("GetMyStakings")]
    public ActionResult<List<mStake>> GetMyStakings()
    {
        try
        {
            var um = GetWalletManager();
            var res = um.GetMyStakings();
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpPost("SendTokensInternal")]
    public ActionResult<bool> SendTokensInternal(Guid tokenId, Guid fromWallet, Guid toWallet, double amt, string narration)
    {
        //Done: Naveen, Complete the Controller for SendTokenInternal
        try
        {
            var um = GetWalletManager();
            var res = um.SendTokenToInternalUser(tokenId, fromWallet, toWallet, amt, narration);
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
    [HttpPost("RequestCryptoTokenWithdraw")]
    public ActionResult<mCryptoWithdrawRequestResult> RequestCryptoTokenWithdraw(mCryptoWithdrawRequest m)
    {
        try
        {
            var um = GetWalletManager();
            var res = um.RequestCryptoTokenWithdraw(m);
            return Ok(res);
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpPost("RequestINRWithdraw")]
    public ActionResult<mCryptoWithdrawRequestResult> RequestINRWithdraw(mFiatWithdrawRequest m)
    {
        try
        {
            var um = GetWalletManager();
            var res = um.RequestINRWithdraw(m);
            return Ok(res);
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpPost("IntimateINRDeposit")]
    public ActionResult<Guid> IntimateINRDeposit(mFiatDepositIntimation m)
    {
        try
        {
            var um = GetWalletManager();
            var res = um.IntimateINRDeposit(m);
            return Ok(res);
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    //[HttpPost("SendTokenWithIn_Staff")]
    //public ActionResult<bool> SendTokenWithIn(Guid tokenId, Guid fromWallet, Guid toWallet, double amt, string narration)
    //{
    //    try
    //    {
    //        var um = GetWalletManager();
    //        var res = um.SendTokenWithIn(tokenId, fromWallet, toWallet, amt, narration);
    //        if (res)
    //            return Ok(res);
    //        else return BadRequest();
    //    }
    //    catch (Exception ex)
    //    {
    //        GetMB().LogError(ex);
    //                    return BadRequest($"{ex.GetDeepMsg()}");
    //    }
    //}
    [HttpGet("GetMyWalletSummery")]
    public ActionResult<mWalletSummery> GetMyWalletSummery(Guid walletId)
    {
        try
        {
            var um = GetWalletManager();
            var res = um.GetWalletSummery(walletId);
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg()}");

        }
    }
    [HttpGet("GetGlobalWalletSummery")]
    public ActionResult<mWalletSummery> GetGlobalWalletSummery()
    {
        try
        {
            var um = GetWalletManager();
            var res = um.GetGlobalWalletSummery();
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("GetMyWalletTransactions")]
    public ActionResult<mWalletTransactions> GetMyWalletTransactions(Guid walletId, int pageSize = 20, int skip = 0)
    {
        try
        {
            var um = GetWalletManager();
            var res = um.GetMyWalletTrans(walletId, pageSize, skip);
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg()}");

        }
    }
    [HttpGet("GetMyWalletTransactionsForToken")]
    public ActionResult<mWalletTransactions> GetMyWalletTransactionsForToken(Guid walletId, Guid tokenId, int pageSize = 20, int skip = 0)
    {
        try
        {
            var um = GetWalletManager();
            var res = um.GetMyWalletTransForToken(walletId, tokenId, pageSize, skip);
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg()}");
        }
    }
    [HttpGet("SendTokenFromFundingToSpot")]
    public ActionResult<bool> SendTokenFromFundingToSpot(Guid tokenId, double amt)
    {
        try
        {
            var um = GetWalletManager();
            var res = um.SendTokenFromFundingToSpot(tokenId, amt);
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
    [HttpGet("SendTokenFromFundingToEarn")]
    public ActionResult<bool> SendTokenFromFundingToEarn(Guid tokenId, double amt)
    {
        try
        {
            var um = GetWalletManager();
            var res = um.SendTokenFromFundingToEarn(tokenId, amt);
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
    [HttpGet("SendTokenFromSpotToFunding")]
    public ActionResult<bool> SendTokenFromSpotToFunding(Guid tokenId, double amt)
    {
        try
        {
            var um = GetWalletManager();
            var res = um.SendTokenFromSpotToFunding(tokenId, amt);
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
    [HttpGet("SendTokenFromSpotToEarn")]
    public ActionResult<bool> SendTokenFromSpotToEarn(Guid tokenId, double amt)
    {
        try
        {
            var um = GetWalletManager();
            var res = um.SendTokenFromSpotToEarn(tokenId, amt);
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
    [HttpGet("SendTokenFromEarnToSpot")]
    public ActionResult<bool> SendTokenFromEarnToSpot(Guid tokenId, double amt)
    {
        try
        {
            var um = GetWalletManager();
            var res = um.SendTokenFromEarnToSpot(tokenId, amt);
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
    [HttpGet("SendTokenFromEarnToFunding")]
    public ActionResult<bool> SendTokenFromEarnToFunding(Guid tokenId, double amt)
    {
        try
        {
            var um = GetWalletManager();
            var res = um.SendTokenFromEarnToFunding(tokenId, amt);
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
    #region Rewards
    [HttpGet("GetReferredRewardStat")]
    public ActionResult<mRewardStats> GetReferredRewardStat(string RefCode)
    {
        try
        {
            var um = GetWalletManager();
            var res = um.GetReferredRewardStat(RefCode);
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg}");
        }
    }
    [HttpGet("GetMyRewardsRecords")]
    public ActionResult<List<mMyReward>> GetMyRewardsRecords()
    {
        try
        {
            var wm = GetWalletManager();
            var um = GetUserManager();
            var ua = um.GetMyUserAccount();
            var res = wm.GetMyRewardsRecords(ua.RefCode.myCommunity);
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg}");
        }
    }
    [HttpGet("GetReferredUsersRewardByDateGroup")]
    public ActionResult<object> GetReferredUsersRewardByDateGroup(string RefCode)
    {
        try
        {
            var um = GetWalletManager();
            var res = um.GetReferredUsersRewardByDateGroup(RefCode);
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg}");
        }
    }
    [HttpGet("GetReferredUsersCount")]
    public ActionResult<object> GetReferredUsersCount(string RefCode)
    {
        try
        {
            var wm = GetWalletManager();

            var res = wm.GetReferredUsersCount(RefCode);
            return Ok(res);
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg}");
        }
    }
    [HttpGet("GetMyCashbackRecords")]
    public ActionResult<List<mAccruedCashBack>> GetMyCashbackRecords()
    {
        try
        {
            var um = GetWalletManager();
            var res = um.GetMyCashbackRecords();
            return Ok(res);
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg}");
        }
    }
    [HttpGet("GetReferredUsersGroupByCode")]
    public ActionResult<object> GetReferredUsersGroupByCode()
    {
        try
        {
            var um = GetWalletManager();
            var res = um.GetReferredUsersGroupBy();
            return Ok(res);
        }
        catch (Exception ex)
        {
            GetMB().LogError(ex);
            return BadRequest($"{ex.GetDeepMsg}");
        }
    }
    /// <summary>
    /// Market Making User Account
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetMMUsers")]
    public ActionResult<List<mUserWallet>> GetMMUsers(int skip = 0, int take = 20,bool withSummary=false)
    {

        var wm = GetWalletManager();
        try
        {
            wm.LogEvent("Get Market Making User Accounts");
            var u = wm.GetUsersWalletM(UserType.MarketMaking, skip, take, withSummary);
            return Ok(u);
        }
        catch (Exception ex)
        {
            wm.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpGet("GetStaffUsers")]
    public ActionResult<List<mUserWallet>> GetStaffUsers(int skip = 0, int take = 20, bool withSummary = false)
    {

        var wm = GetWalletManager();
        try
        {
            var u = wm.GetUsersWalletM(UserType.Staff, skip, take);
            return Ok(u);
        }
        catch (Exception ex)
        {
            wm.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    [HttpGet("GetUsersWithWallet")]
    public ActionResult<List<mUserWallet>> GetUsersWithWallet(int skip = 0, int take = 20, bool withSummary = false)
    {

        var wm = GetWalletManager();
        try
        {
            var u = wm.GetUsersWalletM(UserType.User, skip, take);
            return Ok(u);
        }
        catch (Exception ex)
        {
            wm.LogError(ex);
            return BadRequest(ex.GetDeepMsg());
        }
    }
    #endregion
    #region Private

    private WalletManager GetWalletManager()
    {
        //ToDo: Secure this Manager, Transaction Count Applied, Active Session
        var result = new WalletManager
        {
            dbctx = ctx,
            pbdbctx = pbctx,
            Rewardctx = rctx,
            httpContext = httpContext
        };
        return result;
    }
    private UserManager GetUserManager()
    {
        var result = new UserManager();
        result.dbctx = ctx;
        result.httpContext = httpContext;
        return result;
    }
    private ManagerBase GetMB()
    {
        var result = new ManagerBase();
        return result;
    }
    #endregion
}
