using Calrom.Training.SocialMedia.Database.Models;
using Calrom.Training.SocialMedia.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calrom.Training.SocialMedia.Database.ORMModels
{
    public class NotificationModel
    {
        public virtual int NotificationId { get; set; }

        public virtual string Text { get; set; }
        
        public virtual NotificationEnum Type { get; set; }

        public virtual DateTime DateCreated { get; set; }

        public virtual UserModel UserModel { get; protected set; }

        public virtual void AddUser(UserModel user)
        {
            UserModel = user;
        }
    }

    public enum NotificationEnum { Like, Follow, Unfollow }
}
