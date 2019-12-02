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
            Id(i => i.cnt_id);
            Map(i => i.country_name);
            Map(i => i.iso_code);
            Map(i => i.iso_number);
            Map(i => i.reg_id);
            Map(i => i.row_version);
            Map(i => i.dialing_code);
            Map(i => i.lto_id);
        }
    }
}
