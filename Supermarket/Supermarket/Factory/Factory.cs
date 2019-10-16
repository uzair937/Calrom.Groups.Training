using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public abstract class Factory
    {
        public abstract Fruit getFruit(FruitTypes type);
    }
}
