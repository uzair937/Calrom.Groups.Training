using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Mapping;

namespace CustomRegionEditor.Database.Maps
{
    public class CityMap : ClassMap<CityModel>
    {
        public CityMap()
        {
            Table("dbo.REF_CTY_city");
            Id(i => i.cty_id);
            Map(i => i.city_name);
            References(i => i.cnt).Column("cnt_id").Cascade.All();
            Map(i => i.row_version);
            HasOne(i => i.sta).Cascade.All();
            Map(i => i.timezone);
            Map(i => i.utc_offset);
            Map(i => i.lto_id);
        }
    }
}
