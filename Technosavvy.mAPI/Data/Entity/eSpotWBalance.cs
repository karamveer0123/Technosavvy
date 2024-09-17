namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    [Index("ConfirmBalance")]
    [Index("TentativeBalance")]
    [Index("TokenId")]
    [Index("SpotWalletId")]
    public class eSpotWBalance : secBaseEntity1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SpotBalanceId { get; set; }

        public double ConfirmBalance { get; set; }//Trade Order and Accepted
        public double TentativeBalance { get; set; }//Trade Ordered but may not be accepted yet

        public Guid? ChangeAgent { get; set; }//OrderID that caused this change

        [ForeignKey("Token")]
        public Guid TokenId { get; set; }
        public eToken Token { get; set; }

        [ForeignKey("SpotWallet")]
        public Guid SpotWalletId { get; set; }
        public eSpotWallet SpotWallet { get; set; }
    }

}
