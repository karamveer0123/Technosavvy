
namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    /// <summary>
    /// After Initial Email Registration, User Must validate his email with OTP. This Entity will Record the OTP Activity for the email and Its Result.
    /// </summary>
    public class eEmailValidationAction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid eEmailValidationActionId { get; set; }
        public DateTime StartedOn { get; set; } = DateTime.UtcNow;
        public DateTime ShouldExpierOn { get; set; } = DateTime.UtcNow.AddMinutes(15);
        public eGeoInfo GEOInfo { get; set; }
        [StringLength(150)]
        public string OTPHash { get; set; }
        public bool IsCompleted { get; set; }
        public eUserAccount UserAccount { get; set; }

    }

}
