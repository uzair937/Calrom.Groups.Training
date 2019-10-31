using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealership
{
    public class CarBuilder 
    {
        public static Car BuildMercedes()
        {
            var car = new Mercedes();
            return car; 
    
        }
    }
}
