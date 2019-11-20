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
            Map(x => x.Password);
            HasMany(x => x.UserBorks).Cascade.All();
            HasMany(x => x.Notifications).Cascade.All();
            HasMany(x => x.Following).Cascade.All();
            HasMany(x => x.Followers).Cascade.All();
        }
    }
}
