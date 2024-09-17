namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    public class eEarnWBalance : secBaseEntity1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EarnWBalanceId { get; set; }

        public double ConfirmBalance { get; set; }//Balance Available in this Wallet
        public double TentativeBalance { get; set; }//It would also have Paid/Charged Transaction Amount, Under process/Initiated Transaction (-200)
        public DateTime AvailabeOn { get; set; } = DateTime.UtcNow; //If Balance is Locked, ConfirmBalance would be availale on some furture date else Current datetime.

        public Guid ChangeAgent { get; set; }//OrderID that caused this change

        [ForeignKey("Token")]
        public Guid TokenId { get; set; }
        public eToken Token { get; set; }

        [ForeignKey("EarnWallet")]
        public Guid EarnWalletId { get; set; }
        public eEarnWallet EarnWallet { get; set; }

    }
}
