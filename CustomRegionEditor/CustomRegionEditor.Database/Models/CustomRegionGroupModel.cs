using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class CustomRegionGroupModel
    {
        public virtual string crg_id { get; set; }
        public virtual string custom_region_name { get; set; }
        public virtual string custom_region_description { get; set; }
        public virtual string stm_id { get; set; }
        public virtual string rsm_id { get; set; }
        public virtual int display_order { get; set; }
        public virtual int row_version { get; set; }
        public virtual List<CustomRegionEntryModel> CustomRegionEntries { get; set; }
    }
}
