namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    //WalletTransaction is one of Many type of Transactions. So Audit Purposes Before and After Balance must be maintained here too
    [Index("Date")]
    [Index("TAmount")]
    [Index("FromWalletId")]
    [Index("FromWalletInternalAccountNo")]
    [Index("ToWalletId")]
    [Index("ToWalletInternalAccountNo")]
    [Index("RecordHash")]
    [Index("IsWithInMyWallet")]
    [Index("Narration")]
    [Index("TokenId")]
    public class eWalletTransaction : secBaseEntity1//ToDo: Naveen Imp=> Inherit from secBaseEntity1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid WalletTransactionId { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public double TAmount { get; set; }
        public Guid FromWalletId { get; set; }
        public Guid FromWalletInternalAccountNo { get; set; }
        public Guid ToWalletId { get; set; }//ToWalletId
        public Guid ToWalletInternalAccountNo { get; set; }
        public double FromWalletBeforeTransactionBalance { get; set; }
        public double FromWalletAfterTransactionBalance { get; set; }
        public double ToWalletBeforeTransactionBalance { get; set; }
        public double ToWalletAfterTransactionBalance { get; set; }
        //WalletTransactionId+Date+TokenId+TAmount+FromWalletId+ToWalletId+FromWalletAfterTransactionBalance+ToWalletAfterTransactionBalance+SessionHash
        public string RecordHash { get; set; }
        public bool IsWithInMyWallet { get; set; }
        [StringLength(1000)]
        public string Narration { get; set; } = string.Empty;

        [ForeignKey("Token")]
        public Guid TokenId { get; set; }
        public eToken Token { get; set; }

    }
    public class eOnDemandTxCheckRequest: secBaseEntity1//ToDo: Naveen Imp=> Inherit from secBaseEntity1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OnDemandTxCheckRequestId { get; set; }
        [StringLength(512)]//in Consideration of other networks as well
        public string txHash { get; set; }
        public string NetworkWallet { get; set; }
        public Guid NetworkId { get; set; }
    }
    }
