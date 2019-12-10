using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class AirportCityMappingModel
    {
        public virtual Guid AcmId { get; set; }
        public virtual AirportModel Airport { get; set; }
        public virtual CityModel MappedCity { get; set; }
        public virtual DateTime LiveToDate { get; set; }
        public virtual int RowVersion { get; set; }
    }
}
