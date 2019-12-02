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
            Id(i => i.cty_id);
            Map(i => i.city_name);
            Map(i => i.cnt_id);
            Map(i => i.row_version);
            Map(i => i.sta_id);
            Map(i => i.timezone);
            Map(i => i.utc_offset);
            Map(i => i.lto_id);
        }
    }
}
