using System.Collections.Concurrent;
namespace NavExM.Int.Watcher.WatchDog.Service
{
    public static class LogList
    {
        //ToDo: Security, Naveen, Memory Log Should have a Limited size or it will crash the application
        //We should consider saving Log In DB for Reporting purposes or Discarding them all together.
        public static ConcurrentQueue<mLogT> mLogData { get; private set; } = new ConcurrentQueue<mLogT>();

        public static bool AddLog(mLogT log)
        {
            mLogData.Enqueue(log);
            return true;
        }
        public static List<mLogT> GetLogs()
        {
            return mLogData.ToList();

        }
        
    }
    /* This is Solution Wide Log and single Record process would have many logging Points
     * we should have at least 10 Million Logs in the Que for meaning full History and Move the Rest of them in some Database for deep history
     *      -- Passive WatchDog Should do that DataLogging in DB
     *      -- Historic Data Report should come from Passive WatchDog
     *      -- IF Passive WatchDog is not available same Instance will do both.
     *      -- Active WatchDog will keep the instances count and Performance in Check
     * 
     */
}
