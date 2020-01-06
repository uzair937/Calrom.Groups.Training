using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Mapping;

namespace CustomRegionEditor.Database.Maps
{
    public class CountryMap : ClassMap<Country>
    {
        public CountryMap()
        {
            Table("dbo.REF_CNT_country");
            Id(i => i.Id).Column("cnt_id");
            Map(i => i.Name).Column("country_name");
            Map(i => i.IsoCode).Column("iso_code");
            Map(i => i.IsoNumber).Column("iso_number");
            References(i => i.Region).Column("reg_id").Cascade.All();
            Map(i => i.RowVersion).Column("row_version");
            Map(i => i.DialingCode).Column("dialing_code");
            Map(i => i.LtoId).Column("lto_id");

            HasMany(i => i.Cities).Cascade.All().Inverse();
            HasMany(i => i.States).Cascade.All().Inverse();
        }
    }
}
