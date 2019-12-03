using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class GlobalDistributionSystemModel
    {
        public virtual Guid gds_id { get; set; }
        public virtual string internal_gds_name { get; set; }
        public virtual string external_gds_name { get; set; }
        public virtual string gds_description { get; set; }
        public virtual string gds_code { get; set; }
        public virtual int row_version { get; set; }
        public virtual string old_gds_id { get; set; }
        public virtual int display_order { get; set; }
        public virtual Guid lto_id { get; set; }
    }
}
