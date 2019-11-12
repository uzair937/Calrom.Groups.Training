using System.Collections.Generic;

namespace Calrom.Training.AuctionHouse.Web.Models
{
    public class ListingsViewModel
    {
        public List<ProductViewModel> ProductList { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}