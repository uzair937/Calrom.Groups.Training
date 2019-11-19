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
        public virtual int UserId { get; set; }

        public virtual string Username { get; set; }

        public virtual string Text { get; set; }
        
        public virtual NotificationType Type { get; set; }

        public virtual string LikedBork { get; set; }

        public virtual string UserPP { get; set; }

        public virtual DateTime DateCreated { get; set; }

    }
}
