namespace NavExM.Int.Maintenance.APIs.Extension
{
    internal static class Signer
    {
        internal static Guid InstanceKey { get; set; } = Guid.NewGuid();
        internal static string PublicKey { get; set; } = "ABCStillPending";
        internal static string PrivateKey { get; set; } = "YouShouldNotSeeMePending";
        internal static string AppSeed { get; set; } = "AppRegistrationSeed";
        internal static bool RegisterInstance()
        {
            //PublicKey ="s"
            //PrivateKey 

            //ToDo: Naveen, Register this App Instanc with Watcher
            return true;
        }
    }
}
