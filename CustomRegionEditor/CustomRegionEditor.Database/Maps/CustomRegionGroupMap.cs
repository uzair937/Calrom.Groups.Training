using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Mapping;

namespace CustomRegionEditor.Database.Maps
{
    public class CustomRegionGroupMap : ClassMap<CustomRegionGroup>
    {
        public CustomRegionGroupMap()
        {
            Table("dbo.ISA_CRG_custom_region");
            Id(i => i.Id).Column("crg_id").GeneratedBy.Guid();
            Map(i => i.Name).Column("custom_region_name");
            Map(i => i.Description).Column("custom_region_description");
            References(i => i.System).Column("stm_id").Cascade.All();
            Map(i => i.RsmId).Column("rsm_id").Nullable();
            Map(i => i.DisplayOrder).Column("display_order").Nullable();
            Map(i => i.RowVersion).Column("row_version");
            HasMany(i => i.CustomRegionEntries).Cascade.All().Inverse();
        }
    }
}
