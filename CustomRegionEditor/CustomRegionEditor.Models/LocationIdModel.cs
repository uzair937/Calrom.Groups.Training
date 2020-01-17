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

        public LocationIdModel()
        {
            SearchId = new List<string>();
        }
    }
}
