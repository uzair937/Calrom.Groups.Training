using System.Collections.Generic;
using Calrom.Training.SocialMedia.Database.ORMRepositories;
using System.Linq;

namespace Calrom.Training.SocialMedia.ViewModels
{
    public class TimeLineViewModel
    {
        public List<BorkViewModel> Borks { get; set; }
        public UserViewModel CurrentUser { get; set; }
        public PaginationViewModel PageView { get; set; }
        public List<BorkViewModel> AllBorks { get; set; }
        public FollowViewModel followViewModel { get; set; }

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