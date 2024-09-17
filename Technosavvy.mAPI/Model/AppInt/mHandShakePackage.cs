namespace NavExM.Int.Maintenance.APIs.Model.AppInt
{
    public class mHandShakePackage
    {
        public Guid InstanceKey { get; set; } = Signer.InstanceKey;
        public string UniqueKey
        {
            get
            {
                return $"{InstanceName}{ProcessId}{InstanceKey}";
            }
            set { }
        }
        public string OS { get; set; }
        public string ComputerName { get; set; }
        public string InstanceName { get; set; }
        public DateTime StartTime { get; set; }
        public string DomainName { get; set; }
        public string ServiceAccount { get; set; }
        public string PubKey { get; set; } = "Pk Not Implemented";
        public List<string> GetLocalIPs { get; set; }
        public List<string> MacAddress { get; set; }
        public int ProcessodCount { get; set; }
        public int ProcessId { get; set; }
        public string? ProcessPath { get; set; }
        public string CurrentDirectory { get; set; }
        public int ManagedThreadId { get; set; }
        public Guid WatcherPrivate { get; set; }
    }
}
