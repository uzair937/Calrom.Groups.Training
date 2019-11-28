using System.Collections.Generic;

namespace Calrom.Training.AuctionHouse.ViewModels
{
    public class AccountViewModel
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string DateOfBirth { get; set; }
        public List<BidProductViewModel> AllUserBids { get; set; }
    }
}