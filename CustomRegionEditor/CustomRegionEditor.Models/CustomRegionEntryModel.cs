using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Models
{
    public class CustomRegionEntryModel
    {
        public Guid Id { get; set; }
        public CustomRegionGroupModel CustomRegionGroup { get; set; }
        public RegionModel Region { get; set; }
        public CountryModel Country { get; set; }
        public StateModel State { get; set; }
        public CityModel City { get; set; }
        public AirportModel Airport { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }

        public string GetLocationType()
        {
            ILocationModel model = null;
            if (this.Region != null)
            {
                model = this.Region;
            }
            else if (this.Country != null)
            {
                model = this.Country;
            }
            else if (this.State != null)
            {
                model = this.State;
            }
            else if (this.City != null)
            {
                model = this.City;
            }
            else if (this.Airport != null)
            {
                model = this.Airport;
            }

            return model.Type;
        }
    }
}
