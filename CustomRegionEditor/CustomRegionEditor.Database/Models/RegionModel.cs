﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class RegionModel
    {
        public virtual Guid reg_id { get; set; }
        public virtual string region_name { get; set; }
        public virtual int row_version { get; set; }
        public virtual Guid lto_id { get; set; }
    }
}
