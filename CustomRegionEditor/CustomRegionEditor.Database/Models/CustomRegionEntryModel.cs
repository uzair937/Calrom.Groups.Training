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
        public virtual RegionModel Region { get; set; }
        public virtual CountryModel Country { get; set; }
        public virtual StateModel State { get; set; }
        public virtual CityModel City { get; set; }
        public virtual AirportModel Airport { get; set; }
        public virtual int row_version { get; set; }
    }
}
