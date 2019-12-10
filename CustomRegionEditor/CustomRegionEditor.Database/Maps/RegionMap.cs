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
            Table("dbo.REF_REG_region");
            Id(i => i.RegionId);
            Map(i => i.RegionName);
            Map(i => i.RowVersion);
            Map(i => i.LtoId);
        }
    }
}
