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
        private static UserRepo UserInstance { get { return UserRepo.getInstance; } }
        private static BidRepo BidInstance { get { return BidRepo.getInstance; } }

        [Authorize]
        public ActionResult Account()
        {
            AccountViewModel accountViewModel;
            var userList = UserInstance.List();
            var bidList = BidInstance.List();
            var user = userList.FirstOrDefault(u => u.Username == this.HttpContext.User.Identity.Name);
            var bid = bidList.FirstOrDefault(b => b.UserID == user.UserID);

            if (bid != null)
            {
                accountViewModel = new AccountViewModel
                {
                    UserID = user.UserID,
                    Username = user.Username,
                    DateOfBirth = user.DateOfBirth,
                    ItemID = bid.ItemID,
                    ItemName = bid.ItemName,
                    Amount = bid.Amount
                };
                return View(accountViewModel);
            } else
            {
                accountViewModel = new AccountViewModel
                {
                    UserID = user.UserID,
                    Username = user.Username,
                    DateOfBirth = user.DateOfBirth
                };
                return View(accountViewModel);
            }
        }
    }
}