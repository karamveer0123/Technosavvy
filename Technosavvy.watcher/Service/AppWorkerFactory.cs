namespace NavExM.Int.Watcher.WatchDog.Service
{
    internal static class AppWorkerFactory
    {
        internal static List<AppConfigBase> workers = new List<AppConfigBase>();

        public static bool AddWorker(AppConfigBase config)
        {
            if (workers.Any(x => x.GetHashCode().CompareTo(config.GetHashCode()) == 0)) return false;
            workers.Add(config);
            return true;
        }
    }
}
