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
            Table("dbo.ISA_CRE_custom_region_entry");
            Id(i => i.CreId).GeneratedBy.Guid();
            References(i => i.CustomRegionGroup).Column("CrgId");
            References(i => i.Region).Column("RegionId");
            References(i => i.Country).Column("CountryId");
            References(i => i.State).Column("StateId");
            References(i => i.City).Column("CityId");
            References(i => i.Airport).Column("AirportId");
            Map(i => i.RowVersion);
        }
    }
}
