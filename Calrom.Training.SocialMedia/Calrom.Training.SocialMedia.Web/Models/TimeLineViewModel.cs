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
        public int CurrentPage { get; set; }

        private List<BorkViewModel> borkDatabaseToViewModel(List<BorkDatabaseModel> borkGet)
        {
            var borks = new List<BorkViewModel>();
            foreach (var bork in borkGet)
            {
                borks.Add(new BorkViewModel
                {
                    BorkText = bork.BorkText,
                    DateBorked = bork.DateBorked
                });
            }
            return borks;
        }

        public int changePage(int newPage)
        {
            if (newPage == 1) CurrentPage++;
            else if (CurrentPage > 0) CurrentPage--;
            return CurrentPage;
        }

        public void newUser(int id)
        {
            var userRepository = UserRepository.getRepository();
            var timeLineViewModel = getTimeLineViewModel();
            var userGet = userRepository.List();
            var currentUserborks = borkDatabaseToViewModel(userGet.ElementAt(0).UserBorks);
            timeLineViewModel.CurrentUser = new UserViewModel
            {
                UserId = userGet.ElementAt(id).UserId,
                UserName = userGet.ElementAt(id).UserName,
                Password = userGet.ElementAt(id).Password,
                UserBorks = currentUserborks,
                UserPP = userGet.ElementAt(id).UserPP,
                FollowingId = userGet.ElementAt(id).FollowingId,
                FollowerId = userGet.ElementAt(id).FollowerId,
            };
        }

        private static TimeLineViewModel timeLineViewModel;

        private TimeLineViewModel() { }

        public static TimeLineViewModel getTimeLineViewModel()
        {
            if (timeLineViewModel == null)
            {
                timeLineViewModel = new TimeLineViewModel();
                timeLineViewModel.CurrentPage = 0;
                timeLineViewModel.newUser(1);
            }
            return timeLineViewModel;
        }

        public void AddBork(string borkBoxString)
        {
            var borkRepository = BorkRepository.getRepository();
            borkRepository.Add(new Database.Models.BorkDatabaseModel
            {
                BorkText = borkBoxString,
                DateBorked = DateTime.Now
            });
        }

    }
}