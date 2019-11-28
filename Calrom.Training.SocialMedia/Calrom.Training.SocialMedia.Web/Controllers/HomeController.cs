﻿using Calrom.Training.SocialMedia.ViewModels;
using Calrom.Training.SocialMedia.Database.ORMRepositories;
using Calrom.Training.SocialMedia.Database.ORMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using AutoMapper;
using Calrom.Training.SocialMedia.Mapper;

namespace Calrom.Training.SocialMedia.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IRepository<UserModel> userRepository;

        private List<string> GetFollowedUsers(int userId)
        {
            userRepository = UserRepository.GetRepository();
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
        private TimeLineViewModel GenerateTimeLineViewModel(int userId)
        {
            if (userId == 0) return null;
            var timeLineViewModel = new TimeLineViewModel();
            var userRepository = UserRepository.GetRepository();
            var converter = new ViewModelConverter();
            var userList = userRepository.List();
            timeLineViewModel.CurrentUser = converter.GetView(userList.First(a => a.UserId == userId));
            timeLineViewModel.followViewModel = new FollowViewModel { FollowedUsers = GetFollowedUsers(userId) };
            return timeLineViewModel;
        }
        private TimeLineViewModel GetBorks(int userId, int pageNum)
        {
            if (userId == 0) RedirectToAction("Logout", "Login");
            var timeLineViewModel = GenerateTimeLineViewModel(userId);
            var borkRepository = BorkRepository.GetRepository();
            var converter = new ViewModelConverter();

            

            var borkGet = borkRepository.GetFollowedBorks(userId);

           

            var PageView = CurrentPageFinder(pageNum, borkGet.Count());

            var borks = new List<BorkViewModel>();
            if (borkGet != null) foreach (var bork in borkGet) borks.Add(converter.GetView(bork));

            borks = borks.Skip(pageNum*5).Take(5).ToList();
            timeLineViewModel.Borks = borks;
            timeLineViewModel.PageView = PageView;
            timeLineViewModel.AllBorks = GetSomeBorks(converter);
            return timeLineViewModel;
        }

        private List<BorkViewModel> GetSomeBorks(ViewModelConverter converter)
        {
            var borkRepository = BorkRepository.GetRepository();
            var borkGet = borkRepository.List();
            var someBorks = new List<BorkViewModel>();

            for (int x = 0; x < 6; x++) someBorks.Add(converter.GetView(borkGet.ElementAt(x)));

            return someBorks;
        }

        private PaginationViewModel CurrentPageFinder(int pageNum, int borkCount)
        {
            var PageView = new PaginationViewModel();

            var pageFinder = borkCount % 5;
            PageView.TotalPages = ((borkCount - pageFinder) / 5);
            if (pageFinder > 0) PageView.TotalPages++;
            if (PageView.TotalPages == 0) PageView.TotalPages++;

            PageView.CurrentPage = Math.Max(0, pageNum);
            PageView.PreviousPage = Math.Max(0, PageView.CurrentPage - 1);
            PageView.NextPage = Math.Min(PageView.TotalPages - 1, PageView.CurrentPage + 1);

            return PageView;
        }

        private bool CheckValidUser(ViewModelConverter converter)
        {
            userRepository = UserRepository.GetRepository();
            var userList = userRepository.List();
            if (converter.GetView(userList.FirstOrDefault(a => a.UserName == HttpContext.User.Identity.Name)) == null || this.HttpContext.Session["UserId"] as int? == null)
            {
                return false;
            }
            return true;
        }

        public ActionResult Index()
        {
            var converter = new ViewModelConverter();
            if (!CheckValidUser(converter)) return RedirectToAction("Logout", "Login");
            var userId = this.HttpContext.Session["UserId"] as int?;
            var pageNum = this.HttpContext.Session["CurrentPage"] as int?;
            var timeLineViewModel = GetBorks(userId ?? 0, pageNum ?? 0);
            return View(timeLineViewModel);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBork(string borkText)
        {
            var userRepo = UserRepository.GetRepository();
            var converter = new ViewModelConverter();
            var currentUser = userRepo.List().FirstOrDefault(u => u.UserName == this.HttpContext.User.Identity.Name);
            
            if (currentUser == null) return HttpNotFound();

            userRepo.AddBork(borkText, currentUser.UserId);
            currentUser = userRepo.FindById(currentUser.UserId);
            var borkViewModel = converter.GetView(currentUser.UserBorks.First(a => a.BorkText == borkText));

            return PartialView("_BorkBox", borkViewModel);
        }

        [HttpPost]
        public ActionResult ChangePage(int pageNum)
        {
            var userRepo = UserRepository.GetRepository();
            var currentUser = userRepo.List().FirstOrDefault(u => u.UserName == this.HttpContext.User.Identity.Name);

            this.HttpContext.Session["CurrentPage"] = pageNum;
            var timeLineViewModel = GetBorks(currentUser.UserId, pageNum);

            return PartialView("Index", timeLineViewModel);
        }

        [HttpPost]
        public ActionResult SearchBork(string searchText, string searchUser)
        {
            if (searchText == "") return PartialView("_SearchBorks", new SearchViewModel());

            var userRepo = UserRepository.GetRepository();
            var converter = new ViewModelConverter();
            var searchViewModel = new SearchViewModel();
            var userId = this.HttpContext.Session["userId"] as int?;

            var user = userRepo.List().FirstOrDefault(u => u.UserName == searchUser);
            if (user != null)
            {
                userId = user.UserId;
                searchViewModel.BorkResults = converter.GetView(userRepo.SearchUserBorks(searchText, userId ?? 0));
                if (searchViewModel.BorkResults.Count > 0) searchViewModel.ValidResults = true;
                else searchViewModel.ValidResults = false;
                searchViewModel.BorkResults = searchViewModel.BorkResults.OrderByDescending(a => a.DateBorked).ToList();
                return PartialView("_SearchBorks", searchViewModel);
            }
            else
            {
                userId = this.HttpContext.Session["userId"] as int?;
                searchViewModel.BorkResults = converter.GetView(userRepo.GetSearchBorks(searchText, userId ?? 0));
                if (searchViewModel.BorkResults.Count > 0) searchViewModel.ValidResults = true;
                else searchViewModel.ValidResults = false;
                searchViewModel.BorkResults = searchViewModel.BorkResults.OrderByDescending(a => a.DateBorked).ToList();
                return PartialView("_SearchBorks", searchViewModel);
            }
        }
    }
}