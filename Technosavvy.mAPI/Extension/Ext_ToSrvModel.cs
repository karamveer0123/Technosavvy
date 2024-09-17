using NavExM.Int.Maintenance.APIs.ServerModel;
using meOrderType = NavExM.Int.Maintenance.APIs.Model.eOrderType;

namespace NavExM.Int.Maintenance.APIs.Extension
{
    public static partial class Ext
    {
        #region ToSrvModel
        public static smOrder ToSrvModel(this mOrder e)
        {
            var m = new smOrder();
            if (e != null)
            {
                m.PlacedOn = e.PlacedOn;
                m.MarketCode = e.MarketCode;
                m.OrderType = (ServerModel.eOrderType)(int)e.OrderType;
                m.OrderSide = (ServerModel.eOrderSide)(int)e.OrderSide;
                m.OriginalVolume = e.Volume;
                m.CurrentVolume = e.Volume;
                m.Price = e.Price;
                m.Status = (ServerModel.eOrderStatus)e.Status;
                m.BaseTokenId = e.BaseTokenId;
                m.BaseTokenCodeName = e.BaseTokenCodeName;
                m.QuoteTokenCodeName = e.QuoteTokenCodeName;
                m.QuoteTokenId = e.QuoteTokenId;
                m.DiscloseAmount = e.DiscloseAmount;
                m.DiscloseValue = e.DiscloseValue;
                m.StopPrice = e.StopPrice;
                m.LimitPrice = e.LimitPrice;
                m.LoopCount = e.LoopCount;
            }
            return m;
        }
        public static smOrder ToSrvModel(this mMMOrder e)
        {
            var m = new smOrder();
            if (e != null)
            {
                m.PlacedOn = e.PlacedOn;
                m.MarketCode = e.MarketCode;
                m.OrderType = (ServerModel.eOrderType)(int)e.OrderType;
                m.OrderSide = (ServerModel.eOrderSide)(int)e.OrderSide;
                m.OriginalVolume = e.Volume;
                m.CurrentVolume = e.Volume;
                m.Price = e.Price;
                m.Status = (ServerModel.eOrderStatus)e.Status;
                m.BaseTokenId = e.BaseTokenId;
                m.BaseTokenCodeName = e.BaseTokenCodeName;
                m.QuoteTokenCodeName = e.QuoteTokenCodeName;
                m.QuoteTokenId = e.QuoteTokenId;
                m.DiscloseAmount = e.DiscloseAmount;
                m.DiscloseValue = e.DiscloseValue;
                m.StopPrice = e.StopPrice;
                m.LimitPrice = e.LimitPrice;
                m.LoopCount = e.LoopCount;
            }
            return m;
        }
        public static smOrder ToClone(this smOrder e)
        {
            var m = new smOrder();
            if (e != null)
            {
                m.id = e.id;
                m.OrderID= e.OrderID;
                m.UserAccountNo= e.UserAccountNo;
                m.SessionId= e.SessionId;
                m.WalletID= e.WalletID;
                m.InternalOrderID= e.InternalOrderID;
                m.AuthId= e.AuthId;
                m.PlacedOn = e.PlacedOn;
                m.MarketCode = e.MarketCode;
                m.OrderType = (ServerModel.eOrderType)(int)e.OrderType;
                m.OrderSide = (ServerModel.eOrderSide)(int)e.OrderSide;
                m.OriginalVolume = e.OriginalVolume;
                m.CurrentVolume = e.CurrentVolume;
                m.Price = e.Price;
                m.Status = e.Status;
                m.BaseTokenId = e.BaseTokenId;
                m.BaseTokenCodeName = e.BaseTokenCodeName;
                m.QuoteTokenCodeName = e.QuoteTokenCodeName;
                m.QuoteTokenId = e.QuoteTokenId;
                m.DiscloseAmount = e.DiscloseAmount;
                m.DiscloseValue = e.DiscloseValue;
                m.StopPrice = e.StopPrice;
                m.LimitPrice = e.LimitPrice;
                m.LoopCount = e.LoopCount;
                m.PoolRefunds = e.PoolRefunds;
                m.SwapRate = e.SwapRate;
                m.SwapTradeValue = e.SwapTradeValue;
                m.SwapValue = e.SwapValue;
                m._OrderAssetAmount = e._OrderAssetAmount;
                m._OrderSwapTradeValue = e._OrderSwapTradeValue;
                m._OrderTrigger = e._OrderTrigger;
            }
            return m;
        }
        public static smUserSession ToSrvModel(this eUserSession e)
        {
            var m = new smUserSession();
            if (e != null)
            {
                m.UserSessionId = e.UserSessionId;
                m.UserAccount = e.UserAccount.ToSrvModel();
                m.StartedOn = e.StartedOn;
                m.ShouldExpierOn = e.ShouldExpierOn;
                m.SessionHash = e.SessionHash;
                m.ExpieredOn = e.ExpieredOn;
                m.SpotWBal ??= new mWalletSummery();
                m.SpotWBal.WalletId = e.UserAccount!.SpotWallet!.SpotWalletId;
                m.FundingWBal ??= new mWalletSummery();
                m.FundingWBal.WalletId = e.UserAccount.FundingWallet!.FundingWalletId;
                m.EscroWBal ??= new mWalletSummery();
                m.EscroWBal.WalletId = e.UserAccount.HoldingWallet!.HoldingWalletId;
                m.EarnWBal ??= new mWalletSummery();
                m.EarnWBal.WalletId = e.UserAccount.EarnWallet!.EarnWalletId;
                m.AuthEventId = e.SessionAuthEventId;
            }
            return m;
        }
        public static smWalletBalance ToSrvModel(this eSpotWallet e)
        {
            var m = new smWalletBalance();
            if (e != null)
            {
                m.WalletId = e.SpotWalletId;
                m.Name = "Spot";
            }
            return m;
        }
        public static smWalletBalance ToSrvModel(this eEarnWallet e)
        {
            var m = new smWalletBalance();
            if (e != null)
            {
                m.WalletId = e.EarnWalletId;
                m.Name = "Earn";
            }
            return m;
        }
        public static smWalletBalance ToSrvModel(this eHoldingWallet e)
        {
            var m = new smWalletBalance();
            if (e != null)
            {
                m.WalletId = e.HoldingWalletId;
                m.Name = "Holding";
            }
            return m;
        }
        public static smWalletBalance ToSrvModel(this eFundingWallet e)
        {
            var m = new smWalletBalance();
            if (e != null)
            {
                m.WalletId = e.FundingWalletId;
                m.Name = "Funding";
            }
            return m;
        }
        public static smUserAccount ToSrvModel(this eUserAccount e)
        {
            var m = new smUserAccount();
            if (e != null)
            {
                m.UserAccountId = e.UserAccountId;
                m.Email = Check<eAuthorizedEmail>(e.AuthEmail).Email;
                m.AccountNumber = e.AccountNumber!.Value;
                m.FAccountNumber = e.FAccountNumber!;
                m.TaxResidency = Check<eTaxResidency>(e.TaxResidency).ToSrvModel();
                m.CitizenshipOf = Check<eCitizenship>(e.CitizenOf).ToSrvModel();
                m.IsActive = e.IsActive;
            }
            return m;
        }
        public static smTaxResidency ToSrvModel(this eTaxResidency e)
        {
            smTaxResidency m = null;
            if (e != null && e.Country != null)
            {
                m = new smTaxResidency();
                m.CountryName = e.Country.Name;
                m.CountryId = e.Country.CountryId;
                m.Abbrivation = e.Country.Abbrivation;
            }
            return m;
        }
        public static smCitizenshipOf ToSrvModel(this eCitizenship e)
        {
            smCitizenshipOf m = null;
            if (e != null && e.Country != null)
            {
                m = new smCitizenshipOf();
                m.CountryName = e.Country.Name;
                m.CountryId = e.Country.CountryId;
                m.Abbrivation = e.Country.Abbrivation;
            }
            return m;
        }
        #endregion

    }
}
