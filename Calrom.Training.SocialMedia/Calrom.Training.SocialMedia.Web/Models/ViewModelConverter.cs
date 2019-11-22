using Calrom.Training.SocialMedia.Database.Models;
using Calrom.Training.SocialMedia.Database.ORMModels;
using Calrom.Training.SocialMedia.Database.ORMRepositories;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Web.Models
{
    public class ViewModelConverter
    {
        public UserViewModel GetView(UserModel userModel)
        {
            if (userModel == null) return null;
            var getViewBorks = new List<BorkViewModel>();
            var viewNotif = new List<NotificationViewModel>();

            if (userModel.Notifications.Count != 0)
            {
                foreach (var notif in userModel.Notifications)
                {
                    viewNotif.Add(this.GetView(notif));
                }
                viewNotif = viewNotif.OrderByDescending(a => a.DateCreated).ToList();
                viewNotif = viewNotif.Take(4).ToList();
            }

            if (userModel.UserBorks.Count != 0)
            {
                foreach (var bork in userModel.UserBorks)
                {
                    getViewBorks.Add(this.GetView(bork));
                }
                getViewBorks = getViewBorks.OrderByDescending(a => a.DateBorked).ToList();
            }

            var FollowingId = new List<int>();
            var FollowerId = new List<int>();
            if (userModel.Following.Count != 0)
            {
                foreach (var user in userModel.Following)
                {
                    FollowingId.Add(user.FollowingId);
                }
            }
            if (userModel.Followers.Count != 0)
            {
                foreach (var user in userModel.Followers)
                {
                    FollowerId.Add(user.FollowerId);
                }
            }

            var newView = new UserViewModel
            {
                UserId = userModel.UserId,
                UserName = userModel.UserName,
                Password = userModel.Password,
                UserBorks = getViewBorks,
                UserPP = userModel.UserPP,
                FollowingId = FollowingId,
                FollowerId = FollowerId,
                Notifications = viewNotif
            };
            return newView;
        }

        public BorkViewModel GetView(BorkModel borkModel)
        {
            var userRepository = UserRepository.GetRepository();
            var userList = userRepository.List();
            var user = new UserModel();
            var borkRaw = userList.SelectMany(a => a.UserBorks).FirstOrDefault(a => a.BorkId == borkModel.BorkId);
            if (borkRaw == null) return null;
            else
            {
                user = borkRaw.UserModel;
            }
            var newBork = new BorkViewModel
            {
                DateBorked = borkModel.DateBorked,
                BorkText = borkModel.BorkText,
                UserId = user.UserId,
                UserPP = user.UserPP,
                UserName = user.UserName
            };
            return newBork;
        }

        public List<BorkViewModel> GetView(List<BorkModel> borkModel)
        {
            var newBorks = new List<BorkViewModel>();
            foreach (var bork in borkModel)
            {
                newBorks.Add(GetView(bork));
            }
            return newBorks;
        }

        public NotificationViewModel GetView(NotificationModel notificationModel)
        {
            var userRepository = UserRepository.GetRepository();
            var userList = userRepository.List();
            var user = new UserModel();
            var notifRaw = userList.SelectMany(a => a.Notifications).FirstOrDefault(a => a.NotificationId == notificationModel.NotificationId);
            if (notifRaw == null) return null;
            else
            {
                user = notifRaw.UserModel;
            }
            var type = (int)notificationModel.Type;

            var newNotif = new NotificationViewModel((NotificationType)type, notifRaw.UserId, notificationModel.Text, notificationModel.DateCreated);
            return newNotif;
        }
    }
}