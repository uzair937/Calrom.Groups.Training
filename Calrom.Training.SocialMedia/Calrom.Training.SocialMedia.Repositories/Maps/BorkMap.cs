using Calrom.Training.SocialMedia.Database.ORMModels;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Database.Maps
{
    public class BorkMap : ClassMap<BorkModel>
    {
        public BorkMap()
        {
            Id(x => x.BorkId);
            Map(x => x.BorkText);
            Map(x => x.DateBorked);
            References(x => x.UserModel).Cascade.All();
        }
    }
}