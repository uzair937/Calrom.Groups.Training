using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Mapping;

namespace CustomRegionEditor.Database.Maps
{
    public class AirportMap : ClassMap<Airport>
    {
        public AirportMap()
        {
            Table("dbo.REF_APT_airport");
            Id(i => i.Id).Column("apt_id");
            Map(i => i.Name).Column("airport_name");
            References(i => i.City).Column("cty_id").Cascade.All();
            Map(i => i.IsMainAirport).Column("is_main_airport");
            Map(i => i.IsGatewayAirport).Column("is_gateway_airport");
            Map(i => i.RowVersion).Column("row_version");
            Map(i => i.GmaEmailAddress).Column("gma_email_address");
            Map(i => i.IsGmaAllowed).Column("is_gma_allowed");
            Map(i => i.IsGroupCheckinAllowed).Column("is_group_checkin_allowed");
            Map(i => i.LtoId).Column("lto_id");
        }
    }
}
