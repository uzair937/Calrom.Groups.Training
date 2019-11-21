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
        public virtual IList<BorkModel> UserBorks { get; set; }
        public virtual string UserPP { get; set; }
        public virtual IList<FollowingModel> Following { get; set; }
        public virtual IList<FollowerModel> Followers { get; set; }
        public virtual IList<NotificationModel> Notifications { get; set; }

        public virtual void AddBorkToUser(BorkModel bork)
        {
            if (UserBorks == null) UserBorks = new List<BorkModel>();
            bork.AddUser(this);
            UserBorks.Add(bork);
        }

        public virtual void RemoveBorkToUser(BorkModel bork)
        {
            if (UserBorks == null) UserBorks = new List<BorkModel>();
            UserBorks.Remove(bork);
        }

        public virtual void AddFollowing(UserModel user)
        {
            if (Following == null)
            {
                Following = new List<FollowingModel>();
            }
            if (this == user) return;
            if (user == null) return;
            var newFollowing = new FollowingModel();
            newFollowing.SetFollowing(user);
            newFollowing.SetUser(this);
            Following.Add(newFollowing);
        }

        public virtual void AddFollowing(FollowingModel user)
        {
            if (Following == null)
            {
                Following = new List<FollowingModel>();
            }
            Following.Add(user);
        }

        public virtual void AddFollower(FollowerModel user)
        {
            if (Followers == null)
            {
                Followers = new List<FollowerModel>();
            }
            Followers.Add(user);
        }

        public virtual void AddFollower(UserModel user)
        {
            if (Followers == null)
            {
                Followers = new List<FollowerModel>();
            }
            if (this == user) return;
            if (user == null) return;
            var newFollower = new FollowerModel();
            newFollower.SetFollower(user);
            newFollower.SetUser(this);
            Followers.Add(newFollower);
        }

        public virtual void RemoveFollowing(UserModel user)
        {
            if (Following == null) Following = new List<FollowingModel>();
            else
            {
                var unFollow = Following.FirstOrDefault(a => a.FollowingId == user.UserId);
                if (unFollow == null) return;
                Following.Remove(unFollow);
            }
        }

        public virtual void RemoveFollower(UserModel user)
        {
            if (Followers == null) Followers = new List<FollowerModel>();
            else
            {
                var unFollow = Followers.FirstOrDefault(a => a.FollowerId == user.UserId);
                if (unFollow == null) return;
                Followers.Remove(unFollow);
            }
        }

        public virtual void AddNotification(NotificationModel Notification)
        {
            if (Notifications == null) Notifications = new List<NotificationModel>();
            Notification.AddUser(this);
            Notifications.Add(Notification);
        }

        public virtual void RemoveNotification(NotificationModel Notification)
        {
            if (Notifications == null) Notifications = new List<NotificationModel>();
            Notifications.Remove(Notification);
        }

    }

}
