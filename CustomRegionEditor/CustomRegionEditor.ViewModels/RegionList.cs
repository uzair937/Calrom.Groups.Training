using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.ViewModels
{
    public class RegionList
    {
        public List<RegionViewModel> Regions { get; set; }
        public List<CountryViewModel> Countries { get; set; }
        public List<StateViewModel> States { get; set; }
        public List<CityViewModel> Cities { get; set; }
        public List<AirportViewModel> Airports { get; set; }
    }
}
