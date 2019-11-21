using Calrom.Training.SocialMedia.Database.ORMModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calrom.Training.SocialMedia.Database.NHibernateTools
{
    class Program
    {
        static void Main(string[] args)
        {
            var nHibernateManager = new NHibernateManager();
            var borkModel = new BorkModel()
            {
                BorkText = "Hello, it works!",
                DateBorked = DateTime.Now
            };
            var notificationModel = new NotificationModel()
            {
                Text = "is following you",
                DateCreated = DateTime.Now
            };
            var userModel = new UserModel()
            {
                UserName = "Will",
                //UserBorks = new List<BorkModel>() { borkModel },
                //Notifications = new List<NotificationModel>() { notificationModel }
            };
            userModel.AddBorkToUser(borkModel);
            userModel.AddNotification(notificationModel);
            nHibernateManager.SaveUserModel(userModel);

            var getUserModel = nHibernateManager.GetUserModel(1);
            var getBorkModel = nHibernateManager.GetBorkModel(1);
            var getNotificationModel = nHibernateManager.GetNotificationModel(1);
            Console.Read();
        }
    }
}
