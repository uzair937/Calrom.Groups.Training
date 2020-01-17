using CustomRegionEditor.Models;
using System;
using System.Collections.Generic;

namespace CustomRegionEditor.ViewModels
{
    public class CustomRegionGroupViewModel : ICloneable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CustomRegionEntryViewModel> CustomRegions { get; set; }
        public HighlightModel HighlightModel { get; set; }

        public CustomRegionGroupViewModel()
        {
            this.CustomRegions = new List<CustomRegionEntryViewModel>();
            this.HighlightModel = new HighlightModel();
        }

        public object Clone()
        {
            var clone = this.MemberwiseClone() as CustomRegionGroupViewModel;
            clone.CustomRegions = new List<CustomRegionEntryViewModel>(this.CustomRegions);
            return clone;
        }
    }
}