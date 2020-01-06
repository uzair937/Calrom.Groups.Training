using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class Country
    {
        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string IsoCode { get; set; }
        public virtual string IsoNumber { get; set; }
        public virtual Region Region { get; set; }
        public virtual int RowVersion { get; set; }
        public virtual string DialingCode { get; set; }
        public virtual Guid LtoId { get; set; }

        public virtual IList<City> Cities { get; set; }
        public virtual IList<State> States { get; set; }
    }
}
