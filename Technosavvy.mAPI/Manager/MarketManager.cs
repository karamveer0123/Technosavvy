using NavExM.Int.Maintenance.APIs.Data.Entity.KYC;
using NavExM.Int.Maintenance.APIs.ServerModel;
using NavExM.Int.Maintenance.APIs.Services;
namespace NavExM.Int.Maintenance.APIs.Manager
{
    internal class MarketMakingOrderManager : ManagerBase { }
    internal class MarketManager : ManagerBase
    {
        internal Tuple<bool, string> CreateMarket(mMarket m)
        {
            if (dbctx.Market.Any(x => x.MarketId == m.MarketId))
                m.ThrowInvalidOperationException("Existing market can't be recreated");
            var e = m.ToEntity();
            e.MarketProfile.Clear();
            eMarket em;
            var eMP = m.MarketProfile.ToEntity();
            var gm = GetTokenManager();
            var fm = GetFiatManager();
            var CD = GetCDManager();
            // e.AllowedCountries = GetCountriesEntity(m.AllowedCountries);
            e.CodeName.CheckAndThrowNullArgumentException();
            e.ShortName.CheckAndThrowNullArgumentException();
            e.MinOrderSizeValueUSD.CheckAndThrowNullArgumentException();
            if (e.MinOrderSizeValueUSD <= 0)
                e.MinOrderSizeValueUSD.ThrowInvalidOperationException("Minimum Order size must be greater than zero");
            if (e.MinBaseOrderTick <= 0)//ToDo: Naveen, Should we have global Minimum Tick
                e.MinBaseOrderTick.ThrowInvalidOperationException("Minimum Order Tick is incorrect");
            switch (e.MarketType)
            {
                case eeMarketType.CryptoCrypto:
                    m.BaseTokenId.CheckAndThrowNullArgumentException();
                    m.QuoteTokenId.CheckAndThrowNullArgumentException();
                    e.BaseToken = gm.GetSpecificToken(m.BaseTokenId!.Value);
                    e.QuoteToken = gm.GetSpecificToken(m.QuoteTokenId!.Value);
                    em = GetMarket(e.BaseTokenId!.Value, e.QuoteTokenId!.Value);
                    if (em != null) em.ThrowInvalidOperationException("Such Market Already Exist");
                    break;
                case eeMarketType.CryptoStable:
                    m.BaseTokenId.CheckAndThrowNullArgumentException();
                    m.QuoteTokenId.CheckAndThrowNullArgumentException();
                    e.BaseToken = gm.GetSpecificToken(m.BaseTokenId!.Value);
                    e.QuoteToken = gm.GetSpecificToken(m.QuoteTokenId!.Value);
                    em = GetMarket(e.BaseTokenId!.Value, e.QuoteTokenId!.Value);
                    if (em != null) em.ThrowInvalidOperationException("Such Market Already Exist");
                    break;
                case eeMarketType.StableStable:
                    m.BaseTokenId.CheckAndThrowNullArgumentException();
                    m.QuoteTokenId.CheckAndThrowNullArgumentException();
                    e.BaseToken = gm.GetSpecificToken(m.BaseTokenId!.Value);
                    e.QuoteToken = gm.GetSpecificToken(m.QuoteTokenId!.Value);
                    em = GetMarket(e.BaseTokenId!.Value, e.QuoteTokenId!.Value);
                    if (em != null) em.ThrowInvalidOperationException("Such Market Already Exist");
                    break;
                case eeMarketType.StableFiat:
                    m.BaseTokenId.CheckAndThrowNullArgumentException();
                    m.QuoteCurrencyId.CheckAndThrowNullArgumentException();
                    e.BaseToken = gm.GetSpecificToken(m.BaseTokenId!.Value);
                    e.QuoteCurrency = fm.GetSpecificFiatCurrencies(m.QuoteCurrencyId!.Value);
                    em = GetMarket(e.BaseTokenId!.Value, e.QuoteCurrencyId!.Value);
                    if (em != null) em.ThrowInvalidOperationException("Such Market Already Exist");
                    break;
                case eeMarketType.CryptoFiat:
                    m.BaseTokenId.CheckAndThrowNullArgumentException();
                    m.QuoteCurrencyId.CheckAndThrowNullArgumentException();
                    e.BaseToken = gm.GetSpecificToken(m.BaseTokenId!.Value);
                    e.QuoteCurrency = fm.GetSpecificFiatCurrencies(m.QuoteCurrencyId!.Value);
                    em = GetMarket(e.BaseTokenId!.Value, e.QuoteCurrencyId!.Value);
                    if (em != null) em.ThrowInvalidOperationException("Such Market Already Exist");
                    break;
                default:
                    break;
            }
            eMP.ForEach(x => Validate(x));
            e.MarketProfile = eMP;
            e.RecordHash = "?";
            dbctx.Market.Add(e);
            dbctx.SaveChanges();
            e.SignRecord(this);

            e.SignRecord(this);
            if (m.IsMarketMakingAccount)
                CreateMarketMakingAccount(e.CodeName);

            EnsureInternalProcessWallets(e);

            return Ok(true, "Exchange Market Created Sussessfully..");

        }
        internal bool EnsureInternalProcessWallets(eMarket e)
        {
            //Market Order Wallet
            var m = dbctx.InternalWallet.FirstOrDefault(x => x.BelongsTo.ToLower() == e.CodeName.ToLower() && x.WalletNature == eWalletNature.Global && x.WalletType == eInternalWalletType.Market);
           
            if (m == null)
            {
                dbctx.InternalWallet.Add(new eInternalWallet
                {
                    Name = $"{e.CodeName} Market Wallet for {e.CodeName}",
                    BelongsTo = e.CodeName,
                    WalletNature = eWalletNature.Global,
                    WalletType = eInternalWalletType.Market
                });
                dbctx.SaveChanges();

            }

            /* Each Market for Each Country Should have a Earning SWAP Wallet
             */
            //Market Global Wallet
            var g = dbctx.InternalWallet.FirstOrDefault(x => x.BelongsTo.ToLower() == e.CodeName.ToLower() && x.WalletNature == eWalletNature.Earnings && x.WalletType == eInternalWalletType.Swap && x.GlobalId == null);
            if (g == null)
            {
                g = new eInternalWallet { Name = "Global Operation Wallet", BelongsTo = e.CodeName, WalletNature = eWalletNature.Earnings, WalletType = eInternalWalletType.Swap };

                dbctx.InternalWallet.Add(g);
                dbctx.SaveChanges();
            }

            var lstExisting = dbctx.InternalWallet.Include(x => x.RelatedCountry).Where(x => x.BelongsTo.ToLower() == e.CodeName.ToLower() && x.WalletNature == eWalletNature.Earnings && x.WalletType == eInternalWalletType.Swap && x.GlobalId != null);

            List<eCountry> lst = new List<eCountry>();

            e.MarketProfile.ForEach(x =>
            {
                if (x != null && x.ProfileFor != null)
                {
                    x.ProfileFor.ForEach(z =>
                    {
                        lst.Add(z.Country);
                    });
                }
            });
            foreach (var ct in lst)
            {
                if (ct != null && !lstExisting.Any(x => x.RelatedCountry.CountryId == ct.CountryId))
                {
                    dbctx.InternalWallet.Add(new eInternalWallet
                    {
                        Name = $"{e.CodeName} Wallet for {ct.Name}",
                        BelongsTo = e.CodeName,
                        GlobalId = g.InternalWalletId,
                        RelatedCountryId = ct.CountryId,
                        WalletNature = eWalletNature.Earnings,
                        WalletType = eInternalWalletType.Swap
                    });
                }
            }

            return dbctx.SaveChanges() > 0;

        }

        internal Tuple<bool, string> ValidateAndCreateMarketMakingAccount(string mCode)
        {
            var um = new UserManager() { dbctx = dbctx };
            if (!um.IsAny(GetMarketMakingAccountName(mCode)))
            {
                return Ok(CreateMarketMakingAccount(mCode), "Processed with Result..");
            }
            return Ok(false, $"Account already Exist for this Market:{mCode}");
        }
        internal bool CreateMarketMakingAccount(string mCode)
        {
            var um = new UserManager() { dbctx = dbctx };

            var email = GetMarketMakingAccountName(mCode);
            var ua = um.AddUserAccount(email);
            um.UpdatePasswordOfUserAccount(ua, RandomPassword(), null, UserType.MarketMaking, mCode);

            Console2.WriteLine_White($"{T}|{mCode} Market Making Account Created");
            return true;
        }
        internal string GetMarketMakingAccountName(string mCode)
        {
            var email = $"{mCode}@{mAppConstant.MMEmailDomain}";
            return email;
        }
        internal mUserSession GetMarketMakingSession(string mCode)
        {
            var um = new UserManager() { dbctx = dbctx };
            var email = $"{mCode}@{mAppConstant.MMEmailDomain}";
            var user = um.GetUser(email);
            var au = um.BuildPassAuthEvent(user);
            var sess = um.RequestSession(au.mAuthId, email);
            return sess;
        }
        internal List<mMarket> GetAllPendingApprovelMarketPair()
        {
            var ret = dbctx.Market
                 .Include(x => x.QuoteCurrency)
                 .Include(x => x.QuoteToken)
                 .Include(x => x.BaseToken)
                 .Include(x => x.Attributes)
                 .Include(x => x.MarketProfile)
                 .ThenInclude(x => x.ProfileFor)
                 .ThenInclude(x => x.Country)
                 .Where(x => x.IsApproved == false).ToList().ToModel();
            var MPs = ret.SelectMany(x => x.MarketProfile).ToList();//

            MPs = LoadDeep(MPs);

            return ret;
        }
        /// <summary>
        /// Return a List of Currently Running Markets from Market Proxi
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        internal List<string> GetLiveMarketPair(int pageSize = 20, int skip = 0)
        {
            if (pageSize <= 0 || skip < 0)
            {
                pageSize = 20;
                skip = 0;
            }
            return SrvMarketProxy.GetLiveMarkets().Skip(skip).Take(pageSize).ToList();
        }
        internal List<mMarket> GetAllActiveMarketPair(int pageSize = 20, int skip = 0)
        {
            if (pageSize <= 0 || skip < 0)
            {
                pageSize = 20;
                skip = 0;
            }
            skip = skip * pageSize;
            var ret = dbctx.Market
                 .Include(x => x.QuoteCurrency)
                 .Include(x => x.QuoteToken)
                 .Include(x => x.BaseToken)
                 .Include(x => x.Attributes)
                 .Include(x => x.MarketProfile)
                 .ThenInclude(x => x.ProfileFor)
                 .ThenInclude(x => x.Country)
                 .Where(x => x.IsApproved && x.IsTradingAllowed).Skip(skip).Take(pageSize).ToList().ToModel();
            var MPs = ret.SelectMany(x => x.MarketProfile).ToList();//

            MPs = LoadDeep(MPs);

            return ret;
        }
        internal List<mMarket> GetAllActiveMarketPairOfQuote(string qCode, bool Paging = true, int pageSize = 20, int skip = 0)
        {
            if (pageSize <= 0 || skip < 0)
            {
                pageSize = 20;
                skip = 0;
            }
            skip = skip * pageSize;
            List<mMarket> ret = new List<mMarket>();
            if (Paging)
            {
                var o = dbctx.Market
                                 .Include(x => x.QuoteCurrency)
                                 .ThenInclude(x => x.Profiles)
                                 .Include(x => x.QuoteToken)
                                 .Include(x => x.BaseToken)
                 .Include(x => x.Attributes)
                                 .Include(x => x.MarketProfile)
                                 .ThenInclude(x => x.ProfileFor)
                                 .ThenInclude(x => x.Country)
                                 .Where(x => x.IsApproved && x.IsTradingAllowed)
                                 .ToList().Where(x => x.QuoteToken.Code == qCode)
                                 .Skip(skip).Take(pageSize).ToList();
                ret = o.ToModel();
            }
            else
            {

                var o = dbctx.Market
                 .Include(x => x.BaseToken)
                 .Include(x => x.QuoteCurrency)
                 .Include(x => x.QuoteToken)
                 .Include(x => x.Attributes)
                 .Include(x => x.MarketProfile)
                 .ThenInclude(x => x.ProfileFor)
                 .ThenInclude(x => x.Country)
                .Where(x => x.IsApproved && x.IsTradingAllowed).ToList().Where(x => x.QuoteToken != null && x.QuoteToken.Code == qCode).ToList();
                ret = o.ToList().ToModel();

            }
            var MPs = ret.SelectMany(x => x.MarketProfile).ToList();//

            MPs = LoadDeep(MPs);

            return ret;
        }
        internal List<mToken> GetAllActiveMarketQuoteToken(bool Paging = true, int pageSize = 20, int skip = 0)
        {
            if (pageSize <= 0 || skip < 0)
            {
                pageSize = 20;
                skip = 0;
            }
            skip = skip * pageSize;
            List<mToken> ret = new List<mToken>();
            if (Paging)
            {
                var t0 = dbctx.Market
                 .Include(x => x.QuoteToken)
                 .ThenInclude(x => x.AllowedCountries)
                 .ThenInclude(x => x.Country)
                 .Where(x => x.IsApproved && x.IsTradingAllowed && x.QuoteTokenId != Guid.Empty).ToList();
                var t1 = t0.Where(x => x.QuoteToken != null).Select(x => x.QuoteToken).Distinct().Skip(skip).Take(pageSize).ToList();
                ret = t1.Select(x => x.ToModel()).ToList();
            }
            else
            {
                var t0 = dbctx.Market
                                .Include(x => x.QuoteToken)
                                .ThenInclude(x => x.AllowedCountries)
                                .ThenInclude(x => x.Country)
                                .Where(x => x.IsApproved && x.IsTradingAllowed && x.QuoteTokenId != Guid.Empty).ToList();
                var t1 = t0.Where(x => x.QuoteToken != null).Select(x => x.QuoteToken).Distinct().ToList();
                ret = t1.Select(x => x.ToModel()).ToList();
            }


            return ret;
        }
        internal List<mMarket> GetAllActiveMarketPairOf(Guid countryId)
        {
            var ret = dbctx.Market
                 .Include(x => x.QuoteCurrency)
                 .Include(x => x.QuoteToken)
                 .Include(x => x.BaseToken)
                 .Include(x => x.Attributes)
                 .Include(x => x.MarketProfile)
                 .ThenInclude(x => x.ProfileFor)
                 .ThenInclude(x => x.Country)
                 .Where(x =>
                 x.MarketProfile.Any(z => z.ProfileFor.Any(a => a.CountryId == countryId))
                && x.IsApproved && x.IsTradingAllowed
                 ).ToList().ToModel();
            var MPs = ret.SelectMany(x => x.MarketProfile).ToList();//

            MPs = LoadDeep(MPs);

            return ret;
        }
        /// <summary>
        /// Return Active Market Pairs for the Provided Country
        /// </summary>
        /// <param name="Abbrivation">Country Abbrivation</param>
        /// <returns></returns>
        internal List<mMarket> GetAllActiveMarketPairOf(string Abbrivation)
        {
            var ret = dbctx.Market
                 .Include(x => x.QuoteCurrency)
                 .Include(x => x.QuoteToken)
                 .Include(x => x.BaseToken)
                 .Include(x => x.Attributes)
                 .Include(x => x.MarketProfile)
                 .ThenInclude(x => x.ProfileFor)
                 .ThenInclude(x => x.Country)
                 .Where(x =>
                 x.MarketProfile.Any(z => z.ProfileFor.Any(a => a.Country.Abbrivation == Abbrivation))
                && x.IsApproved && x.IsTradingAllowed
                 ).ToList().ToModel();
            var MPs = ret.SelectMany(x => x.MarketProfile).ToList();//

            MPs = LoadDeep(MPs);

            return ret;
        }
        private List<mMarketProfile> LoadDeep(List<mMarketProfile> MPs)
        {
            foreach (var x in MPs)
            {
                //Tax
                if (x.BaseTokenFeeTaxId.HasValue)
                    x._BaseTokenFeeTax = GetTax(x.BaseTokenFeeTaxId.Value);
                if (x.QuoteTokenFeeTaxId.HasValue)
                    x._QuoteTokenFeeTax = GetTax(x.QuoteTokenFeeTaxId.Value);
                if (x.QuoteTokenTradeingTaxId.HasValue)
                    x._QuoteTokenTradeingTax = GetTax(x.QuoteTokenTradeingTaxId.Value);
                if (x.BaseTokenTradeingTaxId.HasValue)
                    x._BaseTokenTradeingTax = GetTax(x.BaseTokenTradeingTaxId.Value);
                //Fee
                x._QuoteTokenMakerFee = GetTradingFee(x.QuoteTokenMakerFeeId);
                x._QuoteTokenTakerFee = GetTradingFee(x.QuoteTokenTakerFeeId);
                x._BaseTokenMakerFee = GetTradingFee(x.BaseTokenMakerFeeId);
                x._BaseTokenTakerFee = GetTradingFee(x.BaseTokenTakerFeeId);
            }
            return MPs;
        }
        internal mTradingFee GetTradingFee(Guid id)
        {
            var ret = dbctx.TradingFee.Where(x => x.TokenFeeId == id).FirstOrDefault();
            ret.CheckAndThrowNullArgumentException();
            return ret.ToModel();
        }
        internal List<mMarket> GetMarketPair(Guid Id)
        {
            return dbctx.Market
                 .Include(x => x.QuoteCurrency)
                 .Include(x => x.QuoteToken)
                 .Include(x => x.BaseToken)
                 .Include(x => x.Attributes)
                 .Include(x => x.MarketProfile)
                 .Where(x => x.MarketId == Id).ToList().ToModel();
        }
        internal mMarket? GetMarketPair(string mCode)
        {
            return dbctx.Market
                 .Include(x => x.QuoteCurrency)
                 .Include(x => x.QuoteToken)
                 .Include(x => x.BaseToken)
                 .Include(x => x.Attributes)
                 .Include(x => x.MarketProfile)
                 .ThenInclude(x => x.ProfileFor)
                 .ThenInclude(x => x.Country)
                 .FirstOrDefault(x => x.CodeName.ToLower() == mCode.ToLower())!.ToModel();
        }

        internal Tuple<bool, string> ApproveMarketPair(Guid id)
        {
            var ret = dbctx.Market
                .Include(x => x.QuoteCurrency)
                 .Include(x => x.QuoteToken)
                 .Include(x => x.BaseToken)
                 .Include(x => x.Attributes)
                 .Include(x => x.MarketProfile)
                 .FirstOrDefault(x => x.MarketId == id);
            if (ret == null) ThrowInvalidOperationException($"Invalid market Id provided");
            if (ret.IsApproved) ThrowInvalidOperationException("Market Already approved.");
            ret.VerifyRecord(this, true);
            ret.IsApproved = true;
            ret.ApprovedBy = GetSessionHash();
            ret.ApprovedOn = DateTime.UtcNow;
            ret.SignRecord(this);
            dbctx.SaveChanges();
            return Ok(true, "Exchange Token Approved Sussessfully..");
        }
        internal List<mMarket> GetMarketsOfToken(Guid id)
        {
            var ret = dbctx.Market
                .Include(x => x.QuoteCurrency)
                 .Include(x => x.QuoteToken)
                 .Include(x => x.BaseToken)
                 .Include(x => x.Attributes)
                 .Include(x => x.MarketProfile)
                .Where(x => x.BaseTokenId == id || x.QuoteTokenId == id || x.QuoteCurrencyId == id).ToList().ToModel();
            return ret;
        }
        internal bool AddMarketPair(mToken m)
        {
            throw new NotImplementedException($"{nameof(AddMarketPair)} is not implemented yet..");
        }
        private TokenManager GetTokenManager()
        {
            var tm = new TokenManager();
            tm.dbctx = dbctx;
            tm._http = _http;
            tm.httpContext = httpContext;
            return tm;
        }
        private CommonDataManager GetCDManager()
        {
            var tm = new CommonDataManager();
            tm.dbctx = dbctx;
            tm._http = _http;
            tm.httpContext = httpContext;
            return tm;
        }
        private FiatCurrencyManager GetFiatManager()
        {
            var tm = new FiatCurrencyManager();
            tm.dbctx = dbctx;
            tm._http = _http;
            tm.httpContext = httpContext;
            return tm;
        }
        private eMarket GetMarket(Guid TId1, Guid TId2)
        {
            var m = dbctx.Market.Where(x =>
            (x.BaseTokenId == TId1 || x.BaseTokenId == TId2) && x.DeletedOn.HasValue == false)
                .FirstOrDefault(x => (x.QuoteTokenId == TId1 || x.QuoteTokenId == TId2)
                && x.DeletedOn.HasValue == false);
            if (m != null) return m;

            m = dbctx.Market.Where(x =>
            (x.BaseTokenId == TId1 || x.BaseTokenId == TId2) && x.DeletedOn.HasValue == false)
                .FirstOrDefault(x =>
                (x.QuoteCurrencyId == TId1 || x.QuoteCurrencyId == TId2) && x.DeletedOn.HasValue == false);

            return m;
        }
        private void Validate(eMarketProfile e)
        {
            if (dbctx.MarketProfile.Any(x => x.MarketProfileId == e.MarketProfileId))
                e.ThrowInvalidOperationException("Existing market profile can't be reused");
            e.QuoteTokenMakerFeeId.CheckAndThrowNullArgumentException();
            dbctx.TradingFee.First(x => x.TokenFeeId == e.QuoteTokenMakerFeeId).CheckAndThrowNullArgumentException();

            e.QuoteTokenTakerFeeId.CheckAndThrowNullArgumentException();
            dbctx.TradingFee.First(x => x.TokenFeeId == e.QuoteTokenMakerFeeId).CheckAndThrowNullArgumentException();

            e.BaseTokenMakerFeeId.CheckAndThrowNullArgumentException();
            dbctx.TradingFee.First(x => x.TokenFeeId == e.BaseTokenMakerFeeId).CheckAndThrowNullArgumentException();

            e.BaseTokenTakerFeeId.CheckAndThrowNullArgumentException();
            dbctx.TradingFee.First(x => x.TokenFeeId == e.BaseTokenMakerFeeId).CheckAndThrowNullArgumentException();

            if (e.QuoteTokenFeeTaxId.HasValue && e.QuoteTokenFeeTaxId != Guid.Empty)
            {
                dbctx.Tax.First(x => x.TaxId == e.QuoteTokenFeeTaxId);
            }

            if (e.BaseTokenFeeTaxId.HasValue && e.BaseTokenFeeTaxId != Guid.Empty)
            {
                dbctx.Tax.First(x => x.TaxId == e.BaseTokenFeeTaxId);
            }
            if (e.BaseTokenTradeingTaxId.HasValue && e.BaseTokenTradeingTaxId != Guid.Empty)
            {
                dbctx.Tax.First(x => x.TaxId == e.BaseTokenTradeingTaxId);
            }
            if (e.QuoteTokenTradeingTaxId.HasValue && e.QuoteTokenTradeingTaxId != Guid.Empty)
            {
                dbctx.Tax.First(x => x.TaxId == e.QuoteTokenTradeingTaxId);
            }

            var lst = e.ProfileFor.Select(x => x.CountryId).ToList();
            var clst = dbctx.Country.ToList();
            e.ProfileFor.Clear();
            lst.ForEach(o =>
            {
                e.ProfileFor.Add(new eMarketProfileScope()
                {
                    Country = dbctx.Country.First(s => s.CountryId == o),
                    MarketProfile = e
                });
            });
            return;
        }
        private mTax GetTax(Guid id)
        {
            var ret = dbctx.Tax.Where(x => x.TaxId == id).FirstOrDefault();
            if (ret != null)
                return ret.ToModel();
            return null;
        }

        internal Tuple<bool, string> PlaceOrder(smOrder order)
        {

            //Done: Naveen, A build and Validated Order Should be Received from OrderManager
            //Done: Naveen, Perform a Transaction to take Quote Tokens from Fund Account.
            //ToDo: Naveen, Update Balance in BlockChain upon Trade Confirmation
            //Done: Naveen,Place a Valid Order into Market
            //
            try
            {
                var wobj = new smOrderPublishWrapper()
                {
                    SenderTick = DateTime.UtcNow.Ticks,
                    RelatedEvent = OrderEvent.PlaceOrder,
                    SenderAppId = AppConfigBase.RegistryToken!.AppId,
                    MachineName = Environment.MachineName,
                    Order = order
                };
                if (SrvMarketProxy.PlaceOrUpdateOrder(wobj))
                {
                    Console2.WriteLine_DarkYellow($"{wobj.Order.OrderSide} of {wobj.Order.BaseTokenCodeName} for {wobj.Order.Price} Queued.. at:{DateTime.UtcNow}");
                    return Ok(true, $"Order Queued..");
                }
                else
                {
                    LogError($"An Attempt was made to place an order in Market:{wobj.Order.MarketCode}, when No Such Active Market Exist");
                    return Ok(false, $"No Such Active Market..");
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                return Ok(false, ex.Message);
            }
        }
        internal Tuple<bool, string> CancelOrder(smOrder order)
        {
            //Done: Naveen, A build and Validated Order Should be Received from OrderManager
            //ToDo: Naveen, Update Balance in BlockChain upon Trade Confirmation
            //Done: Naveen,Place a Valid Order into Market
            //
            try
            {
                if (order == null)
                    return Ok(false, $"No Such Order Exist..");

                var wobj = new smOrderPublishWrapper()
                {
                    SenderTick = DateTime.UtcNow.Ticks,
                    RelatedEvent = OrderEvent.CancelOrder,
                    SenderAppId = AppConfigBase.RegistryToken!.AppId,
                    MachineName = Environment.MachineName,
                    Order = order
                };
                SrvMarketProxy.PlaceOrUpdateOrder(wobj);
                return Ok(true, $"Order Queued..");
            }
            catch (Exception ex)
            {
                LogError(ex);
                return Ok(false, ex.Message);
            }
        }
//#if (DEBUG)

//        internal Tuple<bool, string> PlaceTestBuyOrder(DateTime dt, string bCode = "", string QCode = "", string mCode = "")
//        {
//            var wobj = new smOrderPublishWrapper();
//            wobj.SenderTick = DateTime.UtcNow.Ticks;
//            wobj.RelatedEvent = OrderEvent.PlaceOrder;
//            wobj.SenderAppId = (AppConfigBase.RegistryToken is null ? "d" : AppConfigBase.RegistryToken.AppId);
//            wobj.MachineName = Environment.MachineName;
//            if (bCode.IsNOT_NullorEmpty() && QCode.IsNOT_NullorEmpty() && mCode.IsNullorEmpty())
//                mCode = $"{bCode}{QCode}";
//            double v = 0;
//            while (v <= 0)
//            {
//                v = new Random().NextDouble();
//            }
//            wobj.Order = new smOrder
//            {
//                BaseTokenId = Guid.NewGuid(),
//                OrderType = ServerModel.eOrderType.Limit,
//                AuthId = Guid.NewGuid().ToString(),
//                SessionId = "ABC Test",
//                OrderID = $"100525898.1.NavCUSDT.{DateTime.UtcNow.Ticks}-{new Random().Next(101, 999)}",
//                InternalOrderID = Guid.NewGuid(),
//                PlacedOn = dt,//DateTime.UtcNow,
//                CreatedOn = DateTime.UtcNow,
//                OriginalVolume = v,
//                CurrentVolume = v,
//                Price = new Random().Next(16350, 16450),
//                OrderSide = ServerModel.eOrderSide.Buy,
//                Status = ServerModel.eOrderStatus.Placed,
//                MarketCode = mCode,
//                BaseTokenCodeName = bCode,
//                QuoteTokenCodeName = QCode
//            };
//            if (wobj.Order.MarketCode == string.Empty)
//            {
//                var mkLst = SrvMarketProxy.GetLiveMarkets();
//                if (mkLst.Count > 0)
//                    wobj.Order.MarketCode = mkLst.First();
//            }
//            if (wobj.Order.BaseTokenCodeName.IsNullorEmpty())
//                wobj.Order.BaseTokenCodeName = "NavC";
//            if (wobj.Order.QuoteTokenCodeName.IsNullorEmpty())
//                wobj.Order.QuoteTokenCodeName = "USDT";
//            if (wobj.Order.MarketCode.IsNullorEmpty())
//                wobj.Order.MarketCode = "NavCUSDT";

//            wobj.Order.SwapRate = GetmyTradingSwap(wobj.Order.MarketCode);
//            wobj.Order.CBThreashold = GetCBthreashold(wobj.Order.MarketCode);
//            SrvMarketProxy.PlaceOrUpdateOrder(wobj);
//            return Ok(true, $"Test Order Buy for Qty {v} @{wobj.Order.Price} Placed..");
//        }
//        internal Tuple<bool, string> PlaceTestSellOrder(DateTime dt, string bCode = "", string QCode = "", string mCode = "")
//        {
//            var wobj = new smOrderPublishWrapper();
//            wobj.SenderTick = DateTime.UtcNow.Ticks;
//            wobj.RelatedEvent = OrderEvent.PlaceOrder;
//            wobj.SenderAppId = (AppConfigBase.RegistryToken is null ? "d" : AppConfigBase.RegistryToken.AppId);
//            wobj.MachineName = Environment.MachineName;
//            if (bCode.IsNOT_NullorEmpty() && QCode.IsNOT_NullorEmpty() && mCode.IsNullorEmpty())
//                mCode = $"{bCode}{QCode}";
//            double v = 0;
//            while (v <= 0)
//            {
//                v = new Random().NextDouble();
//            }
//            wobj.Order = new smOrder
//            {
//                BaseTokenId = Guid.NewGuid(),
//                AuthId = Guid.NewGuid().ToString(),
//                OrderType = ServerModel.eOrderType.Limit,
//                SessionId = "ABC Test",
//                OrderID = $"100525898.1.BTCUSDT.{DateTime.UtcNow.Ticks}-{new Random().Next(101, 999)}",
//                InternalOrderID = Guid.NewGuid(),
//                PlacedOn = dt,//DateTime.UtcNow,
//                CreatedOn = DateTime.UtcNow,
//                OriginalVolume = v,
//                CurrentVolume = v,
//                Price = new Random().Next(16350, 16450),
//                OrderSide = ServerModel.eOrderSide.Sell,
//                Status = ServerModel.eOrderStatus.Placed,
//                MarketCode = mCode,
//                BaseTokenCodeName = bCode,
//                QuoteTokenCodeName = QCode
//            };
//            if (wobj.Order.MarketCode == string.Empty)
//            {
//                wobj.Order.MarketCode = SrvMarketProxy.GetLiveMarkets().First();
//            }
//            wobj.Order.BaseTokenCodeName ??= "BTC";
//            wobj.Order.QuoteTokenCodeName ??= "USDT";
//            wobj.Order.MarketCode ??= "BTCUSDT";
//            wobj.Order.SwapRate = GetmyTradingSwap(wobj.Order.MarketCode);
//            wobj.Order.CBThreashold = GetCBthreashold(wobj.Order.MarketCode);
//            SrvMarketProxy.PlaceOrUpdateOrder(wobj);
//            return Ok(true, $"Test Order Sell for Qty {v} @{wobj.Order.Price} Placed..");
//        }
//        internal double GetCBthreashold(string mCode)
//        {
//            //As per POLICY we will pay upto 0.05% of Value Addition
//            //This Information Should be Read From Database
//            //ToDo: Naveen, CashBackThreashold Varivable should be defined in Database
//            return 0.0005;
//        }
//        internal double GetmyTradingSwap(string mCode)
//        {
//            return 0.0002;//0.02%
//            /* 0. Based on User Profile Country, We should Fetch the Market Profile for Such Country
//             * 1. Based on User, staking status, we should establish his Community status
//             * 2. We should get the related Fee Percentage from the Market Profile
//             */
//            //ToDo: Naveen Use UM to Fetch User Session Status, Country Profile
//            //var mkt = GetMarketPair(mCode);
//            //if (mkt != null && mkt.MarketProfile.Count > 0)
//            //{
//            //    mkt.MarketProfile[0]._BaseTokenMakerFee.FeeCommunity;
//            //}
//        }
//#endif
        static string RandomPassword()
        {


            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string number = "0123456789";
            const string sp = "!@$";
            var s1 = new string(Enumerable.Repeat(chars, 7).Select(s => s[random.Next(s.Length)]).ToArray());
            var s2 = new string(Enumerable.Repeat(number, 2).Select(s => s[random.Next(s.Length)]).ToArray());
            var s3 = new string(Enumerable.Repeat(sp, 1).Select(s => s[random.Next(s.Length)]).ToArray());
            return s1 + s3 + s2;
        }
    }

}
