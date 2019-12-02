using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class CustomRegionEntry
    {
        public string cre_id { get; set; }
        public string crg_id { get; set; }
        public string reg_id { get; set; }
        public string cnt_id { get; set; }
        public string sta_id { get; set; }
        public string cty_id { get; set; }
        public string apt_id { get; set; }
        public string row_version { get; set; }
    }
}
