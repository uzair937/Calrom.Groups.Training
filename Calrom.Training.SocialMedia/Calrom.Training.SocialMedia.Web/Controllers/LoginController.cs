using Calrom.Training.SocialMedia.Database.Models;
using Calrom.Training.SocialMedia.Database.Repositories;
using Calrom.Training.SocialMedia.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Calrom.Training.SocialMedia.Web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            var loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel entry)
        {
            if (ModelState.IsValid)
            {
                var userId = 0;
                var userList = UserRepository.GetRepository().List();
                foreach (var user in userList)
                {
                    if (user.UserName == entry.UserName) userId = user.UserId;
                }
                bool isValidUser = !(userId == 0);
                if (!isValidUser)
                {
                    ModelState.AddModelError("LogOnError", "Incorrect Username/Password, please try again.");
                    return View(entry);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(entry.UserName, false);
                    this.HttpContext.Session.Add("UserId", userId);
                    this.HttpContext.Session.Add("CurrentPage", 0);
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("LogOnError", "Incorrect Username/Password, please try again.");
            return View(entry);
        }

        [HttpPost]
        public ActionResult Register(LoginViewModel entry)
        {
            if (ModelState.IsValid)
            {
                var alreadyExists = false;
                var userRepository = UserRepository.GetRepository();
                var userList = userRepository.List();
                if (userList.FirstOrDefault(a=>a.UserName == entry.UserName) != null) alreadyExists = true;
                if (alreadyExists)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    var userId = Enumerable.Range(1, int.MaxValue)
                                .Except(userList.Select(u => u.UserId))
                                .FirstOrDefault();
                    var newBorkList = new List<BorkDatabaseModel>();
                    var followingId = new List<int>();
                    var followerId = new List<int>();
                    var notif = new List<NotificationDatabaseModel>();
                    userRepository.Add(new UserDatabaseModel
                    {
                        UserId = userId,
                        UserBorks = newBorkList,
                        UserName = entry.UserName,
                        Password = entry.Password,
                        UserPP = "../../images/def-dog.jpg",
                        FollowerId = followerId,
                        FollowingId = followingId,
                        Notifications = notif
                    });
                    return RedirectToAction("Login");
                }
            }
            return RedirectToAction("Login");
        }
    }
}
