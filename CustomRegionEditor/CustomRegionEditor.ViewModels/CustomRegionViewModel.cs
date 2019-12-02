using System.Collections.Generic;

namespace CustomRegionEditor.Models
{
    public class CustomRegionViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public List<string> Countries { get; set; }
        public List<StateViewModel> States { get; set; }

    }
}