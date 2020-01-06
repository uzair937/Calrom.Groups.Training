using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Models
{
    public class CustomRegionGroupModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<CustomRegionEntryModel> CustomRegionEntries { get; set; }

        public void RemoveEntry(CustomRegionEntryModel customRegionEntryModel)
        {
            CustomRegionEntries.Remove(customRegionEntryModel);
        }
    }
}
