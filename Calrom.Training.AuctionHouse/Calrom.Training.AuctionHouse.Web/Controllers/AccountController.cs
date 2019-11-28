using Calrom.Training.AuctionHouse.Database;
using Calrom.Training.AuctionHouse.EntityMapper;
using Calrom.Training.AuctionHouse.ViewModels;
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
            var userList = UserInstance.List();
            var user = userList.FirstOrDefault(u => u.Username == this.HttpContext.User.Identity.Name);
            var bidList = BidInstance.List();
            var productList = ProductInstance.List();
            var tempProductList = new List<BidProductViewModel>();

            var accountViewModel = AutoMapperConfiguration.GetInstance<AccountViewModel>(user);
            accountViewModel.AllUserBids = new List<BidProductViewModel>();

            if (bidList == null)
            {
                return View(accountViewModel);
            }

            foreach (var bid in bidList)
            {
                if (bid.User.UserID == user.UserID)
                {
                    var product = productList.FirstOrDefault(p => p.ItemID == bid.Product.ItemID);
                    var bidProductViewModel = AutoMapperConfiguration.GetInstance<BidProductViewModel>(product);
                    
                    if (accountViewModel.AllUserBids.Count > 0)
                    {
                        foreach (var item in accountViewModel.AllUserBids)
                        {
                            if (item.ItemID != product.ItemID)
                            {
                                tempProductList.Add(bidProductViewModel);
                            }
                        }
                    } else if (accountViewModel.AllUserBids.Count == 0)
                    {
                        tempProductList.Add(bidProductViewModel);
                    }
                    accountViewModel.AllUserBids = UserBids(tempProductList);
                }
            }
            return View(accountViewModel);
        }

        private List<BidProductViewModel> UserBids(List<BidProductViewModel> list)
        {
            var userProductList = new List<BidProductViewModel>();
            foreach(var child in list)
            {
                if (userProductList.Count > 0 && !userProductList.Contains(userProductList.FirstOrDefault(p => p.ItemID == child.ItemID)))
                {
                    userProductList.Add(child);
                }
                else if (userProductList.Count == 0)
                {
                    userProductList.Add(child);
                }
            }
            return userProductList;
        }
    }
}