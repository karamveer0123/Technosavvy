namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    public enum eGlobalPaymentType
    {
        //This List can be Increased to n resons
        NoReasonsSpecified=0,
        FiatCredited,
        MarketMakingTokensCredited,
        StakingARYPayment,
        StakingRefunds,
        InternalProcessWallet
    }
}
