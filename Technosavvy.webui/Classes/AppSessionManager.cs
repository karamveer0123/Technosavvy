/*using System.Text.Json;

namespace TechnoApp.Ext.Web.UI.Classes;

public class AppSessionManager
{
    private string sessionCookie = "LTID";
    private string refCookie = "RefTTRI";
    HttpContext _httpContext;
    private readonly ITempDataDictionaryFactory TDDF = null;
    public AppSessionManager(HttpContext _ctx, ITempDataDictionaryFactory _tddf)
    {
        _httpContext = _ctx;
        _mySession = new AppSession();
        TDDF = _tddf;
        _mySession.SetManager(this);
        LoadState();
    }


    private AppSession _mySession;

    public AppSession mySession
    {
        get { DoLoad(); return _mySession; }
        private set { _mySession = value; }
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
    public bool SaveState()
    {
        bool retval = false;
        try
        {
            GetCookieMsgSetOff();
            GetJumboMsgSetOff();
            SetUserName();
            CalculateWallet();
            if (!IsLoading)
            {
                var str = GetState();
                var s = dbCtx.SessionAttributes.First(x => x.UserSessionId == mySession.Session.UserSessionId);
                s.SessionState = str;
                dbCtx.SaveChanges();
                retval = true;
            }
        }
        catch (Exception ex)
        {
            //todo: Santosh, Do error logging
        }
        return retval;
    }
    public void LoadState()
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
                CreateAndSaveCookieSession();
                // LoadCookieSession();

            }
            SetPageRequestData1();
            ExtendSession1();

            SetRefererCookies();
            GetCookieMsgSetOff();
            GetJumboMsgSetOff();
            CalculateWallet();
            //Load state from database using the provided session Cookie
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
    private void CreateAndSaveCookieSession()
    {
        SetSessionCookies();
        SetUserGeoAndNewSessionData1();
        RecoverAndSetMsgVal1();
        SetSessionAttribute1();
        //  SetUserVisitData(_mySession.UserID);
    }
    internal string GetRefString()
    {
        if (_httpContext.Request.Cookies.ContainsKey(refCookie))
        {
            return validateRefere(_httpContext.Request.Cookies[refCookie]);
        }
        return string.Empty;
    }
    internal void CalculateWallet()
    {
        if (_mySession.CurrentUser != null)
        {
            var TempData = TDDF.GetTempData(_httpContext);
            var t = _mySession.CurrentUser.UserToken;
            var ucount = dbCtx.Users.Count(x => x.ReferralUserToken == t);
            TempData["myRefToken"] = _mySession.CurrentUser.UserToken;
            TempData["myRefMemC"] = ucount;
            //TempData["myLink"] = $"https://{_httpContext.Request.Host}?ref={t}";
            TempData["myLink"] = $"{AppConstant.TechnoAppSite}?ref={t}";
            TempData["myRefOnetime"] = $"{ucount} TechnoSavvy";
            TempData["myWallet"] = _mySession.UserWallet;
        }
    }
    internal void SetUserName()
    {
        var TempData = TDDF.GetTempData(_httpContext);
        if (_mySession.CurrentUser != null)
            TempData["userName"] = _mySession.UserName;
        else
            TempData["userName"] = string.Empty;

    }
    internal bool GetCookieMsgSetOff()
    {
        var s = _mySession.UserIDc;//_httpContext.Session.Id.ToString();
        var a = _mySession.CookieMsgSessions.ContainsValue(s);
        var TempData = TDDF.GetTempData(_httpContext);
        TempData["CookieMsgSetOff"] = a;
        return a;
    }
    internal bool GetJumboMsgSetOff()
    {
        var s = _mySession.UserIDc; //_httpContext.Session.Id.ToString();
        var a = _mySession.JumboMsgSessions.ContainsValue(s);
        var TempData = TDDF.GetTempData(_httpContext);
        TempData["JumboMsgSetOff"] = a;
        return a;
    }
    internal void CookieMsgSetOff()
    {
        var s = _mySession.UserIDc; //_httpContext.Session.Id;
        if (!_mySession.CookieMsgSessions.ContainsValue(s))
        {
            var l = _mySession.CookieMsgSessions;
            if (l.Count >= 20)
            {
                //remove oldest session
                l.Remove(l.Keys.Min());
            }
            l.Add(DateTime.UtcNow, s);
            _mySession.CookieMsgSessions = l;
        }
        var TempData = TDDF.GetTempData(_httpContext);
        TempData["CookieMsgSetOff"] = true;
        SaveState();
    }
 

    private void LoadCookieSession(bool fromResponse = false)
    {
        var cookieSession = _httpContext.Request.Cookies[sessionCookie];
        UserDataCookie udCookie = JsonSerializer.Deserialize<UserDataCookie>(cookieSession);
        if (udCookie.Analytic.LTUID == Guid.Empty)
            udCookie.Analytic.LTUID = Guid.NewGuid();

        if (udCookie.Analytic.LTUID != Guid.Empty)
        {
            var u = dbCtx.UserDataCookies.FirstOrDefault(x => x.UserId == udCookie.UserId);
            if (u != null)
            {
                _mySession.UserID = u.UserId;
                _mySession.UserIDc = u.UserId.ToString();
            }
            else
            {
                //Cookie Exist but no matching data in database, so Create
                if (CheckUserExistanceAndAction())
                    return;
            }
            //get Latest session of this User
            var s = GetLastSession();
            if (s != null && !s.hasExpiered())
            {
                _mySession.Session = s;
                LoadAppSessionFromAttribute1();
            }
            else
            {
                SetUserGeoAndNewSessionData1();
                RecoverAndSetMsgVal1();
                SetSessionAttribute1();
            }
        }
    }
    private bool CheckUserExistanceAndAction()
    {
        if (_mySession.UserID == Guid.Empty)//&& _mySession.CurrentUser== null
        {
            CreateAndSaveCookieSession();
            return true;
        }
        return false;
    }

    private void SetSessionCookies()
    {

        CookieOptions cookieOptions = new CookieOptions()
        {
            Expires = new DateTimeOffset(2099, 1, 1, 0, 0, 0, TimeSpan.FromHours(0))
        };
        var udCookie = new UserDataCookie();
        //Todo: naveen, we should also create a session for this user
        string userData = JsonSerializer.Serialize(udCookie);
        _httpContext.Response.Cookies.Append(sessionCookie, userData, cookieOptions);
        _mySession.UserID = udCookie.UserId;
        _mySession.UserIDc = udCookie.UserId.ToString();
    }
    internal void UpdateSessionCookies()
    {
        var cookieSession = _httpContext.Request.Cookies[sessionCookie];
        UserDataCookie udCookie = JsonSerializer.Deserialize<UserDataCookie>(cookieSession);
        if (udCookie != null && udCookie.UserId != Guid.Empty)
        {
            if (_mySession.CurrentUser != null)
            {
                //User Logged-In
                udCookie = dbCtx.UserDataCookies.FirstOrDefault(x => x.UserId == udCookie.UserId);
                udCookie.RegUserId = _mySession.CurrentUser.UserId;
                dbCtx.SaveChanges();

                CookieOptions cookieOptions = new CookieOptions()
                {
                    Expires = new DateTimeOffset(2099, 1, 1, 0, 0, 0, TimeSpan.FromHours(0))
                };
                string userData = JsonSerializer.Serialize(udCookie);
                _httpContext.Response.Cookies.Append(sessionCookie, userData, cookieOptions);
            }
            else
            {
                //User Logged-Out
                udCookie = dbCtx.UserDataCookies.FirstOrDefault(x => x.UserId == udCookie.UserId);
                udCookie.RegUserId = Guid.Empty;
                dbCtx.SaveChanges();

                CookieOptions cookieOptions = new CookieOptions()
                {
                    Expires = new DateTimeOffset(2099, 1, 1, 0, 0, 0, TimeSpan.FromHours(0))
                };
                string userData = JsonSerializer.Serialize(udCookie);
                _httpContext.Response.Cookies.Append(sessionCookie, userData, cookieOptions);
            }
        }
    }
    private void SetUserGeoAndNewSessionData1()
    {
        bool isSuccess = false;
        CFGeo cFGeo = SetGeoLOcation();
        if (cFGeo != null)
            cFGeo = GeoInfoManager.GetGeoLocation(dbCtx, cFGeo);

        UserSession session = null;
        session = HomeManager.SetUserSession(cFGeo, _mySession.UserID);


        dbCtx.UserSessions.Add(session);
        dbCtx.SaveChanges();
        _mySession.Session = session;
    }
    private void SetPageRequestData1()
    {
        PageRequested page = HomeManager.SetPageRequested(_mySession.Session, _httpContext);
        dbCtx.PageRequested.Add(page);
        dbCtx.SaveChanges();

    }
    private void ExtendSession1()
    {
        var s = dbCtx.UserSessions.First(x => x.UserSessionId == _mySession.Session.UserSessionId);
        s.SessionShouldExpierOn = DateTime.UtcNow.AddMinutes(60);
        s.LastRequestedOn = DateTime.UtcNow;
        dbCtx.SaveChanges();
    }
    private void RecoverAndSetMsgVal1()
    {
        var last = dbCtx.UserSessions.Where(x => x.UserId == _mySession.UserID && x.UserSessionId != _mySession.Session.UserSessionId).OrderByDescending(x => x.SessionShouldExpierOn).FirstOrDefault();
        if (last == null || last.UserSessionId == _mySession.Session.UserSessionId) return;

        var ls = JsonSerializer.Deserialize<AppSession>(dbCtx.SessionAttributes.First(x => x.UserSessionId == last.UserSessionId).SessionState);
        _mySession.CookieMsgSessions = ls.CookieMsgSessions ?? new Dictionary<DateTime, string>();
        _mySession.JumboMsgSessions = ls.JumboMsgSessions ?? new Dictionary<DateTime, string>();

    }
    private void SetSessionAttribute1()
    {
        var sessionattr = UtilityManager.GetSessionAttr(_httpContext);
        SessionAttribute attr = SetSessionAttriutes(_mySession.Session, sessionattr);
        dbCtx.SessionAttributes.Add(attr);
        dbCtx.SaveChanges();
    }
    private void LoadAppSessionFromAttribute1()
    {
        var sessionattr = UtilityManager.GetSessionAttr(_httpContext);
        SessionAttribute session = dbCtx.SessionAttributes.First(x => x.UserSessionId == _mySession.Session.UserSessionId);
        _mySession = JsonSerializer.Deserialize<AppSession>(session.SessionState);
        _mySession.CookieMsgSessions = _mySession.CookieMsgSessions ?? new Dictionary<DateTime, string>();
        _mySession.JumboMsgSessions = _mySession.JumboMsgSessions ?? new Dictionary<DateTime, string>();
        _mySession.SetManager(this);
        dbCtx.SaveChanges();
    }


    private CFGeoVm SetGeoLOcation()
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
    private void SetRefererCookies()
    {
        var cookie = _httpContext.Request.Cookies.ContainsKey(refCookie)
                               ? _httpContext.Request.Cookies[refCookie] : string.Empty;
        if (string.IsNullOrEmpty(cookie))
        {
            var refer = _httpContext.Request.Query["ref"];
            CookieOptions cookieOptions = new CookieOptions()
            {
                Expires = new DateTimeOffset(2099, 1, 1, 0, 0, 0, TimeSpan.FromHours(0))
            };

            if (!string.IsNullOrEmpty(refer))
                _httpContext.Response.Cookies.Append(refCookie, refer, cookieOptions);

        }
    }


    #endregion

}

/*
 *
 0. Create Refer Cookie to be used when User Register...
 1. Create a Long Term Id Cookie...Never Expier
 2. Create public User Session Id...
 3. Create Page Request Record....
 4. Receive Page Navigation Request 
 */