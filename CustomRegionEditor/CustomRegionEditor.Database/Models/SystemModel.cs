using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class SystemModel
    {
        public virtual int stm_id { get; set; }
        public virtual string internal_system_name { get; set; }
        public virtual string external_system_name { get; set; }
        public virtual string system_description { get; set; }
        public virtual int row_version { get; set; }
        public virtual int comp_id { get; set; }
        public virtual string lto_id { get; set; }
    }
}
