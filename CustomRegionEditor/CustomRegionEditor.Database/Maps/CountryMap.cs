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
            Id(i => i.ID);
            Map(i => i.Name);
            Map(i => i.Abbreviation);
        }
    }
}
