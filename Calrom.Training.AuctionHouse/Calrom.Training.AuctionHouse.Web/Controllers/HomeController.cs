using Calrom.Training.AuctionHouse.Web.Models;
using Calrom.Training.AuctionHouse.Database;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System;
using System.IO;

namespace Calrom.Training.AuctionHouse.Web.Controllers
{
    public class HomeController : Controller
    {
        public static ProductRepo ProductInstance { get { return ProductRepo.getInstance; } }
        private static UserRepo UserInstance { get { return UserRepo.getInstance; } }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewUser(UserDatabaseModel userDatabaseModel)
        {
            UserInstance.Add(userDatabaseModel);
            return RedirectToAction("Login");
        }

        public ActionResult NewUser()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetLogin(UserDatabaseModel userDatabaseModel)
        {
            var tempList = UserInstance.List();
            foreach (var user in tempList)
            {
                if (user.Username == userDatabaseModel.Username && user.Password == userDatabaseModel.Password)
                {
                    return RedirectToAction("Listings");
                }
            }
            return RedirectToAction("Login");
        }

        public ActionResult NewProduct()
        {
            return View();
        }

        [HttpPost]
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
            var tempList = ProductInstance.List();
            foreach (var product in tempList)
            {
                var productViewModel = new ProductViewModel();
                productViewModel.ItemID = product.ItemID;
                productViewModel.ItemName = product.ItemName;
                productViewModel.ItemPrice = product.ItemPrice;
                productViewModel.ItemDescription = product.ItemDescription;
                productViewModel.CurrentBid = product.CurrentBid;
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
            foreach (var product in tempList)
            {
                if (product.ItemID == viewModel.ItemID)
                {
                    if (product.CurrentBid == 0)
                    {
                        product.CurrentBid = product.ItemPrice + viewModel.Amount;
                    } else
                    {
                        product.CurrentBid += viewModel.Amount;
                    }
                }
            }
            return RedirectToAction("Listings");
        }
    }
}