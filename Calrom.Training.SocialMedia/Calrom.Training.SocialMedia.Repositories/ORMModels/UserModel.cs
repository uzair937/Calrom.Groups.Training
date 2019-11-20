using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Database.ORMModels
{
    public class UserModel
    {
        public virtual int UserId { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual IList<BorkModel> UserBorks { get; protected set; }
        public virtual string UserPP { get; set; }
        public virtual IList<UserModel> Following { get; protected set; }
        public virtual IList<UserModel> Followers { get; protected set; }
        public virtual IList<NotificationModel> Notifications { get; protected set; }

        public virtual void AddBorkToUser(BorkModel bork)
        {
            if (UserBorks == null) UserBorks = new List<BorkModel>();
            UserBorks.Add(bork);
        }

        public virtual void RemoveBorkToUser(BorkModel bork)
        {
            if (UserBorks == null) UserBorks = new List<BorkModel>();
            UserBorks.Remove(bork);
        }

        public virtual void AddFollowing(UserModel user)
        {
            if (Following == null) Following = new List<UserModel>();
            Following.Add(user);
        }

        public virtual void AddFollower(UserModel user)
        {
            if (Followers == null) Followers = new List<UserModel>();
            Followers.Add(user);
        }

        public virtual void RemoveFollowing(UserModel user)
        {
            if (Following == null) Following = new List<UserModel>();
            Following.Remove(user);
        }

        public virtual void RemoveFollower(UserModel user)
        {
            if (Followers == null) Followers = new List<UserModel>();
            Followers.Remove(user);
        }

        public virtual void AddNotification(NotificationModel Notification)
        {
            if (Notifications == null) Notifications = new List<NotificationModel>();
            Notifications.Add(Notification);
        }

        public virtual void RemoveNotification(NotificationModel Notification)
        {
            if (Notifications == null) Notifications = new List<NotificationModel>();
            Notifications.Remove(Notification);
        }

    }

}
