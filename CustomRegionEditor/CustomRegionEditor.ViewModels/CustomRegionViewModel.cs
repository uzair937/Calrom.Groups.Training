using System.Collections.Generic;

namespace CustomRegionEditor.Models
{
    public class CustomRegionViewModel
    {
        public int CustomRegionID { get; set; }
        public string CustomRegionName { get; set; }
        public List<string> Countries { get; set; }
        public List<StateViewModel> States { get; set; }

    }
}