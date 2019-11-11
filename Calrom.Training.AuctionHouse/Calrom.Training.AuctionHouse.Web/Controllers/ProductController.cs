using Calrom.Training.AuctionHouse.Web.Models;
using Calrom.Training.AuctionHouse.Database;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System;
using System.IO;
using System.Web.Security;
using System.Linq;

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
                var model = new ProductViewModel();
                model.ItemName = i.ToString();
                model.ItemPrice = i;
                model.ItemDescription = i.ToString() + "Description";
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
            var listingsViewModel = new ListingsViewModel();
            listingsViewModel.ProductList = new List<ProductViewModel>();
            listingsViewModel.IsAuthenticated = this.HttpContext.User.Identity.IsAuthenticated;
            var tempList = ProductInstance.List();
            foreach (var product in tempList)
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
            var tempList = ProductInstance.List();
            var product = tempList.FirstOrDefault(p => p.ItemID == viewModel.ItemID);
            if(product != null)
            {
                if (product.ItemID == viewModel.ItemID)
                {
                    if (product.CurrentBid == 0)
                    {
                        product.CurrentBid = product.ItemPrice + viewModel.Amount;
                        StoreBidItem(product);
                    } else
                    {
                        product.CurrentBid += viewModel.Amount;
                        StoreBidItem(product);
                    }
                }
            }
            return RedirectToAction("IndividualProduct", new { itemId = product.ItemID });
        }

        private void StoreBidItem(ProductDatabaseModel productDatabaseModel)
        {
            var tempList = UserInstance.List();
            var index = 0;
            int index2 = 0;
            foreach (var user in tempList)
            {
                if (user.Username == this.HttpContext.User.Identity.Name)
                {                    
                    if (user.BidList.Count == 0)
                    {
                        var model = new BidDatabaseModel()
                        {
                            ItemID = productDatabaseModel.ItemID,
                            ItemName = productDatabaseModel.ItemName,
                            Amount = productDatabaseModel.CurrentBid
                        };
                        BidInstance.Add(model);
                        user.BidList = BidInstance.List();
                    }
                    foreach (var bid in user.BidList)
                    {
                        if (bid.ItemID != productDatabaseModel.ItemID)
                        {
                            var model = new BidDatabaseModel()
                            {
                                ItemID = productDatabaseModel.ItemID,
                                ItemName = productDatabaseModel.ItemName,
                                Amount = productDatabaseModel.CurrentBid
                            };
                            BidInstance.Add(model);
                            BidInstance.Update(index, index2, model, user);
                            index2++;
                        }
                    }
                    
                }
                index++;
            }
        }

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
            } else
            {
                return HttpNotFound();
            }
        }
    }
}