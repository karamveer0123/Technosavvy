using Newtonsoft.Json;

namespace NavExM.Int.Maintenance.APIs.Data.Entity.Contents
{
    [Index("JDTitle")]
    [Index("Department")]
    [Index("RefNo", IsUnique = true)]
    [Index("Status")]
    public class JD : secBaseEntity4
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SerialNo { get; set; }
        [StringLength(2500)]
        public string JDTitle { get; set; }
        public string JDBody { get; set; }
        [StringLength(100)]
        public string Department { get; set; }
        [StringLength(100)]
        public string RefNo { get; set; }
        public eJDStatus Status { get; set; }
        public List<JDViewers> Viewers { get; set; }
        public DateTime? PublishedOn { get; set; }
        public DateTime? RevokedOn { get; set; }
    }
    public class JDViewers : secBaseEntity4
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [ForeignKey("JD")]
        public Guid JDId { get; set; }
        public JD JD { get; set; }
        [StringLength(50)]
        public string? IpAddress { get; set; }
        [StringLength(500)]
        public string? emailAddress { get; set; }

    }
    public enum eJDStatus
    {
        Draft = 0,
        Published = 1,
        Revoked = 2,

    }
    public class CFGeo
    {
        public string CountryCode { get; set; }
        public string City { get; set; }
        public string Ipcontinent { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string IP { get; set; }
    }
    public class FAQ : secBaseEntity4
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        //public int SerialNo { get; set; }
        public byte[] RowVersion { get; set; }
        [StringLength(2500)]
        public string QuestionText { get; set; }
        [StringLength(2500)]
        public string AnswerText { get; set; }
        [StringLength(2500)]
        public string GroupTitle { get; set; }
        public int OrderNo { get; set; }
        public Guid ContextItemId { get; set; } = Guid.NewGuid();
        public eAuthStatus Status { get; set; }
        public List<AuthAction>? myAuthAction { get; set; }

    }
    public enum eAuthStatus
    {
        Draft,
        Suggested,
        Submitted,
        UnderApproval,
        SuggestRework,
        Approved,
        Rejected,
        Cancelled
            
    }
    public class AuthAction : secBaseEntity4
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [StringLength(250)]
        public Guid ContextItem { get; set; }
        //ToDo: naveen completed this AuthAction Tracking Object for all the Entities that will need multipal User Approval
    }
}
