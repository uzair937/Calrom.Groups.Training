using Calrom.Training.AuctionHouse.Database;
using Calrom.Training.AuctionHouse.EntityMapper;
using Calrom.Training.AuctionHouse.ViewModels;
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
            var productModel = AutoMapperConfiguration.GetInstance<ProductModel>(productViewModel);
            if (productViewModel.ImageFile != null)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                productViewModel.ImageSrc = Path.Combine(Server.MapPath("~/Images"), fileName);
                imageFile.SaveAs(productViewModel.ImageSrc);
                productModel.ImageSrc = Path.GetFileName(imageFile.FileName);
            }
            ProductInstance.Add(productModel);
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
                var newProduct = AutoMapperConfiguration.GetInstance<ProductViewModel>(product);

                if (product.ImageSrc != null)
                {
                    newProduct.ImageSrc = Path.Combine(@"\Images", product.ImageSrc);
                }
                listingsViewModel.ProductList.Add(newProduct);
            }
            return View(listingsViewModel);
        }

        public ActionResult IndividualProduct(int ItemID)
        {
            var productList = ProductInstance.List();
            var product = productList.FirstOrDefault(p => p.ItemID == ItemID);
            if (product != null)
            {
                var individualProductViewModel = AutoMapperConfiguration.GetInstance<IndividualProductViewModel>(product);
                individualProductViewModel.IsAuthenticated = this.HttpContext.User.Identity.IsAuthenticated;
                return View(individualProductViewModel);
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}