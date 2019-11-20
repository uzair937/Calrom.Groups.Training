using Calrom.Training.SocialMedia.Database.ORMModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calrom.Training.SocialMedia.Database.NHibernateTools
{
    public class NHibernateManager
    {
        public void SaveBorkModel(BorkModel borkModel)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                session.SaveOrUpdate(borkModel);
                session.Flush();
            }
        }

        public void SaveUserModel(UserModel userModel)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                session.SaveOrUpdate(userModel);
                session.Flush();
            }
        }

        public BorkModel GetBorkModel(int userId)
        {
            var borkModel = new BorkModel();
            using (var session = NHibernateHelper.OpenSession())
            {
                borkModel = session.Get<BorkModel>(userId);
            }
            return borkModel;
        }

        public UserModel GetUserModel(int userId)
        {
            var userModel = new UserModel();
            using (var session = NHibernateHelper.OpenSession())
            {
                userModel = session.Get<UserModel>(userId);
            }
            return userModel;
        }

        public NotificationModel GetNotificationModel(int userId)
        {
            var notificationModel = new NotificationModel();
            using (var session = NHibernateHelper.OpenSession())
            {
                notificationModel = session.Get<NotificationModel>(userId);
            }
            return notificationModel;
        }
    }
}
