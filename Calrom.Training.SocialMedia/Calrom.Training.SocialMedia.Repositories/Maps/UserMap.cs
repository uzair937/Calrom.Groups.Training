using Calrom.Training.SocialMedia.Database.ORMModels;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Database.Maps
{
    public class UserMap : ClassMap<UserModel>
    {
        public UserMap()
        {
            Id(x => x.UserId);
            Map(x => x.UserName);
            Map(x => x.UserPP);
            Map(x => x.UserBorks);
            Map(x => x.Password);
            Map(x => x.Notifications);
            Map(x => x.FollowingId);
            Map(x => x.FollowerId);
        }
    }
}
