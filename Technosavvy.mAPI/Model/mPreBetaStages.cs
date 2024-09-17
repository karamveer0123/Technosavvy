namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mPreBetaStages
    {
        public int id { get; set; }
        public string StageName { get; set; }
        public DateTime StartDate { get; set; }
        public double NavCSellPrice { get; set; }
        public double TokenCap { get; set; }
        public Nullable<DateTime> EndDate { get; set; }

    }
    //Display Multiplyer
    public class mFractionFactor : BaseEntity1
    {
        public int id { get; set; }
        public string Key { get; set; }
        public double FractionFactor { get; set; }
    }
}
