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
            Id(i => i.CityId);
            Map(i => i.CityName);
            References(i => i.Country).Column("CountryId").Cascade.All();
            Map(i => i.RowVersion);
            References(i => i.State).Column("StateId").Cascade.All();
            Map(i => i.TimeZone);
            Map(i => i.UtcOffset);
            Map(i => i.LtoId);
        }
    }
}
