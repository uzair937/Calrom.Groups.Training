using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Models
{
    public class RegionListModel
    {
        public List<RegionModel> Regions { get; set; }
        public List<CountryModel> Countries { get; set; }
        public List<StateModel> States { get; set; }
        public List<CityModel> Cities { get; set; }
        public List<AirportModel> Airports { get; set; }
    }
}
