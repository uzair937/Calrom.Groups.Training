using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Models
{
    public class StateModel
    {
        public virtual string StateId { get; set; }
        public virtual CountryModel Country { get; set; }
        public virtual string StateName { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual int RowVersion { get; set; }
        public virtual Guid LtoId { get; set; }
        public virtual IList<CityModel> Cities { get; set; }
    }
}
