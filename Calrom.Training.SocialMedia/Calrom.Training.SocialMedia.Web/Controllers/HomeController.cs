using Calrom.Training.SocialMedia.Web.Models;
using Calrom.Training.SocialMedia.Database.Repositories;
using Calrom.Training.SocialMedia.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;


namespace Calrom.Training.SocialMedia.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            
            filterContext.Result = RedirectToAction("Error", "Error");
        }

        private TimeLineViewModel GetBorks(int userId, int pageNum)
        {
            if (userId == 0) RedirectToAction("Logout", "Login");
            var timeLineViewModel = new TimeLineViewModel(userId);
            var borkRepository = BorkRepository.GetRepository();
            var methodBork = new BorkViewModel();
            var borkGet = borkRepository.List(userId);
            var PageView = CurrentPageFinder(pageNum, borkGet.Count());

            var borks = new List<BorkViewModel>();
            foreach (var bork in borkGet) borks.Add(methodBork.getView(bork));

            borks = borks.Skip(pageNum*5).Take(5).ToList();
            timeLineViewModel.Borks = borks;
            timeLineViewModel.PageView = PageView;
            return timeLineViewModel;
        }

        private PaginationViewModel CurrentPageFinder(int pageNum, int borkCount)
        {
            var PageView = new PaginationViewModel();

            var pageFinder = borkCount % 5;
            PageView.TotalPages = ((borkCount - pageFinder) / 5);
            if (pageFinder > 0) PageView.TotalPages++;

            PageView.CurrentPage = Math.Max(0, pageNum);
            PageView.PreviousPage = Math.Max(0, PageView.CurrentPage - 1);
            PageView.NextPage = Math.Min(PageView.TotalPages - 1, PageView.CurrentPage + 1);

            return PageView;
        }

        public ActionResult Index()
        {
            var userId = this.HttpContext.Session["UserId"] as int?;
            var timeLineViewModel = GetBorks(userId ?? 0, 0);
            return View(timeLineViewModel);
        }

        [HttpPost]
        public ActionResult Index(int pageNum)
        {
            ModelState.Clear();
            var userId = this.HttpContext.Session["UserId"] as int?;
            var timeLineViewModel = GetBorks(userId ?? 0, pageNum);
            return View(timeLineViewModel);
        }

        [HttpPost]
        public ActionResult NewBork(BorkViewModel bork)
        {
            var userId = this.HttpContext.Session["UserId"] as int?;
            var timeLineViewModel = new TimeLineViewModel(userId ?? 0);
            timeLineViewModel.AddBork(bork.BorkText);
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}