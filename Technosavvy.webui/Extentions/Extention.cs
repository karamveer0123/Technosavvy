using TechnoApp.Ext.Web.UI.Models;
using TechnoApp.Ext.Web.UI.Model;
using TechnoApp.Ext.Web.UI.Manager;
using System.Runtime.CompilerServices;
using NuGet.Protocol;

namespace TechnoApp.Ext.Web.UI.Extentions
{
    public static class Extentions
    {
        #region Func Implementations

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        public static bool IsGuidNullorEmpty(this Guid guidId)
        {
            return guidId == Guid.Empty;
        }
        public static Guid GVC(this Guid? gId)
        {
            if (gId.HasValue) return gId.Value;
            return Guid.Empty;
        }
        public static long GetCurrentUnix(this DateTime me, int MinutesToAdd = 0)
        {
            return new DateTimeOffset(me.AddMinutes(MinutesToAdd)).ToUnixTimeSeconds();
        }
        public static string GetDeepMsg(this Exception ex)
        {
            if (ex is null) return string.Empty;
            var msg = $"{ex.Message}{ex.InnerException.GetDeepMsg()}{(ex.StackTrace != null ? ex.StackTrace : string.Empty)}";
            return msg;
        }
        //public static void CheckAndThrowNullArgumentException(this string? obj, [CallerMemberName] string mName = "", [CallerArgumentExpression("obj")] string ArguName="")
        //{
        //    if (obj != null && obj.IsNOT_NullorEmpty()) return;
        //    throw new ArgumentNullException($"{ArguName} can't be null");
        //}
        public static void CheckAndThrowNullArgumentException(this string? obj, string msg = "", [CallerMemberName] string mName = "", [CallerArgumentExpression("obj")] string ArguName = "")
        {
            if (obj != null && obj.IsNOT_NullorEmpty()) return;
            if (msg.IsNOT_NullorEmpty())
                throw new ApplicationException(msg);
            throw new ArgumentNullException($"{ArguName} can't be null");
        }
        public static void CheckAndThrowNullArgumentException(this int? obj, [CallerMemberName] string mName = "", [CallerArgumentExpression("obj")] string ArguName = "")
        {
            if (obj.HasValue) return;
            throw new ApplicationException($"{ArguName} can't be null or zero");
        }
        public static void CheckAndThrowNullArgumentException(this int obj, [CallerMemberName] string mName = "", [CallerArgumentExpression("obj")] string ArguName = "")
        {
            if (obj > 0) return;
            throw new ApplicationException($"{ArguName} can't be null or zero");
        }
        public static void CheckAndThrowNullArgumentException(this double? obj, [CallerMemberName] string mName = "", [CallerArgumentExpression("obj")] string ArguName = "")
        {
            if (obj.HasValue) return;
            throw new ApplicationException($"{ArguName} can't be null or zero");
        }
        public static void CheckAndThrowNullArgumentException(this double obj, [CallerMemberName] string mName = "", [CallerArgumentExpression("obj")] string ArguName = "")
        {
            if (obj != null && obj > 0) return;
            throw new ApplicationException($"{ArguName} can't be null or zero");
        }
        public static void CheckAndThrowZeroException(this double obj, [CallerMemberName] string mName = "", [CallerArgumentExpression("obj")] string ArguName = "")
        {
            if (obj != null && obj > 0) return;
            throw new ApplicationException($"{ArguName} can't be null or zero");
        }
        public static void CheckAndThrowNullArgumentException(this Guid obj, [CallerMemberName] string mName = "", [CallerArgumentExpression("obj")] string ArguName = "")
        {
            if (obj != Guid.Empty) return;
            throw new ApplicationException($"{ArguName} can't be null");
        }
        public static void CheckAndThrowNullArgumentException(this Guid? obj, [CallerMemberName] string mName = "", [CallerArgumentExpression("obj")] string ArguName = "")
        {
            if (obj.HasValue) return;
            throw new ApplicationException($"{ArguName} can't be null");
        }
        public static void CheckAndThrowNullArgumentException(this object obj, [CallerMemberName] string mName = "", [CallerArgumentExpression("obj")] string ArguName = "")
        {
            if (obj is not null) return;
            throw new ApplicationException($"{ArguName} can't be null");
        }
        public static void CheckAndThrowNullArgumentException(this IFormFile obj, [CallerMemberName] string mName = "", [CallerArgumentExpression("obj")] string ArguName = "")
        {
            if (obj != null && obj.Length > 0) return;
            throw new ApplicationException($"{ArguName} can't be null");
        }
        public static void CheckAndThrowNullArgumentException(this IFormFile obj, string msg)
        {
            if (obj != null && obj.Length > 0) return;
            throw new ApplicationException($"{msg} ");
        }
        public static void CheckAndThrowNullArgumentException(this IFormFile obj, bool IsImage, [CallerMemberName] string mName = "", [CallerArgumentExpression("obj")] string ArguName = "")
        {
            if (obj != null && obj.Length > 0)
            {
                if (Path.GetExtension(obj.FileName).ToUpper() != ".JPEG" && Path.GetExtension(obj.FileName).ToUpper() != ".JPG" && Path.GetExtension(obj.FileName).ToUpper() != ".PNG")
                    throw new ApplicationException($"{ArguName} File Supported Format are jpeg|jpg|png");
            }
            else
                throw new ApplicationException($"{ArguName} can't be null");
        }
        public static void CheckAndThrowNullArgumentException(this IFormFile obj, int sizeKB, [CallerMemberName] string mName = "", [CallerArgumentExpression("obj")] string ArguName = "")
        {
            if (obj != null && obj.Length > 0)
            {
                if (obj.Length > (1024 * sizeKB))
                    throw new ApplicationException($"{ArguName} size can't be greater than {sizeKB}");
            }
            else
                throw new ApplicationException($"{ArguName} can't be null");
        }

        public static void ThrowInvalidOperationException(this string msg, [CallerMemberName] string mName = "", [CallerArgumentExpression("obj")] string ArguName = "")
        {
            if (msg.IsNullOrEmpty())
                msg = $"Invalid request for operation{mName}. Operation Aborted..";
            throw new InvalidOperationException(msg);
        }
        public static void CheckSignIdAndThrowOperException(this Guid me, string msg, [CallerMemberName] string mName = "", [CallerArgumentExpression("obj")] string ArguName = "")
        {
            if (me != Guid.Empty) return;
            if (msg.IsNullOrEmpty())
                msg = $"Invalid request for operation{mName}. Operation Aborted..";
            throw new InvalidOperationException(msg);
        }
        public static void ThrowInvalidOperationException(this object obj, string msg = "", [CallerMemberName] string mName = "", [CallerArgumentExpression("obj")] string ArguName = "")
        {
            if (msg.IsNullOrEmpty())
                msg = $"Invalid request for operation{mName}. Operation Aborted..";
            msg = $"{msg}.\"{mName}\" Operation Aborted..";
            throw new InvalidOperationException(msg);
        }
        public static bool IsNOT_NullorEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }
        public static Guid ToGUID(this string str)
        {
            var g = Guid.Empty;
            if (!string.IsNullOrEmpty(str))
                Guid.TryParse(str, out g);

            return g;
        }
        public static T Check<T>(this T e) where T : class, new()
        {
            if (e is null)
                return new T();
            else
                return e;
        }
        #endregion
        public static vmWalletHomeClient ToVmClient(this vmWalletHome e)
        {
            var vm = new vmWalletHomeClient();
            if (e != null)
            {
                vm.selectedCryptoCoin = e.selectedCryptoCoin;
                vm.Assets = e.Assets;
            }
            return vm;
        }
        public static vmUser ToVM(this mUser e)
        {
            var m = new vmUser();
            var me = new mEmail();
            BaseVm b = new BaseVm();
            if (e != null)
            {
                me = e.Email;
                m._UserId = e.Id;
            }
            return m;
        }
        public static vmAddress ToVM(this mAddress m)
        {
            var vm = new vmAddress();
            if (m != null)
            {
                vm.AddressId = m.AddressId;
                vm.UnitNo = m.UnitNo;
                vm.StreetAdd = m.StreetAdd;
                vm.State = m.State;
                vm.PostCode = m.PostCode;
                vm.City = m.City;
                vm.selectedAddressCountryId = m.CountryId;
            }
            return vm;
        }
        public static mFiatDepositIntimation TomFiatDepositIntimation(this vmDeposit vm, AppSessionManager appSessionManager)
        {
            var p = appSessionManager.ExtSession.UserSession.UserAccount;
            var m = new mFiatDepositIntimation();
            m.Amount = vm.Amount;
            if (vm.IsBankDeposits && vm.IB?.INRTechnoAppOption?.selectedBankDeposits != null)
            { 
                m.Charges = vm.IB.INRTechnoAppOption.selectedBankDeposits.DepositFee;
                m.TechnoAppBankDetails = vm.IB.INRTechnoAppOption.selectedBankDeposits.ToJson();
                m.SenderBankDetails = vm.IB.INRUserOptions.selBankDeposits.ToJson();
            }
            if (vm.IsUPI && vm.IB?.INRTechnoAppOption?.selectedUPI != null)
            {
                m.Charges = vm.IB.INRTechnoAppOption.selectedUPI.DepositFee;
                m.TechnoAppBankDetails = vm.IB.INRTechnoAppOption.selectedUPI.ToJson();
                m.SenderBankDetails = vm.IB.INRUserOptions.selUPI.ToJson();
            }
            m.CurrencyCode = "INR";
            m.PublicRequestID = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            m.TaxResidencyCountryCode = p.Profile.TaxResidency.Abbrivation;
            m.TaxResidencyCountryName = p.Profile.TaxResidency.Name;
            m.uAccount = p.AccountNumber;
            m.KYCStatus = p.Profile.KYCStatus.ToString();
            m.RequestedOn = DateTime.UtcNow;
            m.GEOInfo = appSessionManager.GetGeoLOcation().ToJson();
            m.Status = new List<mWithdrawlRequestStatus>();
            return m;
        }
        public static vmDeposit ToClone(this vmDeposit vm, AppSessionManager appSessionManager)
        {
            var vm1 = vmFactory.GetvmDeposit(appSessionManager).Result;
            vm1.TokenList = vm.TokenList;
            vm1.selectedCoin = vm.selectedCoin;
            vm1.IsFiat = vm.IsFiat;
            vm1.IsBankDeposits = vm.IsBankDeposits;
            vm1.IsUPI = vm.IsUPI;
            vm1.selectedNetwork = vm.selectedNetwork;
            vm1.ActionMethod = vm.ActionMethod;
            vm1.Amount = vm.Amount;
            vm1.Symbole = vm.Symbole;
            vm1.inforBag = vm.inforBag;
            vm1.IB ??= new InfoBag();
            if (vm.IB != null)
            {
                vm1.IB.INRTechnoAppOption = vm.IB.INRTechnoAppOption;
                vm1.IB.INRUserOptions = vm.IB.INRUserOptions;
            }
            return vm1;
        }
        public static mWithdrawINRBankBag TomWithdrawINRBankBag(this vmWithdraw vm) {
            var vm1 = new mWithdrawINRBankBag();
            vm1.Amount = vm.Amount;
            vm1.Symbol = vm.Symbole;
            vm1.TokenId = vm.selectedCoin!.Value;
            vm1.TokenName = vm.Token.Code;
            vm1.TechnoAppBankAccount= vm.IB.INRTechnoAppOption.selectedBankDeposits;
            vm1.UserBankAccount= vm.IB.INRUserOptions.selBankDeposits;
            vm1.RefNarration = vm.RefNarration;
            return vm1;
        }
        public static mWithdrawINRUPIBag TomWithdrawINRUPIBag(this vmWithdraw vm) { 
            var vm1 = new mWithdrawINRUPIBag();
            vm1.Amount = vm.Amount;
            vm1.Symbol = vm.Symbole;
            vm1.TokenId = vm.selectedCoin!.Value;
            vm1.TokenName = vm.Token.Code;
            vm1.TechnoAppUPI = vm.IB.INRTechnoAppOption.selectedUPI;
            vm1.UserUPIAcc= vm.IB.INRUserOptions.selUPI;
            vm1.RefNarration = vm.RefNarration;
            return vm1;

        }
        public static mWithdrawNetBag TomWithdrawNetBag(this vmWithdraw vm)
        {
            var vm1 = new mWithdrawNetBag();
            vm1.AddToAddressBook = vm.AddToAddressBook;
            vm1.Amount = vm.Amount;
            vm1.IsAll = vm.IsAll;
            vm1.ReceiverAddr = vm.ReceiverAddr;
            vm1.Token = vm.Token;
            vm1.NetFee = vm.IB.NetFee;
            vm1.selectedNetwork = vm.selectedNetwork;
            return vm1;
        }
        public static vmWithdraw ToClone(this vmWithdraw vm, AppSessionManager appSessionManager)
        {
            var vm1 = vmFactory.GetvmWithdraw(appSessionManager).Result;
            vm1.AddToAddressBook = vm.AddToAddressBook;
            vm1.TokenList = vm.TokenList;
            vm1.SupportedNetwork = vm.SupportedNetwork != null ? vm.SupportedNetwork : new List<Model.mSupportedNetwork>();
            vm1.selectedCoin = vm.selectedCoin;
            vm1.IsAll = vm.IsAll;
            vm1.IsFiat = vm.IsFiat;
            vm1.IsBankDeposits = vm.IsBankDeposits;
            vm1.IsUPI = vm.IsUPI;
            vm1.selectedNetwork = vm.selectedNetwork;
            vm1.RefNarration = vm.RefNarration;
            vm1.ReceiverAddr = vm.ReceiverAddr;
            vm1.ActionMethod = vm.ActionMethod;
            vm1.Amount = vm.Amount;
            vm1.AddBalance = vm.AddBalance;
            vm1.Symbole = vm.Symbole;
            vm1.inforBag = vm.inforBag;
            vm1.IB ??= new InfoBag();
            if (vm.IB != null)
            {
                vm1.IB.INRTechnoAppOption = vm.IB.INRTechnoAppOption;
                vm1.IB.INRUserOptions = vm.IB.INRUserOptions;
            }
            Console2.WriteLine_Green($"IsBank:{(vm1.IsBankDeposits).ToString()}|IsUPI:{(vm.IsUPI).ToString()}|IsFiat:{(vm1.IsFiat).ToString()}");
            return vm1;
        }
        public static vmProfile LoadFrom(this vmProfile vm, mProfile m)
        {
            vm.ProfileId = m.ProfileId;
            vm.UserAccountId = m.UserAccountId;
            vm.FirstName = m.FirstName;
            vm.LastName = m.LastName;
            vm.Title = m.Title;
            vm.gender = m.gender;
            vm.NickName = m.NickName;
            if (m.TaxResidencyId != Guid.Empty)
                vm.TaxResidencyId = m.TaxResidencyId;
            if (m.CitizenshipId != Guid.Empty)
                vm.CitizenshipId = m.CitizenshipId;
            if (m.TaxResidency != null)
                vm.selectedTaxResidency = new vmCountry { Abbri = m.TaxResidency.Abbrivation, countryId = m.TaxResidency.CountryId, countryName = m.TaxResidency.Name };
            vm.DateOfBirth = m.DateOfBirth;
            vm.KYCStatus = m.KYCStatus;
            vm.Address = m.Address.ToVM();
            vm.Address = vm.Address ?? new vmAddress();
            vm.Address.Country = vm.Address.Country ?? new vmCountry();
            if (vm.Address.Country.countryId.IsGuidNullorEmpty())
                vm.Address.Country.countryId = m.TaxResidencyId;
            vm.myDocs = m.myDocs;
            return vm;
        }

        public static mProfile ToModel(this vmProfile vm)
        {
            var m = new mProfile();
            if (vm != null)
            {
                m.ProfileId = vm.ProfileId;
                m.UserAccountId = vm.UserAccountId;
                m.FirstName = vm.FirstName;
                m.LastName = vm.LastName;
                m.Title = vm.Title.IsNOT_NullorEmpty() ? vm.Title : "";
                m.gender = vm.gender;
                m.NickName = vm.NickName;
                m.DateOfBirth = vm.DateOfBirth;
                m.TaxResidencyId = vm.TaxResidencyId.HasValue ? vm.TaxResidencyId.Value : vm.selectedTaxResidency.countryId != Guid.Empty ? vm.selectedTaxResidency.countryId : Guid.Empty;

                m.TaxResidency = new mCountry { CountryId = m.TaxResidencyId, Name = "", DialCode = "", Continent = "", Block = "", Abbrivation = "" };

                m.CitizenshipId = vm.CitizenshipId.HasValue ? vm.CitizenshipId.Value : vm.selectedCitizenOf.countryId != Guid.Empty ? vm.selectedCitizenOf.countryId : Guid.Empty;

                m.Address = new mAddress();

                m.Address.AddressId = vm.Address != null ? vm.Address.AddressId : Guid.Empty;
                m.Address.UnitNo = vm.Address?.UnitNo != null ? vm.Address.UnitNo : string.Empty;
                m.Address.StreetAdd = vm.Address != null ? vm.Address.StreetAdd : string.Empty;
                m.Address.State = vm.Address != null ? vm.Address.State : string.Empty;
                m.Address.PostCode = vm.Address != null ? vm.Address.PostCode : string.Empty;
                m.Address.City = vm.Address != null ? vm.Address.City : string.Empty;
                m.Address.CountryId = m.TaxResidencyId;
                m.myDocs = new List<mKYCDocRecord>();
                // (vm.Address != null && vm.Address.selectedAddressCountryId.HasValue) ? vm.Address.selectedAddressCountryId.Value : Guid.Empty;

                //m.Address = (vm.Address != null && vm.Address.selectedAddressCountryId != null) ? new mAddress
                //{
                //    UnitNo = vm.Address.UnitNo,
                //    State = vm.Address.State,
                //    City = vm.Address.City,
                //    StreetAdd = vm.Address.StreetAdd,
                //    Country = new mCountry() { CountryId = vm.Address.selectedAddressCountryId.Value }
                //} : null;
                //m.Address = m.Address ?? new mAddress();//must not be null
            }
            return m;
        }

    }
}
