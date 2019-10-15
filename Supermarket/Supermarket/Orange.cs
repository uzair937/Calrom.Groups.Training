using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    public class Orange : Fruit
    {
        public Orange()
        {
            fruitName = "Orange";
            cost = orangeCost;
            addOrange();
        }

        private void addOrange()
        {
            if (orangeStock > 0)
            {
                orangeStock--;
                addItem(this);
            }
            else
            {
                outOfStock();
            }
        }
    }
}
