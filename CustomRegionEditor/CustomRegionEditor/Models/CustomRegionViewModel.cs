using System.Collections.Generic;

namespace CustomRegionEditor.Models
{
    public class CustomRegionViewModel
    {
        public int CustomRegionID { get; set; }
        public string CustomRegionName { get; set; }
        public List<string> Countries { get; set; }
        public List<string> States { get; set; }
        public List<string> Cities { get; set; }
        public List<string> Airports { get; set; }

    }
}