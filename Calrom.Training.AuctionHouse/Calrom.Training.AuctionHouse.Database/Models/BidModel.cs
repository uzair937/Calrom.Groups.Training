namespace Calrom.Training.AuctionHouse.Database
{
    public class BidModel
    {
        public virtual int BidID { get; set; }
        public virtual ProductModel Product { get; set; }
        public virtual UserModel User { get; set; }
    }
}