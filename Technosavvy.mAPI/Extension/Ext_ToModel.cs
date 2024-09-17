using NavExM.Int.Maintenance.APIs.Data.Entity.Contents;
using NavExM.Int.Maintenance.APIs.Data.Entity.KYC;
using NavExM.Int.Maintenance.APIs.Data.Entity.PreBeta;
using NavExM.Int.Maintenance.APIs.ServerModel;
using NuGet.Protocol;

namespace NavExM.Int.Maintenance.APIs.Extension
{
    public static partial class Ext
    {
        #region ToModel

        public static List<mCategoryDocTemplate> ToModel(this List<eCategory> e)
        {
            var m = new List<mCategoryDocTemplate>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static mCategoryDocTemplate ToModel(this eCategory e)
        {
            var m = new mCategoryDocTemplate();
            if (e != null)
            {
                m = new mCategoryDocTemplate
                {
                    CategoryId = e.CategoryId,
                    ApprovalStatus = (Model.AuthStatus)e.ApprovalStatus,
                    CategoryDesc = e.CategoryDesc,
                    CategoryName = e.CategoryName,
                    CountryAbbr = e.CountryAbbr,
                    CountryName = e.CountryName,
                    DocumentTemplates = e.DocumentTemplates.ToModel(),
                    PassScore = e.PassScore,
                };
            }
            return m;
        }
        public static List<mDocumentTemplate> ToModel(this List<eDocumentTemplate> e)
        {
            var m = new List<mDocumentTemplate>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static mDocumentTemplate ToModel(this eDocumentTemplate e)
        {
            var m = new mDocumentTemplate();
            if (e != null)
            {
                m = new mDocumentTemplate
                {
                    ApprovalStatus = (Model.AuthStatus)e.ApprovalStatus,
                    CategoryId = e.CategoryId,
                    DocInstanceSize = e.DocInstanceSize,
                    DocTemplateDescription = e.DocTemplateDescription,
                    DocTemplateHelpInfo = e.DocTemplateHelpInfo,
                    DocTemplateName = e.DocTemplateName,
                    DocumentTemplateId = e.DocumentTemplateId,
                    IsBackRequired = e.IsBackRequired,
                    IsFrontRequired = e.IsFrontRequired,
                    Score = e.Score
                };
            }
            return m;
        }
        public static List<mPreBetaStages> ToModel(this List<ePreBetaStage> e)
        {
            var m = new List<mPreBetaStages>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static mPreBetaStages ToModel(this ePreBetaStage e)
        {
            var m = new mPreBetaStages();
            if (e != null)
            {
                m.id = e.id;
                m.StageName = e.StageName;
                m.StartDate = e.StartDate;
                m.NavCSellPrice = e.NavCSellPrice;
                m.TokenCap = e.TokenCap;
                if (e.EndDate.HasValue)
                    m.EndDate = e.EndDate;
            }
            return m;
        }
        public static mJD ToModel(this JD e)
        {
            var m = new mJD();
            if (e != null)
            {
                m.id = e.id;
                m.Department = e.Department;
                m.Title = e.JDTitle;
                m.Body = e.JDBody;
                m.RefNo = e.RefNo;
            }
            return m;
        }
        public static List<mPreBetaPurchases> ToModel(this List<ePBMyPurchaseRecords> e)
        {
            var m = new List<mPreBetaPurchases>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static List<mAccruedCashBack> ToModel(this List<eAccruedCashBack> e)
        {
            var m = new List<mAccruedCashBack>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static mAccruedCashBack ToModel(this eAccruedCashBack e)
        {
            var m = new mAccruedCashBack();
            if (e != null)
            {
                m.TradeId = e.TradeId;
                m.CashBackNavCTokens = e.CashBackNavCTokens;
                m.CashBackNavCValue = e.CashBackNavCValue;
                m.Community = e.Community;
                m.MarketCode = e.MarketCode;
                m.RewardTransactionId = e.RewardTransactionId.HasValue ? e.RewardTransactionId.Value : Guid.Empty;
                m.TradeValue = e.TradeValue;
                m.CreatedOn = e.CreatedOn;
            }
            return m;
        }
        public static mPreBetaPurchases ToModel(this ePBMyPurchaseRecords e)
        {
            var m = new mPreBetaPurchases();
            if (e != null)
            {
                m.id = e.id;
                m.BuyWith = e.BuyWith;
                m.DateOf = e.DateOf;
                m.NavCAmountPurchased = e.NavCAmountPurchased;
                m.TranHash = e.TranHash;
                m.WalletAddress = e.WalletAddress;
                m.NavCUnitRate = e.NavCUnitRate;
                m.TokenToUnitRate = e.TokenToUnitRate;

            }
            return m;
        }
        public static List<mFAQDisplay> ToModel(this List<FAQ> e)
        {
            var m = new List<mFAQDisplay>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static mFAQDisplay ToModel(this FAQ e)
        {
            var m = new mFAQDisplay();
            if (e != null)
            {
                m.id = e.id;
                m.QuestionText = e.QuestionText;
                m.GroupTitle = e.GroupTitle;
                m.AnswerText = e.AnswerText;
                m.OrderNo = e.OrderNo;
            }
            return m;
        }
        public static mSupportedNetwork ToModel(this eSupportedNetwork e)
        {
            var m = new mSupportedNetwork();
            if (e != null)
            {
                m.SupportedNetworkId = e.SupportedNetworkId;
                m.Name = e.Name;
                m.Description = e.Description;
                m.NativeCurrencyCode = e.NativeCurrencyCode;
                m.IsSmartContractEnabled = e.IsSmartContractEnabled;
            }
            return m;
        }
        public static mStake ToModel(this eStaking e)
        {
            var m = new mStake();
            if (e != null)
            {
                m.StakeId = e.StakingId;
                m.fromFundWalletId = e.fromFundWalletId;
                m.FromTransactionId = e.FromTransactionId;
                m.Amount = e.Amount;
                m.MatureAmount = e.MatureAmount;
                m.StartedOn = e.StartedOn;
                m.ExpectedEndData = e.ExpectedEndData;
                m.AutoRenew = e.AutoRenew;
                m.userAccountId = e.userAccountId;
                m.StakingOpportunityId = e.StakingOpportunityId;
                m.StakingSlot = e.StakingOpportunity.ToModel2();
            }
            return m;
        }
        public static List<mStake> ToModel(this List<eStaking> e)
        {
            var m = new List<mStake>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static mToken ToModel(this eToken e)
        {
            var m = new mToken();
            if (e != null)
            {
                m.TokenId = e.TokenId;
                m.ShortName = e.ShortName;
                m.FullName = e.FullName;
                m.Code = e.Code;
                m.Category = e.Category;
                m.FavList = e.FavList;
                m.WatchList = e.WatchList;
                m.IsFiatRepresentative = e.IsFiatRepresentative;
                m.WebURL = e.WebURL;
                if (e.WhitePaper.IsNOT_NullorEmpty())
                {
                    m.Attr = System.Text.Json.JsonSerializer.Deserialize<mTokenAttribute>(e.WhitePaper);
                    m.WhitePaper = m.Attr.WhitePaperURL;
                }
                m.CirculatingSupply = e.CirculatingSupply;
                m.Details = e.Details;
                m.FDMarketCap = e.FDMarketCap;
                m.MarketCap = e.MarketCap;
                m.Volumn = e.Volumn;
                m.SupportedCoin = e.SupportedCoin.ToModel();
                m.Tick = e.Tick;
                m.AllowedCountries = e.AllowedCountries.ToModel();

            }
            return m;
        }
        public static mTokenNetworkFee ToModel(this eTokenNetworkFee? e)
        {
            var m = new mTokenNetworkFee();
            if (e != null)
            {
                m.Token = e.Token.ToModel();
                m.SupportedNetwork = e.SupportedNetwork.ToModel();
                m.MaxWithdrawal = e.MaxWithdrawal;
                m.MinWithdrawal = e.MinWithdrawal;
                m.DepositFee = e.DepositFee;
                m.WithdrawalFee = e.WithdrawalFee;
                m.IsDepositAllowed = e.IsDepositAllowed;
                m.IsWithdrawalAllowed = e.IsWithdrawalAllowed;
                m.LastUpdatedOn = e.ModifiedOn.HasValue ? e.ModifiedOn.Value : e.CreatedOn;
            }
            return m;
        }
        public static mSupportedCountry ToModel(this eSupportedCountry e)
        {
            var m = new mSupportedCountry();
            if (e != null)
            {
                m.SupportedCountryId = e.SupportedCountryId;
                m.SupportedSince = e.SupportedSince;
                m.SupportEndedOn = e.SupportEndedOn;
                m.Notes = e.Notes;
                m.Country = e.Country.ToModel();
            }
            return m;
        }
        public static List<mSupportedCountry> ToModel(this List<eSupportedCountry> e)
        {
            var m = new List<mSupportedCountry>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static List<mStakingSlot2> ToModel2(this List<eStakingOpportunity> e)
        {
            var m = new List<mStakingSlot2>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel2()));
            }
            return m;
        }

        public static mStakingSlot2 ToModel2(this eStakingOpportunity e)
        {
            var m = new mStakingSlot2();
            if (e != null)
            {
                m.StakingOpportunityId = e.StakingOpportunityId;
                m.GroupId = e.GroupId;
                m.GroupDetails = e.GroupDetails;
                m.GroupName = e.GroupName;
                m.TokenId = e.TokenId;
                m.Name = e.Name;
                m.Community = e.Community;
                m.TotalTarget = e.TotalTarget;
                m.IsSunSet = e.IsSunSet;
                m.OfferExpiredOn = e.OfferExpiredOn;
                m.OfferShouldExpierOn = e.OfferExpiredOn;
                m.OfferStartedOn = e.OfferStartedOn;
                m.AutoRenewAllowed = e.AutoRenewAllowed;
                m.AYPOffered = e.AYPOffered;
                m.Details = e.Details;
                m.Duration = e.Duration;
                m.Token = e.Token.ToModel();
                m.IsHardFixed = e.IsHardFixed;
                m.MaxAmount = e.MaxAmount.HasValue ? e.MaxAmount!.Value : -1;
                m.MinAmount = e.MinAmount.HasValue ? e.MinAmount!.Value : -1;
            }
            return m;
        }
        public static mStakingSlot ToModel(this List<eStakingOpportunity> e)
        {
            var m = new mStakingSlot();
            m.Instances = new List<mStakingSlotInstance>();
            if (e != null)
            {

                foreach (var x in e)
                {
                    m.StakingSlotId = x.GroupId;
                    m.GroupId = x.GroupId;
                    m.GroupDetails = x.GroupDetails;
                    m.GroupName = x.GroupName;
                    m.TokenId = x.TokenId;
                    m.TotalTarget = x.TotalTarget;
                    m.IsSunSet = x.IsSunSet;
                    m.OfferExpiredOn = x.OfferExpiredOn;
                    m.OfferShouldExpierOn = x.OfferExpiredOn;
                    m.OfferStartedOn = x.OfferStartedOn;
                    m.Instances.Add(new mStakingSlotInstance()
                    {
                        AutoRenewAllowed = x.AutoRenewAllowed,
                        AYPOffered = x.AYPOffered,
                        Details = x.Details,
                        Duration = x.Duration,
                        IsHardFixed = x.IsHardFixed,
                        MaxAmount = x.MaxAmount.HasValue ? x.MaxAmount!.Value : -1,
                        MinAmount = x.MinAmount.HasValue ? x.MinAmount!.Value : -1,
                        Name = x.Name,
                        StakingSlotId = x.StakingOpportunityId
                    });
                }
            }
            return m;
        }
        public static List<mSupportedNetwork> ToModel(this List<eSupportedNetwork> e)
        {
            var m = new List<mSupportedNetwork>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static List<mToken> ToModel(this List<eToken> e)
        {
            var m = new List<mToken>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static List<mTokenNetworkFee> ToModel(this List<eTokenNetworkFee> e)
        {
            var m = new List<mTokenNetworkFee>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }

        public static mSupportedToken ToModel(this eSupportedToken e)
        {
            var m = new mSupportedToken();
            if (e != null)
            {
                m.SupportedTokenId = e.SupportedTokenId;
                m.Code = e.Code;
                m.Narration = e.Narration;
                m.IsNative = e.IsNative;
                m.ContractAddress = e.ContractAddress;
                m.RelatedNetwork = e.RelatedNetwork.ToModel();
            }
            return m;
        }
        public static List<mSupportedToken> ToModel(this List<eSupportedToken> e)
        {
            var m = new List<mSupportedToken>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static mNetworkWallet ToModel(this eFundingNetworkWallet e)
        {
            var m = new mNetworkWallet();
            if (e != null)
            {
                m.NetworkId = e.NetworkWalletAddress.NetworkId;
                m.Address = e.NetworkWalletAddress.Address;
                m.NetworkWalletId = e.NetworkWalletAddressId;
                m.NetworkName = e.NetworkWalletAddress.Network.Name;
                if (e.NetworkWalletAddress.Category == eNetWorkWalletCategory.NavExMSmartContract)
                    m.WType = WalletType.SmartContract;
                else
                    m.WType = WalletType.Standard;

                m.AllocateOn = e.AllocatedOn;
            }
            return m;

        }
        public static mWatchRequest ToDataModel(this eNetworkWalletAddressWatch e)
        {
            var m = new mWatchRequest();
            if (e != null)
            {
                m.RequestId = e.NetworkWalletAddressWatchId;
                m.TokenContractAddress = e.SupportedToken.ContractAddress;
                m.TokenName = e.SupportedToken.Code;
                m.WalletAddress = e.NetworkWalletAddress.Address;
                m.ExpectedAmount = e.ExpectedAmount;
                m.NetworkWalletAddressId = e.NetworkWalletAddressId;
                m.SupportedTokenId = e.SupportedTokenId;
            }
            return m;
        }
        public static mEmail ToModel(this eAuthorizedEmail e)
        {
            var m = new mEmail();
            if (e != null)
            {
                m.To = e.Email;
            }
            return m;
        }
        public static mWalletTransactions ToModel(this eWalletTransaction e)
        {
            var m = new mWalletTransactions();
            if (e != null)
            {
                m.TransactionId = e.WalletTransactionId;
                m.Date = e.Date;
                m.Narration = e.Narration;
                m.Amount = e.TAmount;
            }
            return m;
        }

        public static List<mWalletTransactions> ToModel(this List<eWalletTransaction> e)
        {
            var m = new List<mWalletTransactions>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static List<mCountry> ToModel(this List<eCountry> e)
        {
            var m = new List<mCountry>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static mCountry ToModel(this eCountry e)
        {
            var m = new mCountry();
            if (e != null)
            {
                m.CountryId = e.CountryId;
                m.DialCode = e.DialCode;
                m.Name = e.Name;
                m.Abbrivation = e.Abbrivation;
                m.Continent = e.Continent!;
                m.Block = e.Block!;
            }
            return m;
        }
        public static mAuthenticator ToModel(this eAuthenticator e)
        {
            var m = new mAuthenticator();

            if (e != null)
            {
                m.AuthenticatorId = e.AuthenticatorId.ToString();
                m.Code = e.Code;
            }
            return m;

        }
        public static mMobile ToModel(this eMobile e)
        {
            var m = new mMobile();
            if (e != null)
            {
                m.Id = e.MobileId.ToString();
                m.Country = Check<eCountry>(e.Country).ToModel();
                m.Number = e.Number;
            }
            return m;
        }
        public static mTaxResidency ToModel(this eTaxResidency e)
        {
            var m = new mTaxResidency();
            if (e != null)
            {
                m.Id = e.TaxResidencyId.ToString();
                m.Country = Check<eCountry>(e.Country).ToModel();
            }
            return m;
        }
        public static mCitizenOf ToModel(this eCitizenship e)
        {
            var m = new mCitizenOf();
            if (e != null)
            {
                m.Id = e.CitizenshipId.ToString();
                m.Country = Check<eCountry>(e.Country).ToModel();
            }
            return m;
        }

        public static List<mUserWallet> ToWalletModel(this List<eUserAccount> e)
        {
            var m = new List<mUserWallet>();
            if (e != null)
            {
                foreach (var o in e)
                {
                    m.Add(o.ToWalletModel());
                }
            }
            return m;
        }
        public static List<mAlertMsgBody> ToModel(this List<eUserAlertMsg> e) {
            var m = new List<mAlertMsgBody>();
            if (e != null)
            {
                foreach (var item in e)
                {
                    m.Add(item.ToModel());
                }
            }
            return m;
        }
        public static mAlertMsgBody ToModel(this eUserAlertMsg e)
        {
            var m = new mAlertMsgBody();
            if (e != null)
            {
                m.Body = e.MsgBody;
                m.Title = e.MsgTitle;
                m.GeneratedOn = e.GeneratedOn;
                m.ViewOn = e.ReportedOn;
            }
            return m;
        }
        public static mUser ToModel(this eUserAccount e)
        {
            var m = new mUser();
            if (e != null)
            {
                m.Id = e.UserAccountId.ToString();
                m.Email = Check<eAuthorizedEmail>(e.AuthEmail).ToModel();
                m.AccountNumber = e.FAccountNumber;
                m.RefCodes = Check<eRefCodes>(e.RefCode).ToModel();
                m.Profile = Check<eProfile>(e.UserProfile).ToModel();
                m.TaxResidency = Check<eTaxResidency>(e.TaxResidency).ToModel();
                m.CitizensOf = Check<eCitizenship>(e.CitizenOf).ToModel();
                m.Mobile = Check<eMobile>(e.Mobile).ToModel();
                m.Authenticator = Check<eAuthenticator>(e.Authenticator).ToModel();
                m.IsActive = e.IsActive;
                m.IsPrimaryCompleted = e.IsPrimaryCompleted;
                m.IsMultiFactor = e.IsMultiFactor;
            }
            return m;
        }
        public static mUserWallet ToWalletModel(this eUserAccount e)
        {
            var m = new mUserWallet();
            if (e != null)
            {
                m.Id = e.UserAccountId.ToString();
                m.Email = Check<eAuthorizedEmail>(e.AuthEmail).ToModel().To;
                m.AccountNumber = e.FAccountNumber;
                m.IsActive = e.IsActive;
                m.RefCodes = Check<eRefCodes>(e.RefCode).ToModel();

                if (e.SpotWalletId.HasValue)
                    m.SpotWallet = new mWalletSummery() { WalletId = e.SpotWalletId.Value };

                if (e.FundingWalletId.HasValue)
                    m.FundingWallet = new mWalletSummery() { WalletId = e.FundingWalletId.Value };

                if (e.EarnWalletId.HasValue)
                    m.EarnWallet = new mWalletSummery() { WalletId = e.EarnWalletId.Value };

                if (e.EscrowWalletId.HasValue)
                    m.EscrowWallet = new mWalletSummery() { WalletId = e.EscrowWalletId.Value };

                if (e.HoldingWalletId.HasValue)
                    m.HoldingWallet = new mWalletSummery() { WalletId = e.HoldingWalletId.Value };

            }
            return m;
        }
        public static mRefCodes ToModel(this eRefCodes e)
        {
            var m = new mRefCodes();
            if (e != null)
            {
                m.RefferedBy = e.RefferedBy;
                m.myCommunity = e.myCommunity;
                //ToDo: eRefReward To Model
            }
            return m;
        }
        public static mProfile ToModel(this eProfile e)
        {
            var m = new mProfile();
            if (e != null)
            {
                m.ProfileId = e.ProfileId;
                m.UserAccountId = e.UserAccountId;
                m.FirstName = e.FirstName;
                m.LastName = e.LastName;
                m.Title = e.Title;
                m.gender = e.gender;
                m.NickName = e.NickName;
                m.DateOfBirth = e.DateOfBirth.HasValue ? e.DateOfBirth.Value : DateTime.MinValue;
                m.KYCStatus = e.KYCStatus;
                if (e?.UserAccount?.TaxResidency?.Country != null)
                {
                    m.TaxResidencyId = e.UserAccount.TaxResidency.Country.CountryId;
                    m.TaxResidency = e.UserAccount.TaxResidency.Country.ToModel();
                }
                if (e?.UserAccount?.CitizenOf?.Country != null)
                    m.CitizenshipId = e.UserAccount.CitizenOf.Country.CountryId;
                m.Address = e.CurrentAddress.ToModel();
            }
            return m;
        }
        public static mAddress ToModel(this eAddress e)
        {
            var m = new mAddress();
            if (e != null)
            {
                m.AddressId = e.AddressId;
                m.UserAccountId = e.UserAccountId;
                m.UnitNo = e.UnitNO;
                m.StreetAdd = e.StreetAdd;
                m.State = e.State;
                m.PostCode = e.PostCode;
                m.City = e.City;
                m.CountryId = e.CountryId;
            }
            return m;
        }
        public static mUserSession ToModel(this eUserSession e)
        {
            var m = new mUserSession();
            if (e != null)
            {
                m.UserSessionId = e.UserSessionId;
                m.UserAccount = e.UserAccount.ToModel();
                m.StartedOn = e.StartedOn;
                m.ShouldExpierOn = e.ShouldExpierOn;
                m.SessionHash = e.SessionHash;
                m.CreatedBy = e.CreatedBy;
                m.CreatedOn = e.CreatedOn;
                m.ExpieredOn = e.ExpieredOn;
                m.SpotWalletId = e.UserAccount.SpotWalletId!.Value;
                m.EscrowWalletId = e.UserAccount.EscrowWalletId!.Value;
                m.FundingWalletId = e.UserAccount.FundingWalletId!.Value;
                m.HoldingWalletId = e.UserAccount.HoldingWalletId!.Value;
                m.EarnWalletId = e.UserAccount.EarnWalletId!.Value;
                m.SessionAuthEvent = e.SessionAuthEvent.ToModel();
            }
            return m;
        }
        public static mEnumBoxData ToModel(this eEnumBoxData e)
        {
            var m = new mEnumBoxData();
            if (e != null)
            {
                m.Id = e.EnumBoxDataId;
                m.Name = e.Name;
                m.EnumType = e.EnumType;
                m.EnumValue = e.EnumValue;
                m.VersionNo = e.VersionNo;
                m.SortingOrderId = e.Id;
            }
            return m;
        }
        //public static mKYCRecord ToModel(this eKYCRecord e)
        //{
        //    var m = new mKYCRecord();
        //    if (e != null)
        //    {
        //        m.KYCRecordId = e.eKYCRecordId;
        //        m.UserAccount = e.UserAccount.ToModel();
        //        m.KYCStatus = e.KYCStatus.ToModel();
        //        m.CountryOfCitizenship = e.CountryOfCitizenship.ToModel();
        //        m.CountryOfResidence = e.CountryOfResidence.ToModel();
        //        m.IsCompleted = e.IsCompleted;
        //        m.IsDeclarationAccepted = e.IsDeclarationAccepted;
        //        m.TaxIdentificationNumber = e.TaxIdentificationNumber;
        //        m.TaxIDNType = e.TaxIDNType.ToModel();
        //        m.KYCDocuments = e.KYCDocuments.ToModel();
        //    }
        //    return m;

        //}
        //public static mKYCDocuTemplate ToModel(this eKYCDocuTemplate e)
        //{
        //    var m = new mKYCDocuTemplate();
        //    if (e != null)
        //    {
        //        m.KYCDocuTemplateId = e.KYCDocuTemplatesId;
        //        m.CountryApplicable = e.CountryApplicable.ToModel();
        //        m.Category = e.Category;
        //        m.Group = e.Group;
        //        m.DocPlaceHolder = e.DocPlaceHolder;
        //        m.AllInGroup = e.AllInGroup;
        //        m.AnyInGroup = e.AnyInGroup;

        //    }
        //    return m;
        //}
        public static List<mKYCDocRecord> ToModel(this List<eKYCDocRecord> e)
        {
            var m = new List<mKYCDocRecord>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static mKYCDocRecord ToModel(this eKYCDocRecord e)
        {
            var m = new mKYCDocRecord();
            if (e != null)
            {
                m = new mKYCDocRecord
                {
                    CategoryId = e.CategoryId,
                    CategoryName = e.CategoryName,
                    CountryAbbrivation = e.CountryAbbrivation,
                    DocFileSize = e.DocFileSize,
                    Ext = e.Ext,
                    InternalName = e.InternalName,
                    IsBack = e.IsBack,
                    IsFront = e.IsFront,
                    KYCDocRecordId = e.eKYCDocRecordId,
                    Location = e.Location,
                    PlaceHolderId = e.PlaceHolderId,
                    PlaceHolderName = e.PlaceHolderName,
                    PublicName = e.PublicName,
                    ProfileId = e.ProfileId,
                    Status = e.Status,
                    CreatedOn = e.CreatedOn
                };

            }
            return m;
        }
        public static mAuth ToModel(this eAuthenticationEvent e)
        {
            var m = new mAuth();
            if (e != null)
            {
                m.mAuthId = e.AuthenticationEventId;
                m.userAccountID = e.UserAccount.UserAccountId;
                m.userName = e.UserAccount!.AuthEmail!.Email;
                m.AccountNumber = e.UserAccount!.FAccountNumber!;
                m.SessionHash = e.SessionHash ?? String.Empty;
                m.Is2FAuthenticator = e.Is2FAuthenticator;
                m.Is2FEmail = e.Is2FEmail;
                m.Is2FMobile = e.Is2FMobile;
                m.IsPasswordAuth = e.IsPasswordAuth;
                m.IsUserNameAuth = e.IsUserNameAuth;
                m.StartedOn = e.CreatedOn;
                m.MultiFact_Enabled = e.IsMultiFactor;
            }
            return m;
        }
        //----Starts here--
        public static List<mMarket> ToModel(this List<eMarket> e)
        {
            var m = new List<mMarket>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static mMarket ToModel(this eMarket e)
        {
            var m = new mMarket();
            if (e != null)
            {
                m.MarketId = e.MarketId;
                m.MarketType = e.MarketType;
                m.ShortName = e.ShortName;
                m.CodeName = e.CodeName;
                m.isPrivate = e.isPrivate;
                m.IsCommunity = e.IsCommunity;
                m.IsMarketMakingAccount = e.IsMarketMakingAccount;
                m.MinOrderSizeValueUSD = e.MinOrderSizeValueUSD;
                m.MinBaseOrderTick = e.MinBaseOrderTick;
                m.MinQuoteOrderTick = e.MinQuoteOrderTick;
                m.BaseTokenId = e.BaseToken != null ? e.BaseToken.TokenId : Guid.Empty;
                m.BaseToken = e.BaseToken.ToModel();// != null ? e.BaseToken.TokenId : Guid.Empty;
                m.QuoteTokenId = e.QuoteToken.ToModel().TokenId;//!= null ? e.QuoteToken.TokenId : Guid.Empty;
                m.QuoteToken = e.QuoteToken.ToModel();//!= null ? e.QuoteToken.TokenId : Guid.Empty;
                m.QuoteCurrencyId = e.QuoteCurrency.ToModel().FiatCurrencyId;//!= null ? e.QuoteCurrency.FiatCurrencyId : Guid.Empty;
                m.QuoteCurrency = e.QuoteCurrency.ToModel();//!= null ? e.QuoteCurrency.FiatCurrencyId : Guid.Empty;
                m.IsTradingAllowed = e.IsTradingAllowed;
                m.MarketProfile = e.MarketProfile.ToModel();
                m.Attributes = e.Attributes.ToModel();
            }
            return m;
        }
        public static mMarketAttributes ToModel(this eMarketAttributes e)
        {
            var m = new mMarketAttributes();
            if (e != null)
            {
                m.MarketAttributesId = e.MarketAttributesId;
                m.HighlightTill = e.HighlightTill;
                m.HighlightFrom = e.HighlightFrom;
                m.NewListingTill = e.NewListingTill;
                m.NewListingFrom = e.NewListingFrom;
                m.CreatedOn = e.CreatedOn;
                m.UpdatedOn = e.UpdatedOn;
            }
            return m;
        }
        public static mTradingFee ToModel(this eTradingFee e)
        {
            var m = new mTradingFee();
            if (e != null)
            {
                m.TokenFeeId = e.TokenFeeId;
                m.FeeCommunity = e.FeeCommunity;
                m.FeeNonCommunity = e.FeeNonCommunity;
                m.FeeExempt = e.FeeExempt;
                m.FeeIndependent = e.FeeIndependent;
                m.FeeName = e.FeeName;
                m.Details = e.Details;
                m.DisplayAsSwap = e.DisplayAsSwap;
            }
            return m;
        }
        public static mTax ToModel(this eTax e)
        {
            var m = new mTax();
            if (e != null)
            {
                m.TaxId = e.TaxId;
                m.Rate = e.Rate;
                m.TaxName = e.TaxName;
                m.Details = e.Details;
                m.isInclusive = e.isInclusive;
            }
            return m;
        }
        public static mCountry ToModelFromProfile(this eMarketProfileScope e)
        {
            var m = new mCountry();
            if (e != null)
            {
                m = (e.Country.ToModel());
            }
            return m;
        }
        public static List<mCountry> ToModelFromProfile(this List<eMarketProfileScope> e)
        {
            var m = new List<mCountry>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.Country.ToModel()));
            }
            return m;
        }

        public static List<mCountry> ToModel(this eMarketProfileScope e)
        {
            var m = new List<mCountry>();
            if (e != null)
            {
                m.Add(e.Country.ToModel());
            }
            return m;
        }
        public static mMarketProfile ToModel(this eMarketProfile e)
        {
            var m = new mMarketProfile();
            if (e != null)
            {
                m.MarketProfileId = e.MarketProfileId;
                m.ProfileFor = e.ProfileFor.ToModelFromProfile();
                m.QuoteTokenMakerFeeId = e.QuoteTokenMakerFeeId.GVC();
                m.QuoteTokenTakerFeeId = e.QuoteTokenTakerFeeId.GVC();
                m.BaseTokenMakerFeeId = e.BaseTokenMakerFeeId.GVC();//.TokenFeeId;
                m.BaseTokenTakerFeeId = e.BaseTokenTakerFeeId.GVC();//.TokenFeeId;
                m.QuoteTokenFeeTaxId = e.QuoteTokenFeeTaxId.GVC();//.TaxId;
                m.BaseTokenFeeTaxId = e.BaseTokenFeeTaxId.GVC();//.TaxId;
                m.QuoteTokenTradeingTaxId = e.QuoteTokenTradeingTaxId.GVC();//.TaxId;
                m.BaseTokenTradeingTaxId = e.BaseTokenTradeingTaxId.GVC();//.TaxId;
                m.TechConfig = e.TechConfig;
            }
            return m;
        }
        public static List<mMarketProfile> ToModel(this List<eMarketProfile> e)
        {
            var m = new List<mMarketProfile>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static mFiatCurrency ToModel(this eFiatCurrency? e)
        {
            var m = new mFiatCurrency();
            if (e != null)
            {
                m.FiatCurrencyId = e.FiatCurrencyId;
                m.Code = e.Code;
                m.Name = e.Name;
                m.Description = e.Description;
                m.Symbole = e.Symbole;
                m.Tick = e.Tick;
                m.Profiles = e.Profiles.ToModel();

            }
            return m;
        }
        public static mFiatProfile ToModel(this eFiatProfile e)
        {
            var m = new mFiatProfile();
            if (e != null)
            {
                m.FiatProfileId = e.FiatProfileId;
                m.CountryOrigin = e.CountryOrigin.ToModel();
                m.IsExchangeAllowed = e.IsExchangeAllowed;
                m.IsP2PAllowed = e.IsP2PAllowed;
                m.BankAccounts = e.BankAccounts.ToModel();
            }
            return m;
        }
        public static List<mFiatCurrency> ToModel(this List<eFiatCurrency> e)
        {
            var m = new List<mFiatCurrency>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static List<mFiatProfile> ToModel(this List<eFiatProfile> e)
        {
            var m = new List<mFiatProfile>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static List<mBankAccount> ToModel(this List<eBankAccount> e)
        {
            var m = new List<mBankAccount>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static mBankAccount ToModel(this eBankAccount e)
        {
            var m = new mBankAccount();
            if (e != null)
            {
                m.BankAccountId = e.BankAccountId;
                m.BankAccountWallet = e.BankAccountWallet;
                m.BankName = e.BankName;
                m.AccountNumber = e.AccountNumber;
                m.AdditionalInfo = e.AdditionalInfo;
                m.BranchAddress = e.BranchAddress;
                m.LocatedAt = e.LocatedAt.ToModel();
                m.PaymentMethod = GetPaymentMethod(e.AdditionalInfo);

            }
            return m;
        }
        public static List<mINRBankDeposit> ToModel(this List<eBankDepositPaymentMethod> e)
        {
            var m = new List<mINRBankDeposit>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static mINRBankDeposit ToModel(this eBankDepositPaymentMethod e)
        {
            mINRBankDeposit m = new mINRBankDeposit();
            if (e != null)
            {
                m = new mINRBankDeposit
                {
                    ID = e.ID,
                    AccountHolderName = e.AccountHolderName,
                    IFSCCode = e.IFSCCode,
                    AccountNumber = e.AccountNumber,
                    Bank = e.Bank,
                    BranchAddress = e.BranchAddress,
                    MethodName = e.MethodName,
                    SupportCurrency = e.SupportCurrency,
                    ProfileId = e.profileId,
                    TokenId = e.tokenId,
                    DeletedOn = e.DeletedOn.HasValue ? e.DeletedOn.Value : DateTime.MaxValue
                };
            }
            return m;
        }
        public static List<mINRUPI> ToModel(this List<eUPIPaymentMethod> e)
        {
            var m = new List<mINRUPI>();
            if (e != null)
            {
                e.ForEach(x => m.Add(x.ToModel()));
            }
            return m;
        }
        public static mINRUPI ToModel(this eUPIPaymentMethod e)
        {
            mINRUPI m = new mINRUPI();
            if (e != null)
            {
                m = new mINRUPI
                {
                    ID = e.ID,
                    AccountHolderName = e.AccountHolderName,
                    UPIid = e.UPIid,
                    MethodName = e.MethodName,
                    SupportCurrency = e.SupportCurrency,
                    ProfileId = e.profileId,
                    TokenId = e.tokenId,
                    DeletedOn = e.DeletedOn.HasValue ? e.DeletedOn.Value : DateTime.MaxValue
                };
            }
            return m;
        }

        #endregion
        public static List<IPaymentMethod> GetPaymentMethod(string AdditionInfo = "")
        {
            var ret = GetAllPaymentMethod();
            if (AdditionInfo.NotIsNullOrEmpty() && AdditionInfo.StartsWith("["))
            {
                var lst = System.Text.Json.JsonSerializer.Deserialize<List<string>>(AdditionInfo);

                foreach (var item in lst)
                {
                    var o = PaymentClassFactory.GetClassObject(item);
                    if (o != null)
                    {
                        var v = (IPaymentMethod)o;
                        var p = ret.FirstOrDefault(x => x.SupportCurrency == v.SupportCurrency && x.MethodName == v.MethodName);
                        if (p != null) ret.Remove(p);
                        ret.Add(v);
                    }
                }
            }
            else
            {
                return GetAllPaymentMethod();
            }
            return ret;
        }
        public static List<IPaymentMethod> GetAllPaymentMethod()
        {
            //Add all payments methods object here regardless of Currency
            return new List<IPaymentMethod>
            {
            new mUPIPaymentMethod(),
             new mBankDepositPaymentMethod()
            };
        }
        public static object GetClassObject(string str)
        {
            object obj = null;
            var o = getClassObject<mUPIPaymentMethod>(str);
            if (o != null && !o.ID.IsGuidNullorEmpty() && o.MethodName == "UPI") return o;

            var o2 = getClassObject<mBankDepositPaymentMethod>(str);
            if (o2 != null && !o2.ID.IsGuidNullorEmpty() && o2.MethodName == "BankDeposit") return o2;

            return obj;
        }
        static T getClassObject<T>(string str)
        {
            var obj = System.Text.Json.JsonSerializer.Deserialize<T>(str);
            return obj;
        }
    }
    public static class PaymentClassFactory
    {
        public static string GetClassJson(List<IPaymentMethod> obj)
        {
            List<string> strAll = new List<string>();
            foreach (var o in obj)
            {
                if (o.GetType() == typeof(mUPIPaymentMethod))
                {
                    var str = System.Text.Json.JsonSerializer.Serialize((mUPIPaymentMethod)o);
                    strAll.Add(str);
                }
                if (o.GetType() == typeof(mBankDepositPaymentMethod))
                {
                    var str = System.Text.Json.JsonSerializer.Serialize((mBankDepositPaymentMethod)o);
                    strAll.Add(str);
                }
            }
            return strAll.ToJson();
        }
        public static object GetClassObject(string str)
        {
            object obj = null;
            var o = getClassObject<mUPIPaymentMethod>(str);
            if (o != null && !o.ID.IsGuidNullorEmpty() && o.MethodName == "UPI") return o;

            var o2 = getClassObject<mBankDepositPaymentMethod>(str);
            if (o2 != null && !o2.ID.IsGuidNullorEmpty() && o2.MethodName == "BankDeposit") return o2;

            return obj;
        }
        static T getClassObject<T>(string str)
        {
            var obj = System.Text.Json.JsonSerializer.Deserialize<T>(str);
            return obj;
        }
    }
}
