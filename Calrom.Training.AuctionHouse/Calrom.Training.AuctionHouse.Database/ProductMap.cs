using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calrom.Training.AuctionHouse.Database
{
    public class ProductMap : ClassMap<ProductDatabaseModel>
    {
        public ProductMap()
        {
            Id(i => i.ItemID);
            Map(i => i.ItemName);
            Map(i => i.ItemDescription);
            Map(i => i.ItemPrice);
            Map(i => i.CurrentBid);
            Map(i => i.ImageSrc);
        }
    }
}
