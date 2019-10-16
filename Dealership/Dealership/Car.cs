using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealership
{    
    public abstract class Car
    {
      //public string Buyer { get; set; }  //moved to buyer detail class
        public string CarName { get; set; }
        public string CarModel { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public string Colour { get; set; }
      //public string CarRegistration { get; set; }  //moved to buyer detail class

        public override string ToString()
        {
            return $" {this.CarName} {this.CarModel} {this.Year} {this.Mileage} {this.Colour} ";
        }
    }
}
