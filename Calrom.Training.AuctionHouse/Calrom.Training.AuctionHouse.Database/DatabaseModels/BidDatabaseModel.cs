namespace Calrom.Training.AuctionHouse.Database
{
    public class BidDatabaseModel
    {
        public int ItemID { get; set; } //make these boys virtuals
        public string ItemName { get; set; }
        public double Amount { get; set; }
        public int UserID { get; set; }
    }
}