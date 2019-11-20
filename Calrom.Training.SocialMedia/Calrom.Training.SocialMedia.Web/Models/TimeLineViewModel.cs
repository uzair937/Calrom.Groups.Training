using System;
using System.Collections.Generic;
using Calrom.Training.SocialMedia.Database.ORMRepositories;
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
            var converter = new ViewModelConverter();
            var userList = userRepository.List();
            CurrentUser = converter.GetView(userList.First(a => a.UserId == userId));
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