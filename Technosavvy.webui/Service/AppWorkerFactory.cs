namespace TechnoApp.Ext.Web.UI.Service
{
    internal static class AppWorkerFactory
    {
        internal static List<SvcBase> workers = new List<SvcBase>();

        public static bool AddWorker(SvcBase config)
        {
            if (workers.Any(x => x.GetHashCode().CompareTo(config.GetHashCode()) == 0)) return false;
            workers.Add(config);
            return true;
        }
    }
}
