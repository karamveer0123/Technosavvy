using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;
using TechnoApp.Ext.Web.UI.Models;
using TechnoApp.Ext.Web.UI.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.CodeAnalysis;
using TechnoApp.Ext.Web.UI.Extentions;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.Extensions;

namespace TechnoApp.Ext.Web.UI.Manager
{

    public class AppSessionManager
    {
        private string sessionCookie = "TechnoApp";
        internal HttpContext _httpContext;
        // private readonly ITempDataDictionaryFactory TDDF = null;
        public AppSessionManager(HttpContext _ctx)
        {
            _httpContext = _ctx;
            _mySession = new AppSession();

            LoadState();
            ExtSession = new AppSessionExt<AppSession>(_mySession, this);

        }
        public AppSessionExt<AppSession> ExtSession { get; set; }
        private AppSession _mySession;

        public AppSession mySession
        {
            get { DoLoad(); return _mySession; }
            private set { _mySession = value; }
        }
        public bool VerifyCurrency(string cur)
        {
            return SrvCurrencyPriceHUB.GetAllCurrenciesName().Contains(cur);
        }
        public bool AddToWatchList(string mCode)
        {
            if (mCode.IsNullOrEmpty()) return false;
            var lst = mySession.WatchList;
            var a = lst.FirstOrDefault(x => x.ToLower() == mCode.ToLower());
            if (a.IsNullOrEmpty())
            {
                lst.Add(mCode);
                mySession.WatchList = lst;
                ExtSession.RollTokenWatchListCounter(mCode, 1).GetAwaiter().GetResult();
            }
            return true;
        }
        public bool RemoveFromWatchList(string mCode)
        {
            if (mCode.IsNullOrEmpty()) return false;
            var lst = mySession.WatchList;
            var a = lst.FirstOrDefault(x => x.ToLower() == mCode.ToLower());
            if (a.IsNOT_NullorEmpty())
            {
                lst.Remove(a);
                mySession.WatchList = lst;
                ExtSession.RollTokenWatchListCounter(mCode, -1).GetAwaiter().GetResult();
            }
            return true;
        }
        public bool AddToFavourite(string mCode)
        {
            if (mCode.IsNullOrEmpty()) return false;
            var lst = mySession.FavList;
            var a = lst.FirstOrDefault(x => x.ToLower() == mCode.ToLower());
            if (a.IsNullOrEmpty())
            {
                lst.Add(mCode);
                mySession.FavList = lst;
                ExtSession.RollTokenFavListCounter(mCode, 1).GetAwaiter().GetResult();
            }
            return true;
        }
        public bool RemoveFromFavourite(string mCode)
        {
            if (mCode.IsNullOrEmpty()) return false;
            var lst = mySession.FavList;
            var a = lst.FirstOrDefault(x => x.ToLower() == mCode.ToLower());
            if (a.IsNOT_NullorEmpty())
            {
                lst.Remove(a);
                mySession.FavList = lst;
                ExtSession.RollTokenFavListCounter(mCode, -1).GetAwaiter().GetResult();
            }
            return true;
        }
        public string GetState()
        {
            //retun Json object to be saved in database
            DoLoad();
            var retval = JsonSerializer.Serialize(_mySession);
            if (retval.Length >= 10000)
                retval = retval.Substring(0, 10000);

            return retval;
        }
        public string GetRefCodeIfAny()
        {

            _httpContext.Request.Headers.TryGetValue("uref", out var RefLink);
            if (RefLink.ToString().IsNOT_NullorEmpty())
                _mySession.RefCode = RefLink.ToString();
            return RefLink.ToString();
        }
        public bool SaveState()
        {
            bool retval = false;
            try
            {
                if (!IsLoading)
                {
                    _httpContext.Request.Headers["SessionToken"] = mySession.SessionHash;
                    SetSessionCookies();
                    retval = true;
                }
            }
            catch (Exception ex)
            {
                //todo: Santosh, Do error logging
            }
            return retval;
        }
        private void LoadState()
        {
            if (!IsLoading)
            {
                IsLoading = true;
                if (DoesHaveSessionCookie())
                {
                    LoadCookieSession();
                }
                else
                {
                    SetSessionCookies();
                }

                IsLoaded = true;
                IsLoading = false;
            }
        }
        private void DoLoad()
        {
            if (!IsLoaded) LoadState();
        }
        public bool IsLoaded { get; private set; }
        private bool IsLoading { get; set; }

        #region Loading & Saving Session State
        private bool DoesHaveSessionCookie()
        {
            return _httpContext.Request.Cookies.ContainsKey(sessionCookie);
        }

        internal CFGeoVm GetGeoLOcation()
        {
            CFGeoVm cFGeo = new CFGeoVm();
            if (!string.IsNullOrEmpty(_httpContext.Request.Headers["cf-ipcountry"]))
            {
                cFGeo.IP = _httpContext.Request.Headers["CF-Connecting-IP"];
                cFGeo.CountryCode = _httpContext.Request.Headers["CF-IPCountry"];
                cFGeo.City = _httpContext.Request.Headers["cf-ipcity"];
                cFGeo.Ipcontinent = _httpContext.Request.Headers["cf-ipcontinent"];
                cFGeo.Longitude = _httpContext.Request.Headers["cf-iplongitude"];
                cFGeo.Latitude = _httpContext.Request.Headers["cf-iplatitude"];
            }
            return cFGeo;

        }
        internal void LogOff()
        {
            var sess = new AppSession();
            sess.CookieConsent = mySession.CookieConsent;
            sess.Theme = mySession.Theme;
            sess.WatchList = mySession.WatchList;
            sess.LTUID = mySession.LTUID;
            sess.FavList = mySession.FavList;
            mySession = sess;
            SetSessionCookies();
            //  SaveState();
        }
        internal eThemeName SetDefaultTheme()
        {
            SetTheme(eThemeName.Dark);
            return GetTheme();

        }
        internal eThemeName SwitchTheme()
        {
            if (_mySession.Theme == "Dark")
                SetTheme(eThemeName.Light);
            else
                SetTheme(eThemeName.Dark);

            return GetTheme();
        }
        internal eThemeName GetTheme()
        {
            if (_mySession.Theme == "Light")
                return eThemeName.Light;
            else
                return eThemeName.Dark;
        }
        internal eThemeName SetTheme(eThemeName t)
        {
            _mySession.Theme = t.ToString();
            return GetTheme();
        }
        internal string SetCurrency(string t)
        {
            //if (VerifyCurrency(t))
            //{
            //    _mySession.Currency = t;
            //}
            _mySession.Currency = t;
            return _mySession.Currency;
        }
        internal async Task ReportEvent(string pg, string evName, string id, double sc = 0, double scH = 0)
        {
            var ev = new PageEventRecord();
            ev.At = DateTime.UtcNow;
            ev.City = _mySession.GIO.City;
            ev.PageInstanceId = id;
            ev.Country = mySession.GIO.CountryCode;
            ev.Scroll = sc;
            ev.ScreenHeight = scH;
            ev.Event = evName;
            ev.IP = _mySession.GIO.IP;
            ev.LTUID = _mySession.LTUID;
            ev.Page = pg;
            SrvPageEvents.ReportEvent(ev);
            await Task.CompletedTask;
        }
        private void LoadCookieSession()
        {
            var cookieSession = _httpContext.Request.Cookies[sessionCookie];
            AppSession udCookie = JsonSerializer.Deserialize<AppSession>(cookieSession);
            _mySession = udCookie;
        }
        private void SetSessionCookies()
        {
            CookieOptions cookieOptions = new CookieOptions()
            {
                Expires = new DateTimeOffset(2099, 1, 1, 0, 0, 0, TimeSpan.FromHours(0))
            };
            //Todo: naveen, we should also create a session for this user
            string userData = JsonSerializer.Serialize(_mySession);
            _httpContext.Response.Cookies.Append(sessionCookie, userData, cookieOptions);
        }

        #endregion
    }


    public class AfterProfileAttribute : TypeFilterAttribute
    {
        public AfterProfileAttribute() : base(typeof(AfterProfileFilter))
        {
        }
    }

    public class AfterLogInAttribute : TypeFilterAttribute
    {
        public AfterLogInAttribute() : base(typeof(AfterLogInFilter))
        {
        }
    }
    public class DenyAccessInPreBetaAttribute : TypeFilterAttribute
    {
        public DenyAccessInPreBetaAttribute() : base(typeof(DenyAccessInPreBetaFilter))
        {
        }
    }
    public class DenyAccessInPreBetaFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            if (ConfigEx.VersionType == versionType.PreBeta)
                context.Result = new RedirectToActionResult("nourl", "home", null);


            return;
        }
    }
    public class EnsureCompliantCountryAttribute : TypeFilterAttribute
    {
        public EnsureCompliantCountryAttribute() : base(typeof(AEnsureCompliantCountryFilter))
        {
        }
    }

    public class AEnsureCompliantCountryFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var _userSession = new AppSessionManager(context.HttpContext);
            // When even Logged in users are to be pushed out due to change country rules
            //_userSession.ExtSession.LoadSession().GetAwaiter().GetResult();
            var ct = _userSession.mySession.UserCountry;
            if (SrvPrebeta.CompliantCountry(ct))
                return;
            else
            {
                context.Result = new RedirectToActionResult("noservice", "home", null);

            }
            return;
        }
    }
    public class AfterProfileFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var _userSession = new AppSessionManager(context.HttpContext);
            _userSession.ExtSession.LoadSession().GetAwaiter().GetResult();
            if (!_userSession.ExtSession.IsValid)
            {
                Console.WriteLine($"NO Valid Session, Redirecting to Login at..{DateTime.UtcNow}");
                _userSession.mySession.oURL = "";//Only Refere should be used
                _userSession.mySession.oURL = context.HttpContext.Request.GetDisplayUrl(); ;
                context.Result = new RedirectToActionResult("login", "login", null);
                return;
            }
            if (_userSession.ExtSession?.UserSession?.UserAccount?.CitizensOf == null || _userSession.ExtSession?.UserSession?.UserAccount?.CitizensOf.Id == Guid.Empty.ToString())
            {
                Console.WriteLine($"No Profile, Redirecting to user-Profile at..{DateTime.UtcNow}");

                context.Result = new RedirectToActionResult("user-profile", "KYCUser", null);
                return;
            }
            if (_userSession.mySession.oURL.IsNOT_NullorEmpty())
            {
                context.Result = new RedirectToActionResult("index", "UserRef", null);
            }
            return;
        }
    }
    public class AfterLogInFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var _userSession = new AppSessionManager(context.HttpContext);
            _userSession.ExtSession.LoadSession().GetAwaiter().GetResult();
            if (!_userSession.ExtSession.IsValid)
            {
                Console.WriteLine($"NO Valid Session, Redirecting to Login at..{DateTime.UtcNow}");
                _userSession.mySession.oURL = "";
                _userSession.mySession.oURL = context.HttpContext.Request.GetDisplayUrl(); ;
                context.Result = new RedirectToActionResult("login", "login", null);
            }
            else
            {
                if (_userSession.mySession.oURL.IsNOT_NullorEmpty())
                {
                    context.Result = new RedirectToActionResult("UserRef", "index", null);
                }
            }

            return;
        }
    }
    public class AfterKYCFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var _userSession = new AppSessionManager(context.HttpContext);
            _userSession.ExtSession.LoadSession().GetAwaiter().GetResult();
            if (!_userSession.ExtSession.IsValid)
            {
                context.Result = new RedirectToActionResult("login", "login", null);
            }
            if (_userSession.ExtSession?.UserSession?.UserAccount?.CitizensOf == null)
            {
                context.Result = new RedirectToActionResult("user-profile", "KYCUser", null);
            }
            //ToDo: KYC Status Check
            if (_userSession.ExtSession?.UserSession?.UserAccount.IsPrimaryCompleted == null)
            {
                context.Result = new RedirectToActionResult("user-profile", "KYCUser", null);
            }
            return;
        }
    }

    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        private readonly int _Length;

        public AllowedExtensionsAttribute(string[] extensions, int Length)
        {
            _extensions = extensions;
            _Length = Length;
        }

        public override bool IsValid(object value)
        {
            if (value is null)
                return true;

            var file = value as IFormFile;
            if (file is null) return true;
            if (file.Length > _Length) return true;
            var extension = Path.GetExtension(file.FileName);

            if (!_extensions.Contains(extension.ToLower()))
                return false;

            return true;
        }
    }
    public class AllowedMaxSizeAttribute : ValidationAttribute
    {

        private readonly int _Length;
        public AllowedMaxSizeAttribute(int Length)
        {
            _Length = Length;
        }

        public override bool IsValid(object value)
        {
            if (value is null)
                return true;

            var file = value as IFormFile;
            if (file is null) return true;
            if (file.Length > _Length) return false;
            return true;
        }
    }
    //public static class PermissionExtension
    //{
    //    public static bool HavePermission(this Controller c, string claimValue)
    //    {
    //        var user = c.HttpContext.User as ClaimsPrincipal;
    //        bool havePer = user.HasClaim(claimValue, claimValue);
    //        return havePer;
    //    }
    //    public static bool HavePermission(this IIdentity claims, string claimValue)
    //    {
    //        var userClaims = claims as ClaimsIdentity;
    //        bool havePer = userClaims.HasClaim(claimValue, claimValue);
    //        return havePer;
    //    }
    //}
}