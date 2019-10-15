using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class Apple : Fruit
    {
        private int appleStock = 7;
        private const double price = 0.50;

        public override string getFruitName()
        {
            return "Apple";
        }

        public override double getCost()
        {
            return price;
        }

        public override int getStock()
        {
            return appleStock;
        }

        public override bool checkStock()
        {
            if (appleStock > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
