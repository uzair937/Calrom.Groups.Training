using Calrom.Training.AuctionHouse.Database;
using Calrom.Training.AuctionHouse.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Calrom.Training.AuctionHouse.Web.Controllers
{
    public class LoginController : Controller
    {
        private static UserRepo UserInstance { get { return UserRepo.GetInstance; } }

        public ActionResult Login()
        {
            var model = new LoginViewModel
            {
                IsAuthenticated = this.HttpContext.User.Identity.IsAuthenticated
            };
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
            var userList = UserInstance.DBList();
            var user = userList.FirstOrDefault(u => u.Username == loginViewModel.Username);

            if (user != null)
            {
                if (user.Username == loginViewModel.Username && user.Password == loginViewModel.Password)
                {
                    return true;
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
            var userList = UserInstance.DBList();
            var user = userList.FirstOrDefault(u => u.Username == loginViewModel.Username);
            
            if (user == null)
            {
                var db = new UserDatabaseModel()
                {
                    Username = loginViewModel.Username,
                    Password = loginViewModel.Password,
                    DateOfBirth = loginViewModel.DateOfBirth
                };
                UserInstance.Add(db);
            }
            return RedirectToAction("Login");
        }
    }
}