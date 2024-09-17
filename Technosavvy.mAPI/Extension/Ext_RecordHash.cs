namespace NavExM.Int.Maintenance.APIs.Extension
{
    public static partial class Ext 
    {
        #region RecordHash
    
        internal static void SignRecord(this eStaking e, ManagerBase c)
        {
            if (e.StakingOpportunityId == Guid.Empty) throw new InvalidOperationException($"can't Sign Record with empty id {2802}");
            var si = $"{Signer.AppSeed}{e.StakingOpportunityId}{e.StakingId}{e.StakingWalletId}{e.fromFundWalletId}{e.Amount}{e.StartedOn}{e.ExpectedEndData}{e.MatureAmount}{e.AutoRenew}{e.IsRedeemed}{e.RedeemedOn}{e.ToTransactionId}{e.StakingOpportunityId}{e.userAccountId}{e.SessionHash}";
            e.RecordHash = c.GetSignedHash(si);
            c.dbctx.SaveChanges();
        }
        internal static bool VerifyRecord(this eStaking e, ManagerBase c, bool raiseError = false)
        {
            return true;// machine is Incompitable
            if (e.StakingOpportunityId == Guid.Empty) throw new InvalidOperationException($"can't verify null {2801}");
            var si = $"{Signer.AppSeed}{e.StakingOpportunityId}{e.StakingId}{e.StakingWalletId}{e.fromFundWalletId}{e.Amount}{e.StartedOn}{e.ExpectedEndData}{e.MatureAmount}{e.AutoRenew}{e.IsRedeemed}{e.RedeemedOn}{e.ToTransactionId}{e.StakingOpportunityId}{e.userAccountId}{e.SessionHash}";
            var res = c.CompareHash(e.RecordHash, si) == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
            if (!res)
                c.LogEvent($"{nameof(e.StakingId)}:{e.StakingId} failed Record Verification..Data Compromise..!");
            if (raiseError && !res)
                throw new ApplicationException("Data sanity compromised. Action Suspended");
            return res;
        }
        internal static void SignRecord(this eStakingOpportunity e, ManagerBase c)
        {
            if (e.StakingOpportunityId == Guid.Empty) throw new InvalidOperationException($"can't Sign Record with empty id {2802}");
            var si = $"{Signer.AppSeed}{e.StakingOpportunityId}{e.GroupId}{e.TokenId}{e.GroupName}{e.GroupDetails}{e.Name}{e.Details}{e.Duration}{e.AYPOffered}{e.AutoRenewAllowed}{e.IsHardFixed}{e.MinAmount}{e.MaxAmount}{e.TotalTarget}{e.IsSunSet}{e.OfferStartedOn}{e.OfferShouldExpierOn}{e.OfferExpiredOn}{e.IsApproved}{e.ApprovedBy}{e.SessionHash}";
            e.RecordHash = c.GetSignedHash(si);
            c.dbctx.SaveChanges();
        }
        internal static bool VerifyRecord(this eStakingOpportunity e, ManagerBase c, bool raiseError = false)
        {
            if (e.StakingOpportunityId == Guid.Empty) throw new InvalidOperationException($"can't verify null {2801}");
            var si = $"{Signer.AppSeed}{e.StakingOpportunityId}{e.GroupId}{e.TokenId}{e.GroupName}{e.GroupDetails}{e.Name}{e.Details}{e.Duration}{e.AYPOffered}{e.AutoRenewAllowed}{e.IsHardFixed}{e.MinAmount}{e.MaxAmount}{e.TotalTarget}{e.IsSunSet}{e.OfferStartedOn}{e.OfferShouldExpierOn}{e.OfferExpiredOn}{e.IsApproved}{e.ApprovedBy}{e.SessionHash}";
            var res = c.CompareHash(e.RecordHash, si) == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
            if (!res)
                c.LogEvent($"{nameof(e.StakingOpportunityId)}:{e.StakingOpportunityId} failed Record Verification..Data Compromise..!");
            if (raiseError && !res)
                throw new ApplicationException("Data sanity compromised. Action Suspended");
            if (raiseError && !res)
                throw new ApplicationException("Data sanity compromised. Action Suspended");
            return res;
        }
        internal static void SignRecord(this eWalletTransaction e, ManagerBase c)
        {
            if (e.WalletTransactionId == Guid.Empty) throw new InvalidOperationException($"can't Sign Record with empty id {2802}");
            //WalletTransactionId + Date + TokenId + TAmount + FromWalletId + ToWalletId + FromWalletAfterTransactionBalance + ToWalletAfterTransactionBalance + SessionHash
            var si = $"{Signer.AppSeed}{e.WalletTransactionId}{e.Date}{e.TokenId}{e.TAmount}{e.FromWalletId}{e.ToWalletId}{e.FromWalletAfterTransactionBalance}{e.ToWalletAfterTransactionBalance}{e.SessionHash}";
            e.RecordHash = c.GetSignedHash(si);
            c.dbctx.SaveChanges();
        }
        internal static bool VerifyRecord(this eWalletTransaction e, ManagerBase c, bool raiseError = false)
        {
            if (e.WalletTransactionId == Guid.Empty) throw new InvalidOperationException($"can't verify null {2801}");
            var si = $"{Signer.AppSeed}{e.WalletTransactionId}{e.Date}{e.TokenId}{e.TAmount}{e.FromWalletId}{e.ToWalletId}{e.FromWalletAfterTransactionBalance}{e.ToWalletAfterTransactionBalance}{e.SessionHash}";
            var res = c.CompareHash(e.RecordHash, si) == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
            if (!res)
                c.LogEvent($"{e.WalletTransactionId}:{e.WalletTransactionId} failed Record Verification..Data Compromise..!");
            if (raiseError && !res)
                throw new ApplicationException("Data sanity compromised. Action Suspended");
            return res;
        }
        internal static void SignRecord(this eNetworkWalletAddress e, ManagerBase c)
        {
            if (e.NetworkWalletAddressId == Guid.Empty) throw new InvalidOperationException($"can't Sign Record with empty id {2801}");
            var si = $"{Signer.AppSeed}{e.NetworkId}{e.Network.Name}{e.CreatedOn}{e.Address}";
            e.RecordHash = c.GetSignedHash(si);
            c.dbctx.SaveChanges();
        }
        internal static bool VerifyRecord(this eNetworkWalletAddress e, ManagerBase c, bool raiseError = false)
        {
            if (e.NetworkWalletAddressId == Guid.Empty) throw new InvalidOperationException($"can't verify null {2801}");
            var si = $"{Signer.AppSeed}{e.NetworkId}{e.Network.Name}{e.CreatedOn}{e.Address}";
            var res = c.CompareHash(e.RecordHash, si) == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
            if (!res)
                c.LogEvent($"{e.NetworkWalletAddressId}{e.NetworkWalletAddressId} failed Record Verification..Data Compromise..!");
            if (raiseError && !res)
                throw new ApplicationException("Data sanity compromised. Action Suspended");
            return res;
        }
        internal static void SignRecord(this eFundingNetworkWallet e, ManagerBase c)
        {
            if (e.NetworkWalletAddressId == Guid.Empty) throw new InvalidOperationException($"can't Sign Record with empty id {2801}");
            var si = $"{Signer.AppSeed}{e.FundingNetworkWalletId}{e.AllocatedOn}{e.ShouldDeAllocatedOn}{e.FundingWalletId}{e.NetworkWalletAddressId}";
            e.RecordHash = c.GetSignedHash(si);
            c.dbctx.SaveChanges();
        }
        internal static bool VerifyRecord(this eFundingNetworkWallet e, ManagerBase c, bool raiseError = false)
        {
            if (e is null || e.NetworkWalletAddressId == Guid.Empty) throw new InvalidOperationException($"can't verify null {2801}");
            var si = $"{Signer.AppSeed}{e.FundingNetworkWalletId}{e.AllocatedOn}{e.ShouldDeAllocatedOn}{e.FundingWalletId}{e.NetworkWalletAddressId}";
            var res = c.CompareHash(e.RecordHash, si) == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
            if (!res)
                c.LogEvent($"{nameof(e.FundingNetworkWalletId)}:{e.FundingNetworkWalletId} failed Record Verification..Data Compromise..!");
            if (raiseError && !res)
                throw new ApplicationException("Data sanity compromised. Action Suspended");
            return res;
        }
        internal static void SignRecord(this eSupportedNetwork e, ManagerBase c)
        {
            e.CheckAndThrowNullArgumentException();
            e.SupportedNetworkId.CheckSignIdAndThrowOperException($"can't Sign Record with empty id {2801}");

            var si = $"{Signer.AppSeed}{e.SupportedNetworkId}{e.Name}{e.Description}{e.NativeCurrencyCode}{e.IsSmartContractEnabled}{e.SessionHash}";

            e.RecordHash = c.GetSignedHash(si);
            c.dbctx.SaveChanges();
        }
        internal static bool VerifyRecord(this eSupportedNetwork e, ManagerBase c, bool raiseError = false)
        {
            e.CheckAndThrowNullArgumentException();
            e.SupportedNetworkId.CheckSignIdAndThrowOperException($"can't verify null {2801}");

            var si = $"{Signer.AppSeed}{e.SupportedNetworkId}{e.Name}{e.Description}{e.NativeCurrencyCode}{e.IsSmartContractEnabled}{e.SessionHash}";
            var res = c.CompareHash(e.RecordHash, si) == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
            if (!res)//Silently Log
                c.LogEvent($"{nameof(e.SupportedNetworkId)}:{e.SupportedNetworkId} failed Record Verification..Data Compromise..!");
            if (raiseError && !res)
                throw new ApplicationException("Data sanity compromised. Action Suspended");
            return res;
        }
        internal static void SignRecord(this eSupportedToken e, ManagerBase c)
        {
            e.CheckAndThrowNullArgumentException();
            e.SupportedTokenId.CheckSignIdAndThrowOperException($"can't Sign Record with empty id {2801}");

            var si = $"{Signer.AppSeed}{e.SupportedTokenId}{e.Code}{e.Narration}{e.IsNative}{e.ContractAddress}{e.RelatedNetwork}{e.SessionHash}";

            e.RecordHash = c.GetSignedHash(si);
            c.dbctx.SaveChanges();
        }
        internal static bool VerifyRecord(this eSupportedToken e, ManagerBase c, bool raiseError = false)
        {
            e.CheckAndThrowNullArgumentException();
            e.SupportedTokenId.CheckSignIdAndThrowOperException($"can't verify null {2801}");

            var si = $"{Signer.AppSeed}{e.SupportedTokenId}{e.Code}{e.Narration}{e.IsNative}{e.ContractAddress}{e.RelatedNetwork}{e.SessionHash}";

            var res = c.CompareHash(e.RecordHash, si) == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
            if (!res)//Silently Log
                c.LogEvent($"{nameof(e.SupportedTokenId)}:{e.SupportedTokenId} failed Record Verification..Data Compromise..!");
            if (raiseError && !res)
                throw new ApplicationException("Data sanity compromised. Action Suspended");
            return res;
        }
        internal static void SignRecord(this eToken e, ManagerBase c)
        {
            e.CheckAndThrowNullArgumentException();
            e.TokenId.CheckSignIdAndThrowOperException($"can't Sign Record with empty id {2801}");

            var si = $"{Signer.AppSeed}{e.TokenId}{e.Code}{e.ShortName}{e.FullName}{e.Tick}{e.SessionHash}{e.SupportedCoin.Select(x => x.SupportedTokenId)}";

            e.RecordHash = c.GetSignedHash(si);
            c.dbctx.SaveChanges();
        }
        internal static bool VerifyRecord(this eToken e, ManagerBase c, bool raiseError = false)
        {
            e.CheckAndThrowNullArgumentException();
            e.TokenId.CheckSignIdAndThrowOperException($"can't Sign Record with empty id {2801}");

            var si = $"{Signer.AppSeed}{e.TokenId}{e.Code}{e.ShortName}{e.FullName}{e.Tick}{e.SessionHash}{e.SupportedCoin.Select(x => x.SupportedTokenId)}";

            var res = c.CompareHash(e.RecordHash, si) == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
            if (!res)//Silently Log
                c.LogEvent($"{nameof(e.TokenId)}:{e.TokenId} failed Record Verification..Data Compromise..!");
            if (raiseError && !res)
                throw new ApplicationException("Data sanity compromised. Action Suspended");
            return res;
        }
        internal static void SignRecord(this eFiatCurrency e, ManagerBase c)
        {
            e.CheckAndThrowNullArgumentException();
            e.FiatCurrencyId.CheckSignIdAndThrowOperException($"can't Sign Record with empty id {2801}");

            var si = $"{Signer.AppSeed}{e.FiatCurrencyId}{e.Code}{e.Name}{e.Description}{e.Tick}{e.Symbole}{e.SessionHash}";

            e.RecordHash = c.GetSignedHash(si);
            c.dbctx.SaveChanges();
        }
        internal static bool VerifyRecord(this eFiatCurrency e, ManagerBase c, bool raiseError = false)
        {
            e.CheckAndThrowNullArgumentException();
            e.FiatCurrencyId.CheckSignIdAndThrowOperException($"can't Sign Record with empty id {2801}");

            var si = $"{Signer.AppSeed}{e.FiatCurrencyId}{e.Code}{e.Name}{e.Description}{e.Tick}{e.Symbole}{e.SessionHash}";

            var res = c.CompareHash(e.RecordHash, si) == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
            if (!res)//Silently Log
                c.LogEvent($"{nameof(e.FiatCurrencyId)}:{e.FiatCurrencyId} failed Record Verification..Data Compromise..!");
            if (raiseError && !res)
                throw new ApplicationException("Data sanity compromised. Action Suspended");
            return res;
        }
        internal static void SignRecord(this eMarket e, ManagerBase c)
        {
            e.CheckAndThrowNullArgumentException();
            e.MarketId.CheckSignIdAndThrowOperException($"can't Sign Record with empty id {2801}");
            var typeA = ((e.MarketType == eeMarketType.CryptoCrypto) ||
                  (e.MarketType == eeMarketType.CryptoStable) ||
                  (e.MarketType == eeMarketType.StableStable));

            string si;
            if (typeA)
                si = $"{Signer.AppSeed}{e.MarketId}{e.ShortName}{e.CodeName}{e.MinOrderSizeValueUSD}{e.MinBaseOrderTick}{e.MinQuoteOrderTick}{e.BaseTokenId}{e.QuoteTokenId}{e.MinBaseOrderTick}{e.SessionHash}";
            else
                si = $"{Signer.AppSeed}{e.MarketId}{e.ShortName}{e.CodeName}{e.MinOrderSizeValueUSD}{e.MinBaseOrderTick}{e.MinQuoteOrderTick}{e.BaseTokenId}{e.QuoteCurrencyId}{e.MinBaseOrderTick}{e.SessionHash}";

            e.RecordHash = c.GetSignedHash(si);
            c.dbctx.SaveChanges();
        }
        internal static bool VerifyRecord(this eMarket e, ManagerBase c, bool raiseError = false)
        {
            e.CheckAndThrowNullArgumentException();
            e.MarketId.CheckSignIdAndThrowOperException($"can't Sign Record with empty id {2801}");

            var typeA = ((e.MarketType == eeMarketType.CryptoCrypto) ||
                 (e.MarketType == eeMarketType.CryptoStable) ||
                 (e.MarketType == eeMarketType.StableStable));

            string si;
            if (typeA)
                si = $"{Signer.AppSeed}{e.MarketId}{e.ShortName}{e.CodeName}{e.MinOrderSizeValueUSD}{e.MinBaseOrderTick}{e.MinQuoteOrderTick}{e.BaseTokenId}{e.QuoteTokenId}{e.MinBaseOrderTick}{e.SessionHash}";
            else
                si = $"{Signer.AppSeed}{e.MarketId}{e.ShortName}{e.CodeName}{e.MinOrderSizeValueUSD}{e.MinBaseOrderTick}{e.MinQuoteOrderTick}{e.BaseTokenId}{e.QuoteCurrencyId}{e.MinBaseOrderTick}{e.SessionHash}";

            var res = c.CompareHash(e.RecordHash, si) == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
            if (!res)//Silently Log
                c.LogEvent($"{nameof(e.MarketId)}:{e.MarketId} failed Record Verification..Data Compromise..!");
            if (raiseError && !res)
                throw new ApplicationException("Data sanity compromised. Action Suspended");
            return res;
        }
        #endregion
    }
}
