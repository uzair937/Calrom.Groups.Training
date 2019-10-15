using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealership
{
    public class Mercedes : Car
    {
        public double Carprice
        {
            get { return 31.0; }
        }

        public double Deposit()
        {
            return 2.500;
        }
    }
}
