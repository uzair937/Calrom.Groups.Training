using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class CustomRegionGroupModel
    {
        public virtual string ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Abbreviation { get; set; }
        public virtual List<CustomRegionEntryModel> CustomRegionEntries { get; set; }
    }
}
