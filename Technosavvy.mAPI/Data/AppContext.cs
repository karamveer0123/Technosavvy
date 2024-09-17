using NavExM.Int.Maintenance.APIs.Data.Entity.Contents;
using NavExM.Int.Maintenance.APIs.Data.Entity.Fund;
using NavExM.Int.Maintenance.APIs.Data.Entity.KYC;
using NavExM.Int.Maintenance.APIs.Data.Entity.PreBeta;
using NavExM.Int.Maintenance.APIs.Model;
using NavExM.Int.Maintenance.APIs.ServerModel;

namespace NavExM.Int.Maintenance.APIs.Data
{
    public class CareerAppContext : DbContext
    {
        public DbSet<JD> Jds { get; set; }
        public DbSet<JDViewers> JdViewers { get; set; }
        public CareerAppContext(DbContextOptions<CareerAppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

    }
    public class ContentAppContext : DbContext
    {
        public DbSet<FAQ> FAQs { get; set; }
        public ContentAppContext(DbContextOptions<ContentAppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //RowVersion
            modelBuilder.Entity<FAQ>(x => x.Property(p => p.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate());
        }
    }
    public class PreBetaDBContext : DbContext
    {
        public DbSet<ePreBetaStage> PreBetaStage { get; set; }
        public DbSet<eFractionFactor> FractionFactor { get; set; }
        public DbSet<ePBMyPurchaseRecords> PurchaseRecords { get; set; }

        public PreBetaDBContext(DbContextOptions<PreBetaDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //RowVersion
            modelBuilder.Entity<ePreBetaStage>(x => x.Property(p => p.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate());
            //modelBuilder.Entity<eFractionFactor>(x => x.Property(p => p.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate());
        }
    }
    public static class PBDbInitializer
    {
        static PreBetaDBContext dbctx = null;
        public static void Initialize(PreBetaDBContext ctx)
        {
            //if (!ctx.PreBetaStage.Any())
            {
                InitializeStages(ctx);
            }
            if (!ctx.FractionFactor.Any())
            {
                InitializeFactors(ctx);
            }
        }
        private static void InitializeFactors(PreBetaDBContext ctx)
        {
            dbctx = ctx;
            ctx.FractionFactor.AddRange(Loadfactors());
            dbctx.SaveChanges();
        }
        private static void InitializeStages(PreBetaDBContext ctx)
        {
            dbctx = ctx;
            ctx.PreBetaStage.Where(x => x.id != null).ExecuteDelete();
            ctx.PreBetaStage.AddRange(LoadStages());
            dbctx.SaveChanges();
        }
        private static eFractionFactor[] Loadfactors()
        {

            return new eFractionFactor[] {
                new eFractionFactor(){ keyName="24HrsUsers", FractionFactor=10},
                new eFractionFactor(){ keyName="24HrsTokens", FractionFactor=16},
                new eFractionFactor(){ keyName="TotalUsers", FractionFactor=21.3},
                new eFractionFactor(){ keyName="TotalTokens", FractionFactor=17},
            };
        }
        private static ePreBetaStage[] LoadStages()
        {
            return new ePreBetaStage[]
            {
                new ePreBetaStage(){ StageName="Stage-1", NavCSellPrice=1.15, StartDate=DateTime.Parse("25-sep-2023")},
                 new ePreBetaStage(){ StageName="Stage-2", NavCSellPrice=1.30, StartDate=DateTime.Parse("15-oct-2023")},
                  new ePreBetaStage(){ StageName="Stage-3", NavCSellPrice=1.45, StartDate=DateTime.Parse("04-Nov-2023"), EndDate=DateTime.Parse("24-Nov-2023"),TokenCap=26000000}
            };
        }
    }

    public static class ContentDbInitializer
    {
        static ContentAppContext dbctx = null;
        public static void Initialize(ContentAppContext ctx)
        {
            if (!ctx.FAQs.Any())
            {
                InitializeFAQ(ctx);
            }
        }
        private static void InitializeFAQ(ContentAppContext ctx)
        {
            dbctx = ctx;
            ctx.FAQs.AddRange(LoadFAQs());
            dbctx.SaveChanges();
        }
        private static FAQ[] LoadFAQs()
        {
            return new FAQ[]
            {
                new FAQ{  GroupTitle="A. About NavExM",QuestionText=" What is NavExM?",AnswerText="<p>NavExM, The Next Generation Centralized Crypto Currency Exchange. This future cryptocurrency exchange offers zero transaction fees and rewards to all its community members in the form of cashback benefits for trading in the exchange. NavExM provides a seamless trading experience to the traders trading on the exchange with a smooth interface and next-generation features. </p><p> Traders in NavExM who are eligible for NavExM community members, can get up to 0.05% of the trading volume as cashback after each 150-hour continuous cycle. Which will increase to 0.10% as assured cashback for Premium stackers. This is the unique feature of NavExM, which gives additional returns to all its participants.  </p><p>Initially NavExM will start with Spot trading and gradually it will increase other segments also. </p>", Status= eAuthStatus.Approved, OrderNo=0, },
               new FAQ{  GroupTitle="A. About NavExM",QuestionText="  What is NavC?",AnswerText="<p>NavC is the native utility token of NavExM built on Ethereum chain (ERC-20). It acts as the underlying currency of the exchange. All the trades on the exchange will route through the NavC token itself, creating the demand for the token leading to consistent price appreciation of this token. With the increase in the exchange trading volume NavC price growth will accelerate and benefit its holders to create wealth. </p>", Status= eAuthStatus.Approved, OrderNo=0, },
               new FAQ{  GroupTitle="A. About NavExM",QuestionText=" Does NavExM have a referral program?",AnswerText="<p>Yes, NavExM has a referral program. Once you sign up, you will get a referral link, you can share it with your community to get the referral rewards. When a user registers via your Referral Link and becomes an active trader by making a transaction of at least $10 (USD) within a period of 30 days from the date of registration then both the users i.e., Referrer and Referee will get NavC Tokens worth $1 (USD) in their wallets. </p>\r\n                      <p> A user may send Referral Link as many times as they like. You can share your Referral Link via Email, WhatsApp, SMS, or any other relevant social media platform. It is important that the link on NavExM is used and that the Referred User clicks on the Referral Link. The Referral Links have a special code at the end enabling us to link your account with the Referred User. When someone clicks your Referral Link, a link between your account and the Referred User shall be stored with the help of cookies. The cookie will store the data for 30 days during which the Referred User can either use the provided registration form in the Referral Link or can register directly on the NavExM website. </p>", Status= eAuthStatus.Approved, OrderNo=0, },
               new FAQ{  GroupTitle="A. About NavExM",QuestionText=" Does NavExM have a referral program?",AnswerText="<p>Yes, NavExM has a referral program. Once you sign up, you will get a referral link, you can share it with your community to get the referral rewards. When a user registers via your Referral Link and becomes an active trader by making a transaction of at least $10 (USD) within a period of 30 days from the date of registration then both the users i.e., Referrer and Referee will get NavC Tokens worth $1 (USD) in their wallets. </p>\r\n                      <p> A user may send Referral Link as many times as they like. You can share your Referral Link via Email, WhatsApp, SMS, or any other relevant social media platform. It is important that the link on NavExM is used and that the Referred User clicks on the Referral Link. The Referral Links have a special code at the end enabling us to link your account with the Referred User. When someone clicks your Referral Link, a link between your account and the Referred User shall be stored with the help of cookies. The cookie will store the data for 30 days during which the Referred User can either use the provided registration form in the Referral Link or can register directly on the NavExM website. </p>", Status= eAuthStatus.Approved, OrderNo=0, }
            };
        }
    }
    public class MMOrderAppContext : DbContext
    {
        public DbSet<smOrder> Orders { get; set; }
        public DbSet<OrderAck> OrderAck { get; set; }
        public DbSet<eProcessedOrder> ProcessedOrder { get; set; }

        public DbSet<smIterationOrders> IterationOrders { get; set; }
        public MMOrderAppContext(DbContextOptions<MMOrderAppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //UserAccount as Primary
            //RowVersion
            modelBuilder.Entity<smOrder>(x => x.Property(p => p.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate());
            modelBuilder.Entity<smIterationOrders>(x => x.Property(p => p.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate());
            modelBuilder.Entity<eProcessedOrder>(x => x.Property(p => p.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate());
        }
    }
    public class OrderAppContext : DbContext
    {
        public DbSet<smOrder> Orders { get; set; }
        public DbSet<OrderAck> OrderAck { get; set; }
        public DbSet<eProcessedOrder> ProcessedOrder { get; set; }
        public DbSet<smTrade> Trades { get; set; }
        public DbSet<TradeIssues> TradeIssues { get; set; }
        public OrderAppContext(DbContextOptions<OrderAppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //UserAccount as Primary
            modelBuilder.Entity<smTrade>().HasAlternateKey(o => new { o.SellInternalId, o.BuyInternalId });
            //RowVersion
            modelBuilder.Entity<smOrder>(x => x.Property(p => p.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate());
            modelBuilder.Entity<eProcessedOrder>(x => x.Property(p => p.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate());
            modelBuilder.Entity<TradeIssues>(x => x.Property(p => p.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate());
        }
    }
    public class KYCAppContext : DbContext
    {
        public DbSet<eCategory> Category { get; set; }
        public DbSet<eDocumentTemplate> DocumentTemplate { get; set; }
        public DbSet<eKYCDocAdminRecord> DocumentInstance { get; set; }

        public KYCAppContext(DbContextOptions<KYCAppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ////UserAccount as Primary
            //modelBuilder.Entity<smTrade>().HasAlternateKey(o => new { o.SellInternalId, o.BuyInternalId });
            ////RowVersion
            //modelBuilder.Entity<smOrder>(x => x.Property(p => p.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate());
        }
    }
    public class ApiAppContext : DbContext
    {
        //  internal static Dictionary<string, string> AllItems = new Dictionary<string, string>();
        public DbSet<eAuthorizedEmail> AuthorizedEmail { get; set; }
        public DbSet<eAuthenticator> Authenticator { get; set; }
        public DbSet<eCountry> Country { get; set; }
        public DbSet<eGeoInfo> GeoInfo { get; set; }
        public DbSet<eMobile> Mobile { get; set; }
        public DbSet<eTaxResidency> TaxResidency { get; set; }
        public DbSet<eCitizenship> Citizens { get; set; }
        public DbSet<eUserAccount> UserAccount { get; set; }
        public DbSet<eSecurePassword> SecurePassword { get; set; }
        public DbSet<eUserAccountAuthenticationLog> UserAccountAuthenticationLog { get; set; }
        public DbSet<eEmailValidationAction> EmailValidationAction { get; set; }
        public DbSet<eAuthenticationEvent> AuthenticationEvent { get; set; }
        public DbSet<eProfile> Profile { get; set; }
        public DbSet<eUPIPaymentMethod> UPI { get; set; }
        public DbSet<eBankDepositPaymentMethod> BankDeposit { get; set; }
        public DbSet<eAddress> Address { get; set; }
        public DbSet<eEnumBoxData> EnumBoxData { get; set; }
        public DbSet<eUserSession> UserSession { get; set; }
        //public DbSet<eKYCRecord> KYCRecord { get; set; }
        public DbSet<eKYCDocRecord> KYCDocRecords { get; set; }
        //public DbSet<eKYCDocuTemplate> KYCDocuTemplates { get; set; }
        public DbSet<eGlobalVariables> GlobalVariables { get; set; }
        public DbSet<eInternalWallet> InternalWallet { get; set; }
        public DbSet<eInternalWalletMapToExternal> InternalWalletMapToExternal { get; set; }
        public DbSet<eInternalWBalance> InternalWBalance { get; set; }
        public DbSet<eSpotWallet> SpotWallet { get; set; }
        public DbSet<eSpotWBalance> SpotWBalance { get; set; }
        public DbSet<eEarnWallet> EarnWallet { get; set; }
        public DbSet<eEarnWBalance> EarnWBalance { get; set; }
        public DbSet<eFundingWallet> FundingWallet { get; set; }
        public DbSet<eFundingWBalance> FundingWBalance { get; set; }
        public DbSet<eEscrowWallet> EscrowWallet { get; set; }
        public DbSet<eEscrowWBalance> EscrowWBalance { get; set; }
        public DbSet<eSupportedCountry> SupportedCountry { get; set; }
        public DbSet<eSupportedNetwork> SupportedNetwork { get; set; }
        public DbSet<eToken> Token { get; set; }
        public DbSet<eSupportedToken> SupportedToken { get; set; }
        public DbSet<eWalletTransaction> WalletTransaction { get; set; }
        public DbSet<eOnDemandTxCheckRequest> OnDemandTxCheckRequest { get; set; }
        public DbSet<eStakingOpportunity> StakingOpportunity { get; set; }
        public DbSet<eStaking> Staking { get; set; }
        public DbSet<eHoldingWallet> HoldingWallet { get; set; }
        public DbSet<eHoldingWBalance> HoldingWBalance { get; set; }
        public DbSet<eNetworkWalletAddress> NetworkWalletAddress { get; set; }
        public DbSet<eNetworkWalletBalance> NetworkWalletBalance { get; set; }
        public DbSet<eFundingNetworkWallet> FundingNetworkWallet { get; set; }
        public DbSet<eNetworkWalletAddressWatch> NetworkWalletAddressWatch { get; set; }
        public DbSet<eMarket> Market { get; set; }
        public DbSet<eMarketAttributes> MarketAttributes { get; set; }
        public DbSet<eMarketProfile> MarketProfile { get; set; }

        public DbSet<eFiatCurrency> FiatCurrency { get; set; }
        public DbSet<eFiatProfile> FiatProfile { get; set; }
        public DbSet<eTradingFee> TradingFee { get; set; }
        public DbSet<eTax> Tax { get; set; }
        public DbSet<eTokenNetworkFee> TokenNetworkFee { get; set; }
        public DbSet<eBankAccount> BankAccounts { get; set; }
        public DbSet<eMarketProfileScope> MarketProfileScope { get; set; }

        public ApiAppContext(DbContextOptions<ApiAppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //UserAccount as Primary
            modelBuilder.Entity<eUserAccount>().HasOne(x => x.AuthEmail).WithOne(x => x.UserAccount);
            modelBuilder.Entity<eUserAccount>().HasOne(x => x.Authenticator).WithOne(x => x.UserAccount);
            modelBuilder.Entity<eUserSession>().HasOne(x => x.UserAccount).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<eTaxResidency>().HasOne(x => x.PreviousTaxResidency).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<eTaxResidency>().HasOne(x => x.NextTaxResidency).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<eCitizenship>().HasOne(x => x.PreviousCitizenship).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<eCitizenship>().HasOne(x => x.NextCitizenship).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<eAuthorizedEmail>().HasIndex(x => x.Email);
            modelBuilder.Entity<eAuthorizedEmail>().HasOne(x => x.PreviousAuthorizedEmail).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<eAuthorizedEmail>().HasOne(x => x.NextAuthorizedEmail).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<eProfile>().HasOne(x => x.UserAccount).WithOne().OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<eKYCRecord>().HasOne(x => x.CountryOfCitizenship).WithOne().OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<eKYCRecord>().HasOne(x => x.KYCStatus).WithOne().OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<eKYCRecord>().HasOne(x => x.TaxIDNType).WithOne().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<eSpotWallet>().HasOne(x => x.UserAccount).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<eFundingWallet>().HasOne(x => x.UserAccount).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<eEarnWallet>().HasOne(x => x.UserAccount).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<eEscrowWallet>().HasOne(x => x.UserAccount).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<eNetworkWalletAddressWatch>().HasOne(x => x.SupportedToken).WithOne().OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<eMarket>().HasOne(x => x.QuoteToken).WithOne().OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<eMarket>().HasOne(x => x.QuoteCurrency).WithOne().OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<eMarket>().HasOne(x => x.BaseToken).WithOne().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<eFiatProfile>().HasOne(x => x.FiatCurrency).WithMany(x => x.Profiles).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<eBankAccount>().HasOne(x => x.FiatProfile).WithMany(x => x.BankAccounts).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<eUserSession>().HasIndex(x => x.UserAccountId).IsUnique(false);
            modelBuilder.Entity<eMarket>().HasIndex(x => x.BaseTokenId).IsUnique(false);
            modelBuilder.Entity<eMarket>().HasIndex(x => x.QuoteCurrencyId).IsUnique(false);
            modelBuilder.Entity<eMarket>().HasIndex(x => x.QuoteTokenId).IsUnique(false);
            modelBuilder.Entity<eMarketProfile>().HasIndex(x => x.QuoteTokenFeeTaxId).IsUnique(false);
            modelBuilder.Entity<eMarketProfile>().HasIndex(x => x.QuoteTokenMakerFeeId).IsUnique(false);
            modelBuilder.Entity<eMarketProfile>().HasIndex(x => x.QuoteTokenTakerFeeId).IsUnique(false);
            modelBuilder.Entity<eMarketProfile>().HasIndex(x => x.QuoteTokenTradeingTaxId).IsUnique(false);
            modelBuilder.Entity<eMarketProfile>().HasIndex(x => x.BaseTokenFeeTaxId).IsUnique(false);
            modelBuilder.Entity<eMarketProfile>().HasIndex(x => x.BaseTokenMakerFeeId).IsUnique(false);
            modelBuilder.Entity<eMarketProfile>().HasIndex(x => x.BaseTokenTakerFeeId).IsUnique(false);
            modelBuilder.Entity<eMarketProfile>().HasIndex(x => x.BaseTokenTradeingTaxId).IsUnique(false);

            modelBuilder.Entity<eUserAccount>().OwnsOne(x => x.RefCode).Property(x => x.RefferedBy).HasColumnName("RefFrom");
            modelBuilder.Entity<eUserAccount>().OwnsOne(x => x.RefCode).Property(x => x.myCommunity).HasColumnName("MyRefCode");
            //RowVersion

        }
    }
    internal static class NetworkScWallet
    {
        internal static class Ethereum
        {
            internal static string GetActiveBankWallet
            {
                get
                {
                    //ToDo:Naveen, Read PBC for deployed smartContract for Currently Active/Authorized bank Wallet?
                    var str = ConfigEx.Config.GetSection("MainNetworkID").Value;
                    if (str == "5")
                        return "0xeAA634478D61259b5e04641b65Eff55fe98cF4b6";
                    else if (str == "1")
                        return "0xCC38F8154799831bdFd447c7717B3e60b910F807";
                    else return "";
                }
            }
        }
        internal static class BitCoin
        {
            //ToDo: Implementation Pending
        }
    }
    public static class APIDbInitializer
    {
        static ApiAppContext dbctx = null;
        public static void Initialize(ApiAppContext ctx)
        {
            if (!ctx.Country.Any())
            {
                InitializeCountries(ctx);
            }
        }
        private static void InitializeCountries(ApiAppContext ctx)
        {
            dbctx = ctx;
            ctx.Country.AddRange(LoadCountries());
            ctx.EnumBoxData.AddRange(LoadEnums());
            ctx.GlobalVariables.AddRange(LoadVariables());
            ctx.SaveChanges();
            if (ConfigEx.VersionEnvironment == versionEnv.Prod)
            {
                ctx.Token.AddRange(LoadMainNetTokenForPreBeta());
            }
            else
            {
                ctx.Token.AddRange(LoadToken());
            }
            if (ConfigEx.VersionType != versionType.PreBeta)
            {
                ctx.FiatCurrency.AddRange(LoadFiat());
                ctx.TradingFee.AddRange(TradingFee());
            }
            LoadEnums();
            ctx.SaveChanges();
        }
        private static eCountry[] LoadCountries()
        {
            return new eCountry[]
            {
                new eCountry{ DialCode="+91", Name="India",Abbrivation="IN",Continent="Indian Subcontinet",Block="SA"},
                new eCountry{ DialCode="+61", Name="Australia",Abbrivation="AU",Continent="Australia",Block="Ocieana"},
                new eCountry{ DialCode="+1", Name="USA",Abbrivation="US",Continent="America",Block="AM"}
            };
        }
        private static eEnumBoxData[] LoadEnums()
        {
            return new eEnumBoxData[]
          {
                new eEnumBoxData{Name="Mr.",EnumValue="Mr",EnumType="title"},
                new eEnumBoxData{Name="Mrs.",EnumValue="Mrs",EnumType="title"},
                new eEnumBoxData{Name="Ms.",EnumValue="Ms",EnumType="title"},
                new eEnumBoxData{Name="Mx.",EnumValue="Ms",EnumType="title"},

                new eEnumBoxData{Name="Pending",EnumValue="Pending",EnumType="KYCStatus",Id=0},
                new eEnumBoxData{Name="Started",EnumValue="Started",EnumType="KYCStatus",Id=1},
                new eEnumBoxData{Name="UnderReview",EnumValue="UnderReview",EnumType="KYCStatus",Id=2},
                new eEnumBoxData{Name="Rejected",EnumValue="Rejected",EnumType="KYCStatus", Id = 3},
                new eEnumBoxData{Name="CompletedType1",EnumValue="CompletedType1",EnumType="KYCStatus", Id = 4},
                new eEnumBoxData{Name="UnderReviewForType2",EnumValue="UnderReviewForType2",EnumType="KYCStatus",Id=2},
                new eEnumBoxData{Name="CompletedType2",EnumValue="CompletedType2",EnumType="KYCStatus", Id = 5},
          };

        }
        private static eGlobalVariables[] LoadVariables()
        {
            return new eGlobalVariables[]
            {
                new eGlobalVariables{ Key="Convert",Value="0.03"},
                new eGlobalVariables{ Key="Buy",Value="0.03"},
                new eGlobalVariables{ Key="MinimumLegalAge",Value="18"},
                new eGlobalVariables{ Key="MinumumGlobalTick",Value="8"},//1/10^8
                new eGlobalVariables{ Key="MinOrderSizeValueUSD",Value="10"},//RefUserSignUPOneTimeAward
                new eGlobalVariables{ Key="ReferredUserSignUPOneTimeAward",Value="3"},//
                new eGlobalVariables{ Key="ReferralSignUPOneTimeAward",Value="1"},//
            };
        }
        private static eSupportedNetwork LoadSupportedNetwork()
        {
            return new eSupportedNetwork { Name = "Etherum Main Net", NativeCurrencyCode = "ETH", RecordHash = "?", IsSmartContractEnabled = true, NetworkProxy = "http://goerli.tst.navexm.int.lab", Description = "Reference to Etherum Network" };
        }
        private static eSupportedNetwork LoadMainNetSupportedNetwork()
        {
            return new eSupportedNetwork { Name = "Etherum Main Net", NativeCurrencyCode = "ETH", RecordHash = "?", IsSmartContractEnabled = true, NetworkProxy = "https://eth-mainnet.public.blastapi.io", Description = "Reference to Etherum Network" };
        }
        private static eToken[] LoadToken()
        {
            var net = LoadSupportedNetwork();
            var lst = new List<eToken>();
            var a = new eToken
            {
                Code = "NavC",
                Details = "here is some information for NavC",
                FullName = "NavC Token for NavExM",
                ShortName = "NavC",
                RecordHash = "?",
                WebURL = "www.google.com",
                //WhitePaper="www.whitepaper.com",
                Tick = 0.00000000001,
                AllowedCountries = new List<eSupportedCountry>()
            };
            dbctx.Country.ToList().ForEach(x =>
            {
                a.AllowedCountries.Add(new eSupportedCountry { CountryId = x.CountryId, Country = x, SupportedSince = DateTime.UtcNow, Notes = "System Auto Populate Data" });
            });
            a.SupportedCoin = a.SupportedCoin ?? new List<eSupportedToken>();
            a.SupportedCoin.Add(new eSupportedToken { RelatedNetwork = net, ContractAddress = "0x9c9cF1614f14e8bFE581caeE92Ae7e3a47d3beF2", Code = "NavC", Narration = "NavC narration", RecordHash = "This is RecordHash" });

            var b = new eToken
            {
                Code = "USDT",
                Details = "here is some information for USDT",
                FullName = "USDT Token",
                ShortName = "USDT",
                WebURL = "www.google.com",
                // WhitePaper = "www.whitepaper.com",
                RecordHash = "?",
                Tick = 0.000001,
                //Tick = 0.000000000000000001,
                AllowedCountries = new List<eSupportedCountry>()
            };
            dbctx.Country.ToList().ForEach(x =>
            {
                b.AllowedCountries.Add(new eSupportedCountry { CountryId = x.CountryId, Country = x, SupportedSince = DateTime.UtcNow, Notes = "System Auto Populate Data" });
            });
            b.SupportedCoin = b.SupportedCoin ?? new List<eSupportedToken>();

            b.SupportedCoin.Add(new eSupportedToken { RelatedNetwork = net, ContractAddress = "0x35D73462f27e863789aF43ED9F3b91aDa6CE3770", Code = "USDT", Narration = "USDT narration", RecordHash = "This is RecordHash" });

            var c = new eToken
            {
                Code = "WBTC",
                Details = "here is some information for BTC",
                FullName = "BTC on Etherium Network",
                ShortName = "wBTC",
                WebURL = "www.google.com",
                //WhitePaper = "www.whitepaper.com",
                RecordHash = "?",
                Tick = 0.0000000001,
                AllowedCountries = new List<eSupportedCountry>()
            };
            dbctx.Country.ToList().ForEach(x =>
            {
                c.AllowedCountries.Add(new eSupportedCountry { CountryId = x.CountryId, Country = x, SupportedSince = DateTime.UtcNow, Notes = "System Auto Populate Data" });
            });
            c.SupportedCoin = c.SupportedCoin ?? new List<eSupportedToken>();
            c.SupportedCoin.Add(new eSupportedToken { RelatedNetwork = net, ContractAddress = "0x7a288b03197728Da39EcC7d23CCe14368848f7Ef", Code = "wBTC", Narration = "wBTC narration", RecordHash = "This is RecordHash" });

            var d = new eToken
            {
                Code = "BNB",
                Details = "here is some information for BNB",
                FullName = "BNB on Etherium Network",
                ShortName = "BNB",
                WebURL = "www.google.com",
                //WhitePaper = "www.whitepaper.com",
                RecordHash = "?",
                Tick = 0.000000001,
                AllowedCountries = new List<eSupportedCountry>()

            };
            dbctx.Country.ToList().ForEach(x =>
            {
                d.AllowedCountries.Add(new eSupportedCountry { CountryId = x.CountryId, Country = x, SupportedSince = DateTime.UtcNow, Notes = "System Auto Populate Data" });
            });
            d.SupportedCoin = d.SupportedCoin ?? new List<eSupportedToken>();
            d.SupportedCoin.Add(new eSupportedToken { RelatedNetwork = net, ContractAddress = "0x0c1b7B66BB193bF93640D9e7E32cB481743993b1", Code = "BNB", Narration = "BNB narration", RecordHash = "This is RecordHash" });

            var e = new eToken
            {
                Code = "ETH",
                Details = "here is some information for ETH",
                FullName = "ETH on Etherium Network",
                ShortName = "ETH",
                WebURL = "www.google.com",
                // WhitePaper = "www.whitepaper.com",
                RecordHash = "?",
                Tick = 0.000000000000000001,
                AllowedCountries = new List<eSupportedCountry>()

            };
            dbctx.Country.ToList().ForEach(x =>
            {
                e.AllowedCountries.Add(new eSupportedCountry { CountryId = x.CountryId, Country = x, SupportedSince = DateTime.UtcNow, Notes = "System Auto Populate Data" });
            });

            e.SupportedCoin = e.SupportedCoin ?? new List<eSupportedToken>();
            e.SupportedCoin.Add(new eSupportedToken { RelatedNetwork = net, ContractAddress = "Native", IsNative = true, Code = "ETH", Narration = "ETH narration", RecordHash = "This is RecordHash" });

            lst.Add(a);
            lst.Add(b);
            lst.Add(c);
            lst.Add(d);
            lst.Add(e);
            return lst.ToArray();
        }
        private static eToken[] LoadMainNetTokenForPreBeta()
        {
            var net = LoadMainNetSupportedNetwork();
            var lst = new List<eToken>();

            var b = new eToken
            {
                Code = "USDT",
                FullName = "USDT Token",
                ShortName = "USDT",
                RecordHash = "?",
                //Tick = 0.000001,
                Tick = 0.000001,
                AllowedCountries = new List<eSupportedCountry>()
            };
            dbctx.Country.ToList().ForEach(x =>
            {
                b.AllowedCountries.Add(new eSupportedCountry { CountryId = x.CountryId, Country = x, SupportedSince = DateTime.UtcNow, Notes = "System Auto Populate Data" });
            });
            b.SupportedCoin = b.SupportedCoin ?? new List<eSupportedToken>();

            b.SupportedCoin.Add(new eSupportedToken { RelatedNetwork = net, ContractAddress = "0xdAC17F958D2ee523a2206206994597C13D831ec7", Code = "USDT", Narration = "USDT narration", RecordHash = "This is RecordHash" });

            var e = new eToken
            {
                Code = "ETH",
                FullName = "ETH on Etherium Network",
                ShortName = "ETH",
                RecordHash = "?",
                Tick = 0.000000000000000001,
                AllowedCountries = new List<eSupportedCountry>()

            };
            dbctx.Country.ToList().ForEach(x =>
            {
                e.AllowedCountries.Add(new eSupportedCountry { CountryId = x.CountryId, Country = x, SupportedSince = DateTime.UtcNow, Notes = "System Auto Populate Data" });
            });

            e.SupportedCoin = e.SupportedCoin ?? new List<eSupportedToken>();
            e.SupportedCoin.Add(new eSupportedToken { RelatedNetwork = net, ContractAddress = "Native", IsNative = true, Code = "ETH", Narration = "ETH narration", RecordHash = "This is RecordHash" });

            lst.Add(b);
            lst.Add(e);
            return lst.ToArray();
        }
        private static eFiatCurrency[] LoadFiat()
        {
            var bank = new eBankAccount()
            {
                AccountNumber = "012588-66958",
                BankName = "Citi Bank",
                BranchAddress = "Ohio State",
                AdditionalInfo = "some more info.",
                RecordHash = "?",

                LocatedAt = dbctx.Country.First(x => x.Name == "USA")
            };
            var profile = new eFiatProfile
            {
                CountryOrigin = dbctx.Country.First(x => x.Name == "USA"),
                IsExchangeAllowed = false,
                IsP2PAllowed = false,
                RecordHash = "?",
                BankAccounts = new List<eBankAccount>() { bank }
            };
            var USD = new eFiatCurrency()
            {
                Code = "USD",
                Name = "US Doller",
                Description = "Some more information aboout Doller",
                Symbole = "$",
                Tick = 0.01,
                RecordHash = "?",
                Profiles = new List<eFiatProfile>() { profile }

            };
            var bank1 = new eBankAccount()
            {
                AccountNumber = "091748-66958",
                BankName = "State Bank",
                BranchAddress = "Delhi",
                AdditionalInfo = "some more info.",
                RecordHash = "?",
                LocatedAt = dbctx.Country.First(x => x.Name == "India")
            };
            var IndianProfile = new eFiatProfile
            {
                CountryOrigin = dbctx.Country.First(x => x.Name == "India"),
                IsExchangeAllowed = false,
                RecordHash = "?",
                IsP2PAllowed = true,
                BankAccounts = new List<eBankAccount>() { bank1 }
            };
            var INR = new eFiatCurrency()
            {
                Code = "INR",
                Name = "Indian Rupee",
                Description = "Some more information aboout Indian Rupee",
                Symbole = "₹",
                RecordHash = "?",
                Tick = 0.01,
                Profiles = new List<eFiatProfile>() { IndianProfile }

            };
            return new eFiatCurrency[] { USD, INR };
        }
        private static eTradingFee[] TradingFee()
        {
            var ComM = new eTradingFee()
            {
                FeeName = "NavC Staked Fee",
                FeeCommunity = 0.0001,
                FeeNonCommunity = 0.0002,
                FeeExempt = 0.000,
                FeeIndependent = 0.001,
                Details = "People who have staked",
                DisplayAsSwap = true,
                FeeType = Entity.FeeType.Standard
            };
            var Ex = new eTradingFee()
            {
                FeeName = "NavC Staked Fee",
                FeeExempt = 0.0000,
                Details = "Market that is Charge Free",
                DisplayAsSwap = true,
                FeeType = Entity.FeeType.Exempt
            };
            var Ind1 = new eTradingFee()
            {
                FeeName = "NavC Staked Fee",
                FeeIndependent = 0.001,
                Details = "Market that is Charge Free",
                DisplayAsSwap = true,
                FeeType = Entity.FeeType.Independent
            };
            var Ind2 = new eTradingFee()
            {
                FeeName = "NavC Staked Fee",
                FeeIndependent = 0.002,
                Details = "Market that is Charge Free",
                DisplayAsSwap = true,
                FeeType = Entity.FeeType.Independent
            };

            return new eTradingFee[] { ComM, Ex, Ind1, Ind2 };
        }
    }

    public class EventAppContext : DbContext
    {
        public DbSet<PageEventRecord> PageEvent { get; set; }

        public EventAppContext(DbContextOptions<EventAppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //UserAccount as Primary


        }
    }
    public class PageEventRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RecordId { get; set; }
        [StringLength(50)]
        public string? LTUID { get; set; }
        [StringLength(50)]
        public string? Country { get; set; }
        [StringLength(50)]
        public string? City { get; set; }
        [StringLength(50)]
        public string? PageInstanceId { get; set; }
        public double Scroll { get; set; }
        public double ScreenHeight { get; set; }

        [StringLength(50)]
        public string? IP { get; set; }
        [StringLength(5000)]
        public string? Page { get; set; }
        [StringLength(500)]
        public string? Event { get; set; }
        public DateTime? At { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
    public class RewardAppContext : DbContext
    {
        public DbSet<eAccruedCashBack> AccruedCashBack { get; set; }
        /// <summary>
        /// we will record raw data / Transaction Data as it happens
        /// </summary>
        public DbSet<SignUpUser> SignUpUsers { get; set; }
        /// <summary>
        /// we will record user who have community status as they get on and off the staking
        /// </summary>
        public DbSet<SignUpCommunityUser> SignUpCommunityUser { get; set; }
        /// <summary>
        /// we will calculate and record summary from Raw Data Stored in 'SignUpUsers' & 'SignUpCommunityUser' Objects before reward is given each month
        /// </summary>
        public DbSet<SignUpUserSummary> SignUpUserSummary { get; set; }
        /// <summary>
        /// we will record one row each reward type, each month before deleting data to keep system lite. for Historic Record this Tabel will be referred
        /// </summary>
        public DbSet<RefReward> Rewards { get; set; }

        public RewardAppContext(DbContextOptions<RewardAppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //UserAccount as Primary
            modelBuilder.Entity<SignUpUser>(x => x.Property(p => p.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate());
            modelBuilder.Entity<eAccruedCashBack>(x => x.Property(p => p.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate());

            modelBuilder.Entity<SignUpUserSummary>(x => x.Property(p => p.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate());
            modelBuilder.Entity<RefReward>(x => x.Property(p => p.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate());
            modelBuilder.Entity<SignUpCommunityUser>(x => x.Property(p => p.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate());
        }
    }
    [Index(nameof(RefCode))]
    [Index(nameof(CalculatedOn))]
    [Index(nameof(ForMonthOf))]
    [Index(nameof(RewardBaseValue))]
    [Index(nameof(Reward))]
    [Index(nameof(RewardType))]
    [Index("TransactionId", IsUnique = true)]
    public class RefReward
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public byte[] RowVersion { get; set; }
        [StringLength(50)]
        public string RefCode { get; set; }
        /// <summary>
        /// Date of Reward Calculation, It would be the gain from Previous Position
        /// </summary>
        public DateTime CalculatedOn { get; set; }
        public DateTime ForMonthOf { get; set; }
        public DateTime? DateOfPayment { get; set; }
        /// <summary>
        /// Reward NavC base(USDT) value. Actual Tokens Count will be decided on the date of payment
        /// </summary>
        public double RewardBaseValue { get; set; }
        /// <summary>
        /// NavC Tokens given as Reward
        /// </summary>
        public double? Reward { get; set; }
        /// <summary>
        /// Name of the Reward Type
        /// </summary>
        [StringLength(50)]
        public string RewardType { get; set; }
        /// <summary>
        /// Extended object for additional info if Required
        /// </summary>
        public string AdditionalInfo { get; set; } = string.Empty;
        public Guid TransactionId { get; set; }

    }
    /// <summary>
    /// Entity to Hold Various Stages of Process till SignUp Reward is given to the Referee.
    /// </summary>
    [Index("RefCode")]
    [Index("RegisteredOn")]
    [Index("UserAccountId")]
    [Index("RewardEventOn")]
    [Index("RewardBaseValue")]
    [Index("Reward")]
    [Index("Status")]
    [Index("DateOfPayment")]
    [Index("TransactionId")]
    public class SignUpUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public byte[] RowVersion { get; set; }
        [StringLength(50)]
        public string RefCode { get; set; }
        /// <summary>
        /// Date when Refered User Register with NavExM
        /// </summary>
        public DateTime RegisteredOn { get; set; } = DateTime.UtcNow.Date;
        /// <summary>
        /// Date when Refered User completed the Milestone, qualifing Referral Reward
        /// </summary>
        public DateTime? RewardEventOn { get; set; }
        public Guid UserAccountId { get; set; }

        /// <summary>
        /// Reward NavC base(USDT) value. Actual Tokens Count will be decided on the date of payment
        /// </summary>
        public double RewardBaseValue { get; set; }
        /// <summary>
        /// NavC Tokens given as Reward
        /// </summary>
        public double? Reward { get; set; }
        /// <summary>
        /// Current status of the Reward
        /// </summary>
        public eRewardStatus Status { get; set; }
        /// <summary>
        /// Date of Reward payment
        /// </summary>
        public DateTime? DateOfPayment { get; set; }
        public DateTime? PrimaryCompletedOn { get; set; }
        /// <summary>
        /// Reward Deposit Transaction ID
        /// </summary>
        public Guid? TransactionId { get; set; }

    }
    [Index("RefCode")]
    [Index("RegisteredOnMonth")]
    [Index("RewardBaseValuePaid")]
    [Index("Reward")]
    public class SignUpUserSummary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public byte[] RowVersion { get; set; }
        [StringLength(50)]
        public string RefCode { get; set; }
        /// <summary>
        /// Date (Only month matters) when Refered User Register with NavExM
        /// </summary>
        public DateTime RegisteredOnMonth { get; set; }
        /// <summary>
        /// Reward NavC base(USDT) value. Actual Tokens Count will be decided on the date of payment
        /// </summary>
        public double RewardBaseValuePaid { get; set; }
        /// <summary>
        /// No of Referrals that Resulted this Reward
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// NavC Tokens given as Reward
        /// </summary>
        public double? Reward { get; set; }
    }
    [Index("RefCode")]
    [Index("UserAccountId", IsUnique = true)]
    [Index("StatusGrantedOn")]
    [Index("LastVerificationOn")]
    public class SignUpCommunityUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public byte[] RowVersion { get; set; }
        [StringLength(50)]
        public string RefCode { get; set; }
        public Guid UserAccountId { get; set; }

        /// <summary>
        /// Date when Refered User is given Community Status
        /// </summary>
        public DateTime StatusGrantedOn { get; set; }
        /// <summary>
        /// Date when community status of this user last verified. This is with in This MONTH then we will display this user as Community User in the Reward Page
        /// </summary>
        public DateTime LastVerificationOn { get; set; }
    }
    [Index(nameof(RecordHash))]
    [Index(nameof(Narration))]
    [Index(nameof(userAccount))]
    [Index(nameof(MarketCode))]
    [Index(nameof(TradeId))]
    [Index(nameof(InternalOrderID))]
    [Index(nameof(ProvisionedOn))]
    [Index(nameof(TradeOn))]
    [Index(nameof(RewardTransactionId))]
    [Index(nameof(TradeId),nameof(InternalOrderID),IsUnique =true,Name ="TradeAndOrderID")]
    public class eAccruedCashBack : secBaseEntity1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AccruedCashBackId { get; set; }
        public byte[] RowVersion { get; set; }
        [StringLength(50)]
        public string userAccount { get; set; }
        public eCommunityCategory Community { get; set; }
        public Guid? RewardTransactionId { get; set; }
        public Guid TradeId { get; set; }
        [StringLength(50)]
        public string MarketCode { get; set; }
        public double TradeValue { get; set; }
        public Guid InternalOrderID { get; set; }//Buy/Sell will have 2 rows
        public DateTime TradeOn { get; set; }
        public DateTime ProvisionedOn { get; set; } = DateTime.UtcNow;
        public double CashBackNavCValue { get; set; }
        public double CashBackNavCTokens { get; set; }
        public double PoolRefundNavCTokens { get; set; }

        //WalletTransactionId+Date+TokenId+TAmount+FromWalletId+ToWalletId+FromWalletAfterTransactionBalance+ToWalletAfterTransactionBalance+SessionHash
        public string RecordHash { get; set; }
        [StringLength(1000)]
        public string Narration { get; set; } = string.Empty;

        public string? ReserveId { get; set; }
        public DateTime? ReservasationExpiry { get; set; }

    }

    public class AlertAppContext : DbContext {
        public DbSet<eUserAlertMsg> UserAlerts { get; set; }
        public DbSet<eStaffAlertMsg> StaffAlerts { get; set; }
        public AlertAppContext(DbContextOptions<AlertAppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
    public class CyAuthAppContext : DbContext
    {
        public DbSet<eCryptoWithdrawRequest> CryptoWithdrawRequest { get; set; }
        public DbSet<eWithdrawlRequestStatus> WithdrawlRequestStatus { get; set; }

        public CyAuthAppContext(DbContextOptions<CyAuthAppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //RowVersion
            modelBuilder.Entity<eCryptoWithdrawRequest>(x => x.Property(p => p.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate());

        }
    }
    public class FundRequestAppContext : DbContext
    {
        public DbSet<eFiatWithdrawRequest> FiatWithdrawRequest { get; set; }
        public DbSet<eFiatDepositRequest> FiatDepositRequest { get; set; }
        public DbSet<eWithdrawlRequestStatus> WithdrawlRequestStatus { get; set; }

        public FundRequestAppContext(DbContextOptions<FundRequestAppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //RowVersion
            modelBuilder.Entity<eFiatWithdrawRequest>(x => x.Property(p => p.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate());
            modelBuilder.Entity<eFiatDepositRequest>(x => x.Property(p => p.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate());
        }
    }
    public enum eRewardStatus
    {
        None = 0,
        Earned = 1,
        Paid = 2
    }
    public enum eRewardType
    {
        Referral,Prepetual,OneTimeBonus
    }
    [Index(nameof(userAccount))]
    [Index(nameof(MsgBody))]
    [Index(nameof(MsgTitle))]
    [Index(nameof(GeneratedOn))]
    [Index(nameof(ReportedOn))]
    public class eUserAlertMsg
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [StringLength(200)]
        public string userAccount { get; set; }
        [StringLength(2000)]
        public string MsgBody { get; set; }
        [StringLength(100)]
        public string MsgTitle { get; set; }
        public DateTime GeneratedOn { get; set; } = DateTime.UtcNow;
        public DateTime ReportedOn { get; set; }
    }
    [Index(nameof(staffRole))]
    [Index(nameof(staffAccount))]
    [Index(nameof(MsgBody))]
    [Index(nameof(MsgTitle))]
    [Index(nameof(GeneratedOn))]
    [Index(nameof(ReportedOn))]
    public class eStaffAlertMsg
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string staffRole { get; set; } = string.Empty;
        [StringLength(200)]
        public string staffAccount { get; set; } = string.Empty;
        [StringLength(2000)]
        public string MsgBody { get; set; }
        [StringLength(100)]
        public string MsgTitle { get; set; }
        public DateTime GeneratedOn { get; set; }
        public DateTime ReportedOn { get; set; }
    }
}
