using Calrom.Training.SocialMedia.Database.ORMRepositories;
using Calrom.Training.SocialMedia.Database.ORMModels;
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
        private IRepository<UserModel> userRepository;

        private bool CheckValidUser()
        {
            var userList = userRepository.List();
            if (userList.FirstOrDefault(a => a.UserName == HttpContext.User.Identity.Name) == null || this.HttpContext.Session["UserId"] as int? == null)
            {
                return false;
            }
            return true;
        }

        private List<string> GetFollowedUsers(int userId)
        {
            var userList = userRepository.List();
            var user = userList.First(a => a.UserId == userId);
            var followedUserNames = new List<string>();
            foreach (var otherUser in userList)
            {
                if (user.Following.Select(a => a.FollowingId).Contains(otherUser.UserId))
                {
                    followedUserNames.Add(otherUser.UserName);
                }
            }
            return followedUserNames;
        }

        public ActionResult Account()
        {
            userRepository = UserRepository.GetRepository();
            if (!CheckValidUser()) return RedirectToAction("Logout", "Login");
            var userId = this.HttpContext.Session["UserId"] as int?;
            var accountViewModel = new AccountViewModel(userId ?? 0)
            {
                followViewModel = new FollowViewModel { FollowedUsers = GetFollowedUsers(userId ?? 0) }
            };
            return View(accountViewModel);
        }

        [HttpGet]
        public ActionResult Account(int? userId)
        {
            userRepository = UserRepository.GetRepository();
            if (!CheckValidUser()) return RedirectToAction("Logout", "Login");
            if (userId == null) userId = this.HttpContext.Session["UserId"] as int?;
            var accountViewModel = new AccountViewModel(userId ?? 0)
            {
                followViewModel = new FollowViewModel { FollowedUsers = GetFollowedUsers(userId ?? 0) }
            };
            return View(accountViewModel);
        }

        [HttpPost]
        public ActionResult SearchBork(string searchText, int userId)
        {
            if (searchText == "") return PartialView("_SearchBorks", new SearchViewModel());

            var userRepo = UserRepository.GetRepository();
            var converter = new ViewModelConverter();
            var searchViewModel = new SearchViewModel
            {
                BorkResults = converter.GetView(userRepo.SearchUserBorks(searchText, userId))
            };
            if (searchViewModel.BorkResults.Count > 0) searchViewModel.ValidResults = true;
            else searchViewModel.ValidResults = false;



            return PartialView("_SearchBorks", searchViewModel);
        }
    }
}