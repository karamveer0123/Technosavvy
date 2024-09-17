namespace NavExM.Int.Maintenance.APIs.Manager
{
    public class FiatCurrencyManager : ManagerBase
    {
        //internal Tuple<bool, string> CreateFiatCurrency(mFiatCurrency m)
        //{
        //    if (dbctx.Token.Any(x => x.TokenId == m.FiatCurrencyId))
        //        m.ThrowInvalidOperationException("Existing token can't be recreated");
        //    var e = m.ToEntity();

        //    e.Name.CheckAndThrowNullArgumentException();
        //    e.Description.CheckAndThrowNullArgumentException();
        //    e.Code.CheckAndThrowNullArgumentException();
        //    e.Symbole.CheckAndThrowNullArgumentException();
        //    e.Tick.CheckAndThrowNullArgumentException();

        //    dbctx.FiatCurrency.Add(e);
        //    e.SignRecord(this);
        //    return Ok(true, "Exchange Token Created Sussessfully..");
        //}
        internal List<mFiatCurrency> GetAllFiatCurrencies()
        {
            return GetFiatCurrencies();
        }
        internal List<mBankDepositPaymentMethod> GetINRBankDetails()
        {
            var ret = new List<mBankDepositPaymentMethod>();
            var flst = dbctx.FiatCurrency
               .Include(x => x.Profiles)
               .ThenInclude(x => x.BankAccounts)
               .ThenInclude(x => x.LocatedAt)
               .FirstOrDefault(x => x.Code.ToUpper() == "INR" &&
                  x.DeletedOn.HasValue == false || (x.DeletedOn.Value.Date < DateTime.UtcNow.Date)
                 ).ToModel();
            if (flst == null) return ret;
            var pmlst = flst.Profiles.First().BankAccounts.SelectMany(x => x.PaymentMethod).ToList();
            if (pmlst == null) return ret;
            ret = pmlst.Where(x => x.MethodName == "BankDeposit" && x.SupportCurrency == "INR").Select(x => (mBankDepositPaymentMethod)x).ToList();
            return ret;
        }
        internal List<mUPIPaymentMethod> GetINRUPIDetails()
        {
            var ret = new List<mUPIPaymentMethod>();
            var flst = dbctx.FiatCurrency
               .Include(x => x.Profiles)
               .ThenInclude(x => x.BankAccounts)
               .ThenInclude(x => x.LocatedAt)
               .FirstOrDefault(x => x.Code.ToUpper() == "INR" &&
                  x.DeletedOn.HasValue == false || (x.DeletedOn.Value.Date < DateTime.UtcNow.Date)
                 ).ToModel();
            if (flst == null) return ret;
            var pmlst = flst.Profiles.First().BankAccounts.SelectMany(x => x.PaymentMethod).ToList();
            if (pmlst == null) return ret;

            ret = pmlst.Where(x => x.MethodName == "UPI" && x.SupportCurrency == "INR").Select(x => (mUPIPaymentMethod)x).ToList();
            return ret;
        }
        internal List<mFiatCurrency> GetFiatCurrencies(int count = 0)
        {
            if (count > 0)
            {
                return dbctx.FiatCurrency
                   .Include(x => x.Profiles)
                   .ThenInclude(x => x.BankAccounts)
                   .ThenInclude(x => x.LocatedAt)
                   .Where(x =>
                      x.DeletedOn.HasValue == false || (x.DeletedOn.Value.Date < DateTime.UtcNow.Date)
                     ).Take(count).ToList().ToModel();
            }
            else
            {
                return dbctx.FiatCurrency
               .Include(x => x.Profiles)
               .ThenInclude(x => x.BankAccounts)
               .ThenInclude(x => x.LocatedAt)
               .Where(x =>
                  x.DeletedOn.HasValue == false || (x.DeletedOn.Value.Date < DateTime.UtcNow.Date)
                 ).ToList().ToModel();
            }

        }
        internal mFiatCurrency GetSpecificFiatCurrenciesM(Guid id)
        {
            return GetSpecificFiatCurrencies(id)!.ToModel();
        }
        internal eFiatCurrency? GetSpecificFiatCurrencies(Guid id)
        {
            return dbctx.FiatCurrency
                 .Include(x => x.Profiles)
                 .ThenInclude(x => x.BankAccounts)
                 .ThenInclude(x => x.LocatedAt)
                 .FirstOrDefault(x =>
                 x.FiatCurrencyId == id
                   && x.DeletedOn.HasValue == false || (x.DeletedOn.Value.Date < DateTime.UtcNow.Date)
                   );
        }
    }

}
