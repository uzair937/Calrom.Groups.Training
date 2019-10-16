using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class Apple : Fruit
    {
        public Apple()
        {
            setStock();
            setCost();
        }
        public override void setStock()
        {
            stock = 7;
        }
        public override void setCost()
        {
            cost = 0.50;
        }
        public override string getFruitName()
        {
            return "Apple";
        }

        public override double getCost()
        {
            return cost;
        }

        public override int getStock()
        {
            return stock;
        }

        public override bool checkStock()
        {
            if (stock > 0)
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
