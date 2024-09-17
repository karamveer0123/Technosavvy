namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mAuth
    {
        public Guid mAuthId { get; set; }
        public string userName { get; set; }
        public Guid userAccountID { get; set; }
        public string AccountNumber { get; set; }
        public string GAuthCode { get; set; }
        public bool MultiFact_Enabled { get; set; }
        public bool IsPasswordAuth { get; set; }
        //True if User Name was Used to Authenticate
        public bool IsUserNameAuth { get; set; }
        //Authenticator is Enabled for 2nd Factor
        public bool Is2FAuthenticator { get; set; }
        //Authenticator is Enabled for 2nd Factor
        public bool Is2FEmail { get; set; }
        //Authenticator is Enabled for 2nd Factor
        public bool Is2FMobile { get; set; }
        public DateTime StartedOn { get; set; }
        public string SessionHash { get; set; }
    }

}
