using Calrom.Training.SocialMedia.Database.Models;
using Calrom.Training.SocialMedia.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Web.Models
{
    public class ViewModelConverter
    {
        public UserViewModel GetView(UserDatabaseModel getdb)
        {
            if (getdb == null) return null;
            var getViewBorks = new List<BorkViewModel>();
            var viewNotif = new List<NotificationViewModel>();

            if (getdb.Notifications != null)
            {
                foreach (var notif in getdb.Notifications)
                {
                    viewNotif.Add(this.GetView(notif));
                }
                viewNotif = viewNotif.OrderByDescending(a => a.DateCreated).ToList();
            }

            if (getdb.UserBorks != null)
            {
                foreach (var bork in getdb.UserBorks)
                {
                    getViewBorks.Add(this.GetView(bork));
                }
                getViewBorks = getViewBorks.OrderByDescending(a => a.DateBorked).ToList();
            }

            if (getdb.FollowingId == null) getdb.FollowingId = new List<int>();
            if (getdb.FollowerId == null) getdb.FollowerId = new List<int>();

            var newView = new UserViewModel
            {
                UserId = getdb.UserId,
                UserName = getdb.UserName,
                Password = getdb.Password,
                UserBorks = getViewBorks,
                UserPP = getdb.UserPP,
                FollowingId = getdb.FollowingId,
                FollowerId = getdb.FollowerId,
                Notifications = viewNotif
            };
            return newView;
        }

        public BorkViewModel GetView(BorkDatabaseModel getBork)
        {
            var userRepository = UserRepository.GetRepository();
            var userList = userRepository.List();
            var userPP = userList.First(a => a.UserId == getBork.UserId).UserPP;
            var userName = userList.First(a => a.UserId == getBork.UserId).UserName;
            var newBork = new BorkViewModel
            {
                DateBorked = getBork.DateBorked,
                BorkText = getBork.BorkText,
                UserId = getBork.UserId,
                UserPP = userPP,
                UserName = userName
            };
            return newBork;
        }

        public NotificationViewModel GetView(NotificationDatabaseModel getNotif)
        {
            var newNotif = new NotificationViewModel(getNotif.Type, getNotif.UserId, getNotif.LikedBork, getNotif.DateCreated);
            return newNotif;
        }
    }
}