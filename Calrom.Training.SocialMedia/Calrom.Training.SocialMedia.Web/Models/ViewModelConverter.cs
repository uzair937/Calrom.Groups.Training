using AutoMapper;
using Calrom.Training.SocialMedia.Database.Models;
using Calrom.Training.SocialMedia.Database.ORMModels;
using Calrom.Training.SocialMedia.Database.ORMRepositories;
using Calrom.Training.SocialMedia.Mapper;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.ViewModels
{
    public class ViewModelConverter
    {
        public UserViewModel GetView(UserModel userModel)
        {
            var userViewModel = AutoMapperConfiguration.GetInstance<UserViewModel>(userModel);
            userViewModel.UserBorks = GetView(userModel.UserBorks);
            userViewModel.Notifications = GetView(userModel.Notifications);
            return userViewModel;
        }

        public BorkViewModel GetView(BorkModel borkModel)
        {
            var borkViewModel = AutoMapperConfiguration.GetInstance<BorkViewModel>(borkModel);
            return borkViewModel;
        }

        public List<BorkViewModel> GetView(IList<BorkModel> borkModel)
        {
            var newBorks = new List<BorkViewModel>();
            foreach (var bork in borkModel)
            {
                newBorks.Add(GetView(bork));
            }
            return newBorks;
        }

        public List<NotificationViewModel> GetView(IList<NotificationModel> notificationModel)
        {
            var newNotifications = new List<NotificationViewModel>();
            foreach (var notif in notificationModel)
            {
                newNotifications.Add(GetView(notif));
            }
            return newNotifications;
        }

        public NotificationViewModel GetView(NotificationModel notificationModel)
        {
            var notificationViewModel = AutoMapperConfiguration.GetInstance<NotificationViewModel>(notificationModel);
            return notificationViewModel;
        }
    }
}