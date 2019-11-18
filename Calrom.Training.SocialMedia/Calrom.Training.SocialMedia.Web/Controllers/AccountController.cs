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
        private bool CheckValidUser(UserRepository userRepository)
        {
            var userList = userRepository.List();
            if (userList.FirstOrDefault(a => a.UserName == HttpContext.User.Identity.Name) == null || this.HttpContext.Session["UserId"] as int? == null)
            {
                return false;
            }
            return true;
        }

        private List<string> GetFollowedUsers(UserRepository userRepository, int userId)
        {
            var userList = userRepository.List();
            var user = userList.First(a => a.UserId == userId);
            var followedUserNames = new List<string>();
            foreach(var otherUser in userList)
            {
                if (user.FollowingId.Contains(otherUser.UserId))
                {
                    followedUserNames.Add(otherUser.UserName);
                }
            }
            return followedUserNames;
        }

        public ActionResult Account()
        {
            var userRepository = UserRepository.GetRepository();
            if (!CheckValidUser(userRepository)) return RedirectToAction("Logout", "Login");
            var userId = this.HttpContext.Session["UserId"] as int?;
            var accountViewModel = new AccountViewModel(userId ?? 0)
            {
                FollowedUsers = GetFollowedUsers(userRepository, userId ?? 0)
            };
            return View(accountViewModel);
        }

        [HttpGet]
        public ActionResult Account(int? userId)
        {
            var userRepository = UserRepository.GetRepository();
            if (!CheckValidUser(userRepository)) return RedirectToAction("Logout", "Login");
            if (userId == null) userId = this.HttpContext.Session["UserId"] as int?;
            var accountViewModel = new AccountViewModel(userId ?? 0)
            {
                FollowedUsers = GetFollowedUsers(userRepository, userId ?? 0)
            };
            return View(accountViewModel);
        }
    }
}