using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calrom.Training.AuctionHouse.Database
{
    public class BidMap : ClassMap<BidModel>
    {
        public BidMap()
        {
            Id(i => i.BidID);
            HasOne(i => i.Product).Cascade.All();
            References(i => i.User).Cascade.All();
        }
    }
}
