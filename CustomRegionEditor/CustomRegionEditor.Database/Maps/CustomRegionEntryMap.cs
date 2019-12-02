using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Mapping;

namespace CustomRegionEditor.Database.Maps
{
    public class CustomRegionEntryMap : ClassMap<CustomRegionEntryModel>
    {
        public CustomRegionEntryMap()
        {
            Id(i => i.cre_id);
            References(i => i.crg_id).Cascade.All();
            HasOne(i => i.Region);
            HasOne(i => i.Country);
            HasOne(i => i.State);
            HasOne(i => i.City);
            HasOne(i => i.Airport);
            Map(i => i.row_version);
        }
    }
}
