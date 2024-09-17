using Microsoft.AspNetCore.Identity;
using NavExM.Int.Maintenance.APIs.ServerModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NavExM.Int.Maintenance.APIs.Data.Entity;

public class eAuthorizedEmail:secBaseEntity2
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public Guid AuthorizedEmailId { get; set; }
    [StringLength(250)]
    public string Email { get; set; }
   
    [ForeignKey("PreviousAuthorizedEmail")]
    public Guid? PreviousAuthEmailId { get; set; }
    public   eAuthorizedEmail? PreviousAuthorizedEmail { get; set; }
   
    [ForeignKey("NextAuthorizedEmail")]
    public Guid? NextAuthEmailId { get; set; }
    public eAuthorizedEmail? NextAuthorizedEmail { get; set; }

    [ForeignKey("UserAccount")]
    public   Guid UserAccountId{ get; set; }
    public   eUserAccount UserAccount{ get; set; }
}

[Index("TradeId")]
[Index("GroupId")]
[Index("MarketCode")]
[Index("dateTimeUTC")]
[Index("CreatedOn")]
[Index("SellOrderID")]
[Index("BuyOrderID")]
[Index("SellInternalId")]
[Index("BuyInternalId")]
[Index("BaseTokenCodeName")]
[Index("QuoteTokenCodeName")]
[Index("SwapRate")]
[Index("SwapValue")]
[Index("CBThreashold")]
[Index("CBCommitment")]
[Index("CashBackNavCValue")]
[Index("TradePrice")]
[Index("TradeVolumn")]
[Index("TradeValue")]
[Index("IsApproved")]
[Index("ApprovedAt")]
[Index("ApprovedBy")]
[Index(nameof(BuyInternalId), nameof(SellInternalId), IsUnique = true)]
public class cTrade
{
    [Key]
    public Guid Id { get; set; }
    public Guid TradeId { get; set; }
    public Guid GroupId { get; set; }
    [StringLength(50)]
    public string MarketCode { get; set; }
    public DateTime dateTimeUTC { get; set; }
    public DateTime CreatedOn { get; set; }
    [StringLength(500)]
    public string SellOrderID { get; set; }
    [StringLength(500)]
    public string BuyOrderID { get; set; }
    public Guid SellInternalId { get; set; }
    public Guid BuyInternalId { get; set; }
    [StringLength(50)]
    public string BaseTokenCodeName { get; set; }//Added Field
    [StringLength(50)]
    public string QuoteTokenCodeName { get; set; }//-Added Field
    public double SwapRate { get; set; }//0.02%
    public double SwapValue { get; set; }//Calculation of Trade*SwapRate

    public double CBThreashold { get; set; }//0.05%
    public double CBCommitment { get; set; }//Staking Assurance
    public double CashBackNavCValue { get; set; }
    public double TradePrice { get; set; }
    public double TradeVolumn { get; set; }
    public double TradeValue { get; set; }
    public bool IsApproved { get; set; }
    public DateTime? ApprovedAt { get; set; }
    [StringLength(500)]
    public string? ApprovedBy { get; set; }

    [StringLength(50)]
    public string? WorkerLock { get; set; }
    public DateTime? LockValidTill { get; set; }
}