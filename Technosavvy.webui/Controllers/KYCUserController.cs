using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechnoApp.Ext.Web.UI.Extentions;
namespace TechnoApp.Ext.Web.UI.Controllers
{
    [AfterLogIn]
    [DenyAccessInPreBeta]
    [EnsureCompliantCountry]
    public class KYCUserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IOptions<SmtpConfig> _smtp;
        private IConfiguration _configuration;
        private HttpContext _context;
        private IHttpContextAccessor _accessor;
        IDataProtector _protector;
        AppSessionManager appSessionManager = null;

        public KYCUserController(ILogger<HomeController> logger, IOptions<SmtpConfig> smtp, IConfiguration configuration, IHttpContextAccessor accessor, IDataProtectionProvider provider, ITempDataDictionaryFactory tddf)
        {
            _logger = logger;
            _smtp = smtp;
            _configuration = configuration;
            _accessor = accessor;
            _protector = provider.CreateProtector("TechnoApp.HomeController.v1");
            appSessionManager = new AppSessionManager(accessor.HttpContext);
        }

        [HttpGet]
        [AfterProfile]
        public async Task<IActionResult> Index()
        {

            var mm = GetKYCManager();
            var vm = vmFactory.GetvmProfile(appSessionManager);
            try
            {
                vm.lstCountries = await mm.GetvmCountriesList();
                var m = await mm.GetMyProfile();
                vm.LoadFrom(m);
                return View("Index", vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UserKYC Error:{ex.GetDeepMsg()}");
                return View("Index", vm);
            }
        }
        [HttpPost]
        public async Task<IActionResult> MinDateOfBirth(vmProfile vm)
        {
            bool result = false;
            if (vm.DateOfBirth <= DateTime.UtcNow.AddYears(-18))
                result = true;
            // Console.WriteLine($"MinDate Validation is:{result}");
            return Json(result);
        }
        [HttpGet]
        [ActionName("User-Profile")]
        public async Task<IActionResult> UserMinProfile()
        {
            var mm = GetKYCManager();
            var vm = vmFactory.GetvmProfile(appSessionManager);
            try
            {
                vm.lstCountries = await mm.GetvmCountriesList();
                var m = await mm.GetMyProfile();
                if (m.ProfileId == Guid.Empty)
                {
                    m.UserAccountId = appSessionManager.mySession.UserId.ToGuid();
                    m.DateOfBirth = DateTime.UtcNow.AddYears(-18);
                }
                vm.LoadFrom(m);
                Console.WriteLine($"UserProfile [httpGet] Completed for User:{appSessionManager.mySession.UserName} at..{DateTime.UtcNow}");
                return View("UserMinProfile", vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UserProfile [httpGet] Error:{ex.GetDeepMsg()}");
                return View("UserMinProfile", vm);
            }
        }
        [HttpGet]
        [ActionName("Start-KYC")]
        [AfterProfile]
        public async Task<IActionResult> StartKYC()
        {
            var mm = GetKYCManager();
            var vm = vmFactory.GetvmProfile(appSessionManager);
            try
            {
                vm.lstCountries = await mm.GetvmCountriesList();
                var m = await mm.GetMyProfile();
                vm.LoadFrom(m);
                return View("KycVerification", vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UserProfile [httpGet] Error:{ex.GetDeepMsg()}");
                return View("KycVerification", vm);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AfterProfile]
        [ActionName("Start-KYC")]
        public async Task<IActionResult> StartKYC(vmProfile vm)
        {
            var mm = GetKYCManager();
            // var vm = vmFactory.GetvmProfile(appSessionManager);
            try
            {
                vm.lstCountries = await mm.GetvmCountriesList();
                mm.ValidateForProfile(vm);
                var pro = await mm.UpdateProfilePersonalInfo(vm.ToModel());
                //Stage 2, for document upload
                vm.ProfileId = pro.ProfileId;
                vm.Address = pro.Address.ToVM();
                var vm2 = vmFactory.GetvmKYCDocUploadStage(appSessionManager);
                vm2.Profile = vm;
                vm.Address.Country = vm.lstCountries.First(x => x.countryId == vm.TaxResidencyId);
                vm2.DocInstances = await mm.GetvmKYCDocUploadStage2(vm.selectedTaxResidency.Abbri);
                ModelState.Clear();
                return View("KycUploadDocument", vm2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UserProfile [httpGet] Error:{ex.GetDeepMsg()}");
                ModelState.Clear();
                ModelState.AddModelError("", ex.GetDeepMsg());
                return View("KycVerification", vm);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AfterProfile]
        [ActionName("Finish-KYC")]
        public async Task<IActionResult> FinishKYC(vmKYCDocUploadStage vm)
        {
            var mm = GetKYCManager();
            try
            {
                await appSessionManager.ExtSession.LoadSession();
                vm.Profile = vm.Profile ?? new vmProfile();
                vm.Profile.ProfileId = appSessionManager.ExtSession.myUS.UserAccount.Profile.ProfileId;
                vm.Profile.UserAccountId = appSessionManager.ExtSession.myUS.UserAccount.Profile.UserAccountId;

                var vm2 = vmFactory.GetvmKYCDocUploadStage(appSessionManager);
                vm2.Profile = vm.Profile;
                vm2.DocInstances = vm.DocInstances;
                vm = vm2;
                vm.DocInstances = mm.ValidateDocInstances(vm.DocInstances);
                var result = mm.SaveDocInstances(vm);
                return View("KycUploadConfirm", await vmFactory.GetvmBase(appSessionManager));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FinishKYC Error:{ex.GetDeepMsg()}");
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return View("KycUploadDocument", vm);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("User-Profile")]
        public async Task<IActionResult> UserMinProfile(vmProfile vm)
        {
            var mm = GetKYCManager();
            bool shouldRedirect = false;
            try
            {
                if (mm.ValidateForMiniProfile(vm))
                {
                    if (vm.ProfileId == Guid.Empty)
                    {
                        await mm.CreateProfile(vm.ToModel());
                        mm.LogEvent("Minimum Profile created");
                        shouldRedirect = true;
                    }
                    else
                    {
                        await mm.UpdateProfilePersonalInfo(vm.ToModel());
                        shouldRedirect = true;
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect / Invalid profile details");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Occoured in UserMinProfile..at:{DateTime.UtcNow.GetCurrentUnix()}\n\t{ex.GetDeepMsg()}");
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
            }
            if (shouldRedirect)
                return RedirectToAction("index", "wallet");
            else
            {
                vm.lstCountries = await mm.GetvmCountriesList();
                return View("UserMinProfile", vm);
            }
        }

        //add by kavita


        [HttpGet]
        public async Task<IActionResult> KycUploadDocument(vmProfile profile)
        {
            //await EnsureValidSession();
            var vm = new vmKYCDocUploadStage();
            return View("KycUploadDocument", vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KycUploadDocument(vmKYCDocUploadStage vm)
        {
            try
            {
                //ToDo: Save uploaded documents 

                return View("KycUploadDocument", vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"KycUploadDocument Error:{ex.GetDeepMsg()}");
                ModelState.Clear();
                ModelState.AddModelError("", ex.GetDeepMsg());
                return View("KycUploadDocument", vm);
            }
        }
        public async Task<IActionResult> KycVerification()
        {
            //await EnsureValidSession();
            var vm = await vmFactory.GetvmBase(appSessionManager);
            return View("KycVerification", vm);
        }


        private KYCManager GetKYCManager()
        {
            // await EnsureValidSession();

            var Mgr = new KYCManager();
            Mgr._configuration = _configuration;
            Mgr._http = _accessor.HttpContext;
            Mgr._appSessionManager = appSessionManager;
            return Mgr;
        }
    }
}
