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

        public TimeLineViewModel(int userId)
        {
            if (userId == 0) return;
            var userRepository = UserRepository.GetRepository();
            var MethodUser = new UserViewModel();
            var userList = userRepository.List();
            CurrentUser = MethodUser.getView(userList.ElementAt(userId - 1));
            NewUser(userId);
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
            userList.ElementAt(CurrentUser.UserId - 1).UserBorks.Add(newBork);
            CurrentUser = CurrentUser.getView(userList.ElementAt(CurrentUser.UserId - 1));
        }
  
        public void NewUser(int userId)
        {
            var userRepository = UserRepository.GetRepository();
            var userGet = userRepository.List();
            CurrentUser = CurrentUser.getView(userGet.ElementAt(userId - 1));
        }
    }
}