﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Models
{
    public class CustomRegionEntryModel
    {
        public Guid Id { get; set; }
        public CustomRegionGroupModel CustomRegionGroup { get; set; }
        public RegionModel Region { get; set; }
        public CountryModel Country { get; set; }
        public StateModel State { get; set; }
        public CityModel City { get; set; }
        public AirportModel Airport { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
    }
}
