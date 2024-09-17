namespace NavExM.Int.Maintenance.APIs.Data.Entity.PreBeta
{
    public class ePreBetaStage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public byte[] RowVersion { get; set; }
        [StringLength(200)]
        public string StageName { get; set; }
        public DateTime StartDate { get; set; }
        public double NavCSellPrice { get; set; }
        public double TokenCap { get; set; }
        public DateTime? EndDate { get; set; }

    }
    public class eFractionFactor : BaseEntity1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public double FractionFactor { get; set; }
        public string keyName { get; set; }
    }
    [Index("TranHash", IsUnique = true)]
    [Index("WalletAddress")]
    [Index("userAccount")]
    [Index("userAccountId")]
    [Index("TokenAddress")]
    public class ePBMyPurchaseRecords
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public DateTime DateOf { get; set; } = DateTime.UtcNow;
        //  public double BuyWith { get; set; }
        public double NavCAmountPurchased { get; set; }
        public double NavCUnitRate { get; set; }//in USDT
        public double TokenToUnitRate { get; set; }//in USDT rate 
        //---
        public string WalletAddress { get; set; }
        /// <summary>
        /// User Account Number
        /// </summary>
        public string userAccount { get; set; }
        /// <summary>
        /// Profile Id of the Address Owner
        /// </summary>
        public Guid userAccountId { get; set; }
        /// <summary>
        /// Network proxy URL, that Resulted in this Notification
        /// </summary>
        public string NetworkProxy { get; set; }
        /// <summary>
        /// Network Smart Contarct Address
        /// </summary>
        public string? TokenAddress { get; set; }
        /// <summary>
        /// Network Native Amount
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// If this is Smart Contract Amount, 
        /// </summary>
        public string? Erc20Amount { get; set; }
        /// <summary>
        /// Name of the Token
        /// </summary>
        public string BuyWith { get; set; }
        /// <summary>
        /// Flag for network Native Crypto
        /// </summary>
        public bool IsNativeFund { get; set; }
        /// <summary>
        /// Network Transaction Receipt
        /// </summary>
        public string TranHash { get; set; }
    }
}
