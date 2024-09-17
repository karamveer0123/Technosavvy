namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mUserSession {
        public Guid UserSessionId { get; set; }
        public mUser UserAccount { get; set; }
        public DateTime StartedOn { get; set; } 
        public DateTime ShouldExpierOn { get; set; } 
        public DateTime? ExpieredOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string SessionHash { get; set; }
        public Guid EscrowWalletId { get; set; }
        public Guid FundingWalletId { get; set; }
        public Guid SpotWalletId { get; set; }
        public Guid HoldingWalletId { get; set; }
        public Guid EarnWalletId { get; set; }

        public mAuth SessionAuthEvent { get; set; }
    }

}
