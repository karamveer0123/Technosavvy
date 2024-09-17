using Microsoft.VisualBasic;
using TechnoApp.Ext.Web.UI.Model;

namespace TechnoApp.Ext.Web.UI.Manager;

/// <summary>
/// AppSessionExt Holds Database Loaded Request Lifetime Session Object of mUserSession
/// </summary>
/// <typeparam name="T"></typeparam>
public class AppSessionExt<T> : Service.MaintenanceSvc where T : AppSession
{
    public AppSessionExt(T bag, AppSessionManager mgr)
    {
        Bag = bag;
        _http = mgr._httpContext;
        _appSessionManager = mgr;
        bag.SetManager(mgr);
    }
    public bool IsLoaded { get; private set; } = false;
    private bool? _isvalid;
    /// <summary>
    /// Hold user Data for the Session that are not meant to be stored in Cookie at Client side.
    /// </summary>
    public T Bag { get; private set; }
    public mUserSession UserSession { get; private set; }
    public bool IsValid
    {
        get
        {
            if (_isvalid.HasValue)
                return _isvalid.Value;
            else
                _isvalid = (UserSession != null && !(UserSession.ShouldExpierOn <= DateTime.UtcNow || UserSession.ExpieredOn <= DateTime.UtcNow)) && UserSession.UserSessionId != Guid.Empty;
            if (!_isvalid.Value)
            {
                Bag.SessionHash = String.Empty;
            }
           
            return _isvalid.Value;

        }
    }
    public async Task LoadSession()
    {
        if (!IsLoaded)
        {
            if (Bag.SessionHash.IsNOT_NullorEmpty())
            {
                UserSession = await GetMySession();
                if (UserSession != null)
                {
                    Bag.UserName = UserSession.UserAccount.Email.To;
                    Bag.UserId = UserSession.UserAccount.Id;
                    Bag.SessionHash = UserSession.SessionHash;
                    if (UserSession?.UserAccount?.Profile != null)
                    {
                        _appSessionManager.mySession.NickName = UserSession.UserAccount.Profile.NickName;
                        if (UserSession?.UserAccount?.Profile?.TaxResidency != null)
                        {
                            _appSessionManager.mySession.UserCountry = UserSession.UserAccount.Profile.TaxResidency.Abbrivation;
                        }
                        _appSessionManager.mySession.KYC = UserSession.UserAccount.Profile.KYCStatus == eeKYCStatus.Completed;
                        _appSessionManager.mySession.KYCStatus = UserSession.UserAccount.Profile.KYCStatus;
                    }
                }
                IsLoaded = true;
            }
        }
    }


}