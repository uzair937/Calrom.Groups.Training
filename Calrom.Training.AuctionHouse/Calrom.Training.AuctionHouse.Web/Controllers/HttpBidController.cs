using Calrom.Training.AuctionHouse.Database;
using Calrom.Training.AuctionHouse.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Calrom.Training.SocialMedia.Web.Controllers
{
    [Authorize]
    public class HttpBidController : ApiController
    {
        public static ProductRepo ProductInstance { get { return ProductRepo.GetInstance; } }
        private static UserRepo UserInstance { get { return UserRepo.GetInstance; } }
        private static BidRepo BidInstance { get { return BidRepo.GetInstance; } }

        [HttpPost]
        public IHttpActionResult BidItem(int ItemID, int value)
        {
            var productList = ProductInstance.List();
            var userList = UserInstance.List();
            var bidList = BidInstance.List();
            var product = productList.FirstOrDefault(p => p.ItemID == ItemID);
            var user = userList.FirstOrDefault(u => u.Username == this.RequestContext.Principal.Identity.Name);
            var bid = bidList.FirstOrDefault(b => b.Product.ItemID == product.ItemID);

            if (product != null)
            {
                if (product.CurrentBid == 0)
                {
                    product.CurrentBid = product.ItemPrice + value;
                }
                else
                {
                    product.CurrentBid += value;
                }

                var model = new BidModel()
                {
                    Product = product,
                    User = user
                };
                BidInstance.Add(model);
            }
            return Ok();
        }
    }
}