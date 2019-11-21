using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Database.ORMModels
{
    public class FollowerModel
    {
        public virtual int Id { get; set; }
        public virtual int FollowerId { get; protected set; }
        public virtual int UserId { get; set; }

        public virtual void SetFollower(UserModel user)
        {
            FollowerId = user.UserId;
        }

        public virtual void SetUser(UserModel user)
        {
            if (user.UserId == 0) UserId = (FollowerId % 2) + 1;
            else UserId = user.UserId;
        }
    }

}
