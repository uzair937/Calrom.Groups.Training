using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class System
    {
        public virtual string Id { get; set; }
        public virtual string InternalSystemName { get; set; }
        public virtual string ExternalSystemName { get; set; }
        public virtual string SystemDescription { get; set; }
        public virtual int RowVersion { get; set; }
        public virtual int CompId { get; set; }
        public virtual Guid LtoId { get; set; }
    }
}
