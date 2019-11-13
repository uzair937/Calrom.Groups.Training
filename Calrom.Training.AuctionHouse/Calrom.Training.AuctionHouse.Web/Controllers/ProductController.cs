using Calrom.Training.AuctionHouse.Database;
using Calrom.Training.AuctionHouse.Web.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Calrom.Training.AuctionHouse.Web.Controllers
{
    public class ProductController : Controller
    {
        public static ProductRepo ProductInstance { get { return ProductRepo.getInstance; } }
        private static UserRepo UserInstance { get { return UserRepo.getInstance; } }
        private static BidRepo BidInstance { get { return BidRepo.getInstance; } }

        public ActionResult NewProduct()
        {
            return View();
        }

        public void Populate()
        {
            for (var i = 0; i < 30; i++)
            {
                var model = new ProductViewModel()
                {
                    ItemName = i.ToString(),
                    ItemPrice = i,
                    ItemDescription = i.ToString() + "Description",
                    ImageSrc = "ducky.jpg"
                };
                NewProduct(model, null);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult NewProduct(ProductViewModel productViewModel, HttpPostedFileBase imageFile)
        {
            var productDatabaseModel = new ProductDatabaseModel();
            if (productViewModel.ImageFile != null)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                productViewModel.ImageSrc = Path.Combine(Server.MapPath("~/Images"), fileName);
                imageFile.SaveAs(productViewModel.ImageSrc);
                productDatabaseModel.ImageSrc = Path.GetFileName(imageFile.FileName);
            }
            productDatabaseModel.ItemName = productViewModel.ItemName;
            productDatabaseModel.ItemPrice = productViewModel.ItemPrice;
            productDatabaseModel.ItemDescription = productViewModel.ItemDescription;
            ProductInstance.Add(productDatabaseModel);
            return RedirectToAction("Listings");
        }

        public ActionResult Listings()
        {
            var listingsViewModel = new ListingsViewModel()
            {
                ProductList = new List<ProductViewModel>(),
                IsAuthenticated = this.HttpContext.User.Identity.IsAuthenticated
            };
            var productList = ProductInstance.List();
            foreach (var product in productList)
            {
                var productViewModel = new ProductViewModel
                {
                    ItemID = product.ItemID,
                    ItemName = product.ItemName,
                    ItemPrice = product.ItemPrice,
                    ItemDescription = product.ItemDescription,
                    CurrentBid = product.CurrentBid
                };
                if (product.ImageSrc != null)
                {
                    productViewModel.ImageSrc = Path.Combine(@"\Images", product.ImageSrc);
                }
                listingsViewModel.ProductList.Add(productViewModel);
            }
            return View(listingsViewModel);
        }

        [HttpPost]
        public ActionResult BidProduct(BidProductViewModel viewModel)
        {
            var productList = ProductInstance.List();
            var userList = UserInstance.List();
            var bidList = BidInstance.List();
            var product = productList.FirstOrDefault(p => p.ItemID == viewModel.ItemID);
            var user = userList.FirstOrDefault(u => u.Username == this.HttpContext.User.Identity.Name);
            var bid = bidList.FirstOrDefault(b => b.ItemID == product.ItemID);

            if (product != null)
            {
                if (product.CurrentBid == 0)
                {
                    product.CurrentBid = product.ItemPrice + viewModel.Amount;
                }
                else
                {
                    product.CurrentBid += viewModel.Amount;
                }
                if (bid == null)
                {
                    var model = new BidDatabaseModel()
                    {
                        ItemID = product.ItemID,
                        ItemName = product.ItemName,
                        Amount = product.CurrentBid,
                        UserID = user.UserID
                    };
                    BidInstance.Add(model);
                }
                else
                {
                    bid.Amount = product.CurrentBid;
                    bid.UserID = user.UserID;
                }
            }   
            
            return RedirectToAction("IndividualProduct", new { itemId = product.ItemID });
        }

        //private void StoreBidItem(ProductDatabaseModel productDatabaseModel)
        //{
        //    var tempList = UserInstance.List();
        //    var bidList = BidInstance.List();
        //    foreach (var user in tempList)
        //    {
        //        if (user.Username == this.HttpContext.User.Identity.Name)
        //        {
        //            if (bidList.Count > 0)
        //            {
        //                foreach (var bid in bidList)
        //                {
        //                    if (bid.ItemID != productDatabaseModel.ItemID)
        //                    {
        //                        var model = new BidDatabaseModel()
        //                        {
        //                            ItemID = productDatabaseModel.ItemID,
        //                            ItemName = productDatabaseModel.ItemName,
        //                            Amount = productDatabaseModel.CurrentBid,
        //                            UserID = user.UserID
        //                        };
        //                        BidInstance.Add(model);
        //                    }
        //                    else
        //                    {
        //                        bid.Amount = productDatabaseModel.CurrentBid;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                var model = new BidDatabaseModel()
        //                {
        //                    ItemID = productDatabaseModel.ItemID,
        //                    ItemName = productDatabaseModel.ItemName,
        //                    Amount = productDatabaseModel.CurrentBid,
        //                    UserID = user.UserID
        //                };
        //                BidInstance.Add(model);
        //            }
        //        }
        //    }
        //}

        public ActionResult IndividualProduct(int ItemID)
        {
            var tempList = ProductInstance.List();
            var product = tempList.FirstOrDefault(p => p.ItemID == ItemID);
            if (product != null)
            {
                var viewModel = new IndividualProductViewModel
                {
                    ItemID = product.ItemID,
                    ItemName = product.ItemName,
                    ItemPrice = product.ItemPrice,
                    CurrentBid = product.CurrentBid,
                    ItemDescription = product.ItemName,
                    ImageSrc = product.ImageSrc,
                    IsAuthenticated = this.HttpContext.User.Identity.IsAuthenticated
                };
                return View(viewModel);
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}