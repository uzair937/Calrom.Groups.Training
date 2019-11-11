using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.AuctionHouse.Database
{
    public class BidDatabaseModel
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public double Amount { get; set; }
        public List<BidDatabaseModel> BidList { get; set; }
    }
}