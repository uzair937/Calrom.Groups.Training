using Calrom.Training.SocialMedia.Database.ORMRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Web.Models
{
    public class AccountViewModel
    {
        public UserViewModel CurrentUser { get; set; }
        public bool IsCurrentUser { get; set; }
        public bool HasNotifications { get; set; }
        public bool HasBorks { get; set; }
        public bool FollowsUser { get; set; }
        public List<string> FollowedUsers { get; set; }

        public AccountViewModel(int userId)
        {
            if (userId == 0) return;
            var converter = new ViewModelConverter();
            var userRepository = UserRepository.GetRepository();
            var userList = userRepository.List();
            var viewingUser = converter.GetView(userList.First(a => a.UserName == HttpContext.Current.User.Identity.Name));
            CurrentUser = converter.GetView(userList.First(a => a.UserId == userId));

            if (CurrentUser.UserName == viewingUser.UserName) IsCurrentUser = true;
            else IsCurrentUser = false;

            if (CurrentUser.Notifications != null) HasNotifications = true;
            else HasNotifications = false;

            if (CurrentUser.UserBorks != null) HasBorks = true;
            else HasBorks = false;

            if (viewingUser.FollowingId.Contains(userId)) FollowsUser = true;
            else FollowsUser = false;

            FollowedUsers = GetFollowedUsers(userRepository, CurrentUser.UserId);
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

    }
}