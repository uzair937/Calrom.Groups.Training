using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class GlobalDistributionSystem
    {
        public virtual Guid Id { get; set; }
        public virtual string InternalGdsName { get; set; }
        public virtual string ExternalGdsName { get; set; }
        public virtual string GdsDescription { get; set; }
        public virtual string GdsCode { get; set; }
        public virtual int RowVersion { get; set; }
        public virtual string OldGdsId { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual Guid LtoId { get; set; }
    }
}
