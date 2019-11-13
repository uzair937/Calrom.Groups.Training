using Calrom.Training.SocialMedia.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Web.Models
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<BorkViewModel> UserBorks { get; set; }
        public string UserPP { get; set; }
        public List<int> FollowingId { get; set; }
        public List<int> FollowerId { get; set; }
        public List<NotificationViewModel> Notifications { get; set; }

        public UserViewModel GetView(UserDatabaseModel getdb)
        {
            if (getdb == null) return null;
            var getViewBorks = new List<BorkViewModel>();
            var methodBork = new BorkViewModel();
            var viewNotif = new List<NotificationViewModel>();

            if (getdb.Notifications != null)
            {
                foreach (var notif in getdb.Notifications)
                {
                    viewNotif.Add(new NotificationViewModel(notif.Type, notif.UserId, notif.LikedBork, notif.DateCreated));
                }
                viewNotif = viewNotif.OrderByDescending(a => a.DateCreated).ToList();
            }

            if (getdb.UserBorks != null)
            {
                foreach (var bork in getdb.UserBorks)
                {
                    getViewBorks.Add(methodBork.GetView(bork));
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
    }
}
