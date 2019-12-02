using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class RegionModel
    {
        public virtual string ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Abbreviation { get; set; }
    }
}
