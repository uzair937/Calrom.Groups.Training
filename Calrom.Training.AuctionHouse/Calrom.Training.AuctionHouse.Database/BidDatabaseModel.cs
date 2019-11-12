namespace Calrom.Training.AuctionHouse.Database
{
    public class BidDatabaseModel
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public double Amount { get; set; }
        public int UserID { get; set; }
    }
}