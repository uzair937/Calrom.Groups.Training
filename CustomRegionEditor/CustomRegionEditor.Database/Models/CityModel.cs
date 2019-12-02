using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class CityModel
    {
        public virtual string cty_id { get; set; }
        public virtual string city_name { get; set; }
        public virtual string cnt_id { get; set; }
        public virtual int row_version { get; set; }
        public virtual string sta_id { get; set; }
        public virtual string timezone { get; set; }
        public virtual int utc_offset { get; set; }
        public virtual int lto_id { get; set; }
    }
}
