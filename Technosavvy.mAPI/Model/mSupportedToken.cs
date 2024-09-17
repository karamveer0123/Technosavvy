namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mSupportedToken
    {
        public Guid SupportedTokenId { get; set; }
        public string Code { get; set; }
        public string Narration { get; set; }
        public bool IsNative { get; set; }
        public string ContractAddress { get; set; }
        public mSupportedNetwork RelatedNetwork { get; set; }

    }
    public class mSupportedCountry
    {
        public Guid SupportedCountryId { get; set; }
        public DateTime SupportedSince { get; set; }
        public DateTime? SupportEndedOn { get; set; }
        public string? Notes { get; set; }
        public mCountry Country{ get; set; }
    }
}
