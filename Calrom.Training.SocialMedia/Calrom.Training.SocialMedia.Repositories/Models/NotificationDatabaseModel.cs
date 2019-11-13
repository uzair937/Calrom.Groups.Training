using System;
using System.Collections.Generic;
using System.Text;

namespace Calrom.Training.SocialMedia.Database.Models
{
    public class NotificationDatabaseModel
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string LikedBorkText { get; set; }
        
        public NotificationType Type { get; set; }
    }

    public enum NotificationType { Like, Follow, Unfollow }
}
