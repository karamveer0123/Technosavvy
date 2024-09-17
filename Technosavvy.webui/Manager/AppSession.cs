using TechnoApp.Ext.Web.UI.Model;

namespace TechnoApp.Ext.Web.UI.Manager
{
    /// <summary>
    /// AppSession is Cookie Stored Session
    /// </summary>
    public class AppSession
    {
        AppSessionManager myMgr = null;

        internal void SetManager(AppSessionManager mgr)
        {
            myMgr = myMgr == null ? mgr : myMgr;
        }
        private string _userCode;
        private string _userCurrency = "XX";
        private string _userCoin = "ETH";
        private string _refCode;
        private string _userLanguage = "ENG";
        private string _vName = "Classic";
        private string _userName;
        private string _userCountry;
        private string _userState;
        public string _nickName;

        private string _userId;
        public eeKYCStatus _IsKYCStatus { get; set; }
        private bool _kyc;
        private Guid? _sessionId;
        private string _LTUID = Guid.NewGuid().ToString().Replace("-", "");
        private bool _cookieConsent;
        List<string> _favMarkets = new List<string>();
        List<string> _watchList = new List<string>();
        private string _sessionHash;
        private CFGeoVm? _CFGeoVm;
        public string UserCode
        {
            get { return _userCode; }
            set { _userCode = value; if (myMgr != null) myMgr.SaveState(); }
        }
        /// <summary>
        /// For Non-Logged In User in general browsing Only if they enforce country else, source of Request
        /// For LoggedIn User, KYC/Profile Nominated Country will be used, it will override the values in this field
        /// </summary>
        public string UserCountry
        {
            get { return _userCountry; }
            set { _userCountry = value; if (myMgr != null) myMgr.SaveState(); }
        }
        private string _oURL;

        public string oURL
        {
            get { return _oURL; }
            set
            {
                if (_oURL.IsNullOrEmpty() || value.IsNullOrEmpty())
                {
                    _oURL = value;
                    //Console2.WriteLine_Green($"RefURL Set to:{_oURL}");
                    if (myMgr != null) myMgr.SaveState();
                }
            }
        }

        public string vName
        {
            get => _vName;
            set
            {
                _vName = value;
                if (myMgr != null) myMgr.SaveState();
            }
        }
        public string Currency
        {
            get { return _userCurrency; }
            set
            {
                _userCurrency = value;
                if (myMgr != null)
                    myMgr.SaveState();
            }
        }
        public bool KYC
        {
            get { return _kyc; }
            set
            {
                _kyc = value;
                if (myMgr != null)
                    myMgr.SaveState();
            }
        }
        public eeKYCStatus KYCStatus
        {
            get { return _IsKYCStatus; }
            set
            {
                _IsKYCStatus = value;
                if (myMgr != null)
                    myMgr.SaveState();
            }
        }
        public string NickName
        {
            get
            { return _nickName; }
            set
            {
                _nickName = value;
                if (myMgr != null)
                    myMgr.SaveState();
            }
        }

        public string Coin
        {
            get { return _userCoin; }
            set { _userCoin = value; if (myMgr != null) myMgr.SaveState(); }
        }
        /// <summary>
        /// Third party User Referred this User with RefCode.
        /// Only First Time Referrer will be saved.
        /// </summary>
        public string RefCode
        {
            get
            {
                return _refCode;
            }
            set
            {
                if (_refCode.IsNullOrEmpty())
                { _refCode = value; if (myMgr != null) myMgr.SaveState(); }
            }
        }
        public CFGeoVm? GIO
        {
            get
            {
                if (_CFGeoVm == null && myMgr != null)
                {
                    _CFGeoVm = myMgr.GetGeoLOcation();
                    myMgr.SaveState();
                }
                return _CFGeoVm;
            }
            set { _CFGeoVm = value; if (myMgr != null) myMgr.SaveState(); }
        }
        public string Language
        {
            get { return _userLanguage; }
            set { _userLanguage = value; if (myMgr != null) myMgr.SaveState(); }
        }
        public string UserName//email
        {
            get { return _userName; }
            set { _userName = value; if (myMgr != null) myMgr.SaveState(); }
        }
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; if (myMgr != null) myMgr.SaveState(); }
        }
        public Guid? SessionId
        {
            get { return _sessionId; }
            set { _sessionId = value; if (myMgr != null) myMgr.SaveState(); }
        }
        public string LTUID
        {
            get { return _LTUID; }
            set { _LTUID = value; if (myMgr != null) myMgr.SaveState(); }
        }
        public bool CookieConsent
        {
            get { return _cookieConsent; }
            set { _cookieConsent = value; if (myMgr != null) myMgr.SaveState(); }
        }
        public List<string> FavList
        {
            get { return _favMarkets; }
            set { _favMarkets = value; if (myMgr != null) myMgr.SaveState(); }
        }
        public List<string> WatchList
        {
            get { return _watchList; }
            set { _watchList = value; if (myMgr != null) myMgr.SaveState(); }
        }
        public string SessionHash
        {
            get { return _sessionHash; }
            set
            {
                _sessionHash = value;
                if (myMgr != null) myMgr.SaveState();
            }
        }
        private string _theme;

        public string Theme
        {
            get { return _theme; }
            set { _theme = value; if (myMgr != null) myMgr.SaveState(); }
        }

    }


}