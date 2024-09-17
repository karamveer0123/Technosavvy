namespace NavExM.Int.Watcher.WatchDog.Service
{
    public static class ConfigEx
    {
        public static IConfiguration Config;
        public static void Initialize(IConfiguration configuration)
        {
            Config = configuration;
        }
    }
}
