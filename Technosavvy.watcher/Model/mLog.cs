namespace NavExM.Int.Watcher.WatchDog.Model
{
    public class mLog1
    {
        public string Message { get; set; }
        public string Id { get; set; }
        public DateTime ReportedOn { get; set; } = DateTime.UtcNow;
    }

    public class mLogT
    {
        public string Message { get; set; }
        public long Counter { get; set; } = MsgCounter.Next;
        public string AppId { get; set; }
        public DateTime ReportedOn { get; set; } = DateTime.UtcNow;
        public eLogType Type { get; set; }
    }
    public static class MsgCounter
    {
        static long i = 0;
        public static long Next { get { return i++; } }
    }

    public enum eLogType
    {
        Error, Event, Debug, Action, Health

    }
}
