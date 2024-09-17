namespace NavExM.Int.Watcher.WatchDog.Service
{
    internal static class VotingManager
    {
        public static int ThisRound
        {
            get
            {
                if (Votes.Count > 0)
                    return Votes.Max(x => x.Round);
                else
                    return 0;
            }
        }
        public static int ResolvedRound { get; set; }
        public static Guid DeclaredPrimary { get; set; }
        public static Queue<Vote> Votes { get; private set; } = new Queue<Vote>();
        public static Guid LastPrimary { get; set; }
        public static bool AddVote(Vote v)
        {
            lock (Votes)
            {
                if (!Votes.Contains(v))
                {
                    Votes.Enqueue(v);
                    DoSizinging();
                    EstablishWinner();
                    return true;
                }
                else return false;
            }
        }
        private static void DoSizinging()
        {
            // Last 100 Rounds of Mutual Votes will be saved
            var limit = ThisRound - 100;
            var lst = Votes.Where(x => x.Round < limit).ToList().Count;
            int i = 0;
            while (i < lst && Votes.Count > 100)
            {
                Votes.Dequeue();
                i++;
            }
        }
        private static void EstablishWinner()
        {
            //we will always decide previous round of winner
            var lst = GetQualifiedVotes();
            if (lst.Count <= 0) return;
            // Instance started Earliest
            var min = lst.Min(x => x.StartTime);
            DeclaredPrimary = lst.First(x => x.StartTime == min).Id;

            if (ResolvedRound < (ThisRound - 1))
            {
                EstablishIfPrimary();
                EstablishIfDbLogger();
                EstablishIfReporter();
                ResolvedRound = ThisRound - 1;
            }
        }
        private static void EstablishIfPrimary()
        {
            if (AppConfigBase.WDCofig.MyId == DeclaredPrimary)
            {
                AppConfigBase.WDCofig.SetPrimary();
                AppConfigBase.LogDebug($"I am Primary {AppConfigBase.WDCofig.MyId}");
            }
            else
                AppConfigBase.LogDebug($"Some One is Primary {DeclaredPrimary}");
        }
        private static void EstablishIfDbLogger()
        {
            //if more than 1 Instance, then Last second is DBLogger
            Vote v = null;
            var lst = GetQualifiedVotes();// Votes.Where(x => x.Round == ThisRound - 1).ToList();
            if (lst.Count >= 3)
            {//2
                v = lst.OrderByDescending(x => x.StartTime).Skip(1).FirstOrDefault();
            }
            else if (lst.Count >= 1)
            {//2 or 1
                v = lst.OrderByDescending(x => x.StartTime).FirstOrDefault();
            }
            if (v is null) return;
            if (v.Id == AppConfigBase.WDCofig.MyId)
            {
                AppConfigBase.WDCofig.SetDBLogger();
                AppConfigBase.LogDebug($"I am DBLogger {AppConfigBase.WDCofig.MyId}");
            }
            else
                AppConfigBase.LogDebug($"Some One is DBLogger {v.Id}");
        }
        private static void EstablishIfReporter()
        {
            var lst = GetQualifiedVotes();// Votes.Where(x => x.Round == ThisRound - 1).ToList();
            var v = lst.OrderBy(x => x.StartTime).FirstOrDefault();

            if (v is null) return;
            if (v.Id == AppConfigBase.WDCofig.MyId)
            {
                AppConfigBase.WDCofig.SetReport();
                AppConfigBase.LogDebug($"I am Reported {AppConfigBase.WDCofig.MyId}");
            }
        }
        private static List<Vote> GetQualifiedVotes()
        {
            var ret = new List<Vote>();
            var lst = Votes.GroupBy(x => x.Id, (key, g) => g.OrderByDescending(s => s.PingTime).First()).ToList();
            if (lst.Count <= 0) return lst;
            var MaxPing = lst.Max(x => x.PingTime);
            var r = MaxPing.AddSeconds(-30);//Last 30 Seconds Ping, Server is still Alive
            lst = lst.Where(x => x.PingTime >= r).ToList();
            lst.ForEach(x =>
            {
                AppConfigBase.LogDebug($"Deciding Votes are -> Id:{x.Id} Start:{x.StartTime} Ping:{x.PingTime} ");
            });
            return lst;
        }

    }
}
