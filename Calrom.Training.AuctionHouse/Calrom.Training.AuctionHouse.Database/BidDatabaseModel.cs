namespace Calrom.Training.AuctionHouse.Database
{
    public class BidDatabaseModel
    {
        public virtual int ItemID { get; set; } //make these boys virtuals
        public virtual string ItemName { get; set; }
        public virtual double Amount { get; set; }
        public virtual int UserID { get; set; }
    }
}