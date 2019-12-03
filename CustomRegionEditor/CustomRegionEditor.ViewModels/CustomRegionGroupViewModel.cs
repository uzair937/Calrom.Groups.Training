using System.Collections.Generic;

namespace CustomRegionEditor.ViewModels
{
    public class CustomRegionGroupViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CustomRegionViewModel> CustomRegions { get; set; }
    }
}