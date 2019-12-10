using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Mapping;

namespace CustomRegionEditor.Database.Maps
{
    public class AirportMap : ClassMap<AirportModel>
    {
        public AirportMap()
        {
            Table("dbo.REF_APT_airport");
            Id(i => i.AirportId);
            Map(i => i.AirportName);
            References(i => i.City).Column("CityId").Cascade.All();
            Map(i => i.IsMainAirport);
            Map(i => i.IsGatewayAirport);
            Map(i => i.RowVersion);
            Map(i => i.GmaEmailAddress);
            Map(i => i.IsGmaAllowed);
            Map(i => i.IsGroupCheckinAllowed);
            Map(i => i.LtoId);
        }
    }
}
