using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class CustomRegionEntry
    {
        public virtual Guid Id { get; set; }
        public virtual CustomRegionGroup CustomRegionGroup { get; set; }
        public virtual Region Region { get; set; }
        public virtual Country Country { get; set; }
        public virtual State State { get; set; }
        public virtual City City { get; set; }
        public virtual Airport Airport { get; set; }
        public virtual int RowVersion { get; set; }
    }
}
