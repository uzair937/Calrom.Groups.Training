using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class AirportModel
    {
        public virtual string AirportId { get; set; }
        public virtual string AirportName { get; set; }
        public virtual CityModel City { get; set; }
        public virtual bool IsMainAirport { get; set; }
        public virtual bool IsGatewayAirport { get; set; }
        public virtual int RowVersion { get; set; }
        public virtual string GmaEmailAddress { get; set; }
        public virtual bool IsGmaAllowed { get; set; }
        public virtual bool IsGroupCheckinAllowed { get; set; }
        public virtual Guid LtoId { get; set; }
    }
}
