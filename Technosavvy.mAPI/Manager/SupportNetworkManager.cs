namespace NavExM.Int.Maintenance.APIs.Manager
{
    internal class SupportNetworkManager : ManagerBase
    {
        internal Tuple<bool, string> CreateSupportedNetwork(mSupportedNetwork m)
        {
            m.CheckAndThrowNullArgumentException();
            if (dbctx.SupportedNetwork.Any(x => x.SupportedNetworkId == m.SupportedNetworkId))
               m.ThrowInvalidOperationException("Existing Network can't be recreated");
            var e = m.ToEntity();
            
            e.Name.CheckAndThrowNullArgumentException();
            e.Description.CheckAndThrowNullArgumentException();
            e.NativeCurrencyCode.CheckAndThrowNullArgumentException();
            dbctx.SupportedNetwork.Add(e);
            e.SignRecord(this);

            return Ok(true, "Network Created Sussessfully..");
        }
       
        internal List<mSupportedNetwork> GetAllSupportedNetwork()
        {

           return dbctx.SupportedNetwork.Where(x => !x.DeletedOn.HasValue ||( x.DeletedOn.HasValue && x.DeletedOn.Value.Date <= DateTime.UtcNow.Date)).ToList().ToModel();
        }
         
    }

}
