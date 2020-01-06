using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class Airport
    {
        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
        public virtual City City { get; set; }
        public virtual bool IsMainAirport { get; set; }
        public virtual bool IsGatewayAirport { get; set; }
        public virtual int RowVersion { get; set; }
        public virtual string GmaEmailAddress { get; set; }
        public virtual bool IsGmaAllowed { get; set; }
        public virtual bool IsGroupCheckinAllowed { get; set; }
        public virtual Guid LtoId { get; set; }
    }
}
