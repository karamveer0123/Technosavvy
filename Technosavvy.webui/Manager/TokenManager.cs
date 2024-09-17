using TechnoApp.Ext.Web.UI.Model;
using TechnoApp.Ext.Web.UI.Service;

namespace TechnoApp.Ext.Web.UI.Manager
{
    public class TokenManager : MaintenanceSvc
    {
        internal List<mToken> GetActiveTokens()
        {
            return (base.GetAllActiveTokens().Result);
        }
        internal List<mToken> GetActiveFiatTokens()
        {
            return (base.GetAllActiveFiatTokens().Result);
        }
        internal mToken GetActiveToken(Guid id)
        {
            return (base.GetActiveToken(id).Result);
        }
        internal mToken GetActiveTokenOfCode(string Code)
        {
            return (base.GetActiveTokenOfCode(Code).Result);
        }
        internal List<mTokenNetworkFee> GetAllTokensNetWorkFee()
        {
            return (base.GetAllTokensNetWorkFee().Result);
        }
        internal mTokenNetworkFee GetTokensNetWorkFee(Guid tokenId, Guid netId)
        {
            return (base.GetTokensNetWorkFee(tokenId, netId).Result);
        }
    }
}
