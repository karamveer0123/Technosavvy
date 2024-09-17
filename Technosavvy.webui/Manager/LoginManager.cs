using TechnoApp.Ext.Web.UI.Service;
using TechnoApp.Ext.Web.UI.Model;

namespace TechnoApp.Ext.Web.UI.Manager
{
    public class LoginManager : MaintenanceSvc
    {
        //User Login Related functions should be here
        internal async Task<bool> SetUpSession(mAuth auth)
        {
            var para = new mUserSessionRequest();
            para.authId = auth.mAuthId;
            para.UserName = auth.userName;
            var res = await RequestSession(para);

            _appSessionManager.mySession.SessionHash = res.SessionHash;
            _appSessionManager.mySession.SessionId = res.UserSessionId;
            _http.Request.Headers["SessionToken"] = res.SessionHash;

            return true;
        }
        internal async Task<bool> SetUpSessionAfter2ndFactor(mAuth auth)
        {
            var para = new mUserSessionRequest();
            para.authId = auth.mAuthId;
            para.UserName = auth.userName;
            var res = await ReportGAuthForSession(para);

            _appSessionManager.mySession.SessionHash = res.SessionHash;
            _appSessionManager.mySession.SessionId = res.UserSessionId;
            _http.Request.Headers["SessionToken"] = res.SessionHash;

            return true;
        }
        internal async Task<mAuth> LogIn(vmUserLogin loginVm)
        {
            var ret = await base.LogIn(loginVm);
            return ret;
        }
        internal async Task<bool> LogOut()
        {
            var ret = await base.LogOut();
            return ret;
        }
    }
}
