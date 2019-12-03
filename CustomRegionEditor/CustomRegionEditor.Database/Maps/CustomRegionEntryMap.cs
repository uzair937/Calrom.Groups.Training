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
            References(i => i.Region).Cascade.All();
            References(i => i.Country).Cascade.All();
            References(i => i.State).Cascade.All();
            References(i => i.City).Cascade.All();
            References(i => i.Airport).Cascade.All();
            Map(i => i.row_version);
        }
    }
}
