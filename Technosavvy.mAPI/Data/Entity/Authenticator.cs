using System.ComponentModel.DataAnnotations;
namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    public class eAuthenticator : secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AuthenticatorId { get; set; }
        [StringLength(250)]
        public string Code { get; set; } //This may not be required

        //Navigation
        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }
        public eUserAccount UserAccount { get; set; }
    }
}
