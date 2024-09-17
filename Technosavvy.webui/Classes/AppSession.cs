/*using TechnoApp.Simulation.Web.Data.Entity;
using TechnoApp.Simulation.Web.Data.Model;
using TechnoApp.Simulation.Web.UI.Manager;
using TechnoApp.Simulation.Web.Data.Contexts;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TechnoApp.Ext.Web.UI.Classes;
public class AppSession
{
    AppSessionManager myMgr = null;
    private string passwordOTP;
    private string userName;
    private DateTime passwordOTPTime;
    private User currentUser;
    private UserSession session;
    private string userIDc;
    private Guid userID;
    private string refQryString;
    private string userWallet = "Not Provided";

    internal void SetManager(AppSessionManager mgr)
    {
        myMgr = myMgr == null ? mgr : myMgr;
    }
    public Dictionary<DateTime, string> CookieMsgSessions { get; set; } = new Dictionary<DateTime, string>();
    public string HistoricUser { get; set; }
    public string UserWallet { get => userWallet; set { userWallet = value; if (myMgr != null) myMgr.SaveState(); } }
    public string PasswordOTP { get => passwordOTP; set { passwordOTP = value; if (myMgr != null) myMgr.SaveState(); } }
    public string UserName { get => userName; set { userName = value; if (myMgr != null) myMgr.SaveState(); } }//email
    public DateTime PasswordOTPTime { get => passwordOTPTime; set { passwordOTPTime = value; if (myMgr != null) myMgr.SaveState(); } }
    //Password create otp expiry time
    public string RefQryString { get => refQryString; set { refQryString = value; if (myMgr != null) myMgr.SaveState(); } }
    public Guid UserID { get => userID; set { userID = value; if (myMgr != null) myMgr.SaveState(); } }//non reg userId guid format
    public string UserIDc { get => userIDc; set { userIDc = value; if (myMgr != null) myMgr.SaveState(); } }//non reg userId
    public UserSession Session { get => session; set { session = value; if (myMgr != null) myMgr.SaveState(); } }//Last session



}
public static class Ext
{
    public static bool hasExpiered(this UserSession sess)
    {
        if (sess.SessionShouldExpierOn <= DateTime.UtcNow || sess.SessionExpieredOn.HasValue && sess.SessionExpieredOn <= DateTime.UtcNow)
            return true;
        else
            return false;
    }
}

public class UserDataCookie
{
    public AnalyticCookie Analytic { get; set; }//Cookie 1
    public string Theme { get; set; }//Cookie 1
    public string Currency { get; set; }//Cookie 1
    public string Lang { get; set; }//Cookie 1
    public string Origion { get; set; }//Cookie 1

}
public class AnalyticCookie
{
    public string RefererId { get; set; }//Cookie 1
    public Guid LTUID { get; set; }//Cookie 2//LongTermUserId
    public Guid ThisRequestId { get; set; }//Page variable
    public string PageURL { get; set; }//Page variable
    public string PageEvent { get; set; }//Requested,LoadCompleted,NavigateAway
}

/*
 *
 0. Create Refer Cookie to be used when User Register...
 1. Create a Long Term Id Cookie...Never Expier
 2. Create public User Session Id...
 3. Create Page Request Record....
 4. Receive Page Navigation Request 
 */