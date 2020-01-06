using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Mapping;

namespace CustomRegionEditor.Database.Maps
{
    public class RegionMap : ClassMap<Region>
    {
        public RegionMap()
        {
            Table("dbo.REF_REG_region");
            Id(i => i.Id).Column("reg_id");
            Map(i => i.Name).Column("region_name");
            Map(i => i.RowVersion).Column("row_version");
            Map(i => i.LtoId).Column("lto_id");

            HasMany(i => i.Countries).Cascade.All().Inverse();
        }
    }
}
