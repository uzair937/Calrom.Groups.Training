using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.ShopBasket.Web.Models
{
    public class ProductDetailsViewModel
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductAvailability { get; set; }
        public DateTime ProductPostDate { get; set; }
        public int ProductID { get; set; }
        public double ProductPrice { get; set; }
    }
}