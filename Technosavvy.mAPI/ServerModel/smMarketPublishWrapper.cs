namespace NavExM.Int.Maintenance.APIs.ServerModel
{
    public class smMarketPublishWrapper
    {
        public long SenderTick { get; set; }
        public MarketEvent RelatedEvent { get; set; }
        public bool IsActive { get; set; }
        public string SenderAppId { get; set; }
        public mMarket Market { get; set; }
        public string MarketCode { get; set; }
    }
    public enum MarketEvent
    {
        StartMarket,
        SuspendMarket,
        SwitchSettlementServiceInstance,
        HeartBeat

    }
}
