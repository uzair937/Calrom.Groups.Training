using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Calrom.Training.ShopBasket.Web.Models
{
    public class ProductsController : Controller
    {
        public ActionResult Products()
        {         
            var productsmodel = new ProductsViewModel();
            var newList = new List<ProductDetailsViewModel>();
            newList.Add(new ProductDetailsViewModel
            {
                ProductName = "Macbook Pro",
                ProductDescription = "BRAND NEW Space Grey 13.3inch",
                ProductAvailability = "Available",
                ProductPostDate = DateTime.Now,
                ProductID = 1510992,
                ProductPrice = 1400.00     
            });
 
            return View(newList);
        }  
    }
}