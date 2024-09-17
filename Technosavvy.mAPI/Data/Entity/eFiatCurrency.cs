namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    //It will be created and Discontinued
    public class eFiatCurrency : secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FiatCurrencyId { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [StringLength(10)]
        public string Code { get; set; }//INR USD AUD
        [StringLength(10)]
        public string Symbole { get; set; }//₹ $
        public string RecordHash { get; set; }
        public double Tick { get; set; }// Minimum Amount i.e 1c
        public List<eFiatProfile> Profiles { get; set; }//1-2-Many| since 1 currency may be operating in many countries so as their bank account
        //It would have Bank Accounts Associated, but my not be directly link via Navigation Properties
    }
    public class eFiatProfile : secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FiatProfileId { get; set; }
        public eCountry CountryOrigin { get; set; }
        public bool IsExchangeAllowed { get; set; }//if This Country allows crypto exchange to trade using their Fiat
        public bool IsP2PAllowed { get; set; }//if This Country allows crypto exchange to trade using their Fiat
        public string RecordHash { get; set; }
        //[ForeignKey("FiatCurrency")]
        //public Guid FiatCurrencyId { get; set; }
        public eFiatCurrency FiatCurrency { get; set; }

        public List<eBankAccount> BankAccounts { get; set; }// Bank account of Exchange in that currency 
    }
    public class eBankAccount:secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BankAccountId { get; set; }
        public Guid BankAccountWallet { get; set; } = Guid.NewGuid();// for internal Transaction Only

        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AdditionalInfo { get; set; }
        public string RecordHash { get; set; }
        public string BranchAddress { get; set; }
        public eCountry LocatedAt { get; set; }

        //[ForeignKey("FiatProfile")]
        //public Guid FiatProfileId { get; set; }
        public eFiatProfile FiatProfile { get; set; }
    }
}
