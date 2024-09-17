using TechnoApp.Ext.Web.UI.Models;
using TechnoApp.Ext.Web.UI.Service;
using TechnoApp.Ext.Web.UI.Extentions;
using TechnoApp.Ext.Web.UI.Model;
using System.Net.Http.Headers;

namespace TechnoApp.Ext.Web.UI.Manager;


public class UserManager : MaintenanceSvc
{
    internal async Task<mAuth> LogIn(vmUserLogin loginVm)
    {
        var ret = await base.LogIn(loginVm);
        return ret;
    }
}
