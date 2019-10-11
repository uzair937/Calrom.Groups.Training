using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class Apple : Fruit
    {
        protected const double appleCost = 0.50;

        public Apple()
        {
            addApple();
        }

        public void addApple()
        {
            if (appleStock > 0)
            {
                appleStock--;
                cost = appleCost;
                //appleList.Add(this);
                fruitList.Add(this);
            } else
            {
                outOfStock();
            }
        }
    }
}
