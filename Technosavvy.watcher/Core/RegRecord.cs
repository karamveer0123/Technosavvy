using NavExM.Int.Watcher.WatchDog.Model.AppInt;

namespace NavExM.Int.Watcher.WatchDog.Core
{
    public class RegRecord
    {
        public mHandShakePackage handShakePackage { get; set; }
        public RegCompResponse Response { get; set; }
        public Security Security { get; set; }
        public string QNameRegResponse
        {
            get
            {
                if (handShakePackage is null) return string.Empty;
                return $"{handShakePackage.InstanceName}_RegRes_{handShakePackage.MacAddress.FirstOrDefault()}_{handShakePackage.ProcessId}";
            }
        }
    }
    public class Security
    {
        public bool isCompleted{ get; set; }//True if all Registration Action are done
        public DateTime CompletedOn { get; set; }
        public List<DateTime> PingedOn { get; private set; }=new List<DateTime>();
        public bool Ping()
        {
            PingedOn.Add(DateTime.UtcNow);
            PingedOn=PingedOn.OrderBy(x=>x.Ticks).ToList().Take(10).ToList();
            return true; ;
        }
    }
    public class RegCompResponse
    {
        public Guid Key { get; set; } //InstanceKey from HandShak Package
        public Guid WatcherPrivate { get; set; }
        public string ProcessId { get; set; } //Machine Process retrived from handShake package
        public string AppId { get; set; } //Instance Id allocated to the Application Instance {ComponentId + InstanceCounter}i.e. 13-1
        public string AppSeed { get; set; }//Temp,ToDo: Security ,Naveen, should be encrypted using PublicKey of Requestee

    }
}
