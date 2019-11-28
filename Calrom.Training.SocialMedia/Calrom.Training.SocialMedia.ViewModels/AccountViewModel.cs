namespace Calrom.Training.SocialMedia.ViewModels
{
    public class AccountViewModel
    {
        public UserViewModel CurrentUser { get; set; }
        public bool IsCurrentUser { get; set; }
        public bool HasNotifications { get; set; }
        public bool HasBorks { get; set; }
        public bool FollowsUser { get; set; }
        public FollowViewModel followViewModel { get; set; }

    }
}