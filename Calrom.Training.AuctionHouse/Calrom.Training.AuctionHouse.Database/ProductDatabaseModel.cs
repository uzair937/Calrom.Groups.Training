using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.AuctionHouse.Database
{
    public class ProductDatabaseModel
    {
        public List<ProductDatabaseModel> ProductList { get; set; }
        private int _itemID;
        public int ItemID { 
            get => _itemID;
            set
            {
                Random random = new Random();
                _itemID = random.Next(1, 512);
            }
        }
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public string ItemDescription { get; set; }
        public string ImageSrc { get; set; }

        public ProductDatabaseModel()
        {
            ProductList = new List<ProductDatabaseModel>();
        }
    }
}