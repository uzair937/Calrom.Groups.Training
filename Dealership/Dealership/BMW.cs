using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealership
{
    public class BMW : Car
    {
        public double Carprice {
            get { return 35.0; }
        }
        

        public double Deposit()
        {
            return 3.000;
        }
    }
}
