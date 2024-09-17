using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using NavExM.Int.Maintenance.APIs.Data.Entity.KYC;

namespace NavExM.Int.Maintenance.APIs.Manager
{
    internal class ProfileManager : ManagerBase
    {
        internal eCountry GetCountryById(Guid id)
        {
            if (id == Guid.Empty) return null;
            var res = dbctx.Country.FirstOrDefault(x => x.CountryId == id);
            if (res == null) throw new ArgumentException("Invalid id for country..");
            return res;
        }
        internal eProfile GetProfile(string uName)
        {
            if (uName.IsNullorEmpty()) return null;
            uName = uName.ToLower();
            var result = dbctx.Profile
               .Include(x => x.UserAccount)
                .ThenInclude(x => x.TaxResidency)
                .ThenInclude(x => x.Country)
                .Include(x => x.UserAccount)
                .ThenInclude(x => x.CitizenOf)
                .ThenInclude(x => x.Country)
                .Include(x => x.CurrentAddress)
                .ThenInclude(x => x.Country)
               .FirstOrDefault(x => x.UserAccount.AuthEmail.Email == uName);

            return result;

        }
        internal eProfile UpdateAddress(mProfile m)
        {
            if (m == null) return null;
            var isAny = GetProfile(m.UserAccountId);
            if (isAny != null)
            {
                isAny.CurrentAddress = m.Address.ToEntity();
                dbctx.SaveChanges();
            }
            else
                throw new ArgumentException("No such Profile exists to update.");

            return isAny;
        }
        internal eProfile UpdateProfilePersonalDetails(mProfile m)
        {
            if (m == null) return null;
            var isAny = GetProfile(m.UserAccountId);//GetUserManager().GetMyUserAccountId()
            if (isAny != null)
            {
                var or = isAny.Imprint;
                isAny.FirstName = m.FirstName;
                isAny.LastName = m.LastName;
                isAny.Title = m.Title;
                isAny.NickName = m.NickName;
                isAny.DateOfBirth = m.DateOfBirth;
                isAny.gender = m.gender;
                if (isAny.UserAccount.TaxResidency == null || (isAny.UserAccount.TaxResidency!.Country.CountryId != m.TaxResidencyId))
                {
                    if (m.TaxResidencyId != Guid.Empty)
                    {
                        if (isAny.UserAccount.TaxResidency != null)
                        {
                            isAny.UserAccount.TaxResidency = new eTaxResidency() { Country = GetCountryById(m.TaxResidencyId), CreatedOn = DateTime.UtcNow, SessionHash = GetSessionHash(), PreviousTaxResidency = isAny.UserAccount.TaxResidency };
                        }
                        else
                        {
                            isAny.UserAccount.TaxResidency = new eTaxResidency() { Country = GetCountryById(m.TaxResidencyId), CreatedOn = DateTime.UtcNow, SessionHash = GetSessionHash() };
                        }
                    }
                }
                if (isAny.UserAccount.CitizenOf == null || (isAny.UserAccount.CitizenOf!.Country.CountryId != m.CitizenshipId))
                {
                    if (m.CitizenshipId != Guid.Empty)
                    {
                        if (isAny.UserAccount.CitizenOf != null)
                        {
                            isAny.UserAccount.CitizenOf = new eCitizenship() { Country = GetCountryById(m.CitizenshipId), CreatedOn = DateTime.UtcNow, SessionHash = GetSessionHash(), PreviousCitizenship = isAny.UserAccount.CitizenOf };
                        }
                        else
                        {
                            isAny.UserAccount.CitizenOf = new eCitizenship() { Country = GetCountryById(m.CitizenshipId), CreatedOn = DateTime.UtcNow, SessionHash = GetSessionHash() };
                        }
                    }
                }
                isAny.CurrentAddress ??= new eAddress();
                isAny.CurrentAddress.UnitNO = m.Address.UnitNo;
                isAny.CurrentAddress.StreetAdd = m.Address.StreetAdd;
                isAny.CurrentAddress.City = m.Address.City;
                isAny.CurrentAddress.State = m.Address.State;
                isAny.CurrentAddress.PostCode = m.Address.PostCode;
                isAny.CurrentAddress.CountryId = m.Address.CountryId;
                isAny.CurrentAddress.UserAccount = isAny.UserAccount;
                // var ch = isAny.Imprint;
                //if (ch != or)
                isAny.KYCStatus = eeKYCStatus.Started;
                dbctx.SaveChanges();
                dbctx.KYCDocRecords.Where(x => x.ProfileId == isAny.ProfileId && x.DeletedOn.HasValue == false).ExecuteUpdate(p => p
               .SetProperty(a => a.DeletedOn, DateTime.UtcNow));

            }
            else
                throw new ArgumentException("No such Profile exists to update.");

            return isAny;
        }
        internal eProfile CreateProfile(mProfile m)
        {
            if (m == null) return null;
            var isAny = GetProfile(m.UserAccountId);
            if (isAny != null) throw new InvalidOperationException("Duplicate profile cannot be created, Try updating..");
            var usr = dbctx.UserAccount.First(x => x.UserAccountId == m.UserAccountId);
            var profile = m.ToEntity();
            usr.UserProfile = profile;

            if (usr.TaxResidency == null && m.TaxResidencyId != Guid.Empty)
                usr.TaxResidency = new eTaxResidency()
                {
                    Country = GetCountryById(m.TaxResidencyId),
                    SessionHash = GetSessionHash()
                };
            if (usr.CitizenOf == null && m.CitizenshipId != Guid.Empty)
                usr.CitizenOf = new eCitizenship()
                {
                    Country = GetCountryById(m.TaxResidencyId),
                    SessionHash = GetSessionHash()
                };
            // dbctx.Profile.Add(profile);
            dbctx.SaveChanges(true);

            return profile;
        }
        internal eProfile GetProfile(Guid userAccountId)
        {
            if (userAccountId == Guid.Empty) return null;
            var result = dbctx.Profile
                .Include(x => x.UserAccount)
                .ThenInclude(x => x.TaxResidency)
                .ThenInclude(x => x.Country)
                .Include(x => x.UserAccount)
                .ThenInclude(x => x.CitizenOf)
                .ThenInclude(x => x.Country)
                .Include(x => x.CurrentAddress)
                .ThenInclude(x => x.Country)
                .FirstOrDefault(x => x.UserAccount.UserAccountId == userAccountId);

            return result;

        }
        internal mProfile GetProfileM(Guid userAccountId)
        {
            var ret = new mProfile();
            if (userAccountId == Guid.Empty) return ret;
            var result = dbctx.Profile
                .Include(x => x.UserAccount)
                .ThenInclude(x => x.TaxResidency)
                .ThenInclude(x => x.Country)
                .Include(x => x.UserAccount)
                .ThenInclude(x => x.CitizenOf)
                .ThenInclude(x => x.Country)
                .Include(x => x.CurrentAddress)
                .ThenInclude(x => x.Country)
                .FirstOrDefault(x => x.UserAccount.UserAccountId == userAccountId);
            if (result == null) return ret;
            ret = result.ToModel();
            ret.myDocs = dbctx.KYCDocRecords.Where(x => x.ProfileId == result.ProfileId && x.DeletedOn.HasValue == false).ToList().ToModel();

            return ret;

        }



        #region Private Methods
        private UserManager GetUserManager()
        {
            var result = new UserManager();
            result.dbctx = dbctx;
            result.httpContext = httpContext;
            return result;
        }
        #endregion


    }

}
