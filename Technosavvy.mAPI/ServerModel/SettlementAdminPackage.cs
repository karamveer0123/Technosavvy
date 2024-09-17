namespace NavExM.Int.Maintenance.APIs.ServerModel
{
    public class SettlementAdminPackage
    {
        public Guid SenderInstancId { get; set; } = Guid.Empty;
        public string SenderAppId { get; set; }
        public DateTime SentAt { get; set; }
        public SettlementAdminEvent Event { get; set; }
        public string MarketCode { get; set; }
        // Switch Related
        public Guid ExistingPrimary { get; set; } = Guid.Empty;
        public Guid SuggestedPrimary { get; set; } = Guid.Empty;

    }
    public enum SettlementAdminEvent
    {
        /// <summary>
        /// Service to Send Ping if Active
        /// </summary>
        WorkingAsActive,
        /// <summary>
        /// Service to Send Ping if Passive
        /// </summary>
        WorkingAsPassive,
        /// <summary>
        /// Service to send notification when Assuming Active Role from Passive
        /// </summary>
        AssumingAsActive,
        /// <summary>
        /// When Services are asked to SwitchRole to Active
        /// </summary>
        SwitchRoleToActive,
        ShuttingDown
        /// <summary>
        /// When Client of this Exchange are ASKED to provide info About Active Service
        /// </summary>
       // RequestActiveInfo,
        /// <summary>
        /// When Client(s) of this Exchange RESPONDE to provide info About Active Service
        /// </summary>
        //ResponseActiveInfo
    }
    public enum SettlementSrvRole
    {
        Active, Passive
    }
    public class MarketInstances
    {
        public string MarketCode { get; set; }
        public SettlementSrvRole Role { get; set; }
        public string AppId { get; set; }
        public Guid InstanceId { get; set; }
        public DateTime LastPingOn { get; set; }
        public bool isActive { get; set; }
    }
   
}
