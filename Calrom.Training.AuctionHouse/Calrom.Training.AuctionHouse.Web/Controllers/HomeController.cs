using Calrom.Training.AuctionHouse.Database;
using Calrom.Training.AuctionHouse.ViewModels;
using System.Web.Mvc;

namespace Calrom.Training.AuctionHouse.Web.Controllers
{
    public class HomeController : Controller
    {
        public static ProductRepo ProductInstance { get { return ProductRepo.GetInstance; } }

        public ActionResult Index()
        {
            var model = new IndexViewModel()
            {
                CurrentUser = this.HttpContext.User.Identity.Name,
                IsAuthenticated = this.HttpContext.User.Identity.IsAuthenticated
            };
            return View(model);
        }
    }
}