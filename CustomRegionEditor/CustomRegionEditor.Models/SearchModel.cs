using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Models
{
    public class SearchModel
    {
        public LocationIdModel SearchTerm { get; set; }
        public List<CustomRegionGroupModel> RegionList { get; set; }
    }
}
