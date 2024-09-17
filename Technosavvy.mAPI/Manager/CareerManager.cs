using NavExM.Int.Maintenance.APIs.Data.Entity.Contents;
using NuGet.Protocol;

namespace NavExM.Int.Maintenance.APIs.Manager
{
    public class CareerManager : ManagerBase
    {
        internal mJD? GetPublishedJDByRefNo(string RefNo)
        {
            var ret = JDdbctx.Jds.FirstOrDefault(x => x.RefNo.ToLower() == RefNo.ToLower() && x.Status == eJDStatus.Published);
            return ret.ToModel();
        }
        internal List<JD>? GetPublishedJDs()
        {
            var ret = JDdbctx.Jds.Where(x =>  x.Status == eJDStatus.Published).ToList();
            return ret;
        }
        internal bool RegisterViewer(Guid JDId)
        {
            var um = GetUserManager();
            string email = um.GetMyUserAccount().AuthEmail!.Email;
            var m=JDdbctx.Jds.Include(x => x.Viewers).FirstOrDefault(x => x.id == JDId);
            if (m == null) return false;
            m.Viewers.Add(new JDViewers {emailAddress=email, JDId=JDId,IpAddress= GetGeoLOcation().ToJson() });
           return JDdbctx.SaveChanges() > 0;
            
        }
        private UserManager GetUserManager()
        {
            //ToDo: Secure this Manager, Transaction Count Applied, Active Session
            var result = new UserManager
            {
                dbctx = dbctx,
                JDdbctx = JDdbctx,
                httpContext = httpContext
            };
            return result;
        }
        }

}
