using NavExM.Int.Watcher.WatchDog.Data;
using NavExM.Int.Watcher.WatchDog.Data.Entity;
using NavExM.Int.Watcher.WatchDog.Extention;
using NavExM.Int.Watcher.WatchDog.Model.AppInt;

namespace NavExM.Int.Watcher.WatchDog.Manager
{
    public class WatcherManager : ManagerBase
    {
        public WatcherManager()
        {

        }
        internal Tuple<bool, string> EnableAutoRegistration(string key,int duration =5)
        {
            var r = AppConfigBase.GetRegistryConfig(key);
            var retval = r.EnableAutoRegistration(duration);
            return retval;

        }
        internal List<Tuple<bool, string>> EnableAutoRegistrationForAllRegistries(int duration=5)
        {
            var retval = new List<Tuple<bool, string>>();// Ok(true, $"Auto Component Registration Enabled for Next {duration} Minutes");
            var lst = AppConfigBase.GetAllRegistryConfigKey();
            lst.ForEach(key =>
            {
                var r= EnableAutoRegistration(key,duration);
                retval.Add(Tuple.Create(r.Item1,$"{key} : {r.Item2}"));
            });
            return retval;

        }
        internal List<string> GetRegistries()
        {
            return AppConfigBase.GetComponentRegistries();
        }
        internal List<mHandShakePackage> GetRegistrationCandidates(string RegistryName)
        {
            var lst = AppConfigBase.GetRegistrationSeekers(RegistryName);
            //ToDo: Naveen, we should do some logging here
            return lst;
        }
        internal Tuple<bool, string> RejectRegistration(string RegistryName, Guid InstanceId)
        {
            return AppConfigBase.RejectRegistration(RegistryName, InstanceId);
        }
        internal Tuple<bool, string> AcceptRegistration(string RegistryName, Guid InstanceId)
        {
            return AppConfigBase.AcceptRegistration(RegistryName, InstanceId);

        }
        internal bool GetRegistryStatus(string key)
        {
            var r = AppConfigBase.GetRegistryConfig(key);
            return r.IsAutoRegistration;
        }
        internal List<string> GetAllRegistriesAutoStatus()
        {
            var retval = new List<string>();
            var reg = GetRegistries();
            reg.ForEach(x =>
            {
                retval.Add($"{GetRegistryStatus(x)} {x}");
            });
            return retval;
        }
        internal async Task<bool> SaveLog(mLogT m, string type)
        {
            bool isSaved = false;
            //isSaved = SaveLoginDB(m, type);
            //isSaved = SaveLoginMemory(m, type);
            return isSaved;
        }

        internal List<mLogT> GetLogData()
        {
            return LogList.GetLogs();
        }

        private bool SaveLoginDB(mLogT m, string type)
        {
            Log l = m.ToEntity(type);
            //dbctx.Logs.Add(l);
            //dbctx.SaveChanges();
            return true;
        }
        private bool SaveLoginMemory(mLogT m, string type)
        {
            mLogT l = m.ToModel(type);
            LogList.AddLog(l);
            return true;
        }

        public List<mLogT> GetLogErrorList()
        {
            List<mLogT> ms = new List<mLogT>();
            //var sms = dbctx.Logs.ToList().Where(l => l.Type.Equals(eLogType.Error.ToString())).ToList();
            //sms.ForEach(l => ms.Add(l.ToModel()));
            return ms;
        }

        public List<mLogT> GetLogEventList()
        {
            List<mLogT> ms = new List<mLogT>();
            //var sms = dbctx.Logs.ToList().Where(l => l.Type.Equals(eLogType.Event.ToString())).ToList();
            //sms.ForEach(l => ms.Add(l.ToModel()));
            return ms;
        }

        public List<mLogT> GetLogList()
        {
            List<mLogT> ms = new List<mLogT>();
            //var sms = dbctx.Logs.ToList();
            //sms.ForEach(l => ms.Add(l.ToModel()));
            return ms;
        }
    }
}
