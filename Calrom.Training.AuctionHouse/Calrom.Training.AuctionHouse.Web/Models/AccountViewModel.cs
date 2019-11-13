using System.Collections.Generic;

namespace Calrom.Training.AuctionHouse.Web.Models
{
    public class AccountViewModel
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string DateOfBirth { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public double Amount { get; set; }
    }
}