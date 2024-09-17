namespace NavExM.Int.Maintenance.APIs.Model.AppInt
{
    public class RegCompResponse
    {
        /// <summary>
        /// Instance Key Retrived from HandShak Package
        /// </summary>
        public Guid Key { get; set; } //
        /// <summary>
        /// Private Key Given by WatchDog to this Instance.
        /// To Sign Messages sent to WatchDog.
        /// </summary>
        public Guid WatcherPrivate { get; set; }
        /// <summary>
        /// Machine Process retrived from handShake package
        /// </summary>
        public string ProcessId { get; set; } //
        /// <summary>
        /// Instance Id allocated to the Application Instance
        /// </summary>
        public string AppId { get; set; } //B
        /// <summary>
        /// Temp,ToDo: Security ,Naveen, should be encrypted using PublicKey of Requestee
        /// </summary>
        public string AppSeed { get; set; }//

    }
}
