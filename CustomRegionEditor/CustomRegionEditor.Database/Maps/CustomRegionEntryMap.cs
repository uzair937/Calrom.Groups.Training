using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Mapping;

namespace CustomRegionEditor.Database.Maps
{
    public class CustomRegionEntryMap : ClassMap<CustomRegionEntry>
    {
        public CustomRegionEntryMap()
        {
            Table("dbo.ISA_CRE_custom_region_entry");
            Id(i => i.Id).Column("cre_id").GeneratedBy.Guid();
            References(i => i.CustomRegionGroup).Column("crg_id");
            References(i => i.Region).Column("reg_id");
            References(i => i.Country).Column("cnt_id");
            References(i => i.State).Column("sta_id");
            References(i => i.City).Column("cty_id");
            References(i => i.Airport).Column("apt_id");
            Map(i => i.RowVersion).Column("row_version"); ;
        }
    }
}
