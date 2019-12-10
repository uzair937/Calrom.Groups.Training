using System.Collections.Generic;

namespace CustomRegionEditor.ViewModels
{
    public class CustomRegionEntryViewModel
    {
        public string ID { get; set; }
        public RegionViewModel Region { get; set; }
        public CountryViewModel Country { get; set; }
        public StateViewModel State { get; set; }
        public CityViewModel City { get; set; }
        public AirportViewModel Airport { get; set; }
    }
}