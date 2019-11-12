using System;
using System.Collections.Generic;
using Calrom.Training.SocialMedia.Database.Repositories;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calrom.Training.SocialMedia.Database.Models;

namespace Calrom.Training.SocialMedia.Web.Models
{
    public class TimeLineViewModel
    {
        public List<BorkViewModel> Borks { get; set; }
        public UserViewModel CurrentUser { get; set; }
        public PaginationViewModel PageView { get; set; }
        public List<BorkViewModel> AllBorks { get; set; }

        public TimeLineViewModel(int userId)
        {
            if (userId == 0) return;
            var userRepository = UserRepository.GetRepository();
            var MethodUser = new UserViewModel();
            var userList = userRepository.List();
            CurrentUser = MethodUser.getView(userList.First(a => a.UserId == userId));
        }

        public void AddBork(string borkBoxString)
        {
            var userRepository = UserRepository.GetRepository();
            var borkRepository = BorkRepository.GetRepository();
            var userList = userRepository.List();
            borkRepository.Add(new BorkDatabaseModel
            {
                BorkText = borkBoxString,
                DateBorked = DateTime.Now,
                UserId = CurrentUser.UserId
            });
            var newBork = borkRepository.List().ElementAt(0);
            userList.First(a => a.UserName == HttpContext.Current.User.Identity.Name).UserBorks.Add(newBork);
            CurrentUser = CurrentUser.getView(userList.First(a => a.UserName == HttpContext.Current.User.Identity.Name));
        }

        public string GetUserName(int userId)
        {
            var userRepository = UserRepository.GetRepository();
            var userList = userRepository.List();
            var user = userList.First(a => a.UserId == userId);
            return user.UserName;
        }

        public string GetUserPP (int userId)
        {
            var userRepository = UserRepository.GetRepository();
            var userList = userRepository.List();
            var user = userList.First(a => a.UserId == userId);
            return user.UserPP;
        }
    }
}