namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    public class eAddress:secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AddressId { get; set; }
        [StringLength(50)]
        public string? UnitNO { get; set; }
        [StringLength(500)]
        public string? StreetAdd { get; set; }
        [StringLength(100)]
        public string City { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [StringLength(50)]
        public string PostCode { get; set; }

        [ForeignKey("Country")]
        public Guid CountryId { get; set; }
        public eCountry Country { get; set; }// mandatory to establish Origin

        //Navigation
        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }
        public eUserAccount UserAccount { get; set; }
    }
}
