using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class City
    {
        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Country Country { get; set; }
        public virtual int RowVersion { get; set; }
        public virtual State State { get; set; }
        public virtual string TimeZone { get; set; }
        public virtual int UtcOffset { get; set; }
        public virtual Guid LtoId { get; set; }

        public virtual IList<Airport> Airports { get; set; }
    }
}
