namespace NavExM.Int.Maintenance.APIs.Manager
{
    internal class SupportTokenManager : ManagerBase
    {
        internal Tuple<bool, string> CreateSupportedToken(mSupportedToken m)
        {
            m.CheckAndThrowNullArgumentException();
            if (dbctx.SupportedToken.Any(x => x.SupportedTokenId == m.SupportedTokenId))
                m.ThrowInvalidOperationException("Existing token can't be recreated");
            var e = m.ToEntity();

            e.Code.CheckAndThrowNullArgumentException();
            e.Narration.CheckAndThrowNullArgumentException();
            e.ContractAddress.CheckAndThrowNullArgumentException();
            dbctx.SupportedToken.Add(e);
            e.SignRecord(this);

            return Ok(true, "Supported Token Created Sussessfully..");
        }
        internal List<mSupportedToken> GetAllSupportedTokens()
        {
            return dbctx.SupportedToken.Where(x => (x.DeletedOn.HasValue && x.DeletedOn.Value < DateTime.UtcNow.Date) || !x.DeletedOn.HasValue).ToList().ToModel();
        }
        internal List<mSupportedNetwork> GetAllSupportedNetwork()
        {
           return dbctx.SupportedNetwork.Where(x => (x.DeletedOn.HasValue && x.DeletedOn.Value < DateTime.UtcNow.Date) || !x.DeletedOn.HasValue).ToList().ToModel();
        }
        internal List<mSupportedToken> GetAllSupportedTokensOfNetwork(Guid networkId)
        {
            var all = dbctx.SupportedToken
                 .Include(x => x.RelatedNetwork)
                 .Where(x => x.RelatedNetworkId==networkId 
                 && x.DeletedOn.HasValue == false || (x.DeletedOn.Value.Date < DateTime.UtcNow.Date)
                 ).ToList();
            return all.ToModel();


        }

    }

}
