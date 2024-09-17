using Microsoft.AspNetCore.Mvc;

namespace TechnoApp.Ext.Web.UI.Controllers;

public class t6Controller : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IOptions<SmtpConfig> _smtp;
    private IConfiguration _configuration;
    private HttpContext _context;
    private IHttpContextAccessor _accessor;
    IDataProtector _protector;
    AppSessionManager appSessionManager = null;
    public t6Controller(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
    {
        _logger = logger;
        _smtp = smtp;
        _configuration = configuration;
        _accessor = accessor;
        _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
        appSessionManager = new AppSessionManager(accessor.HttpContext);
        //should it working this timne

    }
    [HttpGet]
    public async Task<IActionResult> List()
    {
       // var vm = vmFactory.GetvmUserLogin(appSessionManager);
        return View("List");
    }

    [HttpGet]
    [ActionName("academy-detail")]
    public async Task<IActionResult> academydetail()
    {
        return View("academy-detail");
    }
    [HttpGet]
    [ActionName("account-activity")]
    public async Task<IActionResult> accountactivity()
    {
        return View("account-activity");
    }
    [HttpGet]
    [ActionName("account-statement")]
    public async Task<IActionResult> accountstatement()
    {
        return View("account-statement");
    }
    [HttpGet]
    [ActionName("account-verify")]
    public async Task<IActionResult> accountverify()
    {
        return View("account-verify");
    }
    [HttpGet]
    [ActionName("addbankaccount")]
    public async Task<IActionResult> addbankaccount()
    {
        return View("addbankaccount");
    }
    [HttpGet]
    [ActionName("address-management-detail")]
    public async Task<IActionResult> addressmanagementdetail()
    {
        return View("address-management-detail");
    }
    [HttpGet]
    [ActionName("address-management")]
    public async Task<IActionResult> addressmanagement()
    {
        return View("address-management");
    }
    [HttpGet]
    [ActionName("am-securityverification")]
    public async Task<IActionResult> amsecurityverification()
    {
        return View("am-securityverification");
    }
    [HttpGet]
    [ActionName("bankaccount")]
    public async Task<IActionResult> bankaccount()
    {
        return View("bankaccount");
    }
    [HttpGet]
    [ActionName("bankaccountdetail")]
    public async Task<IActionResult> bankaccountdetail()
    {
        return View("bankaccountdetail");
    }
    [HttpGet]
    [ActionName("bankaccountdetailconfirmed")]
    public async Task<IActionResult> bankaccountdetailconfirmed()
    {
        return View("bankaccountdetailconfirmed");
    }
    [HttpGet]
    [ActionName("bankaccountedit")]
    public async Task<IActionResult> bankaccountedit()
    {
        return View("bankaccountedit");
    }
    [HttpGet]
    [ActionName("bankconfirmation")]
    public async Task<IActionResult> bankconfirmation()
    {
        return View("bankconfirmation");
    }
    [HttpGet]
    [ActionName("bankpending")]
    public async Task<IActionResult> bankpending()
    {
        return View("bankpending");
    }
    [HttpGet]
    [ActionName("basic-information")]
    public async Task<IActionResult> basicinformation()
    {
        return View("basic-information");
    }
    [HttpGet]
    [ActionName("buy-confirmation")]
    public async Task<IActionResult> buyconfirmation()
    {
        return View("buy-confirmation");
    }
    [HttpGet]
    [ActionName("buy-crypto")]
    public async Task<IActionResult> buycrypto()
    {
        return View("buy-crypto");
    }
    [HttpGet]
    [ActionName("buy-cryptosummary")]
    public async Task<IActionResult> buycryptosummary()
    {
        return View("buy-cryptosummary");
    }
    [HttpGet]
    [ActionName("buy-cryptotimerscreen")]
    public async Task<IActionResult> buycryptotimerscreen()
    {
        return View("buy-cryptotimerscreen");
    }
    [HttpGet]
    [ActionName("cashback-main")]
    public async Task<IActionResult> cashbackmain()
    {
        return View("cashback-main");
    }
    [HttpGet]
    [ActionName("cashback")]
    public async Task<IActionResult> cashback()
    {
        return View("cashback");
    }
    [HttpGet]
    [ActionName("change-password")]
    public async Task<IActionResult> changepassword()
    {
        return View("change-password");
    }
    [HttpGet]
    [ActionName("convert")]
    public async Task<IActionResult> convert()
    {
        return View("convert");
    }
    [HttpGet]
    [ActionName("converttimerscreen")]
    public async Task<IActionResult> converttimerscreen()
    {
        return View("converttimerscreen");
    }
    [HttpGet]
    [ActionName("convertwithdraw")]
    public async Task<IActionResult> convertwithdraw()
    {
        return View("convertwithdraw");
    }
    [HttpGet]
    [ActionName("convertwithdrawfiat")]
    public async Task<IActionResult> convertwithdrawfiat()
    {
        return View("convertwithdrawfiat");
    }
    [HttpGet]
    [ActionName("convertwithdrawfiattimerscreen")]
    public async Task<IActionResult> convertwithdrawfiattimerscreen()
    {
        return View("convertwithdrawfiattimerscreen");
    }
    [HttpGet]
    [ActionName("convertwithdrawtimerscreen")]
    public async Task<IActionResult> convertwithdrawtimerscreen()
    {
        return View("convertwithdrawtimerscreen");
    }
    [HttpGet]
    [ActionName("convertwithdrawupi")]
    public async Task<IActionResult> convertwithdrawupi()
    {
        return View("convertwithdrawupi");
    }
    [HttpGet]
    [ActionName("country-residence")]
    public async Task<IActionResult> countryresidence()
    {
        return View("country-residence");
    }
    [HttpGet]
    [ActionName("create-personal-account")]
    public async Task<IActionResult> createpersonalaccount()
    {
        return View("create-personal-account");
    }
    [HttpGet]
    [ActionName("deposit-fiat-upi")]
    public async Task<IActionResult> depositfiatupi()
    {
        return View("deposit-fiat-upi");
    }
    [HttpGet]
    [ActionName("deposit-fiat")]
    public async Task<IActionResult> depositfiat()
    {
        return View("deposit-fiat");
    }
    [HttpGet]
    [ActionName("deposit")]
    public async Task<IActionResult> deposit()
    {
        return View("deposit");
    }
    [HttpGet]
    [ActionName("device-management")]
    public async Task<IActionResult> devicemanagement()
    {
        return View("device-management");
    }
    [HttpGet]
    [ActionName("earn-wallet")]
    public async Task<IActionResult> earnwallet()
    {
        return View("earn-wallet");
    }
    [HttpGet]
    [ActionName("entity-account")]
    public async Task<IActionResult> entityaccount()
    {
        return View("entity-account");
    }
    [HttpGet]
    [ActionName("entity-summary")]
    public async Task<IActionResult> entitysummary()
    {
        return View("entity-summary");
    }
    [HttpGet]
    [ActionName("fees-charges")]
    public async Task<IActionResult> feescharges()
    {
        return View("fees-charges");
    }
    [HttpGet]
    [ActionName("funding-wallet")]
    public async Task<IActionResult> fundingwallet()
    {
        return View("funding-wallet");
    }
    [HttpGet]
    [ActionName("Index-au")]
    public async Task<IActionResult> Indexau()
    {
        return View("Index-au");
    }
    [HttpGet]
    [ActionName("Index-india")]
    public async Task<IActionResult> Indexindia()
    {
        return View("Index-india");
    }
    [HttpGet]
    [ActionName("Index-svg")]
    public async Task<IActionResult> Indexsvg()
    {
        return View("Index-svg");
    }
    [HttpGet]
    [ActionName("Index-us")]
    public async Task<IActionResult> Indexus()
    {
        return View("Index-us");
    }
    [HttpGet]
    [ActionName("Index")]
    public async Task<IActionResult> Index()
    {
        return View("Index");
    }
    [HttpGet]
    [ActionName("index1complete-profile")]
    public async Task<IActionResult> index1completeprofile()
    {
        return View("index1complete-profile");
    }
    [HttpGet]
    [ActionName("internaltransfer-2Fa")]
    public async Task<IActionResult> internaltransfer2Fa()
    {
        return View("internaltransfer-2Fa");
    }
    [HttpGet]
    [ActionName("internaltransfer-confirmation")]
    public async Task<IActionResult> internaltransferconfirmation()
    {
        return View("internaltransfer-confirmation");
    }
    [HttpGet]
    [ActionName("internaltransfer-summary")]
    public async Task<IActionResult> internaltransfersummary()
    {
        return View("internaltransfer-summary");
    }
    [HttpGet]
    [ActionName("internaltransfer")]
    public async Task<IActionResult> internaltransfer()
    {
        return View("internaltransfer");
    }
    [HttpGet]
    [ActionName("kyc-appendic")]
    public async Task<IActionResult> kycappendic()
    {
        return View("kyc-appendic");
    }
    [HttpGet]
    [ActionName("kyc-personal-verification-upload")]
    public async Task<IActionResult> kycpersonalverificationupload()
    {
        return View("kyc-personal-verification-upload");
    }
    [HttpGet]
    [ActionName("kyc-personal-verification")]
    public async Task<IActionResult> kycpersonalverification()
    {
        return View("kyc-personal-verification");
    }
    [HttpGet]
    [ActionName("kyc-upload-sefie")]
    public async Task<IActionResult> kycuploadsefie()
    {
        return View("kyc-upload-sefie");
    }
    [HttpGet]
    [ActionName("license-nc")]
    public async Task<IActionResult> licensenc()
    {
        return View("license-nc");
    }
    [HttpGet]
    [ActionName("list-aggrement")]
    public async Task<IActionResult> listaggrement()
    {
        return View("list-aggrement");
    }
    [HttpGet]
    [ActionName("market-complete-profile")]
    public async Task<IActionResult> marketcompleteprofile()
    {
        return View("market-complete-profile");
    }
    [HttpGet]
    [ActionName("order-history")]
    public async Task<IActionResult> orderhistory()
    {
        return View("order-history");
    }
    [HttpGet]
    [ActionName("p2p-order")]
    public async Task<IActionResult> p2porder()
    {
        return View("p2p-order");
    }
    [HttpGet]
    [ActionName("personal-identity")]
    public async Task<IActionResult> personalidentity()
    {
        return View("personal-identity");
    }
    [HttpGet]
    [ActionName("redeemconfirm-eth")]
    public async Task<IActionResult> redeemconfirmeth()
    {
        return View("redeemconfirm-eth");
    }
    [HttpGet]
    [ActionName("redeemconfirm")]
    public async Task<IActionResult> redeemconfirm()
    {
        return View("redeemconfirm");
    }
    [HttpGet]
    [ActionName("register-business")]
    public async Task<IActionResult> registerbusiness()
    {
        return View("register-business");
    }
    [HttpGet]
    [ActionName("related-parties")]
    public async Task<IActionResult> relatedparties()
    {
        return View("related-parties");
    }
    [HttpGet]
    [ActionName("reward-center")]
    public async Task<IActionResult> rewardcenter()
    {
        return View("reward-center");
    }
    [HttpGet]
    [ActionName("security-verificationgoogle")]
    public async Task<IActionResult> securityverificationgoogle()
    {
        return View("security-verificationgoogle");
    }
    [HttpGet]
    [ActionName("security")]
    public async Task<IActionResult> security()
    {
        return View("security");
    }
    [HttpGet]
    [ActionName("spot-wallet")]
    public async Task<IActionResult> spotwallet()
    {
        return View("spot-wallet");
    }
    [HttpGet]
    [ActionName("spotopen-orders")]
    public async Task<IActionResult> spotopenorders()
    {
        return View("spotopen-orders");
    }
    [HttpGet]
    [ActionName("staking")]
    public async Task<IActionResult> staking()
    {
        return View("staking");
    }
    [HttpGet]
    [ActionName("stakingconfirmation")]
    public async Task<IActionResult> stakingconfirmation()
    {
        return View("stakingconfirmation");
    }
    [HttpGet]
    [ActionName("stakingconfirmationTechnoSavvy")]
    public async Task<IActionResult> stakingconfirmationTechnoSavvy()
    {
        return View("stakingconfirmationTechnoSavvy");
    }
    [HttpGet]
    [ActionName("stakingstart")]
    public async Task<IActionResult> stakingstart()
    {
        return View("stakingstart");
    }
    [HttpGet]
    [ActionName("stakingsummary")]
    public async Task<IActionResult> stakingsummary()
    {
        return View("stakingsummary");
    }
    [HttpGet]
    [ActionName("stakingsummaryTechnoSavvy")]
    public async Task<IActionResult> stakingsummaryTechnoSavvy()
    {
        return View("stakingsummaryTechnoSavvy");
    }
    [HttpGet]
    [ActionName("token-compliance")]
    public async Task<IActionResult> tokencompliance()
    {
        return View("token-compliance");
    }
    [HttpGet]
    [ActionName("token-infomation")]
    public async Task<IActionResult> tokeninfomation()
    {
        return View("token-infomation");
    }
    [HttpGet]
    [ActionName("token-legal")]
    public async Task<IActionResult> tokenlegal()
    {
        return View("token-legal");
    }
    [HttpGet]
    [ActionName("token-security")]
    public async Task<IActionResult> tokensecurity()
    {
        return View("token-security");
    }
    [HttpGet]
    [ActionName("token-submit")]
    public async Task<IActionResult> tokensubmit()
    {
        return View("token-submit");
    }
    [HttpGet]
    [ActionName("tokeninfo")]
    public async Task<IActionResult> tokeninfo()
    {
        return View("tokeninfo");
    }
    [HttpGet]
    [ActionName("trade-history")]
    public async Task<IActionResult> tradehistory()
    {
        return View("trade-history");
    }
    [HttpGet]
    [ActionName("trading-rules-au")]
    public async Task<IActionResult> tradingrulesau()
    {
        return View("trading-rules-au");
    }
    [HttpGet]
    [ActionName("trading-rules-us")]
    public async Task<IActionResult> tradingrulesus()
    {
        return View("trading-rules-us");
    }
    [HttpGet]
    [ActionName("transaction")]
    public async Task<IActionResult> transaction()
    {
        return View("transaction");
    }
    [HttpGet]
    [ActionName("twofactor-verification-withdraw")]
    public async Task<IActionResult> twofactorverificationwithdraw()
    {
        return View("twofactor-verification-withdraw");
    }
    [HttpGet]
    [ActionName("twofactor-verification")]
    public async Task<IActionResult> twofactorverification()
    {
        return View("twofactor-verification");
    }
    [HttpGet]
    [ActionName("twofactor-verificationbank")]
    public async Task<IActionResult> twofactorverificationbank()
    {
        return View("twofactor-verificationbank");
    }
    [HttpGet]
    [ActionName("twofactorauthenication")]
    public async Task<IActionResult> twofactorauthenication()
    {
        return View("twofactorauthenication");
    }
    [HttpGet]
    [ActionName("unstak-eth")]
    public async Task<IActionResult> unstaketh()
    {
        return View("unstak-eth");
    }
    [HttpGet]
    [ActionName("unstak-TechnoSavvy")]
    public async Task<IActionResult> unstakTechnoSavvy()
    {
        return View("unstak-TechnoSavvy");
    }
    [HttpGet]
    [ActionName("upiadddetail-confirmed")]
    public async Task<IActionResult> upiadddetailconfirmed()
    {
        return View("upiadddetail-confirmed");
    }
    [HttpGet]
    [ActionName("upiadddetail-pending")]
    public async Task<IActionResult> upiadddetailpending()
    {
        return View("upiadddetail-pending");
    }
    [HttpGet]
    [ActionName("upiadddetail")]
    public async Task<IActionResult> upiadddetail()
    {
        return View("upiadddetail");
    }
    [HttpGet]
    [ActionName("UPIchange")]
    public async Task<IActionResult> UPIchange()
    {
        return View("UPIchange");
    }
    [HttpGet]
    [ActionName("uploaded-document")]
    public async Task<IActionResult> uploadeddocument()
    {
        return View("uploaded-document");
    }
    [HttpGet]
    [ActionName("user-profile-2")]
    public async Task<IActionResult> userprofile2()
    {
        return View("user-profile-2");
    }
    [HttpGet]
    [ActionName("user-profile")]
    public async Task<IActionResult> userprofile()
    {
        return View("user-profile");
    }
    [HttpGet]
    [ActionName("wallet-2")]
    public async Task<IActionResult> wallet2()
    {
        return View("wallet-2");
    }
    [HttpGet]
    [ActionName("wallet-3")]
    public async Task<IActionResult> wallet3()
    {
        return View("wallet-3");
    }
    [HttpGet]
    [ActionName("wallet")]
    public async Task<IActionResult> wallet()
    {
        return View("wallet");
    }
    [HttpGet]
    [ActionName("withdraw-fiat")]
    public async Task<IActionResult> withdrawfiat()
    {
        return View("withdraw-fiat");
    }
    [HttpGet]
    [ActionName("withdraw-upi")]
    public async Task<IActionResult> withdrawupi()
    {
        return View("withdraw-upi");
    }
    [HttpGet]
    [ActionName("withdraw")]
    public async Task<IActionResult> withdraw()
    {
        return View("withdraw");
    }
}
