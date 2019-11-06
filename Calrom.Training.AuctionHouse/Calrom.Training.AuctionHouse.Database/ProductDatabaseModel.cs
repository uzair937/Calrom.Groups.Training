using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.AuctionHouse.Database
{
    public class ProductDatabaseModel
    {
        public List<ProductDatabaseModel> ProductList { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public double CurrentBid { get; set; }
        public string ItemDescription { get; set; }
        public string ImageSrc { get; set; }

        public ProductDatabaseModel()
        {
            ProductList = new List<ProductDatabaseModel>();
        }
    }
}