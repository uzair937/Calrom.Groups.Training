using System.Collections.Generic;

namespace Calrom.Training.AuctionHouse.ViewModels
{
    public class ListingsViewModel
    {
        public List<ProductViewModel> ProductList { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}