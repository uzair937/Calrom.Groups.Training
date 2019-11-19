using Calrom.Training.SocialMedia.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calrom.Training.SocialMedia.Database.Models
{
    public class NotificationModel
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Text { get; set; }
        
        public NotificationType Type { get; set; }

        public string LikedBork { get; set; }

        public string UserPP { get; set; }

        public DateTime DateCreated { get; set; }

        public NotificationModel(NotificationType type, int userId, string likedBork)
        {
            var userRepository = UserRepository.GetRepository();
            var userList = userRepository.List();
            DateCreated = DateTime.Now;
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
    }

    public enum NotificationType { Like, Follow, Unfollow }
}
