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
    public class LoginController : Controller
    {
        private static UserRepo UserInstance { get { return UserRepo.getInstance; } }

        public ActionResult Login()
        {
            var model = new UserDatabaseModel();
            //var model = new LoginViewModel();
            //model.UserList = new List<LoginViewModel>();
            //model.IsAuthenticated = this.HttpContext.User.Identity.IsAuthenticated;
            return View(model);
        }

        [HttpGet]
        public ActionResult GetLogin(UserDatabaseModel userDatabaseModel)
        {
            if (ModelState.IsValid)
            {
                bool isValid = IsValidUser(userDatabaseModel);
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(userDatabaseModel.Username, false);
                    Session.Add("UserID", userDatabaseModel.UserID);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Invalid", "Invalid username or password.");
                    return RedirectToAction("Login");
                }
            }
            return RedirectToAction("Login");
        }

        public bool IsValidUser(UserDatabaseModel userDatabaseModel)
        {
            var TempList = UserInstance.List();
            if (TempList.Count > 0)
            {
                foreach (var user in TempList)
                {
                    if (user.Username == userDatabaseModel.Username && user.Password == userDatabaseModel.Password)
                    {
                        return true;
                    } else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        public ActionResult NewUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewUser(UserDatabaseModel userDatabaseModel)
        {
            UserInstance.Add(userDatabaseModel);
            return RedirectToAction("Login");
        }
    }
}