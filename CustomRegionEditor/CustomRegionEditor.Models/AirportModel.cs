using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Models
{
    public class AirportModel : ILocationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public CityModel City { get; set; }

        public string Type => "airport";
    }
}
