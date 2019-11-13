using Calrom.Training.SocialMedia.Database.Repositories;
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

        public AccountViewModel(int userId)
        {
            if (userId == 0) return;
            var converter = new ConverterViewModel();
            var userRepository = UserRepository.GetRepository();
            var userList = userRepository.List();
            CurrentUser = converter.GetView(userList.First(a => a.UserId == userId));

            if(CurrentUser.UserName == HttpContext.Current.User.Identity.Name) IsCurrentUser = true;
            else IsCurrentUser = false;

            if (CurrentUser.Notifications != null) HasNotifications = true;
            else HasNotifications = false;

            if (CurrentUser.UserBorks != null) HasBorks = true;
            else HasBorks = false;
        }

    }
}