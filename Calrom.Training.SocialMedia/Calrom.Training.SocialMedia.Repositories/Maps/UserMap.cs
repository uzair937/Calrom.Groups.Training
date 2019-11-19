using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Database.Maps
{
    public class UserMap
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<BorkMap> UserBorks { get; set; }
        public string UserPP { get; set; }
        public List<int> FollowingId { get; set; }
        public List<int> FollowerId { get; set; }
        public List<NotificationMap> Notifications { get; set; }
    }
}
