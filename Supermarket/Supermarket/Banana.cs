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
            fruitName = "Banana";
            cost = bananaCost;
            addBanana();
        }

        private void addBanana()
        {
            if (bananaStock > 0)
            {
                bananaStock--;
                addItem(this);
            }
            else
            {
                outOfStock();
            }
        }
    }
}
