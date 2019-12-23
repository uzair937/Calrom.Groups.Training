using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomRegionEditor.ViewModels
{
    public class EditViewModel
    {
        public bool IsEditing { get; set; }
        public bool ExistingRegion { get; set; }
        public CustomRegionGroupViewModel CustomRegionGroupViewModel { get; set; }
    }
}