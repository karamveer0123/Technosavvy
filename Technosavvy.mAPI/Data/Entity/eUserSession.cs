
namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    /// <summary>
    /// Post Successful Authentication, a session will be created. This Entity will Record the Session
    /// </summary>
    public class eUserSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserSessionId { get; set; }
        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }
        public eUserAccount UserAccount { get; set; }
        public DateTime StartedOn { get; set; } = DateTime.UtcNow;
        public DateTime ShouldExpierOn { get; set; } = DateTime.UtcNow.AddMinutes(120);
        public DateTime? ExpieredOn { get; set; }// Actual Logout

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        [StringLength(1000)]
        public string CreatedBy { get; set; } = Signer.PublicKey;
        [StringLength(250)]
        public string SessionHash { get; set; }


        [ForeignKey("SessionAuthEvent")]
        public Guid SessionAuthEventId { get; set; }
        public eAuthenticationEvent SessionAuthEvent { get; set; }

    }

}
