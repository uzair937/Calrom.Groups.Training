using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Mapping;

namespace CustomRegionEditor.Database.Maps
{
    public class RegionMap : ClassMap<RegionModel>
    {
        public RegionMap()
        {
            Table("dbo.REF_ACM_region");
            Id(i => i.reg_id).GeneratedBy.Guid();
            Map(i => i.region_name);
            Map(i => i.row_version);
            References(i => i.lto_id).Cascade.All();
        }
    }
}
