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
            var userGet = userRepository.List();
            var currentUser = timeLineViewModel.CurrentUser;
            var borks = new List<BorkViewModel>();
            foreach(var user in userGet)
            {
                if (timeLineViewModel.CurrentUser.FollowingId.Contains(user.UserId) || timeLineViewModel.CurrentUser.UserId == user.UserId)
                {
                    foreach (var bork in user.UserBorks)
                    {
                        borks.Add(new BorkViewModel
                        {
                            BorkText = bork.BorkText,
                            DateBorked = bork.DateBorked
                        });
                    }
                }
            }
            borks = borks.OrderByDescending(a => a.DateBorked).ToList();
            var currentBorks = new List<BorkViewModel>();
            for (int x = timeLineViewModel.CurrentPage * 5; x < timeLineViewModel.CurrentPage * 5 + 5; x++)
            {
                if (x == borks.Count()) break;
                currentBorks.Add(borks.ElementAt(x));
            }
            return currentBorks;
        }

        private void AddModelBorkToDatabase(BorkViewModel bork)
        {
            var borkRepository = BorkRepository.getRepository();
            borkRepository.Add(new BorkDatabaseModel
            {
                BorkText = bork.BorkText,
                DateBorked = bork.DateBorked
            });
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
            var borkRepository = BorkRepository.getRepository();
            bork.DateBorked = DateTime.Now;
            AddModelBorkToDatabase(bork);
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