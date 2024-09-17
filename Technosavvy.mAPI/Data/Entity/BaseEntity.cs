using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    /// <summary>
    /// User Specific Records that Must be Traced back to a session
    /// </summary>
    public abstract class secBaseEntity1    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public Guid? CreatedBy { get; set; }
        [StringLength(250)]
        public string? SessionHash { get; set; }
    }
    public abstract class secBaseEntity2 : secBaseEntity1
    {
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }
    }
    public abstract class secBaseEntity3 : secBaseEntity1
    {
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
    public abstract class secBaseEntity4 : secBaseEntity2
    {
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
    /// <summary>
    /// Non User and Non-Operation Specific Records. More like a System Maintenance Records
    /// </summary>
    public abstract class BaseEntity1
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public Guid? CreatedBy { get; set; }

    }
    public abstract class BaseEntity2 : BaseEntity1
    {
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }
    }
    public abstract class BaseEntity3 : BaseEntity1
    {
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
    public abstract class BaseEntity4 : BaseEntity2
    {
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
