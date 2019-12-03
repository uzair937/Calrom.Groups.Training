using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class CustomRegionEntryModel
    {
        public virtual Guid cre_id { get; set; }
        public virtual CustomRegionGroupModel crg { get; set; }
        public virtual RegionModel reg { get; set; }
        public virtual CountryModel cnt { get; set; }
        public virtual StateModel sta { get; set; }
        public virtual CityModel cty { get; set; }
        public virtual AirportModel apt { get; set; }
        public virtual int row_version { get; set; }
    }
}
