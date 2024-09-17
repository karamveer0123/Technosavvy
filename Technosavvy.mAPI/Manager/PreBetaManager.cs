using System.Text;
using NuGet.Protocol;
using Newtonsoft.Json;
using NavExM.Int.Maintenance.APIs.Data.Entity.PreBeta;
using NavExM.Int.Maintenance.APIs.Services;

namespace NavExM.Int.Maintenance.APIs.Manager
{
    internal class PreBetaManager : ManagerBase
    {
        public mPreBetaStats GetPreBetaStats()
        {
            var retval = new mPreBetaStats();
            var flst = pbdbctx.FractionFactor.ToList();
            var stages = pbdbctx.PreBetaStage.ToList();
            var tt = pbdbctx.PurchaseRecords.Sum(x => x.NavCAmountPurchased);
            var tt24 = pbdbctx.PurchaseRecords.Where(x => x.DateOf.Date >= DateTime.UtcNow.Date.AddDays(-1)).Sum(x => x.NavCAmountPurchased);
            var tuser = dbctx.UserAccount.Count();
            var tuser24 = dbctx.UserAccount.Where(x => x.CreatedOn.Date >= DateTime.UtcNow.Date.AddDays(-1)).Count();
            Console2.WriteLine_Green($"TotalNavC:{tt}|NavC24:{tt24}");
            Console2.WriteLine_Green($"TotalUser:{tuser}|User24:{tuser24}");
            foreach (var f in flst)
            {
                if (f.keyName.IsNOT_NullorEmpty())
                {
                    if (f.keyName.ToLower().Contains("user"))
                    {
                        if (f.keyName.Contains("24"))
                            retval.User24Hrs = f.FractionFactor * tuser24;
                        else
                            retval.UserTotal = f.FractionFactor * tuser;
                    }
                    else
                    {
                        if (f.keyName.Contains("24"))
                            retval.Token24Hrs = f.FractionFactor * tt24;
                        else
                            retval.TokenTotal = f.FractionFactor * tt;
                    }
                }
            }
            var stage = stages.Where(x => x.StartDate.Date <= DateTime.UtcNow.Date).OrderByDescending(x => x.StartDate).FirstOrDefault();
            if (stage == null)
            {
                Console.WriteLine($"Error:NO Active Stage for PreBeta.Please fix configuration;");
                retval.NavVCurrentPrice = 1.6;
            }
            else
                retval.NavVCurrentPrice = stage.NavCSellPrice;
            retval.BetaLiveIn = stages.First(x => x.EndDate != null).EndDate.Value;
            //Stages
            retval.TotalStages = stages.Count();
            retval.CompletedStages = stages.Where(x => x.StartDate.Date <= DateTime.UtcNow.Date).Count();
            //Info Statements
            var nextS = pbdbctx.PreBetaStage.FirstOrDefault(x => x.StartDate.Date > DateTime.UtcNow.Date);
            var next = DateTime.MinValue;
            if (nextS == null)
                next = retval.BetaLiveIn.Date;
            else
                next = nextS.StartDate.Date;

            var slst = GetAllPreBetaStages();

            retval.NavCPriceInfo.Add(NextIncrease(next));
            retval.NavCPriceInfo.Add("Beta User Registration NavC Holding");
            var l = DateTime.MinValue;
            mPreBetaStages last = null;
            foreach (var item in slst)
            {
                if (last != null)
                {
                    retval.NavCPriceInfo.Add($"{last.StartDate.ToString("dd MMM")} - {item.StartDate.ToString("dd MMM")}, Price is USDT {last.NavCSellPrice.ToString("0.00")}");
                }
                else
                {
                    //retval.NavCPriceInfo.Add($"till {item.StartDate.ToString("dd MMM")}, Price is USDT {item.NavCSellPrice}");
                }
                last = item;
            }
            retval.NavCPriceInfo.Add($"{last.StartDate.ToString("dd MMM")} - {retval.BetaLiveIn.ToString("dd MMM")}, Price is USDT {last.NavCSellPrice.ToString("0.00")}");
            retval.NavCPriceInfo.Add($"Beta Launch Price is USDT 1.60");
            Console2.WriteLine_Green($"Info:AfterFactor:TotalNavC:{retval.TokenTotal}|NavC24:{retval.Token24Hrs}");
            Console2.WriteLine_Green($"Info:AfterFactor:TotalUser:{retval.UserTotal}|User24:{retval.User24Hrs}");
            Console2.WriteLine_White($"Info:AfterFactor:PreBetaObject:{retval.ToJson()}");
            return retval;
        }
        string NextIncrease(DateTime next)
        {
            var d = next.Date - DateTime.UtcNow.Date;
            return $"Next Price will increase at {d.Days} days,";
        }
        internal List<mPreBetaStages> GetAllPreBetaStages()
        {
            var ret = new List<mPreBetaStages>();
            try
            {
                ret = pbdbctx.PreBetaStage.OrderBy(x => x.StartDate).ThenBy(x => x.id).ToList().ToModel();
                return ret;

            }
            catch (Exception ex)
            {
                LogError(ex);
                return ret;
            }
        }
        internal bool SavePreBetaStages(List<mPreBetaStages> vm)
        {
            try
            {
                if (pbdbctx.PreBetaStage.Count() > 0)
                    throw new ApplicationException("Prebeta Stages are already defined and cann't be redefined.");
                //validation
                //last one should have end date
                if (false == vm.OrderByDescending(x => x.StartDate).First().EndDate.HasValue)
                    throw new ApplicationException("Last Stages must have end date.");

                if (vm.Min(x => x.TokenCap) <= 0)
                    throw new ApplicationException("Tokencap must be greater than Zero.");

                var lst = vm.ToEntity();
                pbdbctx.PreBetaStage.AddRange(lst);

                return cdbctx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return false;
        }
        internal List<mFractionFactor> GetAllPreBetaFactors()
        {
            var ret = new List<mFractionFactor>();
            try
            {
                ret = (from x in pbdbctx.FractionFactor
                       select new mFractionFactor
                       {
                           id = x.id,
                           Key = x.keyName,
                           FractionFactor = x.FractionFactor,
                           CreatedOn = x.CreatedOn
                       }).ToList();
                return ret;

            }
            catch (Exception ex)
            {
                LogError(ex);
                return ret;
            }
        }
        internal bool SavePreBetaFactors(mFractionFactor fact)
        {
            try
            {
                fact.CheckAndThrowNullArgumentException();
                var obj = pbdbctx.FractionFactor.FirstOrDefault(x => x.id == fact.id);
                if (obj == null) return false;
                obj.FractionFactor = fact.FractionFactor;
                return pbdbctx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return false;
        }
    }

}
