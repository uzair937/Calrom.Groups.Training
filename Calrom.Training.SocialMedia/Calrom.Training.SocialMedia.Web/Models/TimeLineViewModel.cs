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
        public int TotalPages { get; set; }

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
            var userRepository = UserRepository.getRepository();
            var borkRepository = BorkRepository.getRepository();
            var userList = userRepository.List();
            borkRepository.Add(new BorkDatabaseModel
            {
                BorkText = borkBoxString,
                DateBorked = DateTime.Now,
                UserId = CurrentUser.UserId
            });
            var newBork = borkRepository.List().ElementAt(borkRepository.List().Count() - 1);
            userList.ElementAt(CurrentUser.UserId - 1).UserBorks.Add(newBork);
            CurrentUser = CurrentUser.getView(userList.ElementAt(CurrentUser.UserId - 1));
        }

        public int changePage(int newPage)
        {
            if (newPage == 1 && (CurrentPage + 1) < TotalPages) CurrentPage++;
            else if (newPage != 1 && CurrentPage > 0) CurrentPage--;
            return CurrentPage;
        }

        public void newUser(int id)
        {
            id--;
            var userRepository = UserRepository.getRepository();
            var timeLineViewModel = getTimeLineViewModel();
            timeLineViewModel.CurrentUser = new UserViewModel();
            var userGet = userRepository.List();
            timeLineViewModel.CurrentUser = timeLineViewModel.CurrentUser.getView(userGet.ElementAt(0));
        }

    }
}