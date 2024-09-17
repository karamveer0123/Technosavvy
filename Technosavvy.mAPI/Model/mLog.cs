namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mLog1 
    {
        public string Message { get; set; }
        public string Id { get;set; } 
        public String signature { get; set; } 

    }
    public class mLogT
    {
        public static mLogT GetNewLog()
        {
            return new mLogT
            {
                Counter = MsgCounter.Next,
                ReportedOn = DateTime.UtcNow
            };
        }
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
