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
    public class HomeController : Controller
    {
        private List<BorkViewModel> getBorks()
        {
            var userRepository = UserRepository.getRepository();
            var timeLineViewModel = TimeLineViewModel.getTimeLineViewModel();
            var methodBork = new BorkViewModel();
            var userGet = userRepository.List();
            var currentUser = timeLineViewModel.CurrentUser;
            var borks = new List<BorkViewModel>();
            foreach(var user in userGet)
            {
                if (timeLineViewModel.CurrentUser.FollowingId.Contains(user.UserId) || timeLineViewModel.CurrentUser.UserId == user.UserId)
                {
                    foreach (var bork in user.UserBorks)
                    {
                        borks.Add(methodBork.getView(bork));
                    }
                }
            }
            borks = borks.OrderByDescending(a => a.DateBorked).ToList();
            var currentBorks = new List<BorkViewModel>();
            for (int x = timeLineViewModel.CurrentPage * 5; x < timeLineViewModel.CurrentPage * 5 + 5; x++)
            {
                if (x > borks.Count() - 1) break;
                currentBorks.Add(borks.ElementAt(x));
            }
            var pageFinder = borks.Count() % 5;
            timeLineViewModel.TotalPages = ((borks.Count() - pageFinder) / 5);
            if (pageFinder > 0) timeLineViewModel.TotalPages++;
            return currentBorks;
        }

        public ActionResult Index()
        {
            var timeLineViewModel = TimeLineViewModel.getTimeLineViewModel();
            var borks = getBorks();
            timeLineViewModel.Borks = borks;
            return View(timeLineViewModel);
        }

        [HttpGet]
        public ActionResult NextBork()
        {
            var timeLineViewModel = TimeLineViewModel.getTimeLineViewModel();
            var borkRepository = BorkRepository.getRepository();
            timeLineViewModel.changePage(1);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult PreviousBork()
        {
            var timeLineViewModel = TimeLineViewModel.getTimeLineViewModel();
            var borkRepository = BorkRepository.getRepository();
            timeLineViewModel.changePage(0);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult NewBork(BorkViewModel bork)
        {
            var timeLineViewModel = TimeLineViewModel.getTimeLineViewModel();
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