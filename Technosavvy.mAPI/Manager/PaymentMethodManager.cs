using NavExM.Int.Maintenance.APIs.Model;

namespace NavExM.Int.Maintenance.APIs.Manager
{
    internal class PaymentMethodManager : ManagerBase
    {
        internal List<mINRUPI> GetINRUPISetup(Guid profileId)
        {
            var lst = dbctx.UPI.Where(x => x.profileId == profileId && x.DeletedOn.HasValue==false).ToList().ToModel();
            return lst;
        }
        internal List<mINRBankDeposit> GetINRBankDeposit(Guid profileId)
        {
            var lst = dbctx.BankDeposit.Where(x => x.profileId == profileId && x.DeletedOn.HasValue == false).ToList().ToModel();
            return lst;
        }
        internal mINRUPI CreateINRUPISetup(mINRUPI m)
        {
            ValidateUPI(m);
            var e = m.ToEntity();
            dbctx.UPI.Add(e);
             dbctx.SaveChanges();
            return e.ToModel();
        }
        internal bool DeleteINRUPISetup(Guid m)
        {
            return dbctx.UPI.Where(x => x.ID == m).ExecuteUpdate(x => x.SetProperty(a => a.DeletedOn, DateTime.UtcNow)) > 0;
        }
        internal bool DeleteINRBankDeposit(Guid m)
        {
            return dbctx.BankDeposit.Where(x => x.ID == m).ExecuteUpdate(x => x.SetProperty(a => a.DeletedOn, DateTime.UtcNow)) > 0;
        }
        internal mINRBankDeposit CreateINRBankDepositSetup(mINRBankDeposit m)
        {
            ValidateBankDeposit(m);
            var e = m.ToEntity();
            dbctx.BankDeposit.Add(e);
            dbctx.SaveChanges();
            return e.ToModel();
        }
        internal bool ValidateUPI(mINRUPI m)
        {
            m.AccountHolderName.CheckAndThrowNullArgumentException();
            m.UPIid.CheckAndThrowNullArgumentException();
            m.CheckAndThrowNullArgumentException();
            m.QRCode.CheckAndThrowNullArgumentException();
            m.AccountHolderName.CheckAndThrowNullArgumentException("Account Holder Name must be provided");
            m.UPIid.CheckAndThrowNullArgumentException("UPI Id must be provided");
            m.QRCode.Length.CheckAndThrowNullArgumentException("QR Code must be provided");
            if (m.QRCode.Length > (1024 * 200))
                throw new ApplicationException("QR Image must be less than 200KB");
            var isAny= dbctx.UPI.Any(x => x.UPIid.ToLower() == m.UPIid.ToLower() && x.DeletedOn.HasValue );
            if (isAny)
                throw new InvalidOperationException($"Such UPI Id:{m.UPIid} is already Registerd");
            return true;
        }
        internal bool ValidateBankDeposit(mINRBankDeposit m)
        {
            m.CheckAndThrowNullArgumentException();
            m.CheckAndThrowNullArgumentException("Technical Error 1033");
            m.AccountHolderName.CheckAndThrowNullArgumentException("Account Holder Name must be provided");
            m.IFSCCode.CheckAndThrowNullArgumentException("IFSC Code must be provided");
           
            var isAny = dbctx.BankDeposit.Any(x => x.AccountNumber.ToLower() == m.AccountNumber.ToLower() && x.DeletedOn.HasValue);
            if (isAny)
                throw new InvalidOperationException($"Such Account No:{m.AccountNumber} is already Registerd");
            return true;
        }
    }

}
