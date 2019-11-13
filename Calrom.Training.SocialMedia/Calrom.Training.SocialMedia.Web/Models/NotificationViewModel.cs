using Calrom.Training.SocialMedia.Database.Models;
using Calrom.Training.SocialMedia.Database.Repositories;
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

        public NotificationViewModel(NotificationType type, int userId, string likedBork, DateTime dateCreated)
        {
            var userRepository = UserRepository.GetRepository();
            var userList = userRepository.List();
            DateCreated = dateCreated;
            Type = type;
            UserId = userId;
            Username = userList.First(a => a.UserId == UserId).UserName;
            UserPP = userList.First(a => a.UserId == UserId).UserPP;
            LikedBork = likedBork;
            if (Type == NotificationType.Like)
            {
                Text = Username + " has liked your bork: \n" + LikedBork;
            }
            else if (Type == NotificationType.Follow)
            {
                Text = Username + " has followed you!";
            }
            else if (Type == NotificationType.Unfollow)
            {
                Text = Username + " has unfollowed you!";
            }
        }

        public NotificationDatabaseModel GetDb()
        {
            var newNotif = new NotificationDatabaseModel(Type, UserId, LikedBork);
            return newNotif;
        }
    }
}