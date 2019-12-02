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
            Map(i => i.apt_id);
            Map(i => i.mapped_cty_id);
            Map(i => i.live_to_date);
            Map(i => i.row_version);
        }
    }
}
