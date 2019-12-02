using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomRegionEditor.ViewModels
{
    public class SearchViewModel
    {
        public List<CustomRegionViewModel> CustomRegionList { get; set; }

        public List<CustomRegionViewModel> SearchResults { get; set; }

        public string SearchTerm { get; set; }

        public bool ValidResults { get; set; }
    }
}