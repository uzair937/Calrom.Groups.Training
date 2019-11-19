using Calrom.Training.SocialMedia.Database.Models;
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
            Id(x => x.UserId);
            Map(x => x.BorkText);
            Map(x => x.DateBorked);
        }
    }
}