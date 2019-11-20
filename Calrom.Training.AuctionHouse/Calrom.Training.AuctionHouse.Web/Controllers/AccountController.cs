﻿using Calrom.Training.AuctionHouse.Database;
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
            AccountViewModel accountViewModel = new AccountViewModel();
            accountViewModel.AllUserBids = new List<BidProductViewModel>();
            var userList = UserInstance.List();
            var bidList = BidInstance.List();
            var user = userList.FirstOrDefault(u => u.Username == this.HttpContext.User.Identity.Name);

            accountViewModel.UserID = user.UserID;
            accountViewModel.Username = user.Username;
            accountViewModel.DateOfBirth = user.DateOfBirth;

            if (bidList.Count > 0)
            {
                foreach (var bid in bidList)
                {
                    if (user.UserID == bid.UserID)
                    {
                        var bidProductViewModel = new BidProductViewModel
                        {
                            ItemID = bid.ItemID,
                            ItemName = bid.ItemName,
                            Amount = bid.Amount
                        };
                        accountViewModel.AllUserBids.Add(bidProductViewModel);
                    }
                }
            }
            return View(accountViewModel);
        }
    }
}