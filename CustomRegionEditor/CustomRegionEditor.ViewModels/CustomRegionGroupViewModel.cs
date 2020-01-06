using System.Collections.Generic;

namespace CustomRegionEditor.ViewModels
{
    public class CustomRegionGroupViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CustomRegionEntryViewModel> CustomRegions { get; set; }

        public CustomRegionGroupViewModel()
        {
            CustomRegions = new List<CustomRegionEntryViewModel>();
        }
    }
}