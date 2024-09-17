using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.IdentityModel.Tokens;
using NavExM.Int.Maintenance.APIs.Data.Entity;
using NavExM.Int.Maintenance.APIs.ServerModel;
using NavExM.Int.Maintenance.APIs.Services;
using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;

namespace NavExM.Int.Maintenance.APIs.Manager
{
    internal class WalletManager : ManagerBase
    {
        public List<mPreBetaPurchases> GetPreBetaPurchases(int PageSize = 500, int Skip = 0)
        {
            if (PageSize < 500 || PageSize > 10000) PageSize = 500;
            if (Skip < 0) Skip = 0;
            else
                Skip = Skip * PageSize;
            if (Skip <= 0)
                return pbdbctx.PurchaseRecords.Where(x => x.NavCAmountPurchased > 0).OrderBy(x => x.DateOf).Take(PageSize).ToList().ToModel();
            return pbdbctx.PurchaseRecords.Where(x => x.NavCAmountPurchased > 0).OrderBy(x => x.DateOf).Skip(Skip).Take(PageSize).ToList().ToModel();

        }
        public List<mPreBetaPurchases> GetMyPreBetaPurchases()
        {
            var um = GetUserManager();
            var ua = um.GetMyUserAccount();
            var retval = GetPreBetaPurchasesOf(ua.FAccountNumber); //pbdbctx.PurchaseRecords.Where(x => x.userAccount == ua.FAccountNumber).ToList().ToModel();
            return retval;

        }
        public List<mPreBetaPurchases> GetPreBetaPurchasesOf(string uAcc)
        {
            uAcc.CheckAndThrowNullArgumentException();
            uAcc = uAcc.ToLower();
            var retval = pbdbctx.PurchaseRecords.Where(x => x.userAccount.ToLower() == uAcc).ToList().ToModel();
            return retval;
        }
        public mPreBetaPurchases GetPreBetaPurchasesOfTxHash(string txhash)
        {
            txhash.CheckAndThrowNullArgumentException();
            var retval = pbdbctx.PurchaseRecords.FirstOrDefault(x => x.TranHash.ToLower() == txhash).ToModel();
            return retval;
        }
        internal eWalletTransaction GetTxWithHash(string Txhash)
        {
            var tx = dbctx.WalletTransaction.FirstOrDefault(x => x.Narration.Length > 0 && x.Narration.ToLower() == Txhash.ToLower());
            return tx;
        }
        internal mOnDemandRequestResult IsPendingTxCheckRequestWithInLimit(string NetworkWallet)
        {
            //mOnDemandRequestResult retval;
            NetworkWallet.CheckAndThrowNullArgumentException();
            NetworkWallet = NetworkWallet.ToLower();
            var lst = dbctx.OnDemandTxCheckRequest.Where(x => x.NetworkWallet.ToLower() == NetworkWallet).ToList();

            if (lst.Count > 20) { return mOnDemandRequestResult.TotalLimitIssue; }

            if (lst.Where(x => x.CreatedOn.Date == DateTime.UtcNow.Date).Count() >= 3) return mOnDemandRequestResult.DailyLimitIssue;

            return mOnDemandRequestResult.NoIssue;
        }
        internal bool ConfirmPendingTxCheckRequest(string Txhash)
        {
            //Clear All Tx before 10 days
            //Clear this Tx Has Transaction
            var tx = dbctx.OnDemandTxCheckRequest.Where(x => x.txHash.ToLower() == Txhash.ToLower() || x.CreatedOn < DateTime.UtcNow.Date.AddDays(-10)).ExecuteDelete();
            return tx >= 1;
        }
        internal bool AddPendingTxCheckRequest(string WalletAddress, Guid networkId, string Txhash)
        {
            dbctx.OnDemandTxCheckRequest.Add(new eOnDemandTxCheckRequest
            {
                txHash = Txhash,
                NetworkId = networkId,
                NetworkWallet = WalletAddress
            });
            return dbctx.SaveChanges() > 0;

        }
        internal eOnDemandTxCheckRequest? GetPendingTxCheckRequest(string Txhash)
        {
            var tx = dbctx.OnDemandTxCheckRequest.FirstOrDefault(x => x.txHash.ToLower() == Txhash.ToLower());
            return tx;
        }
        //pagezie i.e. 10 rows | skip 2 pages=> 20 rows will be skipped
        internal List<mWalletTransactions> GetMyWalletTrans(Guid walletId, int pageSize = 20, int skip = 0)
        {
            if (pageSize <= 0 || pageSize > 100) throw new ArgumentException("Invalid Page size Requested");
            var name = GetWalletType(walletId).ToString();
            var result = (new mWalletTransactions[] { new mWalletTransactions { TransactionId = Guid.Empty } })
                .Union(
                from f in dbctx.WalletTransaction
                join a in dbctx.Token on f.TokenId equals a.TokenId
                where f.FromWalletId == walletId
                select new mWalletTransactions
                {
                    wName = name,
                    TransactionId = f.WalletTransactionId,
                    TokenCode = a.Code,
                    TokenId = a.TokenId,
                    IsFiatRepresentative = a.IsFiatRepresentative,
                    TokenName = a.ShortName,
                    Date = f.Date,
                    Narration = f.Narration,
                    Amount = -(f.TAmount),
                    isFrom = true,
                    IsWithInMyWallet = f.IsWithInMyWallet,
                    Balance = f.FromWalletAfterTransactionBalance
                })
                         .Union
                         (from t in dbctx.WalletTransaction.Where(x => x.ToWalletId == walletId)
                          join a in dbctx.Token on t.TokenId equals a.TokenId
                          select new mWalletTransactions
                          {
                              wName = name,
                              TransactionId = t.WalletTransactionId,
                              TokenCode = a.Code,
                              TokenId = a.TokenId,
                              IsFiatRepresentative = a.IsFiatRepresentative,
                              TokenName = a.ShortName,
                              Date = t.Date,
                              Narration = t.Narration,
                              Amount = t.TAmount,
                              IsWithInMyWallet = t.IsWithInMyWallet,
                              Balance = t.ToWalletAfterTransactionBalance
                          })
                         .OrderBy(x => x.Date).Skip(skip * pageSize).Take(pageSize).ToList();


            //    (dbctx.WalletTransaction.Where(x => x.FromWalletId == walletId)
            //    uni
            //  dbctx.WalletTransaction.Where(x => x.ToWallet == walletId) )
            //var result = dbctx.WalletTransaction.Where(x => x.FromWalletId == walletId || x.ToWallet == walletId).Skip(skip * pageSize).Take(pageSize).ToList().ToModel();
            return result;
        }

        //pagezie i.e. 10 rows | skip 2 pages=> 20 rows will be skipped
        internal List<mWalletTransactions> GetMyWalletTransForToken(Guid walletId, Guid TokenId, int pageSize = 20, int skip = 0)
        {
            if (pageSize <= 0 || pageSize > 100) throw new ArgumentException("Invalid Page size Requested");
            var name = GetWalletType(walletId).ToString();
            var result = (new mWalletTransactions[] { new mWalletTransactions { TransactionId = Guid.Empty } })
                .Union(
                from f in dbctx.WalletTransaction
                join a in dbctx.Token on f.TokenId equals a.TokenId
                where f.FromWalletId == walletId && f.TokenId == TokenId
                select new mWalletTransactions
                {
                    wName = name,
                    TransactionId = f.WalletTransactionId,
                    TokenCode = a.Code,
                    TokenId = a.TokenId,
                    IsFiatRepresentative = a.IsFiatRepresentative,
                    TokenName = a.ShortName,
                    Date = f.Date,
                    Narration = f.Narration,
                    Amount = -f.TAmount,
                    Balance = f.FromWalletAfterTransactionBalance
                })
                         .Union
                         (from t in dbctx.WalletTransaction.Where(x => x.ToWalletId == walletId && x.TokenId == TokenId)
                          join a in dbctx.Token on t.TokenId equals a.TokenId
                          select new mWalletTransactions
                          {
                              wName = name,
                              TransactionId = t.WalletTransactionId,
                              TokenCode = a.Code,
                              TokenId = a.TokenId,
                              IsFiatRepresentative = a.IsFiatRepresentative,
                              TokenName = a.ShortName,
                              Date = t.Date,
                              Narration = t.Narration,
                              Amount = t.TAmount,
                              Balance = t.ToWalletAfterTransactionBalance
                          })
                         .OrderBy(x => x.Date).Skip(skip * pageSize).Take(pageSize).ToList();


            //    (dbctx.WalletTransaction.Where(x => x.FromWalletId == walletId)
            //    uni
            //  dbctx.WalletTransaction.Where(x => x.ToWallet == walletId) )
            //var result = dbctx.WalletTransaction.Where(x => x.FromWalletId == walletId || x.ToWallet == walletId).Skip(skip * pageSize).Take(pageSize).ToList().ToModel();
            return result;
        }
        internal eInternalWallet GetGlobalRewardWallet()
        {
            //Market Global Wallet
            var g = dbctx.InternalWallet.FirstOrDefault(x => x.BelongsTo.ToLower() == "NavC".ToLower() && x.WalletNature == eWalletNature.Rewards && x.WalletType == eInternalWalletType.Cashback && x.GlobalId == null);
            if (g == null)
            {
                g = new eInternalWallet { Name = "Global Reward Wallet", BelongsTo = "NavC", WalletNature = eWalletNature.Rewards, WalletType = eInternalWalletType.Cashback };

                dbctx.InternalWallet.Add(g);
                dbctx.SaveChanges();
            }
            return g;
        }
        internal eInternalWallet? GetGlobalWallet()
        {
            return dbctx.InternalWallet.FirstOrDefault(x => x.WalletNature == eWalletNature.Global && x.WalletType == eInternalWalletType.Global);
        }
        internal mWalletSummery GetGlobalWalletSummery()
        {
            var wallet = dbctx.InternalWallet.FirstOrDefault(x => x.WalletNature == eWalletNature.Global && x.WalletType == eInternalWalletType.Global);
            return GetWalletSummery(wallet.InternalWalletId, eWalletType.Internal);
        }
        internal mWalletSummery GetWalletSummery(Guid walletId)
        {
            return GetWalletSummery(walletId, GetWalletType(walletId));
        }
        internal mWalletSummery GetMyWalletSummery(Guid walletId)
        {

            return GetWalletSummery(walletId, GetWalletType(walletId));
        }
        private mWalletSummery GetWalletSummery(Guid walletId, eWalletType wtype)
        {//This would be relatively very Taxing Method for database, Should be Directed on Reporting Database
            /* 1. Check Wallet Belongs to Active User
             * 2. Get List of All Coins(Active & InActive)
             * 3. Check Wallet Balance for each of such Coin
             */
            mWalletSummery retval = new mWalletSummery();
            switch (wtype)
            {
                case eWalletType.SpotWallet:
                    retval.WalletId = walletId;
                    retval.Name = "Spot Wallet";

                    retval.Tokens = (from wB in dbctx.SpotWBalance
                                     join t in dbctx.Token on wB.TokenId equals t.TokenId
                                     where wB.SpotWalletId == walletId
                                     select new
                                     {
                                         wB.TokenId,
                                         t.FullName,
                                         t.ShortName,
                                         t.IsFiatRepresentative,
                                         t.Code,
                                         wB.ConfirmBalance,
                                         wB.CreatedOn,
                                         wB.ChangeAgent
                                     }
                            into r2
                                     group r2 by r2.TokenId into g
                                     select new mWalletCoin
                                     {
                                         CoinId = g.Key,
                                         FullName = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).FullName,
                                         ShortName = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).ShortName,
                                         IsFiatRepresentative = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).IsFiatRepresentative,
                                         Code = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).Code,
                                         Amount = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).ConfirmBalance,
                                         LastTransactionOn = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).CreatedOn,
                                         LastChangeAgent = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).ChangeAgent
                                     }).ToList();

                    break;
                case eWalletType.FundingWallet:
                    retval.WalletId = walletId;
                    retval.Name = "Fund Wallet";
                    retval.Tokens = (from wB in dbctx.FundingWBalance
                                     join t in dbctx.Token on wB.TokenId equals t.TokenId
                                     where wB.TokenId.HasValue == true && wB.FundingWalletId == walletId
                                     select new
                                     {
                                         wB.TokenId,
                                         t.FullName,
                                         t.ShortName,
                                         t.IsFiatRepresentative,
                                         t.Code,
                                         wB.Balance,
                                         wB.CreatedOn,
                                         wB.ChangeAgent
                                     }
                           into r2
                                     group r2 by r2.TokenId into g
                                     select new mWalletCoin
                                     {
                                         CoinId = g.Key!.Value,
                                         FullName = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).FullName,
                                         ShortName = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).ShortName,
                                         IsFiatRepresentative = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).IsFiatRepresentative,
                                         Code = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).Code,
                                         Amount = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).Balance,
                                         LastTransactionOn = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).CreatedOn,
                                         LastChangeAgent = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).ChangeAgent
                                     }).ToList();

                    break;
                case eWalletType.EarnWallet:
                    retval.WalletId = walletId;
                    retval.Name = "Earn Wallet";
                    retval.Tokens = (from wB in dbctx.EarnWBalance
                                     join t in dbctx.Token on wB.TokenId equals t.TokenId
                                     where wB.EarnWalletId == walletId
                                     select new
                                     {
                                         wB.TokenId,
                                         t.FullName,
                                         t.ShortName,
                                         t.IsFiatRepresentative,
                                         t.Code,
                                         wB.ConfirmBalance,
                                         wB.CreatedOn,
                                         wB.ChangeAgent
                                     }
                          into r2
                                     group r2 by r2.TokenId into g
                                     select new mWalletCoin
                                     {
                                         CoinId = g.Key,
                                         FullName = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).FullName,
                                         ShortName = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).ShortName,
                                         IsFiatRepresentative = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).IsFiatRepresentative,

                                         Code = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).Code,
                                         Amount = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).ConfirmBalance,
                                         LastTransactionOn = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).CreatedOn,
                                         LastChangeAgent = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).ChangeAgent
                                     }).ToList();


                    break;
                case eWalletType.EscrowWallet:
                    retval.WalletId = walletId;
                    retval.Name = "Escrow Wallet";
                    retval.Tokens = (from wB in dbctx.EscrowWBalance
                                     join t in dbctx.Token on wB.TokenId equals t.TokenId
                                     where wB.EscrowWalletId == walletId
                                     select new
                                     {
                                         wB.TokenId,
                                         t.FullName,
                                         t.ShortName,
                                         t.IsFiatRepresentative,
                                         t.Code,
                                         wB.Balance,
                                         wB.CreatedOn,
                                         wB.ChangeAgent
                                     }
                         into r2
                                     group r2 by r2.TokenId into g
                                     select new mWalletCoin
                                     {
                                         CoinId = g.Key,
                                         FullName = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).FullName,
                                         ShortName = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).ShortName,
                                         IsFiatRepresentative = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).IsFiatRepresentative,
                                         Code = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).Code,
                                         Amount = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).Balance,
                                         LastTransactionOn = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).CreatedOn,
                                         LastChangeAgent = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).ChangeAgent
                                     }).ToList();

                    break;
                case eWalletType.Internal:
                    retval.WalletId = walletId;
                    var w = dbctx.InternalWallet.FirstOrDefault(x => x.InternalWalletId == walletId);
                    if (w != null)
                        retval.Name = w.Name.IsNOT_NullorEmpty() ? w.Name : "Internal Fund Wallet";
                    retval.Tokens = (from wB in dbctx.InternalWBalance
                                     join t in dbctx.Token on wB.TokenId equals t.TokenId
                                     where wB.TokenId.HasValue == true && wB.InternalWalletId == walletId
                                     select new
                                     {
                                         wB.TokenId,
                                         t.FullName,
                                         t.ShortName,
                                         t.IsFiatRepresentative,
                                         t.Code,
                                         wB.Balance,
                                         wB.CreatedOn,
                                         wB.ChangeAgent
                                     }
                           into r2
                                     group r2 by r2.TokenId into g
                                     select new mWalletCoin
                                     {
                                         CoinId = g.Key!.Value,
                                         FullName = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).FullName,
                                         ShortName = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).ShortName,
                                         IsFiatRepresentative = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).IsFiatRepresentative,

                                         Code = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).Code,
                                         Amount = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).Balance,
                                         LastTransactionOn = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).CreatedOn,
                                         LastChangeAgent = g.First(s => s.CreatedOn == g.Max(q => q.CreatedOn)).ChangeAgent
                                     }).ToList();

                    break;
                case eWalletType.None:
                    break;
                default:
                    break;
            }
            return retval;
        }
        internal bool SendTokenFromGlobalToWallet(Guid toWalletId, Guid tokenId, double amt, eGlobalPaymentType reason = 0)
        {
            var w = GetGlobalWallet();
            // var um = GetUserManager();
            // var u = um.GetMyUserAccount();
            // if (u is null) return false;
            //Transfer Tokens from Global Wallet To Nominated Wallet
            /* 1. Global Wallet May not have Mentioned Token
             * 3. Amount should be greater than zero 
             * 4. Narration Should be provided
             */
            //var isTrue = DoesCoinExistAndCanTrade(tokenId).Item1;//1

            var narration = reason.ToString();
            //if (!isTrue) return false;
            var t = CreateTransaction(tokenId, w.InternalWalletId, toWalletId, amt, narration, true);
            return t.WalletTransactionId != Guid.Empty;
        }
        internal bool SendTokenFromFundingToSpot(Guid tokenId, double amt)
        {
            var um = GetUserManager();
            var u = um.GetMyUserAccount();
            if (u is null) return false;
            return SendTokenWithIn(tokenId, u.FundingWallet!.FundingWalletId, u.SpotWallet!.SpotWalletId, amt, "To Spot");
        }
        internal bool SendTokenFromFundingToEarn(Guid tokenId, double amt)
        {
            var um = GetUserManager();
            var u = um.GetMyUserAccount();
            if (u is null) return false;
            return SendTokenWithIn(tokenId, u.FundingWallet!.FundingWalletId, u.EarnWallet!.EarnWalletId, amt, "To Earn");
        }
        internal bool SendTokenFromSpotToFunding(Guid tokenId, double amt)
        {
            var um = GetUserManager();
            var u = um.GetMyUserAccount();
            if (u is null) return false;
            return SendTokenWithIn(tokenId, u.SpotWallet!.SpotWalletId, u.FundingWallet!.FundingWalletId, amt, "To Funding");
        }
        internal bool SendTokenFromSpotToEarn(Guid tokenId, double amt)
        {
            var um = GetUserManager();
            var u = um.GetMyUserAccount();
            if (u is null) return false;
            return SendTokenWithIn(tokenId, u.SpotWallet!.SpotWalletId, u.EarnWallet!.EarnWalletId, amt, "To Earn");
        }

        internal bool SendTokenFromEarnToSpot(Guid tokenId, double amt)
        {
            var um = GetUserManager();
            var u = um.GetMyUserAccount();
            if (u is null) return false;
            return SendTokenWithIn(tokenId, u.EarnWallet!.EarnWalletId, u.SpotWallet!.SpotWalletId, amt, "To Spot");
        }
        internal bool SendTokenFromEarnToFunding(Guid tokenId, double amt)
        {
            var um = GetUserManager();
            var u = um.GetMyUserAccount();
            if (u is null) return false;
            return SendTokenWithIn(tokenId, u.EarnWallet!.EarnWalletId, u.FundingWallet!.FundingWalletId, amt, "To Funidng");
        }

        internal bool SendTokenFromStakingToEarn(Guid tokenId, double amt)
        {
            //ToDo: Naveen, Earn Wallet Staking implementation
            //There should be staking opportunity entity that defines the attribute of the staking
            //There should be a staking Entity that will hold all the Tokens that are commited for staking
            throw new NotImplementedException("Staking is not implemented yet..");

        }
        private bool SendTokenWithIn(Guid tokenId, Guid fromWallet, Guid toWallet, double amt, string narration)
        {
            //Transfer Tokens within own wallets
            /* 1. Token Should Exist even if it has been stopped for trading 
             * 2. From & To should be of same User but not same wallet
             * 3. Amount should be greater than zero but less than equalto the 'from' Wallet Balance
             * 4. Narration Should be provided
             */
            var isTrue = DoesCoinExistAndCanTrade(tokenId).Item1;//1
            var w = GetUserManager().GetMyUserAccountId();
            isTrue = isTrue && ConfirmInternalWalletWithSameUserAccount(w, fromWallet, toWallet); //2
            var avaiBalance = GetAvailbaleConfirmBalanceOfWallet(fromWallet, tokenId);
            isTrue = isTrue && avaiBalance.Item2 >= amt;

            if (!isTrue) return false;
            var t = CreateTransaction(tokenId, fromWallet, toWallet, amt, narration);
            return t.WalletTransactionId != Guid.Empty;

        }
        internal bool SendTokenToInternalUser(Guid tokenId, Guid fromWallet, Guid toWallet, double amt, string narration)
        {
            //Transfer Tokens to other Internal NavExM User
            //Taxes may be involved
            var isTrue = DoesCoinExistAndCanTrade(tokenId).Item1;//1
            var w = GetUserManager().GetMyUserAccountId();

            // ensure wallet belongs to sender
            isTrue = isTrue && ConfirmInternalWalletWithSameUserAccount(w, fromWallet);
            //ensure sending wallet is funding wallet
            isTrue = isTrue && GetWalletType(toWallet) == eWalletType.FundingWallet;
            //ensure sender and Receiver are not same
            var recUserId = GetWalletUserAccountId(toWallet);
            if (w == recUserId) throw new InvalidOperationException("Receiver and Sender can not be same User");
            var usr = GetUserManager().GetUserbyId(recUserId);
            if (usr.IsActive == false)
                throw new InvalidOperationException("Receiving User Account is not Active..");

            //toWallet should be Funding Wallet
            isTrue = isTrue && GetWalletType(toWallet) == eWalletType.FundingWallet;
            if (!isTrue) return false;
            var t = CreateTransaction(tokenId, fromWallet, usr.HoldingWalletId!.Value, amt, narration);

            var hWbal = new eHoldingWBalance() { Balance = amt, ProposedChangeAgent = t.WalletTransactionId, Status = HoldingTransStatus.NotDecided, HoldingWalletId = usr.HoldingWalletId!.Value };

            dbctx.Add(hWbal);
            dbctx.SaveChanges();
            //ToDo: Naveen, Notify Receiving User of Pending Acceptance of Incoming Funds from Another User

            return true;
        }
        internal bool SendTokenOut(Guid tokenId, Guid fromWallet, Guid toWallet, double amt, string narration)
        {
            //Transfer Tokens To External Wallet on the network
            //Taxes for Withdrawl may be involved
            throw new NotImplementedException("SendTokenOut");
        }
        internal async Task<mReceiveTokenTicket> CheckMyExternalReceiveStatus(mReceiveTokenTicket tt)
        {
            /* 1. Check if Token belongs to User
             * 2. Check Receive Status with Watch Collection
             * 
             */
            throw new NotImplementedException("CheckMyExternalReceiveStatus Implementation is not completed as per design");
        }
        internal List<mStake> GetMyStakings()
        {
            var my = GetUserManager().GetMyUserAccount();
            var lst = dbctx.Staking.Include(x => x.StakingOpportunity)
                .ThenInclude(x => x.Token)
                .Where(x => x.userAccountId == my.UserAccountId && x.IsRedeemed == false).ToList();
            return lst.ToModel();
        }
        internal List<mStake> GetMyStakings(string uAcc)
        {
            var my = GetUserManager().GetUserOfAcc(uAcc);
            var lst = dbctx.Staking.Include(x => x.StakingOpportunity)
                .ThenInclude(x => x.Token)
                .Where(x => x.userAccountId == my.UserAccountId && x.IsRedeemed == false).ToList();
            return lst.ToModel();
        }
        internal eCommunityCategory GetMyCommunityStatus(string uAcc)
        {
            var my = GetUserManager().GetUserOfAcc(uAcc);
            var lst = dbctx.Staking.Include(x => x.StakingOpportunity)
                .ThenInclude(x => x.Token)
                .Where(x => x.userAccountId == my.UserAccountId && x.IsRedeemed == false && x.StakingOpportunity.Community != eCommunityCategory.None).ToList();
            if (lst.Count > 0)
                return lst.MaxBy(x => x.StakingOpportunity.Community)!.StakingOpportunity.Community;

            return eCommunityCategory.None;
        }
        internal mStake StakeTokens(mCreateStake m)
        {
            /*  1. validate Staking Opportunity
             *  2. validate funds i.e. minimum..
             *  3. Fund account has balance
             *  4. we are not checking if token is still tradeable
             */
            var opp = dbctx.StakingOpportunity.Include(x => x.Token).FirstOrDefault(x => x.StakingOpportunityId == m.StakingOpportunityId);
            if (opp is null) throw new InvalidDataException("Slot id is not valid");
            if (opp.MinAmount.HasValue && opp.MinAmount.Value > m.Amount) throw new InvalidDataException($"Minimum Allowed Amount is{opp.MinAmount.Value}");
            if (opp.MaxAmount.HasValue && opp.MaxAmount.Value < m.Amount) throw new InvalidDataException($"Maximum Allowed Amount is{opp.MaxAmount.Value}");
            var e = StakingFromTemplate(m);
            var avaiBalance = GetAvailbaleConfirmBalanceOfWallet(e.fromFundWalletId, opp.TokenId);

            if (avaiBalance.Item2 < m.Amount) throw new InvalidDataException($"Not enough funds for staking");

            dbctx.Staking.Add(e);
            var t = CreateTransaction(opp.TokenId, e.fromFundWalletId, e.StakingWalletId, e.Amount, $"Staking {e.Amount}{opp.Token.Code}");
            t.SignRecord(this);
            e.FromTransactionId = t.WalletTransactionId;
            e.SignRecord(this);
            dbctx.SaveChanges();
            return e.ToModel();
        }
        internal mStake RedeemMyStake(Guid id)
        {
            /* 1. Check It belongs to User
             * 2. Check if It can be Renewed - Template is still valid
             * 3.
             */
            var ua = GetUserManager().GetMyUserAccount();
            if (ua == null) throw new ApplicationException("No active user session exist.");

            var obj = dbctx.Staking.Include(x => x.StakingOpportunity)
                .ThenInclude(x => x.Token)
                .FirstOrDefault(x => x.StakingId == id && x.IsRedeemed == false
            && x.userAccountId == ua.UserAccountId);

            return RedeemStake(obj);
        }
        internal mStake RedeemStake(eStaking obj)
        {
            if (obj == null) throw new ApplicationException("No Such Staking exists..");
            obj.VerifyRecord(this, true);
            var amt = obj.ExpectedEndData.Date < DateTime.UtcNow.Date ? obj.MatureAmount : (!obj.StakingOpportunity.IsHardFixed ? obj.Amount : 0);
            if (amt <= 0)
                throw new ApplicationException("Redeemtion Conditions doesn't meet. Process abandoned");
            // Return Mature Amount 
            var t = CreateTransaction(obj.StakingOpportunity.TokenId, obj.StakingWalletId, obj.fromFundWalletId, amt, $"Redeem Staking {amt}{obj.StakingOpportunity.Token.Code}", isExternalFund: true);
            t.SignRecord(this);
            obj.ToTransactionId = t.WalletTransactionId;
            obj.IsRedeemed = true;
            obj.RedeemedOn = DateTime.UtcNow;
            obj.SignRecord(this);
            return obj.ToModel();

        }

        internal bool RenewMyStake(Guid id)
        {
            /* 1. Check It belongs to User
             * 2. Check if It can be Renewed - Template is still valid
             * 3.
             */
            var ua = GetUserManager().GetMyUserAccount();
            if (ua == null) throw new ApplicationException("No active user session exist.");

            var obj = dbctx.Staking.Include(x => x.StakingOpportunity).FirstOrDefault(x => x.StakingId == id &&
            x.ExpectedEndData.Date <= DateTime.UtcNow.Date && x.IsRedeemed == false
            && x.userAccountId == ua.UserAccountId
            && x.AutoRenew == true);

            if (obj == null) throw new ApplicationException("No Such Staking exists..");
            if (obj.StakingOpportunity.OfferExpiredOn.HasValue && obj.StakingOpportunity.OfferExpiredOn.Value.Date <= DateTime.UtcNow.Date)
                throw new ApplicationException("Staking Offer doesn't Exist any more..");

            obj.VerifyRecord(this, true);
            var opp = obj.StakingOpportunity;
            var d = DateTime.Today - obj.StartedOn;
            var m = +d.Days / obj.StakingOpportunity.Duration;
            while (obj.StartedOn.AddDays(m * obj.StakingOpportunity.Duration) <= DateTime.UtcNow.Date)
            {
                m++;
            }
            obj.SessionHash = GetSessionHash();
            obj.ExpectedEndData = obj.StartedOn.AddDays(m * obj.StakingOpportunity.Duration);// DateTime.UtcNow.AddDays(obj.StakingOpportunity.Duration),
            obj.MatureAmount = (((obj.Amount * opp.AYPOffered) / 365) * m);

            obj.SignRecord(this);
            return true;
        }
        private eStaking StakingFromTemplate(mCreateStake m)
        {
            var opp = dbctx.StakingOpportunity.FirstOrDefault(x => x.StakingOpportunityId == m.StakingOpportunityId);
            if (opp is null) return null;
            // opp.VerifyRecord(this, true);
            var my = GetUserManager().GetMyUserAccount();
            var e = new eStaking()
            {
                SessionHash = GetSessionHash(),
                StakingWalletId = GetStakingWallets(m.StakingOpportunityId).InternalWalletId,
                StartedOn = DateTime.UtcNow.Date.AddDays(1),
                ExpectedEndData = DateTime.UtcNow.Date.AddDays(opp.Duration),
                StakingOpportunityId = m.StakingOpportunityId,
                Amount = m.Amount,
                CreatedOn = DateTime.UtcNow,
                fromFundWalletId = my.FundingWalletId!.Value,
                FromTransactionId = Guid.Empty,
                MatureAmount = m.Amount + (((m.Amount * (opp.AYPOffered) / 100) / 365) * opp.Duration),
                userAccount = my,
                userAccountId = my.UserAccountId,
                RecordHash = "?",
                AutoRenew = opp.AutoRenewAllowed ? m.AutoRenew : false
            };
            if (my.RefCode?.RefferedBy.IsNullorEmpty() == false)
                RecordCommunityStatusForReffer(my.RefCode.RefferedBy, my.UserAccountId);
            //Do not call DBContext here
            return e;
        }
        internal eInternalWallet GetStakingWallets(Guid oppId)
        {
            var op = dbctx.StakingOpportunity.Include(x => x.Token).First(x => x.StakingOpportunityId == oppId);

            var g = dbctx.InternalWallet.FirstOrDefault(x => x.BelongsTo.ToLower() == op.StakingOpportunityId.ToString().ToUpper() && x.WalletNature == eWalletNature.Liability && x.WalletType == eInternalWalletType.Staking && x.GlobalId == null);
            if (g == null)
            {
                EnsureStakingWallets(op.GroupId);
                g = dbctx.InternalWallet.FirstOrDefault(x => x.BelongsTo.ToLower() == op.StakingOpportunityId.ToString().ToUpper() && x.WalletNature == eWalletNature.Liability && x.WalletType == eInternalWalletType.Staking && x.GlobalId == null);
            }
            return g;
        }
        internal bool EnsureStakingWallets(Guid gId)
        {
            var lst = dbctx.StakingOpportunity.Include(x => x.Token).Where(x => x.GroupId == gId).ToList();
            //Staking Global Wallet
            foreach (var op in lst)
            {
                var g = dbctx.InternalWallet.FirstOrDefault(x => x.BelongsTo.ToLower() == op.StakingOpportunityId.ToString().ToUpper() && x.WalletNature == eWalletNature.Liability && x.WalletType == eInternalWalletType.Staking && x.GlobalId == null);
                if (g == null)
                {
                    g = new eInternalWallet { Name = $"Global Staking Wallet for {op.Token.Code}", BelongsTo = op.StakingOpportunityId.ToString().ToUpper(), WalletNature = eWalletNature.Liability, WalletType = eInternalWalletType.Staking };

                    dbctx.InternalWallet.Add(g);
                }
            }
            return dbctx.SaveChanges() > 0;
        }
        internal mNetworkWallet? GetMyNetworkWallet(Guid networkId)
        {
            /* 1. validate Network Id
             * 2. Validate Current Session User
             * 3. Get Network Wallet if Already Exist
             */
            var net = dbctx.SupportedNetwork.FirstOrDefault(x => x.SupportedNetworkId == networkId);
            if (net is null) throw new InvalidDataException("Network id is not valid");
            var um = GetUserManager();
            var wall = GetNetworkWalletForFundingForUserAcc(um.GetMyUserAccountId());
            eFundingNetworkWallet? nwall = null;
            if (wall is not null)
                nwall = wall!.Where(x => x.NetworkWalletAddress.NetworkId == networkId).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
            if (nwall is not null)
            {
                //#if (!DEBUG)
                //                if (nwall.VerifyRecord(this))
                //                    return nwall.ToModel();
                //                else
                //                {
                //                    LogEvent($"Record Hash of {nameof(nwall)} with id{nwall.FundingNetworkWalletId} might have been compromised..");

                //                }
                //#else
                //                return nwall.ToModel();
                //#endif
                return nwall.ToModel();

            }
            return null;

        }
        internal bool ProvisionScNetworkWalletForMe(Guid networkId)
        {
            var um = GetUserManager();
            var ua = um.GetMyUserAccount();
            SrvEthNetworkWalletWorker.RequestETHNetworkWallet(new smNetWalletBox { NetworkId = networkId, userAccountId = ua.UserAccountId, userAccount = ua.FAccountNumber, OwnerType = WalletOwnerType.ClientPermanent, SessionHash = um.GetSessionHash(), CreatedOn = DateTime.UtcNow });
            return true;
        }
        internal bool ProvisionNetworkWalletForMe(Guid networkId)
        {
            /* 1. validate Network Id
             * 2. Validate Current Session User
             * 3. Check if Network Wallet Already Exist for this User
             * 4. Source Network Wallet, Provision the same for this User
             * 5. 
             */
            var net = dbctx.SupportedNetwork.FirstOrDefault(x => x.SupportedNetworkId == networkId);
            if (net is null) throw new InvalidDataException("Network id is not valid");
            var um = GetUserManager();
            var wall = GetNetworkWalletForFundingForUserAcc(um.GetMyUserAccountId());
            eFundingNetworkWallet? nwall = null;
            if (wall is not null)
            {
                //Recently Create Wallet should always be preferred
                nwall = wall!.Where(x => x.NetworkWalletAddress.NetworkId == networkId).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
            }
            if (nwall is not null)
            {
                if (nwall.VerifyRecord(this))
                    return false;

                // return nwall.ToModel();
                else
                    LogEvent($"Record Hash of {nameof(nwall)} with id{nwall.FundingNetworkWalletId} might have been compromised..");
            }
            var ua = um.GetMyUserAccount();
            SrvEthNetworkWalletWorker.RequestETHNetworkWallet(new smNetWalletBox { NetworkId = networkId, userAccountId = ua.UserAccountId, userAccount = ua.FAccountNumber, OwnerType = WalletOwnerType.ClientInitial, SessionHash = um.GetSessionHash(), CreatedOn = DateTime.UtcNow });
            return true;
        }
        internal bool PreBeta_ValidateAndReceiveExternalFundsForNavCBuy(smFundsNotification sm)
        {
            //ToDo: PreBeta implementation..

            try
            {
                if (ConfigEx.VersionType != versionType.PreBeta) return false;

                var rec = new Data.Entity.PreBeta.ePBMyPurchaseRecords
                {
                    IsNativeFund = sm.IsNativeFund,
                    NetworkProxy = sm.NetworkProxy,
                    TranHash = sm.TranHash,
                    userAccount = sm.userAccount,
                    userAccountId = sm.userAccountId,
                    WalletAddress = sm.WalletAddress
                };
                if (rec.IsNativeFund)
                {
                    rec.Amount = sm.Amount;
                }
                else
                {
                    rec.Erc20Amount = sm.Erc20Amount;
                    rec.TokenAddress = sm.TokenAddress;
                }
                //
                var tk = GetExchangeTokenForFundNotification(sm);

                var isOK = GetReceiverUserOfFundNotification(sm, out var user, out var nw);
                if (isOK == false)
                {
                    LogError($"ERROR:'PreBeta_ValidateAndReceiveExternalFundsForNavCBuy' External Funds received in Wallet:{sm.WalletAddress} with Transaction:{sm.TranHash} Couldn't be Credited");
                    LogError($"ERROR:No Such User exist to Credit this Transaction");
                    return false;
                }
                var lst = SrvCoinWatch.GetAllExternalCoins();
                var startedOn = DateTime.UtcNow;
                var t = lst.FirstOrDefault(x => x.TokenName.ToLower() == tk.Code.ToLower());

                if (t != null && double.IsNaN(t.Price))
                {
                    while (startedOn.AddMinutes(3) > DateTime.UtcNow)
                    {
                        Thread.Sleep(4000);
                        lst = SrvCoinWatch.GetAllExternalCoins();
                        t = lst.FirstOrDefault(x => x.TokenName.ToLower() == tk.Code.ToLower());
                        if (t != null && double.IsNaN(t.Price) == false)
                            break;
                    }
                    if (double.IsNaN(t.Price))
                    {
                        LogError($"ERROR:'PreBeta_ValidateAndReceiveExternalFundsForNavCBuy' External Funds received in Wallet:{sm.WalletAddress} with Transaction:{sm.TranHash} Couldn't be Credited");
                        LogError($"ERROR:'PreBeta_ValidateAndReceiveExternalFundsForNavCBuy' even After 3 Minutes Wait, ExternalPrice of:{t.TokenName} is not received.");
                        return false;
                    }
                }
                if (t == null || double.IsNaN(t.Price))
                {
                    LogError($"ERROR:'PreBeta_ValidateAndReceiveExternalFundsForNavCBuy' External Funds received in Wallet:{sm.WalletAddress} with Transaction:{sm.TranHash} Couldn't be Credited");
                    LogError($"ERROR:'PreBeta_ValidateAndReceiveExternalFundsForNavCBuy' Failed to Receive Token Conversion Rate at..{DateTime.UtcNow}");
                    return false;
                }
                rec.BuyWith = t.TokenName;

                double Amt;
                if (sm.IsNativeFund)
                {
                    Amt = sm.Amount;
                    if (Amt > 0)
                    {
                        var dAmt = Convert.ToDecimal(Amt / double.Parse($"1E{tk.Tick.FractionCount()}"));
                        Amt = Convert.ToDouble(dAmt);
                        //get current stage and Token Rate to calculate the number
                    }
                }
                else
                {
                    double.TryParse(sm.Erc20Amount, out Amt);
                    if (Amt > 0)
                    {
                        var dAmt = Convert.ToDecimal(Amt / double.Parse($"1E{tk.Tick.FractionCount()}"));
                        Amt = Convert.ToDouble(dAmt);
                    }
                }
                //Calculate the NavC Token to Pay as per Current Rate in PreBeta
                rec.Amount = Amt;
                var um = GetUserManager();

                var stage = pbdbctx.PreBetaStage.Where(x => x.StartDate <= DateTime.UtcNow).OrderByDescending(x => x.StartDate).FirstOrDefault();
                if (stage.EndDate.HasValue == false || stage.EndDate > DateTime.UtcNow)
                {
                    rec.NavCAmountPurchased = (Amt * t.Price) / stage.NavCSellPrice;
                    rec.NavCUnitRate = stage.NavCSellPrice;
                    rec.TokenToUnitRate = t.Price;
                    if ((Amt * t.Price) < 20)
                    {
                        //Todo: Send Email of less than 20USDT for sale
                        LogError($"ERROR:'PreBeta_ValidateAndReceiveExternalFundsForNavCBuy' External Funds received in Wallet:{sm.WalletAddress} with Transaction:{sm.TranHash} Couldn't be Credited");
                        LogError($"ERROR:'PreBeta_ValidateAndReceiveExternalFundsForNavCBuy' Minimum Purchases must be of 20 USDT and Token Receivedc:{t.TokenName} value is:{(Amt * t.Price)} at..{DateTime.UtcNow}");
                        um.SendEmailForLessThanLimitBuyLimit(user.AuthEmail!.Email, $"{Amt.ToString("#,##0.0000")} {t.TokenName}", smtp);
                        return false;
                    }

                }
                else
                {
                    LogError($"ERROR:'PreBeta_ValidateAndReceiveExternalFundsForNavCBuy' External Funds received in Wallet:{sm.WalletAddress} with Transaction:{sm.TranHash} Couldn't be Credited");
                    LogError($"ERROR:'PreBeta_ValidateAndReceiveExternalFundsForNavCBuy' Prebeta is still enabled when its Scheduled Enddate has reached at:{DateTime.UtcNow}");
                    return false;
                }

                if ((Amt * t.Price) > 100000)
                {
                    var ret = DepostFundsIntoWallet(sm);
                    um.SendEmailForExceedBuyLimit(user.AuthEmail!.Email, $"{Amt.ToString("#,##0.0000")} {t.TokenName}", smtp);
                    return ret;
                }


                pbdbctx.PurchaseRecords.Add(rec);
                pbdbctx.SaveChanges();

                //ToDo: Send email for Purchases Confirmation.
                var bw = $"{((rec.NavCAmountPurchased * rec.NavCUnitRate) / rec.TokenToUnitRate).ToString("0.000")} {rec.BuyWith}";
                um.SendEmailPreBetaPurchasesSignup(user.AuthEmail!.Email, rec.NavCAmountPurchased.ToString("#,##0.0000"), bw, rec.TranHash, rec.DateOf, smtp);

                //send email for cashback category
                var s = pbdbctx.PurchaseRecords.Where(x => x.userAccountId == sm.userAccountId).ToList().Sum(x => x.NavCAmountPurchased);

                if (s >= 500 && s < 5000)
                    um.SendEmailFor500Signup(user.AuthEmail!.Email, smtp);
                else if (s >= 5000 && s < 50000)
                    um.SendEmailFor5000Signup(user.AuthEmail!.Email, smtp);
                else if (s >= 50000)
                    um.SendEmailFor50000Signup(user.AuthEmail!.Email, smtp);

                return true;
            }
            catch (Exception ex)
            {
                LogError($"ERROR:'PreBeta_ValidateAndReceiveExternalFundsForNavCBuy' External Funds received in Wallet:{sm.WalletAddress} with Transaction:{sm.TranHash} Couldn't be Credited");
                LogError($"ERROR:{ex.GetDeepMsg()}");
            }
            return false;

        }
        internal bool ValidateAndReceiveExternalFunds(smFundsNotification sm)
        {
            ConfirmPendingTxCheckRequest(sm.TranHash);//Just clear this Tx since we have received Notification

            if (ConfigEx.VersionType == versionType.PreBeta)
            {
                var lst = pbdbctx.PurchaseRecords.Where(x => x.userAccountId == sm.userAccountId).ToList();
                if (lst.Count < 20)
                    return PreBeta_ValidateAndReceiveExternalFundsForNavCBuy(sm);
            }

            return DepostFundsIntoWallet(sm);
        }
        internal bool DepostFundsIntoWallet(smFundsNotification sm)
        {
            var tk = GetExchangeTokenForFundNotification(sm);

            var isOK = GetReceiverUserOfFundNotification(sm, out var user, out var nw);
            if (isOK == false)
            {
                LogError($"ERROR:External Funds received in Wallet:{sm.WalletAddress} with Transaction:{sm.TranHash} Couldn't be Credited");
                return false;
            }
            double Amt;
            if (sm.IsNativeFund)
            {
                Amt = sm.Amount;
                if (Amt > 0)
                {
                    var dAmt = Convert.ToDecimal(Amt / double.Parse($"1E{tk.Tick.FractionCount()}"));
                    Amt = Convert.ToDouble(dAmt);
                }

            }
            else
            {
                double.TryParse(sm.Erc20Amount, out Amt);
                if (Amt > 0)
                {
                    var dAmt = Convert.ToDecimal(Amt / double.Parse($"1E{tk.Tick.FractionCount()}"));
                    Amt = Convert.ToDouble(dAmt);
                }
            }

            var t = CreateTransaction(tk.TokenId, nw.NetworkWalletAddressId, user.FundingWalletId!.Value, Amt, sm.TranHash, true);
            if (t != null)
            {
                LogEvent($"EVENT:External Funds Credited|User:{user.AccountNumber} has Received Amount:{Amt} of {tk.Code} from Network Proxy {sm.NetworkProxy}");
            }
            return true;
        }
        internal bool SaveEthAddress(smNetWalletBox Addr)
        {
            var net = dbctx.SupportedNetwork.FirstOrDefault(x => x.SupportedNetworkId == Addr.NetworkId);
            if (net == null)
                LogError($"{AppConfigBase.RegistryToken.AppId} Invalid Network Id for External Eth wallet at {DateTime.UtcNow}");
            var nw = new eNetworkWalletAddress()
            {
                NetworkId = Addr.NetworkId,
                Address = Addr.WalletAddress,
                Notes = String.Empty,
                Network = net,
                RecordHash = "?",
                SessionHash = Addr.SessionHash
            };
            if (Addr.OwnerType == WalletOwnerType.ClientInitial)
                nw.Category = eNetWorkWalletCategory.NavExMClient;
            else if (Addr.OwnerType == WalletOwnerType.ClientPermanent)
                nw.Category = eNetWorkWalletCategory.NavExMSmartContract;
            else
                nw.Category = eNetWorkWalletCategory.NavExMTemp;


            dbctx.NetworkWalletAddress.Add(nw);
            dbctx.SaveChanges();
            nw.SignRecord(this);
            //allocate the same to Current User
            var um = GetUserManager();
            var ur = um.GetUserbyId(Addr.userAccountId);
            var allwall = new eFundingNetworkWallet()
            {
                NetworkWalletAllocationMode = eNetworkWalletAllocationMode.Preptual,
                NetworkWalletAddress = nw,
                FundingWalletId = ur.FundingWalletId!.Value,
                RecordHash = "?",
                SessionHash = Addr.SessionHash
            };
            dbctx.FundingNetworkWallet.Add(allwall);
            dbctx.SaveChanges();
            allwall.SignRecord(this);

            return true;// allwall.ToModel();
        }

        //--- This Flow of Logic is not Actually Implemented, but Some of the features offered in this Logic may be Required.
        //internal List<eNetworkWalletAddressWatch> GetMyTokensReceivedButNotPaid()
        //{
        //    var allTrans = GetMyAllTokenReceiveRequests();

        //    var ReceivedButNotPaid = allTrans.Where(x => x.NetworkTransactionId!.IsNOT_NullorEmpty() && x.InternalTransactionId!.IsNullorEmpty()).ToList();
        //    return ReceivedButNotPaid;
        //}
        //private List<eNetworkWalletAddressWatch> GetMyAllTokenReceiveRequests()
        //{
        //    var um = GetUserManager();
        //    var wall = GetNetworkWalletForFundingForUserAcc(um.GetMyUserAccountId());

        //    var allTrans = wall.SelectMany(x => x.NetworkWalletAddress.AddressWatch.Where(z => z.CreatedOn >= x.AllocatedOn && (x.ShouldDeAllocatedOn.HasValue == false || z.CreatedOn <= x.ShouldDeAllocatedOn.Value))).ToList();

        //    return allTrans;
        //}
        internal bool CreateReceiveMyToken(Guid tokenId, Guid networkId, Guid externalWalletId, double amt)
        {
            /* 1. Check if Network is Supported
             * 2. Check Wallet belongs to NavExM
             * 3. Check Wallet is Authorized for this User
             * 4. Token Belongs to this Network
             * 5. Token is Enable for Trade in the Exchange
             * 6. Create Transaction Watch Record for 
             *      1. DB Record --> mReceiveTokenTicket
             *      2. Notify Wallet Watcher
             *      3. 
             */
            var net = dbctx.SupportedNetwork.FirstOrDefault(x => x.SupportedNetworkId == networkId);
            if (net is null) throw new InvalidDataException("Network id is not valid");
            if (net.DeletedOn.HasValue) throw new InvalidOperationException("Network is not supported any more..");

            var isTrue = true && IsCurrentNavExMExternalWallet(externalWalletId, networkId);//2
            var usr = GetUserManager().GetMyUserAccount();
            isTrue = isTrue && IsNetWorkWalletCurrentlyAssociatedToFunding(usr.FundingWalletId!.Value, externalWalletId);//3
            var t = GetToken(tokenId);
            if (t is null) throw new InvalidDataException("Invalid Token Id provided");
            isTrue = isTrue && IsTokenLocatedOnNetwork(tokenId, networkId);//4
            isTrue = isTrue && !(t!.DeletedOn.HasValue && t.DeletedOn.Value <= DateTime.UtcNow);//5
                                                                                                // Save DB Record
            var supTokenId = t.SupportedCoin.First(x => x.RelatedNetworkId == networkId).SupportedTokenId;
            var e = new eNetworkWalletAddressWatch() { ExpectedAmount = amt, NetworkWalletAddressId = externalWalletId, SupportedTokenId = supTokenId };
            dbctx.NetworkWalletAddressWatch.Add(e);
            dbctx.SaveChanges();
            //Send Watch Request to Watcher
            return RequestWatch(e.ToDataModel());
        }
        private eToken? GetToken(Guid tokenId)
        {
            return dbctx.Token
                  .Include(x => x.SupportedCoin)
                  .ThenInclude(x => x.RelatedNetwork)
                  .FirstOrDefault(x => x.TokenId == tokenId);
        }
        private bool IsTokenLocatedOnNetwork(Guid tokenId, Guid networkId)
        {
            var t = dbctx.Token.FirstOrDefault(x => x.TokenId == tokenId && x.SupportedCoin.Any(z => z.RelatedNetworkId == networkId));
            return t != null;
        }
        private bool IsNetWorkWalletCurrentlyAssociatedToFunding(Guid FundingWid, Guid extNetWalId)
        {
            var rec = dbctx.FundingNetworkWallet.FirstOrDefault(x => x.FundingWalletId == FundingWid && x.NetworkWalletAddressId == extNetWalId);
            if (rec is null) return false;
            if (rec.ShouldDeAllocatedOn.HasValue && rec.ShouldDeAllocatedOn.Value <= DateTime.UtcNow) return false;
            return true;
        }
        private List<eFundingNetworkWallet> GetNetworkWalletForFundingForUserAcc(Guid userAccId)
        {
            if (userAccId == Guid.Empty) return null;
            var fw = dbctx.FundingWallet
                .Include(x => x.myNetworkWallets)
                .ThenInclude(x => x.NetworkWalletAddress)
                .ThenInclude(x => x.AddressWatch)
                .Include(x => x.myNetworkWallets)
                .ThenInclude(x => x.NetworkWalletAddress)
                .ThenInclude(x => x.Network)
                .FirstOrDefault(x => x.UserAccountId == userAccId);
            if (fw is null) return null;
            return fw.myNetworkWallets;
        }
        internal bool ReceiveToken(Guid tokenId, Guid fromWallet, Guid toWallet, double amt, string narration)
        {
            //Staff Support
            //Receive Tokens from external wallets into Fund Wallet
            /* 0. There should be an Internal Company's wallet that we update with desired number of Tokens when we receive such tokens in External Network
             * 1. Check Associated External Network Wallet for desired Transaction, If Received then
             * 2. Update Funding Wallet with Received amount
             */
            throw new NotImplementedException("ReceiveToken Implementation is not completed as per design, External Wallet Mirror should exist and confirm for received coins");

            var isTrue = DoesCoinExistAndCanTrade(tokenId).Item1;//1
            var w = GetUserManager().GetMyUserAccountId();
            isTrue = isTrue && ConfirmInternalWalletWithSameUserAccount(w, fromWallet, toWallet); //2
            var avaiBalance = GetAvailbaleConfirmBalanceOfWallet(fromWallet, tokenId);
            isTrue = isTrue && avaiBalance.Item2 >= amt;

            if (!isTrue) return false;
            var t = CreateTransaction(tokenId, fromWallet, toWallet, amt, narration);
            return t.WalletTransactionId != Guid.Empty;

        }
        //#if (DEBUG)
        //        internal bool ReceiveToken_TempTesting(Guid fundingWalletId, Guid TokenId, double Amt)
        //        {
        //            var t = CreateTransaction(TokenId, Guid.Empty, fundingWalletId, Amt, "HalfTest Transaction");
        //            return t.WalletTransactionId != Guid.Empty;
        //        }
        //#endif
        internal bool SellToken()
        {
            //Sell Tokens for Fiat Currency
            //Taxes may be involved
            throw new NotImplementedException("SellToken");
        }
        internal bool BuyTokens()
        {
            //Buy Tokens using Fiat Currency
            throw new NotImplementedException("BuyTokens");
        }

        internal List<mWalletSummery> GetWalletSummeries()
        {
            throw new NotImplementedException("GetWalletSummeries is not implemented");
        }
        private bool IsCurrentNavExMExternalWallet(Guid walletID, Guid NetworkId)
        {
            var w = dbctx.NetworkWalletAddress.FirstOrDefault(x => x.NetworkWalletAddressId == walletID && x.NetworkId == NetworkId);
            if (w is null || w.DeletedOn.HasValue) return false;
            //ToDO: Naveen, Confirm Record Hash

            return true;
        }
        internal void GenerateWallets(eUserAccount user)
        {
            var Holding = new eHoldingWallet() { InternalAccountNumber = Guid.NewGuid() };
            Holding.UserAccount = user;
            user.HoldingWallet = Holding;

            var Spot = new eSpotWallet() { InternalAccountNumber = Guid.NewGuid() };
            Spot.UserAccount = user;
            user.SpotWallet = Spot;
            // dbctx.SpotWallet.Add(Spot);

            var Fund = new eFundingWallet() { InternalAccountNumber = Guid.NewGuid() };
            Fund.UserAccount = user;
            user.FundingWallet = Fund;
            //dbctx.FundingWallet.Add(Fund);

            var Earn = new eEarnWallet() { InternalAccountNumber = Guid.NewGuid() };
            Earn.UserAccount = user;
            user.EarnWallet = Earn;
            // dbctx.EarnWallet.Add(Earn);

            var Escro = new eEscrowWallet() { InternalAccountNumber = Guid.NewGuid() };
            Escro.UserAccount = user;
            user.EscrowWallet = Escro;
            // dbctx.EscrowWallet.Add(Escro);
            dbctx.SaveChanges();
            SetInitialBalance(Spot);
            SetInitialBalance(Fund);
            SetInitialBalance(Earn);
            SetInitialBalance(Escro);
            dbctx.SaveChanges();
        }
        private bool SetInitialBalance(eEscrowWallet w)
        {
            var tlist = dbctx.Token.Where(x => x.DeletedOn.HasValue == false || x.DeletedOn <= DateTime.UtcNow).ToList();
            w.EscrowWBalance = w.EscrowWBalance ?? new List<eEscrowWBalance>();
            tlist.ForEach(x =>
            {
                w.EscrowWBalance.Add(new eEscrowWBalance() { EscrowWallet = w, SessionHash = GetSessionHash(), Token = x, Balance = 0.0, ChangeAgent = Guid.Empty });
            });
            return true;
        }
        private bool SetInitialBalance(eEarnWallet w)
        {
            //get List of Tokens and Set Initial Balance of 0 for this Wallet

            var tlist = dbctx.Token.Where(x => x.DeletedOn.HasValue == false || x.DeletedOn <= DateTime.UtcNow).ToList();
            w.EarnWBalance = w.EarnWBalance ?? new List<eEarnWBalance>();
            tlist.ForEach(x =>
            {
                w.EarnWBalance.Add(new eEarnWBalance() { EarnWallet = w, SessionHash = GetSessionHash(), Token = x, ConfirmBalance = 0.0, ChangeAgent = Guid.Empty });
            });
            return true;
        }
        private bool SetInitialBalance(eFundingWallet w)
        {

            var tlist = dbctx.Token.Where(x => x.DeletedOn.HasValue == false || x.DeletedOn <= DateTime.UtcNow).ToList();
            w.FundingWBalance = w.FundingWBalance ?? new List<eFundingWBalance>();
            tlist.ForEach(x =>
            {
                w.FundingWBalance.Add(new eFundingWBalance() { FundingWallet = w, SessionHash = GetSessionHash(), Token = x, });
            });
            return true;
        }
        private bool SetInitialBalance(eSpotWallet w)
        {
            //get List of Tokens and Set Initial Balance of 0 for this Wallet

            var tlist = dbctx.Token.Where(x => x.DeletedOn.HasValue == false || x.DeletedOn <= DateTime.UtcNow).ToList();
            w.SpotWBalance = w.SpotWBalance ?? new List<eSpotWBalance>();
            tlist.ForEach(x =>
            {
                w.SpotWBalance.Add(new eSpotWBalance() { SpotWallet = w, SessionHash = GetSessionHash(), Token = x, ConfirmBalance = 0.0 });
            });
            return true;
        }

        //True,True: If Exist and Can be Traded
        //True,False: Coin Exist but can not be traded
        //False,False: Coin doesn't exist and Cannot be Traded
        internal Tuple<bool, bool> DoesCoinExistAndCanTrade(Guid Tokenid)
        {
            if (Tokenid == Guid.Empty) return new Tuple<bool, bool>(false, false);
            var isT = dbctx.Token.FirstOrDefault(x => x.TokenId == Tokenid);
            if (isT is null) return new Tuple<bool, bool>(false, false);
            if (isT.DeletedOn.HasValue) return new Tuple<bool, bool>(true, false);
            return new Tuple<bool, bool>(true, true);
        }
        private bool GetReceiverUserOfFundNotification(smFundsNotification sm, out eUserAccount user, out eFundingNetworkWallet NetworkWalletAddress)
        {
            NetworkWalletAddress = null;
            var um = new UserManager();
            um.dbctx = dbctx;
            user = um.GetUserForNetworkWalletbyId(sm.userAccountId);
            if (user == null) return false;
            user.FundingWallet.myNetworkWallets.CheckAndThrowNullArgumentException();
            NetworkWalletAddress = user.FundingWallet.myNetworkWallets.FirstOrDefault(x => x.NetworkWalletAddress.Address.ToLower() == sm.WalletAddress.ToLower());
            NetworkWalletAddress.CheckAndThrowNullArgumentException();
            return NetworkWalletAddress != null;
        }
        private eToken GetExchangeTokenForFundNotification(smFundsNotification sm)
        {
            var sn = dbctx.SupportedNetwork.FirstOrDefault(x => x.NetworkProxy.ToLower() == sm.NetworkProxy.ToLower());
            if (sn == null)
                LogError($"ERROR:'GetExchangeTokenForFundNotification' No Such 'Supported Network Proxy':{sm.NetworkProxy} Exists that is provided in FundNotification:{sm.TranHash}");

            //#if (DEBUG)
            //            if (sn == null)
            //                sn = dbctx.SupportedNetwork.FirstOrDefault();
            //#endif
            sn.CheckAndThrowNullArgumentException();

            var sclst = dbctx.SupportedToken.Where(x => x.RelatedNetworkId == sn.SupportedNetworkId).ToList();
            eSupportedToken? sc = null;
            if (sm.IsNativeFund)
            {
                sc = sclst.FirstOrDefault(x => x.IsNative == true);
            }
            else
            {
                sc = sclst.FirstOrDefault(x => x.ContractAddress.IsNOT_NullorEmpty()
                && x.IsNative == false
                && x.ContractAddress.ToLower() == sm.TokenAddress.ToLower());
            }
            sc.CheckAndThrowNullArgumentException();
            var tk = dbctx.Token
                 .Include(x => x.SupportedCoin)
                 .FirstOrDefault(x => x.SupportedCoin.Any(z => z.SupportedTokenId == sc.SupportedTokenId));
            tk.CheckAndThrowNullArgumentException();
            return tk;
        }
        private eWalletType GetWalletType(Guid walletId)
        {
            if (dbctx.SpotWallet.Any(x => x.SpotWalletId == walletId))
                return eWalletType.SpotWallet;
            else if (dbctx.FundingWallet.Any(x => x.FundingWalletId == walletId))
                return eWalletType.FundingWallet;
            else if (dbctx.EscrowWallet.Any(x => x.EscrowWalletId == walletId))
                return eWalletType.EscrowWallet;
            else if (dbctx.EarnWallet.Any(x => x.EarnWalletId == walletId))
                return eWalletType.EarnWallet;
            else if (dbctx.InternalWallet.Any(x => x.InternalWalletId == walletId))
                return eWalletType.Internal;
            else return eWalletType.None;
        }
        private bool ConfirmInternalWalletWithSameUserAccount(Guid userAccountId, params Guid[] wallets)
        {
            var usr = dbctx.UserAccount
                .Include(x => x.SpotWallet)
                .Include(x => x.FundingWallet)
                .Include(x => x.EscrowWallet)
                .Include(x => x.EarnWallet)
                .FirstOrDefault(x => x.UserAccountId == userAccountId);
            if (usr is null) return false;
            var isOK = true;
            wallets.ToList().ForEach(x =>
            {
                isOK = isOK && usr.SpotWalletId == x || usr.FundingWalletId == x || usr.EscrowWalletId == x || usr.EarnWalletId == x;
            });

            return isOK;
        }
        private Guid GetWalletUserAccountId(Guid wallet)
        {
            var usr = dbctx.UserAccount
                .Include(x => x.SpotWallet)
                .Include(x => x.FundingWallet)
                .Include(x => x.EscrowWallet)
                .Include(x => x.EarnWallet)
                .FirstOrDefault(usr => usr.SpotWalletId == wallet || usr.FundingWalletId == wallet || usr.EscrowWalletId == wallet || usr.EarnWalletId == wallet);
            if (usr is null) return Guid.Empty;

            return usr.UserAccountId;
        }
        internal bool ToBank(string wAdd, string tAdd, bool IsNative)
        {
            SrvFundReceiver.PublishFundsToBankRequest(new smErc20ToBank { IsNative = IsNative, TokenAddress = tAdd, WalletAddress = wAdd });
            return true;
        }
        internal Tuple<Guid, double, eWalletType> GetAvailbaleConfirmBalanceOfWallet(Guid walletId, Guid tokenId)
        {
            if (walletId != Guid.Empty)
            {
                var w1 = dbctx.SpotWallet.Include(x => x.SpotWBalance).FirstOrDefault(x => x.SpotWalletId == walletId);
                if (w1 is not null)//wallet found
                {
                    var r = w1.SpotWBalance.Where(x => x.TokenId == tokenId).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                    if (r is not null) return new Tuple<Guid, double, eWalletType>(w1.InternalAccountNumber, r.ConfirmBalance, eWalletType.SpotWallet);
                    return new Tuple<Guid, double, eWalletType>(w1.InternalAccountNumber, 0, eWalletType.SpotWallet);
                }
                var w2 = dbctx.FundingWallet.Include(x => x.FundingWBalance).FirstOrDefault(x => x.FundingWalletId == walletId);
                if (w2 is not null)
                {
                    var r = w2.FundingWBalance.Where(x => x.TokenId == tokenId).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                    if (r is not null) return new Tuple<Guid, double, eWalletType>(w2.InternalAccountNumber, r.Balance, eWalletType.FundingWallet);
                    return new Tuple<Guid, double, eWalletType>(w2.InternalAccountNumber, 0, eWalletType.FundingWallet);
                }
                var w3 = dbctx.EarnWallet.Include(x => x.EarnWBalance).FirstOrDefault(x => x.EarnWalletId == walletId);
                if (w3 is not null)
                {
                    var r = w3.EarnWBalance.Where(x => x.TokenId == tokenId).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                    if (r is not null) return new Tuple<Guid, double, eWalletType>(w3.InternalAccountNumber, r.ConfirmBalance, eWalletType.EarnWallet);
                    return new Tuple<Guid, double, eWalletType>(w3.InternalAccountNumber, 0, eWalletType.EarnWallet);
                }
                var w4 = dbctx.EscrowWallet.Include(x => x.EscrowWBalance).FirstOrDefault(x => x.EscrowWalletId == walletId);
                if (w4 is not null)
                {
                    var r = w4.EscrowWBalance.Where(x => x.TokenId == tokenId).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                    if (r is not null) return new Tuple<Guid, double, eWalletType>(w4.InternalAccountNumber, r.Balance, eWalletType.EscrowWallet);
                    return new Tuple<Guid, double, eWalletType>(w4.InternalAccountNumber, 0, eWalletType.EscrowWallet);
                }
                var w5 = dbctx.InternalWallet.Include(x => x.InternalWBalance).FirstOrDefault(x => x.InternalWalletId == walletId);
                if (w5 is not null)
                {
                    var r = w5.InternalWBalance.Where(x => x.TokenId == tokenId).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                    if (r is not null) return new Tuple<Guid, double, eWalletType>(w5.InternalWalletId, r.Balance, eWalletType.Internal);
                    return new Tuple<Guid, double, eWalletType>(w5.InternalWalletId, 0, eWalletType.Internal);
                }
            }
            return new Tuple<Guid, double, eWalletType>(Guid.Empty, 0, eWalletType.None);

        }

        #region Private Methods

        internal eWalletTransaction CreateTransaction(Guid tokenId, Guid fromWallet, Guid toWallet, double amt, string narration, bool isExternalFund = false, bool isTrade = false)
        {
            //ToDo:Naveen, Write a Transaction lock here to ensure that no other Transaction can take place when this in underway..
            var f = GetAvailbaleConfirmBalanceOfWallet(fromWallet, tokenId);
            var t = GetAvailbaleConfirmBalanceOfWallet(toWallet, tokenId);

            var trans = new eWalletTransaction()
            {
                FromWalletId = fromWallet,
                FromWalletInternalAccountNo = f.Item1,
                FromWalletBeforeTransactionBalance = f.Item2,
                FromWalletAfterTransactionBalance = f.Item2 - amt,
                TAmount = amt,
                ToWalletId = toWallet,
                ToWalletInternalAccountNo = t.Item1,
                ToWalletBeforeTransactionBalance = t.Item2,
                ToWalletAfterTransactionBalance = t.Item2 + amt,
                Narration = narration,
                IsWithInMyWallet = !(isExternalFund || isTrade),
                RecordHash = "?",
                TokenId = tokenId
            };
            dbctx.WalletTransaction.Add(trans);
            dbctx.SaveChanges();
            trans.SignRecord(this);
            UpdateWalletBalance(tokenId, fromWallet, f.Item2 - amt, f.Item3, trans, (isExternalFund || isTrade));
            if (fromWallet != toWallet)//since 1 Trans should result 1 Wbalance Update
                UpdateWalletBalance(tokenId, toWallet, t.Item2 + amt, t.Item3, trans, (isExternalFund || isTrade));


            dbctx.SaveChanges();
            return trans;
        }
        void UpdateSpotWalletBalance(Guid tokenId, Guid WalletId, double amt, Guid transId, bool ExternalFunds = false)
        {
            if (!ExternalFunds)
            {
                var from_b = new eSpotWBalance() { SpotWalletId = WalletId, SessionHash = GetSessionHash(), TokenId = tokenId, ConfirmBalance = amt, ChangeAgent = transId };
                dbctx.SpotWBalance.Add(from_b);
                dbctx.SaveChanges();
            }
            else
            {
                var from_b = new eSpotWBalance() { SpotWalletId = WalletId, TokenId = tokenId, ConfirmBalance = amt, ChangeAgent = transId };
                dbctx.SpotWBalance.Add(from_b);
                dbctx.SaveChanges();
            }
            Console2.WriteLine_White($"{T}|SpotWallet id:{WalletId} Balance of:{tokenId} has been up Updated to:{amt} with Transaction Id:{transId}");

        }
        void UpdateFundingWalletBalance(Guid tokenId, Guid WalletId, double amt, Guid transId, bool ExternalFunds = false)
        {
            if (!ExternalFunds)
            {
                var from_b = new eFundingWBalance() { FundingWalletId = WalletId, SessionHash = GetSessionHash(), TokenId = tokenId, Balance = amt, ChangeAgent = transId };
                dbctx.FundingWBalance.Add(from_b);
                dbctx.SaveChanges();
            }
            else
            {
                var from_b = new eFundingWBalance() { FundingWalletId = WalletId, TokenId = tokenId, Balance = amt, ChangeAgent = transId };
                dbctx.FundingWBalance.Add(from_b);
                dbctx.SaveChanges();
            }

            Console2.WriteLine_White($"{T}|FundingWallet id:{WalletId} Balance of:{tokenId} has been up Updated to:{amt} with Transaction Id:{transId}");
        }
        void UpdateEarnWalletBalance(Guid tokenId, Guid WalletId, double amt, Guid transId, bool ExternalFunds = false)
        {
            if (!ExternalFunds)
            {
                var from_b = new eEarnWBalance() { EarnWalletId = WalletId, SessionHash = GetSessionHash(), TokenId = tokenId, ConfirmBalance = amt, ChangeAgent = transId };
                dbctx.EarnWBalance.Add(from_b);
                dbctx.SaveChanges();
            }
            else
            {
                var from_b = new eEarnWBalance() { EarnWalletId = WalletId, TokenId = tokenId, ConfirmBalance = amt, ChangeAgent = transId };
                dbctx.EarnWBalance.Add(from_b);
                dbctx.SaveChanges();
            }
            Console2.WriteLine_White($"{T}|EarnWallet id:{WalletId} Balance of:{tokenId} has been up Updated to:{amt} with Transaction Id:{transId}");

        }
        void UpdateInternalWalletBalance(Guid tokenId, Guid WalletId, double amt, Guid transId, bool ExternalFunds = false)
        {
            var from_b = new eInternalWBalance() { InternalWalletId = WalletId, TokenId = tokenId, Balance = amt, ChangeAgent = transId };
            dbctx.InternalWBalance.Add(from_b);
            dbctx.SaveChanges();

            Console2.WriteLine_White($"{T}|InternalWallet id:{WalletId} Balance of:{tokenId} has been up Updated to:{amt} with Transaction Id:{transId}");

        }
        void UpdateEscroWalletBalance(Guid tokenId, Guid WalletId, double amt, Guid transId)
        {
            var from_b = new eEscrowWBalance() { EscrowWalletId = WalletId, SessionHash = GetSessionHash(), TokenId = tokenId, Balance = amt, ChangeAgent = transId };
            dbctx.EscrowWBalance.Add(from_b);
            dbctx.SaveChanges();
            Console2.WriteLine_White($"{T}|EscroWallet id:{WalletId} Balance of:{tokenId} has been up Updated to:{amt} with Transaction Id:{transId}");
        }

        void UpdateWalletBalance(Guid tokenId, Guid fromWallet, double amt, eWalletType wt, eWalletTransaction trans, bool isExternalFund = false)
        {
            if (wt == eWalletType.SpotWallet)
                UpdateSpotWalletBalance(tokenId, fromWallet, amt, trans.WalletTransactionId, isExternalFund);
            else if (wt == eWalletType.FundingWallet)
                UpdateFundingWalletBalance(tokenId, fromWallet, amt, trans.WalletTransactionId, isExternalFund);
            else if (wt == eWalletType.EarnWallet)
                UpdateEarnWalletBalance(tokenId, fromWallet, amt, trans.WalletTransactionId, isExternalFund);
            else if (wt == eWalletType.EscrowWallet)
                UpdateEscroWalletBalance(tokenId, fromWallet, amt, trans.WalletTransactionId);
            else if (wt == eWalletType.Internal)
                UpdateInternalWalletBalance(tokenId, fromWallet, amt, trans.WalletTransactionId);
        }
        //internal bool MapInternalWithNetworkWallet()
        //{
        //    //Network Wallet is ScWallet but we must ensure that
        //    //1. This wallet is Issued by WalletWatch  in usually Publish Method
        //    //2. This is Published every time a service is started and every 5 minutes
        //    //3. we receive Transaction Deposit information into this Wallet that lead to update Global Internal Wallet that would be associated with this wallet.
        //}
        internal eNetworkWalletAddress? AddOrGetGlobalEtherumWallet()
        {
            var sn = dbctx.SupportedNetwork.FirstOrDefault(x => x.Name == "Etherum Main Net" && x.NativeCurrencyCode == "ETH");
            if (sn == null)
            {
                Console2.WriteLine_RED($"ERROR:Etherum Main Net Record is missing. Failed to Create Global Wallet..at:{DateTime.UtcNow}");
                return null;
            }
            var bw = NetworkScWallet.Ethereum.GetActiveBankWallet;
            var nw = dbctx.NetworkWalletAddress.FirstOrDefault(x => x.Address == bw && x.NetworkId == sn.SupportedNetworkId);

            if (nw == null)
            {
                nw = new eNetworkWalletAddress()
                {
                    NetworkId = sn.SupportedNetworkId,
                    Address = bw,
                    Notes = "Global Etherrum Wallet (Bank Wallet)",
                    Network = sn,
                    RecordHash = "?",
                    Category = eNetWorkWalletCategory.NavExMGlobal
                };
                dbctx.NetworkWalletAddress.Add(nw);
                dbctx.SaveChanges();
                nw.SignRecord(this);
            }
            //ToDo: publish this InternalWallet to Wallet Listner so that each Transaction of this wallet in MainNet get Reported in database
            Console2.WriteLine_DarkYellow("ToDo:Naveen, Implementa Global Network Wallet Transaction Listener for Detailed Records of all the Transaction");
            return nw;
        }
        internal bool AddOrGetDefaultGlobalInternalWallet()
        {
            var olst = dbctx.InternalWallet.Where(x => x.BelongsTo == "NavExM" && x.WalletNature == eWalletNature.Global && x.WalletType == eInternalWalletType.Global).ToList();

            if (olst != null && olst.Count > 0) return false;
            //ensure NetworkWallet Record
            var gw = AddOrGetGlobalEtherumWallet();

            if (gw == null) return false;
            var o = new eInternalWallet
            {
                Name = "NavExM 1",
                BelongsTo = "NavExM",
                WalletNature = eWalletNature.Global,
                WalletType = eInternalWalletType.Global,
            };
            dbctx.InternalWallet.Add(o);
            dbctx.SaveChanges();
            dbctx.InternalWalletMapToExternal.Add(new eInternalWalletMapToExternal
            {
                InternalWalletId = o.InternalWalletId,
                NetworkWalletAddressId = gw.NetworkWalletAddressId
            });
            dbctx.SaveChanges();

            return dbctx.SaveChanges() > 0;
        }
        /// <summary>
        /// INR Deposit Save for
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        internal Guid IntimateINRDeposit(mFiatDepositIntimation m)
        {
            m.CheckAndThrowNullArgumentException();
            m.PublicRequestID.CheckAndThrowNullArgumentException();
            m.uAccount.CheckAndThrowNullArgumentException();
            m.TaxResidencyCountryCode.CheckAndThrowNullArgumentException();
            m.TaxResidencyCountryName.CheckAndThrowNullArgumentException();
            m.KYCStatus.CheckAndThrowNullArgumentException();
            m.CurrencyCode.CheckAndThrowNullArgumentException();
            m.CurrencySymbole.CheckAndThrowNullArgumentException();
            m.Amount.CheckAndThrowNullArgumentException();
            //m.Charges.CheckAndThrowNullArgumentException();
            m.RequestedOn.CheckAndThrowNullArgumentException();
            m.NavExMBankDetails.CheckAndThrowNullArgumentException();
            m.SenderBankDetails.CheckAndThrowNullArgumentException();
            m.DepositEvidence.CheckAndThrowNullArgumentException();
            var e = m.ToEntity();
            e.Status.Add(new Data.Entity.Fund.eWithdrawlRequestStatus
            {
                PublicStatus = Data.Entity.Fund.RequestStatus.Submitted,
                InternalStatus = Data.Entity.Fund.RequestInternalStatus.Submitted
            });
            using (var db = _FundAuthctx("INR"))
            {
                db.FiatDepositRequest.Add(e);
                db.SaveChanges();
            }
            return e.FiatDepositRequestId;
        }
        /// <summary>
        /// INR Withdraw Request Save
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        internal mCryptoWithdrawRequestResult RequestINRWithdraw(mFiatWithdrawRequest m)
        {
            m.CheckAndThrowNullArgumentException();
            m.PublicRequestID.CheckAndThrowNullArgumentException();
            m.uAccount.CheckAndThrowNullArgumentException();
            m.TaxResidencyCountryCode.CheckAndThrowNullArgumentException();
            m.TaxResidencyCountryName.CheckAndThrowNullArgumentException();
            m.KYCStatus.CheckAndThrowNullArgumentException();
            m.CurrencyCode.CheckAndThrowNullArgumentException();
            m.CurrencySymbole.CheckAndThrowNullArgumentException();
            m.Amount.CheckAndThrowNullArgumentException();
            m.Charges.CheckAndThrowNullArgumentException();
            m.RequestedOn.CheckAndThrowNullArgumentException();
            m.ReceiverBankDetails.CheckAndThrowNullArgumentException();
            var e = m.ToEntity();

            var ret = new mCryptoWithdrawRequestResult
            {
                Amount = m.Amount,
                PublicRequestId = m.PublicRequestID,
                status = Data.Entity.Fund.CryptoWithdrawRequestStatus.Reserved,
                TokenCode = m.CurrencySymbole,
                CryptoWithdrawRequestId = e.FiatWithdrawRequestId
            };
            var tm = GetTokenManager();
            var um = GetUserManager();
            var ua = um.GetMyUserAccount();
            var t = tm.GetSpecificToken(m.CurrencyCode);
            if (t == null)
            {
                ret.status = Data.Entity.Fund.CryptoWithdrawRequestStatus.Failed;
                return ret;
            }
            var fbal = GetAvailbaleConfirmBalanceOfWallet(ua.FundingWalletId!.Value, t.TokenId);
            if (fbal.Item2 < m.Amount)
            {
                Console2.WriteLine_RED($"Amount Request in RequestId:{m.PublicRequestID} doesn't match with the Balance in FundWallet. Operation Abandonded..at:{DateTime.UtcNow}");
                ret.status = Data.Entity.Fund.CryptoWithdrawRequestStatus.Failed;
                return ret;
            }
            else
            {
                var gw = GetGlobalWallet();
                //done: Token Reservation Transaction
                var tra = CreateTransaction(t.TokenId, ua.FundingWalletId!.Value, gw.InternalWalletId, m.Amount, $"Withdraw Currency for Request Id:{m.PublicRequestID}", isExternalFund: true);
                ret.status = Data.Entity.Fund.CryptoWithdrawRequestStatus.Reserved;
                e.RequestRefId = tra.WalletTransactionId;
            }
            e.Status.Add(new Data.Entity.Fund.eWithdrawlRequestStatus
            {
                PublicStatus = Data.Entity.Fund.RequestStatus.Submitted,
                InternalStatus = Data.Entity.Fund.RequestInternalStatus.Submitted
            });
            using (var db = _FundAuthctx("INR"))
            {
                db.FiatWithdrawRequest.Add(e);
                db.SaveChanges();
            }
            return ret;
        }
        /// <summary>
        /// Cryto Token Network Withdraw Request
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        internal mCryptoWithdrawRequestResult RequestCryptoTokenWithdraw(mCryptoWithdrawRequest m)
        {
            m.CheckAndThrowNullArgumentException();
            m.PublicRequestID.CheckAndThrowNullArgumentException();
            m.uAccount.CheckAndThrowNullArgumentException();
            m.TaxResidencyCountryCode.CheckAndThrowNullArgumentException();
            m.TaxResidencyCountryName.CheckAndThrowNullArgumentException();
            m.KYCStatus.CheckAndThrowNullArgumentException();
            m.TokenCode.CheckAndThrowNullArgumentException();
            m.TokenId.CheckAndThrowNullArgumentException();
            m.TokenContractAddress.CheckAndThrowNullArgumentException();
            m.NetworkId.CheckAndThrowNullArgumentException();
            m.NetworkName.CheckAndThrowNullArgumentException();
            m.Amount.CheckAndThrowNullArgumentException();
            m.Charges.CheckAndThrowNullArgumentException();
            // m.RequestRefId.CheckAndThrowNullArgumentException();
            m.RequestedOn.CheckAndThrowNullArgumentException();
            m.ReceiverAddress.CheckAndThrowNullArgumentException();
            var e = m.ToEntity();
            //ToDo: validate for Funds Available
            var um = GetUserManager();
            var tm = GetTokenManager();
            var ua = um.GetMyUserAccount();

            var ret = new mCryptoWithdrawRequestResult
            {
                Amount = m.Amount,
                PublicRequestId = m.PublicRequestID,
                status = Data.Entity.Fund.CryptoWithdrawRequestStatus.Reserved,
                TokenCode = m.TokenCode,
                CryptoWithdrawRequestId = e.CryptoWithdrawRequestId
            };
            var t = tm.GetSpecificToken(m.TokenId);
            if (t == null)
            {
                ret.status = Data.Entity.Fund.CryptoWithdrawRequestStatus.Failed;
                return ret;
            }

            var sc = t.SupportedCoin.FirstOrDefault(x => x.RelatedNetworkId == m.NetworkId);
            e.TokenContractAddress = sc.ContractAddress.IsNullorEmpty() ? "Native" : sc.ContractAddress;
            if (m.IsAll)
            {
                //Sum of All should match the Amount
                var fbal = GetAvailbaleConfirmBalanceOfWallet(ua.FundingWalletId!.Value, m.TokenId);
                var sbal = GetAvailbaleConfirmBalanceOfWallet(ua.SpotWalletId!.Value, m.TokenId);
                var ebal = GetAvailbaleConfirmBalanceOfWallet(ua.EarnWalletId!.Value, m.TokenId);
                if ((fbal.Item2 + sbal.Item2 + ebal.Item2) == m.Amount && m.Amount > 0)
                {
                    var gw = GetGlobalWallet();
                    if (sbal.Item2 > 0)
                        CreateTransaction(m.TokenId, ua.SpotWalletId!.Value, ua.FundingWalletId!.Value, sbal.Item2, $"Move Token for Request Id:{m.PublicRequestID}", isExternalFund: false);

                    if (ebal.Item2 > 0)
                        CreateTransaction(m.TokenId, ua.EarnWalletId!.Value, ua.FundingWalletId!.Value, ebal.Item2, $"Move Token for Request Id:{m.PublicRequestID}", isExternalFund: false);

                    var tra = CreateTransaction(m.TokenId, ua.FundingWalletId!.Value, gw.InternalWalletId, m.Amount, $"Withdraw Token for Request Id:{m.PublicRequestID}", isExternalFund: true);
                    e.RequestRefId = tra.WalletTransactionId;
                    ret.status = Data.Entity.Fund.CryptoWithdrawRequestStatus.Reserved;
                    e.RequestRefId = tra.WalletTransactionId;
                }
                else
                {
                    Console2.WriteLine_RED($"Amount Request in RequestId:{m.PublicRequestID} doesn't match with the Collective Balance of all Wallets. Operation Abandonded..at:{DateTime.UtcNow}");
                }
            }
            else
            {
                //Amount must be >= wallet balance
                var fbal = GetAvailbaleConfirmBalanceOfWallet(ua.FundingWalletId!.Value, m.TokenId);
                if (fbal.Item2 < m.Amount)
                {
                    Console2.WriteLine_RED($"Amount Request in RequestId:{m.PublicRequestID} doesn't match with the Balance in FundWallet. Operation Abandonded..at:{DateTime.UtcNow}");
                    ret.status = Data.Entity.Fund.CryptoWithdrawRequestStatus.Failed;
                    return ret;
                }
                else
                {
                    var gw = GetGlobalWallet();
                    //done: Token Reservation Transaction
                    var tra = CreateTransaction(m.TokenId, ua.FundingWalletId!.Value, gw.InternalWalletId, m.Amount, $"Withdraw Token for Request Id:{m.PublicRequestID}", isExternalFund: true);
                    ret.status = Data.Entity.Fund.CryptoWithdrawRequestStatus.Reserved;
                    e.RequestRefId = tra.WalletTransactionId;
                }
            }

            //Update the Model Information with TranRef ID
            e.Status.Add(new Data.Entity.Fund.eWithdrawlRequestStatus
            {
                PublicStatus = Data.Entity.Fund.RequestStatus.Submitted,
                InternalStatus = Data.Entity.Fund.RequestInternalStatus.Submitted
            });
            //ToDo: Decide GREEN Channel Payment for this Withdraw over the NETWORK
            //Or
            //RED Channel (Manual Approval by FundManager)
            using (var db = _CyAuthctx())
            {
                db.CryptoWithdrawRequest.Add(e);
                db.SaveChanges();
            }

            return ret;
        }
        //
        internal bool OrderCancelTokenRefunds(eProcessedOrder po)
        {
            try
            {
                if (po == null || po.OrderID.IsNullorEmpty()) return false;
                if (po.Status == ServerModel.eOrderStatus.Cancelled || po.Status == ServerModel.eOrderStatus.Rejected)
                {
                    var um = GetUserManager();
                    var amt = (po.OriginalVolume - po.ProcessedVolume);
                    Guid tId = Guid.Empty;
                    if (po.OrderSide == ServerModel.eOrderSide.Buy)
                    {
                        tId = po.QuoteTokenId;
                        amt = amt * po.Price;
                    }
                    else
                        tId = po.BaseTokenId;

                    var wKey = SrvMarketProxy.GetOrderWalletForMarket(po.MarketCode);
                    var wID = um.GetSpotWalletOf(po.UserAccountNo);

                    var BNarr = $"{po.MarketCode}|{po.OrderID} Order Cancelled Refund";
                    var Btran = dbctx.Database.BeginTransaction();
                    var BTran = CreateTransaction(tId, wKey, wID, amt, BNarr, isTrade: true);

                    Btran.Commit();
                    Console2.WriteLine_White($"Infor:OrderCancel: Refund Completed for:{BNarr}");
                }
                else
                    return false;
                // Console2.WriteLine_RED($"ToDo:Naveen, Implement Cancel Order Token Refund");
                return true;
            }
            catch (Exception ex)
            {
                Console2.WriteLine_RED($"{T}|ERROR:OrderCancelTokenRefunds for Trade:{ex.GetDeepMsg()}");

            }
            return true;
        }
        internal bool TradePayOut(smTrade tr)
        {
            try
            {
                Console2.WriteLine_White($"{T}:Initiating trade payout for:{tr.TradeId}");
                //SIMPLE Implementation as of now
                // ToDo:OCO, Market and other Complex Order related implementation is pending
                var um = GetUserManager();
                var om = GetOrderManager();
                var wKey = SrvMarketProxy.GetOrderWalletForMarket(tr.MarketCode);
                var buy = om.GetOrderDetails(tr.MarketCode, tr.BuyInternalId, tr, out bool buyFirst, out var bW, out var buycStatus);

                var sell = om.GetOrderDetails(tr.MarketCode, tr.SellInternalId, tr, out bool SellFirst, out var sW, out var sellcStatus);

                Console2.WriteLine_RED($"ToDo:Naveen, Delete this Testing/Development Code before Prod Version");
                tr = SaveToDB(tr);

                if (buy == null || buy.UserAccountNo.IsNullorEmpty())
                {
                    // buy = om.x_GetFirstOrder(tr.MarketCode);
                    om.RegisterTradeTokenSettlementIssue(tr, new ApplicationException($"Buy Order:{tr.BuyOrderID} of this Trade:{tr.Id} Doesn't have Associated UserAccount.Token Credit Operation Suspended"));
                    Console2.WriteLine_RED($"Infor:TradePayout:Buy Order:{tr.BuyOrderID} of this Trade:{tr.Id} Doesn't have Associated UserAccount.Token Credit Operation Suspended");
                }
                else
                {
                    // var bW = um.GetSpotWalletOf(buy.UserAccountNo);
                    var cId = um.GetTaxResidencyOf(buy.UserAccountNo);
                    var RefCode = um.GetUserRefCodes(buy.UserAccountNo);
                    var SwapKeyBuy = SrvMarketProxy.GetSWAPWalletForMarket(tr.MarketCode, cId);

                    var BNarr = $"{tr.MarketCode}|{tr.BuyOrderID} Trade CREDTI";
                    var bsc = tr._BuySWAPRate * tr.TradeVolumn;

                    var Btran = dbctx.Database.BeginTransaction();

                    var BTran = CreateTransaction(buy.BaseTokenId, wKey, bW, tr.TradeVolumn - bsc, BNarr, isTrade: true);

                    BTran = CreateTransaction(buy.BaseTokenId, wKey, SwapKeyBuy, bsc, BNarr, isTrade: true);

                    Btran.Commit();
                    Console2.WriteLine_White($"Info:TradePayout:Trade PayOut Completed for:{BNarr}");
                    ScheduleAccrualCashback(tr, buy, buycStatus, RefCode.RefferedBy.IsNullorEmpty() ? "" : RefCode.RefferedBy);


                }

                if (sell == null || sell.UserAccountNo.IsNullorEmpty())
                {
                    // sell = om.x_GetFirstOrder(tr.MarketCode);
                    om.RegisterTradeTokenSettlementIssue(tr, new ApplicationException($"Sell Order:{tr.SellOrderID} of this Trade:{tr.Id} Doesn't have Associated UserAccount.Token Credit Operation Suspended"));
                    Console2.WriteLine_RED($"Infor:TradePayout:Sell Order:{tr.SellOrderID} of this Trade:{tr.Id} Doesn't have Associated UserAccount.Token Credit Operation Suspended");
                }
                else
                {
                    //var sW = um.GetSpotWalletOf(sell.UserAccountNo);
                    var cId = um.GetTaxResidencyOf(sell.UserAccountNo);
                    var RefCode = um.GetUserRefCodes(sell.UserAccountNo);
                    var SwapKeySell = SrvMarketProxy.GetSWAPWalletForMarket(tr.MarketCode, cId);

                    //Token DEBIT Transaction
                    var SNarr = $"{tr.MarketCode}|{tr.SellOrderID} Trade CREDTI";
                    var ssc = tr._SellSWAPRate * tr.TradeValue;
                    var tran = dbctx.Database.BeginTransaction();

                    var STran = CreateTransaction(sell.QuoteTokenId, wKey, sW, tr.TradeValue - ssc, SNarr, isTrade: true);
                    STran = CreateTransaction(sell.QuoteTokenId, wKey, SwapKeySell, ssc, SNarr, isTrade: true);
                    tran.Commit();
                    Console2.WriteLine_White($"Info:TradePayout:Trade PayOut Completed for:{SNarr}");

                    ScheduleAccrualCashback(tr, sell, sellcStatus, RefCode.RefferedBy.IsNullorEmpty() ? "" : RefCode.RefferedBy);

                }
                //First Time Trade Check and Reward
                if (buyFirst)
                {
                    var b = um.GetEarnWalletOf(buy.UserAccountNo);
                    PayMyReferredUserSignUpAwards(b);

                    GetAlertManager().SendUserAlertbySystem(buy.UserAccountNo, AlertMsg.Reward_YourFirstTradeReward());

                    var id = um.GetUserAccountId(buy.UserAccountNo);
                    ScheduleReferralSignUpAwards(id);
                }
                if (SellFirst)
                {
                    var s = um.GetEarnWalletOf(sell.UserAccountNo);
                    PayMyReferredUserSignUpAwards(s);

                    GetAlertManager().SendUserAlertbySystem(sell.UserAccountNo, AlertMsg.Reward_YourFirstTradeReward());

                    var id = um.GetUserAccountId(sell.UserAccountNo);
                    ScheduleReferralSignUpAwards(id);
                }
            }
            catch (Exception ex)
            {
                var om = GetOrderManager();
                om.RegisterTradeTokenSettlementIssue(tr, ex);
            }


            return true;
        }

        #region Rewards Transactions And Payments

        /// <summary>
        /// Provides User Account Number with its Associated wallets and its Associated wallets with Current balance
        /// </summary>
        /// <param name="ut">User Type</param>
        /// <param name="skip">Skip the number of Records</param>
        /// <param name="take">Take the Number of Records</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        internal List<mUserWallet> GetUsersWalletM(UserType ut, int skip = 0, int take = 20, bool withSummary = true)
        {
            string filter = "";
            if (skip < 0) skip = 0;
            if (take < 0) take = 20;
            switch (ut)
            {
                case UserType.User:
                    filter = $"U%";
                    break;
                case UserType.Staff:
                    filter = $"S%";
                    break;
                case UserType.StaffAdmin:
                    filter = $"SA%";
                    break;
                case UserType.MarketMaking:
                    filter = $"MM%";
                    break;
                default:
                    throw new ApplicationException($"{ut} is not currently supported in 'GetUsersWalletM'..at:{DateTime.UtcNow}");
            }
            var str1 = $"select * from UserAccount where FAccountNumber like '{filter}'";
            // var str2 = $"select UserAccountId from UserAccount where FAccountNumber like '{filter}'";
            var usr = dbctx.UserAccount.FromSqlRaw(str1).Skip(skip).Take(take).ToList();
            // var usr1 = dbctx.Database.SqlQueryRaw<Guid>(str2).ToList();

            var retval = usr.ToWalletModel();
            if (withSummary)
            {
                // var wm = GetWalletManager();
                //Get Balance of each of the wallet of each of the account
                foreach (var mWall in retval)
                {
                    mWall.SpotWallet = GetWalletSummery(mWall.SpotWallet.WalletId);
                    mWall.FundingWallet = GetWalletSummery(mWall.FundingWallet.WalletId);
                    mWall.EarnWallet = GetWalletSummery(mWall.EarnWallet.WalletId);
                    mWall.EscrowWallet = GetWalletSummery(mWall.EscrowWallet.WalletId);
                }
            }
            return retval;
        }
        /// <summary>
        /// This is User who is referred, It will Receive the Reward after first Trade, then and There.
        /// </summary>
        /// <param name="earnW"></param>
        /// <returns></returns>
        internal bool PayMyReferredUserSignUpAwards(Guid earnW)
        {
            try
            {
                if (earnW == Guid.Empty) return false;
                var tm = GetTokenManager();
                var NavC = tm.GetSpecificToken("navc");
                var price = SrvCoinWatch.GetCoin("navc").Price;
                var aw = dbctx.GlobalVariables.FirstOrDefault(x => x.Key.ToLower() == "ReferredUserSignUPOneTimeAward".ToLower());
                dbctx.UserAccount.First(x => x.EarnWalletId == earnW).RefCode.myRefRewardProcessed = true;
                if (aw == null || double.TryParse(aw.Value, out double awVal) == false) return false;

                var BNarr = $" Referred User One Time, First Trade Award";
                var BTran = CreateTransaction(NavC.TokenId, NavC.TokenId, earnW, awVal, BNarr, isExternalFund: true);
                return BTran != null;
            }
            catch (Exception ex)
            {

                Console2.WriteLine_RED($"'PayMyReferredUserSignUpAwards' Caused error for EarnWallet:{earnW}.\n{ex.GetDeepMsg()}");
            }
            return false;

        }
        internal bool ScheduleAccrualCashback(smTrade tr, smOrder or, eCommunityCategory cStatus, string RefCode)
        {
            try
            {
                if (cStatus != eCommunityCategory.None)
                {
                    using (var db = _Rewardctx())
                    {
                        var tran = new eAccruedCashBack
                        {
                            CashBackNavCValue = or.CashBackNavCValue,
                            InternalOrderID = or.InternalOrderID,
                            MarketCode = tr.MarketCode,
                            TradeId = tr.TradeId,
                            TradeOn = tr.dateTimeUTC,
                            userAccount = or.UserAccountNo,
                            TradeValue = tr.TradeValue,
                            Community = cStatus,
                            Narration = RefCode,
                            RecordHash = "?"
                        };
                        db.AccruedCashBack.Add(tran);
                        return db.SaveChanges() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console2.WriteLine_RED($"'ScheduleAccrualCashback' Caused error for Trade:{tr.TradeId} for Internal Order:{or.InternalOrderID}.\n{ex.GetDeepMsg()}");

            }
            return false;
        }
        /// <summary>
        /// This is Referral Earner, We will Record the development and pay him at the end of the Month, First Pay Cycle with Applicable navC Rate. Service will call and provision
        /// </summary>
        /// <param name="earnW"></param>
        /// <returns></returns>
        internal bool ScheduleReferralSignUpAwards(Guid userId)
        {

            var aw = dbctx.GlobalVariables.FirstOrDefault(x => x.Key.ToLower() == "ReferralSignUPOneTimeAward".ToLower());
            double awVal = 0;
            if (aw == null || double.TryParse(aw.Value, out awVal) == false)
            {
                Console2.WriteLine_RED("'ReferralSignUPOneTimeAward' No Such Globle Entry for Reward Event");
                return false;
            }
            Rewardctx ??= _Rewardctx();
            var usr = Rewardctx.SignUpUsers.FirstOrDefault(x => x.UserAccountId ==
  userId);
            if (usr == null) return false;
            usr.RewardEventOn = DateTime.UtcNow.Date;
            usr.Status = eRewardStatus.Earned;
            usr.RewardBaseValue = awVal;
            return Rewardctx.SaveChanges() > 0;
        }
        internal bool RecordCommunityStatusForReffer(string ReferCode, Guid AccID)
        {
            Rewardctx ??= _Rewardctx();
            var usr = Rewardctx.SignUpCommunityUser.FirstOrDefault(x => x.UserAccountId == AccID);
            if (usr != null)
                usr.LastVerificationOn = DateTime.UtcNow;
            else
            {
                usr = new SignUpCommunityUser
                {
                    RefCode = ReferCode,
                    LastVerificationOn = DateTime.UtcNow,
                    StatusGrantedOn = DateTime.UtcNow,
                    UserAccountId = AccID
                };
                Rewardctx.SignUpCommunityUser.Add(usr);
            }
            return Rewardctx.SaveChanges() > 0;//Different Ctx..call here
        }
        internal bool DEBITCashBackForThisCycle(DateTime startDate, DateTime endDate, double basValNavC)
        {
            Rewardctx ??= _Rewardctx();
            var myLock = Guid.NewGuid();
            var NavC = dbctx.Token.FirstOrDefault(x => x.Code.ToLower() == "navc");
            var RewardWallet = GetGlobalRewardWallet();
            var um = GetUserManager();
            //Now Update Reward with UserSummary Info
            List<string> ulst = new List<string>();
            do
            {
                ulst = Rewardctx.AccruedCashBack.Where(x => x.CashBackNavCValue > 0
            && x.CashBackNavCTokens == 0 && x.RewardTransactionId.HasValue == false && x.ProvisionedOn > startDate && x.ProvisionedOn <= endDate).Select(x => x.userAccount)
                .Distinct().Take(10).ToList();
                foreach (var u in ulst)
                {
                    Rewardctx.AccruedCashBack.Where(x => x.userAccount == u).ExecuteUpdate(prop => prop
             .SetProperty(a => a.RewardTransactionId, a => myLock));


                    //Loop till completed
                    var lst = Rewardctx.AccruedCashBack.Where(x => x.RewardTransactionId == myLock).ToList();
                    var glst = lst.GroupBy(x => x.userAccount);
                    foreach (var grp in glst)
                    {
                        Console2.WriteLine_White($"Info:DEBIT CashBack for:{grp.Key}");

                        var usr = um.GetUserOfAcc(grp.Key);
                        // Group these Trans into all 4 type of Possible Community Status
                        var grpCom = grp.GroupBy(x => x.Community);

                        var cs = GetMyCommunityStatus(usr.FAccountNumber);
                        if (cs != eCommunityCategory.None)//Current Community Status
                        {
                            foreach (var cStatus in grpCom)//Tran Time Status
                            {
                                var tCountVal = cStatus.Sum(x => x.CashBackNavCValue);
                                var tCount = tCountVal / basValNavC;
                                if (tCount > 0 && cStatus.Key <= cs && cStatus.Key != eCommunityCategory.None)
                                {
                                    var Narr = $"CashBack NavC({tCount} of {tCountVal} usdt @ {basValNavC}) DEBIT for Cycle End at{(endDate.ToString("dd-MMM-yyyy"))}";
                                    Console2.WriteLine_White($"Info:{Narr}");

                                    var tran = CreateTransaction(NavC!.TokenId, RewardWallet.InternalWalletId, usr.EarnWalletId!.Value, tCount, Narr, true);
                                    Rewardctx.AccruedCashBack.Where(x => x.userAccount == grp.Key && x.RewardTransactionId == myLock && x.Community <= cStatus.Key)
                                .ExecuteUpdate(prop => prop
                      .SetProperty(x => x.RewardTransactionId, tran.WalletTransactionId)
                      .SetProperty(x => x.CashBackNavCTokens, x => x.CashBackNavCValue / basValNavC));
                                }
                                else
                                {
                                    Rewardctx.AccruedCashBack.Where(x =>
                            x.userAccount == grp.Key && x.RewardTransactionId == myLock)
                                .ExecuteUpdate(prop => prop
                            .SetProperty(x => x.RewardTransactionId, Guid.Empty)
                            .SetProperty(x => x.CashBackNavCTokens, x => x.CashBackNavCValue / basValNavC)
                            .SetProperty(x => x.PoolRefundNavCTokens, x => x.CashBackNavCValue / basValNavC));
                                }
                            }
                        }
                        else
                        {
                            Rewardctx.AccruedCashBack.Where(x =>
                            x.userAccount == grp.Key && x.RewardTransactionId == myLock)
                                .ExecuteUpdate(prop => prop
                            .SetProperty(x => x.RewardTransactionId, Guid.Empty)
                            .SetProperty(x => x.CashBackNavCTokens, x => x.CashBackNavCValue / basValNavC)
                            .SetProperty(x => x.PoolRefundNavCTokens, x => x.CashBackNavCValue / basValNavC));
                        }
                    }

                }
                ulst = Rewardctx.AccruedCashBack.Where(x => x.CashBackNavCValue > 0
             && x.CashBackNavCTokens == 0 && x.RewardTransactionId.HasValue == false && x.ProvisionedOn > startDate && x.ProvisionedOn <= endDate).Select(x => x.userAccount)
                 .Distinct().Take(10).ToList();
            } while (ulst.Count() > 0);
            return true;
        }
        internal bool DEBITRewardForThisMonth(double basValNavC)
        {
            Rewardctx ??= _Rewardctx();
            var myLock = Guid.NewGuid();
            var lastMonthEnd = DateTime.UtcNow.AddDays(-DateTime.UtcNow.Day).Date;
            var lastMonthBegin = lastMonthEnd.AddMonths(-1);
            var NavC = dbctx.Token.FirstOrDefault(x => x.Code.ToLower() == "navc");
            var RewardWallet = GetGlobalRewardWallet();
            var um = GetUserManager();
            var ulst = new List<string>();
            //Now Update Reward with UserSummary Info
            do
            {
                ulst = Rewardctx.Rewards.Where(x => x.RewardBaseValue > 0
              && x.Reward.HasValue == false && x.ForMonthOf == lastMonthEnd
              && x.TransactionId == Guid.Empty)
                  .Select(x => x.RefCode).Distinct().Take(10).ToList();
                foreach (var u in ulst)
                {
                    Rewardctx.Rewards.Where(x => x.RefCode == u)
                .ExecuteUpdate(prop => prop
             .SetProperty(a => a.TransactionId, a => myLock));

                    //Loop till completed
                    var lst = Rewardctx.Rewards.Where(x => x.TransactionId == myLock).ToList();
                    var glst = lst.GroupBy(x => x.RefCode);
                    foreach (var grp in glst)
                    {
                        Console2.WriteLine_White($"Info:DEBIT Reward for:{grp.Key}");
                        var usr = um.GetUserOfCommunity(grp.Key);
                        var tCount = grp.Sum(x => x.RewardBaseValue) / basValNavC;
                        var Narr = $"Reward NavC({tCount}) DEBIT for Month Of {(lastMonthEnd.ToString("dd-MMM-yyyy"))}";
                        Console2.WriteLine_White($"Info:{Narr}");
                        //Reward NavC Payment
                        var tran = CreateTransaction(NavC!.TokenId, RewardWallet.InternalWalletId, usr.EarnWalletId!.Value, tCount, Narr, true);

                        Rewardctx.Rewards.Where(x => x.RefCode == grp.Key && x.TransactionId == myLock).ExecuteUpdate(prop => prop
                        .SetProperty(x => x.TransactionId, tran.WalletTransactionId)
                        .SetProperty(x => x.Reward, x => x.RewardBaseValue / basValNavC)
                        .SetProperty(x => x.DateOfPayment, tran.CreatedOn));

                        Rewardctx.SignUpUserSummary.Where(x => x.RefCode == grp.Key && x.RegisteredOnMonth == lastMonthEnd && x.Reward.HasValue == false).ExecuteUpdate(prop => prop
                        .SetProperty(x => x.Reward, x => x.RewardBaseValuePaid / basValNavC));

                        Rewardctx.SignUpUsers.Where(x => x.RefCode == grp.Key && x.RewardEventOn <= lastMonthEnd && x.RewardEventOn > lastMonthBegin && x.Reward.HasValue == false)
                            .ExecuteUpdate(prop => prop
                       .SetProperty(x => x.Reward, x => x.RewardBaseValue / basValNavC)
                       .SetProperty(x => x.Status, eRewardStatus.Paid)
                       .SetProperty(x => x.DateOfPayment, tran.Date)
                       .SetProperty(x => x.TransactionId, tran.WalletTransactionId)
                       );
                    }
                }
                ulst = Rewardctx.Rewards.Where(x => x.RewardBaseValue > 0
            && x.Reward.HasValue == false && x.ForMonthOf == lastMonthEnd
            && x.TransactionId == Guid.Empty)
                .Select(x => x.RefCode).Distinct().Take(10).ToList();
            } while (ulst.Count() > 0);
                SrvNavCCashbackPool.IsRefCalculationBegin = false;
                return true;
            }
        /// <summary>
        /// It is Long Running Resource intensive Operation.
        /// It will calculate all Referral Rewards for the Last Month(Calander)
        /// </summary>
        /// <param name="basValNavC"></param>
        /// <returns></returns>
        internal bool CalculateRewardForThisMonth()
            {
                /*
                 * Reward Types:-
                 *     - User Referral Registered award =1$NavC
                 *     - User Self Registered award =3$NavC
                 *     - CashBack Prepatual Award based on Community Size
                 *     - for Non-Qualified Prepatual Award winner onetime
                 *         - 10$ navC for every 100 Active Registered User of this Month
                 */
                SrvNavCCashbackPool.IsRefCalculationBegin = true;
                Rewardctx ??= _Rewardctx();
                var myLock = Guid.NewGuid();
                var lastMonthEnd = DateTime.UtcNow.AddDays(-DateTime.UtcNow.Day).Date;
                var lastMonthBegin = lastMonthEnd.AddMonths(-1);
                var ulst = new List<string>();
                //Referral Reward Calculation
                //Lock Records in case of Multi Instances
                do
                {
                    ulst = Rewardctx.SignUpUsers.Where(x => x.Status == eRewardStatus.Earned
               && x.RewardEventOn.HasValue && x.RewardEventOn.Value <= lastMonthEnd && x.RewardEventOn.Value > lastMonthBegin
               && (x.DateOfPayment.HasValue == false || x.DateOfPayment.Value < DateTime.UtcNow))
                     .OrderByDescending(x => x.RefCode)
                     .OrderByDescending(x => x.RegisteredOn).Select(x => x.RefCode).Distinct()
                     .Take(50).ToList();//
                    foreach (var u in ulst)
                    {

                        Rewardctx.SignUpUsers.Where(x => x.RefCode == u)
                            .ExecuteUpdate(p => p
                            .SetProperty(a => a.TransactionId, myLock)
                        .SetProperty(a => a.DateOfPayment, DateTime.UtcNow.AddMinutes(1)));

                        var dlst = Rewardctx.SignUpUsers.Where(x => x.TransactionId.HasValue && x.TransactionId.Value == myLock).ToList();
                        var lst = dlst.GroupBy(x => x.RefCode).ToList();
                        foreach (var rec in lst)
                        {
                            var obj = new SignUpUserSummary
                            {
                                RefCode = rec.Key,
                                Count = rec.Count(),
                                RegisteredOnMonth = lastMonthEnd,
                                RewardBaseValuePaid = rec.Sum(z => z.RewardBaseValue) * 1//1 USDT per referred User
                            };
                            var Reward = new RefReward
                            {
                                CalculatedOn = DateTime.UtcNow,
                                ForMonthOf = lastMonthEnd,
                                RefCode = rec.Key,
                                RewardBaseValue = obj.RewardBaseValuePaid,
                                RewardType = eRewardType.Referral.ToString()
                            };
                            Rewardctx.Rewards.Add(Reward);
                            Rewardctx.SignUpUserSummary.Add(obj);
                            Rewardctx.SaveChanges();

                            Rewardctx.SignUpUsers.Where(x => x.TransactionId.HasValue && x.TransactionId.Value == myLock && x.RefCode == obj.RefCode).ExecuteDelete();
                        }
                    }
                    ulst = Rewardctx.SignUpUsers.Where(x => x.Status == eRewardStatus.Earned
                    && x.RewardEventOn.HasValue && x.RewardEventOn.Value <= lastMonthEnd && x.RewardEventOn.Value > lastMonthBegin
                    && (x.DateOfPayment.HasValue == false || x.DateOfPayment.Value < DateTime.UtcNow))
                          .OrderByDescending(x => x.RefCode)
                          .OrderByDescending(x => x.RegisteredOn).Select(x => x.RefCode).Distinct()
                          .Take(50).ToList();//
                } while (ulst.Count() > 0);

                //Prepetual Award based on cashback Calculation
                do
                {
                    ulst = Rewardctx.AccruedCashBack.Where(x => (x.ReserveId == null || x.ReserveId.Equals("")) &&
                     x.ProvisionedOn <= lastMonthEnd && x.ProvisionedOn > lastMonthBegin
                     ).Select(x => x.Narration).Distinct()
                     .Take(50).ToList();
                    foreach (var u in ulst)
                    {
                        Rewardctx.AccruedCashBack.Where(x => x.Narration == u)
                            .ExecuteUpdate(p => p
                              .SetProperty(a => a.ReserveId, myLock.ToString())
                              .SetProperty(a => a.ReservasationExpiry, DateTime.UtcNow.AddMinutes(3)));

                        //All Available Records with lock
                        var dlst = Rewardctx.AccruedCashBack.Where(x => x.ReserveId == myLock.ToString()).ToList();
                        Console2.WriteLine_White($"Info:Prepetual Award Calculation is underway for:{dlst.Count}..at:{DateTime.UtcNow}");
                        //Group of Referrers
                        var glst = dlst.GroupBy(x => x.Narration);
                        foreach (var g in glst)
                        {
                            //Members of this Referrer Community
                            var clst = g.GroupBy(x => x.userAccount).ToList();
                            if (clst.Count() >= 0 && clst.Count() < 5000)
                            {
                                //No Pay for Prepetual
                                /*- for Non - Qualified Prepatual Award winner onetime
                                * -10$ navC for every 100 Active Registered User of this Month
                                */
                                var uCount = Rewardctx.SignUpUserSummary.Where(x => x.RefCode == g.Key && x.RegisteredOnMonth == lastMonthEnd).Sum(x => x.Count);

                                if (uCount > 0)
                                {
                                    var re = new RefReward
                                    {
                                        CalculatedOn = DateTime.UtcNow,
                                        ForMonthOf = lastMonthEnd,
                                        RefCode = g.Key,
                                        RewardBaseValue = uCount / 10,
                                        RewardType = eRewardType.OneTimeBonus.ToString()
                                    };
                                    Rewardctx.Rewards.Add(re);
                                    Rewardctx.SaveChanges();
                                }

                            }
                            if (clst.Count() >= 5000 && clst.Count() < 10000)//20%
                            {
                                double rate = 0.2;
                                var sum = dlst.Where(x => x.Narration == g.Key).Sum(x => x.CashBackNavCValue);

                                var re = new RefReward { CalculatedOn = DateTime.UtcNow, ForMonthOf = lastMonthEnd, RefCode = g.Key, RewardBaseValue = sum * rate, RewardType = eRewardType.Prepetual.ToString() };
                                Rewardctx.Rewards.Add(re);
                                Rewardctx.SaveChanges();
                                Rewardctx.AccruedCashBack.Where(x => x.Narration == g.Key && x.ProvisionedOn <= lastMonthEnd && x.ProvisionedOn > lastMonthBegin).ExecuteUpdate(pro => pro
                                .SetProperty(x => x.ReserveId, Guid.NewGuid().ToString())
                                .SetProperty(x => x.ReservasationExpiry, DateTime.MaxValue));
                            }
                            if (clst.Count() >= 10000 && clst.Count() < 20000)//30%
                            {
                                double rate = 0.3;
                                var sum = dlst.Where(x => x.Narration == g.Key).Sum(x => x.CashBackNavCValue);

                                var re = new RefReward { CalculatedOn = DateTime.UtcNow, ForMonthOf = lastMonthEnd, RefCode = g.Key, RewardBaseValue = sum * rate, RewardType = eRewardType.Prepetual.ToString() };
                                Rewardctx.Rewards.Add(re);
                                Rewardctx.SaveChanges();
                                Rewardctx.AccruedCashBack.Where(x => x.Narration == g.Key && x.ProvisionedOn <= lastMonthEnd && x.ProvisionedOn > lastMonthBegin).ExecuteUpdate(pro => pro
                                .SetProperty(x => x.ReserveId, Guid.NewGuid().ToString())
                                .SetProperty(x => x.ReservasationExpiry, DateTime.MaxValue));

                            }
                            if (clst.Count() >= 20000 && clst.Count() < 100000)//40%
                            {
                                double rate = 0.4;
                                var sum = dlst.Where(x => x.Narration == g.Key).Sum(x => x.CashBackNavCValue);

                                var re = new RefReward { CalculatedOn = DateTime.UtcNow, ForMonthOf = lastMonthEnd, RefCode = g.Key, RewardBaseValue = sum * rate, RewardType = eRewardType.Prepetual.ToString() };
                                Rewardctx.Rewards.Add(re);
                                Rewardctx.SaveChanges();
                                Rewardctx.AccruedCashBack.Where(x => x.Narration == g.Key && x.ProvisionedOn <= lastMonthEnd && x.ProvisionedOn > lastMonthBegin).ExecuteUpdate(pro => pro
                                .SetProperty(x => x.ReserveId, Guid.NewGuid().ToString())
                                .SetProperty(x => x.ReservasationExpiry, DateTime.MaxValue));

                            }
                            if (clst.Count() >= 100000)//50%
                            {
                                double rate = 0.5;
                                var sum = dlst.Where(x => x.Narration == g.Key).Sum(x => x.CashBackNavCValue);

                                var re = new RefReward { CalculatedOn = DateTime.UtcNow, ForMonthOf = lastMonthEnd, RefCode = g.Key, RewardBaseValue = sum * rate, RewardType = eRewardType.Prepetual.ToString() };
                                Rewardctx.Rewards.Add(re);
                                Rewardctx.SaveChanges();
                                Rewardctx.AccruedCashBack.Where(x => x.Narration == g.Key && x.ProvisionedOn <= lastMonthEnd && x.ProvisionedOn > lastMonthBegin).ExecuteUpdate(pro => pro
                                .SetProperty(x => x.ReserveId, Guid.NewGuid().ToString())
                                .SetProperty(x => x.ReservasationExpiry, DateTime.MaxValue));

                            }

                        }
                    }
                    ulst = Rewardctx.AccruedCashBack.Where(x => (x.ReserveId == null || x.ReserveId.Equals("")) &&
                  x.ProvisionedOn <= lastMonthEnd && x.ProvisionedOn > lastMonthBegin
                  ).Select(x => x.Narration).Distinct()
                  .Take(50).ToList();
                } while (ulst.Count() > 0);
                return true;
            }
            /// <summary>
            /// List of Rewards that are Earned or Paid, all the way back, untill deleted
            /// </summary>
            /// <param name="RefCode"></param>
            /// <returns></returns>
            /// <remarks> we will usually have 1 Transaction of Referral and cashback each </remarks>
            internal List<mMyReward> GetMyRewardsRecords(string RefCode)
            {
                Rewardctx ??= _Rewardctx();
                var paidlst = (from r in Rewardctx.Rewards

                               where r.RefCode == RefCode
                               select new mMyReward
                               {
                                   EarnedOn = r.ForMonthOf.Date,
                                   PaidOn = r.DateOfPayment,
                                   RewardType = r.RewardType,//Referral or CashBack
                                   Amount = r.RewardBaseValue,
                                   Reward = r.Reward,
                                   Status = eRewardStatus.Paid
                               }).ToList();

                var earnedlst = Rewardctx.SignUpUsers.Where(x => x.RefCode == RefCode && x.Status == eRewardStatus.Earned && x.RewardEventOn.HasValue).ToList()
                    .GroupBy(x => new { x.RewardEventOn!.Value.Date })
                    .Select(x => new mMyReward
                    {
                        EarnedOn = x.Key.Date,
                        PaidOn = null,
                        RewardType = eRewardType.Referral.ToString(),
                        Amount = x.Sum(z => z.RewardBaseValue),
                        Status = eRewardStatus.Earned
                    }).ToList();

                earnedlst.AddRange(paidlst);

                earnedlst = earnedlst.OrderByDescending(x => x.EarnedOn).ToList();

                return earnedlst.OrderByDescending(x => x.EarnedOn).ToList();

            }
            internal mRewardStats GetReferredRewardStat(string RefCode)
            {
                Rewardctx ??= _Rewardctx();
                mRewardStats ret = new mRewardStats();
                RefCode.CheckAndThrowNullArgumentException();

                var lm_end = DateTime.UtcNow.AddDays(-DateTime.UtcNow.Day).Date;
                var lm_start = lm_end.AddMonths(-1).Date;
                var MaxRegMonth = Rewardctx.SignUpUserSummary.Where(x => x.RefCode == RefCode).Max(x => x.RegisteredOnMonth);

                ret.TotalNoOfRef =
                    Rewardctx.SignUpUsers.Where(x => x.RefCode == RefCode && (x.Status == eRewardStatus.None || x.RewardEventOn > MaxRegMonth)).Count()
                    +
                    Rewardctx.SignUpUserSummary.Where(x => x.RefCode == RefCode).Sum(x => x.Count);


                //till last month end
                ret.LastMonthRef = Rewardctx.SignUpUsers.Where(x => x.RefCode == RefCode && (x.Status == eRewardStatus.None) && x.RegisteredOn <= lm_end).Count()
                     +
                    Rewardctx.SignUpUserSummary.Where(x => x.RefCode == RefCode && x.RegisteredOnMonth <= lm_end).Sum(x => x.Count);



                ret.LastMonthPayStatus = Rewardctx.SignUpUsers.Where(x => x.RefCode == RefCode && x.RegisteredOn <= lm_end && x.RegisteredOn > lm_start && x.DateOfPayment.HasValue).Count() > 0;

                ret.ThisMonthRef = ret.TotalNoOfRef - ret.LastMonthRef;
                ret.ThisMonthPayStatus = false;
                ret.CommunityUserCount = Rewardctx.SignUpCommunityUser.Where(x => x.LastVerificationOn > lm_start).Count();


                //QualifiedUserCount


                ret.QualifiedUserCount = Rewardctx.SignUpUsers.Count(x => x.RefCode == RefCode && x.Status >= eRewardStatus.Earned && x.RewardEventOn > MaxRegMonth)
                    +
                      Rewardctx.SignUpUserSummary.Where(x => x.RefCode == RefCode).Sum(x => x.Count);

                //registered User Counts
                ret.RegisteredUserCount = ret.QualifiedUserCount
                    +
                    Rewardctx.SignUpUsers.Count(x => x.RefCode == RefCode && x.PrimaryCompletedOn.HasValue && x.Status == eRewardStatus.None);


                //NavC count
                var v = Rewardctx.Rewards.Where(x => x.RefCode == RefCode && x.RewardType == eRewardType.Referral.ToString()).Sum(x => x.Reward);

                if (v.HasValue)
                    ret.NavCPaid = v.Value;
                //USDT value
                ret.NavCUnPaid = Rewardctx.SignUpUsers.Where(x => x.RefCode == RefCode && x.Status == eRewardStatus.Earned).Sum(x => x.RewardBaseValue);

                return ret;
            }
            internal List<mUserRef> GetReferredUsersRewardByDateGroup(string RefCode)
            {
                RefCode.CheckAndThrowNullArgumentException();
                List<mUserRef> retval = new List<mUserRef>();
                var result = Rewardctx.SignUpUsers.Where(x => x.RefCode == RefCode).ToList().GroupBy(x => new
                { x.RegisteredOn, x.Status }
                ).ToList();

                var lst = (from x in result
                           select new mUserRef
                           {
                               RegistrationDate = x.Key.RegisteredOn,
                               UserStatus = x.Key.Status.ToString(),
                               Amount = x.Sum(z => z.RewardBaseValue),
                               Count = x.Count()
                           }).ToList();
                return lst.OrderByDescending(x => x.RegistrationDate).ToList();
            }
            internal int GetReferredUsersCount(string RefCode)
            {
                RefCode.CheckAndThrowNullArgumentException();
                var result = dbctx.UserAccount.Count(x => x.RefCode.RefferedBy == RefCode);
                return result;
            }
            internal List<mAccruedCashBack> GetMyCashbackRecords()
            {
                var um = GetUserManager();
                var sess = um.GetMySession();
                using (var db = _Rewardctx())
                {
                    var ret = db.AccruedCashBack.Where(x => x.userAccount.ToLower() == sess.UserAccount.AccountNumber.ToLower()).OrderByDescending(x => x.CreatedOn)
                        .ToList().ToModel();
                    return ret;
                }
            }
            internal List<Tuple<string, int>> GetReferredUsersGroupBy()
            {
                var ret = new List<Tuple<string, int>>();
                var result = dbctx.UserAccount.ToList().GroupBy(x => x.RefCode.RefferedBy).ToList();
                foreach (var x in result)
                {
                    var k = x.Key.IsNOT_NullorEmpty() ? x.Key : "";
                    ret.Add(new Tuple<string, int>(k, x.Count()));
                }
                return ret;
            }
            #endregion
            smTrade SaveToDB(smTrade tr)
            {
                if (tr == null) return tr;
                using (var db = Orderctx(tr.MarketCode))
                {
                    try
                    {
                        db.Trades.Add(tr);
                        db.SaveChanges();
                        return tr;
                    }
                    catch (Exception ex)
                    {
                        Console2.WriteLine_RED($"{T}|ERROR:SaveToDB for Trade:{ex.GetDeepMsg()}");
                    }

                }
                return null;
            }
            private OrderAppContext Orderctx(string mCode)
            {
                try
                {
                    var o = new DbContextOptionsBuilder<OrderAppContext>();
                    var str = ConfigEx.Config.GetConnectionString($"OrderAppContext");
                    str = str!.Replace("<template>", $"OrdBank{mCode}");
                    o = o.UseSqlServer(str);
                    var ctx = new OrderAppContext(o.Options);
                    ctx.Database.SetConnectionString(str);
                    ctx.Database.EnsureCreated();
                    return ctx;
                }
                catch (Exception ex)
                {
                    SrvPlugIn.LogErrorG($"{AppConfigBase.RegistryToken!.AppId}- in Market:{mCode}|{T} in Connection caused error in Order SQL Connection/Object {ex.GetDeepMsg()}");
                }
                return null;
            }
            private OrderManager GetOrderManager()
            {
                var result = new OrderManager();
                result.dbctx = dbctx;
                result.httpContext = httpContext;
                return result;
            }
            private UserManager GetUserManager()
            {
                var result = new UserManager();
                result.dbctx = dbctx;
                result.httpContext = httpContext;
                return result;
            }
            private TokenManager GetTokenManager()
            {
                var result = new TokenManager();
                result.dbctx = dbctx;
                result.httpContext = httpContext;
                return result;
            }
            private WalletManager GetWalletManager()
            {
                //ToDo: Secure this Manager, Transaction Count Applied, Active Session

                var result = new WalletManager();
                result.dbctx = dbctx;

                return result;
            }
            private AlertManager GetAlertManager()
            {
                var result = new AlertManager();
                result.dbctx = dbctx;

                return result;
            }
            private CyAuthAppContext _CyAuthctx()
            {
                var o = new DbContextOptionsBuilder<CyAuthAppContext>();
                o = o.UseSqlServer(ConfigEx.Config.GetConnectionString("CyAuthAppContext"));
                return new CyAuthAppContext(o.Options);
            }
            private RewardAppContext _Rewardctx()
            {
                var o = new DbContextOptionsBuilder<RewardAppContext>();
                o = o.UseSqlServer(ConfigEx.Config.GetConnectionString("RewardAppContext"));
                return new RewardAppContext(o.Options);
            }
            private FundRequestAppContext _FundAuthctx(string mCode)
            {
                try
                {
                    var o = new DbContextOptionsBuilder<FundRequestAppContext>();
                    var str = ConfigEx.Config.GetConnectionString($"FundRequestAppContext");
                    str = str!.Replace("<template>", $"FundRequest_{mCode}");
                    o = o.UseSqlServer(str);
                    var ctx = new FundRequestAppContext(o.Options);
                    ctx.Database.SetConnectionString(str);
                    ctx.Database.EnsureCreated();
                    return ctx;
                }
                catch (Exception ex)
                {
                    SrvPlugIn.LogErrorG($"{AppConfigBase.RegistryToken!.AppId}- in FundRequestAppContext Currency:{mCode}|{T} in Connection caused error in Order SQL Connection/Object {ex.GetDeepMsg()}");
                }
                return null;
            }

            #endregion
        }

    }
