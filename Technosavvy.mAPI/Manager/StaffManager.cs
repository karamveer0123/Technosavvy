using NavExM.Int.Maintenance.APIs.ServerModel;
using NavExM.Int.Maintenance.APIs.Services;

namespace NavExM.Int.Maintenance.APIs.Manager
{
    internal class StaffManager : ManagerBase
    {
        //ToDo: Naveen, This Function should never be used in this, MarketManager Staff App will be doing this activity 
        internal Tuple<bool, string> CreateToken(mToken m)
        {
            m.CheckAndThrowNullArgumentException();
            m.SupportedCoin.CheckAndThrowNullArgumentException();
            m.AllowedCountries.CheckAndThrowNullArgumentException();
            m.Code.CheckAndThrowNullArgumentException();
            m.ShortName.CheckAndThrowNullArgumentException();
            m.FullName.CheckAndThrowNullArgumentException();
            m.Tick.CheckAndThrowNullArgumentException();
            if (m.SupportedCoin.Count <= 0) m.SupportedCoin.ThrowInvalidOperationException($"No Supported Coin Provided for this Token");
            if (m.AllowedCountries.Count <= 0) m.SupportedCoin.ThrowInvalidOperationException($"No Counties provided tp allow for Such Token");
            bool isOk = true;
            m.SupportedCoin.ForEach(x =>
            {
                isOk = isOk && dbctx.SupportedToken.FirstOrDefault(z => z.SupportedTokenId == x.SupportedTokenId) != null;
            });
            if (!isOk) ThrowInvalidOperationException("Invalid Supported Token Provided");
            var t = m.ToEntity();
            t.AllowedCountries = t.AllowedCountries ?? new List<eSupportedCountry>();
            m.AllowedCountries.ForEach(x =>
            {
                t.AllowedCountries.Add(new eSupportedCountry { CountryId = x.Country.CountryId, SupportedSince = DateTime.UtcNow });
            });
            t.SupportedCoin = t.SupportedCoin ?? new List<eSupportedToken>();
            m.SupportedCoin.ForEach(x =>
            {
                t.SupportedCoin.Add(dbctx.SupportedToken.First(z => z.SupportedTokenId == x.SupportedTokenId));
            });
            t.RecordHash = "?";
            dbctx.Token.Add(t);
            dbctx.SaveChanges();
            t.SignRecord(this);
            return Tuple.Create(true, $" Token '{m.Code}' with id '{t.TokenId}' has been created.");

        }
        internal Tuple<bool, string> CreateSupportToken(mSupportedToken m)
        {
            m.CheckAndThrowNullArgumentException();
            m.RelatedNetwork.CheckAndThrowNullArgumentException();
            m.Code.CheckAndThrowNullArgumentException();
            m.ContractAddress.CheckAndThrowNullArgumentException();
            bool isOk = true;
            isOk = isOk && !dbctx.SupportedToken.Any(z => z.ContractAddress == m.ContractAddress);
            isOk = isOk && !dbctx.SupportedToken.Any(z => z.Code == m.Code);
            isOk = isOk && dbctx.SupportedNetwork.Any(z => z.SupportedNetworkId == m.RelatedNetwork.SupportedNetworkId);
            if (!isOk) ThrowInvalidOperationException("Invalid Details provided for Network Token");
            var t = m.ToEntity();
            t.RecordHash = "?";
            dbctx.SupportedToken.Add(t);
            dbctx.SaveChanges();
            t.SignRecord(this);
            return Tuple.Create(true, $"Support Token '{m.Code}' with Id '{t.SupportedTokenId}' has been created.");

        }
        internal Tuple<bool, string> ApproveStakingSlot(Guid GroupId)
        {
            var lst = GetStakePendingApproval(GroupId);
            if (lst.Count <= 0) return new Tuple<bool, string>(false, "No such Staking Slot to Approve..You may have missed the time window");

            foreach (var x in lst)
            {
                if (x.VerifyRecord(this))
                {
                    x.IsApproved = true;
                    x.ApprovedBy = GetSessionHash();
                    x.SignRecord(this);
                }
                else
                {
                    var str = $"Record Hash of {nameof(x)} with id{x.StakingOpportunityId} might have been compromised..further Process aborted..";
                    LogEvent(str);
                    throw new Exception(str);
                }
            }
            return new Tuple<bool, string>(true, "Staking Slot Approved..");
        }
        internal Tuple<bool, string> Job_RenewAllStakingsThatAreDueForToken(Guid TokenId, int pageSize = 5, int skip = 0, int HowManyPages = 1)
        {
            if ((pageSize * HowManyPages) > 10000) throw new ApplicationException("Job size is too big to be executed in time bound manner.Do not exceed 10,000/- each Job");

            var lst = dbctx.Staking.Include(x => x.StakingOpportunity).ThenInclude(x => x.Token).Where(x => x.ExpectedEndData.Date <= DateTime.UtcNow.Date
            && x.StakingOpportunity.TokenId == TokenId
            && x.IsRedeemed == false && x.AutoRenew == true)
                .Select(x => x.StakingId)
                .Skip(pageSize * skip).Take(pageSize * HowManyPages)
                .ToList();
            var jId = SrvStakingRenewal.AddPendingRenewal(lst);
            return new Tuple<bool, string>(true, $"{lst.Count} has been scheduled for Renewal with Job Id{jId}");
        }
        internal Tuple<bool, string> Job_ReFundAllStakingsThatAreDueForToken(Guid TokenId, int pageSize = 5, int skip = 0, int HowManyPages = 1)
        {
            if ((pageSize * HowManyPages) > 10000) throw new ApplicationException("Job size is too big to be executed in time bound manner.Do not exceed 10,000/- each Job");

            var lst = dbctx.Staking.Include(x => x.StakingOpportunity).ThenInclude(x => x.Token).Where(x => x.ExpectedEndData.Date <= DateTime.UtcNow.Date
            && x.StakingOpportunity.TokenId == TokenId
            && x.IsRedeemed == false && x.AutoRenew == false).Select(x => x.StakingId)
                .Skip(pageSize * skip).Take(pageSize * HowManyPages)
                .ToList();
            var jId = SrvStakingReFunds.AddPendingRefunds(lst);
            return new Tuple<bool, string>(true, $"{lst.Count} has been scheduled for Refunds with Job Id{jId}");
        }
        internal Tuple<bool, string> Job_RenewAllStakingsThatAreDue(int pageSize = 5, int skip = 0, int HowManyPages = 1)
        {
            if ((pageSize * HowManyPages) > 10000) throw new ApplicationException("Job size is too big to be executed in time bound manner.Do not exceed 10,000/- each Job");

            var lst = dbctx.Staking.Include(x => x.StakingOpportunity).ThenInclude(x => x.Token).Where(x => x.ExpectedEndData.Date <= DateTime.UtcNow.Date && x.IsRedeemed == false && x.AutoRenew == true).Select(x => x.StakingId)
                .Skip(pageSize * skip).Take(pageSize * HowManyPages)
                .ToList();
            var jId = SrvStakingRenewal.AddPendingRenewal(lst);
            return new Tuple<bool, string>(true, $"{lst.Count} has been scheduled for Renewal with Job Id{jId}");
        }
        internal Tuple<bool, string> Job_ReFundAllStakingsThatAreDue(int pageSize = 5, int skip = 0, int HowManyPages = 1)
        {
            if ((pageSize * HowManyPages) > 10000) throw new ApplicationException("Job size is too big to be executed in time bound manner.Do not exceed 10,000/- each Job");

            var lst = dbctx.Staking.Include(x => x.StakingOpportunity).ThenInclude(x => x.Token).Where(x => x.ExpectedEndData.Date <= DateTime.UtcNow.Date && x.IsRedeemed == false && x.AutoRenew == false).Select(x => x.StakingId)
                .Skip(pageSize * skip).Take(pageSize * HowManyPages)
                .ToList();
            var jId = SrvStakingReFunds.AddPendingRefunds(lst);
            return new Tuple<bool, string>(true, $"{lst.Count} has been scheduled for Refunds with Job Id{jId}");
        }
        internal bool CheckAndReFundStakings(Guid id)
        {

            var obj = dbctx.Staking.Include(x => x.StakingOpportunity).ThenInclude(x => x.Token).Include(x => x.StakingOpportunity)
                .ThenInclude(x => x.Token)
                .FirstOrDefault(x => x.StakingId == id && x.IsRedeemed == false);


            var wm = new WalletManager();
            wm.dbctx = dbctx;
            return wm.RedeemStake(obj)!=null;

        }
        internal bool CheckAndRenewStakings(Guid id)
        {
            var obj = dbctx.Staking.Include(x => x.StakingOpportunity).ThenInclude(x => x.Token).Include(x => x.StakingOpportunity).FirstOrDefault(x => x.StakingId == id && x.ExpectedEndData.Date <= DateTime.UtcNow.Date && x.IsRedeemed == false && x.AutoRenew == true);

            if (obj == null) return false;
            if (obj.VerifyRecord(this)) return false;
            if (obj.StakingOpportunity.OfferExpiredOn.HasValue && obj.StakingOpportunity.OfferExpiredOn.Value.Date <= DateTime.UtcNow.Date)
            {
                LogEvent($"Staking Offer for {obj.StakingId} doesn't Exist any more..can't Renew.");
                return false;
            }
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
        internal List<mStake> GetActiveStakings(int pageSize = 5, int skip = 0)
        {
            if (pageSize <= 0 || pageSize > 100) pageSize = 5;
            return dbctx.Staking.Include(x => x.StakingOpportunity).ThenInclude(x => x.Token).Where(x => x.StartedOn.Date <= DateTime.UtcNow.Date && x.IsRedeemed == false)
                .Skip(pageSize * skip).Take(pageSize)
                .ToList()
                .ToModel();
        }
        internal List<mStake> GetRenewalDueStakings(int pageSize = 5, int skip = 0)
        {
            if (pageSize <= 0 || pageSize > 100) pageSize = 5;
            return dbctx.Staking.Include(x => x.StakingOpportunity).ThenInclude(x => x.Token).Where(x => x.ExpectedEndData.Date <= DateTime.UtcNow.Date && x.IsRedeemed == false && x.AutoRenew == true)
                .Skip(pageSize * skip).Take(pageSize)
                .ToList()
                .ToModel();
        }
        internal List<mStake> GetRefundDueOrOverDueStakings(int pageSize = 5, int skip = 0)
        {
            if (pageSize <= 0 || pageSize > 100) pageSize = 5;
            return dbctx.Staking.Include(x => x.StakingOpportunity).ThenInclude(x => x.Token).Include(x => x.StakingOpportunity).ThenInclude(x => x.Token).Where(x => x.ExpectedEndData.Date <= DateTime.UtcNow.Date && x.IsRedeemed == false && x.AutoRenew == false)
                .Skip(pageSize * skip).Take(pageSize)
                .ToList()
                .ToModel();
        }

        internal List<mStakingSlot2> GetActiveStakingSlots(int pageSize = 0, int skip = 0)
        {
            var retval = new List<mStakingSlot>();
            if (pageSize <= 0)
            {
                var gr1 = dbctx.StakingOpportunity.Include(x => x.Token).Where(x => x.IsApproved && x.OfferStartedOn.Date <= DateTime.UtcNow.Date
                                && (!x.OfferShouldExpierOn.HasValue || x.OfferShouldExpierOn.Value > DateTime.UtcNow)
                                && (!x.OfferExpiredOn.HasValue)).OrderByDescending(x => x.CreatedOn).OrderBy(x => x.GroupId).ToList();
                return gr1.ToModel2();
            }
            else
            {
                if (pageSize <= 0 || pageSize > 100) pageSize = 50;

                var gr1 = dbctx.StakingOpportunity.Include(x => x.Token).Where(x => x.IsApproved && x.OfferStartedOn.Date <= DateTime.UtcNow.Date
                && (!x.OfferShouldExpierOn.HasValue || x.OfferShouldExpierOn.Value > DateTime.UtcNow)
                && (!x.OfferExpiredOn.HasValue)).OrderByDescending(x => x.CreatedOn).OrderBy(x => x.GroupId)
                    .Skip(pageSize * skip).Take(pageSize).ToList().ToModel2();
                return gr1;
            }
            //var gr = gr1.GroupBy(x => x.GroupId).ToList();
            //gr.ForEach(x =>
            //{
            //    retval.Add(x.ToList().ToModel());
            //});
            //return retval;

        }
        internal List<mStakingSlot2> GetStakingSlots(Guid GroupId)
        {
            return dbctx.StakingOpportunity.Where(x => x.GroupId == GroupId).ToList().ToModel2();

        }
        internal List<mStakingSlot2> GetActiveStakingSlotsOf(string tCode)
        {
            return dbctx.StakingOpportunity.Include(x=>x.Token).Where(x => x.Token.Code.ToLower()== tCode.ToLower()).ToList().ToModel2();

        }
        //For slots that can yet be approved
        internal List<mStakingSlot2> GetPendingApprovalForStakingSlots(int pageSize = 5, int skip = 0)
        {
            var retval = new List<mStakingSlot>();

            if (pageSize <= 0 || pageSize > 100) pageSize = 5;
            var gr1 = dbctx.StakingOpportunity.Where(x => x.IsApproved == false && x.OfferStartedOn >= DateTime.UtcNow).OrderByDescending(x => x.CreatedOn).OrderBy(x => x.GroupId)
                .Skip(pageSize * skip).Take(pageSize).ToList().ToModel2();
            return gr1;
            //var gr = gr1.GroupBy(x => x.GroupId).ToList();
            //gr.ForEach(x =>
            //{
            //    retval.Add(x.ToList().ToModel());
            //});
            //return retval;
        }
        //For historic reasons
        internal List<mStakingSlot2> GetDraftStakingSlot(int pageSize = 5, int skip = 0)
        {
            var retval = new List<mStakingSlot>();
            if (pageSize <= 0 || pageSize > 100) pageSize = 5;

            var gr1 = dbctx.StakingOpportunity.Where(x => x.IsApproved == false).OrderByDescending(x => x.CreatedOn).OrderBy(x => x.GroupId)
                .Skip(pageSize * skip).Take(pageSize).ToList().ToModel2();
            return gr1;
            //var gr = gr1.GroupBy(x => x.GroupId).ToList();
            //gr.ForEach(x =>
            //{
            //    retval.Add(x.ToList().ToModel());
            //});
            //return retval;
        }
        internal Tuple<bool, string> CreateStakingSlot(mStakingSlot m)
        {
            /* 1. One Coin should have one Staking Arrangement
             * 2. Minimum Staking duration should be 7 or more days
             * 3. ToDo: Naveen, Multi User Approval workflow should be followedprocess
             */
            if (m is null) throw new ArgumentNullException(nameof(m));
            if (m.TokenId == Guid.Empty) new ArgumentNullException("Token should be selected for Skating");

            if (GetExistingValidOppFor(m.TokenId).Count > 0) throw new ArgumentException("Already have Staking Options for this Token");
            if (m.Instances.Any(x => x.Duration < 7)) throw new ArgumentException("one or more slot(s) are having less than 7 days duration");
            if (m.Instances.Any(x => x.MinAmount <= 0)) throw new ArgumentException("one or more slot(s) are having '0' Minimum commitment");
            if (m.Instances.Any(x => x.MaxAmount <= 0)) throw new ArgumentException("one or more slot(s) are having '0' Maximum commitment");
            //Some benefits would be apart of AYP i.e. NavC
            // if (m.Instances.Any(x => x.AYPOffered <= 0)) throw new ArgumentException("one or more slot(s) are having '0' AYP on offer");
            if (m.TotalTarget <= 0) throw new ArgumentException("total target must be over '0'");
            if (m.GroupDetails.IsNullorEmpty()) throw new ArgumentException("'Group Details' must be provided");
            if (m.GroupName.IsNullorEmpty()) throw new ArgumentException("'Group Name' must be provided");
            if (m.Instances.Any(x => x.Name.IsNullorEmpty())) throw new ArgumentException("one or more slot(s) are missing 'Name'");
            if (m.Instances.Any(x => x.Details.IsNullorEmpty())) throw new ArgumentException("one or more slot(s) are missing 'Details'");
            if (m.OfferStartedOn <= DateTime.UtcNow) throw new ArgumentException("Offer can't start in Past");
            if (m.OfferStartedOn >= DateTime.UtcNow.AddDays(30)) throw new ArgumentException("Offer can't be scheduled for more than 30 days in future");
            if (m.OfferStartedOn.AddDays(1) <= m.OfferShouldExpierOn) throw new ArgumentException("Offer can't be provisioned for less than 1 day");
            if (m.Instances.Any(x => x.AYPOffered < 1)) throw new ArgumentException("AYP can't be less than 100%. Amount received must be returned.");

            m.GroupId = Guid.NewGuid();
            var lst = new List<eStakingOpportunity>();
            foreach (var o in m.Instances)
            {
                lst.Add(new eStakingOpportunity()
                {
                    TokenId = m.TokenId,
                    GroupName = m.GroupName,
                    GroupDetails = m.GroupDetails,
                    GroupId = m.GroupId,
                    Name = o.Name,
                    Details = o.Details,
                    MinAmount = o.MinAmount,
                    MaxAmount = o.MaxAmount,
                    AYPOffered = o.AYPOffered,
                    AutoRenewAllowed = o.AutoRenewAllowed,
                    Duration = o.Duration,
                    IsHardFixed = o.IsHardFixed,
                    TotalTarget = m.TotalTarget,
                    IsSunSet = m.IsSunSet,
                    OfferExpiredOn = m.OfferExpiredOn,
                    OfferShouldExpierOn = m.OfferShouldExpierOn,
                    OfferStartedOn = m.OfferStartedOn,
                    RecordHash = "?"
                });
            }
            dbctx.StakingOpportunity.AddRange(lst);
            dbctx.SaveChanges();
            lst.ForEach(x => x.SignRecord(this));
            return new Tuple<bool, string>(true, "Staking Opportunity Created!.Pending Approval");
        }

        #region Settlement Services Instances
        internal List<MarketInstances> GetAllMarketSettlementServices()
        {
            return SrvMarketProxy.GetMarketInstances();
        }
        internal Tuple<bool, string> SWITCHSettlementInstance(string mCode, Guid Current, Guid Proposed)
        {
            var str = $"No Such Market:{mCode} is Running";
            var lst = SrvMarketProxy.GetMarketSettlementInstancesOf(mCode);
            if (lst is null || lst.Count == 0) return Ok(false, str);
            bool go = null != lst.FirstOrDefault(x => x.InstanceId == Current && x.Role == SettlementSrvRole.Active);
            if (!go) return Ok(false, "Incorrect Current Instance Id");
            go = null != lst.FirstOrDefault(x => x.InstanceId == Proposed && x.Role == SettlementSrvRole.Passive);
            if (!go) return Ok(false, "Incorrect Proposed Instance Id");
            go = SrvMarketProxy.SWITCHSettlementInstance(mCode, Current, Proposed);
            if (go)
                return Ok(go, "SWITCH Instruction Issues Successfully");
            return Ok(go, "SWITCH Instruction failed to be Broadcasted..");
        }
        internal bool SuspendMarket(Guid Id)
        {
            var m = dbctx.Market.FirstOrDefault(x => x.MarketId == Id);
            m.CheckAndThrowNullArgumentException($"Count:{dbctx.Market.Count()} | {Id} | Invalid market Id provided");
            m.IsTradingAllowed = false;
            SrvMarketProxy.STOPMarketInstance(m.CodeName);
            dbctx.SaveChanges();

            return true;
        }
        internal bool RevokeSuspensionMarket(Guid Id)
        {
            var m = dbctx.Market.FirstOrDefault(x => x.MarketId == Id);
            m.CheckAndThrowNullArgumentException($"Count:{dbctx.Market.Count()} | {Id} | Invalid market Id provided");
            if (m.IsTradingAllowed) ThrowInvalidOperationException($"{m.CodeName} is not suspended. Operation Aborted");
            m.IsTradingAllowed = true;
            SrvMarketProxy.RestoreSUSPENDEDMarket(m.CodeName);
            dbctx.SaveChanges();

            return true;
        }
        #endregion

        private List<eStakingOpportunity> GetExistingValidOppFor(Guid tokenId)
        {
            if (tokenId == Guid.Empty) throw new ArgumentNullException(nameof(tokenId));
            var lst = dbctx.StakingOpportunity.Where(x => x.TokenId == tokenId && x.IsApproved && ((x.OfferShouldExpierOn.HasValue && x.OfferShouldExpierOn.Value <= DateTime.UtcNow) || (x.OfferExpiredOn.HasValue && x.OfferExpiredOn.Value <= DateTime.UtcNow))).ToList();
            return lst;
        }
        private List<eStakingOpportunity> GetStakePendingApproval(Guid GroupId)
        {
            var retval = new List<eStakingOpportunity>();
            if (GroupId == Guid.Empty) return retval;
            var lst = dbctx.StakingOpportunity.Where(x => x.GroupId == GroupId && x.IsApproved == false && x.OfferStartedOn >= DateTime.UtcNow).ToList();
            return lst;
        }

    }

}
