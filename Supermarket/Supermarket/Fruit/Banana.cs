using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class Banana : Fruit
    {
        public Banana()
        {
            setFruitName();
            setStock();
            setCost();
        }
        public override void setStock()
        {
            stock = 12;
        }
        public override void setCost()
        {
            cost = 0.30;
        }
        public override void setFruitName()
        {
            fruitName = "Banana";
        }
        public override string getFruitName()
        {
            return fruitName;
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
