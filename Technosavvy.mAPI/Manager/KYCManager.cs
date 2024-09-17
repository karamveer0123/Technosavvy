using NavExM.Int.Maintenance.APIs.Controllers;
using NavExM.Int.Maintenance.APIs.Data.Entity.KYC;
namespace NavExM.Int.Maintenance.APIs.Manager;

public class KYCManager : ManagerBase
{
    internal List<mCategoryDocTemplate> GetDocTemplates(string Abb)
    {
        using (var db = _KYCctx())
        {
            var lst = db.DocumentTemplate.Include(x => x.Category).Where(x => x.DeletedOn.HasValue == false
            && x.Category.DeletedOn.HasValue == false
            && x.Category.IsSuspended == false
            && x.ApprovalStatus == Data.Entity.KYC.AuthStatus.Accepted
            && x.Category.ApprovalStatus == Data.Entity.KYC.AuthStatus.Accepted
            && x.Category.CountryAbbr.ToLower() == Abb.ToLower()
            ).ToList();

            var mClst = lst.Select(x => x.Category).DistinctBy(x => x.CategoryId).ToList().ToModel();

            foreach (var cat in mClst)
            {
                cat.DocumentTemplates = lst.Where(x => x.CategoryId == cat.CategoryId).ToList().ToModel();
            }

            return mClst;
        }

    }
    internal List<mCategoryDocTemplate> GetDocTemplates()
    {
        using (var db = _KYCctx())
        {
            var lst = db.DocumentTemplate.Include(x => x.Category).Where(x => x.DeletedOn.HasValue == false
            && x.Category.DeletedOn.HasValue == false
            && x.Category.IsSuspended == false
            && x.ApprovalStatus == Data.Entity.KYC.AuthStatus.Accepted
            && x.Category.ApprovalStatus == Data.Entity.KYC.AuthStatus.Accepted
            ).ToList();


            var mClst = lst.Select(x => x.Category).DistinctBy(x => x.CategoryId).ToList().ToModel();

            foreach (var cat in mClst)
            {
                cat.DocumentTemplates = lst.Where(x => x.CategoryId == cat.CategoryId).ToList().ToModel();
            }

            return mClst;
        }

    }
    internal List<mKYCDocRecord> AddKYCDoc(List<mKYCDocRecord> mlst)
    {
        var retval = new List<mKYCDocRecord>();
        if (mlst == null || mlst.Count <= 0) return null;
        var abb = mlst.First().CountryAbbrivation;
        abb.CheckAndThrowNullArgumentException();
        var lst = GetDocTemplates(abb);
        var pm = GetProfileManager();
        var pro = pm.GetProfile(mlst.First().UserAccountId);
        if (pro.ProfileId != mlst.First().ProfileId)
            mlst.ThrowInvalidOperationException($"ERROR:AddKYCDoc Invalid ProfileId:{pro.ProfileId} or UserId:{mlst.First().UserAccountId} proivided.");

        pro.KYCStatus = eeKYCStatus.Started;
        dbctx.SaveChanges();
        dbctx.KYCDocRecords.Where(x => x.ProfileId == pro.ProfileId && x.DeletedOn.HasValue == false).ExecuteUpdate(p => p
                   .SetProperty(a => a.DeletedOn, DateTime.UtcNow));

        using (var db = _KYCctx())
        {
            db.DocumentInstance.Where(x => x.ProfileId == pro.ProfileId && x.DeletedOn.HasValue == false).ExecuteUpdate(p => p
                   .SetProperty(a => a.DeletedOn, DateTime.UtcNow));


            foreach (var m in mlst)
            {
                if (!lst.Any(x => x.CategoryId == m.CategoryId))
                    m.ThrowInvalidOperationException($"Invalid category Id:{m.CategoryId} provided");
                if (!lst.Any(x => x.DocumentTemplates.Any(z => z.DocumentTemplateId == m.PlaceHolderId)))
                    m.ThrowInvalidOperationException($"Invalid Placeholder Id:{m.KYCDocRecordId} provided");
                var e = m.ToEntity();
                var ee = m.ToAdminEntity(e.MatchId);
                dbctx.KYCDocRecords.Add(e);
                db.DocumentInstance.Add(ee);
                retval.Add(e.ToModel());
            }
            db.SaveChanges();
            dbctx.SaveChanges();
        }

        pro.KYCStatus = eeKYCStatus.UnderProcess;
        dbctx.SaveChanges();
        return retval;
    }
    private ProfileManager GetProfileManager()
    {
        var result = new ProfileManager();
        result.dbctx = dbctx;
        result.httpContext = httpContext;
        return result;
    }
    private KYCAppContext _KYCctx()
    {
        var o = new DbContextOptionsBuilder<KYCAppContext>();
        o = o.UseSqlServer(ConfigEx.Config.GetConnectionString("KYCAppContext"));
        return new KYCAppContext(o.Options);
    }
}
