using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Mapping;

namespace CustomRegionEditor.Database.Maps
{
    public class CityMap : ClassMap<City>
    {
        public CityMap()
        {
            Table("dbo.REF_CTY_city");
            Id(i => i.Id).Column("cty_id");
            Map(i => i.Name).Column("city_name");
            References(i => i.Country).Column("cnt_id").Cascade.All();
            Map(i => i.RowVersion).Column("row_version");
            References(i => i.State).Column("sta_id").Cascade.All();
            Map(i => i.TimeZone).Column("timezone");
            Map(i => i.UtcOffset).Column("utc_offset");
            Map(i => i.LtoId).Column("lto_id");

            HasMany(i => i.Airports).Cascade.All().Inverse();
        }
    }
}
