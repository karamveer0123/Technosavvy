namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    /// <summary>
    /// This will Represent balance of all the funds/Tokens of a funding wallet
    /// </summary>
    /// <remarks>
    /// 1. It will hold Balance in either Fiat, Crypto, FiatCrypto, as per CurrencyType Property.
    /// 2.  Balance will Represent the current Balance of such Token/Fiat as per ChangeAgent (TransactionId), So Latest Balance of the Token would be Recorded Last
    /// 3. Fiat Balance must be matched with related Currency Bank Transaction, as duly Recorded in Bank Account Transaction Records
    /// 
    /// </remarks>
    [Index("Balance")]
    [Index("ChangeAgent")]
    [Index("TokenId")]
    [Index("FiatCurrencyId")]
    [Index("FundingWalletId")]
    [Index("CurrencyType")]
    public class eFundingWBalance: secBaseEntity1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FundingWBalanceId { get; set; }
        public double Balance { get; set; }

        public Guid ChangeAgent { get; set; }//Transaction Id
        public eCurrencyType CurrencyType { get; set; }
       
        [ForeignKey("Token")]
        public Guid? TokenId { get; set; }
        public eToken Token { get; set; }
        //OR
        [ForeignKey("FiatCurrency")]
        public Guid? FiatCurrencyId { get; set; }
        public eFiatCurrency? FiatCurrency { get; set; }

        [ForeignKey("FundingWallet")]
        public Guid FundingWalletId { get; set; }
        public eFundingWallet FundingWallet { get; set; }
    }
}
