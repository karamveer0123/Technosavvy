namespace NavExM.Int.Maintenance.APIs.Data.Entity;

public class eSpotWallet : secBaseEntity1
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid SpotWalletId { get; set; }
    public Guid InternalAccountNumber { get; set; }

    [ForeignKey("UserAccount")]
    public Guid UserAccountId { get; set; }
    public eUserAccount UserAccount { get; set; }

    public DateTime StartedOn { get; set; } = DateTime.UtcNow;
    public DateTime? LastActedOn { get; set; }//Last Trade Time
    public List<eSpotWBalance> SpotWBalance { get; set; }

}
public class eHoldingWallet : secBaseEntity1
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid HoldingWalletId { get; set; }
    public Guid InternalAccountNumber { get; set; }

    [ForeignKey("UserAccount")]
    public Guid UserAccountId { get; set; }
    public eUserAccount UserAccount { get; set; }

    public DateTime StartedOn { get; set; } = DateTime.UtcNow;
    public DateTime? LastActedOn { get; set; }//Last Trade Time
    public List<eHoldingWBalance> HoldingWBalance { get; set; }

}
public class eHoldingWBalance : secBaseEntity1
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid HoldingWBalanceId { get; set; }

    public double Balance { get; set; }//Trade Order and Accepted
    //WalletTransaction ID that has been given to this user
    public Guid ProposedChangeAgent { get; set; }
    //Transaction ID that was generated for either returning the funds back to sender to accepting the funds into Recevier FundWallet
    public Guid FinalChangeAgent { get; set; }

    public HoldingTransStatus Status { get; set; }
    public DateTime? ResolvedOn { get; set; }

    [ForeignKey("HoldingWallet")]
    public Guid HoldingWalletId { get; set; }
    public eHoldingWallet HoldingWallet { get; set; }
}
public enum HoldingTransStatus
{
    Accepted, Rejected, NotDecided
}
