﻿using Calrom.Training.SocialMedia.Database.Repositories;
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
            var accountViewModel = new AccountViewModel(userId ?? 0);
            return View(accountViewModel);
        }

        [HttpPost]
        public ActionResult Account(int? userId)
        {
            var accountViewModel = new AccountViewModel(userId ?? 0);
            return View(accountViewModel);
        }
    }
}