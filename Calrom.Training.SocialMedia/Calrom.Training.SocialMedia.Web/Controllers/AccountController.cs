using Calrom.Training.SocialMedia.Database.ORMRepositories;
using Calrom.Training.SocialMedia.Database.ORMModels;
using Calrom.Training.SocialMedia.ViewModels;
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
            var accountViewModel = GenerateAccountViewModel(userId ?? 0);
            accountViewModel.followViewModel = new FollowViewModel { FollowedUsers = GetFollowedUsers(userId ?? 0) };
            return View(accountViewModel);
        }

        private List<string> GetFollowedUsers(UserRepository userRepository, int userId)
        {
            var userList = userRepository.List();
            var user = userList.First(a => a.UserId == userId);
            var followedUserNames = new List<string>();
            foreach (var otherUser in userList)
            {
                foreach (var following in user.Following)
                {
                    if (following.FollowingId == otherUser.UserId)
                    {
                        followedUserNames.Add(otherUser.UserName);
                    }
                }
            }
            return followedUserNames;
        }

        private AccountViewModel GenerateAccountViewModel(int userId)
        {
            var acccountViewModel = new AccountViewModel();
            if (userId == 0) return null;
            var converter = new ViewModelConverter();
            var userRepository = UserRepository.GetRepository();
            var userList = userRepository.List();
            var viewingUser = converter.GetView(userList.First(a => a.UserName == HttpContext.User.Identity.Name));

            acccountViewModel.CurrentUser = converter.GetView(userList.First(a => a.UserId == userId));

            if (acccountViewModel.CurrentUser.UserName == viewingUser.UserName) acccountViewModel.IsCurrentUser = true;
            else acccountViewModel.IsCurrentUser = false;

            if (acccountViewModel.CurrentUser.Notifications != null) acccountViewModel.HasNotifications = true;
            else acccountViewModel.HasNotifications = false;

            if (acccountViewModel.CurrentUser.UserBorks != null) acccountViewModel.HasBorks = true;
            else acccountViewModel.HasBorks = false;

            if (viewingUser.FollowingId.Contains(userId)) acccountViewModel.FollowsUser = true;
            else acccountViewModel.FollowsUser = false;

            var followedUsers = GetFollowedUsers(userRepository, viewingUser.UserId);
            acccountViewModel.followViewModel = new FollowViewModel { FollowedUsers = followedUsers };
            return acccountViewModel;
        }

        [HttpGet]
        public ActionResult Account(int? userId)
        {
            userRepository = UserRepository.GetRepository();
            if (!CheckValidUser()) return RedirectToAction("Logout", "Login");
            if (userId == null) userId = this.HttpContext.Session["UserId"] as int?;
            var accountViewModel = GenerateAccountViewModel(userId ?? 0);
            accountViewModel.followViewModel = new FollowViewModel { FollowedUsers = GetFollowedUsers(userId ?? 0) };

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