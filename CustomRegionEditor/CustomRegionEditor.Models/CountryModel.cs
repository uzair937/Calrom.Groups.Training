using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Models
{
    public class CountryModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public RegionModel Region { get; set; }

        public IList<CityModel> Cities { get; set; }
        public IList<StateModel> States { get; set; }
    }
}
