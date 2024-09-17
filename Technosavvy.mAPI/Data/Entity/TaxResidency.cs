namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    public class eTaxResidency : secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TaxResidencyId { get; set; }
        public eCountry Country { get; set; }


        //Navigation 
        [ForeignKey("PreviousTaxResidency")]
        public Guid? PreviousTaxResidencyId { get; set; }
        public eTaxResidency? PreviousTaxResidency { get; set; }

        [ForeignKey("NextTaxResidency")]
        public Guid? NextTaxResidencyId { get; set; }
        public eTaxResidency? NextTaxResidency { get; set; }

        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }
        public eUserAccount UserAccount { get; set; }

    }
    public class eCitizenship : secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CitizenshipId { get; set; }
        public eCountry Country { get; set; }


        //Navigation 
        [ForeignKey("PreviousTaxResidency")]
        public Guid? PreviousCitizenshipId { get; set; }
        public eCitizenship? PreviousCitizenship { get; set; }

        [ForeignKey("NextTaxResidency")]
        public Guid? NextCitizenshipId { get; set; }
        public eCitizenship? NextCitizenship { get; set; }

        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }
        public eUserAccount UserAccount { get; set; }

    }
}
