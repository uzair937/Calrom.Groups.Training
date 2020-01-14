using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Models
{
    public class StateModel : ILocationModel
    {
        public string Id { get; set; }
        public CountryModel Country { get; set; }
        public string Name { get; set; }
        public IList<CityModel> Cities { get; set; }

        public string Type => "state";
    }
}
