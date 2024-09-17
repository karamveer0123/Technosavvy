using TechnoApp.Ext.Web.UI.Service;
using TechnoApp.Ext.Web.UI.Model;

namespace TechnoApp.Ext.Web.UI.Manager;

public class ContentManager : MaintenanceSvc
{
    internal async Task<vmFAQDisplay> GetAllApprovedFAQsToDisplay(vmFAQDisplay vm)
    {
        Dictionary<string, List<mFAQDisplay>> retGrp = new Dictionary<string, List<mFAQDisplay>>();
        var ret = await base.GetAllApprovedFAQs();
        var gt = ret.DistinctBy(x => x.GroupTitle).Select(x => x.GroupTitle).ToList();
        foreach (var item in gt)
        {
            var tlst = ret.Where(x => x.GroupTitle == item).OrderBy(x=>x.OrderNo).ToList();
            retGrp.Add(item, tlst);   
        }
        vm.FAQs = retGrp;
        return vm;

    }
}