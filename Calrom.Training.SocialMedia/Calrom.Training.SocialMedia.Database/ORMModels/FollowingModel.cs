using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Database.ORMModels
{
    public class FollowingModel
    {
        public virtual int Id { get; set; }
        public virtual int FollowingId { get; protected set; }
        public virtual int UserId { get; set; }

        public virtual void SetFollowing(UserModel user)
        {
            FollowingId = user.UserId;
        }

        public virtual void SetUser(UserModel user)
        {
            if (user.UserId == 0) UserId = (FollowingId % 2) + 1;
            else UserId = user.UserId;
        }
    }
}
