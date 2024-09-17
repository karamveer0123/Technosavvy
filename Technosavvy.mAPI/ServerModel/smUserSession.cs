namespace NavExM.Int.Maintenance.APIs.ServerModel
{
    public class smUserSession
    {
        public Guid UserSessionId { get; set; }
        public smUserAccount UserAccount { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime ShouldExpierOn { get; set; }
        public DateTime? ExpieredOn { get; set; }// Actual Logout
        public string SessionHash { get; set; }// Session 
        public mWalletSummery FundingWBal { get; set; }
        public mWalletSummery EscroWBal { get; set; }
        public mWalletSummery SpotWBal { get; set; }
        public mWalletSummery EarnWBal { get; set; }
        public Guid AuthEventId { get; set; }
        public string RecordHash { get; set; }
        public int SessionCount { get; set; }
    }
    public class smUserAccount
    {
        public Guid UserAccountId { get; set; }
        public ulong AccountNumber { get; set; }
        public string Email { get; set; }
        public string FAccountNumber { get; set; }
        public bool IsActive { get; set; }
        public smTaxResidency TaxResidency { get; set; }
        public smCitizenshipOf CitizenshipOf { get; set; }
    }
    public class smTaxResidency
    {
        public Guid CountryId { get; set; }
        public string CountryName { get; set; }
        public string Abbrivation { get; set; }

    }
    public class smCitizenshipOf
    {
        public Guid CountryId { get; set; }
        public string CountryName { get; set; }
        public string Abbrivation { get; set; }

    }
    public class smWalletBalance
    {
        public Guid WalletId { get; set; }
        public string Name { get; set; }
        public List<smToken> Tokens { get; set; }
        //Fiat can come for funding wallet
        //Escro wallet may also required for p2p
    }
    public class smToken
    {
        public Guid TokenId { get; set; } //Token Id in Exchange
        public Guid WBalanceId { get; set; }//Balance Record of that Token
        public string Code { get; set; }
        public string Name { get; set; }
        public double ConfirmBalance { get; set; }//Trade Order and Accepted
        public double TentativeBalance { get; set; }//Trade Ordered but may not be accepted yet
        public Guid ChangeAgent { get; set; }//OrderID that caused this change
        public DateTime CreatedOn { get; set; }//It is when Srv mark this Record
    }
    public class smSessionPublishWrapper
    {
        public long SenderTick { get; set; }
        public SessionEvent RelatedEvent { get; set; }
        public string SenderAppId { get; set; }
        public smUserSession Session { get; set; }
        public string SessionHash { get; set; }
        public Guid? UserId { get; set; }//for all device logoff
    }
    public enum SessionEvent
    {
        NewFirstSession,
        NewAnotherSession,
        WalletUpdate,
        LogOff, 
        LogOffAll,
        TimeOut,
        HeartBeat
    }
 //public class smSettlementMaintDetails
 //   {
 //       public DateTime AsOf { get; set; }
 //       public string PrimarySettlementService { get; set; }
 //       public string SecodarySettlementService { get; set; }
 //       //.... and More

 //   }
   
}
