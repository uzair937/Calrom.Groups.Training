using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calrom.Training.AuctionHouse.Database
{
    public class BidMap : ClassMap<BidDatabaseModel>
    {
        public BidMap()
        {
            //Id(i => i.ID);
        }
    }
}
