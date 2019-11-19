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
        public virtual List<BorkModel> UserBorks { get; set; }
        public virtual string UserPP { get; set; }
        public virtual List<int> FollowingId { get; set; }
        public virtual List<int> FollowerId { get; set; }
        public virtual List<NotificationModel> Notifications { get; set; }
    }
}
