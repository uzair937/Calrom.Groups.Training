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
            Id(i => i.AcmId).GeneratedBy.Guid();
            References(i => i.Airport).Column("AirportId").Cascade.All();
            References(i => i.MappedCity).Column("mapped_cty_id").Cascade.All();
            Map(i => i.LiveToDate);
            Map(i => i.RowVersion);
        }
    }
}
