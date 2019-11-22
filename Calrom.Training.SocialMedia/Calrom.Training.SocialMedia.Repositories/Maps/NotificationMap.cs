using Calrom.Training.SocialMedia.Database.ORMModels;
using Calrom.Training.SocialMedia.Database.Repositories;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calrom.Training.SocialMedia.Database.Maps
{
    public class NotificationMap : ClassMap<NotificationModel>
    {
        public NotificationMap()
        {
            Id(x => x.NotificationId);
            Map(x => x.Type);
            Map(x => x.Text);
            Map(x => x.UserId);
            Map(x => x.DateCreated);
            References(x => x.UserModel);
        }
    }
}
