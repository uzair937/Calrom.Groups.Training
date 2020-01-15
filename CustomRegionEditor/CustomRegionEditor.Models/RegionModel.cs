using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Models
{
    public class RegionModel : ILocationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IList<CountryModel> Countries { get; set; }

        public string Type => "region";
    }
}
