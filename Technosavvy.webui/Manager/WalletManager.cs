using TechnoApp.Ext.Web.UI.Model;
using TechnoApp.Ext.Web.UI.Models;
using TechnoApp.Ext.Web.UI.Service;
using TechnoApp.Ext.Web.UI.ViewModels;
using NuGet.Protocol;
using System.Text;

namespace TechnoApp.Ext.Web.UI.Manager
{
    public class WalletManager : MaintenanceSvc
    {
        //internal async Task<Tuple<string, string, string>> GetvmWalletUserDetails()
        internal async Task<List<vmPBMyPurchaseRecords>> GetvmMyPrebetaPurshases(vmPBMyPurchase vm)
        {
            var retval = new List<vmPBMyPurchaseRecords>();
            var val = await GetMyPrebetaPurshases();
            if (val != null && val.Count > 0)
            {
                val = val.OrderByDescending(x => x.DateOf).ToList();
                foreach (var item in val)
                {
                    retval.Add(new vmPBMyPurchaseRecords
                    {
                        Amount = item.TechnoSavvyAmountPurchased,
                        BuyWithName = item.BuyWith,
                        DateOf = item.DateOf,
                        BuyWith = (item.TechnoSavvyAmountPurchased * item.TechnoSavvyUnitRate) / item.TokenToUnitRate,
                        TxnID = item.TranHash
                    });
                    vm.TotalPurchasValue += item.TechnoSavvyAmountPurchased * item.TechnoSavvyUnitRate;

                }
            }
            vm.TotalTechnoSavvy = val.Sum(x => x.TechnoSavvyAmountPurchased);
            vm.TotalLaunchValue = (val.Sum(x => x.TechnoSavvyAmountPurchased) * 1.60);
            return retval;
        }

        internal async Task<List<mStakingSlot2>> GetStakingOpportunities()
        {

            var lst = await base.GetStakingOpportunities();
            //var tklst = await GetActiveTokens();
            //lst.ForEach(x => {
            //    x.Token =  tklst.FirstOrDefault(z => z.TokenId == x.TokenId);
            //});
            var all = lst.Where(x => x.Token != null).ToList();
            return all;
        }
        internal async Task<mStake> CommitStake(vmMyStaking vm)
        {
            bool ret = true;
            double shortAmount = vm.Amount;
            if (vm.IsIncludeOtherBalance)
            {

                var blst = await GetMyWalletSummery(myUS.FundingWalletId);
                var wbalT = blst.Tokens.FirstOrDefault(x => x.CoinId == vm.Token.TokenId);
                if (wbalT != null)
                {
                    shortAmount = vm.Amount - wbalT.Amount;
                }
                if (shortAmount > 0)
                {
                    blst = await GetMyWalletSummery(myUS.EarnWalletId);
                    wbalT = blst.Tokens.FirstOrDefault(x => x.CoinId == vm.Token.TokenId);
                    if (wbalT != null && wbalT.Amount > 0)
                    {
                        if (shortAmount > wbalT.Amount)
                        {
                            shortAmount = shortAmount - wbalT.Amount;
                            ret = ret && await SendTokenFromEarnToFunding(vm.Token.TokenId, wbalT.Amount);
                        }
                        else
                        {
                            ret = ret && await SendTokenFromEarnToFunding(vm.Token.TokenId, shortAmount);
                            if (ret)
                                shortAmount = 0;
                        }
                    }
                }
                if (shortAmount > 0)
                {
                    blst = await GetMyWalletSummery(myUS.SpotWalletId);
                    wbalT = blst.Tokens.FirstOrDefault(x => x.CoinId == vm.Token.TokenId);
                    if (wbalT != null && wbalT.Amount > 0)
                    {
                        if (shortAmount > wbalT.Amount)
                        {
                            shortAmount = shortAmount - wbalT.Amount;
                            ret = ret && await SendTokenFromSpotToFunding(vm.Token.TokenId, wbalT.Amount);
                        }
                        else
                        {
                            ret = ret && await SendTokenFromSpotToFunding(vm.Token.TokenId, shortAmount);
                            if (ret)
                                shortAmount = 0;
                        }
                    }
                }
            }
            if (ret)
            {
                var result = await StakeTokens(new mCreateStake { Amount = vm.Amount, AutoRenew = vm.AutoRenew, StakingOpportunityId = vm.selectedStakingSlot.StakingOpportunityId });
                return result;
            }
            return null;
        }
        internal async Task<bool> ValidateVmMyStaking(vmMyStaking vm)
        {
            vm.Token.CheckAndThrowNullArgumentException(ArguName: "Token");
            vm.Amount.CheckAndThrowNullArgumentException(ArguName: "Amount");
            vm.selectedStakingSlot.CheckAndThrowNullArgumentException(ArguName: "Duration");
            if (vm.Amount < vm.selectedStakingSlot.MinAmount)
                throw new ApplicationException($"Minimum Staking is {vm.selectedStakingSlot.MinAmount}");
            if (vm.Amount > vm.selectedStakingSlot.MaxAmount)
                throw new ApplicationException($"Maximum Staking is {vm.selectedStakingSlot.MinAmount}");
            double bal = 0;
            var t = vm.Token;
            if (vm.IsIncludeOtherBalance)
            {
                var blst = await GetMyWalletSummery(myUS.EarnWalletId);
                var wbalT = blst.Tokens.FirstOrDefault(x => x.CoinId == t.TokenId);
                if (wbalT != null)
                    bal = wbalT.Amount;

                blst = await GetMyWalletSummery(myUS.FundingWalletId);
                wbalT = blst.Tokens.FirstOrDefault(x => x.CoinId == t.TokenId);
                if (wbalT != null)
                    bal += wbalT.Amount;

                blst = await GetMyWalletSummery(myUS.SpotWalletId);
                wbalT = blst.Tokens.FirstOrDefault(x => x.CoinId == t.TokenId);
                if (wbalT != null)
                    bal += wbalT.Amount;
            }
            else
            {
                var blst = await GetMyWalletSummery(myUS.FundingWalletId);
                var wbalT = blst.Tokens.FirstOrDefault(x => x.CoinId == t.TokenId);
                if (wbalT != null)
                    bal = wbalT.Amount;
            }

            if (bal < vm.Amount)
                throw new ApplicationException($"Requested Amount ({vm.Amount.ToString("#,##0.###")}) exceeds the Balance Available ({bal.ToString("#,##0.###")})");

            return true;
        }
        internal async Task<vmMyStaking> LoadvmMyStaking(vmMyStaking vm, string code = "")
        {
            var olst = await GetStakingOpportunities();

            vm.TokenList = olst.DistinctBy(x => x.Token.Code).Select(x => x.Token).ToList(); // await GetActiveTokens();
            if (code.IsNOT_NullorEmpty())
                vm.Token = vm.TokenList.FirstOrDefault(x => x.Code.ToLower() == code.ToLower());
            else if (vm.selectedStakingSlot != null && !vm.selectedStakingSlot.StakingOpportunityId.IsGuidNullorEmpty())
            {
                vm.selectedStakingSlot = olst.FirstOrDefault(x => x.StakingOpportunityId == vm.selectedStakingSlot.StakingOpportunityId);
                vm.Token = vm.TokenList.FirstOrDefault(x => x.TokenId == vm.selectedStakingSlot.Token.TokenId);
            }

            if (vm.Token != null && vm.Token.TokenId.IsGuidNullorEmpty() == false)
            {
                var t = vm.TokenList.First(x => x.TokenId == vm.Token.TokenId);
                vm.Token = t;
                vm.RelatedStakingSlot = olst.Where(x => x.Token.Code == t.Code).ToList();

                var blst = await GetMyWalletSummery(myUS.FundingWalletId);
                var wbalT = blst.Tokens.FirstOrDefault(x => x.CoinId == t.TokenId);
                if (wbalT != null)
                    vm.wBalance = wbalT.Amount;

                blst = await GetMyWalletSummery(myUS.EarnWalletId);
                wbalT = blst.Tokens.FirstOrDefault(x => x.CoinId == t.TokenId);
                if (wbalT != null)
                    vm.otherBalance = wbalT.Amount;

                blst = await GetMyWalletSummery(myUS.SpotWalletId);
                wbalT = blst.Tokens.FirstOrDefault(x => x.CoinId == t.TokenId);
                if (wbalT != null)
                    vm.otherBalance += wbalT.Amount;

            }
            if (vm.selectedStakingSlot != null && !vm.selectedStakingSlot.StakingOpportunityId.IsGuidNullorEmpty())
            {
                vm.selectedStakingSlot = olst.FirstOrDefault(x => x.StakingOpportunityId == vm.selectedStakingSlot.StakingOpportunityId);
                vm.selectedStakingSlot ??= vm.RelatedStakingSlot.MaxBy(x => x.Duration);
            }
            else
            {
                if (vm.RelatedStakingSlot != null)
                    vm.selectedStakingSlot = vm.RelatedStakingSlot.MaxBy(x => x.Duration);
            }
            return vm;
        }
        internal async Task<vmWalletProfile> GetvmWalletUserDetails()
        {
            var ret = new vmWalletProfile();
            ret.Name = $"{myUS.UserAccount.Profile.FirstName} {myUS.UserAccount.Profile.LastName}";
            ret.AccountNo = myUS.UserAccount.AccountNumber;
            ret.KYCStatus = myUS.UserAccount.Profile.KYCStatus;
            ret.LastLogin = myUS.StartedOn.ToString("dd-MMM-yy t");
            return ret;
            // myCookieState.UserName
            var result2 = myUS.CreatedOn.ToString();
            // var profile = await GetProfile(myUS.UserAccount.Id.ToGuid());
            // var result3 = profile.KYCStatus.ToString();
            // return Tuple.Create(result1, result2, String.Empty);
        }
        internal async Task<List<vmSpotWalletTokenDetail>> GetSpotWalletTokens()
        {
            var retval = new List<vmSpotWalletTokenDetail>();
            var spot = await GetMyWalletSummery(myUS.SpotWalletId);
            foreach (var t in spot.Tokens)
            {
                retval.Add(new vmSpotWalletTokenDetail
                {
                    Code = t.Code,
                    ImgLocation = t.IconLocation,
                    AvailableAmount = t.Amount,
                });
            }
            return retval;
        }
        internal async Task<vmEarnWallet> GetvmEarnWalletAssetDetails(vmEarnWallet vm)
        {
            var result = new List<vmEarnWalletTokenDetail>();
            var len = await MinumumGlobalTick();

            var spot = await GetMyWalletSummery(myUS.EarnWalletId);
            vm.selectedCryptoCoin = myCookieState.Coin;

            vm.TokenList = new List<Model.mToken2>();
            foreach (var item in await GetAllActiveCryptoTokens())
            {
                vm.TokenList.Add(new mToken2 { token = item, basePrice = SrvCoinPriceHUB.GetCoin(item.Code).Price });
            }

            double USDtotalValue = 0;
            foreach (var t in spot.Tokens)
            {
                result.Add(new vmEarnWalletTokenDetail
                {
                    Code = t.Code,
                    AvailableAmount = t.Amount,
                    TotalAmount = t.Amount,
                    ImgLocation = t.IconLocation,
                    PolyPoints = await GetPoly(t.Code)
                });

                USDtotalValue += t.Amount * SrvCoinPriceHUB.GetCoin(t.Code).Price;
            }
            vm.Details = result;
            vm.AccountBalance_baseV = USDtotalValue;//
            vm.AccountBalance_CryotoV = USDtotalValue / SrvCoinPriceHUB.GetCoin(myCookieState.Coin).Price;

            return vm;
        }
        internal async Task<vmSpotWallet> GetvmSpotWalletAssetDetails(vmSpotWallet vm)
        {
            var result = new List<vmSpotWalletTokenDetail>();
            var len = await MinumumGlobalTick();

            var spot = await GetMyWalletSummery(myUS.SpotWalletId);
            vm.selectedCryptoCoin = myCookieState.Coin;
            // vm.selectedCoin = myCookieState.Coin;
            vm.TokenList = new List<Model.mToken2>();
            foreach (var item in await GetAllActiveCryptoTokens())
            {
                vm.TokenList.Add(new mToken2 { token = item, basePrice = SrvCoinPriceHUB.GetCoin(item.Code).Price });
            }
            // vm.SelectTokenList = new SelectList(await GetActiveTokens(100), "Code", "Code", vm.selectedCoin);
            double USDtotalValue = 0;
            foreach (var t in spot.Tokens)
            {
                result.Add(new vmSpotWalletTokenDetail
                {
                    Code = t.Code,
                    AvailableAmount = t.Amount,
                    TotalAmount = t.Amount,
                    ImgLocation = t.IconLocation,
                    PolyPoints = await GetPoly(t.Code)
                });

                USDtotalValue += t.Amount * SrvCoinPriceHUB.GetCoin(t.Code).Price;
            }
            vm.Details = result;
            vm.AccountBalance_baseV = USDtotalValue;//
            vm.AccountBalance_CryotoV = USDtotalValue / SrvCoinPriceHUB.GetCoin(myCookieState.Coin).Price;
            // vm.AccountBalance = Tuple.Create(await GetValueInMyDefaultToken(USDtotalValue), await GetValueInMyDefaultCurrency(USDtotalValue));

            return vm;
        }
        internal async Task<vmFundingWallet> GetvmvmFundingWalletAssetDetails(vmFundingWallet vm)
        {
            var result = new List<vmFundingWalletTokenDetail>();
            var len = await MinumumGlobalTick();

            var wallet = await GetMyWalletSummery(myUS.FundingWalletId);
            vm.selectedCryptoCoin = myCookieState.Coin;

            vm.TokenList = new List<Model.mToken2>();
            foreach (var item in await GetAllActiveCryptoTokens())
            {
                vm.TokenList.Add(new mToken2 { token = item, basePrice = SrvCoinPriceHUB.GetCoin(item.Code).Price });
            }

            double USDtotalValue = 0;
            foreach (var t in wallet.Tokens)
            {
                result.Add(new vmFundingWalletTokenDetail
                {
                    Code = t.Code,
                    AvailableAmount = t.Amount,
                    TotalAmount = t.Amount,
                    ImgLocation = t.IconLocation,
                    PolyPoints = await GetPoly(t.Code)
                });
                USDtotalValue += t.Amount * SrvCoinPriceHUB.GetCoin(t.Code).Price;
            }
            vm.Details = result;
            vm.AccountBalance_baseV = USDtotalValue;//
            vm.AccountBalance_CryotoV = USDtotalValue / SrvCoinPriceHUB.GetCoin(myCookieState.Coin).Price;

            //vm.AccountBalance = Tuple.Create(await GetValueInMyDefaultToken(USDtotalValue), await GetValueInMyDefaultCurrency(USDtotalValue));

            return vm;
        }
        internal async Task<mConvertTokenRequest> CreateAndSendConvertTokensRequest(vmConvert m)
        {
            var ret = new mConvertTokenRequest
            {
                bCode = m.Token.Code,
                fromAmt = m.PayAmount,
                toTokenAmt = m.BuyAmount,
                fromTokenId = m.PayToken.TokenId,
                IsFundWalletAllowed = true,
                IsSpotWalletAllowed = m.IsAll,
                qCode = m.PayToken.Code,
                toTokenId = m.Token.TokenId,
                RateOfOneToToken = m.ValueOfOne,
                ErrMsg = ""
            };
            ret = await ConvertToken(ret);
            return ret;
        }
        internal async Task<mConvertTokenRequest> CreateAndSendConvertTokensRequest(vmBuy m)
        {
            var ret = new mConvertTokenRequest
            {
                bCode = m.Token.Code,
                fromAmt = m.PayAmount,
                toTokenAmt = m.BuyAmount,
                fromTokenId = m.PayToken.token.TokenId,
                IsFundWalletAllowed = true,
                qCode = m.PayToken.token.Code,
                toTokenId = m.Token.TokenId,
                RateOfOneToToken = m.ValueOfOne,
                ErrMsg = ""
            };
            ret = await ConvertToken(ret);
            return ret;
        }
        internal async Task<mCoinInWallet> GetCoinBalInMyWallets(Guid tokenId)
        {
            var ret = new mCoinInWallet();
            var spotwallet = await GetMyWalletSummery(myUS.SpotWalletId);
            var fundwallet = await GetMyWalletSummery(myUS.FundingWalletId);
            var earnwallet = await GetMyWalletSummery(myUS.EarnWalletId);
            ret.SpotWallet = spotwallet.Tokens.FirstOrDefault(x => x.CoinId == tokenId);
            ret.FundWallet = fundwallet.Tokens.FirstOrDefault(x => x.CoinId == tokenId);
            ret.EarnWallet = earnwallet.Tokens.FirstOrDefault(x => x.CoinId == tokenId);
            return ret;
        }
        // Wallet Index Page for all the wallet details
        internal async Task<List<vmWalletAsset>> GetvmWalletHomeAssetDetails(vmWalletHome vm)
        {
            var len = await MinumumGlobalTick();
            var spotwallet = GetMyWalletSummery(myUS.SpotWalletId);
            var fundwallet = GetMyWalletSummery(myUS.FundingWalletId);
            var ernwallet = GetMyWalletSummery(myUS.EarnWalletId);

            await Task.WhenAll(spotwallet, fundwallet, ernwallet);
            vm.selectedCryptoCoin = myCookieState.Coin;
            vm.TokenList = new List<Model.mToken2>();
            foreach (var item in await GetAllActiveCryptoTokens())
            {
                vm.TokenList.Add(new mToken2 { token = item, basePrice = SrvCoinPriceHUB.GetCoin(item.Code).Price });
            }
            var tokens = (from x in spotwallet.Result.Tokens
                          select x)
                          .Union(fundwallet.Result.Tokens)
                          .Union(ernwallet.Result.Tokens)
                          .GroupBy(z => z.Code)
                          .ToList()
                          .Select(t => new vmWalletAsset
                          {
                              Code = t.Key,
                              ShortName = t.First().ShortName,
                              IsFiat = t.First().IsFiatRepresentative,
                              cType = CoinType.Crpto,
                              Amt = t.Sum(z => z.Amount),
                              BaseVal = SrvCoinPriceHUB.GetCoin(t.Key).Price * t.Sum(z => z.Amount)
                          }).ToList();
            if (spotwallet?.Result?.Fiats != null)
            {
                var fi = (from t in spotwallet.Result.Fiats
                          select t)
               .Select(t => new vmWalletAsset
               {
                   Code = t.Code,
                   IsFiat = true,
                   ShortName = t.ShortName,
                   cType = CoinType.Fiat,
                   Amt = t.Amount,
                   BaseVal = SrvCoinPriceHUB.GetCoin(t.Code).Price * t.Amount
               });
                tokens.AddRange(fi);
            }
            double USDtotalValue = tokens.Sum(x => x.BaseVal);

            //foreach (var t in tokens)
            //{
            //    result.Add(new vmWalletAsset {Code= t.Key,ShortName=t.First().ShortName, Amt= t.Sum(z => z.Amount) });
            //    USDtotalValue += t.Sum(z => z.Amount) * SrvCoinPriceHUB.GetCoin(t.Key).Price;
            //}
            //ToDo: Naveen, handle Client Side
            //DisplayFormat(len, t.Sum(z => z.Amount))
            vm.Assets = vm.Assets ?? new List<vmWalletAsset>();
            vm.Assets.AddRange(tokens);

            //ToDo: Naveen, handle Client Side
            vm.AccountBalance_baseV = USDtotalValue;//
            vm.AccountBalance_CryotoV = USDtotalValue / SrvCoinPriceHUB.GetCoin(myCookieState.Coin).Price;
            //vm.AccountBalance = Tuple.Create(await GetValueInMyDefaultToken(USDtotalValue), await GetValueInMyDefaultCurrency(USDtotalValue));

            return vm.Assets;
        }

        internal async Task<string> GetValueInMyDefaultToken(double USDAmt)
        {
            return $"{await USDtoCoinValue(USDAmt, myCookieState.Coin)} {myCookieState.Coin}";
        }
        internal async Task<string> GetValueInMyDefaultCurrency(double USDAmt)
        {
            return $"{await USDtoCurrencyValue(USDAmt, myCookieState.Currency)} {myCookieState.Currency}";
        }
        internal void GetEstimatedValueIn(string bCode, string qCode, double Amt, out double Rate, out double MinTrade)
        {
            var ret=GetEstimatedValueIn(bCode, qCode, Amt).GetAwaiter().GetResult();

            if (ret != null)
            {
                Rate = ret.Item1;
                MinTrade = ret.Item2;
            }
            else
            {
                Rate = double.NaN;
                MinTrade = double.NaN;
            }
            return;
          
        }
        internal double GetUSDValue(string Tokencode, double Amt)
        {
            if (Amt == 0) return Amt;
            return SrvCoinPriceHUB.GetCoin(Tokencode).Price * Amt;
        }
        //Crypto Coin
        internal async Task<string> USDtoCoinValue(double usdAmt, string ToTokencode)
        {
            var f = await MinumumGlobalTick();

            if (usdAmt == 0) return usdAmt.ToString();
            return DisplayFormat(f, usdAmt / SrvCoinPriceHUB.GetCoin(ToTokencode).Price);
        }
        //Fiat Currency
        internal async Task<string> USDtoCurrencyValue(double usdAmt, string ToCurrencycode)
        {
            var f = await MinumumGlobalTick();

            if (usdAmt == 0) return DisplayFormat(f, usdAmt);
            var ans = SrvCurrencyPriceHUB.GetCurrency(ToCurrencycode).Price * usdAmt;
            return DisplayFormat(f, ans);
        }
        internal async Task<List<mWalletCoin>> GetWalletTokens(int wtype)
        {
            if (wtype == 2)
            {
                return (await GetMySpotWalletSummery()).Tokens;
            }
            else if (wtype == 3)
            {
                var fw = (await GetMyFundingWalletSummery());
                var tks = fw.Tokens;
                if (fw.Fiats != null && fw.Fiats.Count > 0)
                    tks.AddRange(fw.Fiats);
                return tks;
            }
            else if (wtype == 1)
            {
                return (await GetMyEarnWalletSummery()).Tokens;
            }
            throw new InvalidOperationException("Invalid Wallet Type");
        }
        internal async Task<mWalletSummery> GetMySpotWalletSummery()
        {
            var spotwallet = await GetMyWalletSummery(myUS.SpotWalletId);
            return spotwallet;
        }
        internal async Task<mWalletSummery> GetMyFundingWalletSummery()
        {
            var FundingWallet = await GetMyWalletSummery(myUS.FundingWalletId);
            return FundingWallet;
        }
        internal async Task<mWalletSummery> GetMyEarnWalletSummery()
        {
            var spotwallet = await GetMyWalletSummery(myUS.EarnWalletId);
            return spotwallet;
        }
        internal async Task<vmWalletTransactions> GetvmWalletTransactions(vmWalletTransactions vm)
        {
            vm.UserDetails = await GetvmWalletUserDetails();
            vm.WalletTransactions = await GetMyAllTransactions();
            vm.WalletTransactions = vm.WalletTransactions.OrderByDescending(x => x.Date).ThenBy(x => x.TransactionId).ToList();
            vm.TokenList = new List<Model.mToken2>();
            foreach (var item in await GetAllActiveCryptoTokens())
            {
                vm.TokenList.Add(new mToken2 { token = item, basePrice = SrvCoinPriceHUB.GetCoin(item.Code).Price });
            }
            return vm;
        }
        internal async Task<List<mWalletTransactions>> GetMyAllTransactions()
        {
            var vm = new List<mWalletTransactions>();
            var spotwallet = await GetMyWalletTransactions(myUS.SpotWalletId);
            var fundwallet = await GetMyWalletTransactions(myUS.FundingWalletId);
            var ernwallet = await GetMyWalletTransactions(myUS.EarnWalletId);
            //ToDo: other wallet as well..
            vm.AddRange(spotwallet);
            vm.AddRange(fundwallet);
            vm.AddRange(ernwallet);
            return vm;
        }
        //Prebeta
        internal async Task<bool> EnsureNetworkWallet_Prebeta()
        {
            var lst = await GetMyPrebetaPurshases();
            await _appSessionManager.ExtSession.LoadSession();

            var NetworkList = await GetAllSupportedNetwork();
            if (NetworkList.Count > 0)
            {
                var wallet = await GetMyNetworkWallet(NetworkList.First().SupportedNetworkId);
                if (lst.Count > 0 && wallet != null)
                {
                    //Request SmartContract Wallet if user has some purchases
                    //var plst = await GetMyPrebetaPurshases();
                    var tt = lst.Sum(x => x.TechnoSavvyAmountPurchased);
                    if (tt > 0 && wallet != null)
                        Console2.WriteLine_Green($"Info:{myUS.UserAccount.Email} Check to Confirm ContractWallet Type:{wallet.WType} and TechnoSavvy in hand:{tt}");


                    if (tt > 500)
                    {
                        if (wallet != null && wallet.WType == WalletType.Standard)
                        {
                            //Check If Not Already Have Smart Contract
                            Console2.WriteLine_Green($"Info:{myUS.UserAccount.Email} has {tt} TechnoSavvy, Initiating Request for SC Wallet");
                            var v = await RequestToGenerateMySmartNetworkWallet(NetworkList.First().SupportedNetworkId);
                            return v.HasValue && v.Value;
                        }
                        else
                            Console2.WriteLine_Green($"Info:{myUS.UserAccount.Email} has WalletType: {(wallet == null ? "No Wallet" : wallet.WType)}");

                    }
                    return true;
                }
                else
                {
                    //PreBeta should only have 1
                    if (wallet != null)
                    {
                        Console2.WriteLine_Green($"Info:{myUS.UserAccount.Email} Check to Confirm ContractWallet Type:{wallet.WType} and TechnoSavvy in hand:{0}");

                        return true;
                    }
                    else
                    {
                        var v = await RequestToGenerateMyNetworkWallet(NetworkList.First().SupportedNetworkId);
                        Console2.WriteLine_Green($"Info:{myUS.UserAccount.Email} has been requested for new Standard Wallet Wallet");

                        return v.HasValue && v.Value;
                    }
                }
            }
            return false;

        }
    }
}
