namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mProfile {
        public Guid UserAccountId { get; set; }
        public Guid ProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public Gender gender { get; set; }
        public string Title { get; set; }

        public DateTime DateOfBirth { get; set; }
        public eeKYCStatus KYCStatus { get; set; }
        public Guid TaxResidencyId { get; set; }
        public Guid CitizenshipId { get; set; }
         public mCountry TaxResidency { get; set; }
        public mAddress Address { get; set; }
        public List<mKYCDocRecord> myDocs { get; set; }
    }
    
}
