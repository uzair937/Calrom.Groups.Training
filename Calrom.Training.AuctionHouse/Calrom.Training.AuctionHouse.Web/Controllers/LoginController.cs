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
            var model = new LoginViewModel();
            model.UserList = new List<LoginViewModel>();
            model.IsAuthenticated = this.HttpContext.User.Identity.IsAuthenticated;
            return View(model);
        }

        [HttpPost]
        public ActionResult GetLogin(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                bool isValid = IsValidUser(loginViewModel);
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(loginViewModel.Username, false);
                    Session.Add("UserID", loginViewModel.UserID);
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

        public bool IsValidUser(LoginViewModel loginViewModel)
        {
            var tempList = UserInstance.List();
            if (tempList.Count > 0)
            {
                foreach (var user in tempList)
                {
                    if (user.Username == loginViewModel.Username && user.Password == loginViewModel.Password)
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
        public ActionResult NewUser(LoginViewModel loginViewModel)
        {
            var db = new UserDatabaseModel();
            db.Username = loginViewModel.Username;
            db.Password = loginViewModel.Password;
            db.DateOfBirth = loginViewModel.DateOfBirth;
            UserInstance.Add(db);
            return RedirectToAction("Login");
        }
    }
}