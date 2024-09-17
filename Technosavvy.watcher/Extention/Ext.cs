using NavExM.Int.Watcher.WatchDog.Data.Entity;
using NavExM.Int.Watcher.WatchDog.Model;

namespace NavExM.Int.Watcher.WatchDog.Extention
{
    public static partial class Ext
    {
        public static Log ToEntity(this mLogT m,string en)
        {
            var e = new Log();
            if (m != null)
            {
                e.AppId = m.AppId;
                e.Message = m.Message;
                e.ReportedOn = m.ReportedOn;
                e.Type = en.ToString();
            }
            return e;
        }

        public static mLogT ToModel(this mLogT m, string en)
        {
            var e = new mLogT();
            if (m != null)
            {
                e.AppId = m.AppId;
                e.Message = m.Message;
                e.ReportedOn = m.ReportedOn;
               // e.Type = en.ToString();
            }
            return e;
        }

        public static mLogT ToModel(this Log m)
        {
            var e = new mLogT();
            if (m != null)
            {
                e.AppId = m.AppId;
                e.Message = m.Message;
                e.ReportedOn = m.ReportedOn;
               // e.Type = m.Type;
            }
            return e;
        }
    }
}
