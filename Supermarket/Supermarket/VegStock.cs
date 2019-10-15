using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public abstract class VegStock
    {
        protected static int carrotStock = 15;
        protected static int potatoStock = 20;
        protected static int spinachStock = 100;
        protected const double carrotCost = 0.10;
        protected const double potatoCost = 0.15;
        protected const double spinachCost = 0.05;
    }
}
