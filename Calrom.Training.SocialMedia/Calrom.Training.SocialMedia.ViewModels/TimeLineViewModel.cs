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
    }
}