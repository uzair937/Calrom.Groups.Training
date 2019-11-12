using Calrom.Training.SocialMedia.Database.Repositories;
using Calrom.Training.SocialMedia.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Calrom.Training.SocialMedia.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private bool CheckValidUser()
        {
            var userRepository = UserRepository.GetRepository();
            var userList = userRepository.List();
            if (userList.FirstOrDefault(a => a.UserName == HttpContext.User.Identity.Name) == null || this.HttpContext.Session["UserId"] as int? == null)
            {
                return false;
            }
            return true;
        }

        public ActionResult Account()
        {
            if (!CheckValidUser()) return RedirectToAction("Logout", "Login");
            var userId = this.HttpContext.Session["UserId"] as int?;
            var timeLineViewModel = new TimeLineViewModel(userId ?? 0);
            return View(timeLineViewModel);
        }

        [HttpPost]
        public ActionResult Account(int? userId)
        {
            var timeLineViewModel = new TimeLineViewModel(userId ?? 0);
            return View(timeLineViewModel);
        }

        [HttpPost]
        public ActionResult FollowUser(int? userId)
        {
            var followThisId = userId ?? 0;
            if (followThisId == 0) return RedirectToAction("Account");
            var currentUserId = this.HttpContext.Session["UserId"] as int?;
            var userRepository = UserRepository.GetRepository();
            userRepository.FollowUser(currentUserId ?? 0, followThisId);
            return RedirectToAction("Account", "Account", userId);
        }
    }
}