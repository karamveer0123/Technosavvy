using System.ComponentModel.DataAnnotations;
namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    public class eGeoInfo:BaseEntity1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GeoInfoId { get; set; }
        [StringLength(100)]
        public string? IP { get; set; }
        [StringLength(6)]
        public string? CountryCode { get; set; }
        [StringLength(100)]
        public string? City { get; set; }
        public string? Ipcontinent { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }//
    }
}
