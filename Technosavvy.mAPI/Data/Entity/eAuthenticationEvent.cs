
namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    /// <summary>
    /// User Login will be a Result of Authentication. This Entity will Record the Authentication Process and Its Result.
    /// </summary>
    public class eAuthenticationEvent : secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AuthenticationEventId { get; set; }
        public eUserAccount UserAccount { get; set; }
        public eGeoInfo GeoInfo { get; set; }
        //True if Password was provided
        public bool IsPasswordAuth { get; set; }
        //True if User Name was Used to Authenticate
        public bool IsUserNameAuth { get; set; }
        public bool IsMultiFactor { get; set; }
        //Authenticator is Enabled for 2nd Factor
        public bool Is2FAuthenticator { get; set; }
        //Authenticator is Enabled for 2nd Factor
        public bool Is2FEmail { get; set; }
        //Authenticator is Enabled for 2nd Factor
        public bool Is2FMobile { get; set; }

    }

}
