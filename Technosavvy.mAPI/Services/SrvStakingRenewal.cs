namespace NavExM.Int.Maintenance.APIs.Services
{
    internal class SrvStakingRenewal : AppConfigBase
    {
        static Dictionary<Guid, Queue<Guid>> Renewal = new Dictionary<Guid, Queue<Guid>>();
        public static Guid AddPendingRenewal(List<Guid> Ids)
        {
            var i = Guid.NewGuid();
            var mQ = new Queue<Guid>();
            Ids.ForEach(x => mQ.Enqueue(x));
            Renewal.Add(i, mQ);
            return i;
        }
        protected override async Task DoStart()
        {
            var sm = new StaffManager();
            sm.dbctx = dbctx();
            var k = Renewal.Keys.ToList().FirstOrDefault();
            if (k == Guid.Empty) return;
            var q = Renewal[k];
            int i = 0;
            while (i < 50) //50 update at a time, then let the loop through // Concurrent Paraller
            {
                if (q.Count() <= 0) break;
                var x = q.Peek();
                var res = sm.CheckAndRenewStakings(x);
                q.Dequeue();
                if (!res)
                {
                    LogError($"Failed to Renew Staking Transaction id: {x}");
                    //ToDO: Naveen, Log faliuer with WatchDog
                }
                i++;
            }
            pulse = 2000;//2 sec wait
            //await Task.CompletedTask;
        }
        
        private ApiAppContext dbctx()
        {
            var o = new DbContextOptionsBuilder<ApiAppContext>();
            o = o.UseSqlServer(ConfigExtention.Configuration.GetConnectionString("ApiDBContext"));
            return new ApiAppContext(o.Options);
        }
    }
}
