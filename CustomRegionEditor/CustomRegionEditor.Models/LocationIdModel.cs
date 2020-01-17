using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Models
{
    public class LocationIdModel
    {
        public List<string> SearchId { get; set; }
        public string Type { get; set; }
        public HighlightModel HighlightModel { get; set; }
        public LocationIdModel()
        {
            this.SearchId = new List<string>();
            this.HighlightModel = new HighlightModel();
        }
    }
}
