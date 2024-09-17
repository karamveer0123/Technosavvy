using NavExM.Int.Maintenance.APIs.Data.Entity.KYC;
using NavExM.Int.Maintenance.APIs.Data.Entity.PreBeta;
using NavExM.Int.Maintenance.APIs.Model;
using NavExM.Int.Maintenance.APIs.ServerModel;

namespace NavExM.Int.Maintenance.APIs.Extension
{
    public static partial class Ext
    {

        #region ToEntity

        public static List<ePreBetaStage> ToEntity(this List<mPreBetaStages> m)
        {
            var e = new List<ePreBetaStage>();
            if (m != null)
            {
                m.ForEach(x => e.Add(x.ToEntity()));
            }
            return e;
        }
        public static ePreBetaStage ToEntity(this mPreBetaStages m)
        {
            var e = new ePreBetaStage();
            if (m != null)
            {
                e.StageName = m.StageName;
                e.StartDate = m.StartDate;
                e.NavCSellPrice = m.NavCSellPrice;
                e.TokenCap = m.TokenCap;
                if (m.EndDate.HasValue)
                    e.EndDate = m.EndDate;
            }
            return e;
        }
        public static Data.Entity.Fund.eFiatDepositRequest ToEntity(this mFiatDepositIntimation m)
        {
            var e = new Data.Entity.Fund.eFiatDepositRequest();
            if (m != null)
            {
                e.PublicRequestID = m.PublicRequestID;
                e.uAccount = m.uAccount;
                e.TaxResidencyCountryCode = m.TaxResidencyCountryCode;
                e.TaxResidencyCountryName = m.TaxResidencyCountryName;
                e.KYCStatus = m.KYCStatus;
                e.CurrencyCode = m.CurrencyCode;
                e.CurrencySymbole = m.CurrencySymbole;
                e.Amount = m.Amount;
                e.Charges = m.Charges;
                e.RequestedOn = m.RequestedOn;
                e.NavExMBankDetails = m.NavExMBankDetails;
                e.SenderBankDetails = m.SenderBankDetails;
                e.DepositEvidence = m.DepositEvidence;
                e.GEOInfo = m.GEOInfo;
                e.Status ??= new List<Data.Entity.Fund.eWithdrawlRequestStatus>();
            }
            return e;
        }
        public static Data.Entity.Fund.eFiatWithdrawRequest ToEntity(this mFiatWithdrawRequest m)
        {
            var e = new Data.Entity.Fund.eFiatWithdrawRequest();
            if (m != null)
            {
                e.PublicRequestID = m.PublicRequestID;
                e.uAccount = m.uAccount;
                e.TaxResidencyCountryCode = m.TaxResidencyCountryCode;
                e.TaxResidencyCountryName = m.TaxResidencyCountryName;
                e.KYCStatus = m.KYCStatus;
                e.CurrencyCode = m.CurrencyCode;
                e.CurrencySymbole = m.CurrencySymbole;
                e.Amount = m.Amount;
                e.Charges = m.Charges;
                e.RequestedOn = m.RequestedOn;
                e.CompletedOn = m.CompletedOn;
                e.ReceiverBankDetails = m.ReceiverBankDetails;
                e.Narration = m.Narration;
                e.GEOInfo = m.GEOInfo;
                e.Status ??= new List<Data.Entity.Fund.eWithdrawlRequestStatus>();
            }
            return e;
        }
        public static Data.Entity.Fund.eCryptoWithdrawRequest ToEntity(this mCryptoWithdrawRequest m)
        {
            var e = new Data.Entity.Fund.eCryptoWithdrawRequest();
            if (m != null)
            {
                e.PublicRequestID = m.PublicRequestID;
                e.uAccount = m.uAccount;
                e.TaxResidencyCountryCode = m.TaxResidencyCountryCode;
                e.TaxResidencyCountryName = m.TaxResidencyCountryName;
                e.KYCStatus = m.KYCStatus;
                e.TokenCode = m.TokenCode;
                e.TokenId = m.TokenId;
                e.TokenContractAddress = m.TokenContractAddress;
                e.NetworkId = m.NetworkId;
                e.NetworkName = m.NetworkName;
                e.Amount = m.Amount;
                e.Charges = m.Charges;
                e.RequestRefId = m.RequestRefId;
                e.RequestedOn = m.RequestedOn;
                e.CompletedOn = m.CompletedOn;
                e.ReceiverAddress = m.ReceiverAddress;
                e.GEOInfo = m.GEOInfo;
                e.TransactionReceipt = m.TransactionReceipt;
                e.Status ??= new List<Data.Entity.Fund.eWithdrawlRequestStatus>();
            }
            return e;
        }
        public static eAuthorizedEmail ToEntity(this mEmail m)
        {
            var e = new eAuthorizedEmail();
            if (m != null)
            {
                e.AuthorizedEmailId = m.Id.ToGUID();
                //e.Email = m.Email;
            }
            return e;
        }
        public static eCountry ToEntity(this mCountry m)
        {
            var e = new eCountry();
            if (e != null)
            {
                e.CountryId = m.CountryId;
                //Application User are not expected to update Maintenance data Tables like country
            }
            return e;
        }
        public static List<eCountry> ToEntity(this List<mCountry> m)
        {
            var e = new List<eCountry>();
            if (m != null)
            {
                m.ForEach(x => e.Add(x.ToEntity()));

                //Application User are not expected to update Maintenance data Tables like country
            }
            return e;
        }
        public static List<eMarketProfileScope> ToEntityToMarketProfileScope(this List<mCountry> m, eMarketProfile eMP)
        {
            var e = new List<eMarketProfileScope>();
            if (m != null)
            {
                m.ForEach(x => e.Add(
                    new eMarketProfileScope()
                    {
                        Country = x.ToEntity(),
                        CountryId = x.CountryId,
                        MarketProfile = eMP
                    }));

                //Application User are not expected to update Maintenance data Tables like country
            }
            return e;
        }
        public static eMarket ToEntity(this mMarket m)
        {
            var e = new eMarket();
            if (m != null)
            {
                e.MarketId = m.MarketId;
                e.MarketType = m.MarketType;
                e.ShortName = m.ShortName;
                e.CodeName = m.CodeName;
                e.MinOrderSizeValueUSD = m.MinOrderSizeValueUSD;
                e.MinBaseOrderTick = m.MinBaseOrderTick;
                e.MinQuoteOrderTick = m.MinQuoteOrderTick;
                e.BaseTokenId = m.BaseTokenId;
                e.QuoteTokenId = m.QuoteTokenId;
                //  e.BaseCurrency = m.BaseCurrency.ToEntity();
                e.QuoteCurrencyId = m.QuoteCurrencyId;
                e.IsTradingAllowed = m.IsTradingAllowed;
                e.MarketProfile = m.MarketProfile.ToEntity();
                e.Attributes = m.Attributes.ToEntity();
            }
            return e;
        }
        public static eMarketAttributes ToEntity(this mMarketAttributes m)
        {
            var e = new eMarketAttributes();
            if (m != null)
                e = new eMarketAttributes()
                {
                    MarketAttributesId = m.MarketAttributesId,
                    HighlightFrom = m.HighlightFrom,
                    HighlightTill = m.HighlightTill,
                    NewListingFrom = m.NewListingFrom,
                    NewListingTill = m.NewListingTill,
                    UpdatedOn = m.UpdatedOn,
                    CreatedOn = m.CreatedOn,
                };
            return e;
        }
        public static eTradingFee ToEntity(this mTradingFee m)
        {
            var e = new eTradingFee();
            if (m != null)
            {
                e.TokenFeeId = m.TokenFeeId;
                e.FeeCommunity = m.FeeCommunity;
                e.FeeNonCommunity = m.FeeNonCommunity;
                e.FeeExempt = m.FeeExempt;
                e.FeeIndependent = m.FeeIndependent;
                e.FeeName = m.FeeName;
                e.Details = m.Details;
                e.DisplayAsSwap = m.DisplayAsSwap;
            }
            return e;
        }
        public static eTax ToEntity(this mTax m)
        {
            var e = new eTax();
            if (m != null)
            {
                e.TaxId = m.TaxId;
                e.Rate = m.Rate;
                e.TaxName = m.TaxName;
                e.Details = m.Details;
                e.isInclusive = m.isInclusive;
            }
            return e;
        }
        public static eMarketProfile ToEntity(this mMarketProfile m)
        {
            var e = new eMarketProfile();
            if (m != null)
            {
                e.MarketProfileId = m.MarketProfileId;
                e.ProfileFor = m.ProfileFor.ToEntityToMarketProfileScope(e);
                e.QuoteTokenMakerFeeId = m.QuoteTokenMakerFeeId;
                e.QuoteTokenTakerFeeId = m.QuoteTokenTakerFeeId;
                e.BaseTokenMakerFeeId = m.BaseTokenMakerFeeId;
                e.BaseTokenTakerFeeId = m.BaseTokenTakerFeeId;
                if (m.QuoteTokenFeeTaxId.HasValue)
                    e.QuoteTokenFeeTaxId = m.QuoteTokenFeeTaxId.Value;
                if (m.BaseTokenFeeTaxId.HasValue)
                    e.BaseTokenFeeTaxId = m.BaseTokenFeeTaxId.Value;
                if (m.QuoteTokenTradeingTaxId.HasValue)
                    e.QuoteTokenTradeingTaxId = m.QuoteTokenTradeingTaxId.Value;
                if (m.BaseTokenTradeingTaxId.HasValue)
                    e.BaseTokenTradeingTaxId = m.BaseTokenTradeingTaxId.Value;
                e.TechConfig = m.TechConfig;
            }
            return e;
        }

        public static List<eMarketProfile> ToEntity(this List<mMarketProfile> m)
        {
            var e = new List<eMarketProfile>();
            if (m != null)
            {
                m.ForEach(x => e.Add(x.ToEntity()));
            }
            return e;
        }
        public static eSupportedToken ToEntity(this mSupportedToken m)
        {
            var e = new eSupportedToken();
            if (m != null)
            {
                e.SupportedTokenId = m.SupportedTokenId;
                e.Code = m.Code;
                e.Narration = m.Narration;
                e.ContractAddress = m.ContractAddress;
                e.IsNative = m.IsNative;
                e.RelatedNetworkId = m.RelatedNetwork.SupportedNetworkId;
            }
            return e;
        }
        public static eToken ToEntity(this mToken m)
        {
            var e = new eToken();
            if (m != null)
            {
                e.TokenId = m.TokenId;
                e.Code = m.Code;
                e.ShortName = m.ShortName;
                e.FullName = m.FullName;
                e.Tick = m.Tick;
                e.Category = m.Category;
                e.FavList = m.FavList;
                e.WatchList = m.WatchList;
                e.WebURL = m.WebURL;
                // e.WhitePaper = m.WhitePaper;
                e.CirculatingSupply = m.CirculatingSupply;
                e.Details = m.Details;
                e.FDMarketCap = m.FDMarketCap;
                e.MarketCap = m.MarketCap;
                e.Volumn = m.Volumn;
            }
            return e;
        }
        public static eFiatCurrency ToEntity(this mFiatCurrency m)
        {
            var e = new eFiatCurrency();
            if (m != null)
            {
                e.FiatCurrencyId = m.FiatCurrencyId;
                e.Code = m.Code;
                e.Name = m.Name;
                e.Description = m.Description;
                e.Symbole = m.Symbole;
                e.Tick = m.Tick;
                e.Profiles = m.Profiles.ToEntity();

                //e.AllowedCountries=m.AllowedCountries.toe
            }
            return e;
        }
        public static eFiatProfile ToEntity(this mFiatProfile m)
        {
            var e = new eFiatProfile();
            if (m != null)
            {
                e.FiatProfileId = m.FiatProfileId;
                e.CountryOrigin = m.CountryOrigin.ToEntity();
                e.IsExchangeAllowed = m.IsExchangeAllowed;
                e.IsP2PAllowed = m.IsP2PAllowed;
                e.BankAccounts = m.BankAccounts.ToEntity();
            }
            return e;
        }
        public static List<eFiatProfile> ToEntity(this List<mFiatProfile> m)
        {
            var e = new List<eFiatProfile>();
            if (m != null)
            {
                m.ForEach(x => e.Add(x.ToEntity()));
            }
            return e;
        }
        public static List<eBankAccount> ToEntity(this List<mBankAccount> m)
        {
            var e = new List<eBankAccount>();
            if (m != null)
            {
                m.ForEach(x => e.Add(x.ToEntity()));
            }
            return e;
        }
        public static eBankAccount ToEntity(this mBankAccount m)
        {
            var e = new eBankAccount();
            if (m != null)
            {
                e.BankAccountId = m.BankAccountId;
                e.BankAccountWallet = m.BankAccountWallet;
                e.BankName = m.BankName;
                e.AccountNumber = m.AccountNumber;
                e.AdditionalInfo = m.AdditionalInfo;
                e.BranchAddress = m.BranchAddress;
                e.LocatedAt = m.LocatedAt.ToEntity();
            }
            return e;
        }
        public static eSupportedNetwork ToEntity(this mSupportedNetwork m)
        {
            var e = new eSupportedNetwork();
            if (m != null)
            {
                e.SupportedNetworkId = m.SupportedNetworkId;
                e.Name = m.Name;
                e.NativeCurrencyCode = m.NativeCurrencyCode;
                e.Description = m.Description;
                e.IsSmartContractEnabled = m.IsSmartContractEnabled;
            }
            return e;
        }
        public static eAddress ToEntity(this mAddress m)
        {
            var e = new eAddress();
            if (m != null)
            {
                e.AddressId = m.AddressId;
                e.UnitNO = m.UnitNo;
                e.StreetAdd = m.StreetAdd;
                e.City = m.City;
                e.PostCode = m.PostCode;
                e.State = m.State;
                e.CountryId = m.CountryId;
            }
            return e;
        }
        public static eAddress ToEntityWithCheck(this mAddress m)
        {
            eAddress e = null;
            if (m != null && m.CountryId != Guid.Empty && m.AddressId == Guid.Empty && m.City.IsNOT_NullorEmpty() && m.State.IsNOT_NullorEmpty() && m.PostCode.IsNOT_NullorEmpty())
            {
                e = new eAddress();
                e.AddressId = m.AddressId;
                e.UnitNO = m.UnitNo;
                e.StreetAdd = m.StreetAdd;
                e.City = m.City;
                e.State = m.State;
                e.PostCode = m.PostCode;
                e.CountryId = m.CountryId;
                e.CreatedOn = DateTime.UtcNow;
            }
            return e;
        }
        public static eBankDepositPaymentMethod ToEntity(this mINRBankDeposit m)
        {
            eBankDepositPaymentMethod e = new eBankDepositPaymentMethod();
            if (m != null)
            {
                e = new eBankDepositPaymentMethod
                {
                    ID = m.ID,
                    AccountHolderName = m.AccountHolderName,
                    IFSCCode = m.IFSCCode,
                    AccountNumber = m.AccountNumber,
                    Bank = m.Bank,
                    BranchAddress = m.BranchAddress,

                    MethodName = m.MethodName,
                    SupportCurrency = m.SupportCurrency,
                    profileId = m.ProfileId,
                    tokenId = m.TokenId
                };
            }
            return e;
        }
        public static eUPIPaymentMethod ToEntity(this mINRUPI m)
        {
            eUPIPaymentMethod e = new eUPIPaymentMethod();
            if (m != null)
            {
                e = new eUPIPaymentMethod
                {
                    ID = m.ID,
                    AccountHolderName = m.AccountHolderName,
                    UPIid = m.UPIid,
                    QRCode=m.QRCode,
                    MethodName = m.MethodName,
                    SupportCurrency = m.SupportCurrency,
                    profileId = m.ProfileId,
                    tokenId = m.TokenId
                };
            }
            return e;
        }
        public static eKYCDocRecord ToEntity(this mKYCDocRecord m)
        {
            eKYCDocRecord e = null;
            if (m == null) return e;
            if (m.KYCDocRecordId == Guid.Empty)
            {
                e = new eKYCDocRecord
                {
                    CategoryId = m.CategoryId,
                    ProfileId = m.ProfileId,
                    CategoryName = m.CategoryName,
                    CountryAbbrivation = m.CountryAbbrivation,
                    DocFileSize = m.DocFileSize,
                    Location = m.Location,
                    InternalName = m.InternalName,
                    IsBack = m.IsBack,
                    IsFront = m.IsFront,
                    MatchId = Guid.NewGuid(),
                    Ext = m.Ext,
                    PlaceHolderId = m.PlaceHolderId,
                    PlaceHolderName = m.PlaceHolderName,
                    PublicName = m.PublicName,
                    Status = m.Status,
                    SessionHash = "??"
                };
            }
            return e;
        }

        public static eKYCDocAdminRecord ToAdminEntity(this mKYCDocRecord m, Guid MatchId)
        {
            eKYCDocAdminRecord e = null;
            if (m == null) return e;
            if (m.KYCDocRecordId == Guid.Empty)
            {
                e = new eKYCDocAdminRecord
                {
                    CategoryId = m.CategoryId,
                    ProfileId = m.ProfileId,
                    CategoryName = m.CategoryName,
                    CountryAbbrivation = m.CountryAbbrivation,
                    DocFileSize = m.DocFileSize,
                    Location = m.Location,
                    InternalName = m.InternalName,
                    IsBack = m.IsBack,
                    IsFront = m.IsFront,
                    MatchId = MatchId,
                    Ext = m.Ext,
                    PlaceHolderId = m.PlaceHolderId,
                    PlaceHolderName = m.PlaceHolderName,
                    PublicName = m.PublicName,
                    data = m.data,
                    Status = (Data.Entity.KYC.eDocumentStatus)m.Status,
                };
            }
            return e;
        }

        public static eProfile ToEntity(this mProfile m)
        {
            eProfile e = null;
            if (m == null) return e;

            if (m.ProfileId == Guid.Empty)
            {
                e = new eProfile();
                e.UserAccountId = m.UserAccountId;
                if (m.DateOfBirth > DateTime.MinValue)
                    e.DateOfBirth = m.DateOfBirth;
                e.FirstName = m.FirstName;
                e.LastName = m.LastName;
                e.Title = m.Title;
                e.gender = m.gender;
                e.NickName = m.NickName;
                e.CreatedOn = DateTime.UtcNow;

                var add = m.Address.ToEntityWithCheck();
                if (add != null)
                    e.CurrentAddress = add;
            }

            return e;
        }
        public static eMobile ToEntity(this mMobile m)
        {
            var e = new eMobile();
            if (e != null)
            {
                e.MobileId = m.Id.ToGUID();
                e.Country = m.Country.ToEntity();
                e.Number = m.Number;
                e.CreatedOn = DateTime.UtcNow;

            }
            return e;
        }
        public static eTaxResidency ToEntity(this mTaxResidency m)
        {
            var e = new eTaxResidency();
            if (e != null)
            {
                e.TaxResidencyId = m.Id.ToGUID();
                e.Country = m.Country.ToEntity();
                e.CreatedOn = DateTime.UtcNow;

            }
            return e;
        }
        public static eCitizenship ToEntity(this mCitizenOf m)
        {
            var e = new eCitizenship();
            if (e != null)
            {
                e.CitizenshipId = m.Id.ToGUID();
                e.Country = m.Country.ToEntity();
                e.CreatedOn = DateTime.UtcNow;
            }
            return e;
        }
        public static eAuthenticator ToEntity(this mAuthenticator m)
        {
            var e = new eAuthenticator();
            if (e != null)
            {
                e.AuthenticatorId = m.AuthenticatorId.ToGUID();
                e.Code = m.Code;
            }
            return e;
        }

        public static eUserAccount xToEntity(this mUser m)
        {
            var e = new eUserAccount();
            if (e != null)
            {
                e.UserAccountId = m.Id.ToGUID();
                //e.AccountNumber = m.AccountNumber;
                e.AuthEmail = m.Email.ToEntity();
                e.Authenticator = m.Authenticator.ToEntity();
                e.TaxResidency = m.TaxResidency.ToEntity();
                e.Mobile = m.Mobile.ToEntity();
                e.IsMultiFactor = m.IsMultiFactor;
                e.IsPrimaryCompleted = m.IsPrimaryCompleted;
                e.IsActive = m.IsActive;
            }
            return e;
        }

        #endregion


    }
}
