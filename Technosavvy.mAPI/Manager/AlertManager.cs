using Microsoft.AspNetCore.SignalR;
using NavExM.Int.Maintenance.APIs.Services;

namespace NavExM.Int.Maintenance.APIs.Manager
{
    internal class AlertManager : ManagerBase
    {
        internal List<mAlertMsgBody> GetMyAlerts(int skip = 0, int count = 0)
        {
            if (count < 0 || count > 100)
                count = 100;
            if (skip < 0)
                skip = 0;
            var um = GetUserManager();
            var ua = um.GetMyUserAccount();
            using (var db = _AlertAppContext())
            {
                return db.UserAlerts.Where(x => x.userAccount == ua.FAccountNumber)
                       .OrderByDescending(x => x.GeneratedOn)
                       .Skip(skip).Take(count)
                       .ToList()
                       .ToModel();
            }
        }
        /// <summary>
        /// System Processing when need to notify a Message should use this
        /// </summary>
        /// <param name="uAccount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal bool SendUserAlertbySystem(string uAccount, mAlertMsgBody msg)
        {
            var um = GetUserManager();
            var ua = um.GetUserOfAcc(uAccount);
            return SendUserAlert(ua, msg);
        }
        internal bool SendUserAlertbyAction(mAlertMsgBody msg)
        {
            var um = GetUserManager();
            var ua = um.GetMyUserAccount();
            return SendUserAlert(ua, msg);
        }
        bool SendUserAlert(eUserAccount ua, mAlertMsgBody msg)
        {
            //update database
            //send message to user via HubAPI
            try
            {

                using (var db = _AlertAppContext())
                {
                    db.UserAlerts.Add(new eUserAlertMsg { GeneratedOn = msg.GeneratedOn, MsgBody = msg.Body, MsgTitle = msg.Title, ReportedOn = msg.ViewOn, userAccount = ua.FAccountNumber });
                    db.SaveChanges();
                }
                if (APIHub.AllClients != null && APIHub.AllClients.All != null)
                {
                    APIHub.AllClients.All.SendAsync("Alert", ua.AccountNumber, msg);
                }
                Console2.WriteLine_White($"Alert Sent to WebUI over SignalR  for:{ua.AccountNumber} at ..{DateTime.UtcNow}");
                return true;
            }
            catch (Exception ex)
            {
                Console2.WriteLine_RED($"SendUserAlertbyAction caused ERROR on SignalR  for:{ua.AccountNumber} at ..{DateTime.UtcNow}|ERROR:{ex.GetDeepMsg()}");
            }

            return false;
        }
        private OrderManager GetOrderManager()
        {
            var result = new OrderManager();
            result.dbctx = dbctx;
            result.httpContext = httpContext;
            return result;
        }
        private UserManager GetUserManager()
        {
            var result = new UserManager();
            result.dbctx = dbctx;
            result.httpContext = httpContext;
            return result;
        }
        private TokenManager GetTokenManager()
        {
            var result = new TokenManager();
            result.dbctx = dbctx;
            result.httpContext = httpContext;
            return result;
        }
        private WalletManager GetWalletManager()
        {
            //ToDo: Secure this Manager, Transaction Count Applied, Active Session

            var result = new WalletManager();
            result.dbctx = dbctx;

            return result;
        }
        private AlertAppContext _AlertAppContext()
        {
            var o = new DbContextOptionsBuilder<AlertAppContext>();
            o = o.UseSqlServer(ConfigEx.Config.GetConnectionString("AlertAppContext"));
            return new AlertAppContext(o.Options);
        }
    }

}
