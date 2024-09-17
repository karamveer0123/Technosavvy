using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NavExM.Int.Watcher.WatchDog.Data.Entity
{
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid LogId { get; set; }
        public string Message { get; set; }
        //ApplicationId
        public string AppId { get; set; }
        public DateTime ReportedOn { get; set; }
        public string Type { get; set; } 
    }
}
