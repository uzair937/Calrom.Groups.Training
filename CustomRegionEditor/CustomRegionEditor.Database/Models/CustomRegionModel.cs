using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class CustomRegionModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public List<RegionModel> RegionList { get; set; }
        public List<CountryModel> Country { get; set; }
        public List<StateModel> State { get; set; }
        public List<CityModel> City { get; set; }
        public List<AirportModel> Airport { get; set; }
    }
}
