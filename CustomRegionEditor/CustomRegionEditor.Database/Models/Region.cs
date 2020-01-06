using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class Region
    {
        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int RowVersion { get; set; }
        public virtual Guid LtoId { get; set; }

        public virtual IList<Country> Countries { get; set; }
    }
}
