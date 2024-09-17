namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    //For Cryto holding where Third Party P2P Assurance is Required
    public class eEscrowWBalance : secBaseEntity1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EscrowWBalanceId { get; set; }

        public double Balance { get; set; }//Token Amount

        public Guid ChangeAgent { get; set; }//TransactionID that caused this change

        [ForeignKey("Token")]
        public Guid TokenId { get; set; }
        public eToken Token { get; set; }

        [ForeignKey("EscrowWallet")]
        public Guid EscrowWalletId{ get; set; }
        public eEscrowWallet EscrowWallet { get; set; }
    }
}
