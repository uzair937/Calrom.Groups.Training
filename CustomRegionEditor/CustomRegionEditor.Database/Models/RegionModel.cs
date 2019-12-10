using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class RegionModel
    {
        public virtual string RegionId { get; set; }
        public virtual string RegionName { get; set; }
        public virtual int RowVersion { get; set; }
        public virtual Guid LtoId { get; set; }

        public virtual IList<CountryModel> Countries { get; set; }
    }
}
