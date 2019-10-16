using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealership
{
    public class Audi : Car
    {
        public double CarPrice
        {
           get  { return 29.000; }
        }
        
        public double Deposit
        {
            get { return 2.000; }
        }

    }
}
