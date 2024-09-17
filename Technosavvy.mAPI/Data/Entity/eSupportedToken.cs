namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    /// <summary>
    /// Real world 'Token' Details to Map Exchange Trade with Real world.
    /// </summary>
    public class eSupportedToken : secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SupportedTokenId { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        public string Narration { get; set; }
        public bool IsNative { get; set; }
        [StringLength(1000)]
        public string? ContractAddress { get; set; }
        [StringLength(1000)]
        public string RecordHash { get; set; }//{SupportedTokenId+Code+IsNative+ContractAddress+PrimaryNetworkId+SessionHash}

        [ForeignKey("RelatedNetwork")]
        public Guid RelatedNetworkId { get; set; }
        public eSupportedNetwork RelatedNetwork { get; set; }

    }
  
}
