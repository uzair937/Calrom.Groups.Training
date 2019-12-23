using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomRegionEditor.ViewModels
{
    public class EditViewModel
    {
        public RegionViewModel NewRegion { get; set; }
        public CountryViewModel NewCountry { get; set; }
        public StateViewModel NewState { get; set; }
        public CityViewModel NewCity { get; set; }
        public AirportViewModel NewAirport { get; set; }
        public bool IsEditing { get; set; }
        public bool ExistingRegion { get; set; }
        public CustomRegionGroupViewModel CustomRegionGroupViewModel { get; set; }
    }
}