namespace NavExM.Int.Maintenance.APIs.Data.Entity
{

    public class eEscrowWallet : secBaseEntity1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EscrowWalletId { get; set; }

        public Guid InternalAccountNumber { get; set; }


        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }
        public eUserAccount UserAccount { get; set; }
        public DateTime StartedOn { get; set; } = DateTime.UtcNow;
        public DateTime? LastActedOn { get; set; }//Last Trade Time
        public List<eEscrowWBalance> EscrowWBalance { get; set; }
     
    }
}
