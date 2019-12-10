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
            Id(i => i.CrgId).GeneratedBy.Guid();
            Map(i => i.CustomRegionName);
            Map(i => i.CustomRegionDescription);
            References(i => i.System).Column("SystemId").Cascade.All();
            Map(i => i.RsmId).Nullable();
            Map(i => i.DisplayOrder).Nullable();
            Map(i => i.RowVersion);
            HasMany(i => i.CustomRegionEntries).Cascade.All().Inverse();
        }
    }
}
