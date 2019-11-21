using Calrom.Training.AuctionHouse.Database;
using Calrom.Training.AuctionHouse.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Calrom.Training.AuctionHouse.Web.Controllers
{
    public class AccountController : Controller
    {
        private static UserRepo UserInstance { get { return UserRepo.GetInstance; } }
        private static BidRepo BidInstance { get { return BidRepo.GetInstance; } }

        [Authorize]
        public ActionResult Account()
        {
            var userList = UserInstance.DBList();
            var bidList = BidInstance.DBList();
            var user = userList.FirstOrDefault(u => u.Username == this.HttpContext.User.Identity.Name);
            AccountViewModel accountViewModel = new AccountViewModel
            {
                AllUserBids = new List<BidProductViewModel>(),
                UserID = user.UserID,
                Username = user.Username,
                DateOfBirth = user.DateOfBirth
            };

            if (bidList.Count > 0)
            {
                foreach (var bid in bidList)
                {
                    if (user.UserID == bid.User.UserID)
                    {
                        var bidProductViewModel = new BidProductViewModel
                        {
                            ItemID = bid.Product.ItemID,
                            ItemName = bid.Product.ItemName,
                            Amount = bid.Product.CurrentBid
                        };
                        accountViewModel.AllUserBids.Add(bidProductViewModel);
                    }
                }
            }
            return View(accountViewModel);
        }
    }
}