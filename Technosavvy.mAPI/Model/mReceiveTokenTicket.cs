namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mReceiveTokenTicket
    {
        public Guid TokenId { get; set; }
        public string TokenName { get; set; }
        public string TokenImgLocation { get; set; }
        public string NetworkName { get; set; }
        public Guid NetworkId { get; set; }
        public string fromWalletAddress { get; set; }
        public string ReceiveTokenTicketId { get; set; }
        public string TicketStatus { get; set; }
    }
}
