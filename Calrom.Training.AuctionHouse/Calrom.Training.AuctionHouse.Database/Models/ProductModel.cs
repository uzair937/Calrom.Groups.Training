using System.Collections.Generic;

namespace Calrom.Training.AuctionHouse.Database
{
    public class ProductModel
    {
        public virtual int ItemID { get; set; }
        public virtual string ItemName { get; set; }
        public virtual double ItemPrice { get; set; }
        public virtual double CurrentBid { get; set; }
        public virtual string ItemDescription { get; set; }
        public virtual string ImageSrc { get; set; }
    }
}