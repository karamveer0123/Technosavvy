namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    public class eSecurePassword:secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PasswordId { get; set; }
        public string Password { get; set; }


    }
}
