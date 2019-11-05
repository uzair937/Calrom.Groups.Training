using Calrom.Training.AuctionHouse.Web.Models;
using Calrom.Training.AuctionHouse.Database;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Calrom.Training.AuctionHouse.Web.Controllers
{
    public class HomeController : Controller
    {
        private static ProductRepo productRepo = new ProductRepo();
        public static ProductRepo repoInstance { get { return ProductRepo.getInstance; } }

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult NewProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewProduct(ProductDatabaseModel viewModel)
        {
            productRepo.Add(viewModel);
            return RedirectToAction("Listings");
        }

        public ActionResult Listings()
        {
            var listingsViewModel = new ListingsViewModel();
            var tempList = productRepo.List();
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

        public ActionResult Login()
        {
            return View();
        }
    }
}