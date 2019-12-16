using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomRegionEditor.ViewModels
{
    public class SubRegionViewModel
    {
        public bool ValidResults { get; set; }

        public bool IsViewing { get; set; }

        public string InvalidSearchTerm { get; set; }

        public CustomRegionGroupViewModel CustomRegionGroupViewModel { get; set; }
    }
}