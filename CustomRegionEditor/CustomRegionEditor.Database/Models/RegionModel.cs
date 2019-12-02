using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class RegionModel
    {
        public virtual string reg_id { get; set; }
        public virtual string region_name { get; set; }
        public virtual int row_version { get; set; }
        public virtual int lto_id { get; set; }
    }
}
