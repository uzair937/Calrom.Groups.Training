using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class CustomRegionModel
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public RegionModel Region { get; set; }

        public CountryModel Country { get; set; }

        public StateModel State { get; set; }

        public CityModel City { get; set; }

        public AirportModel Airport { get; set; }
    }
}
