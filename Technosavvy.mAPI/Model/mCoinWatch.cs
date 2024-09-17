namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mCoinWatch
    {
        public string CoinCode { get; set; }
        public List<string> vs_Currency{ get; set; }
        public int duration { get; set; } = 10;
    }
}
