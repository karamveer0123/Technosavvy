namespace NavExM.Int.Maintenance.APIs.Model.AppInt
{
    public class MsgWrapper
    {
        public Guid Key { get; set; }
        public RegMsgType MsgType { get; set; }
        public string PayLoad { get; set; }
        public mHandShakePackage Package { get; set; }
        public HealthDetails HealthDetails { get; set; }
    }
}
