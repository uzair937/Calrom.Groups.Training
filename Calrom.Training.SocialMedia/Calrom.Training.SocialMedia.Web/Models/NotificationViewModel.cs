using Calrom.Training.SocialMedia.Database.Models;
using Calrom.Training.SocialMedia.Database.ORMRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Web.Models
{
    public class NotificationViewModel
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Text { get; set; }

        public NotificationType Type { get; set; }

        public string LikedBork { get; set; }

        public string UserPP { get; set; }

        public DateTime DateCreated { get; set;  }

        public NotificationViewModel(NotificationType type, int userId, string text, DateTime dateCreated)
        {
            var userRepository = UserRepository.GetRepository();
            var user = userRepository.FindById(userId);
            DateCreated = dateCreated;
            Type = type;
            UserId = userId;
            Username = user.UserName;
            UserPP = user.UserPP;
            Text = text;
        }
    }
}