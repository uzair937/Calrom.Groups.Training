﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class CountryModel
    {
        public virtual Guid cnt_id { get; set; }
        public virtual string country_name { get; set; }
        public virtual string iso_code { get; set; }
        public virtual string iso_number { get; set; }
        public virtual RegionModel reg { get; set; }
        public virtual int row_version { get; set; }
        public virtual string dialing_code { get; set; }
        public virtual Guid lto_id { get; set; }
    }
}
