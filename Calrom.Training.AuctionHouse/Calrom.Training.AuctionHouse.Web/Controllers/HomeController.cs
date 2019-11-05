using Calrom.Training.AuctionHouse.Web.Models;
using Calrom.Training.AuctionHouse.Database;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Helpers;
using System;
using System.IO;

namespace Calrom.Training.AuctionHouse.Web.Controllers
{
    public class HomeController : Controller
    {
        //private static ProductRepo productRepo = new ProductRepo();
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
        public ActionResult NewProduct(ProductDatabaseModel productDatabaseModel)
        {
            WebImage webImage = WebImage.GetImageFromRequest();
            if (webImage != null)
            {
                var newFileName = Guid.NewGuid().ToString() + "_" +
                Path.GetFileName(webImage.FileName);
                productDatabaseModel.ImageSrc = @"Images\" + newFileName;
                webImage.Save(@"~\" + productDatabaseModel.ImageSrc);
            }
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
                listingsViewModel.ProductList.Add(productViewModel);
            }
            return View(listingsViewModel);
        }
    }
}