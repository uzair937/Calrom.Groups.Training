﻿using System;
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
            References(i => i.reg_id).Cascade.All();
            Map(i => i.row_version);
            Map(i => i.dialing_code);
            References(i => i.lto_id).Cascade.All();
        }
    }
}
