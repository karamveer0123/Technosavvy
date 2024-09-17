namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    public class eUserAccountAuthenticationLog: secBaseEntity1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserAccountAuthenticationLogId { get; set; }
        public eUserAccount UserAccount { get; set; }
        public eGeoInfo GeoInfo { get; set; }
        public bool IsSuccess { get; set; }
        [StringLength(2500)]
        public string LogMsg { get; set; }//Use a Model Json to hold Evolving information
        public bool ResultedAutoLock { get; set; }//On Policy Lock...24hrs
        public DateTime? AutoLockExpierOn { get; set; }//Lock Expiery Date
        public bool ManualInterventionMandated { get; set; }//No Auto Rule, Enable by Manual Intervention
    }
}
