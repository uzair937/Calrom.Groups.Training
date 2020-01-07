using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomRegionEditor.ViewModels
{
    public class ContentViewModel
    {
        public List<string> dbChanges { get; set; }

        public SearchViewModel SearchViewModel { get; set; }

        public EditViewModel EditViewModel { get; set; }

        public SubRegionViewModel SubRegionViewModel { get; set; }

        public ContentViewModel()
        {
            SearchViewModel = new SearchViewModel();
            EditViewModel = new EditViewModel();
            SubRegionViewModel = new SubRegionViewModel();
        }
    }
}