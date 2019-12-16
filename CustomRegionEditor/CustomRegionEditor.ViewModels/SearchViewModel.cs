using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomRegionEditor.ViewModels
{
    public class SearchViewModel
    {
        public bool IsSearching { get; set; }

        public List<CustomRegionGroupViewModel> SearchResults { get; set; }

        public string SearchTerm { get; set; }

        public bool ValidResults { get; set; }

        public string InvalidSearchTerm { get; set; }
    }
}