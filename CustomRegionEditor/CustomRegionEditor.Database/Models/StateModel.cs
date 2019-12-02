using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class StateModel
    {
        public virtual string sta_id { get; set; }
        public virtual string cnt_id { get; set; }
        public virtual string state_name { get; set; }
        public virtual int display_order { get; set; }
        public virtual int row_version { get; set; }
        public virtual string lto_id { get; set; }
    }
}
