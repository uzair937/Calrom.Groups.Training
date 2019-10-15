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
            fruitName = "Apple";
            cost = appleCost;
            addApple();
        }

        private void addApple()
        {
            if (appleStock > 0)
            {
                appleStock--;
                addItem(this);
            }
            else
            {
                outOfStock();
            }
        }
    }
}
