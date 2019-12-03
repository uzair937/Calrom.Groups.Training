using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Mapping;

namespace CustomRegionEditor.Database.Maps
{
    public class CustomRegionGroupMap : ClassMap<CustomRegionGroupModel>
    {
        public CustomRegionGroupMap()
        {
            Table("dbo.ISA_CRG_custom_region");
            Id(i => i.crg_id).GeneratedBy.Guid();
            Map(i => i.custom_region_name);
            Map(i => i.custom_region_description);
            HasOne(i => i.stm);
            Map(i => i.rsm_id);
            Map(i => i.display_order);
            Map(i => i.row_version);
            HasMany(i => i.CustomRegionEntries).Cascade.All();
        }
    }
}
