using Microsoft.EntityFrameworkCore;
using NavExM.Int.Maintenance.APIs.ServerModel;
using NavExM.Int.Maintenance.APIs.Services;
using NuGet.Common;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Security.Principal;

namespace NavExM.Int.Maintenance.APIs.Manager;
internal class OrderManager : ManagerBase
{
    internal Tuple<bool, string> ConfirmAndCancel(string orderId, string mCode)
    {
        try
        {
            var mm = GetMarketManager();
            var um = GetUserManager();
            var sess = um.GetMySession();
            var Order = ConfirmOwnershipAndOrderToCancel(sess.UserAccount.AccountNumber, orderId, mCode);

            return mm.CancelOrder(Order);
        }
        catch (Exception ex)
        {
            LogError(ex);
            return new Tuple<bool, string>(false, ex.Message);
        }

    }

    internal Tuple<bool, string> TryBuildAndPlaceOrder(mOrder order)
    {
        //ToDo:Naveen, Create an Order Request Object for DB, If Order is unsuccessfull, It can be Reviewed by staff. Account may get bared
        //Done:Naveen,Check if Market for This Order exists, Trading(LIVE)
        //Done:Naveen,Check if User has a valid Session
        //ToDo:Naveen,Check if Rate of Order Doesn't Exceed User's Allowance per sec.
        //Done:Naveen,Check if User is allowed to place order
        //Done:Naveen,Check if User has Tokens in Fund Wallet to place order
        //ToDo:Naveen,Check if the Category of this User to establish SWAP Rate and Reward for this order
        //ToDo:Naveen,Sign this order with Instance key for verification purposes
        // ToDo:Naveen,Check and Validate if this Order 
        var ret = new Tuple<bool, string>(false, "Something went wrong");
        try
        {
            var mm = GetMarketManager();
            var um = GetUserManager();
            //ToDo:Naveen//
            var sess= um.GetMySession();
            var isOk=um.IsTradingAllowed(sess.UserAccount.AccountNumber);
            if (!isOk)
                throw new ApplicationException("User Account is Restricted for Trading");
            if (order.OrderSide == Model.eOrderSide.Buy)
            {
                var sm = GetBuyOrderPackage(order);
                sm = SaveToDB(sm);
                ret = mm.PlaceOrder(sm);
                return ret;
            }
            else
            {
                var sm = GetSellOrderPackage(order);
                sm = SaveToDB(sm);
                ret = mm.PlaceOrder(sm);
                return ret;
            }
        }
        catch (Exception ex)
        {
            LogError(ex);
            return new Tuple<bool, string>(false, ex.Message);
        }
    }
    internal Tuple<bool, string> TryBuildAndPlace_MMOrder(mMMOrder order, mUserSession sess)
    {
        //ToDo:Naveen, Create an Order Request Object for DB, If Order is unsuccessfull, It can be Reviewed by staff. Account may get bared
        //Done:Naveen,Check if Market for This Order exists, Trading(LIVE)
        //Done:Naveen,Check if User has a valid Session
        //ToDo:Naveen,Check if Rate of Order Doesn't Exceed User's Allowance per sec.
        //Done:Naveen,Check if User is allowed to place order
        //Done:Naveen,Check if User has Tokens in Fund Wallet to place order
        //ToDo:Naveen,Check if the Category of this User to establish SWAP Rate and Reward for this order
        //ToDo:Naveen,Sign this order with Instance key for verification purposes
        // ToDo:Naveen,Check and Validate if this Order 
        var ret = new Tuple<bool, string>(false, "Something went wrong");
        try
        {
            var mm = GetMarketManager();
            var um = GetUserManager();

            //ToDo:Naveen//um.IsTradingAllowed()
            //var sess = um.GetMySession();
            //var isOk = um.IsTradingAllowed(sess.UserAccount.AccountNumber);

            if (order.OrderSide == Model.eOrderSide.Buy)
            {
                var sm = GetBuyMMOrderPackage(order, sess);
                var smO = SaveToDB(sm.ToClone());
                sm = SaveToMMOrderDB(sm.ToClone());
                ret = mm.PlaceOrder(smO);
                return ret;
            }
            else
            {
                var sm = GetSellMMOrderPackage(order, sess);
                var smO = SaveToDB(sm.ToClone());
                sm = SaveToMMOrderDB(sm.ToClone());
                ret = mm.PlaceOrder(smO);
                return ret;
            }
        }
        catch (Exception ex)
        {
            LogError(ex);
            Console2.WriteLine_RED($"ERROR:TryBuildAndPlace_MMOrder has error:{ex.GetDeepMsg()}");
            return new Tuple<bool, string>(false, ex.Message);
        }
    }
    smOrder SaveToDB(smOrder sm)
    {
        using (var db = Orderctx(sm.MarketCode))
        {
            sm.id = Guid.NewGuid();
            db.Orders.Add(sm);
            db.SaveChanges();
        }
        return sm;
    }
    smOrder SaveToMMOrderDB(smOrder sm)
    {
        using (var db = MMOrderctx(sm.MarketCode))
        {
            var io = new smIterationOrders { IterationId = sm.id, OrderID = sm.OrderID, WalletID = sm.WalletID, mCode = sm.MarketCode };
            sm.id = Guid.NewGuid();
            db.Orders.Add(sm);
            db.IterationOrders.Add(io);
            db.SaveChanges();
        }
        return sm;
    }
    internal smOrder? GetOrderDetails(string mCode, Guid InternalOrderId, smTrade tr, out bool isFirstTrade, out Guid SpotWID, out eCommunityCategory cStatus)
    {
        smOrder oo = null;
        smOrder ot = null;
        isFirstTrade = false;
        SpotWID = Guid.Empty;
        cStatus = eCommunityCategory.None;
        using (var db = Orderctx(mCode))
        {
            oo = db.Orders.Include(x => x.ProcessedOrder).FirstOrDefault(x => x.InternalOrderID == InternalOrderId);
            if (tr.BuyInternalId == InternalOrderId)
                ot = db.Orders.Include(x => x.ProcessedOrder).FirstOrDefault(x => x.InternalOrderID == tr.SellInternalId);
            else
                ot = db.Orders.Include(x => x.ProcessedOrder).FirstOrDefault(x => x.InternalOrderID == tr.BuyInternalId);

            if (oo == null) return oo;
            var um = GetUserManager();
            SpotWID = um.GetSpotWalletOf(oo!.UserAccountNo);
            //ToDo: checked if This Order originates from FundWallet, if so, It is Convert/Buy Order, No Fee or CB Applies and Trde Debit should go to FUND wallets as well
            //if (oo.WalletID != SpotWID)
            //{
            //    var my = GetUserManager().GetUserOfAcc(oo.UserAccountNo);
            //    if (my != null && my.FundingWallet!.FundingWalletId == oo.WalletID)
            //    {
            //        SpotWID = my.FundingWallet.FundingWalletId;
            //        return oo;
            //    }
            //}

            //ToDo: Confirm if SWAP rate Suggested in this order still stands
            if (oo.OrderSide == ServerModel.eOrderSide.Buy)
            {
                var isAllowed = GetmyBuyTradingSwap(mCode, oo.UserAccountNo!, out var SWAP, out var cb, out var cbCommit, out var Limit, out cStatus);
                if (isAllowed)
                {
                    // Charge which ever is higer
                    //ToDo: Decide if we should update Persisted swOrder/Processed Order with Changed SWAP rate.
                    tr._BuySWAPRate = SWAP;
                    tr._BuySWAPValue = SWAP * tr.TradeValue;
                    var c = UserTradeCountOfToday(SpotWID);
                    if (c < Limit)
                        tr._BuyCashBackNavCValue = tr.TradeValue * cbCommit;
                    else
                    {
                        if (ot == null)
                            tr._BuyCashBackNavCValue = tr.TradeValue * cb * 0.6;
                        else
                            tr._BuyCashBackNavCValue = oo.PlacedOn > ot.PlacedOn ? tr.TradeValue * cb * 0.4 : tr.TradeValue * cb * 0.6;
                    }
                    if (cStatus == eCommunityCategory.None)
                        tr._BuyPoolRefund = tr._BuyCashBackNavCValue;
                    //non Binding, just estimation
                    tr._BuyAssetAmount = tr.TradeVolumn - (tr.TradeVolumn*SWAP);
                    tr._BuyAssetAmountValue = (tr._BuyAssetAmount * tr.TradePrice);// + tr._BuyCashBackNavCValue;
                }
            }
            else//Sell
            {
                var isAllowed = GetmySellTradingSwap(mCode, oo.UserAccountNo!, out var SWAP, out var cb, out var cbCommit, out var Limit, out cStatus);
                if (isAllowed)
                {
                    // Charge which ever is higer
                    //ToDo: Decide if we should update Persisted swOrder/Processed Order with Changed SWAP rate.
                    tr._SellSWAPRate = SWAP;
                    tr._SellSWAPValue = SWAP * tr.TradeValue;
                    var c = UserTradeCountOfToday(SpotWID);
                    if (c <= Limit)
                        tr._SellCashBackNavCValue = tr.TradeValue * cbCommit;
                    else
                    {
                        if (ot == null)
                            tr._SellCashBackNavCValue = tr.TradeValue * cb * 0.6;
                        else
                            tr._SellCashBackNavCValue = oo.PlacedOn > ot.PlacedOn ? tr.TradeValue * cb * 0.4 : tr.TradeValue * cb * 0.6;
                    }
                    if (cStatus == eCommunityCategory.None)
                        tr._SellPoolRefund = tr._SellCashBackNavCValue;
                    //non Binding, just estimation
                    tr._SellAssetAmount = tr.TradeVolumn - (tr.TradeVolumn * SWAP);
                    tr._SellAssetAmountValue = (tr._SellAssetAmount * tr.TradePrice);// + tr._SellCashBackNavCValue;
                }
            }
            db.SaveChanges();
        }
        //ToDo: Check if Referral Reward is payable and this is first Trade
        var us = dbctx.UserAccount.FirstOrDefault(x => x.FAccountNumber == oo.UserAccountNo);
        if (us != null && us.RefCode.RefferedBy.IsNOT_NullorEmpty())
        {
            isFirstTrade = !us.RefCode.myRefRewardProcessed;
        }

        return oo;
    }
    internal int UserTradeCountOfToday(Guid wID)
    {
        try
        {
            var lst = dbctx.WalletTransaction.Where(x => x.CreatedOn >= DateTime.UtcNow.Date && x.IsWithInMyWallet == false && x.ToWalletId == wID).ToList();
            return lst.Where(x => x.Narration.Contains("Trade CREDTI")).Count();
        }
        catch (Exception ex)
        {
            Console2.WriteLine_RED($"{T}|ERROR:UserTradeCountOfToday for Trade:{ex.GetDeepMsg()}");
        }

        return 0;
    }
    internal void RegisterTradeTokenSettlementIssue(smTrade tr, Exception Iss)
    {
        try
        {
            using (var db = Orderctx(tr.MarketCode))
            {
                var issue = new TradeIssues
                {
                    MarketCode = tr.MarketCode,
                    Issue = Iss.GetDeepMsg(),
                    BuyInternalId = tr.BuyInternalId,
                    BuyOrderID = tr.BuyOrderID,
                    SellInternalId = tr.SellInternalId,
                    SellOrderID = tr.SellOrderID,
                    TradeId = tr.TradeId,
                    TradePrice = tr.TradePrice,
                    TradeValue = tr.TradeValue,
                    TradeVolumn = tr.TradeVolumn
                };
                db.TradeIssues.Add(issue);
                db.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            Console2.WriteLine_RED($"{T}|Failed to Register Trade Token Settlement Issue\n{ex.GetDeepMsg()}");
            SrvPlugIn.LogErrorG($"{T}|Failed to Register Trade Token Settlement Issue\n{ex.GetDeepMsg()}");
        }
    }
    // market Making Order Package ToDo:Naveen MMOrderPackage
    internal smOrder GetBuyMMOrderPackage(mMMOrder ord, mUserSession sess)
    {
        double avaiBalance, total;
        var wm = GetWalletManager();
        var um = GetUserManager();

        //In order to STOP further MM Activity Market User Account should be suspended
        if (!sess.UserAccount.IsActive)
            throw new ApplicationException("InActive Market Maker User Account");

        var Order = ord.ToSrvModel();
        Order.id = ord.IterationID;//temp Place Iteration ID here
        var l = sess.UserAccount.AccountNumber.Split("-").ToList();
        Order.OrderID = $"{l.First()}{l.Last()}.{ord.MarketCode.ToUpper()}.{DateTime.UtcNow.Ticks}.{DateTime.UtcNow.Millisecond}";
        Order.UserAccountNo = sess.UserAccount.AccountNumber;
        Order.WalletID = sess.SpotWalletId;
        Order.AuthId = sess.SessionAuthEvent.mAuthId.ToString();
        Order.SessionId = sess.UserSessionId.ToString();
        //--
        avaiBalance = wm.GetAvailbaleConfirmBalanceOfWallet(sess.SpotWalletId, ord.QuoteTokenId).Item2;

        total = ord.Volume * ord.Price;
        if (avaiBalance >= total)
        {
            Order.AuthId = sess.SessionAuthEvent.mAuthId.ToString();
            Order.SessionId = sess.UserSessionId.ToString();

            var o = GetmyBuyTradingSwap(ord.MarketCode, sess.UserAccount.AccountNumber, out var SWAP, out var cbPctVal, out var cbCommitVal, out var Limit, out var cStatus);
            Order.CBCommitment = cbCommitVal;
            Order.CBThreashold = cbPctVal; //GetCBthreashold(ord.MarketCode, sess.UserAccount.AccountNumber);
            Order.SwapRate = SWAP;
            //------non Binding, just estimation
            var c = UserTradeCountOfToday(sess.SpotWalletId);
            if (c < Limit)
                Order.CashBackNavCValue = Order.Price * Order.OriginalVolume * cbCommitVal;
            else
                Order.CashBackNavCValue = Order.Price * Order.OriginalVolume * cbPctVal * 0.6;

            //non Binding, just estimation
            Order._OrderAssetAmount = (Order.Price * Order.OriginalVolume) - (Order.Price * Order.OriginalVolume * SWAP);
            Order._OrderSwapTradeValue = Order._OrderAssetAmount + Order.CashBackNavCValue;
            //------
            var wKey = SrvMarketProxy.GetOrderWalletForMarket(ord.MarketCode);
            //Token DEBIT Transaction
            var BNarr = $"{Order.MarketCode}|{Order.OrderID} Order DEBIT";
            //var narr = $"Market Order Trade Debit";
            var wTran = wm.CreateTransaction(ord.QuoteTokenId, sess.SpotWalletId, wKey, total, BNarr);
            //Transaction that Deposit Token in Market wallet will be used as InternalOrderId
            Order.InternalOrderID = wTran.WalletTransactionId;
        }
        else
        {
            throw new ApplicationException($"Insufficient funds in Spot Wallet:{Order.WalletID} for Market:{Order.MarketCode} with :{avaiBalance} than Required:{total} {ord.QuoteTokenCodeName} to Place Market Making Order.");
        }

        return Order;
    }
    internal smOrder GetSellMMOrderPackage(mMMOrder ord, mUserSession sess)
    {
        double avaiBalance, total;
        var wm = GetWalletManager();
        var um = GetUserManager();

        //In order to STOP further MM Activity Market User Account should be suspended
        if (!sess.UserAccount.IsActive)
            throw new ApplicationException("InActive Market Maker User Account");

        var Order = ord.ToSrvModel();
        Order.id = ord.IterationID;//temp Place Iteration ID here
        var l = sess.UserAccount.AccountNumber.Split("-").ToList();
        Order.OrderID = $"{l.First()}{l.Last()}.{ord.MarketCode.ToUpper()}.{DateTime.UtcNow.Ticks}.{DateTime.UtcNow.Millisecond}";
        Order.UserAccountNo = sess.UserAccount.AccountNumber;
        Order.WalletID = sess.SpotWalletId;
        Order.AuthId = sess.SessionAuthEvent.mAuthId.ToString();
        Order.SessionId = sess.UserSessionId.ToString();
        //--
        avaiBalance = wm.GetAvailbaleConfirmBalanceOfWallet(sess.SpotWalletId, ord.BaseTokenId).Item2;

        if (avaiBalance >= ord.Volume)
        {
            Order.AuthId = sess.SessionAuthEvent.mAuthId.ToString();
            Order.SessionId = sess.UserSessionId.ToString();
            var o = GetmyBuyTradingSwap(ord.MarketCode, sess.UserAccount.AccountNumber, out var SWAP, out var cbPctVal, out var cbCommitVal, out var Limit, out var cStatus);
            Order.CBCommitment = cbCommitVal;
            Order.CBThreashold = cbPctVal; //GetCBthreashold(ord.MarketCode, sess.UserAccount.AccountNumber);
            Order.SwapRate = SWAP;
            //------non Binding, just estimation
            var c = UserTradeCountOfToday(sess.SpotWalletId);
            if (c < Limit)
                Order.CashBackNavCValue = Order.Price * Order.OriginalVolume * cbCommitVal;
            else
                Order.CashBackNavCValue = Order.Price * Order.OriginalVolume * cbPctVal * 0.6;
            //non Binding, just estimation
            Order._OrderAssetAmount = (Order.Price * Order.OriginalVolume) - (Order.Price * Order.OriginalVolume * SWAP);
            Order._OrderSwapTradeValue = Order._OrderAssetAmount + Order.CashBackNavCValue;
            //------
            var wKey = SrvMarketProxy.GetOrderWalletForMarket(ord.MarketCode);
            //Token DEBIT Transaction
            var narr = $"{Order.MarketCode}|{Order.OrderID} Market Order Order Debit";
            var wTran = wm.CreateTransaction(ord.BaseTokenId, sess.SpotWalletId, wKey, ord.Volume, narr);
            //Transaction that Deposit Token in Market wallet will be used as InternalOrderId
            Order.InternalOrderID = wTran.WalletTransactionId;
        }
        else
        {
            throw new ApplicationException($"Insufficient funds in Spot Wallet:{Order.WalletID} for Market:{Order.MarketCode} with :{avaiBalance} than Required:{ord.Volume} {ord.BaseTokenCodeName} for Market Making Order.");
        }
        return Order;
    }
    //internal smOrder GetConvertSellOrderPackage(string mCode, mUserSession sess, mConvertTokenRequest Req)
    //{
    //    var wm = GetWalletManager();
    //    var Order = new smOrder
    //    {
    //        PlacedOn = DateTime.UtcNow,
    //        MarketCode = mCode,
    //        OrderType = ServerModel.eOrderType.Market,
    //        OrderSide = ServerModel.eOrderSide.Buy,
    //        BaseTokenCodeName = Req.bCode,
    //        BaseTokenId = Req.toTokenId,
    //        QuoteTokenCodeName = Req.qCode,
    //        QuoteTokenId = Req.fromTokenId,
    //        OrderID = $"{sess.UserAccount.AccountNumber}.{mCode.ToUpper()}.{DateTime.UtcNow.Ticks}.{DateTime.UtcNow.Millisecond}",
    //        UserAccountNo = sess.UserAccount.AccountNumber,
    //        WalletID = sess.FundingWalletId,
    //        OriginalVolume = Req.toTokenAmt,
    //        CurrentVolume = Req.toTokenAmt,
    //        Price = Req.RateOfOneToToken,
    //        //--
    //        AuthId = sess.SessionAuthEvent.mAuthId.ToString(),
    //        SessionId = sess.UserSessionId.ToString()
    //    };

    //    var MarketWallet = SrvMarketProxy.GetOrderWalletForMarket(mCode);

    //    //Token DEBIT Transaction
    //    var BNarr = $"{Order.MarketCode}|{Order.OrderID} Convert DEBIT";
    //    var wTran = wm.CreateTransaction(Order.BaseTokenId, sess.FundingWalletId, MarketWallet, Req.fromAmt, BNarr, isTrade: true);

    //    //Transaction that Deposit Token in Market wallet will be used as InternalOrderId
    //    Order.InternalOrderID = wTran.WalletTransactionId;

    //    return Order;

    //}
    //    internal smOrder GetConvertBuyOrderPackage(string mCode, mUserSession sess, mConvertTokenRequest Req)
    //{
    //    var wm = GetWalletManager();
    //    var Order = new smOrder
    //    {
    //        PlacedOn = DateTime.UtcNow,
    //        MarketCode = mCode,
    //        OrderType = ServerModel.eOrderType.Market,
    //        OrderSide = ServerModel.eOrderSide.Buy,
    //        BaseTokenCodeName = Req.bCode,
    //        BaseTokenId = Req.toTokenId,
    //        QuoteTokenCodeName = Req.qCode,
    //        QuoteTokenId = Req.fromTokenId,
    //        OrderID = $"{sess.UserAccount.AccountNumber}.{mCode.ToUpper()}.{DateTime.UtcNow.Ticks}.{DateTime.UtcNow.Millisecond}",
    //        UserAccountNo = sess.UserAccount.AccountNumber,
    //        WalletID = sess.FundingWalletId,
    //        OriginalVolume = Req.toTokenAmt,
    //        CurrentVolume = Req.toTokenAmt,
    //        Price = Req.RateOfOneToToken,
    //        //--
    //        AuthId = sess.SessionAuthEvent.mAuthId.ToString(),
    //        SessionId = sess.UserSessionId.ToString()
    //    };

    //    var MarketWallet = SrvMarketProxy.GetOrderWalletForMarket(mCode);

    //    //Token DEBIT Transaction
    //    var BNarr = $"{Order.MarketCode}|{Order.OrderID} Convert DEBIT";
    //    var wTran = wm.CreateTransaction(Order.QuoteTokenId, sess.FundingWalletId, MarketWallet, Req.fromAmt, BNarr, isTrade: true);

    //    //Transaction that Deposit Token in Market wallet will be used as InternalOrderId
    //    Order.InternalOrderID = wTran.WalletTransactionId;


    //    return Order;
    //}
    internal smOrder GetBuyOrderPackage(mOrder ord)
    {
        double avaiBalance, total;
        var wm = GetWalletManager();
        var um = GetUserManager();
        var sess = um.GetMySession();
        if (sess.ExpieredOn.HasValue || sess.ShouldExpierOn <= DateTime.UtcNow)
            throw new ApplicationException("User Session has Expired");

        if (!sess.UserAccount.IsActive)
            throw new ApplicationException("InActive User Account");

        var Order = ord.ToSrvModel();
        Order.OrderID = $"{sess.UserAccount.AccountNumber}.{ord.MarketCode.ToUpper()}.{DateTime.UtcNow.Ticks}.{DateTime.UtcNow.Millisecond}";
        Order.UserAccountNo = sess.UserAccount.AccountNumber;
        Order.WalletID = sess.SpotWalletId;

        //--
        avaiBalance = wm.GetAvailbaleConfirmBalanceOfWallet(sess.SpotWalletId, ord.QuoteTokenId).Item2;

        total = ord.Volume * ord.Price;
        if (avaiBalance >= total)
        {
            Order.AuthId = sess.SessionAuthEvent.mAuthId.ToString();
            Order.SessionId = sess.UserSessionId.ToString();
            var o = GetmyBuyTradingSwap(ord.MarketCode, sess.UserAccount.AccountNumber, out var SWAP, out var cbPctVal, out var cbCommitVal, out var Limit, out var cStatus);
            if (o == false)
                throw new ApplicationException($"User:{sess.UserAccount.AccountNumber} is not Authorized to place order in:{ord.MarketCode} due to its Tax Residency");
            Order.CBCommitment = cbCommitVal;
            Order.CBThreashold = cbPctVal;
            //GetCBthreashold(ord.MarketCode, sess.UserAccount.AccountNumber);
            Order.SwapRate = SWAP;
            //------non Binding, just estimation
            var c = UserTradeCountOfToday(sess.SpotWalletId);
            if (c < Limit)
                Order.CashBackNavCValue = Order.Price * Order.OriginalVolume * cbCommitVal;
            else
                Order.CashBackNavCValue = Order.Price * Order.OriginalVolume * cbPctVal * 0.6;

            //non Binding, just estimation
            Order._OrderAssetAmount = (Order.Price * Order.OriginalVolume) - (Order.Price * Order.OriginalVolume * SWAP);
            Order._OrderSwapTradeValue = Order._OrderAssetAmount + Order.CashBackNavCValue;
            //------

            var wKey = SrvMarketProxy.GetOrderWalletForMarket(ord.MarketCode);
            //Token DEBIT Transaction
            var BNarr = $"{Order.MarketCode}|{Order.OrderID} Trade DEBIT";
            var wTran = wm.CreateTransaction(ord.QuoteTokenId, sess.SpotWalletId, wKey, total, BNarr, isTrade: true);
            //Transaction that Deposit Token in Market wallet will be used as InternalOrderId
            Order.InternalOrderID = wTran.WalletTransactionId;
        }
        else
        {
            throw new ApplicationException("Insufficient funds in Spot Wallet for this Order.");
        }

        return Order;
    }
    internal smOrder GetSellOrderPackage(mOrder ord)
    {
        double avaiBalance, total;
        var wm = GetWalletManager();
        var um = GetUserManager();
        var sess = um.GetMySession();

        if (sess.ExpieredOn.HasValue || sess.ShouldExpierOn <= DateTime.UtcNow)
            throw new ApplicationException("User Session has Expired");
        if (!sess.UserAccount.IsActive)
            throw new ApplicationException("InActive User Account");

        var Order = ord.ToSrvModel();
        Order.OrderID = $"{sess.UserAccount.AccountNumber}.{ord.MarketCode.ToUpper()}.{DateTime.UtcNow.Ticks}.{DateTime.UtcNow.Millisecond}";

        Order.UserAccountNo = sess.UserAccount.AccountNumber;
        Order.WalletID = sess.SpotWalletId;
        //--
        avaiBalance = wm.GetAvailbaleConfirmBalanceOfWallet(sess.SpotWalletId, ord.BaseTokenId).Item2;

        if (avaiBalance >= ord.Volume)
        {
            Order.AuthId = sess.SessionAuthEvent.mAuthId.ToString();
            Order.SessionId = sess.UserSessionId.ToString();
            var o = GetmySellTradingSwap(ord.MarketCode, sess.UserAccount.AccountNumber, out var SWAP, out var cbPctVal, out var cbCommitVal, out var Limit, out var cStatus);
            if (o == false)
                throw new ApplicationException($"User:{sess.UserAccount.AccountNumber} is not Authorized to place order in:{ord.MarketCode} due to its Tax Residency");
            Order.CBCommitment = cbCommitVal;
            Order.CBThreashold = cbPctVal;
            Order.SwapRate = SWAP;
            //------non Binding, just estimation
            var c = UserTradeCountOfToday(sess.SpotWalletId);
            if (c < Limit)
                Order.CashBackNavCValue = Order.Price * Order.OriginalVolume * cbCommitVal;
            else
                Order.CashBackNavCValue = Order.Price * Order.OriginalVolume * cbPctVal * 0.6;
            //non Binding, just estimation
            Order._OrderAssetAmount = (Order.Price * Order.OriginalVolume) - (Order.Price * Order.OriginalVolume * SWAP);
            Order._OrderSwapTradeValue = Order._OrderAssetAmount + Order.CashBackNavCValue;
            //------
            var wKey = SrvMarketProxy.GetOrderWalletForMarket(ord.MarketCode);
            //Token DEBIT Transaction
            var SNarr = $"{Order.MarketCode}|{Order.OrderID} Trade DEBIT";
            var wTran = wm.CreateTransaction(ord.BaseTokenId, sess.SpotWalletId, wKey, ord.Volume, SNarr, isTrade: true);
            //Transaction that Deposit Token in Market wallet will be used as InternalOrderId
            Order.InternalOrderID = wTran.WalletTransactionId;
        }
        else
        {
            throw new ApplicationException("Insufficient funds in Spot Wallet for this Order.");
        }



        return Order;
    }
    //internal double GetCBthreashold(string mCode, string uAccount)
    //{
    //    //As per POLICY we will pay upto 0.05% of Value Addition/ Upto 5 Times of SWAP fee
    //    //This Information Should be Read From Database
    //    // Trader Staking Status will result in variation
    //}
    internal bool GetmyBuyTradingSwap(string mCode, string uAccount, out double SWAP, out double CashbackPctValue, out double CashbackCommitValue, out int Limit, out eCommunityCategory cStatus)
    {
        CashbackPctValue = 0;
        CashbackCommitValue = 0;
        Limit = 0;
        cStatus = eCommunityCategory.None;
        var um = GetUserManager();
        var wm = GetWalletManager();
        var cId = um.GetTaxResidencyOf(uAccount);

        //  return 0.002;//0.02%
        /* 0. Based on User Profile Country, We should Fetch the Market Profile for Such Country
         * - Base on mCode we should fetch Market FeeType as well.
         * - Based on FeeType (Standard) we should Fetch User's Community/NonCommunity Status
         * 1. Based on User, staking status, we should establish his Community status
         * 2. We should get the related Fee Percentage from the Market Profile
         */
        if (SrvMarketProxy.GetSWAPRateForMarket(mCode, cId, out var buySwap, out var sellSwap))
        {
            switch (buySwap.FeeType)
            {
                case Data.Entity.FeeType.Standard:
                    //Based on Community Status
                    SWAP = buySwap.FeeNonCommunity;
                    CashbackPctValue = SWAP * 5;

                    var lst = wm.GetMyStakings(uAccount).Where(x => x.StakingSlot.Community != eCommunityCategory.None && x.IsRedeemed == false).ToList();
                    if (lst.Count > 0)
                    {
                        SWAP = buySwap.FeeCommunity;
                        var cc = lst.MaxBy(x => x.StakingSlot.Community);
                        cStatus = cc.StakingSlot.Community;
                        switch (cStatus)
                        {

                            case eCommunityCategory.Community:
                                CashbackPctValue = SWAP * 5;
                                return true;
                            case eCommunityCategory.Standard:
                                CashbackPctValue = SWAP * 5;
                                //First 5 Trans at PayoutTime
                                CashbackCommitValue = SWAP * 5;
                                Limit = 5;
                                return true;
                            case eCommunityCategory.Premimum:
                                CashbackPctValue = SWAP * 5;
                                //First 10 Trans at PayoutTime
                                CashbackCommitValue = SWAP * 10;
                                Limit = int.MaxValue;
                                return true;
                            default:
                                break;
                        }
                    }
                    return true;
                case Data.Entity.FeeType.Exempt:
                    //Always free, So No Cashback either
                    SWAP = buySwap.FeeExempt;// usually Zero
                    CashbackPctValue = 0;
                    return true;
                case Data.Entity.FeeType.Independent:
                    //Apply Rate-it is Non-Community Market
                    // Usually private Market or New Tokens
                    SWAP = buySwap.FeeIndependent;
                    CashbackPctValue = 0;
                    return true;
                default:
                    break;
            }
        }

        //User Account should not have access to this Market due TaxResidency Status
        CashbackPctValue = SWAP = 0;
        return false;
    }
    internal bool GetmySellTradingSwap(string mCode, string uAccount, out double SWAP, out double CashbackPctValue, out double CashbackCommitValue, out int Limit, out eCommunityCategory cStatus)
    {
        CashbackPctValue = 0;
        CashbackCommitValue = 0;
        Limit = 0;
        cStatus = eCommunityCategory.None;
        // Console2.WriteLine_RED($"{T}ToDo:GetmyTradingSwap is not Implemented");
        var um = GetUserManager();
        var wm = GetWalletManager();
        var cId = um.GetTaxResidencyOf(uAccount);

        /*  return 0.002; //0.02%
         0. Based on User Profile Country, We should Fetch the Market Profile for Such Country
         * - Base on mCode we should fetch Market FeeType as well.
         * - Based on FeeType (Standard) we should Fetch User's Community/NonCommunity Status
         * 1. Based on User, staking status, we should establish his Community status
         * 2. We should get the related Fee Percentage from the Market Profile
         */

        if (SrvMarketProxy.GetSWAPRateForMarket(mCode, cId, out var buySwap, out var sellSwap))
        {
            switch (sellSwap.FeeType)
            {
                case Data.Entity.FeeType.Standard:
                    //Based on Community Status
                    SWAP = sellSwap.FeeNonCommunity;
                    CashbackPctValue = SWAP * 5;

                    var lst = wm.GetMyStakings(uAccount).Where(x => x.StakingSlot.Community != eCommunityCategory.None && x.IsRedeemed == false).ToList();
                    if (lst.Count > 0)
                    {
                        SWAP = sellSwap.FeeCommunity;
                        var cc = lst.MaxBy(x => x.StakingSlot.Community);
                        cStatus = cc.StakingSlot.Community;
                        switch (cStatus)
                        {

                            case eCommunityCategory.Community:
                                CashbackPctValue = SWAP * 5;
                                return true;
                            case eCommunityCategory.Standard:
                                CashbackPctValue = SWAP * 5;
                                //First 5 Trans at PayoutTime
                                CashbackCommitValue = SWAP * 5;
                                Limit = 5;
                                return true;
                            case eCommunityCategory.Premimum:
                                CashbackPctValue = SWAP * 5;
                                //First 10 Trans at PayoutTime
                                CashbackCommitValue = SWAP * 10;
                                Limit = int.MaxValue;
                                return true;
                            default:
                                break;
                        }
                    }
                    return true;
                case Data.Entity.FeeType.Exempt:
                    //Always free, So No Cashback either
                    SWAP = sellSwap.FeeExempt;// usually Zero
                    CashbackPctValue = 0;
                    return true;
                case Data.Entity.FeeType.Independent:
                    //Apply Rate-it is Non-Community Market
                    // Usually private Market or New Tokens
                    SWAP = sellSwap.FeeIndependent;
                    CashbackPctValue = 0;
                    return true;
                default:
                    break;
            }
        }

        //User Account should not have access to this Market due TaxResidency Status
        CashbackPctValue = SWAP = 0;
        return false;
    }
    internal smOrder? ConfirmOwnershipAndOrderToCancel(string uAcc, string InternalID, string mCode)
    {
        if (uAcc.IsNOT_NullorEmpty())
            uAcc = uAcc.ToLower();
        if (InternalID.IsNOT_NullorEmpty())
            InternalID = InternalID.ToUpper();

        using (var db = Orderctx(mCode))
        {
            var ret = db.Orders.FirstOrDefault(x => x.UserAccountNo != null && x.UserAccountNo.ToLower() == uAcc && x.InternalOrderID.ToString().ToUpper() == InternalID.ToUpper());
            return ret;
        }

    }
    //My ORDER
    internal List<rOrder> GetOpenOrdersOf(string userAccount)
    {
        var ret = new List<rOrder>();
        var mkts = SrvMarketProxy.GetMarketsWithOperatingQueue();
        foreach (var m in mkts)
        {
            var res = GetOpenOrdersOf(userAccount, m.CodeName);
            if (res != null && res.Count > 0)
                ret.AddRange(res);
        }

        return ret;
    }
    internal List<rOrder> GetOpenOrdersOf(string userAccount, string mCode)
    {
        userAccount = userAccount.ToLower();
        /* Only My Order that Resulted Debiting my Spot Wallet
         * Order that have ack received
         * ordfer have that Processed Order created
         * Only last 20 orders ? ?
         */
        using (var db = Orderctx(mCode))
        {
            var lst = db.Orders.Include(x => x.ProcessedOrder).Where(x => x.UserAccountNo == userAccount && mCode.ToLower() == x.MarketCode.ToLower() && x.ProcessedOrder.Count <= 0)
                .Select(
                oo => new rOrder
                {
                    MarketCode = oo.MarketCode,
                    MarketName = $"{oo.BaseTokenCodeName}/{oo.QuoteTokenCodeName}",
                    Status = (Model.eOrderStatus)oo.Status,
                    InternalOrderID = oo.InternalOrderID,
                    BaseTokenCodeName = oo.BaseTokenCodeName,
                    BaseTokenId = oo.BaseTokenId,
                    QuoteTokenCodeName = oo.QuoteTokenCodeName,
                    QuoteTokenId = oo.QuoteTokenId,
                    OriginalVolume = oo.OriginalVolume,
                    CurrentVolume = oo.OriginalVolume,
                    PlacedOn = oo.PlacedOn,
                    OrderSide = (Model.eOrderSide)oo.OrderSide,
                    OrderType = (Model.eOrderType)oo.OrderType,
                    OrderID = oo.OrderID,
                    Price = oo.Price,
                    _OrderSwapTradeValue = oo._OrderSwapTradeValue,
                    _OrderAssetAmount = oo._OrderAssetAmount,
                    _OrderTrigger = 00.000
                }).ToList();

            var olst = db.ProcessedOrder.Include(x => x.myOrder).Where(x => x.myOrderId != null && x.myOrder.UserAccountNo != null && x.myOrder.UserAccountNo.ToLower() == userAccount
            //&& (x.Status == ServerModel.eOrderStatus.Placed || x.Status == ServerModel.eOrderStatus.Received || x.Status == ServerModel.eOrderStatus.PartialCompleted)
            )
                 .ToList();

            var plst = olst.GroupBy(x => x.InternalOrderID, (k, v) => v.OrderByDescending(f => f.Status).First())
                 .Select(
                 oo => new rOrder
                 {
                     MarketCode = oo.MarketCode,
                     MarketName = $"{oo.BaseTokenCodeName}/{oo.QuoteTokenCodeName}",
                     Status = (Model.eOrderStatus)oo.Status,
                     InternalOrderID = oo.InternalOrderID,
                     BaseTokenCodeName = oo.BaseTokenCodeName,
                     BaseTokenId = oo.BaseTokenId,
                     QuoteTokenCodeName = oo.QuoteTokenCodeName,
                     QuoteTokenId = oo.QuoteTokenId,
                     OriginalVolume = oo.OriginalVolume,
                     CurrentVolume = oo.OriginalVolume - oo.ProcessedVolume,
                     PlacedOn = oo.PlacedOn,
                     OrderSide = (Model.eOrderSide)oo.OrderSide,
                     OrderType = (Model.eOrderType)oo.OrderType,
                     OrderID = oo.OrderID,
                     Price = oo.Price,
                     _OrderSwapTradeValue = oo.myOrder._OrderSwapTradeValue,
                     _OrderAssetAmount = oo.myOrder._OrderAssetAmount,
                     _OrderTrigger = 00.000
                 }).Where(x => x.CurrentVolume > 0).ToList();
            if (lst.Count > 0)
                plst.AddRange(lst);
            if (plst.Count > 0)
            {
                //do we need this..?
                var plst1 = plst.GroupBy(x => x.InternalOrderID).Select(x => x.MaxBy(z => z.Status)).ToList();
                plst1 = plst1.Where(x => x.Status <= Model.eOrderStatus.PartialCompleted).OrderByDescending(x => x.PlacedOn).ToList();
                return plst1;
            }
            return plst;
        }
    }
    internal List<rOrder> GetOrdersHistoryOf(string userAccount)
    {
        var ret = new List<rOrder>();

        var mkts = SrvMarketProxy.GetMarketsWithOperatingQueue();
        foreach (var m in mkts)
        {
            var res = GetOrdersHistoryOf(userAccount, m.CodeName);
            if (res != null && res.Count > 0)
                ret.AddRange(res);
        }
        return ret;
    }
    internal List<rOrder> GetOrdersHistoryOf(string userAccount, string mCode)
    {
        var ret = new List<eProcessedOrder>();
        using (var db = Orderctx(mCode))
        {
            var olst = db.ProcessedOrder.Include(x => x.myOrder).Where(x => x.myOrderId != null && x.myOrder.UserAccountNo != null && x.myOrder.UserAccountNo.ToLower() == userAccount && x.ProcessedOn >= DateTime.UtcNow.AddDays(-7))
                .ToList();
            var plst = olst.GroupBy(x => x.InternalOrderID, (k, v) => v.OrderByDescending(f => f.ProcessedOn)).ToList();

            foreach (var ord in plst)
            {
                //Since we can have split Order
                var l = ord.Where(x => x.Status >= ServerModel.eOrderStatus.Cancelled).ToList();
                if (l.Count <= 0)
                {
                    ret.Add(ord.OrderByDescending(x => x.Status).First());
                }
                else
                {
                    var gl = l.GroupBy(x => x.Status).Select(x => x.OrderByDescending(z => z.ProcessedOn).FirstOrDefault()).ToList();
                    ret.AddRange(gl);
                }
            }
            var rlst = ret.Select(
                  oo => new rOrder
                  {
                      MarketCode = oo.MarketCode,
                      MarketName = $"{oo.BaseTokenCodeName}/{oo.QuoteTokenCodeName}",
                      InternalOrderID = oo.InternalOrderID,
                      BaseTokenCodeName = oo.BaseTokenCodeName,
                      BaseTokenId = oo.BaseTokenId,
                      QuoteTokenCodeName = oo.QuoteTokenCodeName,
                      QuoteTokenId = oo.QuoteTokenId,
                      OriginalVolume = oo.OriginalVolume,
                      CurrentVolume = oo.OriginalVolume - oo.ProcessedVolume,
                      ProcessedVolume = oo.ProcessedVolume,
                      Trigger = oo.StopPrice,
                      PlacedOn = oo.PlacedOn,
                      OrderSide = (Model.eOrderSide)oo.OrderSide,
                      OrderType = (Model.eOrderType)oo.OrderType,
                      OrderID = oo.OrderID,
                      Price = oo.Price,
                      Status = (Model.eOrderStatus)oo.Status,
                      _OrderSwapTradeValue = oo.myOrder!._OrderSwapTradeValue,
                      _OrderAssetAmount = oo.myOrder!._OrderSwapTradeValue,
                  }).OrderByDescending(x => x.PlacedOn).ToList();
            return rlst;
        }
    }
    internal List<mTrade> GetRecentTradesOf(string userAccount, int count)
    {
        userAccount = userAccount.ToLower();
        count = count <= 0 ? 20 : count;
        var ret = new List<mTrade>();
        /* Only My Recent Trades
         * Only last 20 Trades ? ?
         */
        var mkts = SrvMarketProxy.GetMarketsWithOperatingQueue();
        foreach (var m in mkts)
        {

            using (var db = Orderctx(m.CodeName))
            {
                var blst = (from x in db.ProcessedOrder
                            where x.UserAccountNo != null && x.UserAccountNo.ToLower() == userAccount && x.ProcessedOn >= DateTime.UtcNow.AddDays(-7)
                            join t in db.Trades on x.InternalOrderID equals t.BuyInternalId
                            select new mTrade
                            {
                                OrderID = t.BuyOrderID,
                                CashBackNavCValue = t.CashBackNavCValue,
                                dateTimeUTC = t.dateTimeUTC,
                                MarketCode = m.ShortName,
                                SwapValue = t.SwapValue,
                                TradeId = t.TradeId,
                                OrderSide = ServerModel.eOrderSide.Buy,
                                TradePrice = t.TradePrice,
                                TradeValue = t.TradeValue,
                                TradeVolumn = t.TradeVolumn,
                                _SWAPRate = t._BuySWAPRate,
                                _AssetAmount = t._BuyAssetAmount,
                                _AssetAmountValue = t._BuyAssetAmount,
                                _CashBackNavCValue = t._BuyCashBackNavCValue,
                                _PoolRefund = t._BuyPoolRefund,
                                _SWAPValue = t._BuySWAPValue,
                            }).ToList();
                var slst = (from x in db.ProcessedOrder
                            where x.UserAccountNo != null && x.UserAccountNo.ToLower() == userAccount && x.ProcessedOn >= DateTime.UtcNow.AddDays(-7)
                            join t in db.Trades on x.InternalOrderID equals t.SellInternalId
                            select new mTrade
                            {
                                OrderID = t.SellOrderID,
                                CashBackNavCValue = t.CashBackNavCValue,
                                dateTimeUTC = t.dateTimeUTC,
                                MarketCode = m.ShortName,
                                SwapValue = t.SwapValue,
                                TradeId = t.TradeId,
                                OrderSide = ServerModel.eOrderSide.Sell,
                                TradePrice = t.TradePrice,
                                TradeValue = t.TradeValue,
                                TradeVolumn = t.TradeVolumn,
                                _SWAPRate = t._SellSWAPRate,
                                _AssetAmount = t._SellAssetAmount,
                                _AssetAmountValue = t._SellAssetAmount,
                                _CashBackNavCValue = t._SellCashBackNavCValue,
                                _PoolRefund = t._SellPoolRefund,
                                _SWAPValue = t._SellSWAPValue,
                            }).ToList();

                if (blst.Count() > 0)
                    ret.AddRange(blst);
                if (slst.Count() > 0)
                    ret.AddRange(slst);
            }

        }
        return ret.OrderByDescending(x => x.dateTimeUTC).Take(count).ToList();
    }
    internal List<mTrade> GetTradesOf(string userAccount, string mCode = "", int count = 0)
    {
        var ret = GetBuyTradesOf(userAccount, mCode, count);
        ret.AddRange(GetSellTradesOf(userAccount, mCode, count));

        userAccount = userAccount.ToLower();
        count = count == 0 ? 20 : count;

        /* Only My Recent Trades
         * Only last 20 Trades ? ?
         */

        if (count > 0)
            return ret.OrderByDescending(x => x.dateTimeUTC).Take(count).ToList();
        return ret.OrderByDescending(x => x.dateTimeUTC).ToList();
    }
    internal List<mTrade> GetBuyTradesOf(string userAccount, string mCode = "", int count = 0)
    {
        userAccount = userAccount.ToLower();
        count = count == 0 ? 20 : count;
        var ret = new List<mTrade>();

        if (mCode == string.Empty)
        {
            var mkts = SrvMarketProxy.GetMarketsWithOperatingQueue();
            foreach (var m in mkts)
            {
                using (var db = Orderctx(m.CodeName))
                {
                    var blst = (from x in db.ProcessedOrder
                                where x.UserAccountNo != null && x.UserAccountNo.ToLower() == userAccount && x.ProcessedOn >= DateTime.UtcNow.AddDays(-7)
                                join t in db.Trades on x.InternalOrderID equals t.BuyInternalId
                                select new mTrade
                                {
                                    OrderID = t.BuyOrderID,
                                    CashBackNavCValue = t.CashBackNavCValue,
                                    dateTimeUTC = t.dateTimeUTC,
                                    MarketCode = m.ShortName,
                                    OrderType = x.OrderType,
                                    SwapValue = t.SwapValue,
                                    TradeId = t.TradeId,
                                    OrderSide = ServerModel.eOrderSide.Buy,
                                    TradePrice = t.TradePrice,
                                    TradeValue = t.TradeValue,
                                    TradeVolumn = t.TradeVolumn,
                                    _SWAPRate = t._BuySWAPRate,
                                    _AssetAmount = t._BuyAssetAmount,
                                    _AssetAmountValue = t._BuyAssetAmount,
                                    _CashBackNavCValue = t._BuyCashBackNavCValue,
                                    _PoolRefund = t._BuyPoolRefund,
                                    _SWAPValue = t._BuySWAPValue,

                                }).ToList();
                    if (blst.Count() > 0)
                        ret.AddRange(blst);
                }
            }
        }
        else
        {
            var mkts = SrvMarketProxy.GetMarketsWithOperatingQueue();
            var m = mkts.First(x => x.CodeName.ToLower() == mCode.ToLower());
            using (var db = Orderctx(mCode))
            {
                var blst = (from x in db.ProcessedOrder
                            where x.UserAccountNo != null && x.UserAccountNo.ToLower() == userAccount && x.ProcessedOn >= DateTime.UtcNow.AddDays(-7)
                            join t in db.Trades on x.InternalOrderID equals t.BuyInternalId
                            select new mTrade
                            {
                                OrderID = t.BuyOrderID,
                                CashBackNavCValue = t.CashBackNavCValue,
                                dateTimeUTC = t.dateTimeUTC,
                                OrderType = x.OrderType,
                                MarketCode = m.ShortName,
                                SwapValue = t.SwapValue,
                                TradeId = t.TradeId,
                                OrderSide = ServerModel.eOrderSide.Buy,
                                TradePrice = t.TradePrice,
                                TradeValue = t.TradeValue,
                                TradeVolumn = t.TradeVolumn,
                                _SWAPRate = t._BuySWAPRate,
                                _AssetAmount = t._BuyAssetAmount,
                                _AssetAmountValue = t._BuyAssetAmount,
                                _CashBackNavCValue = t._BuyCashBackNavCValue,
                                _PoolRefund = t._BuyPoolRefund,
                                _SWAPValue = t._BuySWAPValue,

                            }).ToList();
                if (blst.Count() > 0)
                    ret.AddRange(blst);
            }
        }
        ret = ret.GroupBy(x => x.TradeId, (k, v) => v.First()).ToList();
        if (count > 0)
            return ret.OrderByDescending(x => x.dateTimeUTC).Take(count).ToList();
        return ret.OrderByDescending(x => x.dateTimeUTC).ToList();
    }
    internal List<mTrade> GetSellTradesOf(string userAccount, string mCode = "", int count = 0)
    {
        userAccount = userAccount.ToLower();
        count = count == 0 ? 20 : count;
        var ret = new List<mTrade>();

        if (mCode == string.Empty)
        {
            var mkts = SrvMarketProxy.GetMarketsWithOperatingQueue();
            foreach (var m in mkts)
            {
                using (var db = Orderctx(m.CodeName))
                {
                    var slst = (from x in db.ProcessedOrder
                                where x.UserAccountNo != null && x.UserAccountNo.ToLower() == userAccount && x.ProcessedOn >= DateTime.UtcNow.AddDays(-7)
                                join t in db.Trades on x.InternalOrderID equals t.SellInternalId
                                select new mTrade
                                {
                                    OrderID = t.SellOrderID,
                                    CashBackNavCValue = t.CashBackNavCValue,
                                    dateTimeUTC = t.dateTimeUTC,
                                    OrderType = x.OrderType,
                                    MarketCode = m.ShortName,
                                    SwapValue = t.SwapValue,
                                    TradeId = t.TradeId,
                                    OrderSide = ServerModel.eOrderSide.Sell,
                                    TradePrice = t.TradePrice,
                                    TradeValue = t.TradeValue,
                                    TradeVolumn = t.TradeVolumn,
                                    _SWAPRate = t._BuySWAPRate,
                                    _AssetAmount = t._SellAssetAmount,
                                    _AssetAmountValue = t._SellAssetAmount,
                                    _CashBackNavCValue = t._SellCashBackNavCValue,
                                    _PoolRefund = t._SellPoolRefund,
                                    _SWAPValue = t._SellSWAPValue,
                                }).ToList();
                    if (slst.Count() > 0)
                        ret.AddRange(slst);
                }
            }
        }
        else
        {
            var mkts = SrvMarketProxy.GetMarketsWithOperatingQueue();
            var m = mkts.First(x => x.CodeName.ToLower() == mCode.ToLower());
            using (var db = Orderctx(m.CodeName))
            {
                var slst = (from x in db.ProcessedOrder
                            where x.UserAccountNo != null && x.UserAccountNo.ToLower() == userAccount && x.ProcessedOn >= DateTime.UtcNow.AddDays(-7)
                            join t in db.Trades on x.InternalOrderID equals t.SellInternalId
                            select new mTrade
                            {
                                OrderID = t.SellOrderID,
                                CashBackNavCValue = t.CashBackNavCValue,
                                dateTimeUTC = t.dateTimeUTC,
                                MarketCode = m.ShortName,
                                OrderType = x.OrderType,
                                SwapValue = t.SwapValue,
                                TradeId = t.TradeId,
                                OrderSide = ServerModel.eOrderSide.Sell,
                                TradePrice = t.TradePrice,
                                TradeValue = t.TradeValue,
                                TradeVolumn = t.TradeVolumn,
                                _SWAPRate = t._BuySWAPRate,
                                _AssetAmount = t._SellAssetAmount,
                                _AssetAmountValue = t._SellAssetAmount,
                                _CashBackNavCValue = t._SellCashBackNavCValue,
                                _PoolRefund = t._SellPoolRefund,
                                _SWAPValue = t._SellSWAPValue,
                            }).ToList();
                if (slst.Count() > 0)
                    ret.AddRange(slst);
            }
        }
        ret = ret.GroupBy(x => x.TradeId, (k, v) => v.First()).ToList();

        if (count > 0)
            return ret.OrderByDescending(x => x.dateTimeUTC).Take(count).ToList();
        return ret.OrderByDescending(x => x.dateTimeUTC).ToList();
    }
    internal async Task<mConvertTokenRequest> ConvertToken(mConvertTokenRequest Req)
    {
        /* Get Session Info, Authenticate
         * Get Wallet Balance Check
         * Get List of Markets that will be hopped for this conversion
         * Track this Conversion and Record Every Step output
         */
        mConvertTokenRequest retval = new mConvertTokenRequest();
        try
        {
            var cm = new CommonDataManager();
            var mm = GetMarketManager();
            var tm = GetTokenManager();
            var wm = GetWalletManager();
            var um = GetUserManager();
            var sess = um.GetMySession();
            double frmBal = 0;
            if (sess.ExpieredOn.HasValue || sess.ShouldExpierOn <= DateTime.UtcNow)
                throw new ApplicationException("User Session has Expired");
            if (!sess.UserAccount.IsActive)
                throw new ApplicationException("InActive User Account");

            if (!(wm.DoesCoinExistAndCanTrade(Req.fromTokenId).Item2 && wm.DoesCoinExistAndCanTrade(Req.toTokenId).Item2))
                throw new ApplicationException("Provided Token is no longer available for Trading");

            var fAvaiBalance = wm.GetAvailbaleConfirmBalanceOfWallet(sess.FundingWalletId, Req.fromTokenId).Item2;
            var sAvaiBalance = wm.GetAvailbaleConfirmBalanceOfWallet(sess.SpotWalletId, Req.fromTokenId).Item2;

            var Next = fAvaiBalance < Req.fromAmt;
            Next = Next && Req.IsSpotWalletAllowed;
            Next = Next && (fAvaiBalance + sAvaiBalance) >= Req.fromAmt;
            Next = Next && (fAvaiBalance - Req.fromAmt >= 0 ? true : wm.SendTokenFromSpotToFunding(Req.fromTokenId, Req.fromAmt - fAvaiBalance));

            frmBal = wm.GetAvailbaleConfirmBalanceOfWallet(sess.FundingWalletId, Req.fromTokenId).Item2;

            if (frmBal < Req.fromAmt)
                throw new ApplicationException("Insufficient Balance in Fund Wallet");

            //Now we Convert TOKEN that after establishing the Market
            var gw = wm.GetGlobalWallet();
            //Token DEBIT Transaction
            var BNarr = $"{Req.bCode}{Req.qCode}|@{Req.RateOfOneToToken} to {sess.UserAccount} Convert DEBIT";
            var BNarr2 = $"{Req.bCode}{Req.qCode}|@{Req.RateOfOneToToken} to {sess.UserAccount} Convert CREDIT";
            var wTran = wm.CreateTransaction(Req.fromTokenId, sess.FundingWalletId, gw.InternalWalletId, Req.fromAmt, BNarr, isTrade: true);

            var wTran2 = wm.CreateTransaction(Req.toTokenId, gw.InternalWalletId, sess.FundingWalletId, Req.toTokenAmt, BNarr2, isTrade: true);

            Req.TradeId = wTran2.WalletTransactionId;
            Req.TransactionId = wTran.WalletTransactionId;


            //------ Direct Trade inTrade Engine
            //var m = SrvCoinWatch.GetAllCoins();
            //var d = m.FirstOrDefault(x => x.TokenName.ToLower() == $"{Req.bCode}{Req.qCode}".ToLower());
            //if (d != null)
            //{
            //    //Direct Market -Buy Order
            //    var price = tm.GetTokenMarketEstimatedBuyPrice(d.TokenName, Req.toTokenAmt);
            //    Req.toTokenAmt = Req.fromAmt / price;
            //    Req.RateOfOneToToken = price;
            //    var sm = GetConvertBuyOrderPackage($"{Req.bCode}{Req.qCode}", sess, Req);
            //    sm = SaveToDB(sm);
            //    var ret = mm.PlaceOrder(sm);
            //    retval.TransactionId = sm.InternalOrderID;
            //    return retval;
            //}

            //d = m.FirstOrDefault(x => x.TokenName.ToLower() == $"{Req.qCode}{Req.bCode}".ToLower());
            //if (d != null)
            //{
            //    //Direct Market -Sell Order

            //    var price = tm.GetTokenMarketEstimatedBuyPrice(d.TokenName, Req.toTokenAmt);
            //    Req.fromAmt = Req.toTokenAmt * price;
            //    Req.RateOfOneToToken = price;
            //    var sm = GetConvertSellOrderPackage($"{Req.bCode}{Req.qCode}", sess, Req);
            //    sm = SaveToDB(sm);
            //    var ret = mm.PlaceOrder(sm);
            //    retval.TransactionId = sm.InternalOrderID;
            //    return retval;
            //}
            ////Now we are in double Trade Zone
            ////-To USDT
            //// -From USDT
            //var toUSDT = m.FirstOrDefault(x => x.TokenName.ToLower() == $"USDT{Req.qCode}".ToLower());
            //var toNavC = m.FirstOrDefault(x => x.TokenName.ToLower() == $"{Req.bCode}USDT".ToLower());

            //if (toUSDT != null && toNavC != null)
            //{
            //   var USDTmkt= SrvMarketProxy.GetMarketsWithOperatingQueue().FirstOrDefault(x => x.CodeName.ToUpper() == toUSDT.TokenName.ToUpper());
            //   var NavCmkt = SrvMarketProxy.GetMarketsWithOperatingQueue().FirstOrDefault(x => x.CodeName.ToUpper() == toNavC.TokenName.ToUpper());
            //    if (USDTmkt == null || NavCmkt == null) throw new ApplicationException("Though Market Exist but they do not have Active & Operating Queues");

            //    //First Trade will be to secure Last Trade Currency(USDT)
            //    var USDTReq = new mConvertTokenRequest {bCode= USDTmkt.BaseToken!.Code, qCode= USDTmkt.QuoteToken!.Code, toTokenId= USDTmkt.BaseToken.TokenId,fromTokenId= USDTmkt.QuoteToken.TokenId };

            //    var NavCReq = new mConvertTokenRequest {bCode=NavCmkt.BaseToken!.Code, qCode= NavCmkt.QuoteToken!.Code, toTokenId = NavCmkt.BaseToken.TokenId,fromTokenId=NavCmkt.QuoteToken.TokenId };

            //    var firstRate = tm.GetTokenMarketEstimatedBuyPrice(toNavC.TokenName, Req.toTokenAmt);
            //    USDTReq.toTokenAmt = Req.toTokenAmt * firstRate;
            //    USDTReq.RateOfOneToToken = firstRate;

            //    var sm = GetConvertBuyOrderPackage(USDTmkt.CodeName, sess, USDTReq);
            //    sm = SaveToDB(sm);
            //    retval.TransactionId = sm.InternalOrderID;
            //    //ToDo:Schedule 2nd Order when First Completed..
            //    var ret = mm.PlaceOrder(sm);
            //    return retval;
            //}


            //--
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Occoured in ConvertToken..at:{DateTime.UtcNow}\n\t{ex.GetDeepMsg()}");
            if (ex is ApplicationException || ex is ArgumentException || ex is InvalidOperationException)
            {
                retval.ErrMsg = ex.Message;
            }
            else
            {
                Console2.WriteLine_RED($"ERROR:{System.Reflection.MethodBase.GetCurrentMethod().Name} in {this.GetType().Name} caused error:{ex.GetDeepMsg()}");
                retval.ErrMsg = "Technical Issue. Please try again later";
            }
            retval.IsError = true;
        }
        return retval;
    }


    private MarketManager GetMarketManager()
    {
        var result = new MarketManager();
        result.dbctx = dbctx;
        result.httpContext = httpContext;
        return result;
    }
    private WalletManager GetWalletManager()
    {
        var result = new WalletManager();
        result.dbctx = dbctx;
        result.httpContext = httpContext;
        return result;
    }
    private UserManager GetUserManager()
    {
        var result = new UserManager();
        result.dbctx = dbctx;
        result.httpContext = httpContext;
        return result;
    }
    private TokenManager GetTokenManager()
    {
        var result = new TokenManager();
        result.dbctx = dbctx;
        result.httpContext = httpContext;
        return result;
    }
    private OrderAppContext Orderctx(string mCode)
    {
        try
        {
            var o = new DbContextOptionsBuilder<OrderAppContext>();
            var str = ConfigEx.Config.GetConnectionString($"OrderAppContext");
            str = str!.Replace("<template>", $"OrdBank{mCode}");
            o = o.UseSqlServer(str);
            var ctx = new OrderAppContext(o.Options);
            // ctx.Database.SetConnectionString(str);
            ctx.Database.EnsureCreated();
            return ctx;
        }
        catch (Exception ex)
        {
            SrvPlugIn.LogErrorG($"{AppConfigBase.RegistryToken!.AppId}- in Market:{mCode}|{T} in Connection caused error in Order SQL Connection/Object {ex.GetDeepMsg()}");
        }
        return null;
    }
    private MMOrderAppContext MMOrderctx(string mCode)
    {
        try
        {
            var o = new DbContextOptionsBuilder<MMOrderAppContext>();
            var str = ConfigEx.Config.GetConnectionString($"MMAppContext");
            var po = ConfigEx.Config.GetSection("OrderPostFix").Value;
            str = str!.Replace("<template>", $"MMOrdBank{mCode}{po}");
            o = o.UseSqlServer(str);
            var ctx = new MMOrderAppContext(o.Options);
            // ctx.Database.SetConnectionString(str);
            ctx.Database.EnsureCreated();
            return ctx;
        }
        catch (Exception ex)
        {
            SrvPlugIn.LogErrorG($"{AppConfigBase.RegistryToken!.AppId}- in MMOrderCtx Market:{mCode}|{T} in Connection caused error in Order SQL Connection/Object {ex.GetDeepMsg()}");
        }
        return null;
    }

}
