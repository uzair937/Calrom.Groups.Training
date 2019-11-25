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
        public static ProductRepo ProductInstance { get { return ProductRepo.GetInstance; } }

        [Authorize]
        public ActionResult Account()
        {
            var userList = UserInstance.DBList();
            var user = userList.FirstOrDefault(u => u.Username == this.HttpContext.User.Identity.Name);
            var bidList = BidInstance.DBList();
            var productList = ProductInstance.DBList();

            AccountViewModel accountViewModel = new AccountViewModel
            {
                AllUserBids = new List<BidProductViewModel>(),
                UserID = user.UserID,
                Username = user.Username,
                DateOfBirth = user.DateOfBirth
            };
            if (bidList == null)
            {
                return View(accountViewModel);
            }

            foreach (var bid in bidList)
            {
                if (bid.User.UserID == user.UserID)
                {
                    var product = productList.FirstOrDefault(p => p.ItemID == bid.Product.ItemID);
                    BidProductViewModel bidProductViewModel = new BidProductViewModel
                    {
                        ItemID = product.ItemID,
                        ItemName = product.ItemName,
                        Amount = product.CurrentBid
                    };
                    accountViewModel.AllUserBids.Add(bidProductViewModel);
                }
            }
            return View(accountViewModel);
        }
    }
}