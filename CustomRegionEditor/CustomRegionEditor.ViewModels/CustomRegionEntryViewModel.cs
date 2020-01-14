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
        public string Value { get; set; }
        public string Name { get; set; }

        public CustomRegionEntryViewModel()
        {
            this.Region = new RegionViewModel();
            this.Country = new CountryViewModel();
            this.State = new StateViewModel();
            this.Airport = new AirportViewModel();
            this.City = new CityViewModel();
        }
    }
}