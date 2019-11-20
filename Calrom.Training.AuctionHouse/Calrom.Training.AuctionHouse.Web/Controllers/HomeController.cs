using Calrom.Training.AuctionHouse.Database;
using Calrom.Training.AuctionHouse.Web.Models;
using System.Web.Mvc;

namespace Calrom.Training.AuctionHouse.Web.Controllers
{
    public class HomeController : Controller
    {
        public static ProductRepo ProductInstance { get { return ProductRepo.GetInstance; } }

        public ActionResult Index()
        {
            var model = new IndexViewModel();
            model.CurrentUser = this.HttpContext.User.Identity.Name;
            model.IsAuthenticated = this.HttpContext.User.Identity.IsAuthenticated;
            return View(model);
        }
    }
}