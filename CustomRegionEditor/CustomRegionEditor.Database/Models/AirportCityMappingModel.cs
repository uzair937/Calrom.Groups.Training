using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class AirportCityMappingModel
    {
        public virtual Guid acm_id { get; set; }
        public virtual AirportModel apt { get; set; }
        public virtual CityModel mapped_cty { get; set; }
        public virtual DateTime live_to_date { get; set; }
        public virtual int row_version { get; set; }
    }
}
