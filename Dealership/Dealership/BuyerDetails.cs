using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealership
{
    public class BuyerDetails
    {
        public string Buyer { get; set; } 
        public string CarRegistration { get; set; }

        public override string ToString()
        {
            return $" {this.Buyer} {this.CarRegistration}";
        }
    }
}
