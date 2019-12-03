using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Mapping;

namespace CustomRegionEditor.Database.Maps
{
    public class AirportCityMappingMap : ClassMap<AirportCityMappingModel>
    {
        public AirportCityMappingMap()
        {
            Id(i => i.acm_id);
            References(i => i.apt_id).Cascade.All();
            References(i => i.mapped_cty_id).Cascade.All();
            Map(i => i.live_to_date);
            Map(i => i.row_version);
        }
    }
}
