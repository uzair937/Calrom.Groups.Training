using Calrom.Training.AuctionHouse.Web.Models;
using Calrom.Training.AuctionHouse.Database;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System;
using System.IO;
using System.Web.Security;
using System.Linq;

namespace Calrom.Training.AuctionHouse.Web.Controllers
{
    public class HomeController : Controller
    {
        public static ProductRepo ProductInstance { get { return ProductRepo.getInstance; } }

        public ActionResult Index()
        {
            var model = new IndexViewModel();
            model.CurrentUser = this.HttpContext.User.Identity.Name;
            model.IsAuthenticated = this.HttpContext.User.Identity.IsAuthenticated;
            return View(model);
        }
    }
}