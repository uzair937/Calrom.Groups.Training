using Calrom.Training.AuctionHouse.Database;
using Calrom.Training.AuctionHouse.Web.Models;
using log4net;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Calrom.Training.AuctionHouse.Web.Controllers
{
    public class ProductController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ProductController));
        public static ProductRepo ProductInstance { get { return ProductRepo.GetInstance; } }

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
            var productDatabaseModel = new ProductModel();
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
            Log.Debug("Creating Product.");
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

        public ActionResult IndividualProduct(int ItemID)
        {
            var productList = ProductInstance.List();
            var product = productList.FirstOrDefault(p => p.ItemID == ItemID);
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