using Azure.Core;

namespace NavExM.Int.Maintenance.APIs.Model;
public class mAlertMsgBody
{
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime GeneratedOn { get; set; } = DateTime.UtcNow;
    public DateTime ViewOn { get; set; } = DateTime.MinValue;
}
public static class AlertMsg
{
    public static mAlertMsgBody Login_UserLogin(string uEmail, string dName, string time, string ip)
    {
        return new mAlertMsgBody
        {
            Title = "Login attempted from new IP",
            Body = $"\r\nThe system has detected that your account is logged in from an New IP address.\r\nAccount: {uEmail}\r\nDevice: {dName}\r\nTime: {time}\r\nIP: {ip}.\r\n\r\nIf this activity is not your own operation, please disable your account and contact us immediately."
        };
    }
    public static mAlertMsgBody Login_ResetPwd()
    {
        return new mAlertMsgBody
        {
            Title = "Password Reset Successfully.",
            Body = $"Your password have been reset successfully."
        };
    }
    public static mAlertMsgBody Reward_YourFirstTradeReward()
    {
        return new mAlertMsgBody
        {
            Title = "Your First Trade Reward.",
            Body = $"Your First Trade reward has been Deposited! Happy Trading..."
        };
    }
    public static mAlertMsgBody KYC_Verification()
    {
        return new mAlertMsgBody
        {
            Title = "Complete your Identify Verification.",
            Body = $"Verify Your identity to avail multiple features of NavExM Exchange"
        };
    }
    public static mAlertMsgBody KYC_NotCompleted()
    {
        return new mAlertMsgBody
        {
            Title = "Complete your Identify Verification.",
            Body = $"Complete Your identity verification to avail multiple features of NavExM Exchnage"
        };
    }
    public static mAlertMsgBody KYC_Completed()
    {
        return new mAlertMsgBody
        {
            Title = "Identify Verification Completed.",
            Body = $"Your KYC request has been submitted."
        };
    }
    public static mAlertMsgBody DepositPayment_AddNew()
    {
        return new mAlertMsgBody
        {
            Title = "New Deposit & Payment Method is Added",
            Body = $"Your request for addition of new Deposit & payment method is Approved!"
        };
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="v1">no of crypto</param>
    /// <param name="v2">name of crypto</param>
    /// <returns></returns>
    public static mAlertMsgBody Staking_Confirmed(string v1, string v2) //v1 no of crypto, v2 crypto name
    {
        return new mAlertMsgBody
        {
            Title = "Staking Confirmed.",
            Body = $"Your Staking of {v1}{v2} is Successful."
        };
    }
    public static mAlertMsgBody Staking_MatureAfterTP(string v1, string v2)//v1 no of crypto, v2 crypto name
    {
        return new mAlertMsgBody
        {
            Title = "Staking Mature.",
            Body = $"Your Staking of {v1}{v2} is Mature! \r\n Check wallet for view"
        };
    }
    public static mAlertMsgBody Staking_Restake(string v1, string v2)//v1 no of crypto, v2 crypto name
    {
        return new mAlertMsgBody
        {
            Title = "Staking restake Succsessfuly.",
            Body = $"Your Staking of {v1}{v2} is restake!\r\n Check wallet for view  "

        };
    }
    public static mAlertMsgBody Staking_ReedemSuccsessfuly(string v1, string v2)//v1 no of crypto, v2 crypto name
    {
        return new mAlertMsgBody
        {
            Title = "Staking Reedmed Succsessfuly.",
            Body = $"Your request for reedment Staking for {v1}{v2} is Approved!\r\n Check wallet for view  "

        };
    }
    public static mAlertMsgBody Deposit_RequestSubmited(string v1, string v2)//v1 no of crypto, v2 crypto name
    {
        return new mAlertMsgBody
        {
            Title = "Deposit Request Submitted.",
            Body = $"Your deposit request for {v1}{v2} is  Submited!\r\n Check wallet for view  "

        };
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="v3">request id</param>
    /// <returns></returns>
    public static mAlertMsgBody Deposit_Confirmed(string v3)//v3 request id
    {
        return new mAlertMsgBody
        {
            Title = "Deposit Confirmed!",
            Body = $"Your deposit Request Id {v3} for crypto deposit is successful."

        };
    }
    public static mAlertMsgBody Withdrawal_RequestSubmited(string v1, string v2)//v1 no of crypto, v2 crypto name
    {
        return new mAlertMsgBody
        {
            Title = "Withdrawal Request Submitted.",
            Body = $"Your request for {v1}{v2} withdrawal is  Submited!\r\n Check wallet for view"

        };
    }
    public static mAlertMsgBody Withdrawal_Confirmed(string v3)//v3 request id
    {
        return new mAlertMsgBody
        {
            Title = "Your Crypto withdrawal requests is successful.",
            Body = $"Your withdrawal Request Id {v3} for crypto withdrawal is successful.\r\n Check wallet for view"

        };
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="v5">amount of fiat</param>
    /// <param name="v6">name of fiat</param>
    /// <returns></returns>
    public static mAlertMsgBody FiatDeposit_RequestSubmited(string v5, string v6)//v5 amount, v6 fiat name
    {
        return new mAlertMsgBody
        {
            Title = "Deposit Request Submitted.",
            Body = $"Your request for Deposit {v5} {v6} is Submited!\r\n Check wallet for view"

        };
    }
    public static mAlertMsgBody FiatDeposit_Confirmed(string v3)//v3 request id
    {
        return new mAlertMsgBody
        {
            Title = "Your Fiat deposit request is successful.",
            Body = $"Your deposit Request Id {v3}. for Fiat deposit is successful.\r\n Check wallet for view"

        };
    }
    public static mAlertMsgBody FiatWithdrawal_RequestSubmited(string v1, string v2)//v1 no of crypto, v2 crypto name
    {
        return new mAlertMsgBody
        {
            Title = "Withdrawal Request Submitted.",
            Body = $"Your request for {v1}{v2} withdrawal is  Submited ! \r\n Check wallet for view"

        };
    }
    public static mAlertMsgBody FiatTransfer_Account(string v3)//v3 request id
    {
        return new mAlertMsgBody
        {
            Title = "Your Fiat withdrawal requests is successful.",
            Body = $"Your withdrawal Request Id {v3} for Fiat withdrawal is successful. \r\n Check wallet for view"

        };
    }
    //trade order
    public static mAlertMsgBody TSellOrder_Placed()
    {
        return new mAlertMsgBody
        {
            Title = "Order placed!",
            Body = $"Your Sell Order has been placed successfully. \r\n Check order history for view."

        };
    }
    public static mAlertMsgBody TSellOrder_Cancelled(string v4)//v4 order id
    {
        return new mAlertMsgBody
        {
            Title = "Your order has been cancelled!",
            Body = $"Your sell order id {v4} cancelled. \r\n Check order history for view."

        };
    }
 
    public static mAlertMsgBody TSellOrder_Successful(string v4)//v4 order id
    {
        return new mAlertMsgBody
        {
            Title = "Your sell order has been executed successful!",
            Body = $"Your sell order id {v4} successfully! \r\n Check order history for view."

        };
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="v4">Order Id</param>
    /// <returns></returns>
    public static mAlertMsgBody PartialTSellOrder_Successful(string v1, string v2, string v4)//v4 order id, v1 no of crypto, v2 crypto name
    {
        return new mAlertMsgBody
        {
            Title =$"Your sell order has been executed for {v1}{v2}.",
            Body = $"Your sell order id {v4} successfully! \r\n Check order history for view."

        };
    }
    public static mAlertMsgBody TSellOrder_Rejected(string v4)//v4 order id
    {
        return new mAlertMsgBody
        {
            Title ="Sell Order has been rejected/ cancelled by the exchange!",
            Body = $"Your sell order id {v4} has been rejected/ cancelled by the exchange due to technical glitch. \r\n Check order history for view."

        };
    }
    public static mAlertMsgBody TBuyOrder_Placed()
    {
        return new mAlertMsgBody
        {
            Title = "Order placed!",
            Body = $"Your Buy Order has been placed successfully.\r\n Check order history for view."

        };
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="v4">order id</param>
    /// <returns></returns>
    public static mAlertMsgBody TBuyOrder_Cancelled(string v4)//v4 order id
    {
        return new mAlertMsgBody
        {
            Title = "Your order has been cancelled!",
            Body = $"Your buy order id {v4} cancelled. \r\n Check order history for view."

        };
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="v4">order id</param>
    /// <returns></returns>
    public static mAlertMsgBody TBuyOrder_Successful(string v4)//v4 order id
    {
        return new mAlertMsgBody
        {
            Title = "Your buy order has been executed successful!",
            Body = $"Your buy order id {v4} successfully! "

        };
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="v1">no of crypto</param>
    /// <param name="v2">name of crypto</param>
    /// <param name="v4">order id</param>
    /// <returns></returns>
    public static mAlertMsgBody PartialTBuyOrder_Successful(string v1, string v2, string v4)//v4 order id, v1 no of crypto, v2 crypto name
    {
        return new mAlertMsgBody
        {
            Title = $"Your buy order has been executed for {v1}{v2}.",
            Body = $"Your buy order id {v4} successfully! \r\n Check order history for view."

        };
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="v4">order id</param>
    /// <returns></returns>
    public static mAlertMsgBody TBuyOrder_Rejected(string v4)//v4 order id
    {
        return new mAlertMsgBody
        {
            Title = "Buy Order has been rejected/ cancelled by the exchange!",
            Body = $"Your buy order id {v4} has been rejected/ cancelled by the exchange due to technical glitch. \r\n Check order history for view."

        };
    }
    //buy crypto
    /// <summary>
    /// 
    /// </summary>
    /// <param name="v4">order id</param>
    /// <returns></returns>
    public static mAlertMsgBody BuyOrder_Successful(string v4)//v4 order id
    {
        return new mAlertMsgBody
        {
            Title = "Your buy order successful!",
            Body = $"Your buy order id {v4} successful! \r\n Check transaction history for view."

        };
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="v4">order id</param>
    /// <returns></returns>
    public static mAlertMsgBody ConvertOrder_Successful(string v4)//v4 order id
    {
        return new mAlertMsgBody
        {
            Title = "Your Convert order successful!",
            Body = $"Your convert order id {v4} successful! \r\n Check transaction history for view."

        };
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="v4">order id</param>
    /// <returns></returns>
    public static mAlertMsgBody ConvertWithdraw_Successful(string v4)//v4 order id
    {
        return new mAlertMsgBody
        {
            Title = "Your convert & withdraw is  successful!",
            Body = $"Your convert & withdraw order id {v4} successful! \r\n Check transaction history for view."

        };
    }
    public static mAlertMsgBody Cashback_Credit()
    {
        return new mAlertMsgBody
        {
            Title = "Cashback credited to your wallet!",
            Body = $"Your cashback for the period credited to your earn wallet.\r\n Check transaction history for view."

        };
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="v3">request id</param>
    /// <returns></returns>
    public static mAlertMsgBody InterWallet_requests(string v3)//v3 request id
    {
        return new mAlertMsgBody
        {
            Title = "Your Inter wallet requests is successful.",
            Body = $"Your request id {v3} for Inter wallet is successful.\r\n Check transaction history for view."

        };
    }
    public static mAlertMsgBody Rewards_Credited( ) 
    {
        return new mAlertMsgBody
        {
            Title = "Referral reward credited to your wallet!",
            Body = $"Your Referral reward  for the period credited to your earn wallet.\r\n Check transaction history for view."

        };
    }
    public static mAlertMsgBody Signup_Successfull()
    {
        return new mAlertMsgBody
        {
            Title = "Your email id have been registered successfully.",
              Body =" "
        };
    }
    public static mAlertMsgBody M_BuyOrder_Insufficient()
    {
        return new mAlertMsgBody
        {
            Title = " ",
            Body = $"Unable to place order, as Insufficient balance for the corresponding amount of assets due to rapid rise in prices. Place buy order again by reducing the amount or use Total instead of Amount."

        };
    }
    public static mAlertMsgBody M_BuyOrder_Sufficient()
    {
        return new mAlertMsgBody
        {
            Title = " ",
            Body = $"Market Order has been placed successfully! visit Open Orders"

        };
    }
    public static mAlertMsgBody M_SellOrder_Insufficient()
    {
        return new mAlertMsgBody
        {
            Title = " ",
            Body = $"Unable to place order, as Insufficient balance for the corresponding amount of assets due to rapid rise in prices. Place sell order again by reducing the Total or use Amount instead of Total."

        };
    }
    public static mAlertMsgBody M_SellOrder_Sufficient()
    {
        return new mAlertMsgBody
        {
            Title = " ",
            Body = $"Market Order has been placed successfully! visit Open Orders."

        };
    }
    public static mAlertMsgBody Whitlisting_WalAddress()
    {
        return new mAlertMsgBody
        {
            Title = "Wallet Address is whitelisted successfully.",
            Body = " "

        };
    }
    public static mAlertMsgBody Setup_2Factor_Authentication()
    {
        return new mAlertMsgBody
        {
            Title = "2F is unabled  successfully..",
            Body = " "

        };
    }
}
