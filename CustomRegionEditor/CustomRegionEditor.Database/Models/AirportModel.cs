using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class AirportModel
    {
        public virtual string apt_id { get; set; }
        public virtual string airport_name { get; set; }
        public virtual string cty_id { get; set; }
        public virtual bool is_main_airport { get; set; }
        public virtual bool is_gateway_airport { get; set; }
        public virtual int row_version { get; set; }
        public virtual string gma_email_address { get; set; }
        public virtual bool is_gma_allowed { get; set; }
        public virtual bool is_group_checkin_allowed { get; set; }
        public virtual string lto_id { get; set; }
    }
}
