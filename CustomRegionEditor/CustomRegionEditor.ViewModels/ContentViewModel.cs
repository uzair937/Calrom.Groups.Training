using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomRegionEditor.Models
{
    public class ContentViewModel
    {
        public SearchViewModel SearchViewModel { get; set; }
        public EditViewModel EditViewModel { get; set; }
        public bool IsEditing { get; set; }
        public bool IsSearching { get; set; }
    }
}