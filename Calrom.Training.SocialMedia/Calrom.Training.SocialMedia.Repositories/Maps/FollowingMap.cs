using Calrom.Training.SocialMedia.Database.ORMModels;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Database.Maps
{
    public class FollowingMap : ClassMap<FollowingModel>
    {
        public FollowingMap()
        {
            Id(x => x.Id);
            Map(x => x.FollowingId);
            Map(x => x.UserId);
        }
    }
}
