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
            Table("dbo.REF_ACM_airport");
            Id(i => i.apt_id);
            Map(i => i.airport_name);
            References(i => i.cty).Column("cty_id").Cascade.All();
            Map(i => i.is_main_airport);
            Map(i => i.is_gateway_airport);
            Map(i => i.row_version);
            Map(i => i.gma_email_address);
            Map(i => i.is_gma_allowed);
            Map(i => i.is_group_checkin_allowed);
            Map(i => i.lto_id);
        }
    }
}
