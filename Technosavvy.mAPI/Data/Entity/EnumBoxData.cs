namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    public class eEnumBoxData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EnumBoxDataId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string EnumValue { get; set; }
        [StringLength(100)]
        public string EnumType { get; set; }
        [StringLength(10)]
        public string VersionNo { get; set; } = "0";
        public int Id { get; set; }//for sorting order
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiledOn { get; set; }
 
    }
}
