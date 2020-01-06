using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class State
    {
        public virtual string Id { get; set; }
        public virtual Country Country { get; set; }
        public virtual string Name { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual int RowVersion { get; set; }
        public virtual Guid LtoId { get; set; }
        public virtual IList<City> Cities { get; set; }
    }
}
