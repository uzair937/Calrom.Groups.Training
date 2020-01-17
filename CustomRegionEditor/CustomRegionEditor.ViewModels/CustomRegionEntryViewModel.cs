using CustomRegionEditor.Models;
using System.Collections.Generic;

namespace CustomRegionEditor.ViewModels
{
    public class CustomRegionEntryViewModel
    {
        public string Id { get; set; }
        public RegionViewModel Region { get; set; }
        public CountryViewModel Country { get; set; }
        public StateViewModel State { get; set; }
        public CityViewModel City { get; set; }
        public AirportViewModel Airport { get; set; }
        public HighlightModel HighlightModel { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }

        public CustomRegionEntryViewModel()
        {
            this.HighlightModel = new HighlightModel();
            this.Region = new RegionViewModel();
            this.Country = new CountryViewModel();
            this.State = new StateViewModel();
            this.Airport = new AirportViewModel();
            this.City = new CityViewModel();
        }
    }
}