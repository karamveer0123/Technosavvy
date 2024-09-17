using NavExM.Int.Watcher.WatchDog.Model.AppInt;

namespace NavExM.Int.Watcher.WatchDog.Core
{
    /// <summary>
    /// This Config File will hold and manipulate values in Memory For WatchDog Instance ItSelf
    /// </summary>
    public class RegConfig
    {
        public Guid MyId { get; set; } = Guid.NewGuid();
        public DateTime StartedOn { get; } = DateTime.UtcNow;
        public bool IsAutoRegistration { get { return AutoRegTill > DateTime.UtcNow; } }
        public long LastPing { get { return (DateTime.UtcNow.Ticks - PingTime.Ticks) / TimeSpan.TicksPerSecond; } }
        DateTime AutoRegTill = DateTime.MinValue;
        DateTime PingTime = DateTime.MinValue;

        public List<WatchDogInstanceRole> Roles { get; private set; } = new List<WatchDogInstanceRole>();//Primary or Secondary Instance
        public bool IsPrimary
        {
            get
            {
                if (Roles.Count <= 0) return false;
                return Roles.Any(x => x == WatchDogInstanceRole.Primary);
            }
        }
        public bool IsDBLog
        {
            get
            {
                return Roles.Any(x => x == WatchDogInstanceRole.DBLogger);
            }
        }
        public bool IsReport
        {
            get
            {
                return Roles.Any(x => x == WatchDogInstanceRole.Report);
            }
        }
        public mHandShakePackage Instance { get; internal set; }

        public void ClearRoles()
        {
            Roles.Clear();
        }
        public void SetPrimary()
        {
            if (!Roles.Any(x => x == WatchDogInstanceRole.Primary))
                Roles.Add(WatchDogInstanceRole.Primary);
        }
        public void SetDBLogger()
        {
            if (!Roles.Any(x => x == WatchDogInstanceRole.DBLogger))
                Roles.Add(WatchDogInstanceRole.DBLogger);
        }
        public void SetReport()
        {
            if (!Roles.Any(x => x == WatchDogInstanceRole.Report))
                Roles.Add(WatchDogInstanceRole.Report);
        }
        public Tuple<bool, string> EnableAutoRegistration(int duration = 5)
        {
            if (duration <= 0 || duration > 720) duration = 5;
            AutoRegTill = DateTime.UtcNow.AddMinutes(duration);//ToDo: ERROR Change 50
            return new Tuple<bool, string>(true, $"Auto Registration Enabled for Next {duration} Minutes");
        }
        public void Ping()
        {//Ping Activity With WatchDog Instances
            PingTime = DateTime.UtcNow;
        }

    }
    public enum WatchDogInstanceRole
    {
        Primary, DBLogger, Report
    }
}
