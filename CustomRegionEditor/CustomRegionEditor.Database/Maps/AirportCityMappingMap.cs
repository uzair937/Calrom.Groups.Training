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
            Table("dbo.REF_ACM_airport_city_mapping");
            Id(i => i.acm_id).GeneratedBy.Guid();
            References(i => i.apt).Column("apt_id").Cascade.All();
            HasOne(i => i.mapped_cty).Cascade.All();
            Map(i => i.live_to_date);
            Map(i => i.row_version);
        }
    }
}
