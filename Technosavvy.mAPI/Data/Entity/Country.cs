using System.ComponentModel.DataAnnotations;
namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    /// <summary>
    /// List of All The countries
    /// </summary>
    public class eCountry:BaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CountryId { get; set; }
        [StringLength(5)]
        public string DialCode { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(6)]
        public string Abbrivation { get; set; }
        [StringLength(250)]
        public string? Continent { get; set; } //Africa, America
        [StringLength(250)]
        public string? Block { get; set; }//Asia, central America..
       
    }
    public class eMarketProfileScope:secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MarketProfileScopeId { get; set; }
        public Guid CountryId { get; set; }
        public eCountry Country { get; set; }
        [ForeignKey("MarketProfile")]
        public Guid? MarketProfileId { get; set; }
        public eMarketProfile? MarketProfile { get; set; }
    }
    /// <summary>
    /// This Exchange Supported Countries. Services of the Exchange will be available in these Countries Only
    /// </summary>
    public class eSupportedCountry : BaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SupportedCountryId { get; set; }
        public DateTime SupportedSince { get; set; }
        public DateTime? SupportEndedOn { get; set; }
        [StringLength(5000)]
        public string? Notes { get; set; }


        [ForeignKey("Country")]
        public Guid CountryId { get; set; }
        public eCountry Country { get; set; }
    }
    /// <summary>
    /// This entity is about what Fiat Currency is supported in which Country
    /// </summary>
    public class eCountryFiatCurrency:BaseEntity2
    {
        public Guid CountryFiatCurrencyId { get; set; }

        public DateTime SupportedSince { get; set; }
        public DateTime SupportEndedOn { get; set; }


        [ForeignKey("SupportedCountry")]
        public Guid SupportedCountryId { get; set; }
        public eSupportedCountry SupportedCountry { get; set; }
        [ForeignKey("FiatCurrency")]
        public Guid FiatCurrencyId { get; set; }

        public eFiatCurrency FiatCurrency { get; set; }


    }
}
