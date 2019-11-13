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
        private TimeLineViewModel GetBorks(int userId, int pageNum)
        {
            if (userId == 0) RedirectToAction("Logout", "Login");
            var timeLineViewModel = new TimeLineViewModel(userId);
            var borkRepository = BorkRepository.GetRepository();
            var converter = new ConverterViewModel();
            var borkGet = borkRepository.List(userId);
            var PageView = CurrentPageFinder(pageNum, borkGet.Count());

            var borks = new List<BorkViewModel>();
            if (borkGet != null) foreach (var bork in borkGet) borks.Add(converter.GetView(bork));

            borks = borks.Skip(pageNum*5).Take(5).ToList();
            timeLineViewModel.Borks = borks;
            timeLineViewModel.PageView = PageView;
            timeLineViewModel.AllBorks = GetSomeBorks(converter);
            return timeLineViewModel;
        }

        private List<BorkViewModel> GetSomeBorks(ConverterViewModel converter)
        {
            var borkRepository = BorkRepository.GetRepository();
            var borkGet = borkRepository.List();
            var someBorks = new List<BorkViewModel>();

            for (int x = 0; x < 6; x++) someBorks.Add(converter.GetView(borkGet.ElementAt(x)));

            return someBorks;
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

        private bool CheckValidUser(ConverterViewModel converter)
        {
            
            var userRepository = UserRepository.GetRepository();
            var userList = userRepository.List();
            if (converter.GetView(userList.FirstOrDefault(a => a.UserName == HttpContext.User.Identity.Name)) == null || this.HttpContext.Session["UserId"] as int? == null)
            {
                return false;
            }
            return true;
        }

        public ActionResult Index()
        {
            var converter = new ConverterViewModel();
            if (!CheckValidUser(converter)) return RedirectToAction("Logout", "Login");
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