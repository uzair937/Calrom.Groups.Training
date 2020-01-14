using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Models
{
    public class CityModel : ILocationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public CountryModel Country { get; set; }
        public StateModel State { get; set; }
        public IList<AirportModel> Airports { get; set; }

        public string Type => "city";
    }
}
