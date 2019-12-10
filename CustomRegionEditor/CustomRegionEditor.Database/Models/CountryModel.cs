using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class CountryModel
    {
        public virtual string CountryId { get; set; }
        public virtual string CountryName { get; set; }
        public virtual string IsoCode { get; set; }
        public virtual string IsoNumber { get; set; }
        public virtual RegionModel Region { get; set; }
        public virtual int RowVersion { get; set; }
        public virtual string DialingCode { get; set; }
        public virtual Guid LtoId { get; set; }
    }
}
