using TechnoApp.Ext.Web.UI.Model;
using TechnoApp.Ext.Web.UI.Service;

namespace TechnoApp.Ext.Web.UI.Manager
{
    public class AddBookManager : MaintenanceSvc
    {

        internal List<mNetAddr> GetMyNetWhiteList(Guid networkId)
        {
            Console2.WriteLine_RED($"ToDo:Dummay Address Book entry Returned");
            var ret = new List<mNetAddr>() { new mNetAddr() { Address = "0xDDSA88763S0878s0877", Network = "ETH", Name = "ABC" }, new mNetAddr() { Address = "0xDASA88763S0878s0873", Network = "ETH", Name = "ABC2" } };
            return ret;
        }
        internal bool AddToMyNetWhiteList(Guid networkId,string Address)
        { 
            Console2.WriteLine_RED($"ToDo:Dummay AddToMyNetWhiteList entry Returned");
            return true;
        }
        internal bool AuthThisAddress( string Address)
        { 
            Console2.WriteLine_RED($"ToDo:Dummay AuthThisAddress entry Returned");
            return true;

        }
    }
}
