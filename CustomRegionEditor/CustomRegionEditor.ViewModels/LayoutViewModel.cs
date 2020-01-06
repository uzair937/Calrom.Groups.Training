using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomRegionEditor.ViewModels
{
    public class LayoutViewModel
    {
        public ContentViewModel ContentViewModel { get; set; }

        public LayoutViewModel()
        {
            ContentViewModel = new ContentViewModel();
        }
    }
}