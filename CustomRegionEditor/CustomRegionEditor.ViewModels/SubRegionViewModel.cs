using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomRegionEditor.ViewModels
{
    public class SubRegionViewModel
    {
        public bool IsViewing { get; set; }

        public CustomRegionGroupViewModel CustomRegionGroupViewModel { get; set; }
    }
}