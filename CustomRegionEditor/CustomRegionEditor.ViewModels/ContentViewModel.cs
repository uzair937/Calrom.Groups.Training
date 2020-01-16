using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CustomRegionEditor.Models;

namespace CustomRegionEditor.ViewModels
{
    public class ContentViewModel
    {
        public List<ErrorViewModel> ErrorModels { get; set; }

        public SearchViewModel SearchViewModel { get; set; }

        public EditViewModel EditViewModel { get; set; }

        public SubRegionViewModel SubRegionViewModel { get; set; }

        public ContentViewModel()
        {
            ErrorModels = new List<ErrorViewModel>();
            SearchViewModel = new SearchViewModel();
            EditViewModel = new EditViewModel();
            SubRegionViewModel = new SubRegionViewModel();
        }
    }
}