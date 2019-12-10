using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class CustomRegionGroupModel
    {
        public virtual Guid CrgId { get; set; }
        public virtual string CustomRegionName { get; set; }
        public virtual string CustomRegionDescription { get; set; }
        public virtual SystemModel System { get; set; }
        public virtual string RsmId { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual int RowVersion { get; set; }
        public virtual IList<CustomRegionEntryModel> CustomRegionEntries { get; set; }

        public virtual void RemoveEntry(CustomRegionEntryModel customRegionEntryModel)
        {
            CustomRegionEntries.Remove(customRegionEntryModel);
        }
    }
}
