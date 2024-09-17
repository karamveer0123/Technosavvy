using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TechnoApp.Ext.Web.UI.Model;
using TechnoApp.Ext.Web.UI.Service;
using NuGet.Protocol;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace TechnoApp.Ext.Web.UI.ViewModels
{
    public class vmBase
    {
        string _country = "IN";
        public string _Country
        {
            get => _country; set
            {
                if (value.IsNOT_NullorEmpty() && value.Length == 2) { _country = value; }
                else
                {
                    Console.WriteLine($"An Attempt was made to set a Country Code that was not 2 Digit long:{value}. System Retained the Original value of :{_country}");
                }
            }
        } //Country of Origin for Local Law
        public string _Lang { get; set; }
        public string _LangShort
        {
            get
            {
                if (_Lang.IsNOT_NullorEmpty())
                {
                    if (_Lang.ToLower() == "eng") return "en";
                    else
                    {
                        Console.WriteLine($"An Attempt was made to set a Language preference other than English. It is prohibited as of now..!");
                    }
                }
                return "en";
            }
        }
        string _curr = "USD";
        public string _Curr
        {
            get => _curr;
            set
            {
                if (value.IsNOT_NullorEmpty() && value.Length == 3) { _curr = value; }
                else
                {
                    Console.WriteLine($"An Attempt was made to set a Currency Code that was not 3 Digit long. System Retained the Original value of :{_curr}");
                }
            }
        }
        public string _Theme { get; set; }
        /// <summary>
        /// Trade View Preference
        /// </summary>
        public string _PrefView { get; set; }
        public string _UserName { get; set; }
        public string _NickName { get; set; }
        public string _UserId { get; set; }
        public bool _IsKYC { get; set; }
        public eeKYCStatus _IsKYCStatus { get; set; }
        public string _Page { get; set; }//Current Requested Page
        public bool _Is2FAEnabled { get; set; }
        public string _LTUID { get; set; }//Long Term User ID
        public bool _CookieConsent { get; set; }//Long Term User ID
        public string _SessionHash { get; set; }
        public string _favList { get; set; }
        public string _watchList { get; set; }
        public mPreBetaStats PrebetaBase { get; set; }

        public List<string> _CurrencyList { get; set; } = SrvCurrencyPriceHUB.GetAllCurrenciesName();
    }
    public class vmPBTechnoSavvyBuyForm
    {
        public string ethNetWalletAddress { get; set; }
        public Guid NetworkId { get; set; }
        [MinLength(66, ErrorMessage = "Invalid Transaction Hash")]
        [MaxLength(66, ErrorMessage = "Invalid Transaction Hash")]
        public string TxHash { get; set; }
        public bool isAwaiting { get; set; }
        public int failCount { get; set; }
        public string ErrorMsg { get; set; }
    }
    public class vmMyStakingConfirm : vmBase
    {
        public mStake Record { get; set; }
        public mToken Token { get; set; }
    }
    public class vmMyStaking : vmBase
    {
        public List<mStakingSlot2> RelatedStakingSlot { get; set; }
        public List<mToken> TokenList { get; set; }
        public mToken Token { get; set; }
        public double wBalance { get; set; }
        public double otherBalance { get; set; }
        public mStakingSlot2 selectedStakingSlot { get; set; }
        public double Amount { get; set; }
        public bool AutoRenew { get; set; }
        public bool IsIncludeOtherBalance { get; set; }
        public bool IsAgree { get; set; }
    }
    public class vmPBTechnoSavvyBuy : vmBase
    {
        public vmPBTechnoSavvyBuyForm frm { get; set; }
    }
    public class vmJD : vmBase
    {
        public mJD JD { get; set; }
    }
    public class vmPrebetaBase
    {
        public double Last24UserFactor { get; set; }
        public double TotalUserFactor { get; set; }
        public double Last24TokenFactor { get; set; }
        public double TotalTokenFactor { get; set; }
        public double CurrentTechnoSavvyPrice { get; set; }
        public DateTime BetaLiveIn { get; set; }
    }
    public class vmPBMyPurchaseRecords
    {
        public DateTime DateOf { get; set; }
        public string TxnID { get; set; }
        public double BuyWith { get; set; }
        public string BuyWithName { get; set; }
        public double Amount { get; set; }
    }
    public class vmPBMyPurchase : vmBase
    {
        public double TotalTechnoSavvy { get; set; }
        public double TotalLaunchValue { get; set; }
        public double TotalCurrentValue { get; set; }
        public double TotalPurchasValue { get; set; }
        public List<vmPBMyPurchaseRecords> myRecords { get; set; }
    }
    public class vmBaseTrade : vmBase
    {
        public string q { get; set; }
        public string b { get; set; }
        public string qf { get; set; }
        public string bf { get; set; }
        //TechnoSavvy (cashback) fraction to be displayed cashback  Qty
        public string nf { get; set; }
        //USDT fraction as denominator to display value
        public string uf { get; set; }
        public string mCode { get; set; }

        //-----
        //True if User is good for Trading UI
        public bool IsUserAuth { get; set; }
        public double AvailableBase { get; set; }
        public double AvailableQuote { get; set; }
        public string BaseTokenId { get; set; }
        public string QuoteTokenId { get; set; }

        //Min Fraction of Quote in a Trade
        public double MinQToken { get; set; }
        //Min Fraction of Base in a Trade
        public double MinBToken { get; set; }
        //Swap Rate for this User(staking,Community member/Market)
        public double Bid_SwapRate { get; set; }
        public double Ask_SwapRate { get; set; }
        //Max Cashback this User can availe in this market
        public double CBthreashold { get; set; }
        //in USDT value
        public double MinTradeValue { get; set; }

    }
    public static class vmFactory
    {
        public static vmBase InitializeBase(vmBase vm, AppSessionManager aM)
        {

            vm._Theme = aM.GetTheme().ToString();

            if (aM.ExtSession.IsLoaded || aM.mySession.UserCountry.IsNOT_NullorEmpty())
                vm._Country = aM.mySession.UserCountry;
            else if (aM.mySession.UserCountry.IsNullOrEmpty())
                vm._Country = aM.mySession.GIO.CountryCode;

            aM.ExtSession.LoadSession().GetAwaiter().GetResult();
            if (aM.ExtSession.UserSession?.UserAccount != null)
                vm._Is2FAEnabled = aM.ExtSession.UserSession.UserAccount.IsMultiFactor && aM.ExtSession.UserSession.UserAccount.Authenticator.Code.IsNOT_NullorEmpty();

            vm._LTUID = aM.mySession.LTUID;
            vm._CookieConsent = aM.mySession.CookieConsent;
            vm._Curr = aM.mySession.Currency;
            vm._Lang = aM.mySession.Language;
            vm._UserName = aM.mySession.UserName;
            vm._UserId = aM.mySession.UserId;
            vm._NickName = aM.mySession.NickName;
            vm._IsKYC = aM.mySession.KYC;
            vm._IsKYCStatus = aM.mySession.KYCStatus;
            vm._SessionHash = aM.mySession.SessionHash;
            vm._favList = aM.mySession.FavList.ToJson().ToLower();
            vm._watchList = aM.mySession.WatchList.ToJson().ToLower();

            aM._httpContext.Request.Query.TryGetValue("uref", out var RefLink);
            aM.mySession.RefCode = RefLink;
            vm.PrebetaBase = SrvPrebeta.GetPrebetaStats;
            return vm;

        }
        public static async Task<vmBase> GetvmBase(AppSessionManager aM, string PageName = "HomePage")
        {
            var vm = InitializeBase(new vmBase(), aM);
            vm._Page = PageName;
            return vm;
        }
        public static async Task<vmMyStakingConfirm> GetvmMyStakingConfirm(AppSessionManager aM, string PageName = "HomePage")
        {
            var vm = new vmMyStakingConfirm();
            vm = (vmMyStakingConfirm)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static async Task<vmMyStaking> GetvmMyStaking(AppSessionManager aM, string PageName = "HomePage")
        {
            var vm = new vmMyStaking();
            vm = (vmMyStaking)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static async Task<vmPBTechnoSavvyBuy> GetvmPrebetaTechnoSavvyBuy(AppSessionManager aM, string PageName = "HomePage")
        {
            var vm = new vmPBTechnoSavvyBuy();
            vm.frm = new vmPBTechnoSavvyBuyForm();
            vm = (vmPBTechnoSavvyBuy)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static async Task<vmPaymentMethodSetup> GetvmPaymentMethodSetup(AppSessionManager aM, string PageName = "HomePage")
        {
            var vm = new vmPaymentMethodSetup();
            vm = (vmPaymentMethodSetup)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static async Task<vmPaymentDeposits> GetvmPaymentDeposits(AppSessionManager aM, string PageName = "HomePage")
        {
            var vm = new vmPaymentDeposits();
            vm = (vmPaymentDeposits)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static async Task<vmPBMyPurchase> GetvmPBMyPurchase(AppSessionManager aM, string PageName = "HomePage")
        {
            var vm = new vmPBMyPurchase();
            vm = (vmPBMyPurchase)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static async Task<vmJD> GetvmJD(AppSessionManager aM, string PageName = "HomePage")
        {
            var vm = new vmJD();
            vm = (vmJD)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static async Task<vmOrders> GetvmOrders(AppSessionManager aM, string PageName = "HomePage")
        {
            var vm = new vmOrders();
            vm = (vmOrders)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static async Task<vmTrades> GetvmTrades(AppSessionManager aM, string PageName = "HomePage")
        {
            var vm = new vmTrades();
            vm = (vmTrades)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static async Task<vmWalletFundTransfer> GetvmWalletFundTransfer(AppSessionManager aM, string PageName = "HomePage")
        {
            var vm = new vmWalletFundTransfer();
            vm = (vmWalletFundTransfer)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static async Task<vmBuy> GetvmBuy(AppSessionManager aM, string PageName = "HomePage")
        {
            var vm = new vmBuy();
            vm = (vmBuy)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static async Task<vmConvert> GetvmConvert(AppSessionManager aM, string PageName = "HomePage")
        {
            var vm = new vmConvert();
            vm = (vmConvert)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static async Task<vmDeposit> GetvmDeposit(AppSessionManager aM, string PageName = "HomePage")
        {
            var vm = new vmDeposit();
            vm = (vmDeposit)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static async Task<vmClaimTranDeposit> GetvmClaimTranDeposit(AppSessionManager aM, string PageName = "HomePage")
        {
            var vm = new vmClaimTranDeposit();
            vm = (vmClaimTranDeposit)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static async Task<vmWithdraw> GetvmWithdraw(AppSessionManager aM, string PageName = "HomePage")
        {
            var vm = new vmWithdraw();
            vm = (vmWithdraw)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static async Task<vmWithdrawConfirmation> GetvmWithdrawConfirmation(AppSessionManager aM, string PageName = "HomePage")
        {
            var vm = new vmWithdrawConfirmation();
            vm = (vmWithdrawConfirmation)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static vmUser GetvmUser(AppSessionManager aM, string PageName = "UserLogIn")
        {
            var vm = new vmUser();
            vm = (vmUser)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static vmMarketInfo GetvmMarketTile(AppSessionManager aM, string PageName = "MarketInfo")
        {
            var vm = new vmMarketInfo();
            vm = (vmMarketInfo)InitializeBase(vm, aM);
            vm._Page = PageName;

            return vm;
        }
        public static vmTokenDetails GetvmTokenDetails(AppSessionManager aM, string PageName = "MarketInfo")
        {
            var vm = new vmTokenDetails();
            vm = (vmTokenDetails)InitializeBase(vm, aM);
            vm._Page = PageName;

            return vm;
        }
        public static vmRegisterUser GetvmRegisterUser(AppSessionManager aM, string PageName = "UserRegister")
        {
            var vm = new vmRegisterUser();
            vm = (vmRegisterUser)InitializeBase((vmBase)vm, aM);
            vm._Page = PageName;

            vm._SessionHash = aM.mySession.SessionHash;
            vm.vmRegUserName = new vmRegisterUserName();
            vm.vmPassword = new vmRegisterUserPassword();
            vm.EmailOTP = new vmRegisterUserNameOTP();
            return vm;
        }

        public static vmForgetPassword GetvmForgetPassword(AppSessionManager aM, string PageName = "ForgetPassword")
        {
            var vm = new vmForgetPassword();
            vm = (vmForgetPassword)InitializeBase(vm, aM);
            vm._Page = PageName;

            return vm;
        }

        public static vmUserLogin GetvmUserLogin(AppSessionManager aM, string PageName = "Dummy")
        {
            var vm = new vmUserLogin();
            vm = (vmUserLogin)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static vmFAQDisplay GetvmFaqDisplay(AppSessionManager aM, string PageName = "Dummy")
        {
            var vm = new vmFAQDisplay();
            vm = (vmFAQDisplay)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static vmAcademyDetail GetvmAcademyDetail(AppSessionManager aM, string PageName = "Dummy")
        {
            var vm = new vmAcademyDetail();
            vm = (vmAcademyDetail)InitializeBase(vm, aM);
            vm._Page = PageName;

            return vm;
        }
        public static vm2ndAuth Getvm2ndAuth(AppSessionManager aM, string PageName = "2ndAuth")
        {
            var vm = new vm2ndAuth();
            vm = (vm2ndAuth)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static vm2ndAuthPaymentMethod Getvm2ndAuthPaymentMethod(AppSessionManager aM, string PageName = "2ndAuth")
        {
            var vm = new vm2ndAuthPaymentMethod();
            vm = (vm2ndAuthPaymentMethod)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static vmSettingsAuth GetvmSettingsAuth(AppSessionManager aM, string PageName = "SettingsAuth")
        {
            var vm = new vmSettingsAuth();
            vm = (vmSettingsAuth)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
        public static vmSettings GetvmSettings(AppSessionManager aM, string PageName = "Setting")
        {
            var vm = new vmSettings();
            vm = (vmSettings)InitializeBase(vm, aM);
            vm._Page = PageName;

            return vm;
        }
        public static vmDashboard GetvmDashboard(AppSessionManager aM, string PageName = "DashBoard")
        {
            var vm = new vmDashboard();
            vm = (vmDashboard)InitializeBase(vm, aM);
            vm._Page = PageName;

            return vm;
        }
        public static vmWalletHome GetvmWalletHome(AppSessionManager aM, string PageName = "WalletHome")
        {
            var vm = new vmWalletHome();
            vm = (vmWalletHome)InitializeBase(vm, aM);
            vm._Page = PageName;

            return vm;
        }
        public static vmWalletTransactions GetvmWalletTransactions(AppSessionManager aM, string PageName = "WalletHome")
        {
            var vm = new vmWalletTransactions();
            vm = (vmWalletTransactions)InitializeBase(vm, aM);
            vm._Page = PageName;

            return vm;
        }
        public static vmSpotWallet GetvmSpotWallet(AppSessionManager aM, string PageName = "Spot")
        {
            var vm = new vmSpotWallet();
            vm = (vmSpotWallet)InitializeBase(vm, aM);
            vm._Page = PageName;

            return vm;
        }
        public static vmFundingWallet GetvmFundingWallet(AppSessionManager aM, string PageName = "Funding")
        {
            var vm = new vmFundingWallet();
            vm = (vmFundingWallet)InitializeBase(vm, aM);
            vm._Page = PageName;

            return vm;
        }
        public static vmEarnWallet GetvmEarnWallet(AppSessionManager aM, string PageName = "Earn")
        {
            var vm = new vmEarnWallet();
            vm = (vmEarnWallet)InitializeBase(vm, aM);
            vm._Page = PageName;

            return vm;
        }
        public static vmRewardCenter GetvmRewardCenter(AppSessionManager aM, string PageName = "Home Page")
        {
            var vm = new vmRewardCenter();
            vm = (vmRewardCenter)InitializeBase(vm, aM);
            vm._Page = PageName;

            return vm;
        }

        public static vmProfile GetvmProfile(AppSessionManager aM, string PageName = "Profile")
        {
            var vm = new vmProfile();
            vm = (vmProfile)InitializeBase(vm, aM);
            vm._Page = PageName;

            return vm;
        }
        public static vmKYCDocUploadStage GetvmKYCDocUploadStage(AppSessionManager aM, string PageName = "Profile")
        {
            var vm = new vmKYCDocUploadStage();
            vm = (vmKYCDocUploadStage)InitializeBase(vm, aM);
            vm._Page = PageName;

            return vm;
        }
        public static vmMarketTrade GetvmMarketTrade(AppSessionManager aM, string PageName = "MarketTrade")
        {
            var vm = new vmMarketTrade();
            vm = (vmMarketTrade)InitializeBase(vm, aM);
            vm._Page = PageName;
            return vm;
        }
    }

    public class vmRewardCenter : vmBase
    {
        public string RefLink { get; set; }
        public mRewardStats Reward { get; set; }
        public double TechnoSavvyUSDTVal { get; set; }
        public List<mUserRef> myReferrals { get; set; }
        public List<mMyReward> myRewards { get; set; }
      //  public List<mRewardPrepetual> myPrepetualRewards { get; set; }
    }
    public class mRewardRef
    {
        //SignUp and Bonus Awards
        public DateTime Date { get; set; }
        public string RewardType { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }

    }
    public class mRewardPrepetual
    {
        //SWAP CB Percentage
        public DateTime Date { get; set; }
        public string RewardType { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }

    }
    public class mUserRef
    {
        public string Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string UserStatus { get; set; }
        public double Amount { get; set; }
        public int Count { get; set; }
        public bool IsActive { get; set; }
    }
    public class mAccruedCashBack
    {
        public eCommunityCategory Community { get; set; }
        public Guid RewardTransactionId { get; set; }
        public Guid TradeId { get; set; }
        public string MarketCode { get; set; }
        public double TradeValue { get; set; }
        public double CashBackTechnoSavvyValue { get; set; }
        public double CashBackTechnoSavvyTokens { get; set; }
        public DateTime CreatedOn { get; set; }
    }
    public class vmMarketInfo : vmBase
    {
        public List<vmMarketTile> Info { get; set; } = new List<vmMarketTile>();

    }
    public class vmMarketTile
    {
        public string TokenCode { get; set; }
        public string TokenName { get; set; }
        public double Price { get; set; }
        public double CashbackYTD { get; set; }
        public double Price24HrChange { get; set; }
        public double Volumn24HrChange { get; set; }
        public double Volumn { get; set; }
        public string BaseFormat { get; set; }
        public List<string> Category { get; set; } = new List<string>();
        public List<string> SubCategory { get; set; } = new List<string>();
        public int[] Poly { get; set; }
        public string MarketCapRank { get; set; }
        public string CirculationSupply { get; set; }
        public string MaxSupply { get; set; }
        public string TotalSupply { get; set; }
        public string IssueDate { get; set; }
        public string IssuePrice { get; set; }
        public string Details { get; set; }
        public string Network { get; set; }
        public string ContractAddress { get; set; }

    }
    public class vmTokenDetails : vmBase
    {
        public string TokenCode { get; set; }
        public string TokenName { get; set; }
        public double Price { get; set; }
        public double CashbackYTD { get; set; }
        public double Price24HrChange { get; set; }
        public double Volumn24HrChange { get; set; }
        public double Volumn { get; set; }
        public string BaseFormat { get; set; }
        public List<string> Category { get; set; } = new List<string>();
        public List<string> SubCategory { get; set; } = new List<string>();
        public int[] Poly { get; set; }
        public string MarketCapRank { get; set; }
        public string CirculationSupply { get; set; }
        public string MaxSupply { get; set; }
        public string TotalSupply { get; set; }
        public string IssueDate { get; set; }
        public string IssuePrice { get; set; }
        public string Details { get; set; }
        public string Network { get; set; }
        public string ContractAddress { get; set; }

    }
    public class vmCurrencyOptions
    {
        public List<string> CurrencyNames { get; set; }
        public string SelectedCurrency { get; set; }
    }
    public class vmDashboard : vmBase
    {
        public string name { get; set; }
        public vmWalletSummery WSummery { get; set; } = new vmWalletSummery();
        public vmWalletTransactions WTrans { get; set; } = new vmWalletTransactions();
    }

    public class vmFAQDisplay : vmBase
    {
        public Dictionary<string, List<mFAQDisplay>> FAQs { get; set; }
    }
    public class vmWalletProfile
    {
        public string Name { get; set; }
        public string AccountNo { get; set; }
        public eeKYCStatus KYCStatus { get; set; }
        public string LastLogin { get; set; }

    }

    public class vmWalletBase : vmBase
    {
        /// <summary>
        /// Sum of USD value of all tokens in this wallet
        /// </summary>
        public double AccountBalance_baseV { get; set; }
        /// <summary>
        /// Sum of USD value of all tokens in this wallet
        /// </summary>
        public double AccountBalance_CryotoV { get; set; }
        /// <summary>
        /// Diplay Value Coin
        /// </summary>
        public string selectedCryptoCoin { get; set; }
        public List<mToken2> TokenList { get; set; }
        
    }
    public class _vmWallet
    {
        public double tokenPrecision { get; set; } = 6;
    }
    public class vmWalletAsset : _vmWallet
    {
        public string Code { get; set; }
        public string ShortName { get; set; }
        public CoinType cType { get; set; }
        /// <summary>
        /// Token Quantity
        /// </summary>
        public double Amt { get; set; }
        /// <summary>
        /// USDT value of the Token
        /// </summary>
        public double BaseVal { get; set; }
        public bool IsFiat { get; set; }
    }

    public class vmWalletHome : vmWalletBase
    {
        public vmWalletProfile UserDetails { get; set; }
        public List<vmWalletAsset> Assets { get; set; } //Asset | Amount
        /// <summary>
        /// Cashback Records for Trades
        /// </summary>
        public List<mAccruedCashBack> MyCashBack { get; set; }
        public List<mMyReward> MyRewards{ get; set; }

    }
    public class vmWalletHomeClient
    {
        public string selectedCryptoCoin { get; set; }
        public List<vmWalletAsset> Assets { get; set; } //Asset | Amount

    }
    public class vmFundingWallet : vmWalletBase
    {
        public vmWalletProfile UserDetails { get; set; }//USername | Last Login | KYCStatus
        public List<vmFundingWalletTokenDetail> Details { get; set; }
    }
    public class vmEarnWallet : vmWalletBase
    {
        public vmWalletProfile UserDetails { get; set; }//USername | Last Login | KYCStatus
        public List<vmEarnWalletTokenDetail> Details { get; set; }
        public List<mStakingSlot2> StakeOpportunity { get; set; }
        public List<mStake> Stakings { get; set; }
    }

    public class vmSpotWallet : vmWalletBase
    {
        public vmWalletProfile UserDetails { get; set; }//USername | Last Login | KYCStatus
        public List<vmSpotWalletTokenDetail> Details { get; set; }
        //  public vmPaginationStrip PaginationStrip { get; set; }
    }
    public class vmOrders : vmBase
    {
        public List<rOrder> myOrders { get; set; }

    }
    public class vmTrades : vmBase
    {
        public List<mTrade> myTrades { get; set; }

    }

    public class vmWalletFundTransfer : vmBase
    {
        public int fromWalletType { get; set; }
        public int toWalletType { get; set; }
        public List<mToken> TokenList { get; set; }
        public Guid? selectedCoin { get; set; } //Diplay Value Coin
        public string Code { get; set; }
        public double? Amount { get; set; }
        public bool Status { get; set; }
    }

    public class vmWithdrawConfirmation : vmBase
    {
        // public mFiatWithdrawRequest FiatRequest { get; set; }
        public mWithdrawRequestResult Result { get; set; }
    }
    public class vmWithdraw : vmBase
    {
        public string isBack { get; set; }
        public bool AddToAddressBook { get; set; }
        public bool IsToken { get; set; }
        public bool IsAll { get; set; }//All Amount of this Token across all wallet
        public List<mToken> TokenList { get; set; }
        public mToken Token { get; set; }
        public List<mToken3> CurrencyList { get; set; }
        public List<Model.mSupportedNetwork> SupportedNetwork { get; set; }
        public Guid? selectedCoin { get; set; } //Diplay Value Coin
        public Guid? selectedNetwork { get; set; } //Diplay Value Coin
        public string? ReceiverAddr { get; set; }
        public double AddBalance { get; set; }
        public double WBalance { get; set; }
        //For Currency Token
        public string? RefNarration { get; set; }
        public double FiatWithdrawFee { get; set; }
        public string inforBag { get; set; }
        public bool IsFiat { get; set; }
        public bool IsBankDeposits { get; set; }
        public bool IsUPI { get; set; }
        public string ActionMethod { get; set; }
        public double Amount { get; set; }
        public string Symbole { get; set; }
        public InfoBag IB { get; set; }
        public IFormFile Receipt { get; set; }
        public string? TransferMethod { get; set; }
    }
    public class mWithdrawNetBag
    {
        public double Amount { get; set; }
        public mTokenNetworkFee NetFee { get; set; }
        public mToken Token { get; set; }
        public bool IsAll { get; set; }
        public string ReceiverAddr { get; set; }
        public bool AddToAddressBook { get; set; }
        public Guid? selectedNetwork { get; set; } //Diplay Value Coin
    }
    public class mWithDrawBag
    {
        public bool IsUPI { get; set; }
        public bool IsBank { get; set; }
        public bool IsToken { get; set; }
        public string infoBag { get; set; }
    }
    public class mWithdrawINRBankBag
    {
        public double Amount { get; set; }
        public Guid TokenId { get; set; }
        public string TokenName { get; set; }
        public string Symbol { get; set; }
        public mBankDepositPaymentMethod TechnoAppBankAccount { get; set; }
        public mINRBankDeposit UserBankAccount { get; set; }
        public string RefNarration { get; set; }
    }
    public class mWithdrawINRUPIBag
    {
        public double Amount { get; set; }
        public Guid TokenId { get; set; }
        public string TokenName { get; set; }
        public string Symbol { get; set; }
        public mUPIPaymentMethod TechnoAppUPI { get; set; }
        public mINRUPI UserUPIAcc { get; set; }
        public string RefNarration { get; set; }
    }
    public class vmClaimTranDeposit : vmBase
    {
        public string TranHash { get; set; }
        public IEnumerable<mSupportedNetwork> NetworkList { get; set; }
        public Guid NetworkId { get; set; }
        public string Msg { get; set; }
        public vmTranStatus status { get; set; }
        public string AssetCode { get; set; }
        public double Amount { get; set; }
    }
    public enum vmTranStatus
    {
        NewClaim = 0, ClaimAccepted = 1, ClaimAlreadyActioned,
    }
    public class vmDeposit : vmBase
    {
        public string isBack { get; set; }
        public List<mToken> TokenList { get; set; }
        public List<mToken3> CurrencyList { get; set; }
        public Guid? selectedCoin { get; set; } //Diplay Value Coin
        public Guid? selectedNetwork { get; set; } //Diplay Value Coin
        //For Currency Token
        public string inforBag { get; set; }
        public bool IsFiat { get; set; }
        public bool IsBankDeposits { get; set; }
        public bool IsUPI { get; set; }
        public string ActionMethod { get; set; }
        public double Amount { get; set; }
        public string Symbole { get; set; }
        public InfoBag IB { get; set; }
        public IFormFile Receipt { get; set; }
        public string? TransferMethod { get; set; }
    }
    public class InfoBag
    {
        public mTokenNetworkFee NetFee { get; set; }
        /// <summary>
        /// TechnoApp INR Bank Account/Payment Options
        /// </summary>
        public mINRDepositOption INRTechnoAppOption { get; set; }
        /// <summary>
        /// User Bank & UPI Accounts
        /// </summary>
        public mPaymentDeposits INRUserOptions { get; set; }
    }
    public class mINRDepositOption
    {
        public mBankDepositPaymentMethod selectedBankDeposits { get; set; }
        public List<mBankDepositPaymentMethod> BankDeposits { get; set; }
        public mUPIPaymentMethod selectedUPI { get; set; }
        public List<mUPIPaymentMethod> UPI { get; set; }
    }
    public class vmConvert : vmBase
    {
        public IEnumerable<mToken> TokenList { get; set; }
       // public IEnumerable<mToken> CurrencyList { get; set; }
        // public List<mToken3> CurrencyList { get; set; }
        public Guid? selectedCoin { get; set; } //Diplay Value Coin
        public Guid? PaySelectedCoin { get; set; } //Diplay Value Coin
        public mToken Token { get; set; }
        public mToken PayToken { get; set; }
        public double BuyAmount { get; set; }
        public double PayAmount { get; set; }
        public double AvailablePayAmount { get; set; }
        public double AddBalance { get; set; }
        public bool IsAll { get; set; }
        public double ValueOfOne { get; set; }
        public double MinBuyAmt { get; set; }
    }
    public class vmBuy : vmBase
    {
        public IEnumerable<mToken> TokenList { get; set; }
      //  public List<mWalletCoin> CurrencyList { get; set; }
       public List<mToken3> CurrencyList { get; set; }
        public Guid? selectedCoin { get; set; } //Diplay Value Coin
        public mToken Token { get; set; }
        public mToken3 PayToken { get; set; }
        public Guid? PaySelectedCoin { get; set; } //Diplay Value Coin
        public double BuyAmount { get; set; }
        public double PayAmount { get; set; }
        public double AvailablePayAmount { get; set; }
        public double ValueOfOne { get; set; }
        public double MinBuyAmt { get; set; }
    }
    public class vmPaginationStrip
    {
        public int RecordsPerPage { get; set; }//i.e 10
        public vmPaginationPage Previous { get; set; }
        public vmPaginationPage Next { get; set; }
        public vmPaginationPage Current { get; set; }
        public List<vmPaginationPage> Pages { get; set; }
    }
    public class vmPaginationPage
    {
        public string Display { get; set; }
        public string CommandLink { get; set; }
    }
    public class vmSpotWalletTokenDetail : _vmWallet
    {
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string ImgLocation { get; set; }
        public bool Active { get; set; }//If Trading is Allowed in Exchange for this Token
        public double TotalAmount { get; set; }
        public double AvailableAmount { get; set; }
        public double OpenOrderAmount { get; set; }
        /// <summary>
        /// USDT value of the single unit of this Token
        /// </summary>
        public double UnitBaseVal { get; set; }
        //ToDo: Action Command Links
        //ToDo: Graph Generation
        public string PolyPoints { get; set; }// 00,00 20,20...
    }
    public class vmEarnWalletTokenDetail : _vmWallet
    {
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string ImgLocation { get; set; }
        public bool Active { get; set; }//If Trading is Allowed in Exchange for this Token
        public double TotalAmount { get; set; }
        public double AvailableAmount { get; set; }
        public double OpenOrderAmount { get; set; }
        /// <summary>
        /// USDT value of the single unit of this Token
        /// </summary>
        public double UnitBaseVal { get; set; }
        //ToDo: Action Command Links
        //ToDo: Graph Generation
        public string PolyPoints { get; set; }// 00,00 20,20...
    }
    public class vmFundingWalletTokenDetail : _vmWallet
    {
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string ImgLocation { get; set; }
        public bool Active { get; set; }//If Trading is Allowed in Exchange for this Token
        public double TotalAmount { get; set; }
        public double AvailableAmount { get; set; }
        public double OpenOrderAmount { get; set; }
        /// <summary>
        /// USDT value of the single unit of this Token
        /// </summary>
        public double UnitBaseVal { get; set; }
        //ToDo: Action Command Links
        //ToDo: Graph Generation
        public string PolyPoints { get; set; }// 00,00 20,20...
    }
    public class vmWalletSummery
    {
        public Tuple<string, string, double> SpotTokens { get; set; }
        public Tuple<string, string, double> FundTokens { get; set; }
        public Tuple<string, string, double> EarnTokens { get; set; }
        //public List<mWalletSummery> WalletList { get; set; } = new List<mWalletSummery>();
        public double TotalBalance { get; set; }
    }

    public class vmWalletTransactions : vmWalletBase
    {
        public vmWalletProfile UserDetails { get; set; }//USername | Last Login | KYCStatus

        public List<mWalletTransactions> WalletTransactions { get; set; }
    }
    public enum CoinType
    {
        Crpto = 0, Fiat = 1
    }
}
