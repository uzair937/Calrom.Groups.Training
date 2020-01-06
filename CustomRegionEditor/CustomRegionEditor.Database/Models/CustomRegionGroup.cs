using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class CustomRegionGroup
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual System System { get; set; }
        public virtual string RsmId { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual int RowVersion { get; set; }
        public virtual IList<CustomRegionEntry> CustomRegionEntries { get; set; }

        public virtual void RemoveEntry(CustomRegionEntry customRegionEntryModel)
        {
            CustomRegionEntries.Remove(customRegionEntryModel);
        }
    }
}
