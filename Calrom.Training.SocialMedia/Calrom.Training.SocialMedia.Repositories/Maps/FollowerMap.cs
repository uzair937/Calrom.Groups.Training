using Calrom.Training.SocialMedia.Database.ORMModels;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Database.Maps
{
    public class FollowerMap : ClassMap<FollowerModel>
    {
        public FollowerMap()
        {
            Id(x => x.Id);
            Map(x => x.FollowerId);
            Map(x => x.UserId);
        }
    }
}
