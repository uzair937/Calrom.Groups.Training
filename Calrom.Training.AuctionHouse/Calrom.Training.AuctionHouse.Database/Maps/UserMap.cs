using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calrom.Training.AuctionHouse.Database
{
    public class UserMap : ClassMap<UserModel>
    {
        public UserMap()
        {
            Id(i => i.UserID);
            Map(i => i.Username);
            Map(i => i.Password);
            Map(i => i.DateOfBirth);
            HasMany(i => i.BidList);
        }
    }
}
