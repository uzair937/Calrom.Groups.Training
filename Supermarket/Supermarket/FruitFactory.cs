using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public abstract class FruitFactory
    {
        public static Fruit getFruit(FruitTypes type)
        {
            Fruit fruit = null;
            switch(type)
            {
                case FruitTypes.Apple:
                    fruit = new Apple();
                    break;
                case FruitTypes.Banana:
                    fruit = new Banana();
                    break;
                case FruitTypes.Orange:
                    fruit = new Orange();
                    break;
                default:
                    fruit = null;
                    break;
            }
            return fruit;
        }
    }
}
