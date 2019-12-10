using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Mapping;

namespace CustomRegionEditor.Database.Maps
{
    public class CountryMap : ClassMap<CountryModel>
    {
        public CountryMap()
        {
            Table("dbo.REF_CNT_country");
            Id(i => i.CountryId);
            Map(i => i.CountryName);
            Map(i => i.IsoCode);
            Map(i => i.IsoNumber);
            References(i => i.Region).Column("RegionId").Cascade.All();
            Map(i => i.RowVersion);
            Map(i => i.DialingCode);
            Map(i => i.LtoId);
        }
    }
}
