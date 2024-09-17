namespace NavExM.Int.Watcher.WatchDog.Service
{
    public class Vote
    {
        public int Round { get; set; }
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime PingTime { get; set; } = DateTime.UtcNow;
    }
}
