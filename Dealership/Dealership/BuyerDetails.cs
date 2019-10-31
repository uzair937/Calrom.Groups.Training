using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealership
{
    public class BuyerDetails
    {
        public string BuyerName { get; set; } 
        public string CarRegistration { get; set; }

        public override string ToString()
        {
            return $" {this.BuyerName} {this.CarRegistration}";
        }
    }
}
