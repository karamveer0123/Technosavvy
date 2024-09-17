namespace NavExM.Int.Maintenance.APIs.Data.Entity
{
    /// <summary>
    /// This Class Object will be used for Trading in NavExM. It is backed by Real world 'eSupportedToken' Object
    /// </summary>
    [Index(nameof(Code), IsUnique = true)]
    public class eToken : secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TokenId { get; set; }
        [StringLength(100)]
        public string ShortName { get; set; }
        [StringLength(100)]
        public string FullName { get; set; }
        [StringLength(25)]
        public string Code { get; set; }
        public string? Category { get; set; }
        public string Details { get; set; }
        public bool IsFiatRepresentative { get; set; }
        public int WatchList { get; set; }
        public int FavList { get; set; }
        public string? WebURL { get; set; }
        public string? WhitePaper { get; set; }
        //We may not use these Numbers in case of NavExM only information is to displayed
        public double MarketCap { get; set; }
        public double FDMarketCap { get; set; }
        public double CirculatingSupply { get; set; }
        public double Volumn { get; set; }
        public List<eSupportedToken> SupportedCoin { get; set; }//ToDo: Naveen Imp, It should be a collection since 1 token can be deployed on many network
        /* Defining Token's Minimum 
         *  -Tick (Minimum Fraction of Token that can be increased/decreased in Trade volumn/Price Denominator
         *  -Movement
         *  -Price Movement
         *  -Order Size
         *  will ensure that all Amount and Quantity Movement are within expected parameter
         */
        public double Tick { get; set; }
        public string RecordHash { get; set; }
        // public double MinOrderSize { get; set; }//i.e. $10
        /// <summary>
        /// It should be the percentage of the Liquid Available in the market
        /// 0.1% by default
        /// </summary>
        //public double MaxMarketOrderAmount { get; set; }

        //This Token would be allowed to user who are resident of these countries
        public List<eSupportedCountry>? AllowedCountries { get; set; }
    }
    public class eMarket : secBaseEntity2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MarketId { get; set; }
        public eeMarketType MarketType { get; set; }
        //Base.CodeName/Quote.CodeName
        public string ShortName { get; set; }
        //Base.CodeNameQuote.CodeName
        public string CodeName { get; set; }
        public bool isPrivate  { get; set; }
        public bool IsCommunity { get; set; }
        public bool IsMarketMakingAccount { get; set; }

        public double MinOrderSizeValueUSD { get; set; }//for this market vs Global Variable
        public double MinBaseOrderTick { get; set; }//for this market vs Global Variable
        public double MinQuoteOrderTick { get; set; }//for this market vs Global Variable
        // public double? MaxOrderSize { get; set; }
        // It should be some Rules, Dynamically calculated..
        [ForeignKey("BaseToken")]
        public Guid? BaseTokenId { get; set; }
        public eToken? BaseToken { get; set; }

        [ForeignKey("QuoteToken")]
        public Guid? QuoteTokenId { get; set; }
        public eToken? QuoteToken { get; set; }


        [ForeignKey("QuoteCurrency")]
        public Guid? QuoteCurrencyId { get; set; }
        public eFiatCurrency? QuoteCurrency { get; set; }

        public bool IsTradingAllowed { get; set; }//Suspend Trading with false
        public bool IsApproved { get; set; }
        public string? ApprovedBy { get; set; }//sessionHash
        public DateTime? ApprovedOn { get; set; }

        //[ForeignKey("MarketProfile")]
        //public Guid MarketProfileId { get; set; }
        public List<eMarketProfile> MarketProfile { get; set; }

        public eMarketAttributes Attributes { get; set; }
        public string RecordHash { get; set; }

    }
    public class eMarketAttributes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MarketAttributesId { get; set; }
        public DateTime HighlightFrom { get; set; } = DateTime.MinValue;
        public DateTime HighlightTill { get; set; } = DateTime.MinValue;
        public DateTime NewListingFrom { get; set; } = DateTime.MinValue;
        public DateTime NewListingTill { get; set; } = DateTime.MinValue;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

    }
    public class eMarketProfile : secBaseEntity1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MarketProfileId { get; set; }
        //India, US, Australia
        public List<eMarketProfileScope> ProfileFor { get; set; }

        //Trading Fee for Quote Token since, Exchange can decide to make this token trading free
        public Guid? QuoteTokenMakerFeeId { get; set; }

        //Trading Fee for Quote Token since, Exchange can decide to make this token trading free
        public Guid? QuoteTokenTakerFeeId { get; set; }


        //Trading Fee for Base Token
        public Guid? BaseTokenMakerFeeId { get; set; }

        // [ForeignKey("BaseTokenTakerFee")]
        public Guid? BaseTokenTakerFeeId { get; set; }
        // public eTradingFee BaseTokenTakerFee { get; set; }//Trading Fee for Base Token

        // [ForeignKey("QuoteTokenFeeTax")]
        public Guid? QuoteTokenFeeTaxId { get; set; }//tax on Trading Fee Charged if Any
                                                     // public eTax QuoteTokenFeeTax { get; set; }//tax on Trading Fee Charged if Any

        // [ForeignKey("BaseTokenFeeTax")]
        public Guid? BaseTokenFeeTaxId { get; set; }//tax on Trading Fee Charged if Any
                                                    //public eTax BaseTokenFeeTax { get; set; }//tax on Trading Fee Charged if Any

        // [ForeignKey("QuoteTokenTradeingTax")]
        public Guid? QuoteTokenTradeingTaxId { get; set; }//Tax if Applicable for Trading on this Country Profile
                                                          // public eTax QuoteTokenTradeingTax { get; set; }//Tax if Applicable for Trading on this Country Profile

        // [ForeignKey("BaseTokenTradeingTax")]
        public Guid? BaseTokenTradeingTaxId { get; set; }//Tax if Applicable for Trading on this Country Profile
                                                         // public eTax BaseTokenTradeingTax { get; set; }//Tax if Applicable for Trading on this Country Profile
        [StringLength(10000)]

        [ForeignKey("Market")]
        public Guid? MarketId { get; set; }
        public eMarket Market { get; set; }

        public string TechConfig { get; set; }
        /*
         * Settlement Service Instance
         *  -Location
         *  -Name
         *  -Exclusivity /shared with other settlement Service Operation
         *  -InstanceKey
         */
    }
    public class eTradingFee : secBaseEntity1 //Staff User knowledge Only for UI, Implementation will be hard coded
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TokenFeeId { get; set; }
        /// <summary>
        /// Community Fee is for Community Members in Community Markets
        /// </summary>
        public double FeeCommunity { get; set; } //0.0002 would be 0.02%
        /// <summary>
        /// Non-Community Fee is for Non-Community Members in Community Markets
        /// </summary>
        public double FeeNonCommunity { get; set; } //0.0002 would be 0.02%
        /// <summary>
        /// Exempt Fee is for Fee Exempt Members Account for Community/Non-Community Markets
        /// </summary>
        public double FeeExempt { get; set; } //0.0002 would be 0.02%
        /// <summary>
        /// Certain Markets will be charged a Fee, regardless of Member's Community/Non-Community Status. This fee would be applied on such markets.
        /// </summary>
        public double FeeIndependent { get; set; } //0.0002 would be 0.02%
        [StringLength(500)]
        public string FeeName { get; set; } //Trading fee
        [StringLength(5000)]
        public string Details { get; set; } //Some inside on this fee
        /// <summary>
        /// if True, No fee name will be displayed, Cashback will be applied on the Trade
        /// </summary>
        public bool DisplayAsSwap { get; set; }
        public FeeType FeeType { get; set; }// This process will apply for fee charging
    }
    public class eTax : secBaseEntity2 //Staff User knowledge Only for UI, Implementation will be hard coded
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TaxId { get; set; }
        public double Rate { get; set; } //0.01 would be 10%
        [StringLength(5000)]
        public string TaxName { get; set; } //GST
        [StringLength(5000)]
        public string Details { get; set; } //Some inside on this fee
        public bool isInclusive { get; set; }//if True, Amount Charged/Traded would include Tax Component else it would be applied on Top
    }
    public class eTokenNetworkFee : secBaseEntity4
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TokenNetworkFeeId { get; set; }

        [ForeignKey(nameof(Token))]
        public Guid TokenId { get; set; }
        public eToken Token { get; set; }

        [ForeignKey(nameof(SupportedNetwork))]
        public Guid SupportedNetworkId { get; set; }
        public eSupportedNetwork SupportedNetwork { get; set; }

        public double MaxWithdrawal { get; set; }
        public double MinWithdrawal { get; set; }
        public double DepositFee { get; set; }
        public double WithdrawalFee { get; set; }
        public bool IsDepositAllowed { get; set; }
        public bool IsWithdrawalAllowed { get; set; }
    }
    public enum FeeType
    {//Hardcore implementation
        /// <summary>
        /// Standard Community Market, Member status will govern the fee charged in such markets.
        /// </summary>
        Standard,
        /// <summary>
        /// If So, Market will be exempt from Fee, Exempty Fee will be changed from all
        /// </summary>
        Exempt,
        /// <summary>
        ///  If So, Market will be Independent, Independent Fee will be applied, to all Trades Regardless of Member Status
        /// </summary>
        Independent
    }
}
