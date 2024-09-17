namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    public class eMobile:secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MobileId { get; set; }
        [ForeignKey("Country")]
        public Guid CountryId { get; set; }
        public eCountry Country { get; set; }
        public string Number { get; set; }
        [ForeignKey("UserAccount")]
        public Guid UserAccountId{ get; set; }
        public eUserAccount UserAccount { get; set; }
    }
}
