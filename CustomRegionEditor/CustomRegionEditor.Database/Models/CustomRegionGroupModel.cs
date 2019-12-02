using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class CustomRegionGroupModel
    {
        public int CreID { get; set; }
        public int CrgID { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public List<CustomRegionModel> CustomRegions { get; set; }
    }
}
