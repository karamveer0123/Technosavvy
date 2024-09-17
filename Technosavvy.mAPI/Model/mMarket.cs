namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mPageEventRecord
    {
        public Guid RecordId { get; set; } 
        public string LTUID { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string PageInstanceId { get; set; }
        public double Scroll { get; set; }
        public double ScreenHeight { get; set; }
        public string? IP { get; set; }
        public string? Page { get; set; }
        public string? Event { get; set; }
        public DateTime? At { get; set; }
    }
    public class mMarket
    {
        public Guid MarketId { get; set; }
        public eeMarketType MarketType { get; set; }
        //Base.CodeName/Quote.CodeName
        public string ShortName { get; set; }
        //Base.CodeNameQuote.CodeName
        public string CodeName { get; set; }
        public bool isPrivate { get; set; }
        public bool IsCommunity { get; set; }

        public double MinOrderSizeValueUSD { get; set; }
        public double MinBaseOrderTick { get; set; }
        public double MinQuoteOrderTick { get; set; }
        public Guid? BaseTokenId { get; set; }
        public mToken? BaseToken { get; set; }
        public Guid? QuoteTokenId { get; set; }
        public mToken? QuoteToken { get; set; }
        //public mFiatCurrency BaseCurrency { get; set; }
        public Guid? QuoteCurrencyId { get; set; }
        public mFiatCurrency? QuoteCurrency { get; set; }
        public bool IsTradingAllowed { get; set; }//Suspend Trading with false
        public List<mMarketProfile> MarketProfile { get; set; }
        public mMarketAttributes Attributes { get; set; }
        public bool IsMarketMakingAccount { get; set; }

    }
    public class mMarketAttributes
    {
        public Guid MarketAttributesId { get; set; }
        public DateTime HighlightFrom { get; set; } = DateTime.MinValue;
        public DateTime HighlightTill { get; set; } = DateTime.MinValue;
        public DateTime NewListingFrom { get; set; } = DateTime.MinValue;
        public DateTime NewListingTill { get; set; } = DateTime.MinValue;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

    }
    public class mMarketProfile
    {
        public Guid MarketProfileId { get; set; }
        public List<mCountry> ProfileFor { get; set; }//India, US, Australia
        public Guid QuoteTokenMakerFeeId { get; set; }
        public mTradingFee? _QuoteTokenMakerFee { get; set; }
        public Guid QuoteTokenTakerFeeId { get; set; }
        public mTradingFee? _QuoteTokenTakerFee { get; set; }
        public Guid BaseTokenMakerFeeId { get; set; }//Trading Fee for Base Token
        public mTradingFee? _BaseTokenMakerFee { get; set; }
        public Guid BaseTokenTakerFeeId { get; set; }//Trading Fee for Base Token
        public mTradingFee? _BaseTokenTakerFee { get; set; }
        public Guid? QuoteTokenFeeTaxId { get; set; }//tax on Trading Fee Charged if Any
        public mTax? _QuoteTokenFeeTax { get; set; }//tax on Trading Fee Charged if Any
        public Guid? BaseTokenFeeTaxId { get; set; }//tax on Trading Fee Charged if Any
        public mTax? _BaseTokenFeeTax { get; set; }
        public Guid? QuoteTokenTradeingTaxId { get; set; }//Tax if Applicable for Trading on this Country Profile
        public mTax? _QuoteTokenTradeingTax { get; set; }//Tax if Applicable for Trading on this Country Profile
        public Guid? BaseTokenTradeingTaxId { get; set; }//Tax if Applicable for Trading on this Country Profile
        public mTax? _BaseTokenTradeingTax { get; set; }//Tax if Applicable for Trading on this Country Profile
        public string TechConfig { get; set; }
        /*
         * Settlement Service Instance
         *  -Location
         *  -Name
         *  -Exclusivity /shared with other settlement Service Operation
         *  -InstanceKey
         */
    }
    public class mTradingFee
    {//Master Data to be used by all Markets
        public Guid TokenFeeId { get; set; }
        public double FeeCommunity { get; set; } //0.0002 would be 0.02%
        public double FeeNonCommunity { get; set; } //0.0002 would be 0.02%
        public double FeeExempt { get; set; } //0.0002 would be 0.02%
        public double FeeIndependent { get; set; } //0.0002 would be 0.02%
        public string FeeName { get; set; } //Trading fee
        public string Details { get; set; } //Some inside on this fee
        public bool DisplayAsSwap { get; set; }//if True, No fee name will be displayed
        public FeeType FeeType { get; set; }
    }
    public enum FeeType
    {//Hardcore implementation
        Standard, Exempt, Independent
    }
    public class mTax
    {
        public Guid TaxId { get; set; }
        public double Rate { get; set; } //0.01 would be 10%
        public string TaxName { get; set; } //GST
        public string Details { get; set; } //Some inside on this fee
        public bool isInclusive { get; set; }//if True, Amount Charged/Traded would include Tax Component else it would be applied on Top
    }

    public class mMarketData
    {
        public long Tick { get; set; }//Id for the object, Begininng of Time for this Data, i.e. 1 Minute then Tick is at the start of 1 Minute
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Volumn { get; set; }
        public decimal Trades { get; set; }//No of Trades
        public decimal Cashback { get; set; }


    }
    public class mMarketDataSummary
    {
        public eeMarketDataSummaryType SummaryType { get; set; }
        public long Tick { get; set; }//begining of Datetime for this summarized data
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Volumn { get; set; }
        public decimal Trades { get; set; }//No of Trades
        public decimal Cashback { get; set; }
        public double ValueChange { get; set; }//Changes wrt IMMEDIARE SAME Period
        public double PercentageChange { get; set; }//Changes wrt IMMEDIARE SAME Period
    }
    public enum eeMarketDataSummaryType
    {
        _4Hours,
        _6Hours, _8Hours, _12Hours,_24Hours
    }
   
}
