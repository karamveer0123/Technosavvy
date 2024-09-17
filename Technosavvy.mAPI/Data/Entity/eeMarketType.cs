namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    public enum eeMarketType
    {
        CryptoCrypto,
        CryptoStable,
        StableStable,
        StableFiat,
        CryptoFiat,
    }
    public enum eeKYCStatus
    {
        NotStarted, 
        Started, 
        Pending, 
        AdditionalInfo, 
        UnderProcess, 
        Completed
    }
}
