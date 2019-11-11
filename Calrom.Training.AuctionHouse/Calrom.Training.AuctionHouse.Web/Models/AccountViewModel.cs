using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Calrom.Training.AuctionHouse.Database;

namespace Calrom.Training.AuctionHouse.Web.Models
{
    public class AccountViewModel
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string DateOfBirth { get; set; }
        public List<BidDatabaseModel> BidList { get; set; }
    }
}