using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.AuctionHouse.Web.Models
{
    public class IndividualProductViewModel
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public double CurrentBid { get; set; }
        public string ItemDescription { get; set; }
        public string ImageSrc { get; set; }
    }
}