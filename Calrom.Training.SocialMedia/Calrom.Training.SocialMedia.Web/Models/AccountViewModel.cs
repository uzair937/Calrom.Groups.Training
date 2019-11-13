using Calrom.Training.SocialMedia.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Web.Models
{
    public class AccountViewModel
    {
        public UserViewModel CurrentUser { get; set; }

        public AccountViewModel(int userId)
        {
            if (userId == 0) return;
            var userRepository = UserRepository.GetRepository();
            var MethodUser = new UserViewModel();
            var userList = userRepository.List();
            CurrentUser = MethodUser.getView(userList.First(a => a.UserId == userId));
        }

    }
}