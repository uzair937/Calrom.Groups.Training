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
            Id(i => i.cre_id).GeneratedBy.Guid();
            References(i => i.crg).Column("crg_id").Cascade.All();
            References(i => i.reg).Column("reg_id").Cascade.All();
            References(i => i.cnt).Column("cnt_id").Cascade.All();
            References(i => i.sta).Column("sta_id").Cascade.All();
            References(i => i.cty).Column("cty_id").Cascade.All();
            References(i => i.apt).Column("apt_id").Cascade.All();
            Map(i => i.row_version);
        }
    }
}
