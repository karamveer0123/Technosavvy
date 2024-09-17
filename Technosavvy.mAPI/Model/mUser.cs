namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mUser
    {
        public string Id { get; set; }
        public string AccountNumber { get; set; }
        public mRefCodes RefCodes { get; set; }
        public mProfile Profile { get; set; }
        public mEmail Email { get; set; }
        public bool IsMultiFactor { get; set; }
        public mTaxResidency TaxResidency { get; set; }
        public mCitizenOf CitizensOf { get; set; }
        public mMobile Mobile { get; set; }
        public mAuthenticator Authenticator { get; set; }
        public bool IsPrimaryCompleted { get; set; }
        public bool IsActive { get; set; }
    }
    public class mUserWallet
    {
        public string Id { get; set; }
        public string AccountNumber { get; set; }
        public mRefCodes RefCodes { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public mWalletSummery SpotWallet { get; set; }
        public mWalletSummery FundingWallet { get; set; }
        public mWalletSummery HoldingWallet { get; set; }
        public mWalletSummery EarnWallet { get; set; }
        public mWalletSummery EscrowWallet { get; set; }
    }
     
    public class mRefCodes
    {
        public string? RefferedBy { get; set; }
        public string myCommunity { get; set; }
    }
    public enum UserType
    {
        User=0,Staff=1,StaffAdmin=2,MarketMaking=3
    }
}