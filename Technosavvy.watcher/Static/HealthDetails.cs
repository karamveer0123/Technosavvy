namespace NavExM.Int.Watcher.WatchDog.Service
{
    public class HealthDetails
    {
        public static HealthDetails GetMyHealthDetails(Guid _key, string _AppId, string _LocalIP, string _MacAddress)
        {
            return new HealthDetails
            {
                ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id,
                MachineName = System.Diagnostics.Process.GetCurrentProcess().MachineName,
                Threads = System.Diagnostics.Process.GetCurrentProcess().Threads.Count,
                MinWorkingSet = System.Diagnostics.Process.GetCurrentProcess().MinWorkingSet.ToInt64(),
                MaxWorkingSet = System.Diagnostics.Process.GetCurrentProcess().MaxWorkingSet.ToInt64(),
                OperatingSystem = Environment.OSVersion.VersionString,
                HandleCount = System.Diagnostics.Process.GetCurrentProcess().HandleCount,
                NonpagedSystemMemorySize64 = System.Diagnostics.Process.GetCurrentProcess().NonpagedSystemMemorySize64,
                PagedMemorySize64 = System.Diagnostics.Process.GetCurrentProcess().PagedMemorySize64,
                PagedSystemMemorySize64 = System.Diagnostics.Process.GetCurrentProcess().PagedSystemMemorySize64,
                PeakPagedMemorySize64 = System.Diagnostics.Process.GetCurrentProcess().PeakPagedMemorySize64,
                PeakWorkingSet64 = System.Diagnostics.Process.GetCurrentProcess().PeakWorkingSet64,
                PeakVirtualMemorySize64 = System.Diagnostics.Process.GetCurrentProcess().PeakVirtualMemorySize64,
                PrivateMemorySize64 = System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64,
                WorkingSet64 = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64,
                PrivilegedProcessorTime = System.Diagnostics.Process.GetCurrentProcess().PrivilegedProcessorTime,
                StartTime = System.Diagnostics.Process.GetCurrentProcess().StartTime,
                TotalProcessorTime = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime,
                DomainName = Environment.UserDomainName,
                ComputerName = Environment.MachineName,
                Key = _key,
                AppId = _AppId,
                LocalIP = _LocalIP,
                MacAddress = _MacAddress,
            };
        }
        public int ProcessId { get; set; }
        public string? MachineName { get; set; }
        public int Threads { get; set; }
        public long MinWorkingSet { get; set; }
        public long MaxWorkingSet { get; set; }
        public string? OperatingSystem { get; set; }
        public int HandleCount { get; set; }
        public long NonpagedSystemMemorySize64 { get; set; }
        public long PagedMemorySize64 { get; set; }
        public long PagedSystemMemorySize64 { get; set; }
        public long PeakPagedMemorySize64 { get; set; }
        public long PeakWorkingSet64 { get; set; }
        public long PeakVirtualMemorySize64 { get; set; }
        public long PrivateMemorySize64 { get; set; }
        public long WorkingSet64 { get; set; }
        public TimeSpan PrivilegedProcessorTime { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan TotalProcessorTime { get; set; }
        public string? DomainName { get; set; }
        public string? ComputerName { get; set; }
        public Guid Key { get; set; } //instance Id
        public string? AppId { get; set; }
        public string? LocalIP { get; set; }
        public string? MacAddress { get; set; }
    }
    }
